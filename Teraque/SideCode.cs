namespace Teraque
{

	/// <summary>
	/// Describes whether an asset is aquired or disposed.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum SideCode
	{

		/// <summary>
		/// As Defined.
		/// </summary>
		AsDefined,

		/// <summary>
		/// Borrow.
		/// </summary>
		Borrow,

		/// <summary>
		/// Buy.
		/// </summary>
		Buy,

		/// <summary>
		/// Covers a previous short sell.
		/// </summary>
		BuyCover,

		/// <summary>
		/// Buy minus.
		/// </summary>
		BuyMinus,

		/// <summary>
		/// Cross an order.
		/// </summary>
		Cross,

		/// <summary>
		/// Sell short on a crossing network.
		/// </summary>
		CrossShort,

		/// <summary>
		/// Sell short exempt from the up-tick rule on a crossing network.
		/// </summary>
		CrossShortExempt,

		/// <summary>
		/// Deposit currency into an account.
		/// </summary>
		Deposit,

		/// <summary>
		/// Lend.
		/// </summary>
		Lend,

		/// <summary>
		/// None.
		/// </summary>
		None,

		/// <summary>
		/// Opposite.
		/// </summary>
		Opposite,

		/// <summary>
		/// Redeem a coupon.
		/// </summary>
		Redeem,

		/// <summary>
		/// Sell.
		/// </summary>
		Sell,

		/// <summary>
		/// Sell on the next plus tick.
		/// </summary>
		SellPlus,

		/// <summary>
		/// Sell a borrowed security.
		/// </summary>
		SellShort,

		/// <summary>
		/// Sell short exempt from the up-tick rule.
		/// </summary>
		SellShortExempt,

		/// <summary>
		/// Subscribe to a liquidity pool.
		/// </summary>
		Subscribe,

		/// <summary>
		/// Side is not disclosed.
		/// </summary>
		Undisclosed,

		/// <summary>
		/// Widthdraw money from an account.
		/// </summary>
		Withdraw

	}

}
