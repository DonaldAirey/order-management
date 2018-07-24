namespace Teraque.AssetNetwork
{

	using System;
	using System.ComponentModel;

	/// <summary>
	/// Information about each endpoint that subscribes to data from the simulatorl
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal class TenantInfo : ClientInfo
	{

		/// <summary>
		/// The connection string used to connect a tenant to the database server.
		/// </summary>
		String connectionStringField;

		/// <summary>
		/// The name of the tenant.
		/// </summary>
		String nameField;

		/// <summary>
		/// Describes how the tenant is started with the monitor loads.
		/// </summary>
		StartupType startupTypeField;

		/// <summary>
		/// Describes the status of the tenant.
		/// </summary>
		Status statusField;

		/// <summary>
		/// The connection string used to connect a tenant to the database server.
		/// </summary>
		public String ConnectionString
		{
			get
			{
				return this.connectionStringField;
			}
			set
			{
				if (this.connectionStringField != value)
				{
					this.connectionStringField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("ConnectionString"));
				}
			}
		}

		/// <summary>
		/// The connection string used to connect a tenant to the database server.
		/// </summary>
		public String Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				if (this.nameField != value)
				{
					this.nameField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Name"));
				}
			}
		}

		/// <summary>
		/// Describes how the tenant is started with the monitor loads.
		/// </summary>
		public StartupType StartupType
		{
			get
			{
				return this.startupTypeField;
			}
			set
			{
				if (this.startupTypeField != value)
				{
					this.startupTypeField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("StartupType"));
				}
			}
		}

		/// <summary>
		/// Describes the status of the tenant.
		/// </summary>
		public Status Status
		{
			get
			{
				return this.statusField;
			}
			set
			{
				if (this.statusField != value)
				{
					this.statusField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Status"));
				}
			}
		}

	}

}
