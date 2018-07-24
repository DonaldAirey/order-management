namespace Teraque
{

	using System;
    using System.Data;
	using System.Globalization;

	/// <summary>
	/// Physically stores the data in the associated table with the sequence given in the index.
	/// </summary>
	/// <copyright>Copyright © 2010 - 2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ClusteredIndex : DataIndex
	{

		/// <summary>
		/// The associated table.
		/// </summary>
		DataTable ownerTable;

		/// <summary>
		/// Initializes a new instance of the ClusteredIndex class.
		/// </summary>
		/// <param name="indexName">The name of the index.</param>
		/// <param name="columns">The columns that describe a unique key.</param>
		public ClusteredIndex(String indexName, DataColumn[] columns)
			: base(indexName)
		{

			// Make sure that we can build a clustered index using the columns specified.
			if (columns == null || columns.Length == 0)
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Properties.Resources.ClusteredIndexColumnError, indexName));

			// The underlying table will use the intrinsic 'Find' function to locate records.
			this.ownerTable = columns[0].Table;
			this.ownerTable.PrimaryKey = columns;

		}

		/// <summary>
		/// Gets the DataTable to which this index belongs.
		/// </summary>
		public DataTable Table
		{
			get
			{
				return this.ownerTable;
			}
		}

		/// <summary>
		/// Gets the row specified by the primary key value.
		/// </summary>
		/// <param name="key">A single key value.</param>
		/// <returns>A DataRow that contains the primary key value specified; otherwise a null value if the primary key value does not exist in the
		/// DataRowCollection.</returns>
		public override DataRow Find(Object key)
		{

			try
			{

				// Use the primary key to locate the record.
				return this.ownerTable.Rows.Find(key);

			}
			catch (ArgumentException exception)
			{

				// Rethrow the exception with a little more information about where the error occurred.
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "{0}: {1}", this.IndexName, exception.Message));

			}
			catch (FormatException formatException)
			{

				// Rethrow the exception with a little more information about where the error occurred.
				throw new FormatException(String.Format(CultureInfo.CurrentCulture, "{0}: {1}", this.IndexName, formatException.Message));

			}
			catch (NullReferenceException nullReferenceException)
			{

				// Translate null reference exceptions into argument exceptions.
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "{0}: {1}", this.IndexName, nullReferenceException.Message));

			}

		}

		/// <summary>
		/// Gets the row that contains the specified primary key values.
		/// </summary>
		/// <param name="keys">An array of primary key values to find. The type of the array is Object.</param>
		/// <returns>A DataRow object that contains the primary key values specified; otherwise a null value if the primary key value does not exist in the
		/// DataRowCollection.</returns>
		public override DataRow Find(Object[] keys)
		{

			try
			{

				// Use the primary key to locate the record.
				return this.ownerTable.Rows.Find(keys);

			}
			catch (ArgumentException argumentException)
			{

				// Rethrow the exception with a little more information about where the error occurred.
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "{0}: {1}", this.IndexName, argumentException.Message));

			}
			catch (FormatException formatException)
			{

				// Rethrow the exception with a little more information about where the error occurred.
				throw new FormatException(String.Format(CultureInfo.CurrentCulture, "{0}: {1}", this.IndexName, formatException.Message));

			}
			catch (NullReferenceException nullReferenceException)
			{

				// Translate null reference exceptions into argument exceptions.
				throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "{0}: {1}", this.IndexName, nullReferenceException.Message));

			}

		}

	}

}

