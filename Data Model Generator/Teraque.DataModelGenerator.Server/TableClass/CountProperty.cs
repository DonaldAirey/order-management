namespace Teraque.DataModelGenerator.TableClass
{

	using System;
	using System.CodeDom;
	using System.ComponentModel;

	/// <summary>
	/// Creates a propert that gets the count of rows in the table.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class CountProperty : CodeMemberProperty
	{

		/// <summary>
		/// Creates a propert that gets the count of rows in the table.
		/// </summary>
		/// <param name="tableSchema">A description of the table.</param>
		public CountProperty(TableSchema tableSchema)
		{

			//		/// <summary>
			//		/// Gets the number of rows in the AccountBase table.
			//		/// </summary>
			//		[global::System.ComponentModel.BrowsableAttribute(false)]
			//		public int Count
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Gets the number of rows in the {0} table.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(BrowsableAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression(false))));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.Type = new CodeGlobalTypeReference(typeof(Int32));
			this.Name = "Count";

			//			get
			//			{
			//				try
			//				{
			CodeTryCatchFinallyStatement tryCatchFinallyStatement = new CodeTryCatchFinallyStatement();

			//					((TenantTarget)this.DataSet).dataLock.EnterReadLock();
			//					return this.Rows.Count;
			//				}
			tryCatchFinallyStatement.TryStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(
						new CodeCastExpression(
							new CodeTypeReference(String.Format("Tenant{0}", tableSchema.DataModelSchema.Name)),
							new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "DataSet")),
						"dataLock"),
					"EnterReadLock"));
			tryCatchFinallyStatement.TryStatements.Add(new CodeMethodReturnStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Rows"), "Count")));

			//				finally
			//				{
			//					((TenantTarget)this.DataSet).dataLock.ExitReadLock();
			//				}
			tryCatchFinallyStatement.FinallyStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(
						new CodeCastExpression(
							new CodeTypeReference(String.Format("Tenant{0}", tableSchema.DataModelSchema.Name)),
							new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "DataSet")),
						"dataLock"),
					"ExitReadLock"));
			this.GetStatements.Add(tryCatchFinallyStatement);

			//			}
			//		}

		}

	}

}
