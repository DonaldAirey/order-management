namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Security.Principal;
	using System.ServiceModel;
	using System.Threading;
	using System.Transactions;
	using Teraque;
	using Teraque.AssetNetwork.MarketService;

	/// <summary>
	/// Manages the business logic for the server data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	public static class MarketEngine
	{

		/// <summary>
		/// Orders are chunked up in lots this big before being sent to the market.
		/// </summary>
		const Int32 chunkSize = 128;

		/// <summary>
		/// This is the thread that send the orders to the destination.
		/// </summary>
		static Thread destinationThread;

		/// <summary>
		/// This provides a connection between the database change event that generates the orders and the transmission of the order to the destination.
		/// </summary>
		static Queue<Message> messageQueue = new Queue<Message>();

		/// <summary>
		/// Used to put the destinationThread to sleep when there are no orders to process.
		/// </summary>
		static ManualResetEvent orderEvent = new ManualResetEvent(false);

		/// <summary>
		/// The amount of time to wait (in milliseconds) after a communication error before trying again.
		/// </summary>
		const Int32 retryTime = 1000;

		/// <summary>
		/// Used to coordinate thread activity.
		/// </summary>
		static Object syncRoot = new Object();

		/// <summary>
		/// Handles a change to the DestinationOrder table.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event data.</param>
		static void OnDestinationOrderRowChanged(object sender, DataModel.DestinationOrderRowChangeEventArgs e)
		{

			// This will turn new DestinationOrder records into orders that can be sent to a destination for execution.
			if (e.Action == DataRowAction.Add)
			{

				// The current transaction is going to be needed to lock records.
				DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

				// Create a new message for the order we're going to build from the DestinationOrder.
				Message message = new Message();

				// The market execution engine will need to know the source firm so it knows how to route the order back.
				OrganizationPrincipal organizationPrincipal = Thread.CurrentPrincipal as OrganizationPrincipal;
				message.SenderCompID = organizationPrincipal.Organization;

				// Copy the basic properties of the DestinationOrder into the message.
				DataModel.DestinationOrderRow destinationOrderRow = e.Row;
				message.ClOrdID = destinationOrderRow.DestinationOrderId.ToString();
				message.OrderQty = destinationOrderRow.OrderedQuantity;
				message.OrdType = destinationOrderRow.OrderTypeCode;
				message.SideCode = destinationOrderRow.SideCode;
				message.TimeInForceCode = destinationOrderRow.TimeInForceCode;

				// Get the symbol to use as a security identifier.
				DataModel.SecurityRow securityRow = destinationOrderRow.SecurityRowByFK_Security_DestinationOrder_SecurityId;
				securityRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
				dataModelTransaction.AddLock(securityRow);
				if (securityRow.RowState == DataRowState.Detached)
					throw new FaultException<RecordNotFoundFault>(new RecordNotFoundFault("Security", new Object[] { destinationOrderRow.SecurityId }));
				message.Symbol = securityRow.Symbol;

				// This will put the new order in a queue.  The DestinatonThread will pull it out, batch it up and send it to the destination to be executed.
				lock (MarketEngine.syncRoot)
				{
					MarketEngine.messageQueue.Enqueue(message);
					if (messageQueue.Count == 1)
						MarketEngine.orderEvent.Set();
				}

			}

		}

		/// <summary>
		/// Extracts orders from the queue and passes them on to the market service to be executed.
		/// </summary>
		static void MarketThread()
		{

			// This provides a connection to a destination that will simulate the execution of orders.
			MarketServiceClient marketServiceClient = new MarketServiceClient();

			while (true)
			{

				// The thread will wait here until there is an order to process.
				MarketEngine.orderEvent.WaitOne();

				// The general idea is to pull messages out of the queue until either the queue is empty or we've got a chunk big enough to send to the 
				// destination.  When the queue has been emptied, we're going to put the thread in a wait until more orders arrive.
				List<Message> messages = new List<Message>();
				lock (MarketEngine.syncRoot)
				{
					while (MarketEngine.messageQueue.Count != 0 && messages.Count < MarketEngine.chunkSize)
						messages.Add(MarketEngine.messageQueue.Dequeue());
					if (MarketEngine.messageQueue.Count == 0)
						MarketEngine.orderEvent.Reset();
				}

				// This will guarantee the delivery of the block of messages.  This loop will continue to try to send execute the web method until until the market
				// service either accepts it or the thread is killed externally.  If the client finds itself in the faulted state after any exception, we'll try
				// to create it again.  Note that we give the thread a little time to sleep when we do run into a fault so as to keep this loop from eating up the
				// CPU when the market service isn't around to process the web service request.
				while (true)
				{
					try
					{
						marketServiceClient.ExecuteOrder(messages.ToArray());
						break;
					}
					catch
					{
						if (marketServiceClient.State != CommunicationState.Opened)
						{
							marketServiceClient = new MarketServiceClient();
							Thread.Sleep(MarketEngine.retryTime);
						}
					}
				}

			}

		}

		/// <summary>
		/// Starts the Market Engine.
		/// </summary>
		public static void Start()
		{

			// These event handlers will pump the destination order into the simulation.
			DataModel.DestinationOrder.DestinationOrderRowValidate += MarketEngine.OnDestinationOrderRowChanged;

			// This thread will pull orders out of the queue and send them to the destination.
			MarketEngine.destinationThread = new Thread(MarketEngine.MarketThread);
			destinationThread.Name = "Destination Thread";
			destinationThread.IsBackground = true;
			destinationThread.Start();

		}

		/// <summary>
		/// Stops the Market Engine.
		/// </summary>
		public static void Stop()
		{

			// These event handlers will pump the destination order into the simulation.
			DataModel.DestinationOrder.DestinationOrderRowChanging -= MarketEngine.OnDestinationOrderRowChanged;

			// The destination thread spends most of its time waiting, so it won't respond to other methods of signaling it to close.  This will terminate the 
			// thread no matter what state it is in.
			if (MarketEngine.destinationThread.IsAlive)
				if (!MarketEngine.destinationThread.Join(1000))
					MarketEngine.destinationThread.Abort();

		}

	}

}
