namespace Teraque
{

	using System;
    using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Describes whether an asset is aquired or disposed.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum OrderStatusCode
	{

		/// <summary>
		/// New.
		/// </summary>
		New,

		/// <summary>
		/// Partially Filled.
		/// </summary>
		PartiallyFilled,

		/// <summary>
		/// Filled.
		/// </summary>
		Filled,

		/// <summary>
		/// Done for the day.
		/// </summary>
		DoneForDay,

		/// <summary>
		/// Canceled.
		/// </summary>
		Canceled,

		/// <summary>
		/// Replaced.
		/// </summary>
		Replaced,

		/// <summary>
		/// Waiting for a Cancel confirmation.
		/// </summary>
		PendingCancel,

		/// <summary>
		/// Stopped.
		/// </summary>
		Stopped,

		/// <summary>
		/// Rejected.
		/// </summary>
		Rejected,

		/// <summary>
		/// Suspended.
		/// </summary>
		Suspended,

		/// <summary>
		/// Waiting for a confirmation on a new order.
		/// </summary>
		PendingNew,

		/// <summary>
		/// Calculated.
		/// </summary>
		Calculated,

		/// <summary>
		/// Expired.
		/// </summary>
		Expired,

		/// <summary>
		/// Accepted for bidding.
		/// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ForBidding")]
        AcceptedForBidding,

		/// <summary>
		/// Waiting for confirmation that the order has been replaced.
		/// </summary>
		PendingReplace

	}

}
