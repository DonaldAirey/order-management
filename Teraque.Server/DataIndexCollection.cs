namespace Teraque
{

	using System;
	using System.Data;
	using System.Diagnostics.CodeAnalysis;
	using System.Collections;

	/// <summary>
	/// Represents a collection of Teraque.Index objects
	/// </summary>
	[SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
	public class DataIndexCollection : InternalDataCollectionBase
	{

		// Private Instance Fields
		ArrayList arrayList;
		Hashtable hashTable;

		/// <summary>
		/// Create a collection of Teraque.Index objects.
		/// </summary>
		public DataIndexCollection()
		{

			// Initialize the object.
			this.arrayList = new ArrayList();
			this.hashTable = new Hashtable();

		}

		/// <summary>
		/// Gets the number of elements actually contained in the Teraque.DataIndexCollection.
		/// </summary>
		public override Int32 Count
		{
			get
			{
				return arrayList.Count;
			}
		}

		/// <summary>
		/// Gets a list of Teraque.DataIndex objects.
		/// </summary>
		protected override ArrayList List
		{
			get
			{
				return this.arrayList;
			}
		}

		/// <summary>
		/// Adds a Teraque.DataIndex to the collection.
		/// </summary>
		/// <param name="dataIndex">The data index to be added to the collection.</param>
		public void Add(DataIndex dataIndex)
		{

			// Validate the argument before using it.
			if (dataIndex == null)
				throw new ArgumentNullException("dataIndex");

			// The list is used for iteration, the table is used for fast access.
			this.arrayList.Add(dataIndex);
			this.hashTable.Add(dataIndex.IndexName, dataIndex);

		}

		/// <summary>
		/// Copy the collection to an array.
		/// </summary>
		/// <param name="array">The desintation array.</param>
		/// <param name="index">The starting index for the copy operation.</param>
		public void CopyTo(DataIndex[] array, int index)
		{
			arrayList.CopyTo(array, index);
		}

		/// <summary>
		/// Removes a Teraque.DataIndex from the collection.
		/// </summary>
		/// <param name="dataIndex">The index to be removed from the collection.</param>
		public void Remove(DataIndex dataIndex)
		{

			// Validate the argument before using it.
			if (dataIndex == null)
				throw new ArgumentNullException("dataIndex");

			// The list is used for iteration, the table is used for fast access.
			this.arrayList.Remove(dataIndex);
			this.hashTable.Remove(dataIndex.IndexName);

		}

		/// <summary>
		/// Returns an enumerator for the entire Teraque.DataIndexCollection.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the collection.</returns>
		public override IEnumerator GetEnumerator()
		{
			return this.arrayList.GetEnumerator();
		}

		/// <summary>
		/// Gets the Teraque.DataIndex at the specified index.
		/// </summary>
		/// <param name="index">An index into the collection.</param>
		/// <returns>The Teraque.DataIndex at the given index.</returns>
		public DataIndex this[Int32 index]
		{
			get { return (DataIndex)this.arrayList[index]; }
		}

		/// <summary>
		/// Gets the Teraque.DataIndex having the specified name.
		/// </summary>
		/// <param name="index">The name of the Teraque.DataIndex.</param>
		/// <returns>The Teraque.DataIndex at the given index.</returns>
		public DataIndex this[String index]
		{
			get { return (DataIndex)this.hashTable[index]; }
		}

	}

}
