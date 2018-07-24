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
	public class TenantNotLoadedFault
	{

		/// <summary>
		/// The target table where the fault occurred.
		/// </summary>
		[DataMember]
		String tenantNameField;

		/// <summary>
		/// The target table where the fault occurred.
		/// </summary>
		public String TenantName
		{
			get
			{
				return this.tenantNameField;
			}
		}

		/// <summary>
		/// Initialize a new instance of the TenantNotLoadedFault.
		/// </summary>
		/// <param name="key">The unique key of the record that faulted.</param>
		/// <param name="tableName">The target name of the table where the fault occurred.</param>
		public TenantNotLoadedFault(String tenantName)
		{

			// Initialize the object
			this.tenantNameField = tenantName;

		}

	}

}
