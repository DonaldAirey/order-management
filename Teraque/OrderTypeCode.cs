namespace Teraque
{

    using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Describes the pricing of the order.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum OrderTypeCode
	{
		/// <summary>
		/// Market
		/// </summary>
		Market,

		/// <summary>
		/// Limit
		/// </summary>
		Limit,

		/// <summary>
		/// Stop
		/// </summary>
		Stop,

		/// <summary>
		/// StopLimit
		/// </summary>
		StopLimit,

		/// <summary>
		/// MarketOnClose
		/// </summary>
		MarketOnClose,

		/// <summary>
		/// WithOrWithout
		/// </summary>
		WithOrWithout,

		/// <summary>
		/// LimitOrBetter
		/// </summary>
		LimitOrBetter,

		/// <summary>
		/// LimitWithOrWithout
		/// </summary>
		LimitWithOrWithout,

		/// <summary>
		/// OnBasis
		/// </summary>
		OnBasis,

		/// <summary>
		/// OnClose
		/// </summary>
		OnClose,

		/// <summary>
		/// LimitOnClose
		/// </summary>
		LimitOnClose,

		/// <summary>
		/// ForexMarket
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Forex")]
		ForexMarket,

		/// <summary>
		/// PreviouslyQuoted
		/// </summary>
		PreviouslyQuoted,

		/// <summary>
		/// PreviouslyIndicated
		/// </summary>
		PreviouslyIndicated,

		/// <summary>
		/// ForexLimit
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Forex")]
		ForexLimit,

		/// <summary>
		/// ForexSwap
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Forex")]
		ForexSwap,

		/// <summary>
		/// ForexPreviouslyQuoted
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Forex")]
		ForexPreviouslyQuoted,

		/// <summary>
		/// Funari
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Funari")]
		Funari,

		/// <summary>
		/// MarketIfTouched
		/// </summary>
		MarketIfTouched,

		/// <summary>
		/// MarketWithLeftOverAsLimit
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "LeftOver")]
		MarketWithLeftOverAsLimit,

		/// <summary>
		/// PreviousFundValuationPoint
		/// </summary>
		PreviousFundValuationPoint,

		/// <summary>
		/// NextFundValuationPoint
		/// </summary>
		NextFundValuationPoint,

		/// <summary>
		/// Pegged
		/// </summary>
		Pegged,

		/// <summary>
		/// CounterOrderSelection
		/// </summary>
		CounterOrderSelection

	}

}
