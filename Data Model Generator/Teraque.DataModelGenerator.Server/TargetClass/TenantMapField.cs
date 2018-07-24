namespace Teraque.DataModelGenerator.TargetClass
{

    using System.CodeDom;

	/// <summary>
	/// Creates a private field that controls how long deleted records are kept until purged.
	/// </summary>
	class TenantMapField : CodeMemberField
	{

		/// <summary>
		/// Creates a private field that maps an tenant to a data model.
		/// </summary>
		public TenantMapField(DataModelSchema dataModelSchema)
		{

			//			static global::System.Collections.Generic.Dictionary<string, DataModelDataSet> tenantMap = new System.Collections.Generic.Dictionary<string, DataModelDataSet>();
			this.Attributes = MemberAttributes.Assembly | MemberAttributes.Static;
			this.Type = new CodeTypeReference(string.Format("global::System.Collections.Generic.Dictionary<string, Tenant{0}>", dataModelSchema.Name));
			this.Name = "tenantMap";
			this.InitExpression = new CodeObjectCreateExpression(this.Type);

		}

	}

}
