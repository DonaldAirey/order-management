namespace Teraque.DataModelGenerator.TenantTargetClass
{

    using System;
    using System.CodeDom;
    using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;
	using System.Threading;

	/// <summary>
	/// Creates the CodeDOM of a method to update a record using transacted logic.
	/// </summary>
	/// <copyright>Copyright � 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class UpdateMethod	: CodeMemberMethod
	{

		/// <summary>
		/// Creates the CodeDOM for a method to update a record in a table using transacted logic.
		/// </summary>
		/// <param name="tableSchema">A description of the table.</param>
		public UpdateMethod(TableSchema tableSchema)
		{

			// Create a matrix of parameters for this operation.
			UpdateParameterMatrix updateParameterMatrix = new UpdateParameterMatrix(tableSchema);

			// This is the key used to identify the record to be deleted.
			CodeArgumentReferenceExpression primaryKeyExpression = null;
			foreach (KeyValuePair<string, ExternalParameterItem> externalParamterPair in updateParameterMatrix.ExternalParameterItems)
				if (externalParamterPair.Value is UniqueConstraintParameterItem)
					primaryKeyExpression = new CodeArgumentReferenceExpression(externalParamterPair.Value.Name);

			//        /// <summary>
			//        /// Updates a Employee record.
			//        /// </summary>
			//        /// <param name="age">The optional value for the Age column.</param>
			//        /// <param name="departmentId">The optional value for the DepartmentId column.</param>
			//        /// <param name="employeeId">The required value for the EmployeeId column.</param>
			//        /// <param name="raceCode">The optional value for the RaceCode column.</param>
			//        /// <param name="rowVersion">Used for Optimistic Concurrency Checking.</param>
			//        [global::System.ServiceModel.OperationBehaviorAttribute(TransactionScopeRequired=true)]
			//        [Teraque.ClaimsPrincipalPermission(global::System.Security.Permissions.SecurityAction.Demand, ClaimType=global::Teraque.ClaimTypes.Update, Resource=global::Teraque.Resources.Application)]
			//        public void UpdateEmployee(object age, object departmentId, int employeeId, object raceCode, ref long rowVersion) {
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Updates a {0} record.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			foreach (KeyValuePair<string, ExternalParameterItem> parameterPair in updateParameterMatrix.ExternalParameterItems)
				this.Comments.Add(new CodeCommentStatement(String.Format("<param name=\"{0}\">{1}</param>", parameterPair.Value.Name, parameterPair.Value.Description), true));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.Name = String.Format("Update{0}", tableSchema.Name);
			foreach (KeyValuePair<string, ExternalParameterItem> parameterPair in updateParameterMatrix.ExternalParameterItems)
				this.Parameters.Add(parameterPair.Value.CodeParameterDeclarationExpression);

			// There is no way to find the existing record without a primary unique key.
			if (tableSchema.PrimaryKey == null)
				return;

			//			DataModelTransaction o1881 = DataModel.CurrentTransaction;
			CodeVariableReferenceExpression transactionExpression = new CodeRandomVariableReferenceExpression();
			this.Statements.Add(new CodeCreateTransactionStatement(tableSchema.DataModel, transactionExpression));

			//            // This is the record that will be updated
			//            global::System.Object[] employeeKey = new global::System.Object(new object[] {
			//                        employeeId});
			//            Teraque.UnitTest.Server.DataModel.EmployeeRow employeeRow = Teraque.UnitTest.Server.DataModel.Employee.FindByEmployeeId(employeeKey);
			//            if ((employeeRow == null)) {
			//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Employee record ({0}) that doesn\'t exist", employeeKey));
			//            }
			CodeVariableReferenceExpression rowVariableExpression = new CodeRandomVariableReferenceExpression();
			this.Statements.Add(new CodeVariableDeclarationStatement(
				new CodeTypeReference(String.Format("{0}.{1}Row", tableSchema.DataModelSchema.Name, tableSchema.Name)),
				rowVariableExpression.VariableName,
				new CodeFindByIndexExpression(tableSchema, primaryKeyExpression, new CodeThisReferenceExpression())));
			this.Statements.Add(new CodeCheckRecordExistsStatement(tableSchema, rowVariableExpression, primaryKeyExpression));
			this.Statements.Add(new CodeAcquireRecordWriterLockExpression(transactionExpression, rowVariableExpression, tableSchema));
			this.Statements.Add(new CodeAddLockToTransactionExpression(transactionExpression, rowVariableExpression));

			//            // This makes sure the record wasn't deleted between the time it was found and the time it was locked.
			//            if ((employeeRow.RowState == global::System.Data.DataRowState.Detached)) {
			//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Employee record ({0}) that doesn\'t exist", employeeId));
			//            }
			this.Statements.Add(new CodeCheckRecordDetachedStatement(tableSchema, rowVariableExpression, primaryKeyExpression));

			//            // The Optimistic Concurrency check allows only one client to update a record at a time.
			//            if ((employeeRow.RowVersion != rowVersion)) {
			//                throw new global::System.ServiceModel.FaultException<OptimisticConcurrencyFault>(new global::Teraque.OptimisticConcurrencyFault("The Employee record (Employee) is busy.  Please try again later.", employeeId));
			//            }
			this.Statements.Add(new CodeCheckConcurrencyStatement(tableSchema, rowVariableExpression, primaryKeyExpression));

			//            // This will provide the defaults elements of the Employee table that haven't changed.
			//            if ((age == null)) {
			//                age = employeeRow[Teraque.UnitTest.Server.DataModel.Employee.AgeColumn];
			//            }
			//            if ((departmentId == null)) {
			//                departmentId = employeeRow[Teraque.UnitTest.Server.DataModel.Employee.DepartmentIdColumn];
			//            }
			//            if ((raceCode == null)) {
			//                raceCode = employeeRow[Teraque.UnitTest.Server.DataModel.Employee.RaceCodeColumn];
			//            }
			bool isDefaultCommentEmitted = false;
			foreach (ColumnSchema columnSchema in tableSchema.Columns.Values)
				if (!columnSchema.IsRowVersion)
				{
					if (!isDefaultCommentEmitted)
					{
						isDefaultCommentEmitted = true;
					}
					this.Statements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(new CodeArgumentReferenceExpression(CommonConversion.ToCamelCase(columnSchema.Name)), CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(null)),
						new CodeAssignStatement(new CodeArgumentReferenceExpression(CommonConversion.ToCamelCase(columnSchema.Name)), new CodeIndexerExpression(rowVariableExpression, new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", tableSchema.Name)), String.Format("{0}Column", columnSchema.Name))))));
				}

			//            // The current parent Department record is locked for reading for the duration of the transaction.
			//            employeeRow.DepartmentRow.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, Teraque.UnitTest.Server.DataModel.lockTimeout);
			//            middleTierTransaction.AdoResourceManager.AddLock(employeeRow.DepartmentRow);
			//            // This makes sure the record wasn't deleted in the time between when it was found and the time it was locked.
			//            if ((employeeRow.DepartmentRow.RowState == global::System.Data.DataRowState.Detached)) {
			//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Department record ({0}) that doesn\'t exist", departmentId));
			//            }
			//            // The current parent Object record is locked for reading for the duration of the transaction.
			//            employeeRow.ObjectRow.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, Teraque.UnitTest.Server.DataModel.lockTimeout);
			//            middleTierTransaction.AdoResourceManager.AddLock(employeeRow.ObjectRow);
			//            // This makes sure the record wasn't deleted in the time between when it was found and the time it was locked.
			//            if ((employeeRow.ObjectRow.RowState == global::System.Data.DataRowState.Detached)) {
			//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Object record ({0}) that doesn\'t exist", employeeId));
			//            }
			//            // The current parent Race record is locked for reading for the duration of the transaction.
			//            if ((employeeRow.RaceRow != null)) {
			//                employeeRow.RaceRow.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, Teraque.UnitTest.Server.DataModel.lockTimeout);
			//                middleTierTransaction.AdoResourceManager.AddLock(employeeRow.RaceRow);
			//                // This makes sure the record wasn't deleted in the time between when it was found and the time it was locked.
			//                if ((employeeRow.RaceRow.RowState == global::System.Data.DataRowState.Detached)) {
			//                    throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Race record ({0}) that doesn\'t exist", raceCode));
			//                }
			//            }
			foreach (KeyValuePair<string, RelationSchema> relationPair in tableSchema.ParentRelations)
				if (tableSchema != relationPair.Value.ParentTable)
				{

					// This is the table containing the parent record that is to be locked for the transaction.
					TableSchema parentTable = relationPair.Value.ParentTable;

					// The varible name for the parent row is decorated with the foreign key name thus making it unique.
					string parentRowName = String.Format("{0}RowBy{1}", CommonConversion.ToCamelCase(parentTable.Name), relationPair.Value.Name);
					string parentRowTypeName = String.Format("{0}.{1}Row", tableSchema.DataModelSchema.Name, parentTable.Name);
					CodePropertyReferenceExpression parentRowExpression = relationPair.Value.IsDistinctPathToParent ?
						new CodePropertyReferenceExpression(rowVariableExpression, String.Format("{0}Row", parentTable.Name)) :
						new CodePropertyReferenceExpression(rowVariableExpression, String.Format("{0}RowBy{1}", parentTable.Name, relationPair.Value.Name));

					//            // The current parent Department record is locked for reading for the duration of the transaction.
					//            employeeRow.DepartmentRow.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, Teraque.UnitTest.Server.DataModel.lockTimeout);
					//            middleTierTransaction.AdoResourceManager.AddLock(employeeRow.DepartmentRow);
					CodeStatementCollection codeStatementCollection;
					if (relationPair.Value.ChildKeyConstraint.IsNullable)
					{
						CodeConditionStatement ifParentKeyExists = new CodeConditionStatement(new CodeBinaryOperatorExpression(parentRowExpression, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null)));
						codeStatementCollection = ifParentKeyExists.TrueStatements;
						this.Statements.Add(ifParentKeyExists);
					}
					else
					{
						codeStatementCollection = this.Statements;
					}
					codeStatementCollection.Add(new CodeAcquireRecordReaderLockExpression(transactionExpression, parentRowExpression, parentTable.DataModel));
					codeStatementCollection.Add(new CodeAddLockToTransactionExpression(transactionExpression, parentRowExpression));

					//            // This makes sure the record wasn't deleted in the time between when it was found and the time it was locked.
					//            if ((employeeRow.DepartmentRow.RowState == global::System.Data.DataRowState.Detached)) {
					//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Department record ({0}) that doesn\'t exist", departmentId));
					//            }
					codeStatementCollection.Add(new CodeCheckRecordDetachedStatement(parentTable, parentRowExpression, new CodeKeyCreateExpression(relationPair.Value.ChildColumns)));

				}

			//            // Find the new proposed parent Department record if it is required for a foreign key constraint.
			//            Teraque.UnitTest.Server.DataModel.DepartmentRow departmentRowByFK_Department_Employee = Teraque.UnitTest.Server.DataModel.Department.FindByDepartmentId(new object[] {
			//                        departmentId});
			//            if ((departmentRowByFK_Department_Employee == null)) {
			//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Department record ({0}) that doesn\'t exist", departmentId));
			//            }
			//            departmentRowByFK_Department_Employee.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, Teraque.UnitTest.Server.DataModel.lockTimeout);
			//            middleTierTransaction.AdoResourceManager.AddLock(departmentRowByFK_Department_Employee);
			//            // This makes sure the record wasn't deleted in the time between when it was found and the time it was locked.
			//            if ((departmentRowByFK_Department_Employee.RowState == global::System.Data.DataRowState.Detached)) {
			//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Department record ({0}) that doesn\'t exist", departmentId));
			//            }
			//            // Find the new proposed parent Object record if it is required for a foreign key constraint.
			//            Teraque.UnitTest.Server.DataModel.ObjectRow objectRowByFK_Object_Employee = Teraque.UnitTest.Server.DataModel.Object.FindByObjectId(new object[] {
			//                        employeeId});
			//            if ((objectRowByFK_Object_Employee == null)) {
			//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Object record ({0}) that doesn\'t exist", employeeId));
			//            }
			//            objectRowByFK_Object_Employee.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, Teraque.UnitTest.Server.DataModel.lockTimeout);
			//            middleTierTransaction.AdoResourceManager.AddLock(objectRowByFK_Object_Employee);
			//            // This makes sure the record wasn't deleted in the time between when it was found and the time it was locked.
			//            if ((objectRowByFK_Object_Employee.RowState == global::System.Data.DataRowState.Detached)) {
			//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Object record ({0}) that doesn\'t exist", employeeId));
			//            }
			//            // Find the new proposed parent Race record if it is required for a foreign key constraint.
			//            if ((raceCode != global::System.DBNull.Value)) {
			//                Teraque.UnitTest.Server.DataModel.RaceRow raceRowByFK_Race_Employee = Teraque.UnitTest.Server.DataModel.Race.FindByRaceCode(new object[] {
			//                            raceCode});
			//                if ((raceRowByFK_Race_Employee == null)) {
			//                    throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Race record ({0}) that doesn\'t exist", raceCode));
			//                }
			//                raceRowByFK_Race_Employee.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, Teraque.UnitTest.Server.DataModel.lockTimeout);
			//                middleTierTransaction.AdoResourceManager.AddLock(raceRowByFK_Race_Employee);
			//                // This makes sure the record wasn't deleted in the time between when it was found and the time it was locked.
			//                if ((raceRowByFK_Race_Employee.RowState == global::System.Data.DataRowState.Detached)) {
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

					//            // Find the new proposed parent Race record if it is required for a foreign key constraint.
					//            if ((raceCode != global::System.DBNull.Value)) {
					CodeExpression lockConditions = null;
					foreach (ColumnSchema columnSchema in relationPair.Value.ChildColumns)
						if (columnSchema.IsNullable)
							lockConditions = lockConditions == null ? new CodeBinaryOperatorExpression(new CodeArgumentReferenceExpression(CommonConversion.ToCamelCase(columnSchema.Name)), CodeBinaryOperatorType.IdentityInequality, new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(DBNull)), "Value")) :
								new CodeBinaryOperatorExpression(new CodeBinaryOperatorExpression(new CodeArgumentReferenceExpression(CommonConversion.ToCamelCase(columnSchema.Name)), CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null)), CodeBinaryOperatorType.BitwiseAnd, lockConditions);
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

					//                global::System.Object[] raceRowByFK_Race_EmployeeKey = new object[] {
					//                            employeeId});
					//                Teraque.UnitTest.Server.DataModel.RaceRow raceRowByFK_Race_Employee = Teraque.UnitTest.Server.DataModel.Race.FindByRaceCode(new object[] {
					//                            raceCode});
					//                if ((raceRowByFK_Race_Employee == null)) {
					//                    throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Race record ({0}) that doesn\'t exist", raceCode));
					//                }
					CodeVariableReferenceExpression parentKeyExpression = new CodeRandomVariableReferenceExpression();
					codeStatementCollection.Add(new CodeVariableDeclarationStatement(new CodeGlobalTypeReference(typeof(Object[])), parentKeyExpression.VariableName, new CodeKeyCreateExpression(relationPair.Value.ChildColumns)));
					if (tableSchema.PrimaryKey == null)
						codeStatementCollection.Add(new CodeVariableDeclarationStatement(parentRowType, parentRowVariableExpression.VariableName, new CodeFindByRowExpression(parentTable, parentKeyExpression, new CodeThisReferenceExpression())));
					else
						codeStatementCollection.Add(new CodeVariableDeclarationStatement(parentRowType, parentRowVariableExpression.VariableName, new CodeFindByIndexExpression(parentTable, parentKeyExpression, new CodeThisReferenceExpression())));
					codeStatementCollection.Add(new CodeCheckRecordExistsStatement(parentTable, parentRowVariableExpression, parentKeyExpression));

					//                raceRowByFK_Race_Employee.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, Teraque.UnitTest.Server.DataModel.lockTimeout);
					//                middleTierTransaction.AdoResourceManager.AddLock(raceRowByFK_Race_Employee);
					codeStatementCollection.Add(new CodeAcquireRecordReaderLockExpression(transactionExpression, parentRowVariableExpression, parentTable.DataModel));
					codeStatementCollection.Add(new CodeAddLockToTransactionExpression(transactionExpression, parentRowVariableExpression));

					//                // This makes sure the record wasn't deleted in the time between when it was found and the time it was locked.
					//                if ((raceRowByFK_Race_Employee.RowState == global::System.Data.DataRowState.Detached)) {
					//                    throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Race record ({0}) that doesn\'t exist", raceCode));
					//                }
					//            }
					codeStatementCollection.Add(new CodeCheckRecordDetachedStatement(parentTable, parentRowVariableExpression, parentKeyExpression));

				}

			//            // Update the Employee record in the ADO data model.
			//            middleTierTransaction.AdoResourceManager.AddRecord(employeeRow);
			//            try {
			//                Teraque.UnitTest.Server.DataModel.Department.AcquireLock();
			//                Teraque.UnitTest.Server.DataModel.Employee.AcquireLock();
			//                Teraque.UnitTest.Server.DataModel.Object.AcquireLock();
			//                Teraque.UnitTest.Server.DataModel.Race.AcquireLock();
			//                employeeRow.BeginEdit();
			//                employeeRow[Teraque.UnitTest.Server.DataModel.Employee.AgeColumn] = age;
			//                employeeRow[Teraque.UnitTest.Server.DataModel.Employee.DepartmentIdColumn] = departmentId;
			//                employeeRow[Teraque.UnitTest.Server.DataModel.Employee.RaceCodeColumn] = raceCode;
			//                employeeRow[Teraque.UnitTest.Server.DataModel.Employee.RowVersionColumn] = global::System.Threading.Interlocked.Increment(ref Teraque.UnitTest.Server.DataModel.masterRowVersion);
			//            }
			//            finally {
			//                employeeRow.EndEdit();
			//                Teraque.UnitTest.Server.DataModel.Department.ReleaseLock();
			//                Teraque.UnitTest.Server.DataModel.Employee.ReleaseLock();
			//                Teraque.UnitTest.Server.DataModel.Object.ReleaseLock();
			//                Teraque.UnitTest.Server.DataModel.Race.ReleaseLock();
			//            }
			this.Statements.Add(new CodeAddRecordToTransactionExpression(transactionExpression, rowVariableExpression));
			CodeTryCatchFinallyStatement tryFinallyStatement = new CodeTryCatchFinallyStatement();
			tryFinallyStatement.TryStatements.Add(new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "dataLock"), "EnterWriteLock"));
			tryFinallyStatement.TryStatements.Add(new CodeMethodInvokeExpression(rowVariableExpression, "BeginEdit"));
			foreach (ColumnSchema columnSchema in tableSchema.Columns.Values)
			{
				if (columnSchema.IsAutoIncrement || columnSchema.IsPrimaryKey)
					continue;
				CodeExpression sourceExpression;
				if (columnSchema.IsRowVersion)
				{
					sourceExpression = sourceExpression = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "IncrementRowVersion");
				}
				else
				{
					sourceExpression = new CodeArgumentReferenceExpression(CommonConversion.ToCamelCase(columnSchema.Name));
				}
				tryFinallyStatement.TryStatements.Add(new CodeAssignStatement(new CodeIndexerExpression(rowVariableExpression, new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", tableSchema.Name)), String.Format("{0}Column", columnSchema.Name))), sourceExpression));
			}
			tryFinallyStatement.FinallyStatements.Add(new CodeMethodInvokeExpression(rowVariableExpression, "EndEdit"));
			tryFinallyStatement.FinallyStatements.Add(new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "dataLock"), "ExitWriteLock"));
			this.Statements.Add(tryFinallyStatement);

			//            // Update the Employee record in the SQL data model.
			//            global::System.Data.SqlClient.SqlCommand sqlCommand = new global::System.Data.SqlClient.SqlCommand("update \"Employee\" set \"Age\"=@age,\"DepartmentId\"=@departmentId,\"RaceCode\"=@raceCod" +
			//                    "e,\"RowVersion\"=@rowVersion,\"RowVersion\"=@rowVersion where \"EmployeeId\"=@employee" +
			//                    "Id", middleTierTransaction.SqlConnection);
			//            sqlCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@age", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, false, 0, 0, null, global::System.Data.DataRowVersion.Current, age));
			//            sqlCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@departmentId", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, false, 0, 0, null, global::System.Data.DataRowVersion.Current, departmentId));
			//            sqlCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@employeeId", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, false, 0, 0, null, global::System.Data.DataRowVersion.Current, employeeId));
			//            sqlCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@raceCode", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, false, 0, 0, null, global::System.Data.DataRowVersion.Current, raceCode));
			//            sqlCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@rowVersion", global::System.Data.SqlDbType.BigInt, 0, global::System.Data.ParameterDirection.Input, false, 0, 0, null, global::System.Data.DataRowVersion.Current, employeeRow[Teraque.UnitTest.Server.DataModel.Employee.RowVersionColumn]));
			//            sqlCommand.ExecuteNonQuery();
			if (tableSchema.IsPersistent)
			{

				CodeVariableReferenceExpression sqlCommandExpression = new CodeRandomVariableReferenceExpression();
				string setList = string.Empty;
				foreach (ColumnSchema columnSchema in tableSchema.Columns.Values)
					if (columnSchema.IsPersistent && !columnSchema.IsAutoIncrement)
						setList += String.Format(setList == string.Empty ? "\"{0}\"=@{1}" : ",\"{0}\"=@{1}", columnSchema.Name, CommonConversion.ToCamelCase(columnSchema.Name));
				string whereClause = string.Empty;
				foreach (ColumnSchema columnSchema in tableSchema.PrimaryKey.Columns)
					whereClause += String.Format(whereClause == string.Empty ? "\"{0}\"=@key{0}" : " and \"{0}\"=@key{0}", columnSchema.Name);
				string insertCommandText = String.Format("update \"{0}\" set {1} where {2}", tableSchema.Name, setList, whereClause);
				this.Statements.Add(new CodeVariableDeclarationStatement(new CodeGlobalTypeReference(typeof(SqlCommand)), sqlCommandExpression.VariableName, new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(SqlCommand)), new CodePrimitiveExpression(insertCommandText), new CodePropertyReferenceExpression(transactionExpression, "SqlConnection"))));

				foreach (ColumnSchema columnSchema in tableSchema.Columns.Values)
					if (columnSchema.IsPersistent)
					{
						string variableName = CommonConversion.ToCamelCase(columnSchema.Name);
						if (!columnSchema.IsAutoIncrement)
						{
							CodeExpression sourceExpression = columnSchema.IsRowVersion ?
								(CodeExpression)new CodeIndexerExpression(rowVariableExpression, new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", tableSchema.Name)), "RowVersionColumn")) :
								(CodeExpression)new CodeArgumentReferenceExpression(variableName);
							this.Statements.Add(new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(sqlCommandExpression, "Parameters"), "Add", new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(SqlParameter)), new CodePrimitiveExpression(String.Format("@{0}", variableName)), TypeConverter.Convert(columnSchema.DataType), new CodePrimitiveExpression(0), new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(ParameterDirection)), "Input"), new CodePrimitiveExpression(false), new CodePrimitiveExpression(0), new CodePrimitiveExpression(0), new CodePrimitiveExpression(null), new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(DataRowVersion)), "Current"), sourceExpression)));
						}
					}

				foreach (ColumnSchema columnSchema in tableSchema.PrimaryKey.Columns)
				{
					string variableName = String.Format("key{0}", columnSchema.Name);
					CodeExpression sourceExpression = new CodePropertyReferenceExpression(rowVariableExpression, columnSchema.Name);
					this.Statements.Add(new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(sqlCommandExpression, "Parameters"), "Add", new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(SqlParameter)), new CodePrimitiveExpression(String.Format("@{0}", variableName)), TypeConverter.Convert(columnSchema.DataType), new CodePrimitiveExpression(0), new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(ParameterDirection)), "Input"), new CodePrimitiveExpression(false), new CodePrimitiveExpression(0), new CodePrimitiveExpression(0), new CodePrimitiveExpression(null), new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(DataRowVersion)), "Current"), sourceExpression)));
				}

				this.Statements.Add(new CodeMethodInvokeExpression(transactionExpression, "Execute", sqlCommandExpression));
			}

			//			DataModel.DestinationOrder.OnRowValidate(new DestinationOrderRowChangeEventArgs(pe9564f2717374e96a76d5222e2258784, System.Data.DataRowAction.Change));
			this.Statements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", tableSchema.Name)),
					"OnRowValidate",
					new CodeObjectCreateExpression(
						String.Format("{0}.{1}RowChangeEventArgs", tableSchema.DataModelSchema.Name, tableSchema.Name),
						rowVariableExpression,
						new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(DataRowAction)), "Change"))));

			//        }

		}

	}

}
