namespace Teraque.DataModelGenerator.TargetClass
{

    using System;
    using System.CodeDom;
	using System.Data;

    /// <summary>
	/// Creates a property to return a collection of the tables in the data model.
	/// </summary>
	public class TablesProperty : CodeMemberProperty
	{

		/// <summary>
		/// Creates a property to return a collection of the tables in the data model.
		/// </summary>
		/// <param name="dataModelSchema">The data model schema.</param>
		public TablesProperty(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Gets the collection of tables contained in the DataModel.
			//		/// </summary>
			//		[global::System.ComponentModel.BrowsableAttribute(false)]
			//		public static global::System.Data.DataTableCollection Tables
			//		{
			//			get
			//			{
			//				global::Teraque.OrganizationPrincipal n222 = ((Teraque.OrganizationPrincipal)(global::System.Threading.Thread.CurrentPrincipal));
			//				DataModelDataSet q223 = DataModel.tenantMap[n222.Tenant];
			//				return q223.Tables;
			//			}
			//		}
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Gets the collection of tables contained in the {0}.", dataModelSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.AddRange(new CodeCustomAttributesForProperties());
			this.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			this.Type = new CodeGlobalTypeReference(typeof(DataTableCollection));
			this.Name = "Tables";

			//			get
			//			{
			//				global::Teraque.OrganizationPrincipal n222 = ((Teraque.OrganizationPrincipal)(global::System.Threading.Thread.CurrentPrincipal));
			//				DataModelDataSet q223 = DataModel.tenantMap[n222.Tenant];
			//				return q223.Relations;
			CodeVariableReferenceExpression organizationPrincipal = new CodeRandomVariableReferenceExpression();
			this.GetStatements.Add(new CodeOrganizationPrincipalExpression(organizationPrincipal));
			CodeVariableReferenceExpression targetDataSet = new CodeRandomVariableReferenceExpression();
			this.GetStatements.Add(new CodeTargetDataModelStatement(dataModelSchema, targetDataSet, organizationPrincipal));
			this.GetStatements.Add(new CodeMethodReturnStatement(new CodePropertyReferenceExpression(targetDataSet, "Tables")));

			//			}
			//		}
		
		}

	}
}
