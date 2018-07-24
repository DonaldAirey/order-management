namespace Teraque.DataModelGenerator.TenantTargetClass
{

	using System;
    using System.CodeDom;

	/// <summary>
	/// Creates a private field that controls how long deleted records are kept until purged.
	/// </summary>
	class CompactTimeField : CodeMemberField
	{

		/// <summary>
		/// Creates a private field that controls how long deleted records are kept until purged.
		/// </summary>
		public CompactTimeField()
		{

			//        private static global::System.TimeSpan freshnessTime = global::System.TimeSpan.FromMinutes(1);
			this.Attributes = MemberAttributes.Private | MemberAttributes.Static;
			this.Type = new CodeGlobalTypeReference(typeof(TimeSpan));
			this.Name = "compactTime";
			this.InitExpression = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(TimeSpan)), "FromMinutes", new CodePrimitiveExpression(1));

		}

	}

}
