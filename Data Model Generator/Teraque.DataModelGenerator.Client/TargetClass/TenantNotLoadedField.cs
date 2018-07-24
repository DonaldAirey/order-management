namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;
	using Teraque;

	class TenantNotLoadedField : CodeMemberField
	{

		/// <summary>
		/// Initialize a new instance of the TenantNotLoaded field.
		/// </summary>
		public TenantNotLoadedField()
		{

			// 		public static global::Teraque.TenantNotLoadedEventHandler TenantNotLoaded;
			this.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			this.Type = new CodeGlobalTypeReference(typeof(TenantNotLoadedEventHandler));
			this.Name = "TenantNotLoaded";

		}

	}

}
