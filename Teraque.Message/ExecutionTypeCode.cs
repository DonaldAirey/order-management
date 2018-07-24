namespace Teraque
{

	/// <summary>
	/// Describes the specific ExecutionRpt (i.e. Pending Cancel) while OrdStatus will always identify the current order status (i.e. Partially Filled)
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum ExecutionTypeCode
	{

		/// <summary>
		/// New
		/// </summary>
		New,

		/// <summary>
		/// DoneForDay
		/// </summary>
		DoneForDay,

		/// <summary>
		/// Canceled
		/// </summary>
		Canceled,

		/// <summary>
		/// Replaced
		/// </summary>
		Replaced,

		/// <summary>
		/// PendingCancel
		/// </summary>
		PendingCancel,

		/// <summary>
		/// Stopped
		/// </summary>
		Stopped,

		/// <summary>
		/// Rejected
		/// </summary>
		Rejected,

		/// <summary>
		/// Suspended
		/// </summary>
		Suspended,

		/// <summary>
		/// PendingNew
		/// </summary>
		PendingNew,

		/// <summary>
		/// Calculated
		/// </summary>
		Calculated,

		/// <summary>
		/// Expired
		/// </summary>
		Expired,

		/// <summary>
		/// Restated
		/// </summary>
		Restated,

		/// <summary>
		/// PendingReplace
		/// </summary>
		PendingReplace,

		/// <summary>
		/// Trade
		/// </summary>
		Trade,

		/// <summary>
		/// TradeCorrect
		/// </summary>
		TradeCorrect,

		/// <summary>
		/// TradeCancel
		/// </summary>
		TradeCancel,

		/// <summary>
		/// OrderStatus
		/// </summary>
		OrderStatus,

		/// <summary>
		/// TradeInAClearingHold
		/// </summary>
		TradeInAClearingHold,

		/// <summary>
		/// TradeHasBeenReleasedToClearing
		/// </summary>
		TradeHasBeenReleasedToClearing,

		/// <summary>
		/// TriggeredOrActivatedBySystem
		/// </summary>
		TriggeredOrActivatedBySystem

	}

}
