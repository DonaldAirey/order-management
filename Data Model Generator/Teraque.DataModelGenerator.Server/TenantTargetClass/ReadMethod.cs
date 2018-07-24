namespace Teraque.DataModelGenerator.TenantTargetClass
{

    using System;
    using System.CodeDom;
    using System.Collections;
    using System.Data;

    /// <summary>
	/// Creates a method to handle moving the deleted records from the active data model to the deleted data model.
	/// </summary>
	class ReadMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method to handle moving the deleted records from the active data model to the deleted data model.
		/// </summary>
		/// <param name="schema">The data model schema.</param>
		public ReadMethod(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Collects the set of modified records that will reconcile the client data model to the master data model.
			//		/// </summary>
			//		/// <param name="identifier">A unique identifier of an instance of the data.</param>
			//		/// <param name="sequence">The sequence of the client data model.</param>
			//		/// <returns>An array of records that will reconcile the client data model to the server.</returns>
			//		[global::Teraque.ClaimsPrincipalPermission(global::System.Security.Permissions.SecurityAction.Demand, ClaimType=global::Teraque.ClaimTypes.Read, Resource=global::Teraque.Resources.Application)]
			//		public object[] Read(global::System.Guid identifier, long sequence)
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Collects the set of modified records that will reconcile the client data model to the master data model.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Comments.Add(new CodeCommentStatement("<param name=\"identifier\">A unique identifier of an instance of the data.</param>", true));
			this.Comments.Add(new CodeCommentStatement("<param name=\"sequence\">The sequence of the client data model.</param>", true));
			this.Comments.Add(new CodeCommentStatement("<returns>An array of records that will reconcile the client data model to the server.</returns>", true));
			this.Name = "Read";
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.ReturnType = new CodeGlobalTypeReference(typeof(Object[]));
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeGlobalTypeReference(typeof(Guid)), "identifier"));
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeGlobalTypeReference(typeof(Int64)), "sequence"));

			//			try
			//			{
			CodeTryCatchFinallyStatement tryTransactionLogLock = new CodeTryCatchFinallyStatement();

			//				DataModel.transactionLogLock.EnterReadLock();
			tryTransactionLogLock.TryStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLogLock"),
					"EnterReadLock"));

			//				object[] dataHeader = new object[3];
			tryTransactionLogLock.TryStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Object[])),
					"dataHeader", new CodeArrayCreateExpression(new CodeGlobalTypeReference(typeof(Object[])), 3)));

			//				dataHeader[0] = DataModel.identifier;
			tryTransactionLogLock.TryStatements.Add(
				new CodeAssignStatement(
					new CodeIndexerExpression(new CodeVariableReferenceExpression("dataHeader"), new CodePrimitiveExpression(0)),
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "identifier")));

			//				dataHeader[1] = ((long)(this.transactionLog.Last.Value.sequence));
			tryTransactionLogLock.TryStatements.Add(
				new CodeAssignStatement(
					new CodeIndexerExpression(new CodeVariableReferenceExpression("dataHeader"), new CodePrimitiveExpression(1)),
					new CodeCastExpression(
						new CodeTypeReference(typeof(Int64)),
						new CodeFieldReferenceExpression(
							new CodePropertyReferenceExpression(
								new CodePropertyReferenceExpression(
									new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLog"),
									"Last"),
								"Value"),
							"sequence"))));

			//				if ((identifier != DataModel.identifier))
			//				{
			CodeConditionStatement ifNewDataModel = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeArgumentReferenceExpression("identifier"),
					CodeBinaryOperatorType.IdentityInequality,
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "identifier")));

			//					sequence = long.MinValue;
			ifNewDataModel.TrueStatements.Add(
				new CodeAssignStatement(
					new CodeArgumentReferenceExpression("sequence"),
					new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(Int64)), "MinValue")));
			tryTransactionLogLock.TryStatements.Add(ifNewDataModel);

			//				}

			//				global::System.Collections.ArrayList data = new global::System.Collections.ArrayList();
			tryTransactionLogLock.TryStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(ArrayList)), "data", new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(ArrayList)))));

			//				global::System.Collections.Generic.LinkedListNode<TransactionLogItem> transactionNode = this.transactionLog.Last;
			tryTransactionLogLock.TryStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference("global::System.Collections.Generic.LinkedListNode<TransactionLogItem>"),
					"transactionNode",
					new CodePropertyReferenceExpression(
						new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLog"),
						"Last")));

			//			logLoop:
			tryTransactionLogLock.TryStatements.Add(new CodeLabeledStatement("logLoop"));

			//				if ((transactionNode == null))
			//				{
			CodeConditionStatement ifListStart = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("transactionNode"),
					CodeBinaryOperatorType.IdentityEquality,
					new CodePrimitiveExpression(null)));

			//					goto endLoop;
			ifListStart.TrueStatements.Add(new CodeGotoStatement("endLoop"));

			//				}
			tryTransactionLogLock.TryStatements.Add(ifListStart);

			//				if ((((long)(transactionNode.Value.sequence)) <= sequence))
			//				{
			CodeConditionStatement ifStale = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeCastExpression(
						new CodeGlobalTypeReference(typeof(Int64)),
						new CodeFieldReferenceExpression(
							new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("transactionNode"), "Value"),
							"sequence")),
					CodeBinaryOperatorType.LessThanOrEqual,
					new CodeVariableReferenceExpression("sequence")));

			//					goto endLoop;
			ifStale.TrueStatements.Add(new CodeGotoStatement("endLoop"));

			//				}
			tryTransactionLogLock.TryStatements.Add(ifStale);

			//				object[] transactionLogItem = ((object[])(transactionNode.Value.data));
			tryTransactionLogLock.TryStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Object[])),
					"transactionLogItem",
					new CodeCastExpression(
						new CodeGlobalTypeReference(typeof(Object[])),
						new CodeFieldReferenceExpression(
							new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("transactionNode"), "Value"),
							"data"))));

			//						global::System.Data.DataTable dataTable = DataModel.dataSet.Tables[((int)(transactionLogItem[1]))];
			tryTransactionLogLock.TryStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(DataTable)),
					"dataTable",
					new CodeIndexerExpression(
						new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Tables"),
						new CodeCastExpression(
							new CodeGlobalTypeReference(typeof(Int32)),
							new CodeIndexerExpression(
								new CodeVariableReferenceExpression("transactionLogItem"),
								new CodePrimitiveExpression(1))))));

			//						data.Add(transactionLogItem);
			tryTransactionLogLock.TryStatements.Add(
				new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("data"), "Add", new CodeVariableReferenceExpression("transactionLogItem")));

			//				transactionNode = transactionNode.Previous;
			tryTransactionLogLock.TryStatements.Add(
				new CodeAssignStatement(
					new CodeVariableReferenceExpression("transactionNode"),
					new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("transactionNode"), "Previous")));

			//				goto logLoop;
			tryTransactionLogLock.TryStatements.Add(new CodeGotoStatement("logLoop"));

			//			endLoop:
			tryTransactionLogLock.TryStatements.Add(new CodeLabeledStatement("endLoop"));
			
			//				dataHeader[2] = data.ToArray();
			tryTransactionLogLock.TryStatements.Add(
				new CodeAssignStatement(
					new CodeIndexerExpression(new CodeVariableReferenceExpression("dataHeader"), new CodePrimitiveExpression(2)),
					new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("data"), "ToArray")));

			//				return dataHeader;
			tryTransactionLogLock.TryStatements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("dataHeader")));

			//			finally
			//			{
			//				DataModel.transactionLogLock.ExitReadLock();
			tryTransactionLogLock.FinallyStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLogLock"),
					"ExitReadLock"));

			//			}
			this.Statements.Add(tryTransactionLogLock);

			//		}

		}

	}

}
