namespace Teraque
{

	using System;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Instructions for crossing one order with another.
	/// </summary>
	/// <copyright>Copyright � 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames"), Flags]
	public enum CrossingCode
	{

		/// <summary>
		/// Always consider the orders for matching.
		/// </summary>
		AlwaysMatch = 0x00000001,

		/// <summary>
		/// Preserve the original matching while the trader is away.
		/// </summary>
		Away = 0x00000008,

		/// <summary>
		/// Never consider an order for matching.
		/// </summary>
		NeverMatch = 0x00000004,

		/// <summary>
		/// No crossing specified.
		/// </summary>
		None = 0x00000000,

		/// <summary>
		/// Allow the order to route to its destination while trying to match.
		/// </summary>
		RouteToDestination = 0x00000010,

		/// <summary>
		/// Allow the users preferences decide when to match.
		/// </summary>
		UsePreferences = 0x00000002
		
	}

}
