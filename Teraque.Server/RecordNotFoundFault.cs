namespace Teraque
{

	using System;
	using System.Collections.ObjectModel;
	using System.Runtime.Serialization;
	using System.Text;

	/// <summary>
	/// A fault that occurs when an update is made to an already updated record.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[DataContract]
	public class RecordNotFoundFault
	{

		/// <summary>
		/// The unique key of the record where the fault occurred.
		/// </summary>
		[DataMember]
		ReadOnlyCollection<Object> keyField;

		/// <summary>
		/// The target table where the fault occurred.
		/// </summary>
		[DataMember]
		String tableNameField;

		/// <summary>
		/// The unique key of the record where the fault occurred.
		/// </summary>
		public ReadOnlyCollection<Object> Key
		{
			get
			{
				return this.keyField;
			}
		}

		/// <summary>
		/// The target table where the fault occurred.
		/// </summary>
		public String TableName
		{
			get
			{
				return this.tableNameField;
			}
		}

		/// <summary>
		/// Initialize a new instance of the RecordNotFoundFault.
		/// </summary>
		/// <param name="key">The unique key of the record that faulted.</param>
		/// <param name="tableName">The target name of the table where the fault occurred.</param>
		public RecordNotFoundFault(String tableName, Object[] key)
		{

			// Initialize the object
			this.keyField = new ReadOnlyCollection<Object>(key);
			this.tableNameField = tableName;

		}

	}

}
