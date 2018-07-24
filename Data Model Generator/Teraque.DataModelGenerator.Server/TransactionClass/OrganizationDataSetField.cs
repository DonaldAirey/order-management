namespace Teraque.DataModelGenerator.TransactionClass
{

	using System;
	using System.CodeDom;

	/// <summary>
	/// Creates a field that holds the locks used in a transaction.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class TenantTargetField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds the locks used in a transaction.
		/// </summary>
		public TenantTargetField(DataModelSchema dataModelSchema)
		{

			//				private TenantTarget tenantDataSet;
			this.Attributes = MemberAttributes.Assembly;
			this.Type = new CodeTypeReference(String.Format("Tenant{0}", dataModelSchema.Name));
			this.Name = "tenantDataSet";

		}

	}

}
