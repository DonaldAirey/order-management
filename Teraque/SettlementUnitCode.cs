namespace Teraque
{

	/// <summary>
	/// Units for setting an order.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum SettlementUnitCode
	{

		/// <summary>
		/// Settlement is specified in 100th of a percentage point (BIP).
		/// </summary>
		BasisPoint,

		/// <summary>
		/// Not defined.
		/// </summary>
		Empty,

		/// <summary>
		/// Settlement is in terms of market value.
		/// </summary>
		MarketValue,

		/// <summary>
		/// Settlement is specified in terms of percent.
		/// </summary>
		Percent

	}

}
