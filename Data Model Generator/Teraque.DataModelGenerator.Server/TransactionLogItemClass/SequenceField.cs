namespace Teraque.DataModelGenerator.TransactionLogItemClass
{

    using System;
    using System.CodeDom;

	/// <summary>
	/// Creates a field that holds sequence of the item in the transaction log.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class SequenceField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds sequence of the item in the transaction log.
		/// </summary>
		public SequenceField()
		{

			//		internal long sequence;
			this.Attributes = MemberAttributes.Assembly;
			this.Type = new CodeGlobalTypeReference(typeof(Int64));
			this.Name = "sequence";

		}

	}

}
