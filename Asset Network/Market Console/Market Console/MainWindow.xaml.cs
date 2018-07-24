namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel.Design;
	using System.Threading;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using Teraque.AssetNetwork.WebService;
	using Teraque.AssetNetwork.MarketConsole.Properties;

	/// <summary>
	/// Main Window for the Simulator Console.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class MainWindow : Window
	{

		/// <summary>
		/// The tenants managed by this console.
		/// </summary>
		static TenantCollection tenants = new TenantCollection();

		/// <summary>
		/// Main window of the Market Console.
		/// </summary>
		public MainWindow()
		{

			// The IDE managed resources are initalized here.
			this.InitializeComponent();

			// This will initialize each of the tenants in the web service.
			if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
				ThreadPool.QueueUserWorkItem(this.InitializeTenants);

		}

		/// <summary>
		/// Initialize each of the tenants in the configuration.
		/// </summary>
		void InitializeTenants(Object state)
		{

			// This is a list of the tenants which must be initialized when this console loads.  Note that this is NOT the same list as the static list that is
			// shared by the foreground threads.  This list is intended to be used privatley by this background thread.
			TenantCollection tenants = new TenantCollection();
	
			// A user with site administration claims is the only user who can query the status of the tenants and start and stop them.  Here, we are going to get 
			// the status of the tenants and start the ones who need to be started automatically (and are not already running).
			using (WebServiceClient administratorClient = new WebServiceClient(tenants.AdministratorClientInfo.EndpointConfigurationName))
			{

				// This establishes the credentials for the site administrator.
				administratorClient.ClientCredentials.UserName.UserName = tenants.AdministratorClientInfo.UserName;
				administratorClient.ClientCredentials.UserName.Password = tenants.AdministratorClientInfo.Password;

				// This will obtain a collection of the current tenants on the server.
				List<String> loadedTenants = new List<String>(administratorClient.GetTenants());

				// This will initialize the web services for each of the tenants in the configuration file.
				foreach (TenantInfo tenantInfo in tenants)
				{

					// If the tenant is not currently loaded and the startup type indicates that it should be automatically loaded, then we're going to load the
					// tenant and initialize it.
					if (!loadedTenants.Contains(tenantInfo.Name) && tenantInfo.StartupType == StartupType.Automatic)
					{

						// This loads the tenant into the data model using the site administrator's credentials.  Only a site administrator has the permissions to
						// load and unload tenants.
						administratorClient.LoadTenant(tenantInfo.Name, tenantInfo.ConnectionString);

						// This uses the tenant operator's credentials to initialize the tenant.  The server itself doesn't have the ability to cross tenant 
						// boundaries as a security measure, so starting the services must be done outside where the credentials can be kept secure.  Note that it 
						// may  take several seconds to a minute or two to start each tenant.  Because there is nothing to be gained by having these multithread, 
						// as we're already in the background here, and the server isn't going to appreciate the multitasking load or get the job done any faster,  
						// I've left this as a serial task.
						using (WebServiceClient tenantClient = new WebServiceClient(tenantInfo.EndpointConfigurationName))
						{
							tenantClient.ClientCredentials.UserName.UserName = tenantInfo.UserName;
							tenantClient.ClientCredentials.UserName.Password = tenantInfo.Password;
							tenantClient.Start();
						}

					}

				}

			}

			// When all the tenants are loaded, invoke the foreground to update the display with the new status.
			this.Dispatcher.BeginInvoke(new Action(() => { this.ServerTab.RefreshDisplay(); }));

		}

		/// <summary>
		/// The collection of tenants managed by this console.
		/// </summary>
		internal static TenantCollection Tenants
		{
			get
			{
				return MainWindow.tenants;
			}
		}

	}

}
