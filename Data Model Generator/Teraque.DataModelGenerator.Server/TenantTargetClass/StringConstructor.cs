namespace Teraque.DataModelGenerator.TenantTargetClass
{

    using System;
	using System.Collections.Generic;
    using System.CodeDom;
	using System.Data;
	using System.Linq;
    using System.Threading;

	/// <summary>
	/// Represents a declaration of a property that gets the parent row.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class VoidConstructor : CodeConstructor
	{

		/// <summary>
		/// Generates a property to get a parent row.
		/// </summary>
		/// <param name="foreignKeyConstraintSchema">The foreign key that references the parent table.</param>
		public VoidConstructor(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Creates the System.DataSet used to hold the data for the DataModel.
			//		/// </summary>
			//		internal DataModelDataSet(String connectionString)
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Creates the System.DataSet used to hold the data for the {0}.", dataModelSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Attributes = MemberAttributes.Assembly;
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeGlobalTypeReference(typeof(String)), "connectionString"));

			//			this.connectionString = connectionString;
			this.Statements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "connectionString"), new CodeArgumentReferenceExpression("connectionString")));


			//			this.DataSetName = "DataModel";
			this.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "DataSetName"),
					new CodePrimitiveExpression(dataModelSchema.Name)));

			//			this.CaseSensitive = true;
			this.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "CaseSensitive"),
					new CodePrimitiveExpression(true)));

			//			this.EnforceConstraints = true;
			this.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "EnforceConstraints"),
					new CodePrimitiveExpression(true)));

			// Create each of the tables and add them to the data set.
			for (int tableIndex = 0; tableIndex < dataModelSchema.Tables.Count; tableIndex++)
			{

				// The 'Tables' element of the schema is indexed by the name of the table normally.  We're going to initialize the tables in the order of their
				// ordinals (order in which they were declared).  This is done primarily to be able to give each table a 'Ordinal' number that is used during
				// transaction processing to quickly find the table (without using the table name).
				KeyValuePair<String, TableSchema> keyValuePair = Enumerable.ElementAt(dataModelSchema.Tables, tableIndex);

				//            this.tableConfiguration = new ConfigurationDataTable();
				this.Statements.Add(
					new CodeAssignStatement(
						new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", keyValuePair.Value.Name)),
						new CodeObjectCreateExpression(String.Format("{0}.{1}DataTable", keyValuePair.Value.DataModel.Name, keyValuePair.Value.Name))));

				//            this.tableConfiguration.Ordinal = 0;
				this.Statements.Add(
					new CodeAssignStatement(
						new CodePropertyReferenceExpression(
							new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", keyValuePair.Value.Name)), "Ordinal"),
							new CodePrimitiveExpression(tableIndex)));

				//            this.Tables.Add(this.tableConfiguration);
				this.Statements.Add(
					new CodeMethodInvokeExpression(
						new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Tables"),
						"Add",
						new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", keyValuePair.Value.Name))));

			}

			//			global::System.Data.ForeignKeyConstraint foreignKeyConstraint4 = new global::System.Data.ForeignKeyConstraint("FK_AccountBase_Account", new global::System.Data.DataColumn[] {
			//						this.tableAccountBase.AccountBaseIdColumn}, new global::System.Data.DataColumn[] {
			//						this.tableAccount.AccountIdColumn});
			//			this.tableAccountBase.Constraints.Add(foreignKeyConstraint4);
			//			global::System.Data.ForeignKeyConstraint foreignKeyConstraint5 = new global::System.Data.ForeignKeyConstraint("FK_LotHandling_Account", new global::System.Data.DataColumn[] {
			//						this.tableLotHandling.LotHandlingIdColumn}, new global::System.Data.DataColumn[] {
			//						this.tableAccount.LotHandlingIdColumn});
			//			this.tableLotHandling.Constraints.Add(foreignKeyConstraint5);
			foreach (KeyValuePair<String, TableSchema> tablePair in dataModelSchema.Tables)
				foreach (KeyValuePair<String, ConstraintSchema> constraintPair in tablePair.Value.Constraints)
					if (constraintPair.Value is ForeignKeyConstraintSchema)
					{

						// Construct a foreign key constraint described by this schema.
						ForeignKeyConstraintSchema foreignKeyConstraintSchema = constraintPair.Value as ForeignKeyConstraintSchema;

						// Refers to the foreign key constraint.
						CodeVariableReferenceExpression foreignKeyConstraintExpression = new CodeRandomVariableReferenceExpression();

						//			global::System.Data.ForeignKeyConstraint foreignKeyConstraint6 = new global::System.Data.ForeignKeyConstraint("FK_Country_AccountBase", new global::System.Data.DataColumn[] {
						//						this.tableCountry.CountryIdColumn}, new global::System.Data.DataColumn[] {
						//						this.tableAccountBase.CountryIdColumn});
						this.Statements.Add(
							new CodeVariableDeclarationStatement(
								new CodeGlobalTypeReference(typeof(ForeignKeyConstraint)),
								foreignKeyConstraintExpression.VariableName,
								new CodeForeignKeyConstraint(foreignKeyConstraintSchema)));
						this.Statements.Add(
							new CodeMethodInvokeExpression(
								new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", tablePair.Value.Name)), "Constraints"),
								"Add",
								foreignKeyConstraintExpression));

					}

			//			this.relationAccessRightAccessControl = new global::System.Data.DataRelation("FK_AccessRight_AccessControl", new global::System.Data.DataColumn[] {
			//						this.tableAccessRight.AccessRightIdColumn}, new global::System.Data.DataColumn[] {
			//						this.tableAccessControl.AccessRightIdColumn}, false);
			foreach (KeyValuePair<String, RelationSchema> relationPair in dataModelSchema.Relations)
			{

				// The name of the relation is decorated with the relation name when the relation between the child and the parent isn't unique.
				String relationName = relationPair.Value.IsDistinctPathToParent ?
					String.Format("relation{0}{1}", relationPair.Value.ParentTable.Name, relationPair.Value.ChildTable.Name) :
					String.Format("relation{0}{1}By{2}", relationPair.Value.ParentTable.Name, relationPair.Value.ChildTable.Name,
					relationPair.Value.Name);

				//            this.relationRaceEmployee = new System.Data.DataRelation("FK_Race_Employee", new Teraque.Column[] {
				//                        Teraque.UnitTest.Server.DataModel.tableRace.RaceCodeColumn}, new Teraque.Column[] {
				//                        Teraque.UnitTest.Server.DataModel.tableEmployee.RaceCodeColumn}, false);
				//            this.Relations.Add(Teraque.UnitTest.Server.DataModel.relationRaceEmployee);
				CodeExpression relationFieldExpression = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), relationName);
				this.Statements.Add(new CodeAssignStatement(relationFieldExpression, new CodeDataRelation(relationPair.Value)));
				this.Statements.Add(
					new CodeMethodInvokeExpression(
						new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Relations"),
						"Add",
						relationFieldExpression));

			}

			//            Teraque.UnitTest.Server.DataModel.Configuration.InitializeRelations();
			//            Teraque.UnitTest.Server.DataModel.Department.InitializeRelations();
			foreach (KeyValuePair<string, TableSchema> keyValuePair in dataModelSchema.Tables)
				this.Statements.Add(
					new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", keyValuePair.Value.Name)), "InitializeRelations"));

			//			this.dataLock = new global::System.Threading.ReaderWriterLockSlim();
			this.Statements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "dataLock"),
					new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(ReaderWriterLockSlim)))));

			//			this.identifier = global::System.Guid.NewGuid();
			this.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "identifier"),
					new CodeMethodInvokeExpression(new CodeGlobalTypeReferenceExpression(typeof(Guid)), "NewGuid")));

			//			this.rowVersion = 0;
			this.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "rowVersion"),
					new CodePrimitiveExpression(0L)));

			//			this.transactionLog = new global::System.Collections.Generic.LinkedList<TransactionLogItem>();
			this.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "transactionLog"),
					new CodeObjectCreateExpression(new CodeTypeReference("global::System.Collections.Generic.LinkedList<TransactionLogItem>"))));

			//			this.transactionLogBatchSize = 10000;
			this.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "transactionLogBatchSize"),
					new CodePrimitiveExpression(10000)));

			//			this.logCompressionInterval = global::System.TimeSpan.FromSeconds(10);
			this.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "logCompressionInterval"),
					new CodeMethodInvokeExpression(
						new CodeGlobalTypeReferenceExpression(typeof(TimeSpan)),
						"FromSeconds",
						new CodePrimitiveExpression(10))));

			//			this.transactionLogItemAge = global::System.TimeSpan.FromSeconds(60);
			this.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "transactionLogItemAge"),
					new CodeMethodInvokeExpression(
						new CodeGlobalTypeReferenceExpression(typeof(TimeSpan)),
						"FromSeconds",
						new CodePrimitiveExpression(60))));

			//			this.transactionLogLock = new global::System.Threading.ReaderWriterLockSlim();
			this.Statements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLogLock"),
					new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(ReaderWriterLockSlim)))));

			//			this.compressorThread = new global::System.Threading.Thread(this.CompressLog);
			this.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "compressorThread"),
					new CodeObjectCreateExpression(
						new CodeGlobalTypeReference(typeof(Thread)),
						new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "CompressLog"))));

			//			this.compressorThread.IsBackground = true;
			this.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(
						new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "compressorThread"),
						"IsBackground"),
					new CodePrimitiveExpression(true)));

			//			this.compressorThread.Name = "Transaction Log Compressor";
			this.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "compressorThread"), "Name"),
					new CodePrimitiveExpression("Transaction Log Compressor")));

			//			this.compressorThread.Priority = global::System.Threading.ThreadPriority.Lowest;
			this.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(
						new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "compressorThread"),
						"Priority"),
					new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(ThreadPriority)), "Lowest")));

			//			this.compressorThread.Start();
			this.Statements.Add(
				new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "compressorThread"), "Start"));

			//		}

			//            this.LoadData();
			this.Statements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "LoadData"));

			//        }



		}

	}

}
