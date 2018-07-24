namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Threading;

	/// <summary>
	/// Manages the connections back to the tenants that generated orders.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class SourceContext
	{

		/// <summary>
		/// The information needed to connect to an order source.
		/// </summary>
		ClientInfo clientInfoField = new ClientInfo();

		/// <summary>
		/// The messages queued up for this order source.
		/// </summary>
		Queue<Message> messageQueueField = new Queue<Message>();

		/// <summary>
		/// The event used to put to put the message thread in a 'Wait' state when no order reports are available.
		/// </summary>
		ManualResetEvent orderEventsField = new ManualResetEvent(false);

		/// <summary>
		/// The thread used to send messages back to the source of the order.
		/// </summary>
		Thread threadField;

		/// <summary>
		/// Gets the information needed to connect to an order source.
		/// </summary>
		public ClientInfo ClientInfo
		{
			get
			{
				return this.clientInfoField;
			}
		}

		/// <summary>
		/// Gets the messages queued up for this order source.
		/// </summary>
		public Queue<Message> MessageQueue
		{
			get
			{
				return this.messageQueueField;
			}
		}

		/// <summary>
		/// Gets the manual event used to put to put the message thread in a 'Wait' state when no order reports are available.
		/// </summary>
		public ManualResetEvent OrderEvents
		{
			get
			{
				return this.orderEventsField;
			}
		}

		/// <summary>
		/// Gets the thread used to send messages back to the source of the order.
		/// </summary>
		public Thread Thread
		{
			get
			{
				return this.threadField;
			}
			set
			{
				this.threadField = value;
			}
		}

	}

}
