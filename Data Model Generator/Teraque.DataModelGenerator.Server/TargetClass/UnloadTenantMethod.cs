namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;

	/// <summary>
	/// Creates a method that returns all the tenants loaded in the data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class UnloadTenantMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method that unloads a tenant from the data model.
		/// </summary>
		/// <param name="tableSchema">The schema used to describe the table.</param>
		public UnloadTenantMethod(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Creates a method that unloads a tenant from the data model.
			//		/// </summary>
			//		/// <param name="tenantName">The name of the tenant.</param>
			//		public static void UnloadTenant(string tenantName, string connectionString)
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Unloads a tenant from the data model.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Comments.Add(new CodeCommentStatement("<param name=\"tenantName\">The name of the tenant.</param>", true));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			this.Name = "UnloadTenant";
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(String)), "tenantName"));

			//			if ((DataModel.tenantMap.ContainsKey(tenantName) == true))
			//				DataModel.tenantMap.Remove(tenantName);
			this.Statements.Add(
				new CodeConditionStatement(
					new CodeBinaryOperatorExpression(
						new CodeMethodInvokeExpression(
							new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "tenantMap"),
							"ContainsKey",
							new CodeArgumentReferenceExpression("tenantName")),
						CodeBinaryOperatorType.IdentityEquality,
						new CodePrimitiveExpression(true)),
						new CodeStatement[]
						{
							new CodeExpressionStatement(
								new CodeMethodInvokeExpression(
									new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "tenantMap"),
									"Remove",
									new CodeArgumentReferenceExpression("tenantName")))
						}));

			//		}

		}

	}

}
