namespace Teraque.DataModelGenerator.TableClass
{

	using System;
	using System.CodeDom;
	using System.Data;

	/// <summary>
	/// Creates a CodeDOM description a method to handle the Row Deleted event.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class OnRowDeletedMethod : CodeMemberMethod
	{

		/// <summary>
		/// Generates the method used to handle the Row Deleted event.
		/// </summary>
		/// <param name="tableSchema">A description of the table.</param>
		public OnRowDeletedMethod(TableSchema tableSchema)
		{

			// Construct the type names for the table and rows within the table.
			string rowTypeName = String.Format("{0}Row", tableSchema.Name);

			//            /// <summary>
			//            /// Raises the DepartmentRowDeleted event.
			//            /// </summary>
			//            /// <param name="e">Provides data for the DepartmentRow changing and deleting events.</param>
			//            protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e) {
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Raises the {0}Deleted event.", rowTypeName), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("<param name=\"e\">Provides data for the {0} changing and deleting events.</param>", rowTypeName), true));
			this.Attributes = MemberAttributes.Family | MemberAttributes.Override;
			this.Name = "OnRowDeleted";
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeGlobalTypeReference(typeof(DataRowChangeEventArgs)), "e"));

			//                base.OnRowDeleted(e);
			//                if ((this.DepartmentRowDeleted != null)) {
			//                    this.DepartmentRowDeleted(this, new DepartmentRowChangeEvent(((DepartmentRow)(e.Row)), e.Action));
			//                }
			this.Statements.Add(new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "OnRowDeleted", new CodeArgumentReferenceExpression("e")));
			CodeConditionStatement ifEventExists = new CodeConditionStatement(new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), String.Format("{0}Deleted", rowTypeName)), CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null)));
			ifEventExists.TrueStatements.Add(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), String.Format("{0}Deleted", rowTypeName), new CodeThisReferenceExpression(), new CodeObjectCreateExpression(String.Format("{0}ChangeEventArgs", rowTypeName), new CodeCastExpression(rowTypeName, new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("e"), "Row")), new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("e"), "Action"))));
			this.Statements.Add(ifEventExists);

			//            }

		}

	}

}
