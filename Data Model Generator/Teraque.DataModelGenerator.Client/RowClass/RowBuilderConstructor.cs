namespace Teraque.DataModelGenerator.RowClass
{

	using System;
	using System.CodeDom;
	using System.Data;

    /// <summary>
	/// Represents a declaration of a constuctor for a strongly typed row.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class RowBuilderConstructor : CodeConstructor
	{

		/// <summary>
		/// Generates a constuctor for a strongly typed row.
		/// </summary>
		/// <param name="tableSchema">The table to which this constructor belongs.</param>
		public RowBuilderConstructor(TableSchema tableSchema)
		{

			// Construct the type names for the table and rows within the table.
			string tableTypeName = String.Format("{0}DataTable", tableSchema.Name);
			string rowTypeName = String.Format("{0}Row", tableSchema.Name);

			//		/// <summary>
			//		/// Creates a row of data from the AccessControl table schema.
			//		/// </summary>
			//		/// <param name="dataRowBuilder">An internal data structure used to build the row from the parent table schema.</param>
			//      internal EmployeeRow(global::System.Data.DataRowBuilder dataRowBuilder) : 
			//             base(dataRowBuilder) {
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Creates a row of data from the {0} table schema.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Comments.Add(new CodeCommentStatement("<param name=\"dataRowBuilder\">An internal data structure used to build the row from the parent table schema.</param>", true));
			this.Attributes = MemberAttributes.Assembly;
			this.Name = rowTypeName;
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeGlobalTypeReference(typeof(DataRowBuilder)), "dataRowBuilder"));
			this.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("dataRowBuilder"));

			//            // This creates a strongly typed Department using the generic construction methods.
			//            this.tableEmployee = ((EmployeeDataTable)(this.Table));
			this.Statements.Add(new CodeCommentStatement(String.Format("This creates a strongly typed {0} row using the generic construction methods.", tableSchema.Name)));
			this.Statements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("table{0}", tableSchema.Name)), new CodeCastExpression(tableTypeName, new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Table"))));

			//        }

		}

	}

}
