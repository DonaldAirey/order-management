namespace Teraque.AssetNetwork
{

	using Teraque;
	using System;
	using System.ComponentModel;
	using System.Collections.Generic;
	using System.Collections;
	using System.Data;
	using System.Diagnostics;
	using System.IdentityModel.Policy;
	using System.IdentityModel.Claims;
	using System.IO;
	using System.Security.Principal;
	using System.Threading;
	using System.Transactions;
	using System.Windows;
	using System.Xml;
	using System.Xml.Linq;

	/// <summary>
	/// Simulates market conditions used to test the AssetNetwork architecture.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal class MarketSimulator
	{

		// Private Constants
		private const Double defaultFrequency = 50.0;
		private const Int32 interval = 100;
        private const Int32 brokerThreadCount = 4;

		// Private Static Fields
		private static Int32 batchSize;
		private static Thread[] brokerSimulationThreads;
		private static DataSetMarket dataSetMarket;
		private static Boolean isBrokerSimulatorThreadRunning;
		private static Boolean isPriceSimulatorThreadRunning;
		private static ManualResetEvent orderEvent;
		private static Thread orderHandlerThread;
		private static Thread priceSimulationThread;
		private static SimulationParameters simulationParameters;
		private static WaitQueue<DataModel.DestinationOrderRow> orderQueue;
		private static Object syncRoot;

		// Indicates the different ways a price can be quoted.
		enum QuoteType { Ask, Bid, Last };

		// Indicates the different ways an order can execute.
		enum ActionType { Execute };

		/// <summary>
		/// Initializes the static elements of the market simulator.
		/// </summary>
		static MarketSimulator()
		{

			// Used for locking the simulator by multithreaded operations.
			MarketSimulator.syncRoot = new Object();

			// The destination orders are picked off of the event handler for the table and fed through a queue to another thread which processes them.  The queue
			// is necessary because the destination table events only fire when committing a transaction and it's impossible to start another transaction (to lock
			// and read tables) while completing the first transaction (adding a row to the destination order table).
			MarketSimulator.orderQueue = new WaitQueue<DataModel.DestinationOrderRow>();
			MarketSimulator.orderEvent = new ManualResetEvent(false);

			// These settings control the simulator.
			MarketSimulator.simulationParameters = new SimulationParameters();
			MarketSimulator.simulationParameters.IsBrokerSimulationRunning = false;
			MarketSimulator.simulationParameters.IsPriceSimulationRunning = false;
			MarketSimulator.simulationParameters.Frequency = MarketSimulator.defaultFrequency;

            // One or more threads are created to simulate a broker servicing the shared order book.
            MarketSimulator.brokerSimulationThreads = new Thread[MarketSimulator.brokerThreadCount];

			// In order to minimize the overhead for creating transaction to service the prices, the prices are batched up and processed in a single transaction.
			// The desired frequence determines the number of prices changed in a single batch since the frequence of the thread is fixed through constants.
			MarketSimulator.batchSize = Convert.ToInt32(
				MarketSimulator.simulationParameters.Frequency * Convert.ToDouble(MarketSimulator.interval) / 1000.0);

			// This DataSet provides structures used by the market simulation.
			MarketSimulator.dataSetMarket = new DataSetMarket();

			try
			{

				// The price simulator needs realistic prices.  An XML file has been embedded with this assembly that provides the seed values for the prices.
				Random random = new Random(DateTime.Now.TimeOfDay.Milliseconds);
				Stream priceStream = Application.GetResourceStream(new Uri("/Teraque.TradingSupport;component/SimulatedPrices.xml", UriKind.Relative)).Stream;
				XmlReader xmlReader = new XmlTextReader(priceStream);
				XDocument xDocument = XDocument.Load(xmlReader);
				foreach (XElement xElement in xDocument.Root.Elements())
				{
					String symbol = xElement.Attribute("Symbol").Value;
					Decimal lastPrice = Convert.ToDecimal(xElement.Attribute("Price").Value);
					Decimal askPrice = lastPrice + Convert.ToDecimal(Math.Round(random.NextDouble(), 2));
					Decimal bidPrice = lastPrice - Convert.ToDecimal(Math.Round(random.NextDouble(), 2));
					MarketSimulator.dataSetMarket.Price.AddPriceRow(askPrice, 0.0M, bidPrice, 0.0M, "US TICKER", "USD", lastPrice, 0.0M, symbol);
				}
				MarketSimulator.dataSetMarket.AcceptChanges();
			}
			catch { }

		}

		/// <summary>
		/// Gets or sets a thread safe flag that controls the broker simulation thread.
		/// </summary>
		private static Boolean IsBrokerSimulatorThreadRunning
		{
			get { lock (MarketSimulator.syncRoot) return MarketSimulator.isBrokerSimulatorThreadRunning; }
			set { lock (MarketSimulator.syncRoot) MarketSimulator.isBrokerSimulatorThreadRunning = value; }
		}

		/// <summary>
		/// Gets or sets a thread safe flag that controls the price simulation thread.
		/// </summary>
		private static Boolean IsPriceSimulatorThreadRunning
		{
			get { lock (MarketSimulator.syncRoot) return MarketSimulator.isPriceSimulatorThreadRunning; }
			set { lock (MarketSimulator.syncRoot) MarketSimulator.isPriceSimulatorThreadRunning = value; }
		}

		/// <summary>
		/// Gets or sets the thread safe parameters that control the simulations.
		/// </summary>
		public static SimulationParameters SimulationParameters
		{

			get
			{

				// These parameters are shared by different threads.
				lock (MarketSimulator.syncRoot)
					return MarketSimulator.simulationParameters;

			}
			set
			{

				// These parameters are shared by different threads.
				lock (MarketSimulator.syncRoot)
				{

					// The saved parameters reflect the state of the simulation.
					MarketSimulator.simulationParameters = value;

					// In order to minimize the overhead for creating transaction to service the prices, the prices are batched up 
					// and processed in a single transaction.  The desired frequence determines the number of prices changed in a
					// single batch since the frequence of the thread is fixed through constants.
					MarketSimulator.batchSize = Convert.ToInt32(
						MarketSimulator.simulationParameters.Frequency * Convert.ToDouble(MarketSimulator.interval) / 1000.0);

					// This starts the broker simulation when it isn't running.
					if (!IsBrokerSimulatorThreadRunning && MarketSimulator.SimulationParameters.IsBrokerSimulationRunning)
					{

						// This flag controls the execution of the threads used to simulate the brokers.
						MarketSimulator.IsBrokerSimulatorThreadRunning = true;

						// These event handlers will pump the destination order into the simulation.
						DataModel.DestinationOrder.DestinationOrderRowChanging += new DataModel.DestinationOrderRowChangeEventHandler(OnDestinationOrderRowChanging);

						// Multiple threads are created to service the order book.
                        for (Int32 threadIndex = 0; threadIndex < MarketSimulator.brokerThreadCount; threadIndex++)
                        {
                            MarketSimulator.brokerSimulationThreads[threadIndex] = new Thread(new ThreadStart(SimulateBroker));
                            MarketSimulator.brokerSimulationThreads[threadIndex].Name = String.Format("Broker Simulator #{0}", threadIndex);
                            MarketSimulator.brokerSimulationThreads[threadIndex].Start();
                        }

						// This thread provides a 'pump' for the orders between the destination table event handler and the broker simulation.
						MarketSimulator.orderHandlerThread = new Thread(new ThreadStart(OrderHandler));
						MarketSimulator.orderHandlerThread.Name = "Order Queue";
						MarketSimulator.orderHandlerThread.Start();

					}

					// This shuts down the broker simulation when it is running.
					if (IsBrokerSimulatorThreadRunning && !MarketSimulator.SimulationParameters.IsBrokerSimulationRunning)
					{

						// Clearing this flag allows the simulation threads to exit gracefully.
						MarketSimulator.IsBrokerSimulatorThreadRunning = false;

						// Remove the handlers the pump the trades into the simulator.
						DataModel.DestinationOrder.DestinationOrderRowChanging -= new DataModel.DestinationOrderRowChangeEventHandler(OnDestinationOrderRowChanging);

						// Shut down the threads that simulate a broker, with prejudice if necessary.
                        for (Int32 threadIndex = 0; threadIndex < MarketSimulator.brokerThreadCount; threadIndex++)
                            if (!MarketSimulator.brokerSimulationThreads[threadIndex].Join(100))
                                MarketSimulator.brokerSimulationThreads[threadIndex].Abort();
						if (!MarketSimulator.orderHandlerThread.Join(100))
							MarketSimulator.orderHandlerThread.Abort();

					}

					// This starts the price simulation when it isn't running.
					if (!IsPriceSimulatorThreadRunning && MarketSimulator.SimulationParameters.IsPriceSimulationRunning)
					{
						MarketSimulator.IsPriceSimulatorThreadRunning = true;
						MarketSimulator.priceSimulationThread = new Thread(new ThreadStart(SimulatePrice));
						MarketSimulator.priceSimulationThread.Name = "Price Simulator";
						MarketSimulator.priceSimulationThread.Start();
					}

					// This shuts down the price simulation when it is running.
					if (IsPriceSimulatorThreadRunning && !MarketSimulator.SimulationParameters.IsPriceSimulationRunning)
					{
						MarketSimulator.IsPriceSimulatorThreadRunning = false;
						if (!MarketSimulator.priceSimulationThread.Join(100))
							MarketSimulator.priceSimulationThread.Abort();
					}

				}

			}

		}

		/// <summary>
		/// Simulation of price changes in a stock market.
		/// </summary>
		private static void SimulatePrice()
		{

			// This will seed the random number generator.
			Random random = new Random(DateTime.Now.Millisecond);

			// This set of claims gives the current thread the authority to update the price table.
			List<Claim> listClaims = new List<Claim>();
			listClaims.Add(new Claim(Teraque.ClaimTypes.Create, Teraque.Resources.Application, Rights.PossessProperty));
			listClaims.Add(new Claim(Teraque.ClaimTypes.Update, Teraque.Resources.Application, Rights.PossessProperty));
			listClaims.Add(new Claim(Teraque.ClaimTypes.Read, Teraque.Resources.Application, Rights.PossessProperty));
			ClaimSet adminClaims = new DefaultClaimSet(null, listClaims.ToArray());
//			Thread.CurrentPrincipal = new Teraque.ClaimsPrincipal(new GenericIdentity("Price Service"), adminClaims);

			// This thread will run until an external thread sets this property to 'false'.
			while (MarketSimulator.IsPriceSimulatorThreadRunning)
			{

				// The price changes are generated in batches since there is a modest overhead to creating each transaction and applying it to the shared data
				// model.
				using (TransactionScope transactionScope = new TransactionScope())
				{

					// The middle tier context allows ADO operations to participate in the transaction.
					DataModel dataModel = new DataModel();
					DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

					// The simulator parameters need to be locked for each batch of simulated price changes.
					lock (MarketSimulator.syncRoot)
					{

						// The prices are not updated continously because it is trying to simulate a communication interface where a batch of prices is updated
						// periodically.
						for (int tick = 0; tick < MarketSimulator.batchSize; tick++)
						{

							// This will select a random price in the table and change the price by a random amount.
							int priceIndex = random.Next(MarketSimulator.dataSetMarket.Price.Rows.Count);
							DataSetMarket.PriceRow priceRow = MarketSimulator.dataSetMarket.Price[priceIndex];

							// This will simulate a random selection of a price change is quoted. The 'quoteColumn' variable can be used to reference the
							// selected quote from any price row.  Below, a random row will be selected for the price change.
							int quoteTypeIndex = random.Next(Enum.GetValues(typeof(QuoteType)).Length);
							switch ((QuoteType)Enum.GetValues(typeof(QuoteType)).GetValue(quoteTypeIndex))
							{

							case QuoteType.Ask:

								priceRow.AskPrice += Convert.ToDecimal(Math.Round(random.NextDouble() - 0.5, 2));
								priceRow.AskSize += Convert.ToDecimal(random.Next(1, 10));
								break;

							case QuoteType.Bid:

								priceRow.BidPrice += Convert.ToDecimal(Math.Round(random.NextDouble() - 0.5, 2));
								priceRow.BidSize += Convert.ToDecimal(random.Next(1, 10));
								break;

							case QuoteType.Last:

								priceRow.LastPrice += Convert.ToDecimal(Math.Round(random.NextDouble() - 0.5, 2));
								priceRow.LastSize += Convert.ToDecimal(random.Next(1, 10));
								break;

							}

							// This does the work of updating the price table using the external identifiers and the newly generated prices.
							dataModel.CreatePriceEx(priceRow.AskPrice, priceRow.AskSize, priceRow.BidPrice, priceRow.BidSize, 0.0M, priceRow.ConfigurationId,
								0.0M, priceRow.LastPrice, priceRow.LastSize, 0.0M, 0.0M, 0.0M, new object[] { priceRow.Currency }, new object[] { priceRow.Symbol },
								0.0M, 0.0M);

						}

						// This will commit the changes to the price table.
						transactionScope.Complete();

					}

				}

				// This allows other threads the chance to run.
				Thread.Sleep(MarketSimulator.interval);

			}

		}

		/// <summary>
		/// Simulation of brokers executing destination orders.
		/// </summary>
		private static void SimulateBroker()
		{

			// This will seed the random number generator.
			Random random = new Random(DateTime.Now.Millisecond);

			// An instance of the data model is required to update it.
			DataModel dataModel = new DataModel();

			// This set of claims gives the current thread the authority to update the price table.
			List<Claim> listClaims = new List<Claim>();
			listClaims.Add(new Claim(Teraque.ClaimTypes.Create, Teraque.Resources.Application, Rights.PossessProperty));
			listClaims.Add(new Claim(Teraque.ClaimTypes.Update, Teraque.Resources.Application, Rights.PossessProperty));
			listClaims.Add(new Claim(Teraque.ClaimTypes.Read, Teraque.Resources.Application, Rights.PossessProperty));
			ClaimSet adminClaims = new DefaultClaimSet(null, listClaims);
//			Thread.CurrentPrincipal = new ClaimsPrincipal(new GenericIdentity("Broker Service"), adminClaims);

			// Every execution requires a user identifier (the user who created the execution) and the broker who executed the
			// trade.  These values are hard coded at the moment but should be more intelligently assigned in the future.
			Guid userId = Guid.Empty;
			Guid brokerId = Guid.Empty;

			// Operating values are required for the simulation that come from the data model.
			using (TransactionScope transactionScope = new TransactionScope())
			{

				// The middle tier context allows ADO operations to participate in the transaction.
				DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

				// The broker for all the executions is hard coded.
				DataModel.EntityRow brokerRow = DataModel.Entity.EntityKeyExternalId0.Find(new object[] { "ZODIAC SECURITIES" });
				brokerRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
				dataModelTransaction.AddLock(brokerRow);
				brokerId = brokerRow.EntityId;

				// The user who creates these executions is hard coded.
				DataModel.EntityRow userRow = DataModel.Entity.EntityKeyExternalId0.Find(new object[] { "ADMINISTRATOR" });
				userRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
				dataModelTransaction.AddLock(userRow);
				userId = userRow.EntityId;

				// The data model doesn't need to be locked any more.
				transactionScope.Complete();

			}

			// This thread will run until an external thread sets this property to 'false'.
			while (MarketSimulator.IsBrokerSimulatorThreadRunning)
			{

				// Wait for orders to show up in the simulated order book.
				MarketSimulator.orderEvent.WaitOne();

				// The simulator parameters need to be locked for each batch of simulated price changes.
				try
				{

					Monitor.Enter(MarketSimulator.syncRoot);

					// This will select a random price in the table and change the price by a random amount.
					if (MarketSimulator.dataSetMarket.Order.Rows.Count > 0)
					{

						// Select a random row from the order book.
						int orderIndex = random.Next(MarketSimulator.dataSetMarket.Order.Rows.Count);
						DataSetMarket.OrderRow orderRow = MarketSimulator.dataSetMarket.Order[orderIndex];
						if (orderRow.IsBusy)
						{
							Thread.Sleep(0);
							continue;
						}
						orderRow.IsBusy = true;

						// This transaction is required to add the new execution.
						using (TransactionScope transactionScope = new TransactionScope())
						{

							// The middle tier context allows ADO operations to participate in the transaction.
							DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

							// This will simulate a random selection of a price change is quoted. The 'quoteColumn' variable can be
							// used to reference the selected quote from any price row.  Below, a random row will be selected for
							// the price change.
							int actionTypeIndex = random.Next(Enum.GetValues(typeof(ActionType)).Length);
                            switch ((ActionType)Enum.GetValues(typeof(ActionType)).GetValue(actionTypeIndex))
                            {

                                case ActionType.Execute:

                                    // This creates a random execution of the remaining shares on the order.
                                    DateTime dateTime = DateTime.Now;
									Guid blotterId = orderRow.BlotterId;
                                    Decimal quantityLeaves = orderRow.QuantityOrdered - orderRow.QuantityExecuted;
                                    Decimal quantityExecuted = quantityLeaves <= 100 ?
                                        quantityLeaves :
                                        Convert.ToDecimal(random.Next(1, Convert.ToInt32(quantityLeaves / 100.0M))) * 100.0M;
                                    DataSetMarket.PriceRow priceRow =
                                        MarketSimulator.dataSetMarket.Price.FindByConfigurationIdSymbol("US TICKER", orderRow.Symbol);
                                    Decimal executionPrice = 0.0M;
									if (priceRow != null)
									{
										executionPrice = orderRow.SideCode == SideCode.Buy || orderRow.SideCode == SideCode.BuyCover ?
											priceRow.BidPrice :
											priceRow.AskPrice;
									}
                                    Guid executionId = Guid.NewGuid();
                                    Guid destinationOrderId = orderRow.DestinationOrderId;

                                    // When the order is completed, it is removed from the order book.  When the order book is empty, the thread goes to sleep
                                    // until another thread puts something in the order queue.
                                    orderRow.QuantityExecuted += quantityExecuted;
                                    if (orderRow.QuantityOrdered == orderRow.QuantityExecuted)
                                    {
                                        MarketSimulator.dataSetMarket.Order.RemoveOrderRow(orderRow);
                                        if (MarketSimulator.dataSetMarket.Order.Count == 0)
                                            MarketSimulator.orderEvent.Reset();
                                    }

									if (quantityExecuted > 0)
									{
										Monitor.Exit(MarketSimulator.syncRoot);

										dataModel.CreateExecution(
											0.0M, 
											blotterId, 
											null, 
											brokerId, 
											0.0M, 
											dateTime, 
											userId, 
											destinationOrderId, 
											StateMap.FromCode(StateCode.Acknowledged),
											executionId, 
											executionPrice, 
											quantityExecuted, 
											null, 
											null, 
											false, 
											dateTime, 
											userId, 
											null, 
											null, 
											null,
											null,
											StateMap.FromCode(StateCode.Acknowledged), 
											0.0M, 
											0.0M, 
											0.0M, 
											0.0M);
										Monitor.Enter(MarketSimulator.syncRoot);
									}

                                    break;

                            }

							// This will commit the changes to the order book.
							transactionScope.Complete();

						}

						// This allows another thread to work the order.
						orderRow.IsBusy = false;

					}

				}
				finally
				{
					Monitor.Exit(MarketSimulator.syncRoot);
				}

				// This allows other threads the chance to run.
				Thread.Sleep(0);

			}

		}

		/// <summary>
		/// Creates orders in the simulated order book.
		/// </summary>
		private static void OrderHandler()
		{

			// This thread will create orders in the simulated order book from destination orders placed in the queue.  This thread
			// is necessary due to the locking architecture that prevents accessing the data model during a commit operation (which
			// is where the event indicating a new or changed destination order originates).
			while (MarketSimulator.IsBrokerSimulatorThreadRunning)
			{

				// This thread will wait here until a destination order is available in the queue.
				DataModel.DestinationOrderRow destinationOrderRow = MarketSimulator.orderQueue.Dequeue();

				// The code that adds an order to the simulated book must have exclusive access to the simulated data model.
				lock (MarketSimulator.syncRoot)
				{

					// The primary data model needs to be accessed also for ancillary data associated with the new order.
					using (TransactionScope transactionScope = new TransactionScope())
					{

						try
						{

							// This context is used to keep track of the locks aquired for the ancillary data.
							DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

							// Lock the destination order.
							destinationOrderRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
							dataModelTransaction.AddLock(destinationOrderRow);

							// Lock the Security.
							DataModel.SecurityRow securityRow = destinationOrderRow.SecurityRowByFK_Security_DestinationOrder_SecurityId;
							securityRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
							dataModelTransaction.AddLock(securityRow);

							// The working order row must be locked to examine the flags
							DataModel.WorkingOrderRow workingOrderRow = destinationOrderRow.WorkingOrderRow;
							workingOrderRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
							dataModelTransaction.AddLock(workingOrderRow);

							// Lock the Entity.
							DataModel.EntityRow entityRow = securityRow.EntityRow;
							entityRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
							dataModelTransaction.AddLock(entityRow);

							// Calculate the quantity executed on this order (some orders are created with executions, such as crossed orders.)
							Decimal quantityExecuted = 0.0M;
							foreach (DataModel.ExecutionRow executionRow in destinationOrderRow.GetExecutionRows())
							{
								executionRow.AcquireReaderLock(dataModelTransaction);
								dataModelTransaction.AddLock(executionRow);
								quantityExecuted += executionRow.ExecutionQuantity;
							}

							// The simulated order is added to the book.  This collects all the information required to simulate the execution of this order.
							DataSetMarket.OrderRow orderRow = MarketSimulator.dataSetMarket.Order.NewOrderRow();
							orderRow.BlotterId = destinationOrderRow.BlotterId;
							orderRow.DestinationOrderId = destinationOrderRow.DestinationOrderId;
							orderRow.OrderTypeCode = OrderTypeMap.FromId(destinationOrderRow.OrderTypeId);
							orderRow.QuantityOrdered = destinationOrderRow.OrderedQuantity;
							orderRow.QuantityExecuted = quantityExecuted;
							orderRow.SideCode = SideMap.FromId(destinationOrderRow.SideId);
							orderRow.Symbol = securityRow.Symbol;
							orderRow.TimeInForceCode = TimeInForceMap.FromId(destinationOrderRow.TimeInForceId);
							MarketSimulator.dataSetMarket.Order.AddOrderRow(orderRow);

							// Adding the first order to the simulated order book will enable the simulation thread to begin processing the orders.  The order
							// simulation thread will continue until the book is filled.
							if (MarketSimulator.dataSetMarket.Order.Count == 1)
								MarketSimulator.orderEvent.Set();

						}
						catch { }

						// The locks are no longer required.
						transactionScope.Complete();

					}

				}

			}

		}

		/// <summary>
		/// Handles a change to the DestinationOrder table.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event data.</param>
		static void OnDestinationOrderRowChanging(object sender, DataModel.DestinationOrderRowChangeEventArgs e)
		{

			// This will pump the committed destination orders into the simulation.
			if (e.Action == DataRowAction.Commit && e.Row.RowState == DataRowState.Added)
				MarketSimulator.orderQueue.Enqueue(e.Row);

		}

	}

}
