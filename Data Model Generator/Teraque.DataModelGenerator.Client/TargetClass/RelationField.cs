namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;
	using System.Data;

	/// <summary>
	/// Creates a field that holds the relationship between two tables.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class RelationField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds the relationship between two tables.
		/// </summary>
		public RelationField(RelationSchema relationSchema)
		{

			//        // Relates the Employee table to the ProjectMember table.
			//        private static global::System.Data.DataRelation relationEmployeeProjectMember;
			this.Attributes = MemberAttributes.Private | MemberAttributes.Static;
			this.Type = new CodeGlobalTypeReference(typeof(DataRelation));
			this.Name = relationSchema.IsDistinctPathToParent ?
				String.Format("relation{0}{1}", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name) :
				String.Format("relation{0}{1}By{2}", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name, relationSchema.Name);

		}

	}

}
