namespace Teraque
{

	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// A fault that occurs when locking system is screwed up.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[DataContract]
	public class SynchronizationLockFault
	{

		/// <summary>
		/// The table where the fault occurred.
		/// </summary>
		public String TableName { get; private set; }

		/// <summary>
		/// Initialize a new instance of the SynchronizationLockFault class.
		/// </summary>
		/// <param name="tableName">The table where the fault occurred.</param>
		public SynchronizationLockFault(String tableName)
		{

			// Initialize the object.
			this.TableName = tableName;

		}

	}

}
