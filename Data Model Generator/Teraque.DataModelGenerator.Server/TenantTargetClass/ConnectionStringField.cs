namespace Teraque.DataModelGenerator.TenantTargetClass
{

    using System;
    using System.CodeDom;

	/// <summary>
	/// Creates a field that holds a delegate to a method that filters rows from the client data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class ConnectionStringField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds the reader/writer lock for the current data model.
		/// </summary>
		public ConnectionStringField()
		{

			//        internal string connectionString;
			this.Attributes = MemberAttributes.Assembly;
			this.Type = new CodeGlobalTypeReference(typeof(String));
			this.Name = "connectionString";

		}

	}

}
