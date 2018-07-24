namespace Teraque.AssetNetwork
{

	using System;
	using System.ServiceModel;

	/// <summary>
	/// Services for managing the data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class DataModelService
	{

		/// <summary>
		/// Gets the tenants currently loaded on this service.
		/// </summary>
		/// <returns>An array of tenant names currently loaded on this service.</returns>
		internal static String[] GetTenants()
		{

			// This is the list of all the tenants currently loaded in the data model.
			return DataModel.GetTenants();

		}

		/// <summary>
		/// Loads an tenant into the service.
		/// </summary>
		/// <param name="tenantName">The name of the tenant.</param>
		/// <param name="connectionString">A connection string used to connect the tenant to a database.</param>
		internal static void LoadTenant(String tenantName, String connectionString)
		{

			// This will load the tenant into the server.
			DataModel.LoadTenant(tenantName, connectionString);

		}

		/// <summary>
		/// Unloads an tenant from the service.
		/// </summary>
		/// <param name="tenantName">The name of the tenant.</param>
		internal static void UnloadTenant(String tenantName)
		{

			// This will unload the organziation from the server.
			DataModel.UnloadTenant(tenantName);

		}

	}

}
