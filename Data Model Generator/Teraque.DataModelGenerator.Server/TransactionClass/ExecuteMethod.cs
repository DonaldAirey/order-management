namespace Teraque.DataModelGenerator.TransactionClass
{

    using System.CodeDom;
	using System.Data.SqlClient;
	using System.Transactions;

	/// <summary>
	/// Creates a method to read an XML file.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class ExecuteMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method to read an XML file.
		/// </summary>
		/// <param name="schema">The data model schema.</param>
		public ExecuteMethod(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Executes the SQL command asynchronously.
			//		/// </summary>
			//		/// <param name="sqlCommand">The command to be executed asynchronously.</param>
			//		internal void Execute(global::System.Data.SqlClient.SqlCommand sqlCommand)
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("The command to be executed asynchronously.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Comments.Add(new CodeCommentStatement("<param name=\"sqlCommand\">The command to be executed asynchronously.", true));
			this.CustomAttributes.AddRange(new CodeCustomAttributesForMethods());
			this.Attributes = MemberAttributes.Assembly | MemberAttributes.Final;
			this.Name = "Execute";
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeGlobalTypeReference(typeof(SqlCommand)), "sqlCommand"));

			//			this.countdownEvent.AddCount();
			this.Statements.Add(new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "countdownEvent"), "AddCount"));

			//			sqlCommand.BeginExecuteNonQuery(this.HandleCallback, sqlCommand);
			this.Statements.Add(
				new CodeMethodInvokeExpression(
					new CodeArgumentReferenceExpression("sqlCommand"),
					"BeginExecuteNonQuery",
					new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "HandleQueryCallback"),
					new CodeArgumentReferenceExpression("sqlCommand")));

			//		}

		}

	}
}
