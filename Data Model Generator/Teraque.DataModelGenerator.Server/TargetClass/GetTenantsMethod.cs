namespace Teraque.DataModelGenerator.TargetClass
{

    using System;
    using System.CodeDom;
    using System.Collections.Generic;
	using System.ServiceModel;
	using System.Security.Permissions;
	using System.Threading;

    /// <summary>
	/// Creates a method that returns all the tenants loaded in the data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class GetTenantsMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method that returns all the tenants loaded in the data model.
		/// </summary>
		/// <param name="tableSchema">The schema used to describe the table.</param>
		public GetTenantsMethod(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Gets the organizations currently loaded in the data model.
			//		/// </summary>
			//		/// <returns>An array of the tenants currently loaded in the data model.</returns>
			//		public static string[] GetOrganizations()
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Gets the organizations currently loaded in the data model.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			this.ReturnType = new CodeTypeReference(typeof(String[]));
			this.Name = "GetTenants";

			//			string[] organizations = new string[DataModel.organizationMap.Count];
			this.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference(typeof(String[])),
					"tenants",
					new CodeArrayCreateExpression(
						new CodeTypeReference(typeof(String[])),
						new CodePropertyReferenceExpression(
							new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "tenantMap"), "Count"))));

			//			DataModel.organizationMap.Keys.CopyTo(organizations, 0);
			this.Statements.Add(
				new CodeMethodInvokeExpression(
					new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "tenantMap"), "Keys"),
					"CopyTo",
					new CodeVariableReferenceExpression("tenants"),
					new CodePrimitiveExpression(0)));

			//			return organizations;
			this.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("tenants")));

			//		}

		}

	}

}
