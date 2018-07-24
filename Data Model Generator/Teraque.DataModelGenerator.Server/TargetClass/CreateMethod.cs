namespace Teraque.DataModelGenerator.TargetClass
{

    using System;
    using System.CodeDom;
    using System.Collections.Generic;
	using System.Security.Permissions;
	using System.ServiceModel;
	using System.Threading;

    /// <summary>
	/// Creates a method that loads records into the database from an external source.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class CreateMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method that loads records into the database from an external source.
		/// </summary>
		/// <param name="tableSchema">The schema used to describe the table.</param>
		public CreateMethod(TableSchema tableSchema)
		{

			// This shreds the list of parameters up into a metadata stucture that is helpful in extracting ordinary parameters 
			// from those that need to be found in other tables using external identifiers.
			CreateExParameterMatrix createParameterMatrix = new CreateExParameterMatrix(tableSchema);

			//        /// <summary>
			//        /// Loads a record into the Department table from an external source.
			//        /// </summary>
			//        /// <param name="age">The required value for the Age column.</param>
			//        /// <param name="configurationId">Selects a configuration of unique indices used to resolve external identifiers.</param>
			//        /// <param name="genderKey">A required unique key for the parent Gender record.</param>
			//        /// <param name="objectKey">A required unique key for the parent Object record.</param>
			//        /// <param name="unionKey">A required unique key for the parent Union record.</param>
			//        [global::System.ServiceModel.OperationBehaviorAttribute(TransactionScopeRequired=true)]
			//        [global::Teraque.ClaimsPrincipalPermission(global::System.Security.Permissions.SecurityAction.Demand, ClaimType=global::Teraque.ClaimTypes.Create, Resource=global::Teraque.Resources.Application)]
			//        public void CreateEmployee(int age, string configurationId, object[] genderKey, object[] objectKey, object[] unionKey) {
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Creates a record in the {0} table.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.Add(
				new CodeAttributeDeclaration(
					new CodeGlobalTypeReference(typeof(OperationBehaviorAttribute)),
					new CodeAttributeArgument("TransactionScopeRequired", new CodePrimitiveExpression(true))));
			this.CustomAttributes.Add(
				new CodeAttributeDeclaration(
					new CodeGlobalTypeReference(typeof(ClaimsPermission)),
					new CodeAttributeArgument(new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(SecurityAction)), "Demand")),
					new CodeAttributeArgument("ClaimType", new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(ClaimTypes)), "Create")),
					new CodeAttributeArgument("Resource", new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(Resources)), "Application"))));
			foreach (KeyValuePair<string, ExternalParameterItem> parameterPair in createParameterMatrix.ExternalParameterItems)
				this.Comments.Add(new CodeCommentStatement(String.Format("<param name=\"{0}\">{1}</param>", parameterPair.Value.Name, parameterPair.Value.Description), true));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.Name = String.Format("Create{0}", tableSchema.Name);
			foreach (KeyValuePair<string, ExternalParameterItem> parameterPair in createParameterMatrix.ExternalParameterItems)
				this.Parameters.Add(parameterPair.Value.CodeParameterDeclarationExpression);

			//			global::Teraque.OrganizationPrincipal c226 = ((Teraque.OrganizationPrincipal)(global::System.Threading.Thread.CurrentPrincipal));
			CodeVariableReferenceExpression organizationPrincipal = new CodeRandomVariableReferenceExpression();
			this.Statements.Add(new CodeOrganizationPrincipalExpression(organizationPrincipal));

			//			if ((DataModel.tenantMap.ContainsKey(q3953.Organization) == false))
			//			{
			//				throw new global::System.ServiceModel.FaultException<Teraque.TenantNotLoadedFault>(new global::Teraque.TenantNotLoadedFault(q3953.Organization));
			//			}
			this.Statements.Add(
				new CodeConditionStatement(
					new CodeBinaryOperatorExpression(
						new CodeMethodInvokeExpression(
							new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(tableSchema.DataModelSchema.Name), "tenantMap"),
							"ContainsKey",
							new CodePropertyReferenceExpression(organizationPrincipal, "Organization")),
						CodeBinaryOperatorType.IdentityEquality,
						new CodePrimitiveExpression(false)),
					new CodeStatement[]
					{
						new CodeThrowTenantNotLoadedExceptionStatement(new CodePropertyReferenceExpression(organizationPrincipal, "Organization"))
					}));

			//			DataModelTransaction o1881 = DataModel.CurrentTransaction;
			//			TenantTarget d1882 = o1881.tenantDataSet;
			CodeVariableReferenceExpression transactionExpression = new CodeRandomVariableReferenceExpression();
			this.Statements.Add(new CodeCreateTransactionStatement(tableSchema.DataModel, transactionExpression));
			CodeVariableReferenceExpression targetDataSet = new CodeRandomVariableReferenceExpression();
			this.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference(String.Format("Tenant{0}", tableSchema.DataModelSchema.Name)),
					targetDataSet.VariableName,
					new CodeFieldReferenceExpression(transactionExpression, "tenantDataSet")));

			// This will resolve the external identifiers that relate to foreign tables.  The main idea is to map elements from foreign rows into parameters that
			// can be used to call the internal methods.
			foreach (KeyValuePair<string, ExternalParameterItem> parameterPair in createParameterMatrix.ExternalParameterItems)
			{

				// This will recurse into the foreign key relations that use external identifiers and create code to resolve the
				// variables using the external record.
				if (parameterPair.Value is ForeignKeyConstraintParameterItem)
					this.Statements.AddRange(new CodeResolveExternalVariableStatements(tableSchema, transactionExpression, parameterPair.Value as ForeignKeyConstraintParameterItem, targetDataSet));

			}

			// All the external identifiers have been resolved.  Now it is time to see if the record exists or if it has to be created.  Finding the record requires
			// a unique index.  If there are more than one unique index, a decision needs to be made as to which one should be used.  The configuration will drive
			// that decision.  If there is only one unique constraint, then the decision doesn't need to be made.
			UniqueConstraintSchema[] uniqueConstraints = tableSchema.UniqueConstraintSchemas;

			// Optimized code is provided when there is only one unique constraint on a table.  This saves the database administrator from having to configure every
			// single table in the data model with a description of the unique index that is to be used when finding a row in that table.  If there is more than one
			// unique constraint on a table, a value will need to be provided in the configuration to tell the loader which one to use.
			if (uniqueConstraints.Length == 1)
			{

				//				object[] i1650 = new object[] {
				//					configurationId,
				//					relationName};
				this.Statements.Add(
					new CodeVariableDeclarationStatement(
						new CodeGlobalTypeReference(typeof(Object[])),
						createParameterMatrix.UniqueKeyExpression.VariableName,
						new CodeKeyCreateExpression(createParameterMatrix.CreateKey(uniqueConstraints[0]))));

				//				ConfigurationRow d1649 = f1652.tableConfiguration.ConfigurationKey.Find(i1650);
				this.Statements.Add(
					new CodeVariableDeclarationStatement(
						new CodeTypeReference(String.Format("{0}Row", tableSchema.Name)),
						createParameterMatrix.RowExpression.VariableName,
						new CodeMethodInvokeExpression(
							new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(targetDataSet, String.Format("table{0}", tableSchema.Name)), uniqueConstraints[0].Name),
							"Find",
							createParameterMatrix.UniqueKeyExpression)));

			}
			else
			{

				//				object[] g2835 = new object[] {
				//					configurationId,
				//					"Country"};
				CodeVariableReferenceExpression configurationKeyExpression = new CodeRandomVariableReferenceExpression();
				this.Statements.Add(
					new CodeVariableDeclarationStatement(
						new CodeGlobalTypeReference(typeof(Object[])),
						configurationKeyExpression.VariableName,
						new CodeKeyCreateExpression(new CodeArgumentReferenceExpression("configurationId"), new CodePrimitiveExpression(tableSchema.Name))));

				//				ConfigurationRow t2836 = a2833.tableConfiguration.ConfigurationKey.Find(g2835);
				CodeVariableReferenceExpression configurationRowExpression = new CodeRandomVariableReferenceExpression();
				this.Statements.Add(
					new CodeVariableDeclarationStatement(
						new CodeTypeReference("ConfigurationRow"),
						configurationRowExpression.VariableName,
						new CodeMethodInvokeExpression(
							new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(targetDataSet, "tableConfiguration"), "ConfigurationKey"),
							"Find",
							configurationKeyExpression)));

				//				if ((t2836 == null))
				//				{
				//					throw new global::System.ServiceModel.FaultException<Teraque.RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Configuration", g2835));
				//				}
				TableSchema configurationTableSchema = tableSchema.DataModel.Tables["Configuration"];
				this.Statements.Add(new CodeCheckRecordExistsStatement(configurationTableSchema, configurationRowExpression, configurationKeyExpression));

				//				t2836.AcquireReaderLock(f2834.TransactionId, DataModel.lockTimeout);
				this.Statements.Add(new CodeAcquireRecordReaderLockExpression(transactionExpression, configurationRowExpression, tableSchema.DataModel));

				//				f2834.AddLock(t2836);
				this.Statements.Add(new CodeAddLockToTransactionExpression(transactionExpression, configurationRowExpression));

				//				if ((t2836.RowState == global::System.Data.DataRowState.Detached))
				//				{
				//					throw new global::System.ServiceModel.FaultException<Teraque.RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Configuration", g2835));
				//				}
				this.Statements.Add(new CodeCheckRecordDetachedStatement(configurationTableSchema, configurationRowExpression, configurationKeyExpression));

				//				object[] a2831 = null;
				this.Statements.Add(new CodeVariableDeclarationStatement(new CodeGlobalTypeReference(typeof(Object[])), createParameterMatrix.UniqueKeyExpression.VariableName, new CodePrimitiveExpression(null)));

				//				if ((t2836.IndexName == "CountryKey"))
				//				{
				//					a2831 = new object[] {
				//						countryId};
				//				}
				//				if ((t2836.IndexName == "CountryKeyExternalId0"))
				//				{
				//					a2831 = new object[] {
				//						externalId0};
				//				}
				//				if ((t2836.IndexName == "CountryKeyExternalId1"))
				//				{
				//					a2831 = new object[] {
				//						externalId1};
				//				}
				CodePropertyReferenceExpression indexNameExpression = new CodePropertyReferenceExpression(configurationRowExpression, "IndexName");
				foreach (UniqueConstraintSchema uniqueConstraintSchema in uniqueConstraints)
				{
					CodeConditionStatement ifConfiguration = new CodeConditionStatement(
						new CodeBinaryOperatorExpression(indexNameExpression, CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(uniqueConstraintSchema.Name)));
					ifConfiguration.TrueStatements.Add(
						new CodeAssignStatement(createParameterMatrix.UniqueKeyExpression, new CodeKeyCreateExpression(createParameterMatrix.CreateKey(uniqueConstraintSchema))));
					this.Statements.Add(ifConfiguration);
				}

				//				ICountryIndex v2837 = ((ICountryIndex)(DataModel.Country.Indices[t2836.IndexName]));
				CodeTypeReference dataIndexType = new CodeTypeReference(String.Format("I{0}Index", tableSchema.Name));
				CodeVariableReferenceExpression dataIndexExpression = new CodeRandomVariableReferenceExpression();
				this.Statements.Add(
					new CodeVariableDeclarationStatement(
						dataIndexType,
						dataIndexExpression.VariableName,
						new CodeCastExpression(
							dataIndexType,
							new CodeIndexerExpression(
								new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(targetDataSet, String.Format("table{0}", tableSchema.Name)), "Indices"),
								indexNameExpression))));

				//				if ((v2837 == null))
				//				{
				//					throw new global::System.ServiceModel.FaultException<Teraque.IndexNotFoundFault>(new global::Teraque.IndexNotFoundFault("Country", t2836.IndexName));
				//				}
				this.Statements.Add(new CodeCheckIndexExistsStatement(dataIndexExpression, tableSchema, indexNameExpression));

				//				CountryRow g2830 = v2837.Find(a2831);
				this.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(String.Format("{0}Row", tableSchema.Name)), createParameterMatrix.RowExpression.VariableName, new CodeMethodInvokeExpression(dataIndexExpression, "Find", createParameterMatrix.UniqueKeyExpression)));

			}

			//			objectTreeId = global::System.Guid.NewGuid();
			//			this.CreateObjectTree(childId, objectTreeId, parentId, out rowVersion);
			foreach (KeyValuePair<string, InternalParameterItem> internalParameterPair in createParameterMatrix.CreateParameterItems)
			{
				InternalParameterItem internalParameterItem = internalParameterPair.Value;
				if (internalParameterItem.ColumnSchema.DataType == typeof(Guid) && internalParameterItem.ColumnSchema.IsOrphan)
				{
					CodeConditionStatement ifGuidNull = internalParameterItem.ColumnSchema.IsNullable || internalParameterItem.ColumnSchema.DefaultValue != DBNull.Value ?
						new CodeConditionStatement(new CodeBinaryOperatorExpression(new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(internalParameterItem.Name), CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null)), CodeBinaryOperatorType.BooleanAnd,
						new CodeBinaryOperatorExpression(new CodeCastExpression(new CodeGlobalTypeReference(internalParameterItem.ColumnSchema.DataType), new CodeVariableReferenceExpression(internalParameterItem.Name)), CodeBinaryOperatorType.IdentityEquality, new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(Guid)), "Empty")))) :
						new CodeConditionStatement(new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(internalParameterItem.Name), CodeBinaryOperatorType.IdentityEquality, new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(Guid)), "Empty")));
					ifGuidNull.TrueStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(internalParameterItem.Name), new CodeMethodInvokeExpression(new CodeGlobalTypeReferenceExpression(typeof(Guid)), "NewGuid")));
					this.Statements.Add(ifGuidNull);
				}
			}
			this.Statements.Add(
				new CodeMethodInvokeExpression(targetDataSet, String.Format("Create{0}", tableSchema.Name), createParameterMatrix.CreateParameterExpressions));

			//        }

		}

	}

}
