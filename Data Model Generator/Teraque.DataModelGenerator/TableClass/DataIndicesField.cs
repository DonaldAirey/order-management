namespace Teraque.DataModelGenerator.TableClass
{

    using System.CodeDom;

    /// <summary>
	/// Creates a field that holds a collection of Teraque.DataModelGenerator.DataIndex objects on a table.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class DataIndicesField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds a collection of Teraque.DataModelGenerator.DataIndex objects on the a table.
		/// </summary>
		/// <param name="tableSchema">A description of the table.</param>
		public DataIndicesField(TableSchema tableSchema)
		{

			//            // A collection of Teraque.DataModelGenerator.DataIndex objects on the Department table.
			//            private global::Teraque.DataModelGenerator.RowFilterDelegate userFilter;
			this.Type = new CodeGlobalTypeReference(typeof(DataIndexCollection));
			this.Name = "dataIndices";

		}

	}

}
