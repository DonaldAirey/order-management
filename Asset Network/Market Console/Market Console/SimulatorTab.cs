namespace Teraque.AssetNetwork
{

    using System;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Teraque;
    using Teraque.AssetNetwork.MarketService;
    using Teraque.AssetNetwork.WebService;

    /// <summary>
    /// The Simulator Tab of the Console.
    /// </summary>
    /// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
    internal class SimulatorTab : TabItem
    {

        /// <summary>
        /// Used to set the simulator parameters in the dialog from the background thread.
        /// </summary>
        /// <param name="simulatorParameters">The simulator parameters.</param>
        delegate void SetSimulatorParametersHandler(SimulatorParameters simulatorParameters);

        /// <summary>
        /// Initialize a new instance of the SimulatorTab class.
        /// </summary>
        public SimulatorTab()
        {

            // This connects the commands to handlers for those commands.  It's done here because this is a tab.  Normally you would have the command handlers
            // embedded in the XAML, but that arrangment will only route commands to the main window.  Since the main window was getting crowded, I moved
            // the logic into each of the tabs which simplified the MainWindow, but required a little extra processing.
            this.CommandBindings.Add(new CommandBinding(Commands.Apply, this.OnApply));
            this.CommandBindings.Add(new CommandBinding(Commands.ClearOrders, this.OnClearOrders));

            // The initialization of the window requires using the background to query the simulator (which runs as a seperate, independant web service).  The
            // background will then use the dispatcher to pass the results back to the foreground.  Because we require a foreground in order to complete this
            // step, we will wait until the window has been loaded to ask the server for the simulation parameters.
            this.Loaded += this.OnLoaded;

        }

        /// <summary>
        /// Clears the orders and executions from the server.
        /// </summary>
        void ClearOrders(Object state)
        {

            // The simulator needs to be cleaned of orders so it doesn't generate an execution on an order that's been deleted by the server.
            using (MarketServiceClient marketServiceClient = new MarketServiceClient())
                marketServiceClient.ClearOrders();

            // This will clear any of the orders on the server.  The main idea is to allow a replay of a demo scenario.  This is never intended to be part of an
            // actual production environment.
            foreach (TenantInfo tenantInfo in MainWindow.Tenants)
                if (tenantInfo.Status == Status.Running)
                {
                    WebServiceClient webServiceClient = new WebServiceClient(tenantInfo.EndpointConfigurationName);
                    webServiceClient.ClientCredentials.UserName.UserName = tenantInfo.UserName;
                    webServiceClient.ClientCredentials.UserName.Password = tenantInfo.Password;
                    webServiceClient.ClearOrders();
                }

        }

        /// <summary>
        /// Initializes the simulation parameters.
        /// </summary>
        /// <param name="state">The generic thread start parameter.</param>
        void InitializeData(Object state)
        {
            SimulatorParameters simulatorParameters = null;

            // This will ask the simulator for the current set of operating parameters
            using (MarketServiceClient marketServiceClient = new MarketServiceClient())
            {
                try
                {
                    simulatorParameters = marketServiceClient.GetSimulatorParameters();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Market Console Error");
                    Application.Current.Shutdown();
                }
            }

            // Pass the simulator parameters to the foreground to be used as the initialization settings for the management dialog.
            this.Dispatcher.BeginInvoke(
                DispatcherPriority.SystemIdle,
                new SetSimulatorParametersHandler(this.OnSetSimulatorParameters),
                simulatorParameters);
        }

        /// <summary>
        /// Starts a tenant.
        /// </summary>
        /// <param name="sender">The Object that created this event.</param>
        /// <param name="executedRoutedEventArgs">The event arguments.</param>
        void OnApply(Object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {

            // Extract the parameters from the data context and send to the background to be reconciled with the simulator's settings.
            ThreadPool.QueueUserWorkItem(this.SetSimulatorParameter, this.DataContext);

        }

        /// <summary>
        /// Clears the orders and executions from the server.
        /// </summary>
        /// <param name="sender">The Object that created this event.</param>
        /// <param name="executedRoutedEventArgs">The event arguments.</param>
        void OnClearOrders(Object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {

            // Extract the parameters from the data context and send to the background to be reconciled with the simulator's settings.
            ThreadPool.QueueUserWorkItem(this.ClearOrders);

        }

        /// <summary>
        /// Occurs when the element is laid out, rendered, and ready for interaction.
        /// </summary>
        /// <param name="sender">The Object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        void OnLoaded(Object sender, RoutedEventArgs e)
        {

            // This will ask the web service for the simulation parameters in the background.  This will take a while so we're obviously not going to make the
            // foreground wait and we don't want to run anything that involves web services while designing.
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                ThreadPool.QueueUserWorkItem(this.InitializeData);

        }

        /// <summary>
        /// Handles the reconciliation of the dialog controls to the settings from the simulator.
        /// </summary>
        /// <param name="simulatorParameters">The operating parameters for the simulator.</param>
        void OnSetSimulatorParameters(SimulatorParameters simulatorParameters)
        {

            // Initialize the state of the dialog box from the state of the simulation recovered from the server.
            this.DataContext = simulatorParameters;

            // The tab is disabled until we have a connection to the simulator.
            this.IsEnabled = true;

        }

        /// <summary>
        /// Sets the simulation parameters.
        /// </summary>
        /// <param name="simulationParameters">The simulator parameters.</param>
        void SetSimulatorParameter(Object state)
        {

            // Extract the simulator parameters from the generic argument.
            SimulatorParameters simulatorParameters = state as SimulatorParameters;

            // This pushes the simulation parameters to the simulator.
            using (MarketServiceClient marketServiceClient = new MarketServiceClient())
                marketServiceClient.SetSimulatorParameters(simulatorParameters);

        }

    }

}
