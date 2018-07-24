namespace Teraque.DataModelGenerator.TableClass
{

	using System;
	using System.CodeDom;
	using System.Data;

    /// <summary>
	/// Creates a field that holds the relation to a parent table.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ParentRelationField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds the relation to a parent table.
		/// </summary>
		/// <param name="relationSchema">The description of a relationship between two tables.</param>
		public ParentRelationField(RelationSchema relationSchema)
		{

			//            // The Relation between the Department and Employee tables
			//            private global::System.Data.DataRelation relationDepartmentEmployee;
			this.Type = new CodeGlobalTypeReference(typeof(DataRelation));
			this.Name = relationSchema.IsDistinctPathToParent ?
				String.Format("relation{0}{1}", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name) :
				String.Format("relation{0}{1}By{2}", relationSchema.ParentTable.Name, relationSchema.ChildTable.Name, relationSchema.Name);

		}

	}

}
