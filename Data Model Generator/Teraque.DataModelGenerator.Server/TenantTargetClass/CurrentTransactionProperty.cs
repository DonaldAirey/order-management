namespace Teraque.DataModelGenerator.TenantTargetClass
{

    using System;
    using System.CodeDom;
    using System.Threading;
    using System.Transactions;

	/// <summary>
	/// Generates a property that gets the lock for the data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class CurrentTransactionProperty : CodeMemberProperty
	{

		/// <summary>
		/// Generates a property that gets the lock for the data model.
		/// </summary>
		public CurrentTransactionProperty(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Gets the current DataModelTransaction.
			//		/// </summary>
			//		internal DataModelTransaction CurrentTransaction
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Gets the current DataModelTransaction.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.AddRange(new CodeCustomAttributesForProperties());
			this.Attributes = MemberAttributes.Assembly | MemberAttributes.Final;
			this.Type = new CodeTypeReference(String.Format("{0}Transaction", dataModelSchema.Name));
			this.Name = "CurrentTransaction";

			//			get
			//			{
			//				try
			//				{
			CodeTryCatchFinallyStatement tryLockStatement = new CodeTryCatchFinallyStatement();

			//					global::System.Transactions.Transaction transaction = global::System.Transactions.Transaction.Current;
			//					string localIdentifier = transaction.TransactionInformation.LocalIdentifier;
			tryLockStatement.TryStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Transaction)),
					"transaction",
					new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(Transaction)), "Current")));
			tryLockStatement.TryStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(String)),
					"localIdentifier",
					new CodePropertyReferenceExpression(
						new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("transaction"), "TransactionInformation"),
						"LocalIdentifier")));

			//					global::System.Threading.Monitor.Enter(this.syncRoot);
			tryLockStatement.TryStatements.Add(
				new CodeMethodInvokeExpression(new CodeGlobalTypeReferenceExpression(typeof(Monitor)),
					"Enter",
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "syncRoot")));

			//					DataModelTransaction dataModelTransaction;
			CodeVariableReferenceExpression dataModelTransaction = new CodeRandomVariableReferenceExpression();
			tryLockStatement.TryStatements.Add(
				new CodeVariableDeclarationStatement(new CodeTypeReference(String.Format("{0}Transaction", dataModelSchema.Name)), dataModelTransaction.VariableName));

			//					if ((this.transactionTable.TryGetValue(localIdentifier, out dataModelTransaction) == false))
			//					{
			CodeConditionStatement ifTransactionFound = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeMethodInvokeExpression(
						new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionTable"),
						"TryGetValue",
						new CodeVariableReferenceExpression("localIdentifier"),
						new CodeDirectionExpression(FieldDirection.Out, dataModelTransaction)),
					CodeBinaryOperatorType.IdentityEquality,
					new CodePrimitiveExpression(false)));

			//						dataModelTransaction = new DataModelTransaction(transaction, this);
			ifTransactionFound.TrueStatements.Add(
				new CodeAssignStatement(
					dataModelTransaction,
					new CodeObjectCreateExpression(
						new CodeTypeReference(String.Format("{0}Transaction", dataModelSchema.Name)),
						new CodeVariableReferenceExpression("transaction"),
						new CodeThisReferenceExpression())));

			//						this.transactionTable.Add(localIdentifier, dataModelTransaction);
			ifTransactionFound.TrueStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeVariableReferenceExpression("transactionTable"),
					"Add",
					new CodeVariableReferenceExpression("localIdentifier"),
					dataModelTransaction));

			//						transaction.TransactionCompleted += new global::System.Transactions.TransactionCompletedEventHandler(this.OnTransactionCompleted);
			ifTransactionFound.TrueStatements.Add(
				new CodeAttachEventStatement(
					new CodeEventReferenceExpression(new CodeVariableReferenceExpression("transaction"), "TransactionCompleted"),
					new CodeObjectCreateExpression(
						new CodeGlobalTypeReference(typeof(TransactionCompletedEventHandler)),
						new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "OnTransactionCompleted"))));

			//					}
			//					return dataModelTransaction;
			tryLockStatement.TryStatements.Add(ifTransactionFound);
			tryLockStatement.TryStatements.Add(new CodeMethodReturnStatement(dataModelTransaction));

			//				}
			//				finally
			//				{
			//					global::System.Threading.Monitor.Exit(this.syncRoot);
			//				}
			tryLockStatement.FinallyStatements.Add(
					new CodeMethodInvokeExpression(
						new CodeGlobalTypeReferenceExpression(typeof(Monitor)),
						"Exit",
						new CodeFieldReferenceExpression(new CodeThisReferenceExpression(),"syncRoot")));

			//			}
			//		}
			this.GetStatements.Add(tryLockStatement);

			//		}

		}

	}

}
