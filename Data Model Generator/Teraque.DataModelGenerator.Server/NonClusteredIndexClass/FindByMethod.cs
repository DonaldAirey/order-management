namespace Teraque.DataModelGenerator.NonClusteredIndexClass
{

	using System;
	using System.CodeDom;

    /// <summary>
	/// Creates a method that finds a row containing the given elements of an index.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class FindByMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method that finds a row containing the given elements of an index.
		/// </summary>
		/// <param name="uniqueConstraintSchema">A description of a unique constraint.</param>
		public FindByMethod(UniqueConstraintSchema uniqueConstraintSchema)
		{

			//		/// <summary>
			//		/// Finds a row in the BlotterConfiguration table containing the key elements.
			//		/// </summary>
			//		/// <param name="blotterId">The BlotterId element of the key.</param>
			//		/// <param name="reportTypeId">The ReportTypeId element of the key.</param>
			//		/// <returns>The BlotterConfiguration row that contains the key elements, or null if there is no match.</returns>
			//		public BlotterConfigurationRow Find(System.Guid blotterId, System.Guid reportTypeId)
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Finds a row in the {0} table containing the key elements.", uniqueConstraintSchema.Table.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			foreach (ColumnSchema columnSchema in uniqueConstraintSchema.Columns)
				this.Comments.Add(new CodeCommentStatement(String.Format("<param name=\"{0}\">The {1} element of the key.</param>", CommonConversion.ToCamelCase(columnSchema.Name), columnSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement(String.Format("<returns>The {0} row that contains the key elements, or null if there is no match.</returns>", uniqueConstraintSchema.Table.Name), true));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.ReturnType = new CodeTypeReference(String.Format("{0}Row", uniqueConstraintSchema.Table.Name));
			this.Name = "Find";
			foreach (ColumnSchema columnSchema in uniqueConstraintSchema.Columns)
				this.Parameters.Add(new CodeParameterDeclarationExpression(columnSchema.DataType, CommonConversion.ToCamelCase(columnSchema.Name)));

			//			try
			//			{
			//				((TenantDataModel)(this.Table.DataSet)).dataLock.EnterReadLock();
			//				return ((BlotterConfigurationRow)(base.Find(new object[] {
			//							blotterId,
			//							reportTypeId})));
			//			}
			CodeTryCatchFinallyStatement tryCatchFinallyStatement = new CodeTryCatchFinallyStatement();
			CodePropertyReferenceExpression tableExpression = new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(String.Format("{0}", uniqueConstraintSchema.Table.DataModel.Name)), uniqueConstraintSchema.Table.Name);
			tryCatchFinallyStatement.TryStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(
						new CodeCastExpression(
							new CodeTypeReference(String.Format("Tenant{0}", uniqueConstraintSchema.Table.DataModelSchema.Name)),
							new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Table"), "DataSet")),
						"dataLock"),
					"EnterReadLock"));
			tryCatchFinallyStatement.TryStatements.Add(new CodeMethodReturnStatement(new CodeCastExpression(this.ReturnType, new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "Find", new CodeKeyCreateExpression(uniqueConstraintSchema.Columns)))));

			//			catch (global::System.ArgumentException argumentException)
			//			{
			//				throw new global::System.ServiceModel.FaultException<Teraque.ArgumentFault>(new global::Teraque.ArgumentFault(argumentException.Message));
			//			}
			CodeCatchClause catchArgumentException = new CodeCatchClause("argumentException", new CodeGlobalTypeReference(typeof(ArgumentException)));
			catchArgumentException.Statements.Add(new CodeThrowArgumentExceptionStatement(new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("argumentException"), "Message")));
			tryCatchFinallyStatement.CatchClauses.Add(catchArgumentException);

			//			catch (global::System.FormatException formatException)
			//			{
			//				throw new global::System.ServiceModel.FaultException<Teraque.FormatFault>(new global::Teraque.FormatFault(formatException.Message));
			//			}
			CodeCatchClause catchFormatException = new CodeCatchClause("formatException", new CodeGlobalTypeReference(typeof(FormatException)));
			catchFormatException.Statements.Add(new CodeThrowFormatExceptionStatement(new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("formatException"), "Message")));
			tryCatchFinallyStatement.CatchClauses.Add(catchFormatException);

			//			finally
			//			{
			//				((TenantDataModel)(this.Table.DataSet)).dataLock.ExitReadLock();
			tryCatchFinallyStatement.FinallyStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(
						new CodeCastExpression(
							new CodeTypeReference(String.Format("Tenant{0}", uniqueConstraintSchema.Table.DataModelSchema.Name)),
							new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Table"), "DataSet")),
						"dataLock"),
					"ExitReadLock"));
			this.Statements.Add(tryCatchFinallyStatement);

			//			}
			//		}

		}
	}
}
