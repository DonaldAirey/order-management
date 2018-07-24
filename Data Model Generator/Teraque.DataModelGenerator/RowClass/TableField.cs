namespace Teraque.DataModelGenerator.RowClass
{

	using System;
    using System.CodeDom;

    /// <summary>
	/// Represents a declaration of a field used to reference the table that owns this row.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class TableField : CodeMemberField
	{

		/// <summary>
		/// Represents a declaration of a field used to reference the table that owns this row.
		/// </summary>
		/// <param name="tableSchema">The table that owns this row.</param>
		public TableField(TableSchema tableSchema)
		{

			//            // The parent Employee table.
			//            private EmployeeDataTable tableEmployee;
			this.Type = new CodeTypeReference(String.Format("{0}DataTable", tableSchema.Name));
			this.Name = String.Format("table{0}", tableSchema.Name);

		}

	}

}
