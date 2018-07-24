namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Threading;

	/// <summary>
	/// Simulates one or more brokers processing orders in the simulated book.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal class BrokerSimulator
	{

		/// <summary>
		/// Handles an order action (an action that is taken on a random order to simulate something a broker might do).
		/// </summary>
		/// <param name="orderRow">An order row.</param>
		delegate void OrderActionHandler(BrokerContext brokerContext, MarketDataModel.OrderRow orderRow);

		/// <summary>
		/// Different kinds of order actions.
		/// </summary>
		enum OrderAction { Execute };

		/// <summary>
		/// A list of brokers to be simulated.
		/// </summary>
		static BrokerList brokerList = new BrokerList();

		/// <summary>
		/// The frequency of executions.
		/// </summary>
		static Double executionFrequencyField = 250.0;

		/// <summary>
		/// Maps an simulated action to the handler for that action.
		/// </summary>
		static Dictionary<OrderAction, OrderActionHandler> actionMap = new Dictionary<OrderAction, OrderActionHandler>()
		{
			{OrderAction.Execute, BrokerSimulator.ExecuteOrder}
		};

		/// <summary>
		/// The broker threads.
		/// </summary>
		static Thread[] brokerSimulationThreads;

		/// <summary>
		/// The broker contexts.
		/// </summary>
		static Dictionary<String, BrokerContext> brokerContexts = new Dictionary<String, BrokerContext>();

		/// <summary>
		/// The number of brokers simulated.
		/// </summary>
		const Int32 brokerThreadCount = 1;

		/// <summary>
		/// Used to wait for an order to arrive.
		/// </summary>
		static ManualResetEvent orderEvent = new ManualResetEvent(false);

		/// <summary>
		/// Used to coordinate threads.
		/// </summary>
		static Object syncRoot = new Object();

		/// <summary>
		/// Initialize the BrokerSimulator class.
		/// </summary>
		static BrokerSimulator()
		{

			// One or more threads are created to simulate a broker servicing the shared order book.
			BrokerSimulator.brokerSimulationThreads = new Thread[BrokerSimulator.brokerThreadCount];

		}

		/// <summary>
		/// Gets or sets the frequency of equity price changes.
		/// </summary>
		internal static Double ExecutionFrequency
		{
			get
			{
				lock (BrokerSimulator.syncRoot)
					return BrokerSimulator.executionFrequencyField;
			}
			set
			{
				lock (BrokerSimulator.syncRoot)
					BrokerSimulator.executionFrequencyField = value;
			}
		}

		/// <summary>
		/// Adds an order to the simulated broker's book.
		/// </summary>
		/// <param name="message">A message containing the new order.</param>
		internal static void AddOrder(Message message)
		{

			// The database must be locked while we create a new order in the simulated broker book.
			lock (MarketData.SyncRoot)
			{

				// This creates an order from the message and puts it in the simulator order book.
				MarketDataModel.OrderRow orderRow = MarketData.Order.NewOrderRow();
				orderRow.CustomerOrderId = message.ClOrdID;
				if (message.ContainsKey(Tag.StopPx))
					orderRow.LimitPrice = message.StopPx;
				orderRow.OrderId = Guid.NewGuid();
				orderRow.OrderTypeCode = Convert.ToInt32(message.OrdType);
				orderRow.QuantityOrdered = message.OrderQty;
				orderRow.SideCode = Convert.ToInt32(message.SideCode);
				orderRow.Source = message.SenderCompID;
				orderRow.Symbol = message.Symbol;
				orderRow.TimeInForceCode = Convert.ToInt32(message.TimeInForceCode);
				MarketData.Order.AddOrderRow(orderRow);

				// This will wake up the simulated brokers if they are asleep.
				BrokerSimulator.orderEvent.Set();

			}

		}

		/// <summary>
		/// Simulates the execution of an order.
		/// </summary>
		/// <param name="orderRow">The order to be executed.</param>
		static void ExecuteOrder(BrokerContext brokerContext, MarketDataModel.OrderRow orderRow)
		{

			// Create an execution.
			MarketDataModel.ExecutionRow executionRow = MarketData.Execution.NewExecutionRow();
			executionRow.ExecutionId = Guid.NewGuid();
			executionRow.OrderId = orderRow.OrderId;

			// This execution needs a random number of shares to fill.  We can't overcommit the order, so first we need to count up how many shares have already
			// been executed on this order.  Then we can calculated the 'leaves' quantity (the amount remaining to be executed).
			Decimal executedQuantity = 0.0m;
			foreach (MarketDataModel.ExecutionRow childExecutionRow in orderRow.GetExecutionRows())
				executedQuantity += childExecutionRow.Quantity;
			Decimal leavesQuantity = orderRow.QuantityOrdered - executedQuantity;

            // This will generate either a random fill or finish the order with an odd lot.
            executionRow.Quantity = Math.Min(leavesQuantity, brokerContext.Random.Next(1, 100) * 100);

			// The execution requires a price.  The price simulator will provide a bid price or ask price depending on the side of the order.
			MarketDataModel.PriceRow priceRow = MarketData.Price.FindByFeedSymbol("US TICKER", orderRow.Symbol);
			SideCode sideCode = (SideCode)orderRow.SideCode;
			executionRow.Price = 0.0m;
			if (priceRow != null)
				executionRow.Price = sideCode == SideCode.Buy || sideCode == SideCode.BuyCover ? priceRow.BidPrice : priceRow.AskPrice;

			// We're record this execution in the broker's book.
			MarketData.Execution.AddExecutionRow(executionRow);

			// Now that the order is recorded, we can update the 'leaves' to reflect the execution.
			leavesQuantity -= executionRow.Quantity;

			// This message is sent back to the customer to record the execution.  Note that we pick the web service to use from a mapping table.  This table 
			// associates the name of the firm with a preconfigured web service to communicate with that firm.
			Message message = new Message();
			message.ExecBroker = brokerContext.Symbol;
			message.Symbol = orderRow.Symbol;
			message.SenderCompID = orderRow.Source;
			message.Price = executionRow.Price;
			message.OrderID = orderRow.OrderId.ToString();
			message.OrderStatusCode = leavesQuantity == 0.0M ? OrderStatusCode.Filled : OrderStatusCode.PartiallyFilled;
			message.CumQty = executionRow.Quantity;
			message.ClOrdID = orderRow.CustomerOrderId;
			message.LeavesQty = leavesQuantity;

			// If this order is filled then remove it from the simulated order book.  If the order book is empty, then put the broker threads to sleep.
			if (leavesQuantity == 0)
			{
				MarketData.Order.RemoveOrderRow(orderRow);
				if (MarketData.Order.Count == 0)
					BrokerSimulator.orderEvent.Reset();
			}

			// This places the message in a queue of messages owned by the current broker that will route the message back to the source of the order.
			brokerContext.SendReport(message.SenderCompID, message);

		}

		/// <summary>
		/// Simulation of brokers executing destination orders.
		/// </summary>
		static void SimulateBroker(Object state)
		{

			// This defines the operating properties of this broker simulation.
			BrokerContext brokerContext = state as BrokerContext;

			// Run this until shut down from an external thread.
			while (true)
			{

				// This will wait until there is an order to handle.
				BrokerSimulator.orderEvent.WaitOne();

				try
				{

					// Access to the market data must be exclusive while we find and order and process it.
					Monitor.Enter(MarketData.SyncRoot);

					// This will put the thread to sleep until some event comes along (like a new order) that requires its attension.  The main idea is that we
					// don't want to leech the CPU while waiting for orders to simulate.
					if (MarketData.Order.Rows.Count == 0)
					{
						BrokerSimulator.orderEvent.Reset();
						continue;
					}

					// This will grab a random order and see if it can be claimed by this broker.  If the order isn't available, then we'll allow other threads to
					// try processing the order.  We'll keep this up until either an order is available (and not claimed by another simulated broker) or until we've
					// exhausted the order book.
					Int32 orderIndex = brokerContext.Random.Next(MarketData.Order.Rows.Count);
					MarketDataModel.OrderRow orderRow = MarketData.Order[orderIndex];
					if (!orderRow.IsBrokerIdNull() && orderRow.BrokerId != brokerContext.Symbol)
					{
						Monitor.Exit(MarketData.SyncRoot);
						Thread.Sleep(0);
						Monitor.Enter(MarketData.SyncRoot);
						continue;
					}

					// This broker has now taken possession of the order.  No one else will touch it.
					if (orderRow.IsBrokerIdNull())
						orderRow.BrokerId = brokerContext.Symbol;

					// This will select a random action to perform on this order.
					Int32 orderActionIndex = brokerContext.Random.Next(Enum.GetValues(typeof(OrderAction)).Length);
					BrokerSimulator.actionMap[(OrderAction)Enum.GetValues(typeof(OrderAction)).GetValue(orderActionIndex)](brokerContext, orderRow);

				}
				finally
				{
					Monitor.Exit(MarketData.SyncRoot);
				}

				// This allows other threads the chance to run.  Note that the broker simulation will run flat out until the order book has been exhausted.  We 
				// should eventually put a parameter on this to slow it down in order to demonstrate a more realistic feed.
				Thread.Sleep(Convert.ToInt32(BrokerSimulator.ExecutionFrequency));

			}

		}

		/// <summary>
		/// Start the broker simulation.
		/// </summary>
		public static void Start()
		{

			// Create a thread for each simulated broker.  Use a pool of predefined brokers to give each broker thread a personality.
			for (Int32 threadIndex = 0; threadIndex < BrokerSimulator.brokerThreadCount; threadIndex++)
			{

				// The BrokerContext is a structure that simulates a broker, including the connections a broker would have back to the source of the orders.  When
				// the broker simulator is shut down or suspended, those threads and web services need to be disposed of.
				BrokerInfo brokerInfo = BrokerSimulator.brokerList[threadIndex];
				BrokerSimulator.brokerContexts[brokerInfo.Symbol] = new BrokerContext(brokerInfo);

				// These threads will simulate the various brokers.
				BrokerSimulator.brokerSimulationThreads[threadIndex] = new Thread(new ParameterizedThreadStart(BrokerSimulator.SimulateBroker));
				BrokerSimulator.brokerSimulationThreads[threadIndex].Name = String.Format("{0} Thread", brokerInfo.Name);
				BrokerSimulator.brokerSimulationThreads[threadIndex].Start(BrokerSimulator.brokerContexts[brokerInfo.Symbol]);

			}

		}

		/// <summary>
		/// Stop the broker simulation.
		/// </summary>
		public static void Stop()
		{

			// Kill each of the broker threads.
			for (Int32 threadIndex = 0; threadIndex < BrokerSimulator.brokerThreadCount; threadIndex++)
				BrokerSimulator.brokerSimulationThreads[threadIndex].Abort();

			// This will destroy the managed resources used by the broker contexts.  These are mostly in the form of threads used to connect the broker to the
			// source.
			foreach (BrokerContext brokerContext in BrokerSimulator.brokerContexts.Values)
				brokerContext.Dispose();

		}

	}

}
