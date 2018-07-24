namespace Teraque
{

	/// <summary>
	/// Code to identify reason for a Business Message Reject message.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum BusinessRejectReasonCode
	{

		/// <summary>
		/// Other
		/// </summary>
		Other,

		/// <summary>
		/// InvalidPriceIncrement
		/// </summary>
		InvalidPriceIncrement,

		/// <summary>
		/// UnknownID
		/// </summary>
		UnknownID,

		/// <summary>
		/// UnknownSecurity
		/// </summary>
		UnknownSecurity,

		/// <summary>
		/// UnsupportedMessageType
		/// </summary>
		UnsupportedMessageType,

		/// <summary>
		/// ApplicationNotAvailable
		/// </summary>
		ApplicationNotAvailable,

		/// <summary>
		/// ConditionallyRequiredFieldMissing
		/// </summary>
		ConditionallyRequiredFieldMissing,

		/// <summary>
		/// NotAuthorized
		/// </summary>
		NotAuthorized,

		/// <summary>
		/// DeliverToFirmNotAvailableAtThisTime
		/// </summary>
		DeliverToFirmNotAvailableAtThisTime

	}

}