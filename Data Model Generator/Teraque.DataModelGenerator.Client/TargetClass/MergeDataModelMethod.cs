namespace Teraque.DataModelGenerator.TargetClass
{

    using System;
    using System.CodeDom;
    using System.Collections;
    using System.Data;
    using System.Threading;
	using System.ServiceModel;
	using System.Windows;
	using System.Windows.Threading;
	using Teraque;

	/// <summary>
	/// Creates a method to handle moving the deleted records from the active data model to the deleted data model.
	/// </summary>
	class MergeDataModelMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method to handle moving the deleted records from the active data model to the deleted data model.
		/// </summary>
		/// <param name="schema">The data model schema.</param>
		public MergeDataModelMethod(DataModelSchema dataModelSchema)
		{

			// These variable are used to create a connection to the server.
			String clientTypeName = String.Format("{0}Client", dataModelSchema.Name);
			String endpointName = String.Format("{0}Endpoint", dataModelSchema.Name);
			String clientVariableName = CommonConversion.ToCamelCase(clientTypeName);

			//		/// <summary>
			//		/// Merge the data from the server into the client's data model.
			//		/// </summary>
			//		static void MergeDataModel()
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Merge the data from the server into the client's data model.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.AddRange(new CodeCustomAttributesForMethods());
			this.Attributes = MemberAttributes.Private | MemberAttributes.Static;
			this.Name = "MergeDataModel";

			//			int batchCounter = DataModel.batchSize;
			this.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference(typeof(Int32)),
					"batchCounter",
					new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "batchSize")));

			//			MergeState mergeState = mergeStateQueue.Peek();
			this.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference("MergeState"),
					"mergeState",
					new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "mergeStateQueue"), "Peek")));

			//			global::System.Data.DataRow destinationRow = null;
			this.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(DataRow)),
					"destinationRow",
					new CodePrimitiveExpression(null)));

			//		StartRowLoop:
			this.Statements.Add(new CodeLabeledStatement("StartRowLoop"));

			//			try
			//			{
			CodeTryCatchFinallyStatement tryItem = new CodeTryCatchFinallyStatement();

			//				if ((mergeState.RowIndex < 0))
			//				{
			CodeConditionStatement ifRowsAllRead = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "rowIndex"),
					CodeBinaryOperatorType.LessThan,
					new CodePrimitiveExpression(0)));

			//					goto EndRowLoop;
			//				}
			ifRowsAllRead.TrueStatements.Add(new CodeGotoStatement("EndRowLoop"));
			tryItem.TryStatements.Add(ifRowsAllRead);

			//				batchCounter = batchCounter - 1;
			tryItem.TryStatements.Add(
					new CodeAssignStatement(
						new CodeVariableReferenceExpression("batchCounter"),
						new CodeBinaryOperatorExpression(
							new CodeVariableReferenceExpression("batchCounter"), CodeBinaryOperatorType.Subtract, new CodePrimitiveExpression(1))));
	
			//				if (batchCounter == 0)
			//				{
			CodeConditionStatement ifBatchCounter = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("batchCounter"),
					CodeBinaryOperatorType.IdentityEquality,
					new CodePrimitiveExpression(0)));

			//					goto ScheduleNext;
			//				}
			ifBatchCounter.TrueStatements.Add(new CodeGotoStatement("ScheduleNext"));
			tryItem.TryStatements.Add(ifBatchCounter);

			//				object[] transactionLogItem = ((object[])(mergeState.TransactionLog[mergeState.RowIndex]));
			tryItem.TryStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Object[])),
					"transactionLogItem",
					new CodeCastExpression(
						new CodeTypeReference(typeof(Object[])),
						new CodeIndexerExpression(
							new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "transactionLog"),
							new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "rowIndex")))));

			//				global::System.Data.DataTable destinationTable = DataModel.Tables[((int)(transactionLogItem[1]))];
			tryItem.TryStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(DataTable)),
					"destinationTable",
					new CodeIndexerExpression(
						new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "Tables"),
						new CodeCastExpression(
							new CodeTypeReference(typeof(Int32)),
							new CodeIndexerExpression(new CodeVariableReferenceExpression("transactionLogItem"), new CodePrimitiveExpression(1))))));

			//				object[] key = new object[destinationTable.PrimaryKey.Length];
			tryItem.TryStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Object[])),
					"key",
					new CodeArrayCreateExpression(
						new CodeGlobalTypeReference(typeof(Object)),
						new CodePropertyReferenceExpression(
							new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("destinationTable"), "PrimaryKey"),
							"Length"))));

			//				for (int keyIndex = 0; (keyIndex < destinationTable.PrimaryKey.Length); keyIndex = (keyIndex + 1))
			//				{
			CodeIterationStatement forKeyIndex0 = new CodeIterationStatement(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Int32)),
					"keyIndex",
					new CodePrimitiveExpression(0)),
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("keyIndex"),
					CodeBinaryOperatorType.LessThan,
					new CodePropertyReferenceExpression(
						new CodePropertyReferenceExpression(
							new CodeVariableReferenceExpression("destinationTable"), "PrimaryKey"),
						"Length")),
					new CodeAssignStatement(
						new CodeVariableReferenceExpression("keyIndex"),
						new CodeBinaryOperatorExpression(
							new CodeVariableReferenceExpression("keyIndex"),
							CodeBinaryOperatorType.Add,
							new CodePrimitiveExpression(1))));

			//					key[keyIndex] = transactionLogItem[(2 + keyIndex)];
			//				}
			forKeyIndex0.Statements.Add(
				new CodeAssignStatement(
					new CodeIndexerExpression(new CodeVariableReferenceExpression("key"), new CodeVariableReferenceExpression("keyIndex")),
					new CodeIndexerExpression(
						new CodeVariableReferenceExpression("transactionLogItem"),
						new CodeBinaryOperatorExpression(
							new CodePrimitiveExpression(2),
							CodeBinaryOperatorType.Add,
							new CodeVariableReferenceExpression("keyIndex")))));
			tryItem.TryStatements.Add(forKeyIndex0);

			//				destinationRow = destinationTable.Rows.Find(key);
			tryItem.TryStatements.Add(
				new CodeAssignStatement(
					new CodeVariableReferenceExpression("destinationRow"),
					new CodeMethodInvokeExpression(
						new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("destinationTable"), "Rows"),
						"Find",
						new CodeVariableReferenceExpression("key"))));

			//				int dataRowState = ((int)(transactionLogItem[0]));
			tryItem.TryStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Int32)),
					"dataRowState",
					new CodeCastExpression(
						new CodeGlobalTypeReference(typeof(Int32)),
						new CodeIndexerExpression(new CodeVariableReferenceExpression("transactionLogItem"), new CodePrimitiveExpression(0)))));

			//				if ((dataRowState == global::Teraque.RecordState.Modified))
			//				{
			CodeConditionStatement ifModified = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("dataRowState"),
					CodeBinaryOperatorType.IdentityEquality,
					new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(RecordState)), "Modified")));


			
			//							if ((destinationRow == null))
			//							{
			CodeConditionStatement ifNotInModel0 = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("destinationRow"),
					CodeBinaryOperatorType.IdentityEquality,
					new CodePrimitiveExpression(null)));

			//								goto KeepRow;
			//							}
			ifNotInModel0.TrueStatements.Add(new CodeGotoStatement("KeepRow"));
			ifModified.TrueStatements.Add(ifNotInModel0);

			//							int offset = (2 + destinationTable.PrimaryKey.Length);
			ifModified.TrueStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Int32)),
					"offset",
					new CodeBinaryOperatorExpression(
						new CodePrimitiveExpression(2),
						CodeBinaryOperatorType.Add,
						new CodePropertyReferenceExpression(
							new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("destinationTable"), "PrimaryKey"),
							"Length"))));

			//							int fields = ((transactionLogItem.Length - offset) 
			//										/ 2);
			ifModified.TrueStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Int32)),
					"fields",
					new CodeBinaryOperatorExpression(
						new CodeBinaryOperatorExpression(
							new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("transactionLogItem"), "Length"),
							CodeBinaryOperatorType.Subtract,
							new CodeVariableReferenceExpression("offset")),
							CodeBinaryOperatorType.Divide,
							new CodePrimitiveExpression(2))));

			//							for (int parentIndex = 0; (parentIndex < destinationTable.ParentRelations.Count); parentIndex = (parentIndex + 1))
			//							{
			CodeIterationStatement parentLoop0 = new CodeIterationStatement(
				new CodeVariableDeclarationStatement(new CodeGlobalTypeReference(typeof(Int32)), "parentIndex", new CodePrimitiveExpression(0)),
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("parentIndex"),
					CodeBinaryOperatorType.LessThan,
					new CodePropertyReferenceExpression(
						new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("destinationTable"), "ParentRelations"),
						"Count")),
				new CodeAssignStatement(
					new CodeVariableReferenceExpression("parentIndex"),
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("parentIndex"),
						CodeBinaryOperatorType.Add,
						new CodePrimitiveExpression(1))));

			//								global::System.Data.DataRelation parentRelation = destinationTable.ParentRelations[parentIndex];
			parentLoop0.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(DataRelation)),
					"parentRelation",
					new CodeIndexerExpression(
						new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("destinationTable"), "ParentRelations"),
						new CodeVariableReferenceExpression("parentIndex"))));

			//								bool isNullKey = true;
			parentLoop0.Statements.Add(
				new CodeVariableDeclarationStatement(new CodeGlobalTypeReference(typeof(Boolean)), "isNullKey", new CodePrimitiveExpression(true)));

			//								object[] parentKey = new object[parentRelation.ChildColumns.Length];
			parentLoop0.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Object[])),
					"parentKey",
					new CodeArrayCreateExpression(
						typeof(Object[]),
						new CodePropertyReferenceExpression(
							new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("parentRelation"), "ChildColumns"),
							"Length"))));

			//								for (int keyIndex = 0; (keyIndex < parentRelation.ChildColumns.Length); keyIndex = (keyIndex + 1))
			//								{
			CodeIterationStatement keyLoop0 = new CodeIterationStatement(
				new CodeVariableDeclarationStatement(new CodeGlobalTypeReference(typeof(Int32)), "keyIndex", new CodePrimitiveExpression(0)),
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("keyIndex"),
					CodeBinaryOperatorType.LessThan,
					new CodePropertyReferenceExpression(
						new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("parentRelation"), "ChildColumns"),
						"Length")),
				new CodeAssignStatement(
					new CodeVariableReferenceExpression("keyIndex"),
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("keyIndex"),
						CodeBinaryOperatorType.Add,
						new CodePrimitiveExpression(1))));

			//									global::System.Data.DataColumn dataColumn = parentRelation.ChildColumns[keyIndex];
			keyLoop0.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(DataColumn)),
					"dataColumn",
					new CodeIndexerExpression(
						new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("parentRelation"), "ChildColumns"),
						new CodeVariableReferenceExpression("keyIndex"))));

			//									parentKey[keyIndex] = dataColumn.DefaultValue;
			keyLoop0.Statements.Add(
				new CodeAssignStatement(
					new CodeIndexerExpression(new CodeVariableReferenceExpression("parentKey"), new CodeVariableReferenceExpression("keyIndex")),
					new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("dataColumn"), "DefaultValue")));

			//									for (int field = 0; (field < fields); field = (field + 1))
			//									{
			CodeIterationStatement fieldLoop0 = new CodeIterationStatement(
				new CodeVariableDeclarationStatement(new CodeGlobalTypeReference(typeof(Int32)), "field", new CodePrimitiveExpression(0)),
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("field"),
					CodeBinaryOperatorType.LessThan,
					new CodeVariableReferenceExpression("fields")),
				new CodeAssignStatement(
					new CodeVariableReferenceExpression("field"),
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("field"),
						CodeBinaryOperatorType.Add,
						new CodePrimitiveExpression(1))));

			//										int fieldIndex = (offset 
			//													+ (field * 2));
			fieldLoop0.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Int32)),
					"fieldIndex",
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("offset"),
						CodeBinaryOperatorType.Add,
						new CodeBinaryOperatorExpression(
							new CodeVariableReferenceExpression("field"),
							CodeBinaryOperatorType.Multiply,
							new CodePrimitiveExpression(2)))));

			//										if ((((int)(transactionLogItem[fieldIndex])) == dataColumn.Ordinal))
			//										{
			CodeConditionStatement ifIsPartOfIndex0 = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeCastExpression(
						new CodeGlobalTypeReference(typeof(Int32)),
						new CodeIndexerExpression(
							new CodeVariableReferenceExpression("transactionLogItem"),
							new CodeVariableReferenceExpression("fieldIndex"))),
					CodeBinaryOperatorType.IdentityEquality,
					new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("dataColumn"), "Ordinal")));

			//											parentKey[keyIndex] = transactionLogItem[(fieldIndex + 1)];
			//										}
			ifIsPartOfIndex0.TrueStatements.Add(
				new CodeAssignStatement(
					new CodeIndexerExpression(new CodeVariableReferenceExpression("parentKey"), new CodeVariableReferenceExpression("keyIndex")),
					new CodeIndexerExpression(
						new CodeVariableReferenceExpression("transactionLogItem"),
						new CodeBinaryOperatorExpression(
							new CodeVariableReferenceExpression("fieldIndex"),
							CodeBinaryOperatorType.Add,
							new CodePrimitiveExpression(1)))));
			fieldLoop0.Statements.Add(ifIsPartOfIndex0);

			//									}
			keyLoop0.Statements.Add(fieldLoop0);

			//									if ((parentKey[keyIndex] != global::System.DBNull.Value))
			//									{
			CodeConditionStatement isNullKey0 = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeIndexerExpression(new CodeVariableReferenceExpression("parentKey"), new CodeVariableReferenceExpression("keyIndex")),
					CodeBinaryOperatorType.IdentityInequality,
					new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(DBNull)), "Value")));

			//										isNullKey = false;
			//									}
			isNullKey0.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("isNullKey"), new CodePrimitiveExpression(false)));
			keyLoop0.Statements.Add(isNullKey0);

			//								}
			parentLoop0.Statements.Add(keyLoop0);

			//								if (((isNullKey == false) 
			//											&& (parentRelation.ParentTable.Rows.Find(parentKey) == null)))
			//								{
			CodeConditionStatement keepRow0 = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("isNullKey"),
						CodeBinaryOperatorType.IdentityEquality,
						new CodePrimitiveExpression(false)),
					CodeBinaryOperatorType.BooleanAnd,
					new CodeBinaryOperatorExpression(
						new CodeMethodInvokeExpression(
							new CodePropertyReferenceExpression(
								new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("parentRelation"), "ParentTable"),
								"Rows"),
							"Find",
							new CodeVariableReferenceExpression("parentKey")),
						CodeBinaryOperatorType.IdentityEquality,
						new CodePrimitiveExpression(null))));

			//									goto KeepRow;
			//								}
			keepRow0.TrueStatements.Add(new CodeGotoStatement("KeepRow"));
			parentLoop0.Statements.Add(keepRow0);

			//							}
			ifModified.TrueStatements.Add(parentLoop0);

			//							destinationRow.BeginEdit();
			ifModified.TrueStatements.Add(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("destinationRow"), "BeginEdit"));

			//							for (int field = 0; (field < fields); field = (field + 1))
			//							{
			CodeIterationStatement forFields2 = new CodeIterationStatement(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Int32)),
					"field",
					new CodePrimitiveExpression(0)),
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("field"),
					CodeBinaryOperatorType.LessThan,
					new CodeVariableReferenceExpression("fields")),
				new CodeAssignStatement(
					new CodeVariableReferenceExpression("field"),
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("field"),
						CodeBinaryOperatorType.Add,
						new CodePrimitiveExpression(1))));

			//								int fieldIndex = (offset 
			//											+ (field * 2));
			forFields2.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Int32)),
					"fieldIndex",
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("offset"),
						CodeBinaryOperatorType.Add,
						new CodeBinaryOperatorExpression(
							new CodeVariableReferenceExpression("field"),
							CodeBinaryOperatorType.Multiply,
							new CodePrimitiveExpression(2)))));

			//								destinationRow[((int)(transactionLogItem[fieldIndex]))] = transactionLogItem[(fieldIndex + 1)];
			//							}
			forFields2.Statements.Add(
				new CodeAssignStatement(
					new CodeIndexerExpression(
						new CodeVariableReferenceExpression("destinationRow"),
						new CodeCastExpression(
							new CodeGlobalTypeReference(typeof(Int32)),
							new CodeIndexerExpression(
								new CodeVariableReferenceExpression("transactionLogItem"),
								new CodeVariableReferenceExpression("fieldIndex")))),
					new CodeIndexerExpression(
						new CodeVariableReferenceExpression("transactionLogItem"),
						new CodeBinaryOperatorExpression(
							new CodeVariableReferenceExpression("fieldIndex"),
							CodeBinaryOperatorType.Add,
							new CodePrimitiveExpression(1)))));
			ifModified.TrueStatements.Add(forFields2);

			//							destinationRow.EndEdit();
			ifModified.TrueStatements.Add(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("destinationRow"), "EndEdit"));

			//							destinationRow.AcceptChanges();
			ifModified.TrueStatements.Add(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("destinationRow"), "AcceptChanges"));

			//							mergeState.isAnythingMerged = true;
			ifModified.TrueStatements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "isAnythingMerged"),
					new CodePrimitiveExpression(true)));

			//						}
			//						else
			//						{
			//							if ((dataRowState == global::Teraque.RecordState.Added))
			//							{
			CodeConditionStatement ifAdded = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("dataRowState"),
					CodeBinaryOperatorType.IdentityEquality,
					new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(RecordState)), "Added")));

			//								int offset = (2 + destinationTable.PrimaryKey.Length);
			ifAdded.TrueStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Int32)),
					"offset",
					new CodeBinaryOperatorExpression(
						new CodePrimitiveExpression(2),
						CodeBinaryOperatorType.Add,
						new CodePropertyReferenceExpression(
							new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("destinationTable"), "PrimaryKey"),
							"Length"))));

			//								int fields = ((transactionLogItem.Length - offset) 
			//											/ 2);
			ifAdded.TrueStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Int32)),
					"fields",
					new CodeBinaryOperatorExpression(
						new CodeBinaryOperatorExpression(
							new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("transactionLogItem"), "Length"),
							CodeBinaryOperatorType.Subtract,
							new CodeVariableReferenceExpression("offset")),
							CodeBinaryOperatorType.Divide,
							new CodePrimitiveExpression(2))));

			//								for (int parentIndex = 0; (parentIndex < destinationTable.ParentRelations.Count); parentIndex = (parentIndex + 1))
			//								{
			CodeIterationStatement parentLoop1 = new CodeIterationStatement(
				new CodeVariableDeclarationStatement(new CodeGlobalTypeReference(typeof(Int32)), "parentIndex", new CodePrimitiveExpression(0)),
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("parentIndex"),
					CodeBinaryOperatorType.LessThan,
					new CodePropertyReferenceExpression(
						new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("destinationTable"), "ParentRelations"),
						"Count")),
				new CodeAssignStatement(
					new CodeVariableReferenceExpression("parentIndex"),
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("parentIndex"),
						CodeBinaryOperatorType.Add,
						new CodePrimitiveExpression(1))));

			//									global::System.Data.DataRelation parentRelation = destinationTable.ParentRelations[parentIndex];
			parentLoop1.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(DataRelation)),
					"parentRelation",
					new CodeIndexerExpression(
						new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("destinationTable"), "ParentRelations"),
						new CodeVariableReferenceExpression("parentIndex"))));

			//									bool isNullKey = true;
			parentLoop1.Statements.Add(
				new CodeVariableDeclarationStatement(new CodeGlobalTypeReference(typeof(Boolean)), "isNullKey", new CodePrimitiveExpression(true)));

			//									object[] parentKey = new object[parentRelation.ChildColumns.Length];
			parentLoop1.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Object[])),
					"parentKey",
					new CodeArrayCreateExpression(
						typeof(Object[]),
						new CodePropertyReferenceExpression(
							new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("parentRelation"), "ChildColumns"),
							"Length"))));

			//									for (int keyIndex = 0; (keyIndex < parentRelation.ChildColumns.Length); keyIndex = (keyIndex + 1))
			//									{
			CodeIterationStatement keyLoop1 = new CodeIterationStatement(
				new CodeVariableDeclarationStatement(new CodeGlobalTypeReference(typeof(Int32)), "keyIndex", new CodePrimitiveExpression(0)),
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("keyIndex"),
					CodeBinaryOperatorType.LessThan,
					new CodePropertyReferenceExpression(
						new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("parentRelation"), "ChildColumns"),
						"Length")),
				new CodeAssignStatement(
					new CodeVariableReferenceExpression("keyIndex"),
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("keyIndex"),
						CodeBinaryOperatorType.Add,
						new CodePrimitiveExpression(1))));

			//										global::System.Data.DataColumn dataColumn = parentRelation.ChildColumns[keyIndex];
			keyLoop1.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(DataColumn)),
					"dataColumn",
					new CodeIndexerExpression(
						new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("parentRelation"), "ChildColumns"),
						new CodeVariableReferenceExpression("keyIndex"))));

			//										parentKey[keyIndex] = dataColumn.DefaultValue;
			keyLoop1.Statements.Add(
				new CodeAssignStatement(
					new CodeIndexerExpression(new CodeVariableReferenceExpression("parentKey"), new CodeVariableReferenceExpression("keyIndex")),
					new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("dataColumn"), "DefaultValue")));

			//										for (int field = 0; (field < fields); field = (field + 1))
			//										{
			CodeIterationStatement fieldLoop1 = new CodeIterationStatement(
				new CodeVariableDeclarationStatement(new CodeGlobalTypeReference(typeof(Int32)), "field", new CodePrimitiveExpression(0)),
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("field"),
					CodeBinaryOperatorType.LessThan,
					new CodeVariableReferenceExpression("fields")),
				new CodeAssignStatement(
					new CodeVariableReferenceExpression("field"),
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("field"),
						CodeBinaryOperatorType.Add,
						new CodePrimitiveExpression(1))));

			//											int fieldIndex = (offset 
			//														+ (field * 2));
			fieldLoop1.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Int32)),
					"fieldIndex",
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("offset"),
						CodeBinaryOperatorType.Add,
						new CodeBinaryOperatorExpression(
							new CodeVariableReferenceExpression("field"),
							CodeBinaryOperatorType.Multiply,
							new CodePrimitiveExpression(2)))));

			//											if ((((int)(transactionLogItem[fieldIndex])) == dataColumn.Ordinal))
			//											{
			CodeConditionStatement ifisPartOfIndex1 = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeCastExpression(
						new CodeGlobalTypeReference(typeof(Int32)),
						new CodeIndexerExpression(
							new CodeVariableReferenceExpression("transactionLogItem"),
							new CodeVariableReferenceExpression("fieldIndex"))),
					CodeBinaryOperatorType.IdentityEquality,
					new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("dataColumn"), "Ordinal")));

			//												parentKey[keyIndex] = transactionLogItem[(fieldIndex + 1)];
			//											}
			ifisPartOfIndex1.TrueStatements.Add(
				new CodeAssignStatement(
					new CodeIndexerExpression(new CodeVariableReferenceExpression("parentKey"), new CodeVariableReferenceExpression("keyIndex")),
					new CodeIndexerExpression(
						new CodeVariableReferenceExpression("transactionLogItem"),
						new CodeBinaryOperatorExpression(
							new CodeVariableReferenceExpression("fieldIndex"),
							CodeBinaryOperatorType.Add,
							new CodePrimitiveExpression(1)))));
			fieldLoop1.Statements.Add(ifisPartOfIndex1);

			//										}
			keyLoop1.Statements.Add(fieldLoop1);

			//										if ((parentKey[keyIndex] != global::System.DBNull.Value))
			//										{
			CodeConditionStatement isNullKey1 = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeIndexerExpression(new CodeVariableReferenceExpression("parentKey"), new CodeVariableReferenceExpression("keyIndex")),
					CodeBinaryOperatorType.IdentityInequality,
					new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(DBNull)), "Value")));

			//											isNullKey = false;
			//										}
			isNullKey1.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("isNullKey"), new CodePrimitiveExpression(false)));
			keyLoop1.Statements.Add(isNullKey1);

			//									}
			parentLoop1.Statements.Add(keyLoop1);

			//									if (((isNullKey == false) 
			//												&& (parentRelation.ParentTable.Rows.Find(parentKey) == null)))
			//									{
			CodeConditionStatement keepRow1 = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("isNullKey"),
						CodeBinaryOperatorType.IdentityEquality,
						new CodePrimitiveExpression(false)),
					CodeBinaryOperatorType.BooleanAnd,
					new CodeBinaryOperatorExpression(
						new CodeMethodInvokeExpression(
							new CodePropertyReferenceExpression(
								new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("parentRelation"), "ParentTable"),
								"Rows"),
							"Find",
							new CodeVariableReferenceExpression("parentKey")),
						CodeBinaryOperatorType.IdentityEquality,
						new CodePrimitiveExpression(null))));

			//										goto KeepRow;
			//									}
			keepRow1.TrueStatements.Add(new CodeGotoStatement("KeepRow"));
			parentLoop1.Statements.Add(keepRow1);

			//								}
			ifAdded.TrueStatements.Add(parentLoop1);

			//								if ((destinationRow == null))
			//								{
			CodeConditionStatement ifNotInModel1 = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("destinationRow"),
					CodeBinaryOperatorType.IdentityEquality,
					new CodePrimitiveExpression(null)));

			//									destinationRow = destinationTable.NewRow();
			ifNotInModel1.TrueStatements.Add(
				new CodeAssignStatement(
					new CodeVariableReferenceExpression("destinationRow"),
					new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("destinationTable"), "NewRow")));

			//									for (int field = 0; (field < fields); field = (field + 1))
			//									{
			CodeIterationStatement forFields0 = new CodeIterationStatement(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Int32)),
					"field",
					new CodePrimitiveExpression(0)),
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("field"),
					CodeBinaryOperatorType.LessThan,
					new CodeVariableReferenceExpression("fields")),
				new CodeAssignStatement(
					new CodeVariableReferenceExpression("field"),
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("field"),
						CodeBinaryOperatorType.Add,
						new CodePrimitiveExpression(1))));

			//										int fieldIndex = (offset 
			//													+ (field * 2));
			forFields0.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Int32)),
					"fieldIndex",
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("offset"),
						CodeBinaryOperatorType.Add,
						new CodeBinaryOperatorExpression(
							new CodeVariableReferenceExpression("field"),
							CodeBinaryOperatorType.Multiply,
							new CodePrimitiveExpression(2)))));

			//										destinationRow[((int)(transactionLogItem[fieldIndex]))] = transactionLogItem[(fieldIndex + 1)];
			//									}
			forFields0.Statements.Add(
				new CodeAssignStatement(
					new CodeIndexerExpression(
						new CodeVariableReferenceExpression("destinationRow"),
						new CodeCastExpression(
							new CodeGlobalTypeReference(typeof(Int32)),
							new CodeIndexerExpression(
								new CodeVariableReferenceExpression("transactionLogItem"),
								new CodeVariableReferenceExpression("fieldIndex")))),
					new CodeIndexerExpression(
						new CodeVariableReferenceExpression("transactionLogItem"),
						new CodeBinaryOperatorExpression(
							new CodeVariableReferenceExpression("fieldIndex"),
							CodeBinaryOperatorType.Add,
							new CodePrimitiveExpression(1)))));
			ifNotInModel1.TrueStatements.Add(forFields0);

			//									destinationTable.Rows.Add(destinationRow);
			//								}
			ifNotInModel1.TrueStatements.Add(
				new CodeMethodInvokeExpression(
					new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("destinationTable"), "Rows"),
					"Add",
					new CodeVariableReferenceExpression("destinationRow")));

			//								else
			//								{
			//									destinationRow.BeginEdit();
			ifNotInModel1.FalseStatements.Add(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("destinationRow"), "BeginEdit"));

			//									for (int field = 0; (field < fields); field = (field + 1))
			//									{
			CodeIterationStatement forFields1 = new CodeIterationStatement(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Int32)),
					"field",
					new CodePrimitiveExpression(0)),
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("field"),
					CodeBinaryOperatorType.LessThan,
					new CodeVariableReferenceExpression("fields")),
				new CodeAssignStatement(
					new CodeVariableReferenceExpression("field"),
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("field"),
						CodeBinaryOperatorType.Add,
						new CodePrimitiveExpression(1))));

			//										int fieldIndex = (offset 
			//													+ (field * 2));
			forFields1.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Int32)),
					"fieldIndex",
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("offset"),
						CodeBinaryOperatorType.Add,
						new CodeBinaryOperatorExpression(
							new CodeVariableReferenceExpression("field"),
							CodeBinaryOperatorType.Multiply,
							new CodePrimitiveExpression(2)))));

			//										destinationRow[((int)(transactionLogItem[fieldIndex]))] = transactionLogItem[(fieldIndex + 1)];
			//									}
			forFields1.Statements.Add(
				new CodeAssignStatement(
					new CodeIndexerExpression(
						new CodeVariableReferenceExpression("destinationRow"),
						new CodeCastExpression(
							new CodeGlobalTypeReference(typeof(Int32)),
							new CodeIndexerExpression(
								new CodeVariableReferenceExpression("transactionLogItem"),
								new CodeVariableReferenceExpression("fieldIndex")))),
					new CodeIndexerExpression(
						new CodeVariableReferenceExpression("transactionLogItem"),
						new CodeBinaryOperatorExpression(
							new CodeVariableReferenceExpression("fieldIndex"),
							CodeBinaryOperatorType.Add,
							new CodePrimitiveExpression(1)))));
			ifNotInModel1.FalseStatements.Add(forFields1);

			//									destinationRow.EndEdit();
			//								}
			ifNotInModel1.FalseStatements.Add(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("destinationRow"), "EndEdit"));
			ifAdded.TrueStatements.Add(ifNotInModel1);

			//								destinationRow.AcceptChanges();
			ifAdded.TrueStatements.Add(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("destinationRow"), "AcceptChanges"));

			//								mergeState.isAnythingMerged = true;
			//							}
			ifAdded.TrueStatements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "isAnythingMerged"),
					new CodePrimitiveExpression(true)));

			//							else
			//							{
			//								if ((destinationRow == null))
			//								{
			CodeConditionStatement ifNotInModel2 = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("destinationRow"),
					CodeBinaryOperatorType.IdentityEquality,
					new CodePrimitiveExpression(null)));

			//									goto KeepRow;
			//								}
			ifNotInModel2.TrueStatements.Add(new CodeGotoStatement("KeepRow"));
			ifAdded.FalseStatements.Add(ifNotInModel0);

			//								destinationRow.Delete();
			ifAdded.FalseStatements.Add(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("destinationRow"), "Delete"));

			//								destinationRow.AcceptChanges();
			ifAdded.FalseStatements.Add(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("destinationRow"), "AcceptChanges"));

			//								isAnythingMerged = true;
			//							}
			ifAdded.FalseStatements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "isAnythingMerged"),
					new CodePrimitiveExpression(true)));
			ifModified.FalseStatements.Add(ifAdded);

			//						}
			tryItem.TryStatements.Add(ifModified);

			//						goto NextRow;
			tryItem.TryStatements.Add(new CodeGotoStatement("NextRow"));

			//					KeepRow:
			tryItem.TryStatements.Add(new CodeLabeledStatement("KeepRow"));

			//						mergeState.unhandledRows.Add(transactionLogItem);
			tryItem.TryStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "unhandledRows"),
					"Add",
					new CodeVariableReferenceExpression("transactionLogItem")));

			//					NextRow:
			tryItem.TryStatements.Add(new CodeLabeledStatement("NextRow"));

			//						mergeState.rowIndex = (mergeState.rowIndex - 1);
			tryItem.TryStatements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "rowIndex"),
					new CodeBinaryOperatorExpression(
						new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "rowIndex"),
						CodeBinaryOperatorType.Subtract,
						new CodePrimitiveExpression(1))));

			//						goto StartRowLoop;
			//					}
			tryItem.TryStatements.Add(new CodeGotoStatement("StartRowLoop"));

			//					catch (global::System.Exception exception)
			//					{
			CodeCatchClause tryItemCatch = new CodeCatchClause("exception", new CodeGlobalTypeReference(typeof(Exception)));

			//						if ((destinationRow != null))
			//						{
			CodeConditionStatement ifDestinationRowNull = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("destinationRow"),
					CodeBinaryOperatorType.IdentityInequality,
					new CodePrimitiveExpression(null)));

			//							destinationRow.RejectChanges();
			//						}
			ifDestinationRowNull.TrueStatements.Add(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("destinationRow"), "RejectChanges"));
			tryItemCatch.Statements.Add(ifDestinationRowNull);

			//						global::Teraque.Log.Error("{0}, {1}", exception.Message, exception.StackTrace);
			//					}
			tryItemCatch.Statements.Add(
				new CodeMethodInvokeExpression(
					new CodeGlobalTypeReferenceExpression(typeof(Log)),
					"Error",
					new CodePrimitiveExpression("{0}, {1}"),
					new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("exception"), "Message"),
					new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("exception"), "StackTrace")));
			tryItem.CatchClauses.Add(tryItemCatch);

			//				}
			this.Statements.Add(tryItem);

			//				EndRowLoop:
			this.Statements.Add(new CodeLabeledStatement("EndRowLoop"));

			//			if (((mergeState.unhandledRows.Count != 0)
			//						&& (mergeState.isAnythingMerged == false)))
			//			{
			CodeConditionStatement ifUnhandled = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeBinaryOperatorExpression(
						new CodePropertyReferenceExpression(
							new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "unhandledRows"), "Count"),
							CodeBinaryOperatorType.IdentityInequality,
							new CodePrimitiveExpression(0)),
						CodeBinaryOperatorType.BooleanAnd,
						new CodeBinaryOperatorExpression(
							new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "isAnythingMerged"),
							CodeBinaryOperatorType.IdentityEquality,
							new CodePrimitiveExpression(false))));

			//				throw new global::System.Data.ConstraintException("Results from the server couldn\'t be merged into the client data model.");
			//			}
			ifUnhandled.TrueStatements.Add(
				new CodeThrowExceptionStatement(
					new CodeObjectCreateExpression(
						new CodeGlobalTypeReference(typeof(ConstraintException)),
						new CodePrimitiveExpression("Results from the server couldn\'t be merged into the client data model."))));
			this.Statements.Add(ifUnhandled);

			//			if ((mergeState.unhandledRows.Count == 0))
			//			{
			CodeConditionStatement ifUnhandledEmpty = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "unhandledRows"), "Count"),
					CodeBinaryOperatorType.IdentityEquality,
					new CodePrimitiveExpression(0)));

			//				DataModel.mergeStateQueue.Dequeue();
			ifUnhandledEmpty.TrueStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "mergeStateQueue"), "Dequeue"));

			//				if ((DataModel.mergeStateQueue.Count == 0))
			//				{
			CodeConditionStatement ifQueueEmpty = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodePropertyReferenceExpression(
						new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "mergeStateQueue"), "Count"),
					CodeBinaryOperatorType.IdentityEquality,
					new CodePrimitiveExpression(0)));

			//					return;
			//				}
			ifQueueEmpty.TrueStatements.Add(new CodeMethodReturnStatement());
			ifUnhandledEmpty.TrueStatements.Add(ifQueueEmpty);

			//			else
			//			{
			//				mergeState.transactionLog = mergeState.unhandledRows.ToArray();
			ifUnhandledEmpty.FalseStatements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "transactionLog"),
					new CodeMethodInvokeExpression(
						new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "unhandledRows"), "ToArray")));

			//				mergeState.unhandledRows = new System.Collections.ArrayList();
			ifUnhandledEmpty.FalseStatements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "unhandledRows"),
					new CodeObjectCreateExpression(new CodeTypeReference(typeof(ArrayList)))));

			//				mergeState.rowIndex = mergeState.transactionLog.Length - 1;
			ifUnhandledEmpty.FalseStatements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "rowIndex"),
					new CodeBinaryOperatorExpression(
						new CodePropertyReferenceExpression(
							new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "transactionLog"), "Length"),
							CodeBinaryOperatorType.Subtract,
							new CodePrimitiveExpression(1))));

			//				mergeState.isAnythingMerged = false;
			ifUnhandledEmpty.FalseStatements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("mergeState"), "isAnythingMerged"),
					new CodePrimitiveExpression(false)));

			//				goto StartRowLoop;
			ifUnhandledEmpty.FalseStatements.Add(new CodeGotoStatement("StartRowLoop"));

			//			}
			this.Statements.Add(ifUnhandledEmpty);

			//		ScheduleNext:
			this.Statements.Add(new CodeLabeledStatement("ScheduleNext"));

			//			if ((DataModel.mergeStateQueue.Count != 0))
			//			{
			//				global::System.Windows.Application.Current.Dispatcher.BeginInvoke(global::System.Windows.Threading.DispatcherPriority.SystemIdle, new System.Action(DataModel.MergeDataModel));
			//			}
			this.Statements.Add(
				new CodeConditionStatement(
					new CodeBinaryOperatorExpression(
						new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "mergeStateQueue"), "Count"),
						CodeBinaryOperatorType.IdentityInequality,
						new CodePrimitiveExpression(0)),
						new CodeStatement[]
						{
							new CodeExpressionStatement(
								new CodeMethodInvokeExpression(
									new CodePropertyReferenceExpression(
										new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(Application)), "Current"),
										"Dispatcher"),
									"BeginInvoke",
									new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(DispatcherPriority)), "SystemIdle"),
									new CodeObjectCreateExpression(
										typeof(Action),
										new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "MergeDataModel"))))}));

		}

	}

}
