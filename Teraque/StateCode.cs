namespace Teraque
{

	/// <summary>
	/// States of an order.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum StateCode 
	{

		/// <summary>
		/// The order has been acknowledged.
		/// </summary>
		Acknowledged,

		/// <summary>
		/// The cancellatoin was confirmed.
		/// </summary>
		CancelAcknowledged,

		/// <summary>
		/// The order is cancelled.
		/// </summary>
		Canceled,

		/// <summary>
		/// The order is awaiting a cancelation.
		/// </summary>
		CancelPending,

		/// <summary>
		/// The cancellation was rejected.
		/// </summary>
		CancelRejected,

		/// <summary>
		/// The order is done for the day.
		/// </summary>
		DoneForDay,

		/// <summary>
		/// The order is in an error state.
		/// </summary>
		Error,

		/// <summary>
		/// The order is in the initial state.
		/// </summary>
		Initial,

		/// <summary>
		/// The order hs been rejected.
		/// </summary>
		Rejected,

		/// <summary>
		/// The replacement has been acknowleged.
		/// </summary>
		ReplaceAcknowledged,

		/// <summary>
		/// The order has been replaced.
		/// </summary>
		Replaced,

		/// <summary>
		/// The order is awating a replacement.
		/// </summary>
		ReplacePending,

		/// <summary>
		/// The replacement has been rejected.
		/// </summary>
		ReplaceRejected,

		/// <summary>
		/// The order has been sent.
		/// </summary>
		Sent,

		/// <summary>
		/// The order is stopped.
		/// </summary>
		Stopped
		
	}

}
