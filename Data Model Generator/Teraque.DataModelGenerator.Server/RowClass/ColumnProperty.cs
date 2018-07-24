namespace Teraque.DataModelGenerator.RowClass
{

	using System;
    using System.CodeDom;
	using System.Data;

    /// <summary>
	/// Creates a property that gets or sets the value of an item in a row.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class ColumnProperty : CodeMemberProperty
	{

		/// <summary>
		/// Creates a property that gets or sets the value of an item in a row.
		/// </summary>
		/// <param name="tableSchema">The table to which this row belongs.</param>
		/// <param name="columnSchema">The nullable column.</param>
		public ColumnProperty(TableSchema tableSchema, ColumnSchema columnSchema)
		{

			//		/// <summary>
			//		/// Gets or sets the data in the AccountId column.
			//		/// </summary>
			//		public global::System.Guid AccountId
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Gets or sets the data in the {0} column.", columnSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.Type = new CodeGlobalTypeReference(columnSchema.DataType);
			this.Name = columnSchema.Name;

			//			get
			//			{
			//				DataModelTransaction r1233 = DataModel.Current;
			CodeVariableReferenceExpression transactionExpression = new CodeRandomVariableReferenceExpression();
			this.GetStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference(String.Format("{0}Transaction", tableSchema.DataModel.Name)),
					transactionExpression.VariableName,
					new CodePropertyReferenceExpression(
						new CodeTypeReferenceExpression(tableSchema.DataModel.Name), "CurrentTransaction")));

			//				if ((this.IsLockHeld(dataModelTransaction.transactionId) == false))
			//				{
			//					throw new global::System.ServiceModel.FaultException<SynchronizationLockFault>(new global::Teraque.SynchronizationLockFault("Account"));
			//				}
			this.GetStatements.AddRange(
				new CodeCheckReaderLockHeldStatements(new CodeThisReferenceExpression(), tableSchema, transactionExpression));

			//				return ((global::System.Guid)(this[this.tableAccount.AccountIdColumn]));
			//			}
			CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement(
				new CodeCastExpression(
					this.Type,
					new CodeArrayIndexerExpression(
							new CodeThisReferenceExpression(),
							new CodePropertyReferenceExpression(
								new CodePropertyReferenceExpression(
									new CodeThisReferenceExpression(), String.Format("table{0}", tableSchema.Name)),
								String.Format("{0}Column", columnSchema.Name)))));
			if (columnSchema.IsNullable)
			{
				CodeTryCatchFinallyStatement tryCatchBlock = new CodeTryCatchFinallyStatement();
				tryCatchBlock.TryStatements.Add(returnStatement);
				CodeCatchClause catchStrongTypeException = new CodeCatchClause("e", new CodeGlobalTypeReference(typeof(InvalidCastException)));
				catchStrongTypeException.Statements.Add(
					new CodeThrowExceptionStatement(
						new CodeObjectCreateExpression(
							new CodeGlobalTypeReference(typeof(StrongTypingException)),
							new CodePrimitiveExpression("Cannot get value because it is DBNull."),
							new CodeArgumentReferenceExpression("e"))));
				tryCatchBlock.CatchClauses.Add(catchStrongTypeException);
				this.GetStatements.Add(tryCatchBlock);
			}
			else
			{
				this.GetStatements.Add(returnStatement);
			}

			//			set
			//			{
			//				DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;
			this.SetStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference(String.Format("{0}Transaction", tableSchema.DataModel.Name)),
					transactionExpression.VariableName,
					new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(tableSchema.DataModel.Name), "CurrentTransaction")));

			//				if ((this.IsWriterLockHeld(dataModelTransaction.transactionId) == false))
			//				{
			//					throw new global::System.ServiceModel.FaultException<SynchronizationLockFault>(new global::Teraque.SynchronizationLockFault("Account"));
			//				}
			this.SetStatements.AddRange(new CodeCheckWriterLockHeldStatements(new CodeThisReferenceExpression(), tableSchema, transactionExpression));

			//				this[this.tableAccount.AccountIdColumn] = value;
			//			}
			this.SetStatements.Add(new CodeAssignStatement(new CodeArrayIndexerExpression(new CodeThisReferenceExpression(), new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", tableSchema.Name)), String.Format("{0}Column", columnSchema.Name))), new CodePropertySetValueReferenceExpression()));

			//		}

		}

	}

}
