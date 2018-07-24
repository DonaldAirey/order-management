namespace Teraque
{

	using System;
	using System.Collections.ObjectModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Runtime.Serialization;
	using System.Text;

	/// <summary>
	/// A fault that occurs when an update is made to an already updated record.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[DataContract]
	public class OptimisticConcurrencyFault
	{

		/// <summary>
		/// The key elements of the record.
		/// </summary>
		[DataMember]
		Object[] keyField;

		/// <summary>
		/// The table where the optimistic concurrency fault occurred.
		/// </summary>
		[DataMember]
		String tableNameField;

		/// <summary>
		/// Create information about a failure to have the proper row version when accessing a record.
		/// </summary>
		/// <param name="tableName">The table where the optimistic concurrency fault occurred.</param>
		/// <param name="key">The key elements of the record.</param>
		public OptimisticConcurrencyFault(String tableName, Object[] key)
		{

			// Initialize the object.
			this.tableNameField = tableName;
			this.keyField = key;

		}

		/// <summary>
		/// Gets the key column names.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public Object[] Key
		{
			get
			{
				return this.keyField;
			}
		}

		/// <summary>
		/// Gets the name of the table where the optimistic concurrency fault occurred.
		/// </summary>
		public String TableName
		{
			get
			{
				return this.tableNameField;
			}
		}

	}

}
