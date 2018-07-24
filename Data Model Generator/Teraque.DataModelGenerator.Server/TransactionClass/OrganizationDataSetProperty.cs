namespace Teraque.DataModelGenerator.TransactionClass
{

    using System;
    using System.CodeDom;

    /// <summary>
	/// Generates a property that gets transaction identifier.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class TenantTargetProperty : CodeMemberProperty
	{

		/// <summary>
		/// Generates a property that gets transaction identifier.
		/// </summary>
		public TenantTargetProperty(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Gets the TenantTarget target of this transaction.
			//		/// </summary>
			//		public global::System.Guid TenantTarget
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Gets the TenantTarget target of this transaction.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.AddRange(new CodeCustomAttributesForProperties());
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.Type = new CodeTypeReference(String.Format("Tenant{0}", dataModelSchema.Name));
			this.Name = String.Format("Tenant{0}", dataModelSchema.Name);

			//			get { return this.transactionId; }
			this.GetStatements.Add(
				new CodeMethodReturnStatement(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "tenantDataSet")));

			//		}

		}

	}

}
