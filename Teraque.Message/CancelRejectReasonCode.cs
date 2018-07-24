namespace Teraque 
{

	/// <summary>
	/// Code to identify reason for cancel rejection.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum CancelRejectReasonCode
	{

		/// <summary>
		/// TooLateToCancel
		/// </summary>
		TooLateToCancel,

		/// <summary>
		/// InvalidPriceIncrement
		/// </summary>
		InvalidPriceIncrement,

		/// <summary>
		/// UnknownOrder
		/// </summary>
		UnknownOrder,

		/// <summary>
		/// BrokerCredit
		/// </summary>
		BrokerCredit,

		/// <summary>
		/// OrderAlreadyInPendingStatus
		/// </summary>
		OrderAlreadyInPendingStatus,

		/// <summary>
		/// UnableToProcessOrderMassCancelRequest
		/// </summary>
		UnableToProcessOrderMassCancelRequest,

		/// <summary>
		/// OrigOrdModTime
		/// </summary>
		OrigOrdModTime,

		/// <summary>
		/// DuplicateClOrdID
		/// </summary>
		DuplicateClOrdID,

		/// <summary>
		/// PriceExceedsCurrentPrice
		/// </summary>
		PriceExceedsCurrentPrice,

		/// <summary>
		/// PriceExceedsCurrentPriceBand
		/// </summary>
		PriceExceedsCurrentPriceBand,

		/// <summary>
		/// Other
		/// </summary>
		Other

	}

}
