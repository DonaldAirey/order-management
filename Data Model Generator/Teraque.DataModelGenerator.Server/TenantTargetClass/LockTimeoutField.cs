namespace Teraque.DataModelGenerator.TenantTargetClass
{

	using System;
	using System.CodeDom;

	/// <summary>
	/// Creates a private field that holds the amount of time a record lock is held.
	/// </summary>
	class LockTimeoutField : CodeMemberField
	{

		/// <summary>
		/// Creates a private field that holds the amount of time a record lock is held.
		/// </summary>
		public LockTimeoutField()
		{

			//        private static global::System.TimeSpan lockTimeout;
			this.Attributes = MemberAttributes.Assembly | MemberAttributes.Static;
			this.Type = new CodeGlobalTypeReference(typeof(TimeSpan));
			this.Name = "lockTimeout";
			this.InitExpression = new CodePropertyReferenceExpression(
				new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(Teraque.Properties.Settings)), "Default"),
				"LockTimeout");


		}

	}

}
