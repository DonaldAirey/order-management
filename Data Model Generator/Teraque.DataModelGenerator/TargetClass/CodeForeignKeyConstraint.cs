﻿namespace Teraque.DataModelGenerator.TargetClass
{

    using System;
    using System.CodeDom;
    using System.Collections.Generic;
	using System.Data;

	/// <summary>
	/// Create a representation of the creation of a foreign key constraint.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class CodeForeignKeyConstraint : CodeObjectCreateExpression
	{

		/// <summary>
		/// Create a representation of the creation of a foreign key constraint.
		/// </summary>
		/// <param name="relationSchema">The description of the foreign key constraint.</param>
		public CodeForeignKeyConstraint(ForeignKeyConstraintSchema foreignKeyConstraintSchema)
		{

			// Collect the key fields in the parent table.
			List<CodeExpression> parentFieldList = new List<CodeExpression>();
			foreach (ColumnSchema columnSchema in foreignKeyConstraintSchema.RelatedColumns)
				parentFieldList.Add(new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(foreignKeyConstraintSchema.RelatedTable.DataModel.Name), String.Format("table{0}", foreignKeyConstraintSchema.RelatedTable.Name)), String.Format("{0}Column", columnSchema.Name)));

			// Collect the referenced fields in the child table.
			List<CodeExpression> childFieldList = new List<CodeExpression>();
			foreach (ColumnSchema columnSchema in foreignKeyConstraintSchema.Columns)
				childFieldList.Add(new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(foreignKeyConstraintSchema.Table.DataModel.Name), String.Format("table{0}", foreignKeyConstraintSchema.Table.Name)), String.Format("{0}Column", columnSchema.Name)));

			//            new System.Data.ForeignKeyConstraint("FK_Object_Department", new Teraque.DataModelGenerator.Column[] {
			//                        Teraque.UnitTest.Server.DataModel.tableObject.ObjectIdColumn}, new Teraque.DataModelGenerator.Column[] {
			//                        Teraque.UnitTest.Server.DataModel.tableDepartment.DepartmentIdColumn});
			this.CreateType = new CodeGlobalTypeReference(typeof(ForeignKeyConstraint));
			this.Parameters.Add(new CodePrimitiveExpression(foreignKeyConstraintSchema.Name));
			this.Parameters.Add(new CodeArrayCreateExpression(new CodeGlobalTypeReference(typeof(DataColumn)), parentFieldList.ToArray()));
			this.Parameters.Add(new CodeArrayCreateExpression(new CodeGlobalTypeReference(typeof(DataColumn)), childFieldList.ToArray()));

		}

	}

}
