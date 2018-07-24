namespace Teraque.DataModelGenerator.TableClass
{

	using System;
	using System.CodeDom;
	using System.Data;

    /// <summary>
	/// Creates a field that holds the column.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ColumnField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds the column.
		/// </summary>
		/// <param name="columnSchema">A description of the column.</param>
		public ColumnField(ColumnSchema columnSchema)
		{

			//        // The DepartmentId Column
			//        private global::System.Data.DataColumn columnDepartmentId;
			this.Type = new CodeGlobalTypeReference(typeof(DataColumn));
			this.Name = String.Format("column{0}", columnSchema.Name);

		}

	}

}
