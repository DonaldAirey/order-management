namespace Teraque.DataModelGenerator.TargetClass
{

    using System;
    using System.CodeDom;
    using System.Threading;
    using System.Transactions;

	/// <summary>
	/// Generates a property that gets the lock for the data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class CurrentTransactionProperty : CodeMemberProperty
	{

		/// <summary>
		/// Generates a property that gets the lock for the data model.
		/// </summary>
		public CurrentTransactionProperty(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Gets the current transaction.
			//		/// </summary>
			//		[global::System.ComponentModel.BrowsableAttribute(false)]
			//		public static DataModelTransaction CurrentTransaction
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Gets the lock for the data model.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.AddRange(new CodeCustomAttributesForProperties());
			this.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			this.Type = new CodeTypeReference(String.Format("{0}Transaction", dataModelSchema.Name));
			this.Name = "CurrentTransaction";

			//			get
			//			{
			//				global::Teraque.OrganizationPrincipal e8116 = ((Teraque.OrganizationPrincipal)(global::System.Threading.Thread.CurrentPrincipal));
			//				return DataModel.tenantMap[e8116.Tenant].CurrentTransaction;
			CodeVariableReferenceExpression organizationPrincipal = new CodeRandomVariableReferenceExpression();
			this.GetStatements.Add(new CodeOrganizationPrincipalExpression(organizationPrincipal));
			CodeVariableReferenceExpression targetDataSet = new CodeRandomVariableReferenceExpression();
			this.GetStatements.Add(
				new CodeMethodReturnStatement(
					new CodePropertyReferenceExpression(
						new CodeIndexerExpression(
							new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "tenantMap"),
							new CodePropertyReferenceExpression(organizationPrincipal, "Organization")),
						"CurrentTransaction")));

			//			}
			//		}

		}

	}

}
