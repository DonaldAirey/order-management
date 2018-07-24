namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;
    using System.Collections.Generic;

	/// <summary>
	/// Creates a series of statements that resolve the primary key of a table using external identifiers.
	/// </summary>
	class CodeResolvePrimaryKeyStatements : CodeStatementCollection
	{

		/// <summary>
		/// The vendor specific data set.
		/// </summary>
		CodeExpression targetDataSet;

		/// <summary>
		/// Resolves the primary key.
		/// </summary>
		/// <param name="tableSchema">A description of a table.</param>
		/// <param name="uniqueConstraintParameterItem">A description of a unique key parameter.</param>
		public CodeResolvePrimaryKeyStatements(TableSchema tableSchema, CodeVariableReferenceExpression transactionExpression, UniqueConstraintParameterItem uniqueConstraintParameterItem, CodeExpression targetDataSet)
		{

			// This keeps us from having to pass this into the recursion.
			this.targetDataSet = targetDataSet;

			// All the external identifiers have been resolved.  Now it is time to see if the record exists or if it has to be
			// created.  Finding the record requires a unique index.  If there are more than one unique index, a decision needs to
			// be made as to which one should be used.  The configuration will drive that decision.  If there is only one unique
			// constraint, then the decision doesn't need to be made.
			UniqueConstraintSchema[] uniqueConstraints = tableSchema.UniqueConstraintSchemas;

			CodeVariableReferenceExpression rowExpression = uniqueConstraintParameterItem.CodeVariableReferenceExpression;
			CodeVariableReferenceExpression uniqueKeyExpression = new CodeRandomVariableReferenceExpression();

			// Optimized code is provided when there is only one unique constraint on a table.  This saves the database
			// administrator from having to configure every single table in the data model with a description of the unique index
			// that is to be used when finding a row in that table.  If there is more than one unique constraint on a table, a
			// value will need to be provided in the configuration to tell the loader which one to use.
			if (uniqueConstraints.Length == 1)
			{

				// If there are no foreign keys attached to the selected primary key, then the key value is taken directly from 
				// the input parameters.
				ForeignKeyConstraintSchema foreignKeyConstraintSchema = uniqueConstraints[0].ForeignKey;
				if (foreignKeyConstraintSchema == null)
				{
					this.Add(new CodeVariableDeclarationStatement(
						new CodeGlobalTypeReference(typeof(Object[])), uniqueKeyExpression.VariableName, new CodeArgumentReferenceExpression(uniqueConstraintParameterItem.Name)));
					this.Add(
						new CodeVariableDeclarationStatement(
							new CodeTypeReference(String.Format("{0}Row", tableSchema.Name)),
							uniqueConstraintParameterItem.CodeVariableReferenceExpression.VariableName,
							new CodeMethodInvokeExpression(
								new CodePropertyReferenceExpression(
									new CodeFieldReferenceExpression(targetDataSet, String.Format("table{0}", tableSchema.Name)), uniqueConstraints[0].Name), "Find", uniqueKeyExpression)));
				}
				else
				{

					// When there are foreign keys associated with the selected unique constraint, the foreign keys are resolved
					// using the key passed in as a parameter to this method.  This is a recursive operation that digs through the
					// table hierarchy until a record is found that matches the key.
					CodeVariableReferenceExpression rootRowExpression = new CodeRandomVariableReferenceExpression();
					this.AddRange(
						new CodeResolveForeignKeyExpression(
							rootRowExpression,
							transactionExpression,
							new CodeArgumentReferenceExpression(uniqueConstraintParameterItem.Name), foreignKeyConstraintSchema, targetDataSet));
					List<CodeExpression> keyItems = new List<CodeExpression>();
					for (int columnIndex = 0; columnIndex < tableSchema.PrimaryKey.Columns.Length; columnIndex++)
						keyItems.Add(new CodePropertyReferenceExpression(rootRowExpression, foreignKeyConstraintSchema.RelatedColumns[columnIndex].Name));
					this.Add(
						new CodeVariableDeclarationStatement(
							new CodeGlobalTypeReference(typeof(Object[])),
							uniqueKeyExpression.VariableName,
							new CodeArrayCreateExpression(new CodeGlobalTypeReference(typeof(Object)), keyItems.ToArray())));
					this.Add(
						new CodeVariableDeclarationStatement(
							new CodeTypeReference(String.Format("{0}Row", tableSchema.Name)),
							uniqueConstraintParameterItem.CodeVariableReferenceExpression.VariableName,
							new CodeMethodInvokeExpression(
								new CodePropertyReferenceExpression(
									new CodeFieldReferenceExpression(targetDataSet, String.Format("table{0}", tableSchema.Name)), uniqueConstraints[0].Name), "Find", uniqueKeyExpression)));

				}

			}
			else
			{

				//            // This will find and lock the configuration row that selects the unique constraint for this table.
				//            object[] configurationKey3 = new object[] {
				//                    configurationId,
				//                    "ObjectTree"};
				//            ConfigurationRow configurationRow4 = DataModel.Configuration.ConfigurationKey.Find(configurationKey3);
				//            if ((configurationRow4 == null)) {
				//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Configuration record ({0}) that doesn\'t exist", configurationKey3));
				//            }
				//            configurationRow4.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, DataModel.lockTimeout);
				//            middleTierTransaction.AdoResourceManager.AddLock(configurationRow4);
				//            if ((configurationRow4.RowState == global::System.Data.DataRowState.Detached)) {
				//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Configuration record ({0}) that doesn\'t exist", configurationKey3));
				//            }
				CodeVariableReferenceExpression configurationKeyExpression = new CodeRandomVariableReferenceExpression();
				this.Add(
					new CodeVariableDeclarationStatement(
						new CodeGlobalTypeReference(typeof(Object[])),
						configurationKeyExpression.VariableName,
						new CodeKeyCreateExpression(new CodeArgumentReferenceExpression("configurationId"), new CodePrimitiveExpression(tableSchema.Name))));
				CodeVariableReferenceExpression configurationRowExpression = new CodeRandomVariableReferenceExpression();
				this.Add(
					new CodeVariableDeclarationStatement(
						new CodeTypeReference("ConfigurationRow"),
						configurationRowExpression.VariableName,
						new CodeMethodInvokeExpression(
							new CodePropertyReferenceExpression(
								new CodeFieldReferenceExpression(targetDataSet, "tableConfiguration"),
								"ConfigurationKey"),
							"Find",
							configurationKeyExpression)));
				TableSchema configurationTableSchema = tableSchema.DataModel.Tables["Configuration"];
				this.Add(new CodeCheckRecordExistsStatement(configurationTableSchema, configurationRowExpression, configurationKeyExpression));
				this.Add(new CodeAcquireRecordReaderLockExpression(transactionExpression, configurationRowExpression, tableSchema.DataModel));
				this.Add(new CodeAddLockToTransactionExpression(transactionExpression, configurationRowExpression));
				this.Add(new CodeCheckRecordDetachedStatement(configurationTableSchema, configurationRowExpression, configurationKeyExpression));

				//            // This constructs a key based on the unique constraint specified by the configuration.
				//            object[] objectTreeKey2 = null;
				//            if ((configurationRow4.IndexName == "ObjectTreeKeyExternalId0")) {
				//                objectTreeKey2 = new object[] {
				//                        externalId0};
				//            }
				//            if ((configurationRow4.IndexName == "ObjectTreeKeyParentIdChildId")) {
				//                objectTreeKey2 = new object[] {
				//                        parentId,
				//                        childId};
				//            }
				CodePropertyReferenceExpression keyNameExpression = new CodePropertyReferenceExpression(configurationRowExpression, "IndexName");
				this.Add(new CodeVariableDeclarationStatement(new CodeGlobalTypeReference(typeof(Object[])), uniqueKeyExpression.VariableName, new CodePrimitiveExpression(null)));
				foreach (UniqueConstraintSchema uniqueConstraintSchema in uniqueConstraints)
				{

					// For each unique index on this table, a key is created that matches the key columns of the index selected by 
					// the configuration.
					CodeConditionStatement ifConfiguration = new CodeConditionStatement(new CodeBinaryOperatorExpression(keyNameExpression, CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(uniqueConstraintSchema.Name)));

					// If there are no foreign keys attached to the selected primary key, then the key value is taken directly from 
					// the input parameters.
					ForeignKeyConstraintSchema foreignKeyConstraintSchema = uniqueConstraintSchema.ForeignKey;
					if (foreignKeyConstraintSchema == null)
					{

						//				securityKey1 = new object[] {
						//						objectRow4.ObjectId};
						ifConfiguration.TrueStatements.Add(new CodeAssignStatement(uniqueKeyExpression, new CodeArgumentReferenceExpression(uniqueConstraintParameterItem.Name)));

					}
					else
					{

						// When there are foreign keys associated with the selected unique constraint, the foreign keys are resolved
						// using the key passed in as a parameter to this method.  This is a recursive operation that digs through the
						// table hierarchy until a record is found that matches the key.
						CodeVariableReferenceExpression rootRowExpression = new CodeRandomVariableReferenceExpression();
						ifConfiguration.TrueStatements.AddRange(new CodeResolveForeignKeyExpression(rootRowExpression, transactionExpression, new CodeArgumentReferenceExpression(uniqueConstraintParameterItem.Name), foreignKeyConstraintSchema, targetDataSet));
						List<CodeExpression> keyItems = new List<CodeExpression>();
						for (int columnIndex = 0; columnIndex < tableSchema.PrimaryKey.Columns.Length; columnIndex++)
							keyItems.Add(new CodePropertyReferenceExpression(rootRowExpression, foreignKeyConstraintSchema.RelatedColumns[columnIndex].Name));
						ifConfiguration.TrueStatements.Add(new CodeAssignStatement(uniqueKeyExpression, new CodeArrayCreateExpression(new CodeGlobalTypeReference(typeof(Object)), keyItems.ToArray())));

					}

					//				}
					this.Add(ifConfiguration);

				}

				//            // Use the index and the key specified by the configuration to find the record.
				//            IObjectTreeIndex dataIndex2 = ((IObjectTreeIndex)(DataModel.ObjectTree.Indices[configurationRow4.IndexName]));
				//            ObjectTreeRow objectTreeRow = dataIndex2.Find(objectTreeKey2);
				CodeTypeReference dataIndexType = new CodeTypeReference(String.Format("I{0}Index", tableSchema.Name));
				CodeVariableReferenceExpression dataIndexExpression = new CodeRandomVariableReferenceExpression();
				this.Add(new CodeVariableDeclarationStatement(dataIndexType, dataIndexExpression.VariableName,
					new CodeCastExpression(dataIndexType, new CodeIndexerExpression(new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(targetDataSet, String.Format("table{0}", tableSchema.Name)), "Indices"), keyNameExpression))));
				this.Add(new CodeCheckIndexExistsStatement(dataIndexExpression, tableSchema, keyNameExpression));
				this.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(String.Format("{0}Row", tableSchema.Name)), rowExpression.VariableName, new CodeMethodInvokeExpression(dataIndexExpression, "Find", uniqueKeyExpression)));

			}

			//            if ((engineerRow == null)) {
			//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Engineer record ({0}) that doesn\'t exist", engineerKey5));
			//            }
			//            engineerRow.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, DataModel.lockTimeout);
			//            middleTierTransaction.AdoResourceManager.AddLock(engineerRow);
			//            if ((engineerRow.RowState == global::System.Data.DataRowState.Detached)) {
			//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Engineer record ({0}) that doesn\'t exist", engineerKey5));
			//            }
			this.Add(new CodeCheckRecordExistsStatement(tableSchema, rowExpression, uniqueKeyExpression));
			this.Add(new CodeAcquireRecordReaderLockExpression(transactionExpression, rowExpression, tableSchema.DataModel));
			this.Add(new CodeAddLockToTransactionExpression(transactionExpression, rowExpression));
			this.Add(new CodeCheckRecordDetachedStatement(tableSchema, rowExpression, uniqueKeyExpression));

		}

	}

}
