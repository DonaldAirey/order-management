namespace Teraque.DataModelGenerator.TransactionClass
{

    using System.CodeDom;
    using System.Data.SqlClient;

	/// <summary>
	/// Creates a field that holds the connection to the SQL database.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class SqlConnectionField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds the connection to the SQL database.
		/// </summary>
		public SqlConnectionField()
		{

			//		private global::System.Data.SqlClient.SqlConnection sqlConnection;
			this.Attributes = MemberAttributes.Private;
			this.Type = new CodeGlobalTypeReference(typeof(SqlConnection));
			this.Name = "sqlConnection";

		}

	}

}
