namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;

    /// <summary>
	/// Creates a property to get a data table from the data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class TableProperty : CodeMemberProperty
	{

		/// <summary>
		/// Creates a property to get a data table from the data model.
		/// </summary>
		/// <param name="tableSchema">A description of the table.</param>
		public TableProperty(TableSchema tableSchema)
		{

			//        /// <summary>
			//        /// Gets an accessor for the Employee table.
			//        /// </summary>
			//        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//        [global::System.ComponentModel.BrowsableAttribute(false)]
			//        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
			//        public static EmployeeDataTable Employee {
			//            get {
			//                return Teraque.UnitTest.Server.DataModel.tableEmployee;
			//            }
			//        }
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Gets an accessor for the {0} table.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.AddRange(new CodeCustomAttributesForProperties());
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
			this.Type = new CodeTypeReference(String.Format("{0}DataTable", tableSchema.Name));
			this.Name = tableSchema.Name;
			this.GetStatements.Add(
				new CodeMethodReturnStatement(new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(tableSchema.DataModel.Name), String.Format("table{0}", tableSchema.Name))));

		}

	}
}
