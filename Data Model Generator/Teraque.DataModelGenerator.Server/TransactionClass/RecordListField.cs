namespace Teraque.DataModelGenerator.TransactionClass
{

    using System.CodeDom;

	/// <summary>
	/// Creates a field that holds the records used in a transaction.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class RecordListField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds the records used in a transaction.
		/// </summary>
		public RecordListField()
		{

			//		private global::System.Collections.Generic.List<global::Teraque.IRow> recordList = new global::System.Collections.Generic.List<global::Teraque.IRow>();
			this.Attributes = MemberAttributes.Private;
			this.Type = new CodeTypeReference("global::System.Collections.Generic.List<global::Teraque.IRow>");
			this.Name = "recordList";
			this.InitExpression = new CodeObjectCreateExpression(new CodeTypeReference("global::System.Collections.Generic.List<global::Teraque.IRow>"));

		}

	}

}
