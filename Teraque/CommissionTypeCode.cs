namespace Teraque
{

	/// <summary>
	/// The different ways that commission is assessed.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum CommissionTypeCode
	{

		/// <summary>
		/// A hudredth of a percent (BIP) of the market value.
		/// </summary>
		BasisPoint,

		/// <summary>
		/// No commission.
		/// </summary>
		Empty,

		/// <summary>
		/// A flat fee.
		/// </summary>
		Fee,

		/// <summary>
		/// A percent of the market value.
		/// </summary>
		Percent,

		/// <summary>
		/// The commission is calculated based on a schedule.
		/// </summary>
		Schedule

	}

}
