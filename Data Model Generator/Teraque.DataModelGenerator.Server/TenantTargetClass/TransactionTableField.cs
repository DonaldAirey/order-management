namespace Teraque.DataModelGenerator.TenantTargetClass
{

    using System;
    using System.CodeDom;

	/// <summary>
	/// Creates a field that provides a way to look up a transaction based on the local transaction identifier.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class TransactionTableField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that provides a way to look up a transaction based on the local transaction identifier.
		/// </summary>
		public TransactionTableField(DataModelSchema dataModelSchema)
		{

			//				private static global::System.Collections.Generic.Dictionary<string, DataModelTransaction> transactionTable = new global::System.Collections.Generic.Dictionary<string, DataModelTransaction>();
			this.Attributes = MemberAttributes.Private;
			this.Type = new CodeTypeReference(String.Format("global::System.Collections.Generic.Dictionary<string, {0}Transaction>", dataModelSchema.Name));
			this.Name = "transactionTable";
			this.InitExpression = new CodeObjectCreateExpression(this.Type);

		}

	}

}
