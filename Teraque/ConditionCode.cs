namespace Teraque
{

	/// <summary>
	/// Conditions for the execution of an order.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum ConditionCode
	{

		/// <summary>
		/// Execute all or none of the order.
		/// </summary>
		AllOrNone,

		/// <summary>
		/// Execute all or none and do not reduce the order.
		/// </summary>
		AllOrNoneDoNotReduce,

		/// <summary>
		/// Do not reduce the order.
		/// </summary>
		DoNotReduce

	}

}
