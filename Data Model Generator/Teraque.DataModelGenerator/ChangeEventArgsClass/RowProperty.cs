namespace Teraque.DataModelGenerator.ChangeEventArgsClass
{

	using System;
	using System.CodeDom;

    /// <summary>
	/// Creates a property for a strongly typed row event argument.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class RowProperty : CodeMemberProperty
	{

		/// <summary>
		/// Creates a property for a strongly typed row event argument.
		/// </summary>
		/// <param name="schema">The data model schema.</param>
		public RowProperty(TableSchema tableSchema)
		{

			//            /// <summary>
			//            /// Gets the Department row that has been changed.
			//            /// </summary>
			//            public DepartmentRow Row {
			//                get {
			//                    return this.departmentRow;
			//                }
			//            }
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Gets the {0} row that has been changed.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.Type = new CodeTypeReference(String.Format("{0}Row", tableSchema.Name));
			this.Name = "Row";
			this.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("{0}Row", CommonConversion.ToCamelCase(tableSchema.Name)))));
		
		}

	}
}
