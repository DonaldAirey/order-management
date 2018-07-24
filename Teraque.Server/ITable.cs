namespace Teraque
{

	using System;
	using System.Data;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Abstract interface to common features of a table.
	/// </summary>
	public interface ITable
	{

		/// <summary>
		/// Gets the collection of columns in this table.
		/// </summary>
		DataColumnCollection Columns
		{
			get;
		}

		/// <summary>
		/// The absolute index of the table within the collection of tables in a DataSet.
		/// </summary>
		int Ordinal
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the primary key of the table.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		DataColumn[] PrimaryKey
		{
			get;
		}

	}

}
