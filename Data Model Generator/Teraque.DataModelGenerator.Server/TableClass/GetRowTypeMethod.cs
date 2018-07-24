namespace Teraque.DataModelGenerator.TableClass
{

	using System;
	using System.CodeDom;

	/// <summary>
	/// Creates a method that returns the strong type of a given table.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class GetRowTypeMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method that returns the strong type of a given table.
		/// </summary>
		/// <param name="tableSchema">A description of the table.</param>
		public GetRowTypeMethod(TableSchema tableSchema)
		{

			// Construct the type names for the table and rows within the table.
			string rowTypeName = String.Format("{0}Row", tableSchema.Name);

			//            /// <summary>
			//            /// Returns the type of the row in a Department table.
			//            /// </summary>
			//            /// <returns>The DepartmentRow type.</returns>
			//            protected override global::System.Type GetRowType() {
			//                return typeof(DepartmentRow);
			//            }
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Returns the type of the row in a {0} table.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("<returns>The {0} type.</returns>", rowTypeName), true));
			this.Attributes = MemberAttributes.Family | MemberAttributes.Override;
			this.ReturnType = new CodeGlobalTypeReference(typeof(Type));
			this.Name = "GetRowType";
			this.Statements.Add(new CodeMethodReturnStatement(new CodeTypeOfExpression(rowTypeName)));

		}

	}

}
