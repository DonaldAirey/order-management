namespace Teraque.DataModelGenerator.TableClass
{

    using System;
    using System.CodeDom;

    /// <summary>
	/// Creates a field that holds the ordinal (absolute index) of the table in the DataSet.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class OrdinalField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds the ordinal (absolute index) of the table in the DataSet.
		/// </summary>
		public OrdinalField(TableSchema tableSchema)
		{

			//		private int ordinal;
			this.Type = new CodeGlobalTypeReference(typeof(Int32));
			this.Name = "ordinal";

		}

	}

}
