namespace Teraque
{

    using System;
    using System.Data;

	/// <summary>
	/// A common class for looking up data in an index.
	/// </summary>
	/// <copyright>Copyright © 2010 - 2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public abstract class DataIndex
	{

		/// <summary>
		/// The name of the index.
		/// </summary>
		public String IndexName { get; private set; }

		/// <summary>
		/// Initializes a new instance of a DataIndex class.
		/// </summary>
		/// <param name="indexName">The name of the index.</param>
		protected DataIndex(String indexName)
		{

			// Initialize the object
			this.IndexName = indexName;

		}

		/// <summary>
		/// Determines whether two DataIndex objects have the same value.
		/// </summary>
		/// <param name="obj">The DataIndex to compare to this instance.</param>
		/// <returns>true if obj is a String and its value is the same as this instance; otherwise, false.</returns>
		public override bool Equals(Object obj)
		{

			// Compare the index name when comparing to an equivalent type.
			DataIndex dataIndex = obj as DataIndex;
			if (dataIndex != null)
				return this.IndexName.Equals(dataIndex.IndexName);

			// There is no equality to other types.
			return false;

		}

		/// <summary>
		/// Gets the row specified by the primary key value.
		/// </summary>
		/// <param name="key">A single key value.</param>
		/// <returns>A DataRow that contains the primary key value specified; otherwise a null value if the primary key value does not exist in the
		/// DataRowCollection.</returns>
		public abstract DataRow Find(Object key);

		/// <summary>
		/// Gets the row that contains the specified primary key values.
		/// </summary>
		/// <param name="keys">An array of primary key values to find. The type of the array is Object.</param>
		/// <returns>A DataRow object that contains the primary key values specified; otherwise a null value if the primary key value does not exist in the
		/// DataRowCollection.</returns>
		public abstract DataRow Find(Object[] keys);

		/// <summary>
		/// Returns the hash code for this DataIndex.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return this.IndexName.GetHashCode();
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override String ToString()
		{
			return this.IndexName;
		}

	}

}

