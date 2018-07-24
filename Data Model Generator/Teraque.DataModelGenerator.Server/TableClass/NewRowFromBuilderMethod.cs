namespace Teraque.DataModelGenerator.TableClass
{

	using System;
	using System.CodeDom;
	using System.Data;

    /// <summary>
	/// Creates a CodeDOM description a method to handle the Row Changed event.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class NewRowFromBuilderMethod : CodeMemberMethod
	{

		/// <summary>
		/// Generates the method used to handle the Row Changed event.
		/// </summary>
		/// <param name="tableSchema">The table to which this event belongs.</param>
		public NewRowFromBuilderMethod(TableSchema tableSchema)
		{

			//            /// <summary>
			//            /// Initializes a new instance of a Department row.  Constructs a row from the builder.  Only for internal usage.
			//            /// </summary>
			//            /// <returns>A new row with the same schema as the table.</returns>
			//            protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder dataRowBuilder) {
			//                return new DepartmentRow(dataRowBuilder);
			//            }
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Initializes a new instance of a {0} row.  Constructs a row from the builder.  Only for internal usage.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Comments.Add(new CodeCommentStatement("<returns>A new row with the same schema as the table.</returns>", true));
			this.Attributes = MemberAttributes.Family | MemberAttributes.Override;
			this.ReturnType = new CodeGlobalTypeReference(typeof(DataRow));
			this.Name = "NewRowFromBuilder";
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeGlobalTypeReference(typeof(DataRowBuilder)), "dataRowBuilder"));

			//				// This creates a strongly typed Department using the generic construction methods.
			//				return new Teraque.UnitTest.Client.DataModel.DepartmentRow(dataRowBuilder);
			this.Statements.Add(new CodeMethodReturnStatement(new CodeObjectCreateExpression(String.Format("{0}Row", tableSchema.Name), new CodeArgumentReferenceExpression("dataRowBuilder"))));

		}

	}

}
