namespace Teraque.DataModelGenerator.ChangeEventArgsClass
{

	using System;
	using System.CodeDom;
	using System.Data;

    /// <summary>
	/// Creates a constructor for the ChangeEventArgs class.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class Constructor : CodeConstructor
	{

		/// <summary>
		/// Creates a constructor for the ChangeEventArgs class.
		/// </summary>
		/// <param name="tableSchema">The table schema that describes the event arguments.</param>
		public Constructor(TableSchema tableSchema)
		{

			//            /// <summary>
			//            /// Create the arguments for a changing Department row event.
			//            /// </summary>
			//            /// <param name="departmentRow">The Department row that has changed.</param>
			//            /// <param name="dataRowAction">The action that caused the change.</param>
			//            public DepartmentRowChangeEvent(DepartmentRow departmentRow, global::System.Data.DataRowAction dataRowAction) {
			//                // Initialize the object.
			//                this.departmentRow = departmentRow;
			//                this.dataRowAction = dataRowAction;
			//            }
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Create the arguments for a changing {0} row event.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("<param name=\"{0}\">The {1} row that has changed.</param>", String.Format("{0}Row", CommonConversion.ToCamelCase(tableSchema.Name)), tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("<param name=\"dataRowAction\">The action that caused the change.</param>", true));
			this.Attributes = MemberAttributes.Public;
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(String.Format("{0}Row", tableSchema.Name)), String.Format("{0}Row", CommonConversion.ToCamelCase(tableSchema.Name))));
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeGlobalTypeReference(typeof(DataRowAction)), "dataRowAction"));
			this.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), String.Format("{0}Row", CommonConversion.ToCamelCase(tableSchema.Name))), new CodeArgumentReferenceExpression(String.Format("{0}Row", CommonConversion.ToCamelCase(tableSchema.Name)))));
			this.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "dataRowAction"), new CodeArgumentReferenceExpression("dataRowAction")));

		}

	}

}
