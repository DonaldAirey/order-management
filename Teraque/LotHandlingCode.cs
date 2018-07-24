namespace Teraque
{

	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Describes the handling of the order.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum LotHandlingCode
	{

		/// <summary>
		/// First In/First Out
		/// </summary>
		Fifo,

		/// <summary>
		/// Last In/First Out
		/// </summary>
		Lifo,

		/// <summary>
		/// Minimize the taxes
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mintax")]
		Mintax

	}

}
