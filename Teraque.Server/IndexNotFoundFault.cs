namespace Teraque
{

	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// A fault that occurs when a named index doesn't exist on the target table.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[DataContract]
	public class IndexNotFoundFault
	{

		/// <summary>
		/// The index of the fault.
		/// </summary>
		[DataMember]
		public String IndexName { get; private set; }

		/// <summary>
		/// The target table name of the fault.
		/// </summary>
		[DataMember]
		public String TableName { get; private set; }

		/// <summary>
		/// Initializes a new instance of the IndexNotFoundFault class.
		/// </summary>
		/// <param name="tableName">The name of the table which was the target of the index operation.</param>
		/// <param name="indexName">The name of the index which was the target of the index operation.</param>
		public IndexNotFoundFault(String tableName, String indexName)
		{

			// Initialize the object.
			this.TableName = tableName;
			this.IndexName = indexName;

		}

	}

}
