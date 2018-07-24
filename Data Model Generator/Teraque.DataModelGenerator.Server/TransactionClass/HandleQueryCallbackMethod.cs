namespace Teraque.DataModelGenerator.TransactionClass
{

	using System;
	using System.CodeDom;
	using System.Data.SqlClient;
	using System.Transactions;

	/// <summary>
	/// Creates a method to read an XML file.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class HandleQueryCallbackMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method to read an XML file.
		/// </summary>
		/// <param name="schema">The data model schema.</param>
		public HandleQueryCallbackMethod(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Handles the callback signaling the completion of an SQL command.
			//		/// </summary>
			//		/// <param name="iAsyncResult">The result of executing the command.</param>
			//		private void HandleQueryCallbackMethod(global::System.IAsyncResult iAsyncResult)
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Handles the callback signaling the completion of an SQL command.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Comments.Add(new CodeCommentStatement("<param name=\"iAsyncResult\">The result of executing the command.", true));
			this.CustomAttributes.AddRange(new CodeCustomAttributesForMethods());
			this.Attributes = MemberAttributes.Final;
			this.Name = "HandleQueryCallback";
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeGlobalTypeReference(typeof(IAsyncResult)), "iAsyncResult"));

			//			global::System.Data.SqlClient.SqlCommand command = (global::System.Data.SqlClient.SqlCommand)result.AsyncState;
			this.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference(typeof(SqlCommand)),
					"sqlCommand",
					new CodeCastExpression(
						new CodeTypeReference(typeof(SqlCommand)), new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("iAsyncResult"), "AsyncState"))));

			//			command.EndExecuteNonQuery(result);
			this.Statements.Add(
				new CodeMethodInvokeExpression(
					new CodeVariableReferenceExpression("sqlCommand"),
					"EndExecuteNonQuery",
					new CodeVariableReferenceExpression("iAsyncResult")));

			//			this.countdownEvent.Signal();
			this.Statements.Add(new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "countdownEvent"), "Signal"));

			//		}

		}

	}
}
