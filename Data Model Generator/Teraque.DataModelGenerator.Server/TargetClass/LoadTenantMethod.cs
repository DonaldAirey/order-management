namespace Teraque.DataModelGenerator.TargetClass
{

    using System;
    using System.CodeDom;

    /// <summary>
	/// Creates a method that returns all the tenants loaded in the data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class LoadTenantMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method that loads a tenant into the data model.
		/// </summary>
		/// <param name="tableSchema">The schema used to describe the table.</param>
		public LoadTenantMethod(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Loads a tenant into the data model.
			//		/// </summary>
			//		/// <param name="tenantName">The name of the tenant.</param>
			//		/// <param name="connectionString">The connection string used to connect the tenant to the database.</param>
			//		public static void LoadTenant(string tenantName, string connectionString)
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Loads a tenant into the data model.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Comments.Add(new CodeCommentStatement("<param name=\"tenantName\">The name of the tenant.</param>", true));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			this.Name = "LoadTenant";
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(String)), "tenantName"));
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(String)), "connectionString"));

			//			if ((DataModel.tenantMap.ContainsKey(tenantName) == false))
			//				DataModel.tenantMap.Add(tenantName, new TenantDataModel(connectionString));
			this.Statements.Add(
				new CodeConditionStatement(
					new CodeBinaryOperatorExpression(
						new CodeMethodInvokeExpression(
							new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "tenantMap"),
							"ContainsKey",
							new CodeArgumentReferenceExpression("tenantName")),
						CodeBinaryOperatorType.IdentityEquality,
						new CodePrimitiveExpression(false)),
						new CodeStatement[]
						{
							new CodeExpressionStatement(
								new CodeMethodInvokeExpression(
									new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "tenantMap"),
									"Add",
									new CodeArgumentReferenceExpression("tenantName"),
									new CodeObjectCreateExpression(
										new CodeTypeReference(String.Format("Tenant{0}", dataModelSchema.Name)),
										new CodeArgumentReferenceExpression("connectionString"))))
						}));

			//		}

		}

	}

}
