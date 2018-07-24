namespace Teraque.DataModelGenerator.TransactionClass
{

    using System.CodeDom;

	/// <summary>
	/// Creates a field that holds the locks used in a transaction.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class LockListField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds the locks used in a transaction.
		/// </summary>
		public LockListField()
		{

			//		private global::System.Collections.Generic.List<global::Teraque.IRow> lockList = new global::System.Collections.Generic.List<global::Teraque.IRow>();
			this.Attributes = MemberAttributes.Private;
			this.Type = new CodeTypeReference("global::System.Collections.Generic.List<global::Teraque.IRow>");
			this.Name = "lockList";
			this.InitExpression = new CodeObjectCreateExpression(new CodeTypeReference("global::System.Collections.Generic.List<global::Teraque.IRow>"));

		}

	}

}
