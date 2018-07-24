namespace Teraque.DataModelGenerator.RowClass
{

	using System;
	using System.CodeDom;
    using Teraque;

    /// <summary>
	/// Represents a declaration of a method that gets a list of the child rows.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class GetChildRowsMethod : CodeMemberMethod
	{

		/// <summary>
		/// Generates a method to get a list of child rows.
		/// </summary>
		/// <param name="relationSchema">A description of the relation between two tables.</param>
		public GetChildRowsMethod(RelationSchema relationSchema)
		{

			// These variables are used to construct the method.
			TableSchema childTable = relationSchema.ChildTable;
			TableSchema parentTable = relationSchema.ParentTable;
			String tableFieldName = String.Format("table{0}", parentTable.Name);
			String childRowName = String.Format("{0}Row", relationSchema.ChildTable.Name);
			String childRowTypeName = String.Format("{0}Row", relationSchema.ChildTable.Name);

			//        /// <summary>
			//        /// Gets the children rows in the Engineer table.
			//        /// </summary>
			//        public EngineerRow[] GetEngineerRows() {
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Gets the children rows in the {0} table.", relationSchema.ChildTable.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.ReturnType = new CodeTypeReference(childRowTypeName, 1);
			this.Name = relationSchema.IsDistinctPathToChild ?
				String.Format("Get{0}s", childRowName) :
				String.Format("Get{0}sBy{1}", childRowName, relationSchema.Name);
			String relationName = relationSchema.IsDistinctPathToChild ?
				String.Format("{0}{1}Relation", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name) :
				String.Format("{0}{1}By{2}Relation", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name, relationSchema.Name);

			//            return ((EngineerRow[])(this.GetChildRows(this.tableEmployee.EmployeeEngineerRelation)));
			this.Statements.Add(
				new CodeMethodReturnStatement(
					new CodeCastExpression(
						new CodeTypeReference(childRowTypeName, 1),
						new CodeMethodInvokeExpression(
							new CodeThisReferenceExpression(),
							"GetChildRows",
							new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), tableFieldName), relationName)))));

			//        }

		}

	}

}
