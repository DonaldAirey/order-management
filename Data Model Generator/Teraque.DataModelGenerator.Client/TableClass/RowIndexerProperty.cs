namespace Teraque.DataModelGenerator.TableClass
{

    using System;
    using System.CodeDom;
    using Teraque;

	/// <summary>
	/// Represents a declaration of a property that gets the parent row.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class RowIndexerProperty : CodeMemberProperty
	{

		/// <summary>
		/// Generates a property to get a parent row.
		/// </summary>
		/// <param name="foreignKeyConstraintSchema">The foreign key that references the parent table.</param>
		public RowIndexerProperty(TableSchema tableSchema)
		{

			// Construct the type names for the table and rows within the table.
			CodeTypeReference rowType = new CodeTypeReference(String.Format("{0}.{1}Row", tableSchema.DataModel.Name, tableSchema.Name));

			//            /// <summary>
			//            /// Indexer to a row in the Employee table.
			//            /// </summary>
			//            /// <param name="index">The integer index of the row.</param>
			//            /// <returns>The Employee row found at the given index.</returns>
			//            public DataModel.EmployeeRow this[int index] {
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Indexer to a row in the {0} table.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Comments.Add(new CodeCommentStatement("<param name=\"index\">The integer index of the row.</param>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("<returns>The {0} row found at the given index.</returns>", tableSchema.Name), true));
			this.CustomAttributes.AddRange(new CodeCustomAttributesForProperties());
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.Type = rowType;
			this.Name = "Item";
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeGlobalTypeReference(typeof(Int32)), "index"));

			//                get {
			//                    return ((DataModel.EmployeeRow)(this.Rows[index]));
			//                }
			this.GetStatements.Add(
				new CodeMethodReturnStatement(
					new CodeCastExpression(rowType, new CodeIndexerExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Rows"), new CodeArgumentReferenceExpression("index")))));

			//            }

		}

	}

}
