namespace Teraque.DataModelGenerator.TransactionClass
{

    using System;
    using System.CodeDom;
    using System.Data.SqlClient;
    using System.Transactions;

	/// <summary>
	/// Creates a static constructor for the transaction.
	/// </summary>
	class VoidConstructor : CodeConstructor
	{

		/// <summary>
		/// Creates a static constructor for the transaction.
		/// </summary>
		/// <param name="dataModelSchema">A description of the data model.</param>
		public VoidConstructor(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Initializes a new instance of the DataModelTransaction class.
			//		/// </summary>
			//		/// <param name="transaction">The underlying transaction.</param>
			//		/// <param name="tenantDataSet">The data model to which this transaction applies.</param>
			//		internal DataModelTransaction(global::System.Transactions.Transaction transaction, TenantTarget tenantDataSet)
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Initializes a new instance of the {0}Transaction class.", dataModelSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Comments.Add(new CodeCommentStatement("<param name=\"transaction\">The underlying transaction.</param>", true));
			this.Comments.Add(new CodeCommentStatement("<param name=\"tenantDataSet\">The data model to which this transaction applies.</param>", true));
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeGlobalTypeReference(typeof(Transaction)), "transaction"));
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(String.Format("Tenant{0}", dataModelSchema.Name)), "tenantDataSet"));
			this.Attributes = MemberAttributes.Assembly;

			//			this.tenantDataSet = tenantDataSet;
			this.Statements.Add(
				new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "tenantDataSet"), new CodeArgumentReferenceExpression("tenantDataSet")));

			//			this.sqlConnection = new global::System.Data.SqlClient.SqlConnection(this.tenantDataSet.connectionString);
			this.Statements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "sqlConnection"),
					new CodeObjectCreateExpression(
						new CodeGlobalTypeReference(typeof(SqlConnection)),
						new CodeFieldReferenceExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "tenantDataSet"), "connectionString"))));

			//			this.sqlConnection.Open();
			this.Statements.Add(
				new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "sqlConnection"), "Open"));

			//			this.sqlConnection.EnlistTransaction(transaction);
			this.Statements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "sqlConnection"),
					"EnlistTransaction",
					new CodeArgumentReferenceExpression("transaction")));

			//			transaction.EnlistVolatile(this, global::System.Transactions.EnlistmentOptions.None);
			this.Statements.Add(
				new CodeMethodInvokeExpression(
					new CodeArgumentReferenceExpression("transaction"),
					"EnlistVolatile",
					new CodeThisReferenceExpression(),
					new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(EnlistmentOptions)), "None")));

			//		}

		}

	}

}
