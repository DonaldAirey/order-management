namespace Teraque.DataModelGenerator.TableClass
{

	using System;
	using System.CodeDom;

	/// <summary>
	/// Creates a field that holds an index to a table.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class DataIndexField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds an index to a table.
		/// </summary>
		/// <param name="uniqueConstraintSchema">The description of a unique constraint.</param>
		public DataIndexField(UniqueConstraintSchema uniqueConstraintSchema)
		{

			//            // The DepartmentKey Index
			//            private UnitTest.Server.DataModel.DepartmentKeyIndex indexDepartmentKey;
			this.Type = new CodeGlobalTypeReference(typeof(DataIndex));
			this.Name = String.Format("index{0}", uniqueConstraintSchema.Name);

		}

	}

}
