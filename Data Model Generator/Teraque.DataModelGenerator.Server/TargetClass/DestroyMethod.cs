namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;
    using System.Collections.Generic;
	using System.ServiceModel;
	using System.Security.Permissions;
	using System.Threading;

    /// <summary>
	/// Creates a method that loads records into the database from an external source.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class DestroyMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method that loads records into the database from an external source.
		/// </summary>
		/// <param name="tableSchema">The schema used to describe the table.</param>
		public DestroyMethod(TableSchema tableSchema)
		{


			// This shreds the list of parameters up into a metadata stucture that is helpful in extracting ordinary parameters 
			// from those that need to be found in other tables using external identifiers.
			DestroyExParameterMatrix destroyParameterMatrix = new DestroyExParameterMatrix(tableSchema);

			//        /// <summary>
			//        /// Loads a record into the Department table from an external source.
			//        /// </summary>
			//        /// <param name="configurationId">Selects a configuration of unique indices used to resolve external identifiers.</param>
			//        /// <param name="employeeKey">An optional unique key for the parent Employee record.</param>
			//        /// <param name="managerKey">An optional unique key for the parent Manager record.</param>
			//        [global::System.ServiceModel.OperationBehaviorAttribute(TransactionScopeRequired=true)]
			//        [global::Teraque.ClaimsPrincipalPermission(global::System.Security.Permissions.SecurityAction.Demand, ClaimType=global::Teraque.ClaimTypes.Create, Resource=global::Teraque.Resources.Application)]
			//        public void DestroyEngineerEx(string configurationId, object[] employeeKey, object[] managerKey) {
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Loads a record into the Department table from an external source.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(OperationBehaviorAttribute)), new CodeAttributeArgument("TransactionScopeRequired", new CodePrimitiveExpression(true))));
			this.CustomAttributes.Add(
				new CodeAttributeDeclaration(
					new CodeGlobalTypeReference(typeof(ClaimsPermission)),
					new CodeAttributeArgument(new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(SecurityAction)), "Demand")),
					new CodeAttributeArgument("ClaimType", new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(ClaimTypes)), "Create")),
					new CodeAttributeArgument("Resource", new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(Resources)), "Application"))));
			foreach (KeyValuePair<string, ExternalParameterItem> parameterPair in destroyParameterMatrix.ExternalParameterItems)
				this.Comments.Add(new CodeCommentStatement(String.Format("<param name=\"{0}\">{1}</param>", parameterPair.Value.Name, parameterPair.Value.Description), true));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.Name = String.Format("Destroy{0}", tableSchema.Name);
			foreach (KeyValuePair<string, ExternalParameterItem> parameterPair in destroyParameterMatrix.ExternalParameterItems)
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

			// This will resolve the external identifiers and the build the primary key for the target record.  The main idea is to
			// map elements from foreign rows into parameters that can be used to call the internal methods.
			foreach (KeyValuePair<string, ExternalParameterItem> parameterPair in destroyParameterMatrix.ExternalParameterItems)
			{

				// Every internal update method requires a primary key.  The external methods do not have this requirement and can
				// use any unique key.  The translation between the external unique key and the internal primary key is created
				// here.
				if (parameterPair.Value is UniqueConstraintParameterItem)
					this.Statements.AddRange(new CodeResolvePrimaryKeyStatements(tableSchema, transactionExpression, parameterPair.Value as UniqueConstraintParameterItem, targetDataSet));

			}

			// At this point, all the external variables have been resolved and the primary index of the target row has been
			// calculated in the parameter matrix.  This will perform the destroy with the internal method.
			this.Statements.Add(
				new CodeMethodInvokeExpression(targetDataSet, String.Format("Destroy{0}", tableSchema.Name), destroyParameterMatrix.DestroyParameters));

		}

	}

}
