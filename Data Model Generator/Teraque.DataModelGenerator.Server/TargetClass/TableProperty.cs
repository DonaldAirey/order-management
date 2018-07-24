namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
    using System.CodeDom;

    /// <summary>
	/// Creates a property to get a data table from the data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class TableProperty : CodeMemberProperty
	{

		/// <summary>
		/// Creates a property to get a data table from the data model.
		/// </summary>
		/// <param name="tableSchema">A description of the table.</param>
		public TableProperty(TableSchema tableSchema)
		{

			//		/// <summary>
			//		/// Gets an accessor for the Account table.
			//		/// </summary>
			//		[global::System.ComponentModel.BrowsableAttribute(false)]
			//		public static AccountDataTable Account
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Gets an accessor for the {0} table.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.AddRange(new CodeCustomAttributesForProperties());
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
			this.Type = new CodeTypeReference(String.Format("{0}DataTable", tableSchema.Name));
			this.Name = tableSchema.Name;

			//			get
			//			{
			//				global::Teraque.OrganizationPrincipal n205 = ((Teraque.OrganizationPrincipal)(global::System.Threading.Thread.CurrentPrincipal));
			//				TenantTarget t206 = DataModel.tenantMap[n205.Tenant];
			//				return t206.tableAccount;
			CodeVariableReferenceExpression organizationPrincipal = new CodeRandomVariableReferenceExpression();
			this.GetStatements.Add(new CodeOrganizationPrincipalExpression(organizationPrincipal));
			CodeVariableReferenceExpression targetDataSet = new CodeRandomVariableReferenceExpression();
			this.GetStatements.Add(new CodeTargetDataModelStatement(tableSchema.DataModel, targetDataSet, organizationPrincipal));
			this.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(targetDataSet, String.Format("table{0}", tableSchema.Name))));

			//			}
			//		}

		}

	}
}
