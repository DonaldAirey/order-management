namespace Teraque
{

	/// <summary>
	/// Code to identify the price a DiscretionOffsetValue is related to and should be mathematically added to.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum DiscretionInstructionCode
	{

		/// <summary>
		/// RelatedToDisplayedPrice
		/// </summary>
		RelatedToDisplayedPrice,

		/// <summary>
		/// RelatedToMarketPrice
		/// </summary>
		RelatedToMarketPrice,

		/// <summary>
		/// RelatedToPrimaryPrice
		/// </summary>
		RelatedToPrimaryPrice,

		/// <summary>
		/// RelatedToLocalPrimaryPrice
		/// </summary>
		RelatedToLocalPrimaryPrice,

		/// <summary>
		/// RelatedToMidpointPrice
		/// </summary>
		RelatedToMidpointPrice,

		/// <summary>
		/// RelatedToLastTradePrice
		/// </summary>
		RelatedToLastTradePrice,

		/// <summary>
		/// RelatedToVWAP
		/// </summary>
		RelatedToVWAP,

		/// <summary>
		/// AveragePriceGuarantee
		/// </summary>
		AveragePriceGuarantee

	}

}
