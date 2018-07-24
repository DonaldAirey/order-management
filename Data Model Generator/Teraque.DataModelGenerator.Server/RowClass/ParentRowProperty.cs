namespace Teraque.DataModelGenerator.RowClass
{

	using System;
	using System.CodeDom;
	using System.Diagnostics;

    /// <summary>
	/// Creates a property that gets the parent row.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class ParentRowProperty : CodeMemberProperty
	{

		/// <summary>
		/// Creates a property that gets the parent row.
		/// </summary>
		/// <param name="relationSchema">The foreign key that references the parent table.</param>
		public ParentRowProperty(RelationSchema relationSchema)
		{

			// These constructs are used several times to generate the property.
			TableSchema childTable = relationSchema.ChildTable;
			TableSchema parentTable = relationSchema.ParentTable;
			string rowTypeName = String.Format("{0}Row", parentTable.Name);
			string tableFieldName = String.Format("table{0}", childTable.Name);
			string relationName = relationSchema.IsDistinctPathToParent ?
				String.Format("{0}{1}Relation", parentTable.Name, childTable.Name) :
				String.Format("{0}{1}By{2}Relation", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name, relationSchema.Name);
			string relationFieldName = relationSchema.IsDistinctPathToParent ?
				String.Format("{0}{1}Relation", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name) :
				String.Format("{0}{1}By{2}Relation", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name, relationSchema.Name);

			//		/// <summary>
			//		/// Gets the parent row in the Currency table.
			//		/// </summary>
			//		public CurrencyRow CurrencyRow
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Gets the parent row in the {0} table.", relationSchema.ParentTable.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
            this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(DebuggerNonUserCodeAttribute))));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.Type = new CodeTypeReference(rowTypeName);
			this.Name = relationSchema.IsDistinctPathToParent ? rowTypeName : String.Format("{0}By{1}", rowTypeName, relationSchema.Name);

			//			get
			//			{
			//				DataModelTransaction k7494 = DataModel.CurrentTransaction;
			CodeVariableReferenceExpression transactionExpression = new CodeRandomVariableReferenceExpression();
			this.GetStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference(String.Format("{0}Transaction", parentTable.DataModel.Name)),
					transactionExpression.VariableName,
					new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(parentTable.DataModel.Name), "CurrentTransaction")));

			//				if ((this.IsLockHeld(k7494.TransactionId) == false))
			//				{
			//					throw new global::System.ServiceModel.FaultException<Teraque.SynchronizationLockFault>(new global::Teraque.SynchronizationLockFault("AccountBase"));
			//				}
			this.GetStatements.AddRange(
				new CodeCheckReaderLockHeldStatements(new CodeThisReferenceExpression(), relationSchema.ChildTable, transactionExpression));

			//				try
			//				{
			//					((TenantDataModel)this.Table.DataSet).dataLock.EnterReadLock();
			//					return ((CurrencyRow)(this.GetParentRow(this.tableAccountBase.CurrencyAccountBaseRelation)));
			CodeTryCatchFinallyStatement tryCatchFinallyStatement = new CodeTryCatchFinallyStatement();
			tryCatchFinallyStatement.TryStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(
						new CodeCastExpression(
							new CodeTypeReference(String.Format("Tenant{0}", parentTable.DataModelSchema.Name)),
							new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Table"), "DataSet")),
						"dataLock"),
					"EnterReadLock"));
			tryCatchFinallyStatement.TryStatements.Add(
				new CodeMethodReturnStatement(
					new CodeCastExpression(
						rowTypeName,
						new CodeMethodInvokeExpression(
							new CodeThisReferenceExpression(),
							"GetParentRow",
							new CodePropertyReferenceExpression(
								new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), tableFieldName),
								relationFieldName)))));

			//				}
			//				finally
			//				{
			//					((TenantDataModel)this.Table.DataSet).dataLock.ExitReadLock();
			//				}
			tryCatchFinallyStatement.FinallyStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(
						new CodeCastExpression(
							new CodeTypeReference(String.Format("Tenant{0}", parentTable.DataModelSchema.Name)),
							new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Table"), "DataSet")),
						"dataLock"),
					"ExitReadLock"));
			this.GetStatements.Add(tryCatchFinallyStatement);

			//			}
			//		}

		}

	}

}
