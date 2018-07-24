namespace Teraque
{

	/// <summary>
	/// Code to identify reason for order rejection.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum OrdRejReason
	{

		/// <summary>
		/// BrokerCredit
		/// </summary>
		BrokerCredit,

		/// <summary>
		/// InvalidInvestorID
		/// </summary>
		InvalidInvestorID,

		/// <summary>
		/// UnsupportedOrderCharacteristic
		/// </summary>
		UnsupportedOrderCharacteristic,

		/// <summary>
		/// SurveillenceOption
		/// </summary>
		SurveillenceOption,

		/// <summary>
		/// IncorrectQuantity
		/// </summary>
		IncorrectQuantity,

		/// <summary>
		/// IncorrectAllocatedQuantity
		/// </summary>
		IncorrectAllocatedQuantity,

		/// <summary>
		/// UnknownAccount
		/// </summary>
		UnknownAccount,

		/// <summary>
		/// PriceExceedsCurrentPriceBand
		/// </summary>
		PriceExceedsCurrentPriceBand,

		/// <summary>
		/// InvalidPriceIncrement
		/// </summary>
		InvalidPriceIncrement,

		/// <summary>
		/// UnknownSymbol
		/// </summary>
		UnknownSymbol,

		/// <summary>
		/// ExchangeClosed
		/// </summary>
		ExchangeClosed,

		/// <summary>
		/// OrderExceedsLimit
		/// </summary>
		OrderExceedsLimit,

		/// <summary>
		/// TooLateToEnter
		/// </summary>
		TooLateToEnter,

		/// <summary>
		/// UnknownOrder
		/// </summary>
		UnknownOrder,

		/// <summary>
		/// DuplicateOrder
		/// </summary>
		DuplicateOrder,

		/// <summary>
		/// DuplicateOfAVerballyCommunicatedOrder
		/// </summary>
		DuplicateOfAVerballyCommunicatedOrder,

		/// <summary>
		/// StaleOrder
		/// </summary>
		StaleOrder,

		/// <summary>
		/// Other
		/// </summary>
		Other,

		/// <summary>
		/// TradeAlongRequired
		/// </summary>
		TradeAlongRequired

	}

}