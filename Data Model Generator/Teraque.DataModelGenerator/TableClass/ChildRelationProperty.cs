namespace Teraque.DataModelGenerator.TableClass
{

	using System;
	using System.CodeDom;
	using System.Data;

	/// <summary>
	/// Creates a property that joins a child table to its parent.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ChildRelationProperty : CodeMemberProperty
	{

		/// <summary>
		/// Creates a property that joins a child table to its parent.
		/// </summary>
		/// <param name="relationSchema">A description of the relationship between two tables.</param>
		public ChildRelationProperty(RelationSchema relationSchema)
		{

			//        /// <summary>
			//        /// Gets the child relation between the Employee and Engineer tables.
			//        /// </summary>
			//        internal global::System.Data.DataRelation EmployeeEngineerRelation {
			//            get {
			//                return this.relationEmployeeEngineer;
			//            }
			//        }
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Gets the child relation between the {0} and {1} tables.", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Attributes = MemberAttributes.Assembly | MemberAttributes.Final;
			this.Type = new CodeGlobalTypeReference(typeof(DataRelation));
			this.Name = relationSchema.IsDistinctPathToChild ?
				String.Format("{0}{1}Relation", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name) :
				String.Format("{0}{1}By{2}Relation", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name, relationSchema.Name);
			string relationName = relationSchema.IsDistinctPathToChild ?
				String.Format("relation{0}{1}", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name) :
				String.Format("relation{0}{1}By{2}", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name, relationSchema.Name);
			this.GetStatements.Add(new CodeMethodReturnStatement(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), relationName)));

		}

	}

}
