namespace Teraque
{

	/// <summary>
	/// Code to identify reason for a session-level Reject message.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum SessionRejectReason
	{

		/// <summary>
		/// InvalidTagNumber
		/// </summary>
		InvalidTagNumber,

		/// <summary>
		/// SendingTimeAccuracyProblem
		/// </summary>
		SendingTimeAccuracyProblem,

		/// <summary>
		/// InvalidMsgType
		/// </summary>
		InvalidMsgType,

		/// <summary>
		/// XMLValidationError
		/// </summary>
		XMLValidationError,

		/// <summary>
		/// TagAppearsMoreThanOnce
		/// </summary>
		TagAppearsMoreThanOnce,

		/// <summary>
		/// TagSpecifiedOutOfRequiredOrder
		/// </summary>
		TagSpecifiedOutOfRequiredOrder,

		/// <summary>
		/// RepeatingGroupFieldsOutOfOrder
		/// </summary>
		RepeatingGroupFieldsOutOfOrder,

		/// <summary>
		/// IncorrectNumInGroupCountForRepeatingGroup
		/// </summary>
		IncorrectNumInGroupCountForRepeatingGroup,

		/// <summary>
		/// None
		/// </summary>
		None,

		/// <summary>
		/// Invalid
		/// </summary>
		Invalid,

		/// <summary>
		/// RequiredTagMissing
		/// </summary>
		RequiredTagMissing,

		/// <summary>
		/// TagNotDefinedForThisMessageType
		/// </summary>
		TagNotDefinedForThisMessageType,

		/// <summary>
		/// UndefinedTag
		/// </summary>
		UndefinedTag,

		/// <summary>
		/// TagSpecifiedWithoutAValue
		/// </summary>
		TagSpecifiedWithoutAValue,

		/// <summary>
		/// ValueIsIncorrect
		/// </summary>
		ValueIsIncorrect,

		/// <summary>
		/// IncorrectDataFormatForValue
		/// </summary>
		IncorrectDataFormatForValue,

		/// <summary>
		/// DecryptionProblem
		/// </summary>
		DecryptionProblem,

		/// <summary>
		/// SignatureProblem
		/// </summary>
		SignatureProblem,

		/// <summary>
		/// Other
		/// </summary>
		Other,

		/// <summary>
		/// CompIDProblem
		/// </summary>
		CompIDProblem

	}

}
