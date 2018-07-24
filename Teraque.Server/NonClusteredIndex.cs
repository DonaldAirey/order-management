namespace Teraque
{

	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;

	/// <summary>
	/// Used to find records in a table using one or more values as a key.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
	public class NonClusteredIndex : DataIndex
	{

		/// <summary>
		/// This table maps the data row to the index row and is the only way to find an index after the rows has been deleted.
		/// </summary>
		Dictionary<DataRow, DataRow> mapTable;

		/// <summary>
		/// Holds the leaf element of the index that points back to the original data row.
		/// </summary>
		DataColumn dataColumn;

		/// <summary>
		/// The parent DataTable.
		/// </summary>
		DataTable parentTable;
	
		/// <summary>
		/// The columns of the index in the parent table.
		/// </summary>
		DataColumn[] parentColumns;

		/// <summary>
		/// Holds the key elements of this index and a leaf element pointing to the original data row.
		/// </summary>
		DataTable dataTable;

		/// <summary>
		/// Initialize a new instance of the NonClusteredIndex class.
		/// </summary>
		/// <param name="indexName">The name of the index.</param>
		/// <param name="parentColumns">The columns in the index.</param>
		public NonClusteredIndex(String indexName, DataColumn[] parentColumns)
			: base(indexName)
		{

			// Validate the 'parentColumns' argument.
			if (parentColumns == null || parentColumns.Length == 0)
				throw new ArgumentNullException("parentColumns");

			// This dictionary is used to quickly find the index row using the parent row as a key when the parent row is updated, committed, rolled back or
			// deleted.  This table is critical for deleted rows as there are no other ways to map the deleted data row back to the index row.
			this.mapTable = new Dictionary<DataRow, DataRow>();

			// These event handlers will keep the index synchronized to the parent table.
			this.parentTable = parentColumns[0].Table;
			parentTable.TableCleared += new DataTableClearEventHandler(this.OnParentTableCleared);
			parentTable.RowChanged += new DataRowChangeEventHandler(this.OnParentTableRowChanged);
			parentTable.RowDeleted += new DataRowChangeEventHandler(this.OnParentTableRowDeleted);

			// A table is constructed to hold the key elements of the index.  The primary key of this table is used to find the parent DataRow using a given key.
			this.dataTable = new DataTable();
			this.dataTable.Locale = this.parentTable.Locale;
			this.parentColumns = parentColumns;

			// The table that holds the non-clustered index is built from an exact copy of the parent's version of the index columns.
			DataColumn[] childColumns = new DataColumn[parentColumns.Length];
			for (int columnIndex = 0; columnIndex < parentColumns.Length; columnIndex++)
			{
				childColumns[columnIndex] = new DataColumn();
				childColumns[columnIndex].AllowDBNull = parentColumns[columnIndex].AllowDBNull;
				childColumns[columnIndex].ColumnName = parentColumns[columnIndex].ColumnName;
				childColumns[columnIndex].DataType = parentColumns[columnIndex].DataType;
				childColumns[columnIndex].DateTimeMode = parentColumns[columnIndex].DateTimeMode;
				childColumns[columnIndex].DefaultValue = parentColumns[columnIndex].DefaultValue;
				childColumns[columnIndex].Expression = parentColumns[columnIndex].Expression;
				childColumns[columnIndex].MaxLength = parentColumns[columnIndex].MaxLength;
				childColumns[columnIndex].Unique = parentColumns[columnIndex].Unique;
			}
			this.dataTable.Columns.AddRange(childColumns);

			// This is the leaf element that points back to the original data row.
			this.dataColumn = new DataColumn();
			this.dataColumn.AllowDBNull = false;
			this.dataColumn.ColumnName = "DataRow";
			this.dataColumn.DataType = typeof(DataRow);
			this.dataTable.Columns.Add(this.dataColumn);

			// The non-clustered index must be unique on the columns that make up the index.
			this.dataTable.Constraints.Add(new UniqueConstraint(indexName, childColumns, true));

		}

		/// <summary>
		/// Gets the parent table of this index.
		/// </summary>
		public DataTable Table
		{
			get
			{
				return this.parentTable;
			}
		}

		/// <summary>
		/// Clears the index when the parent table is cleared.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnParentTableCleared(object sender, DataTableClearEventArgs e)
		{

			// This will synchronize the index with the parent table when the parent is cleared.
			this.dataTable.Clear();
			this.mapTable.Clear();

		}

		/// <summary>
		/// Handles changes to the parent row.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnParentTableRowChanged(object sender, DataRowChangeEventArgs e)
		{

			// This will reconcile the ancillary index table to the parent table.
			switch (e.Action)
			{

			case DataRowAction.Add:

				// An index is not created when the key elements are null.  This is ANSI standard, not to be confused with the Microsoft SQL Server 2005 "Standard"
				// which doesn't allow unique indices with nulls.
				foreach (DataColumn parentColumn in this.parentColumns)
					if (e.Row[parentColumn] is DBNull)
						return;

				// Construct and add an element to the index that allows the target row to be quickly found using the key elements.
				DataRow addedRow = this.dataTable.NewRow();
				for (int columnIndex = 0; columnIndex < this.parentColumns.Length; columnIndex++)
					addedRow[columnIndex] = e.Row[this.parentColumns[columnIndex]];
				addedRow[this.dataColumn] = e.Row;
				this.dataTable.Rows.Add(addedRow);

				// This hash table is used to map the parent row back to the index row.  It is used to quickly reconcile the index to the parent when changes are
				// made to the parent.
				this.mapTable.Add(e.Row, addedRow);

				break;

			case DataRowAction.Change:

				// This will synchronize the changes in the parent with the non-clustered index.
				DataRow changedRow;
				if (this.mapTable.TryGetValue(e.Row, out changedRow))
					for (int columnIndex = 0; columnIndex < this.parentColumns.Length; columnIndex++)
						changedRow[columnIndex] = e.Row[this.parentColumns[columnIndex]];

				break;

			case DataRowAction.Commit:

				// This will commit any changes made when the parent row changed.  Note that the mapping element is removed when a deleted row is committed.
				DataRow committedRow;
				if (this.mapTable.TryGetValue(e.Row, out committedRow))
				{
					committedRow.AcceptChanges();
					if (e.Row.RowState == DataRowState.Detached)
						this.mapTable.Remove(e.Row);
				}

				break;

			case DataRowAction.Rollback:

				// Any changes made to the index are rolled back when the parent row is rolled back.  Once the row is deleted it should be purged from the mapping 
				// table so we don't leave clutter around.
				DataRow rollbackRow;
				if (this.mapTable.TryGetValue(e.Row, out rollbackRow))
				{
					rollbackRow.RejectChanges();
					if (e.Row.RowState == DataRowState.Detached)
						this.mapTable.Remove(e.Row);
				}

				break;

			}

		}

		/// <summary>
		/// Handles a deletion of a parent row.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnParentTableRowDeleted(object sender, DataRowChangeEventArgs e)
		{

			// This will reconcile the ancillary index table to the parent table.
			switch (e.Action)
			{

			case DataRowAction.Delete:

				// Delete the index row when the parent is deleted.  Note that the row will remain part of the index until the changes are committed.  Also note
				// that the mapping element is not removed here but when the deleted index row is committed.  Make sure we clean up the mapping table also now that the
				// row has been removed.
				DataRow deletedRow;
				if (this.mapTable.TryGetValue(e.Row, out deletedRow))
				{
					deletedRow.Delete();
					this.mapTable.Remove(e.Row);
				}

				break;

			}

		}

		/// <summary>
		/// Finds a row in the table containing the key elements.
		/// </summary>
		/// <param name="key">The identifier element of the key.</param>
		/// <returns>A row that contains the key elements, or null if there is no match.</returns>
		public override DataRow Find(object key)
		{
			try
			{

				// Use the non-clustered index to find the row based on the key values.
				DataRow dataRow = this.dataTable.Rows.Find(key);
				if (dataRow == null)
					return null;
				return (DataRow)dataRow[this.dataColumn];

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
		/// Finds a row in the table containing the key elements.
		/// </summary>
		/// <param name="keys">The identifier element of the key.</param>$
		/// <returns>A row that contains the key elements, or null if there is no match.</returns>
		public override DataRow Find(object[] keys)
		{
			try
			{

				// Use the non-clustered index to find the row based on the key values.
				DataRow dataRow = this.dataTable.Rows.Find(keys);
				if (dataRow == null)
					return null;
				return (DataRow)dataRow[this.dataColumn];

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
	}
}

