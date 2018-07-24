namespace Teraque.AssetNetwork
{

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Teraque.AssetNetwork.WebService;
    using Teraque.AssetNetwork.MarketConsole.Properties;

    /// <summary>
    /// The Server Tab of the Console.
    /// </summary>
    /// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
    internal class ServerTab : TabItem
    {

        /// <summary>
        /// Initializes a new instance of the ServerTab class.
        /// </summary>
        public ServerTab()
        {
            // These command bindings are connected to the context menu that shows up when a tenant is right-clicked.
            this.CommandBindings.Add(new CommandBinding(Commands.Refresh, this.OnRefresh));
            this.CommandBindings.Add(new CommandBinding(Commands.Restart, this.OnRestart, this.OnCanStart));
            this.CommandBindings.Add(new CommandBinding(Commands.Start, this.OnStart, this.OnCanStart));
            this.CommandBindings.Add(new CommandBinding(Commands.Stop, this.OnStop, this.OnCanStop));

            // This provides a data context for the tab.
            this.DataContext = MainWindow.Tenants;

            // This will contact the server to update the status of each of the tenants (but don't tryin this while designing because it attempts to use the web
            // service).
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                this.RefreshDisplay();
        }

        /// <summary>
        /// Determines if the tenant can be started.
        /// </summary>
        /// <param name="sender">The Object that created this event.</param>
        /// <param name="canExecuteRoutedEventArgs">The event arguments.</param>
        void OnCanStart(Object sender, CanExecuteRoutedEventArgs canExecuteRoutedEventArgs)
        {

            // The tenant can be started only if it is not running.
            ListViewItem listViewItem = canExecuteRoutedEventArgs.OriginalSource as ListViewItem;
            if (listViewItem != null)
            {
                TenantInfo tenantInfo = listViewItem.DataContext as TenantInfo;
                canExecuteRoutedEventArgs.CanExecute = tenantInfo.Status == Status.Stopped;
            }

        }

        /// <summary>
        /// Determines if the tenant can be started.
        /// </summary>
        /// <param name="sender">The Object that created this event.</param>
        /// <param name="canExecuteRoutedEventArgs">The event arguments.</param>
        void OnCanStop(Object sender, CanExecuteRoutedEventArgs canExecuteRoutedEventArgs)
        {

            // The tenant can be stopped if it is running.
            ListViewItem listViewItem = canExecuteRoutedEventArgs.OriginalSource as ListViewItem;
            if (listViewItem != null)
            {
                TenantInfo tenantInfo = listViewItem.DataContext as TenantInfo;
                canExecuteRoutedEventArgs.CanExecute = tenantInfo.Status == Status.Running;
            }

        }

        /// <summary>
        /// Refreshes the display.
        /// </summary>
        /// <param name="sender">The Object that created this event.</param>
        /// <param name="executedRoutedEventArgs">The event arguments.</param>
        void OnRefresh(Object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {

            // After loading or unloading, we need to update the status of the display.
            this.RefreshDisplay();

        }

        /// <summary>
        /// Restarts a tenant.
        /// </summary>
        /// <param name="sender">The Object that created this event.</param>
        /// <param name="executedRoutedEventArgs">The event arguments.</param>
        void OnRestart(Object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {

            // The tenant that is to be started can be extracted from the original source of this command.
            ListViewItem listViewItem = executedRoutedEventArgs.OriginalSource as ListViewItem;
            TenantInfo tenantInfo = listViewItem.DataContext as TenantInfo;

            // A user with site administration claims is the only user who can query the status of the tenants and start and stop them.  Here, we are going to get 
            // the status of the tenants and start the ones who need to be started automatically (and are not already running).
            using (WebServiceClient administratorClient = new WebServiceClient(MainWindow.Tenants.AdministratorClientInfo.EndpointConfigurationName))
            {

                // This provides the credentials for the site administrator.
                administratorClient.ClientCredentials.UserName.UserName = MainWindow.Tenants.AdministratorClientInfo.UserName;
                administratorClient.ClientCredentials.UserName.Password = MainWindow.Tenants.AdministratorClientInfo.Password;

                // This loads the tenant into the data model using the site administrator's credentials.  Only a site administrator has the permissions to
                // load and unload tenants.
                administratorClient.UnloadTenant(tenantInfo.Name);
                administratorClient.LoadTenant(tenantInfo.Name, tenantInfo.ConnectionString);

                // This uses the tenant operator's credentials to initialize the tenant.  The server itself doesn't have the ability to cross tenant boundaries as a
                // security measure, so the initialization must be done outside where the credentials can be kept secure.
                using (WebServiceClient tenantClient = new WebServiceClient(tenantInfo.EndpointConfigurationName))
                {
                    tenantClient.ClientCredentials.UserName.UserName = tenantInfo.UserName;
                    tenantClient.ClientCredentials.UserName.Password = tenantInfo.Password;
                    tenantClient.Start();
                }

            }

            // After loading or unloading, we need to update the status of the display.
            this.RefreshDisplay();

        }

        /// <summary>
        /// Starts a tenant.
        /// </summary>
        /// <param name="sender">The Object that created this event.</param>
        /// <param name="executedRoutedEventArgs">The event arguments.</param>
        void OnStart(Object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {

            // The tenant that is to be started can be extracted from the original source of this command.
            ListViewItem listViewItem = executedRoutedEventArgs.OriginalSource as ListViewItem;
            TenantInfo tenantInfo = listViewItem.DataContext as TenantInfo;

            // A user with site administration claims is the only user who can query the status of the tenants and start and stop them.  Here, we are going to get 
            // the status of the tenants and start the ones who need to be started automatically (and are not already running).
            using (WebServiceClient administratorClient = new WebServiceClient(MainWindow.Tenants.AdministratorClientInfo.EndpointConfigurationName))
            {

                // This provides the credentials for the site administrator.
                administratorClient.ClientCredentials.UserName.UserName = MainWindow.Tenants.AdministratorClientInfo.UserName;
                administratorClient.ClientCredentials.UserName.Password = MainWindow.Tenants.AdministratorClientInfo.Password;

                // This loads the tenant into the data model using the site administrator's credentials.  Only a site administrator has the permissions to
                // load and unload tenants.
                administratorClient.LoadTenant(tenantInfo.Name, tenantInfo.ConnectionString);

                // This uses the tenant operator's credentials to initialize the tenant.  The server itself doesn't have the ability to cross tenant boundaries as a
                // security measure, so the initialization must be done outside where the credentials can be kept secure.
                using (WebServiceClient tenantClient = new WebServiceClient(tenantInfo.EndpointConfigurationName))
                {
                    tenantClient.ClientCredentials.UserName.UserName = tenantInfo.UserName;
                    tenantClient.ClientCredentials.UserName.Password = tenantInfo.Password;
                    tenantClient.Start();
                }

            }

            // After loading or unloading, we need to update the status of the display.
            this.RefreshDisplay();

        }

        /// <summary>
        /// Stops a tenant.
        /// </summary>
        /// <param name="sender">The Object that created this event.</param>
        /// <param name="executedRoutedEventArgs">The event arguments.</param>
        void OnStop(Object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {

            // The tenant that is to be stopped can be extracted from the original source of this command.
            ListViewItem listViewItem = executedRoutedEventArgs.OriginalSource as ListViewItem;
            TenantInfo tenantInfo = listViewItem.DataContext as TenantInfo;

            // A user with site administration claims is the only user who can query the status of the tenants and start and stop them.  Here, we are going to get 
            // the status of the tenants and start the ones who need to be started automatically (and are not already running).
            using (WebServiceClient administratorClient = new WebServiceClient(MainWindow.Tenants.AdministratorClientInfo.EndpointConfigurationName))
            {

                // This uses the tenant operator's credentials to initialize the tenant.  The server itself doesn't have the ability to cross tenant 
                // boundaries as a security measure, so starting the services must be done outside where the credentials can be kept secure.  Note that it 
                // may  take several seconds to a minute or two to start each tenant.  Because there is nothing to be gained by having these multithread, 
                // as we're already in the background here, and the server isn't going to appreciate the multitasking load or get the job done any faster,  
                // I've left this as a serial task.
                using (WebServiceClient tenantClient = new WebServiceClient(tenantInfo.EndpointConfigurationName))
                {
                    tenantClient.ClientCredentials.UserName.UserName = tenantInfo.UserName;
                    tenantClient.ClientCredentials.UserName.Password = tenantInfo.Password;
                    tenantClient.Stop();
                }

                // This provides the credentials for the site administrator.
                administratorClient.ClientCredentials.UserName.UserName = MainWindow.Tenants.AdministratorClientInfo.UserName;
                administratorClient.ClientCredentials.UserName.Password = MainWindow.Tenants.AdministratorClientInfo.Password;

                // This unloads the tenant from the data model using the site administrator's credentials.  Only a site administrator has the permissions to unload
                // tenants.
                administratorClient.UnloadTenant(tenantInfo.Name);

            }

            // After loading or unloading, we need to update the status of the display.
            this.RefreshDisplay();

        }

        /// <summary>
        /// Refreshes the display.
        /// </summary>
        internal void RefreshDisplay()
        {

            // A user with site administration claims is the only user who can query the status of the tenants and start and stop them.  Here, we are going to get 
            // the status of the tenants and start the ones who need to be started automatically (and are not already running).
            using (WebServiceClient administratorClient = new WebServiceClient(MainWindow.Tenants.AdministratorClientInfo.EndpointConfigurationName))
            {

                // This provides the credentials for the site administrator who is the only one that can query for the loaded tenants.
                administratorClient.ClientCredentials.UserName.UserName = MainWindow.Tenants.AdministratorClientInfo.UserName;
                administratorClient.ClientCredentials.UserName.Password = MainWindow.Tenants.AdministratorClientInfo.Password;

                try
                {
                    // This queries the server for the loaded (running) tenants.
                    List<String> loadedTenants = new List<String>(administratorClient.GetTenants());

                    // This will initialize the web services for each of the tenants in the configuration file.
                    foreach (TenantInfo tenantInfo in MainWindow.Tenants)
                        tenantInfo.Status = loadedTenants.Contains(tenantInfo.Name) ? Status.Running : Status.Stopped;

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Market Console Error");
                    Application.Current.Shutdown();
                }
            }

        }

    }

}
