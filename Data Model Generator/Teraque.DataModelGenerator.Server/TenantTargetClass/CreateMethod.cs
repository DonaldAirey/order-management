namespace Teraque.DataModelGenerator.TenantTargetClass
{

    using System;
    using System.CodeDom;
    using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;
	using System.Threading;

    /// <summary>
	/// Creates the CodeDOM of a method to insert a record using transacted logic.
	/// </summary>
	/// <copyright>Copyright � 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class CreateMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates the CodeDOM for a method to insert a record into a table using transacted logic.
		/// </summary>
		/// <param name="tableSchema">A description of the table.</param>
		public CreateMethod(TableSchema tableSchema)
		{

			// Create a matrix of parameters for this operation.
			CreateParameterMatrix createParameterMatrix = new CreateParameterMatrix(tableSchema);

			//        /// <summary>
			//        /// Creates a Employee record.
			//        /// </summary>
			//        /// <param name="age">The required value for the Age column.</param>
			//        /// <param name="departmentId">The required value for the DepartmentId column.</param>
			//        /// <param name="employeeId">The required value for the EmployeeId column.</param>
			//        /// <param name="raceCode">The optional value for the RaceCode column.</param>
			//        [global::System.ServiceModel.OperationBehaviorAttribute(TransactionScopeRequired=true)]
			//        [Teraque.ClaimsPrincipalPermission(System.Security.Permissions.SecurityAction.Demand, ClaimType=Teraque.ClaimTypes.Create, Resource=Teraque.Resources.Application)]
			//        public void CreateEmployee(int age, int departmentId, int employeeId, object raceCode) {
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Creates a {0} record.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			foreach (KeyValuePair<string, ExternalParameterItem> parameterPair in createParameterMatrix.ExternalParameterItems)
				this.Comments.Add(new CodeCommentStatement(String.Format("<param name=\"{0}\">{1}</param>", parameterPair.Value.Name, parameterPair.Value.Description), true));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.Name = String.Format("Create{0}", tableSchema.Name);
			foreach (KeyValuePair<string, ExternalParameterItem> parameterPair in createParameterMatrix.ExternalParameterItems)
				this.Parameters.Add(parameterPair.Value.CodeParameterDeclarationExpression);

			//			DataModelTransaction o1881 = DataModel.CurrentTransaction;
			CodeVariableReferenceExpression transactionExpression = new CodeRandomVariableReferenceExpression();
			this.Statements.Add(new CodeCreateTransactionStatement(tableSchema.DataModel, transactionExpression));

			//			if ((configurationId == null))
			//			{
			//				configurationId = "";
			//			}
			//			if ((indexName == null))
			//			{
			//				indexName = "";
			//			}
			foreach (ColumnSchema columnSchema in tableSchema.Columns.Values)
				if (columnSchema.IsNullable || columnSchema.DefaultValue != DBNull.Value)
				{
					//			if ((indexName == null))
					//			{
					//				indexName = "";
					//			}
					CodeConditionStatement ifIsNull = new CodeConditionStatement(
						new CodeBinaryOperatorExpression(
							new CodeArgumentReferenceExpression(CommonConversion.ToCamelCase(columnSchema.Name)),
							CodeBinaryOperatorType.IdentityEquality,
							new CodePrimitiveExpression(null)));
					ifIsNull.TrueStatements.Add(
						new CodeAssignStatement(new CodeArgumentReferenceExpression(CommonConversion.ToCamelCase(columnSchema.Name)), CodeConvert.CreateConstantExpression(columnSchema.DefaultValue)));
					this.Statements.Add(ifIsNull);
				}

			//            Teraque.UnitTest.Server.DataModel.DepartmentRow departmentRowByFK_Department_Employee = Teraque.UnitTest.Server.DataModel.Department.FindByDepartmentId(new object[] {
			//                        departmentId});
			//            if ((departmentRowByFK_Department_Employee == null)) {
			//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Department record ({0}) that doesn\'t exist", departmentId));
			//            }
			//            // This record locked for reading for the duration of the transaction.
			//            departmentRowByFK_Department_Employee.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, Teraque.UnitTest.Server.DataModel.lockTimeout);
			//            middleTierTransaction.AdoResourceManager.AddLock(departmentRowByFK_Department_Employee);
			//            // This makes sure the record wasn't deleted in the time between when it was found and the time it was locked.
			//            if ((departmentRowByFK_Department_Employee.RowState == System.Data.DataRowState.Detached)) {
			//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Department record ({0}) that doesn\'t exist", departmentId));
			//            }
			//            // Find the parent Object record if it is required for a foreign key constraint.
			//            Teraque.UnitTest.Server.DataModel.ObjectRow objectRowByFK_Object_Employee = Teraque.UnitTest.Server.DataModel.Object.FindByObjectId(new object[] {
			//                        employeeId});
			//            if ((objectRowByFK_Object_Employee == null)) {
			//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Object record ({0}) that doesn\'t exist", employeeId));
			//            }
			//            // This record locked for reading for the duration of the transaction.
			//            objectRowByFK_Object_Employee.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, Teraque.UnitTest.Server.DataModel.lockTimeout);
			//            middleTierTransaction.AdoResourceManager.AddLock(objectRowByFK_Object_Employee);
			//            // This makes sure the record wasn't deleted in the time between when it was found and the time it was locked.
			//            if ((objectRowByFK_Object_Employee.RowState == System.Data.DataRowState.Detached)) {
			//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Object record ({0}) that doesn\'t exist", employeeId));
			//            }
			//            // Find the parent Race record if it is required for a foreign key constraint.
			//            if ((raceCode != System.DBNull.Value)) {
			//                Teraque.UnitTest.Server.DataModel.RaceRow raceRowByFK_Race_Employee = Teraque.UnitTest.Server.DataModel.Race.FindByRaceCode(new object[] {
			//                            raceCode});
			//                if ((raceRowByFK_Race_Employee == null)) {
			//                    throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Race record ({0}) that doesn\'t exist", raceCode));
			//                }
			//                // This record locked for reading for the duration of the transaction.
			//                raceRowByFK_Race_Employee.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, Teraque.UnitTest.Server.DataModel.lockTimeout);
			//                middleTierTransaction.AdoResourceManager.AddLock(raceRowByFK_Race_Employee);
			//                // This makes sure the record wasn't deleted in the time between when it was found and the time it was locked.
			//                if ((raceRowByFK_Race_Employee.RowState == System.Data.DataRowState.Detached)) {
			//                    throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Race record ({0}) that doesn\'t exist", raceCode));
			//                }
			//            }
			foreach (KeyValuePair<string, RelationSchema> relationPair in tableSchema.ParentRelations)
				if (tableSchema != relationPair.Value.ParentTable)
				{

					// This is the table containing the parent record that is to be locked for the transaction.
					TableSchema parentTable = relationPair.Value.ParentTable;

					// The varible name for the parent row is decorated with the foreign key name thus making it unique.
					CodeVariableReferenceExpression parentRowVariableExpression = new CodeRandomVariableReferenceExpression();
					CodeTypeReference parentRowType = new CodeTypeReference(String.Format("{0}.{1}Row", tableSchema.DataModelSchema.Name, parentTable.Name));

					// This chains all the non-null values of the primary key into an expression that tests if the given key values to the parent table have been
					// provided in the input arguments to this method.  If the provided values are null, and the columns allow nulls, then there is no need to find
					// the parent record.
					CodeExpression lockConditions = null;
					foreach (ColumnSchema columnSchema in relationPair.Value.ChildColumns)
						if (columnSchema.IsNullable)
							lockConditions =
								lockConditions == null ?
								new CodeBinaryOperatorExpression(
									new CodeArgumentReferenceExpression(CommonConversion.ToCamelCase(columnSchema.Name)),
									CodeBinaryOperatorType.IdentityInequality,
									new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(DBNull)), "Value")) :
								new CodeBinaryOperatorExpression(
									new CodeBinaryOperatorExpression(
										new CodeArgumentReferenceExpression(CommonConversion.ToCamelCase(columnSchema.Name)),
										CodeBinaryOperatorType.IdentityInequality,
										new CodePrimitiveExpression(null)),
									CodeBinaryOperatorType.BitwiseAnd,
									lockConditions);

					// The statements to lock the row are added conditionally when the column is nullable.  They are added to the main part of the method when the
					// constraint is required to match up with a parent record.
					CodeStatementCollection codeStatementCollection;
					if (lockConditions == null)
					{
					    codeStatementCollection = this.Statements;
					}
					else
					{
					    CodeConditionStatement ifParentKeyExists = new CodeConditionStatement(lockConditions);
					    this.Statements.Add(ifParentKeyExists);
					    codeStatementCollection = ifParentKeyExists.TrueStatements;
					}

					//			Teraque.UnitTest.Server.DataModel.DepartmentRow departmentRowByFK_Department_Employee = Teraque.UnitTest.Server.DataModel.Department.FindByDepartmentId(new object[] {
					//						departmentId});
					//			if ((departmentRowByFK_Department_Employee == null))
					//			{
					//				throw new Teraque.RecordNotFoundException("Attempt to access a Department record ({0}) that doesn\'t exist", departmentId);
					//			}
					CodeVariableReferenceExpression parentKeyExpression = new CodeRandomVariableReferenceExpression();
					codeStatementCollection.Add(
						new CodeVariableDeclarationStatement(
							new CodeGlobalTypeReference(typeof(Object[])), parentKeyExpression.VariableName, new CodeKeyCreateExpression(relationPair.Value.ChildColumns)));
					if (tableSchema.PrimaryKey == null)
						codeStatementCollection.Add(
							new CodeVariableDeclarationStatement(parentRowType, parentRowVariableExpression.VariableName, new CodeFindByRowExpression(parentTable, parentKeyExpression, new CodeThisReferenceExpression())));
					else
						codeStatementCollection.Add(
							new CodeVariableDeclarationStatement(parentRowType, parentRowVariableExpression.VariableName, new CodeFindByIndexExpression(parentTable, parentKeyExpression, new CodeThisReferenceExpression())));
					codeStatementCollection.Add(new CodeCheckRecordExistsStatement(parentTable, parentRowVariableExpression, parentKeyExpression));

					//			// This record locked for reading for the duration of the transaction.
					//			departmentRowByFK_Department_Employee.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, Teraque.UnitTest.Server.DataModel.lockTimeout);
					//			middleTierTransaction.AdoResourceManager.AddLock(departmentRowByFK_Department_Employee);
					codeStatementCollection.Add(new CodeAcquireRecordReaderLockExpression(transactionExpression, parentRowVariableExpression, parentTable.DataModel));
					codeStatementCollection.Add(new CodeAddLockToTransactionExpression(transactionExpression, parentRowVariableExpression));

					//			// This makes sure the record wasn't deleted in the time between when it was found and the time it was locked.
					//			if ((departmentRowByFK_Department_Employee.RowState == System.Data.DataRowState.Detached))
					//			{
					//				throw new Teraque.RecordNotFoundException("Attempt to access a Department record ({0}) that doesn\'t exist", departmentId);
					//			}
					codeStatementCollection.Add(new CodeCheckRecordDetachedStatement(parentTable, parentRowVariableExpression, parentKeyExpression));

				}

			//			ConfigurationRow q1641;
			//			try
			//			{
			//				l420.dataLock.EnterWriteLock();
			//				i425 = ((AccountRow)(l420.tableAccount.NewRow()));
			//			}
			//			finally
			//			{
			//				l420.dataLock.ExitWriteLock();
			//			}
			CodeVariableReferenceExpression rowVariableExpression = new CodeRandomVariableReferenceExpression();
			this.Statements.Add(new CodeVariableDeclarationStatement(String.Format("{0}.{1}Row", tableSchema.DataModelSchema.Name, tableSchema.Name), rowVariableExpression.VariableName));
			CodeTryCatchFinallyStatement tryCreateRecord = new CodeTryCatchFinallyStatement();
			tryCreateRecord.TryStatements.Add(new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "dataLock"), "EnterWriteLock"));
			tryCreateRecord.TryStatements.Add(
				new CodeAssignStatement(
					rowVariableExpression,
					new CodeCastExpression(new CodeTypeReference(String.Format("{0}.{1}Row", tableSchema.DataModelSchema.Name, tableSchema.Name)),
						new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", tableSchema.Name)), "NewRow"))));
			tryCreateRecord.FinallyStatements.Add(new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "dataLock"), "ExitWriteLock"));
			this.Statements.Add(tryCreateRecord);

			//            // This record is locked for writing for the duration of the transaction.
			//            employeeRow.AcquireWriterLock(middleTierTransaction.AdoResourceManager.Guid, Teraque.UnitTest.Server.DataModel.lockTimeout);
			//            middleTierTransaction.AdoResourceManager.AddLock(employeeRow);
			this.Statements.Add(new CodeAcquireRecordWriterLockExpression(transactionExpression, rowVariableExpression, tableSchema));
			this.Statements.Add(new CodeAddLockToTransactionExpression(transactionExpression, rowVariableExpression));

			//            // Create the Employee record in the ADO data model.
			//            middleTierTransaction.AdoResourceManager.AddRecord(employeeRow);
			this.Statements.Add(new CodeAddRecordToTransactionExpression(transactionExpression, rowVariableExpression));

			//            try {
			//                // Lock the owner table and any parent tables while the record is populated.  Note that table locks are always held
			//                // momentarily.
			//                Teraque.UnitTest.Server.DataModel.ReaderWriterLock.EnterReadLock();
			//                employeeRow.BeginEdit();
			//                employeeRow[Teraque.UnitTest.Server.DataModel.Employee.AgeColumn] = age;
			//                employeeRow[Teraque.UnitTest.Server.DataModel.Employee.DepartmentIdColumn] = departmentId;
			//                employeeRow[Teraque.UnitTest.Server.DataModel.Employee.EmployeeIdColumn] = employeeId;
			//                employeeRow[Teraque.UnitTest.Server.DataModel.Employee.RaceCodeColumn] = raceCode;
			//                employeeRow[Teraque.UnitTest.Server.DataModel.Employee.RowVersionColumn] = System.Threading.Interlocked.Increment(ref Teraque.UnitTest.Server.DataModel.masterRowVersion);
			//                Teraque.UnitTest.Server.DataModel.Employee.Rows.Add(employeeRow);
			//            }
			//            finally {
			//                // The record create is finished and the momentary table locks are no longer needed.
			//                employeeRow.EndEdit();
			//                Teraque.UnitTest.Server.DataModel.ReaderWriterLock.ExitWriteLock();
			//            }
			CodeTryCatchFinallyStatement tryFinallyStatement = new CodeTryCatchFinallyStatement();
			tryFinallyStatement.TryStatements.Add(new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "dataLock"), "EnterWriteLock"));
			tryFinallyStatement.TryStatements.Add(new CodeMethodInvokeExpression(rowVariableExpression, "BeginEdit"));
			foreach (ColumnSchema columnSchema in tableSchema.Columns.Values)
				if (!columnSchema.IsAutoIncrement)
				{
					CodeExpression sourceExpression;
					if (columnSchema.IsRowVersion)
					{
						sourceExpression = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "IncrementRowVersion");
					}
					else
					{
						sourceExpression = new CodeArgumentReferenceExpression(CommonConversion.ToCamelCase(columnSchema.Name));
					}
					tryFinallyStatement.TryStatements.Add(new CodeAssignStatement(new CodeIndexerExpression(rowVariableExpression, new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", tableSchema.Name)), String.Format("{0}Column", columnSchema.Name))), sourceExpression));
				}
			tryFinallyStatement.TryStatements.Add(
				new CodeMethodInvokeExpression(
					new CodePropertyReferenceExpression(
						new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", tableSchema.Name)), "Rows"), "Add", rowVariableExpression));
			tryFinallyStatement.FinallyStatements.Add(new CodeMethodInvokeExpression(rowVariableExpression, "EndEdit"));
			tryFinallyStatement.FinallyStatements.Add(new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "dataLock"), "ExitWriteLock"));
			this.Statements.Add(tryFinallyStatement);

			//            // Add the Employee record to the SQL data model.
			//            System.Data.SqlClient.SqlCommand sqlCommand = new global::System.Data.SqlClient.SqlCommand("insert \"Employee\" (\"Age\",\"DepartmentId\",\"EmployeeId\",\"RaceCode\",\"RowVersion\",\"Row" +
			//                    "Version\") values (@age,@departmentId,@employeeId,@raceCode,@rowVersion,@rowVersi" +
			//                    "on)", middleTierTransaction.SqlConnection);
			//            sqlCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@age", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, 0, 0, null, System.Data.DataRowVersion.Current, age));
			//            sqlCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@departmentId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, 0, 0, null, System.Data.DataRowVersion.Current, departmentId));
			//            sqlCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@employeeId", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, 0, 0, null, System.Data.DataRowVersion.Current, employeeId));
			//            sqlCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@raceCode", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, false, 0, 0, null, System.Data.DataRowVersion.Current, raceCode));
			//            sqlCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@rowVersion", System.Data.SqlDbType.BigInt, 0, System.Data.ParameterDirection.Input, false, 0, 0, null, System.Data.DataRowVersion.Current, employeeRow[Teraque.UnitTest.Server.DataModel.Employee.RowVersionColumn]));
			//            sqlCommand.ExecuteNonQuery();
			if (tableSchema.IsPersistent)
			{
				CodeVariableReferenceExpression sqlCommandExpression = new CodeRandomVariableReferenceExpression();
				string columnList = string.Empty;
				string variableList = string.Empty;
				int columnIndex = 0;
				foreach (ColumnSchema columnSchema in tableSchema.Columns.Values)
					if (columnSchema.IsPersistent)
					{
						columnList += String.Format(columnIndex < tableSchema.Columns.Count - 1 ? "\"{0}\"," : "\"{0}\"", columnSchema.Name);
						variableList += String.Format(columnIndex < tableSchema.Columns.Count - 1 ? "@{0}," : "@{0}", CommonConversion.ToCamelCase(columnSchema.Name));
						columnIndex++;
					}
				string insertCommandText = String.Format("insert \"{0}\" ({1}) values ({2})", tableSchema.Name, columnList, variableList);
				this.Statements.Add(new CodeVariableDeclarationStatement(new CodeGlobalTypeReference(typeof(SqlCommand)), sqlCommandExpression.VariableName, new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(SqlCommand)), new CodePrimitiveExpression(insertCommandText), new CodePropertyReferenceExpression(transactionExpression, "SqlConnection"))));
				foreach (ColumnSchema columnSchema in tableSchema.Columns.Values)
					if (columnSchema.IsPersistent)
					{
						string variableName = CommonConversion.ToCamelCase(columnSchema.Name);
						if (columnSchema.IsAutoIncrement)
						{
							CodeExpression codeExpression = new CodeIndexerExpression(rowVariableExpression, new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", tableSchema.Name)), String.Format("{0}Column", columnSchema.Name)));
							this.Statements.Add(new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(sqlCommandExpression, "Parameters"), "Add", new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(SqlParameter)), new CodePrimitiveExpression(String.Format("@{0}", variableName)), TypeConverter.Convert(columnSchema.DataType), new CodePrimitiveExpression(0), new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(ParameterDirection)), "Input"), new CodePrimitiveExpression(false), new CodePrimitiveExpression(0), new CodePrimitiveExpression(0), new CodePrimitiveExpression(null), new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(DataRowVersion)), "Current"), codeExpression)));
						}
						else
						{
							CodeExpression sourceExpression = columnSchema.IsRowVersion ?
								(CodeExpression)new CodeIndexerExpression(rowVariableExpression, new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", tableSchema.Name)), "RowVersionColumn")) :
								(CodeExpression)new CodeArgumentReferenceExpression(variableName);
							this.Statements.Add(new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(sqlCommandExpression, "Parameters"), "Add", new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(SqlParameter)), new CodePrimitiveExpression(String.Format("@{0}", variableName)), TypeConverter.Convert(columnSchema.DataType), new CodePrimitiveExpression(0), new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(ParameterDirection)), "Input"), new CodePrimitiveExpression(false), new CodePrimitiveExpression(0), new CodePrimitiveExpression(0), new CodePrimitiveExpression(null), new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(DataRowVersion)), "Current"), sourceExpression)));
						}
					}
				this.Statements.Add(new CodeMethodInvokeExpression(transactionExpression, "Execute", sqlCommandExpression));

			}

			//			DataModel.DestinationOrder.OnRowValidate(new DestinationOrderRowChangeEventArgs(pe9564f2717374e96a76d5222e2258784, System.Data.DataRowAction.Add));
			this.Statements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", tableSchema.Name)),
					"OnRowValidate",
					new CodeObjectCreateExpression(
						String.Format("{0}.{1}RowChangeEventArgs", tableSchema.DataModelSchema.Name, tableSchema.Name),
						rowVariableExpression,
						new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(DataRowAction)), "Add"))));


			// Cast the Auto-Increment values back to their native types when returning from this method.
			foreach (KeyValuePair<string, ColumnSchema> columnPair in tableSchema.Columns)
			{
				ColumnSchema columnSchema = columnPair.Value;
				if (columnSchema.IsAutoIncrement)
				{
					this.Statements.Add(new CodeAssignStatement(new CodeArgumentReferenceExpression(CommonConversion.ToCamelCase(columnSchema.Name)),
						new CodeCastExpression(new CodeGlobalTypeReference(columnSchema.DataType), new CodeIndexerExpression(rowVariableExpression, new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", tableSchema.Name)), String.Format("{0}Column", columnSchema.Name))))));
				}
			}

			//        }

		}

	}

}
