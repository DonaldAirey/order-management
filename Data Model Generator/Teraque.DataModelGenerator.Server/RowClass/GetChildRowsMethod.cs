namespace Teraque.DataModelGenerator.RowClass
{

	using System;
	using System.CodeDom;

    /// <summary>
	/// Represents a declaration of a method that gets a list of the child rows.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class GetChildRowsMethod : CodeMemberMethod
	{

		/// <summary>
		/// Generates a method to get a list of child rows.
		/// </summary>
		/// <param name="relationSchema">A description of the relation between two tables.</param>
		public GetChildRowsMethod(RelationSchema relationSchema)
		{

			// These variables are used to construct the method.
			TableSchema childTable = relationSchema.ChildTable;
			TableSchema parentTable = relationSchema.ParentTable;
			string rowTypeName = String.Format("{0}Row", childTable.Name);
			string tableFieldName = String.Format("table{0}", parentTable.Name);
			string childRowTypeName = String.Format("{0}Row", relationSchema.ChildTable.Name);

			//		/// <summary>
			//		/// Gets the children rows in the AccountGroup table.
			//		/// </summary>
			//		public AccountGroupRow[] GetAccountGroupRows()
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Gets the children rows in the {0} table.", relationSchema.ChildTable.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.ReturnType = new CodeTypeReference(childRowTypeName, 1);
			this.Name = relationSchema.IsDistinctPathToChild ?
				String.Format("Get{0}s", childRowTypeName) :
				String.Format("Get{0}sBy{1}", childRowTypeName, relationSchema.Name);
			string relationName = relationSchema.IsDistinctPathToChild ?
				String.Format("{0}{1}Relation", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name) :
				String.Format("{0}{1}By{2}Relation", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name, relationSchema.Name);

			//			DataModelTransaction p7519 = DataModel.CurrentTransaction;
			CodeVariableReferenceExpression transactionExpression = new CodeRandomVariableReferenceExpression();
			this.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference(String.Format("{0}Transaction", parentTable.DataModel.Name)),
					transactionExpression.VariableName,
					new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(parentTable.DataModel.Name), "CurrentTransaction")));

			//			if ((this.IsLockHeld(p7519.TransactionId) == false))
			//			{
			//				throw new global::System.ServiceModel.FaultException<Teraque.SynchronizationLockFault>(new global::Teraque.SynchronizationLockFault("AccountBase"));
			//			}
			this.Statements.AddRange(new CodeCheckReaderLockHeldStatements(new CodeThisReferenceExpression(), relationSchema.ParentTable, transactionExpression));

			//			try
			//			{
			CodeTryCatchFinallyStatement tryCatchFinallyStatement = new CodeTryCatchFinallyStatement();

			//				((TenantDataModel)(this.Table.DataSet)).dataLock.EnterReadLock();
			tryCatchFinallyStatement.TryStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(
						new CodeCastExpression(
							new CodeTypeReference(String.Format("Tenant{0}", parentTable.DataModel.Name)),
							new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Table"), "DataSet")),
						"dataLock"),
					"EnterReadLock"));

			//				return ((AccountGroupRow[])(this.GetChildRows(this.tableAccountBase.AccountBaseAccountGroupRelation)));
			tryCatchFinallyStatement.TryStatements.Add(new CodeMethodReturnStatement(new CodeCastExpression(new CodeTypeReference(childRowTypeName, 1), new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "GetChildRows", new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), tableFieldName), relationName)))));

			//			}
			//			finally
			//			{
			//				((TenantTarget)(this.Table.DataSet)).dataLock.ExitReadLock();
			tryCatchFinallyStatement.FinallyStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(
						new CodeCastExpression(
							new CodeTypeReference(String.Format("Tenant{0}", parentTable.DataModelSchema.Name)),
							new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Table"), "DataSet")),
						"dataLock"),
					"ExitReadLock"));
			this.Statements.Add(tryCatchFinallyStatement);

			//			}
			//		}

		}

	}

}
