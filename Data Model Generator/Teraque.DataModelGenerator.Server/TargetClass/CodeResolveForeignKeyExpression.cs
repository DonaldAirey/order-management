namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;

	/// <summary>
	/// Create a series of statements that resolves an external reference.
	/// </summary>
	class CodeResolveForeignKeyExpression : CodeStatementCollection
	{

		/// <summary>
		/// The transaction variable expression.
		/// </summary>
		CodeVariableReferenceExpression transactionExpression;

		/// <summary>
		/// The vendor specific DataSet variable expression.
		/// </summary>
		CodeExpression targetDataSet;

		/// <summary>
		/// Create a series of statements that resolves an external reference.
		/// </summary>
		/// <param name="targetVariable">The variable that recieves the resolved row.</param>
		/// <param name="transactionExpression">The manager for the middle tier transaction.</param>
		/// <param name="foreignKeyConstraintSchema">The foreign constraint that is to be resolved.</param>
		public CodeResolveForeignKeyExpression(CodeVariableReferenceExpression targetVariable, CodeVariableReferenceExpression transactionExpression, CodeExpression rootKeyExpression, ForeignKeyConstraintSchema foreignKeyConstraintSchema, CodeExpression targetDataSet)
		{

			// The middle tier context is used by the intermediate expressions to lock rows.
			this.transactionExpression = transactionExpression;

			// This is used to resolve the target data model.
			this.targetDataSet = targetDataSet;

			// This will recurse into the relations and build the statements needed to resolve the external row reference.
			RecurseIntoRelation(targetVariable, foreignKeyConstraintSchema, rootKeyExpression, foreignKeyConstraintSchema);

		}
	
		/// <summary>
		/// Recurse into a hierarchy of relations until a table is found that can resolve external identifiers.
		/// </summary>
		/// <param name="foreignKeyConstraintParameterItem">The original foreign key parameter to be resolved.</param>
		/// <param name="foreignKeyConstraintSchema">The current level of the table hierarchy.</param>
		/// <returns>An expression representing the unique row identified by the foreign key.</returns>
		public void RecurseIntoRelation(CodeVariableReferenceExpression rowExpression, ForeignKeyConstraintSchema rootForeignKeyConstraintSchema, CodeExpression rootKeyExpression, ForeignKeyConstraintSchema currentForeignKeyConstraintSchema)
		{

			// Each time through the recursion, the parent table will be examined to see if there is another ancestor which can be used to resolve the external
			// interfaces.  If one is found, the recursion continues into the ancestor, if not, we've found the ancestor that can resolve the external identifiers.
			TableSchema parentTableSchema = currentForeignKeyConstraintSchema.RelatedTable;
			ForeignKeyConstraintSchema parentForeignKeyConstraintSchema = parentTableSchema.ParentConstraint;

			// If this is the end of the line for the foreign relations that can be use to uniquely identify the original row, then stop recursing and attempt to
			// find a unique constraint.  The recursion then unwinds and the row at the original level of the recursion has values that can be used to identify the
			// target record.
			if (parentForeignKeyConstraintSchema == null)
			{

				//            object[] configurationKey0 = new object[] {
				//                    configurationId,
				//                    "FK_Object_Employee"};
				//            ConfigurationRow configurationRow0 = DataModel.Configuration.ConfigurationKey.Find(configurationKey0);
				//            if ((configurationRow0 == null)) {
				//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Configuration record ({0}) that doesn\'t exist", configurationKey0));
				//            }
				//            configurationRow0.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, DataModel.lockTimeout);
				//            middleTierTransaction.AdoResourceManager.AddLock(configurationRow0);
				//            if ((configurationRow0.RowState == global::System.Data.DataRowState.Detached)) {
				//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Configuration record ({0}) that doesn\'t exist", configurationKey0));
				//            }
				CodeVariableReferenceExpression configurationKeyExpression = new CodeRandomVariableReferenceExpression();
				this.Add(new CodeVariableDeclarationStatement(new CodeGlobalTypeReference(typeof(Object[])), configurationKeyExpression.VariableName,
					new CodeKeyCreateExpression(new CodeArgumentReferenceExpression("configurationId"), new CodePrimitiveExpression(rootForeignKeyConstraintSchema.Name))));
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
				TableSchema configurationTableSchema = parentTableSchema.DataModel.Tables["Configuration"];
				this.Add(new CodeCheckRecordExistsStatement(configurationTableSchema, configurationRowExpression, configurationKeyExpression));
				this.Add(new CodeAcquireRecordReaderLockExpression(transactionExpression, configurationRowExpression, parentTableSchema.DataModel));
				this.Add(new CodeAddLockToTransactionExpression(transactionExpression, configurationRowExpression));
				this.Add(new CodeCheckRecordDetachedStatement(configurationTableSchema, configurationRowExpression, configurationKeyExpression));

				//            IObjectIndex dataIndex0 = ((IObjectIndex)(DataModel.Object.Indices[configurationRow0.IndexName]));
				//            ObjectRow objectRow1 = dataIndex0.Find(engineerId);
				//            if ((objectRow1 == null)) {
				//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Object record ({0}) that doesn\'t exist", engineerId));
				//            }
				CodeTypeReference indexType = new CodeTypeReference(String.Format("I{0}Index", parentTableSchema.Name));
				CodeVariableReferenceExpression indexExpression = new CodeRandomVariableReferenceExpression();
				CodeExpression indexNameExpression = new CodePropertyReferenceExpression(configurationRowExpression, "IndexName");
				this.Add(
					new CodeVariableDeclarationStatement(
						indexType,
						indexExpression.VariableName,
						new CodeCastExpression(
							indexType,
							new CodeIndexerExpression(
								new CodePropertyReferenceExpression(
									new CodePropertyReferenceExpression(targetDataSet, String.Format("table{0}", parentTableSchema.Name)),
									"Indices"),
								indexNameExpression))));
				this.Add(new CodeCheckIndexExistsStatement(indexExpression, parentTableSchema, indexNameExpression));
				CodeTypeReference rowTypeReference = new CodeTypeReference(String.Format("{0}Row", parentTableSchema.Name));
				this.Add(new CodeVariableDeclarationStatement(rowTypeReference, rowExpression.VariableName, new CodeMethodInvokeExpression(indexExpression, "Find", rootKeyExpression)));
				this.Add(new CodeCheckRecordExistsStatement(parentTableSchema, rowExpression, rootKeyExpression));

				//            objectRow1.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, DataModel.lockTimeout);
				//            middleTierTransaction.AdoResourceManager.AddLock(objectRow1);
				//            if ((objectRow1.RowState == global::System.Data.DataRowState.Detached)) {
				//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Object record ({0}) that doesn\'t exist", engineerId));
				//            }
				this.Add(new CodeAcquireRecordReaderLockExpression(this.transactionExpression, rowExpression, parentTableSchema.DataModel));
				this.Add(new CodeAddLockToTransactionExpression(this.transactionExpression, rowExpression));
				this.Add(new CodeCheckRecordDetachedStatement(parentTableSchema, rowExpression, rootKeyExpression));

			}
			else
			{

				// This will recurse into the hierarchy and emit code that will find and lock each row in the line of ancestors to
				// the current table.  When there are no more ancestors to be found, code will be generated to select a record
				// based on a unique constraint.  The generated code then unwinds the relationship choosing one distinct descendant
				// after another until all the foreign relationships to the starting table have been resolved.
				CodeVariableReferenceExpression parentRow = new CodeRandomVariableReferenceExpression();
				RecurseIntoRelation(parentRow, rootForeignKeyConstraintSchema, rootKeyExpression, parentForeignKeyConstraintSchema);

				//            // Employee level of the engineerId foreign key search.
				//            object[] employeeKey1 = new object[] {
				//                    objectRow1.ObjectId};
				//            EmployeeRow employeeRow2 = DataModel.Employee.EmployeeKey.Find(employeeKey1);
				//            if ((employeeRow2 == null)) {
				//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Employee record ({0}) that doesn\'t exist", employeeKey1));
				//            }
				CodeVariableReferenceExpression keyExpression = new CodeRandomVariableReferenceExpression();
				this.Add(new CodeVariableDeclarationStatement(new CodeGlobalTypeReference(typeof(Object[])), keyExpression.VariableName,
					new CodeKeyCreateExpression(parentRow, parentForeignKeyConstraintSchema.RelatedColumns)));
				CodeTypeReference rowType = new CodeTypeReference(String.Format("{0}Row", parentTableSchema.Name));
				UniqueConstraintSchema uniqueConstraintSchema = parentTableSchema.GetUniqueConstraint(parentForeignKeyConstraintSchema.Columns);
				this.Add(
					new CodeVariableDeclarationStatement(
						rowType,
						rowExpression.VariableName,
						new CodeMethodInvokeExpression(
							new CodePropertyReferenceExpression(
								new CodeFieldReferenceExpression(
									this.targetDataSet, String.Format("table{0}", parentTableSchema.Name)),
									uniqueConstraintSchema.Name),
							"Find",
							keyExpression)));
				this.Add(new CodeCheckRecordExistsStatement(parentTableSchema, rowExpression, keyExpression));

				//            employeeRow2.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, DataModel.lockTimeout);
				//            middleTierTransaction.AdoResourceManager.AddLock(employeeRow2);
				//            if ((employeeRow2.RowState == global::System.Data.DataRowState.Detached)) {
				//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Employee record ({0}) that doesn\'t exist", employeeKey1));
				//            }
				this.Add(new CodeAcquireRecordReaderLockExpression(this.transactionExpression, rowExpression, parentTableSchema.DataModel));
				this.Add(new CodeAddLockToTransactionExpression(this.transactionExpression, rowExpression));
				this.Add(new CodeCheckRecordDetachedStatement(parentTableSchema, rowExpression, keyExpression));

			}

		}

	}

}
