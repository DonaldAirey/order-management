namespace Teraque.DataModelGenerator.TargetClass
{

    using System;
    using System.CodeDom;
    using System.Threading;

	/// <summary>
	/// Generates a property that gets the lock for the data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class DataLockProperty : CodeMemberProperty
	{

		/// <summary>
		/// Generates a property that gets the lock for the data model.
		/// </summary>
		public DataLockProperty(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Gets the lock for the data model.
			//		/// </summary>
			//		[global::System.ComponentModel.BrowsableAttribute(false)]
			//		public static global::System.Threading.ReaderWriterLockSlim DataLock
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Gets the lock for the data model.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.AddRange(new CodeCustomAttributesForProperties());
			this.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			this.Type = new CodeGlobalTypeReference(typeof(ReaderWriterLockSlim));
			this.Name = "DataLock";

			//			get
			//			{
			//				global::Teraque.OrganizationPrincipal d220 = ((Teraque.OrganizationPrincipal)(global::System.Threading.Thread.CurrentPrincipal));
			//				DataModelDataSet p221 = DataModel.tenantMap[d220.Tenant];
			//				return p221.dataLock;
			//			}
			CodeVariableReferenceExpression organizationPrincipal = new CodeRandomVariableReferenceExpression();
			this.GetStatements.Add(new CodeOrganizationPrincipalExpression(organizationPrincipal));
			CodeVariableReferenceExpression targetDataSet = new CodeRandomVariableReferenceExpression();
			this.GetStatements.Add(new CodeTargetDataModelStatement(dataModelSchema, targetDataSet, organizationPrincipal));
			this.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(targetDataSet, "dataLock")));

			//		}

		}

	}

}
