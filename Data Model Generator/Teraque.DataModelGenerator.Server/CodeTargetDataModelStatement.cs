namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;
	using System.Threading;

	/// <summary>
	/// Creates a statement that selects the data model for the current tenant.
	/// </summary>
	class CodeTargetDataModelStatement : CodeVariableDeclarationStatement
	{

		/// <summary>
		/// A statement creating an tenant principal.
		/// </summary>
		/// <param name="organizationPrincipal">The organizationPrincipal variable.</param>
		public CodeTargetDataModelStatement(DataModelSchema dataModelSchema, CodeVariableReferenceExpression targetDataSet, CodeVariableReferenceExpression organizationPrincipal)
		{

			//			global::Teraque.OrganizationPrincipal l1651 = ((Teraque.OrganizationPrincipal)(global::System.Threading.Thread.CurrentPrincipal));
			this.Type = new CodeTypeReference(String.Format("Tenant{0}", dataModelSchema.Name));
			this.Name = targetDataSet.VariableName;
			this.InitExpression = new CodeIndexerExpression(
				new CodePropertyReferenceExpression(
					new CodeTypeReferenceExpression(dataModelSchema.Name), "tenantMap"),
					new CodePropertyReferenceExpression(organizationPrincipal, "Organization"));

		}

	}

}
