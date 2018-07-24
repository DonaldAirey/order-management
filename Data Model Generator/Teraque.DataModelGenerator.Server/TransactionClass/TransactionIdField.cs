namespace Teraque.DataModelGenerator.TransactionClass
{

    using System;
    using System.CodeDom;

	/// <summary>
	/// Creates a field that holds the records used in a transaction.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class TransactionIdField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds the records used in a transaction.
		/// </summary>
		public TransactionIdField()
		{

			//		private global::System.Guid transactionId = global::System.Guid.NewGuid();
			this.Attributes = MemberAttributes.Private;
			this.Type = new CodeGlobalTypeReference(typeof(Guid));
			this.Name = "transactionId";
			this.InitExpression = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(Guid)), "NewGuid");

		}

	}

}
