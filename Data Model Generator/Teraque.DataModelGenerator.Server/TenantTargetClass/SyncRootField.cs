namespace Teraque.DataModelGenerator.TenantTargetClass
{

    using System;
    using System.CodeDom;

	/// <summary>
	/// Creates a field that holds the records used in a transaction.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class SyncRootField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds the records used in a transaction.
		/// </summary>
		public SyncRootField()
		{

			//		private static object syncRoot;
			this.Attributes = MemberAttributes.Private;
			this.Type = new CodeGlobalTypeReference(typeof(Object));
			this.Name = "syncRoot";
			this.InitExpression = new CodeObjectCreateExpression(new CodeTypeReference(typeof(Object)));

		}

	}

}
