namespace Teraque.DataModelGenerator.RowClass
{

	using System;
	using System.CodeDom;
	using System.Diagnostics;
    using Teraque;

    /// <summary>
	/// Creates a property that gets the parent row.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class ParentRowProperty : CodeMemberProperty
	{

		/// <summary>
		/// Creates a property that gets the parent row.
		/// </summary>
		/// <param name="relationSchema">The foreign key that references the parent table.</param>
		public ParentRowProperty(RelationSchema relationSchema)
		{

			// These constructs are used several times to generate the property.
			TableSchema childTable = relationSchema.ChildTable;
			TableSchema parentTable = relationSchema.ParentTable;
			String propertyName = String.Format("{0}Row", parentTable);
			String propertyTypeName = String.Format("{0}Row", parentTable);
			String tableFieldName = String.Format("table{0}", childTable.Name);
			String relationName = relationSchema.IsDistinctPathToParent ?
				String.Format("{0}{1}Relation", parentTable.Name, childTable.Name) :
				String.Format("{0}{1}By{2}Relation", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name, relationSchema.Name);
			String relationFieldName = relationSchema.IsDistinctPathToParent ?
				String.Format("{0}{1}Relation", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name) :
				String.Format("{0}{1}By{2}Relation", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name, relationSchema.Name);

			//        /// <summary>
			//        /// Gets the parent row in the Department table.
			//        /// </summary>
			//        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Gets the parent row in the {0} table.", relationSchema.ParentTable.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
            this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(DebuggerNonUserCodeAttribute))));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.Type = new CodeTypeReference(propertyTypeName);
			this.Name = relationSchema.IsDistinctPathToParent ? propertyName : String.Format("{0}By{1}", propertyName, relationSchema.Name);

			//        public DepartmentRow DepartmentRow {
			//            get {
			//                try {
			//                    return ((DepartmentRow)(this.GetParentRow(this.tableEmployee.DepartmentEmployeeRelation)));
			//                }
			//                finally {
			//                }
			//            }
			CodeTryCatchFinallyStatement getTryStatement = new CodeTryCatchFinallyStatement();
			CodeExpression parentRelationExpression = new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), tableFieldName), relationName);
			this.GetStatements.Add(
				new CodeMethodReturnStatement(
					new CodeCastExpression(
						propertyTypeName,
						new CodeMethodInvokeExpression(
							new CodeThisReferenceExpression(),
							"GetParentRow",
							new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), tableFieldName), relationFieldName)))));

			//        }

		}

	}

}
