namespace Teraque.DataModelGenerator.TransactionLogItemClass
{

    using System;
    using System.CodeDom;

	/// <summary>
	/// Creates a field that holds the data in a transaction log item.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class DataField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds the data in a transaction log item.
		/// </summary>
		public DataField()
		{

			//		internal object[] data;
			this.Attributes = MemberAttributes.Assembly;
			this.Type = new CodeGlobalTypeReference(typeof(Object[]));
			this.Name = "data";

		}

	}

}
