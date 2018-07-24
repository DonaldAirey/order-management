namespace Teraque.DataModelGenerator.TargetClass
{

    using System;
    using System.CodeDom;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Data;

    /// <summary>
	/// Creates a property that gets the collection of relationship between tables in the data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class RelationsProperty : CodeMemberProperty
	{

		/// <summary>
		/// Creates a property that gets the collection of relationship between tables in the data model.
		/// </summary>
		/// <param name="dataModelSchema">The data model schema.</param>
		public RelationsProperty(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Gets the collection of relations that link tables and allow navigation between parent tables and child tables.
			//		/// </summary>
			//		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//		[global::System.ComponentModel.BrowsableAttribute(false)]
			//		public static global::System.Data.DataRelationCollection Relations
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Gets the collection of relations that link tables and allow navigation between parent tables and child tables.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
            this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(DebuggerNonUserCodeAttribute))));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(BrowsableAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression(false))));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			this.Type = new CodeGlobalTypeReference(typeof(DataRelationCollection));
			this.Name = "Relations";

			//			get
			//			{
			//				global::Teraque.OrganizationPrincipal n222 = ((Teraque.OrganizationPrincipal)(global::System.Threading.Thread.CurrentPrincipal));
			//				DataModelDataSet q223 = DataModel.tenantMap[n222.Tenant];
			//				return q223.Relations;
			CodeVariableReferenceExpression organizationPrincipal = new CodeRandomVariableReferenceExpression();
			this.GetStatements.Add(new CodeOrganizationPrincipalExpression(organizationPrincipal));
			CodeVariableReferenceExpression targetDataSet = new CodeRandomVariableReferenceExpression();
			this.GetStatements.Add(new CodeTargetDataModelStatement(dataModelSchema, targetDataSet, organizationPrincipal));
			this.GetStatements.Add(new CodeMethodReturnStatement(new CodePropertyReferenceExpression(targetDataSet, "Relations")));

			//			}
			//		}

		}

	}
}
