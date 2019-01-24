// <copyright file="MarketSimulator.cs" company="Teraque, Inc.">
//     Copyright © 2013 - Teraque, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace Teraque.AssetNetwork
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Security;
    using System.ServiceModel;
    using System.ServiceProcess;

    /// <summary>
    /// Simulates market conditions used to test the Guardian architecture.
    /// </summary>
    public class MarketSimulator : ServiceBase
    {
        /// <summary>
        /// The description of the service.  Used when installing the service.
        /// </summary>
        public const String Description = "Asset Network Market Simulator";

        /// <summary>
        /// The name of the source for the event log that's being installed.  Specified in code because config files are not available when the installer runs.
        /// </summary>
        public const String Source = "Market Simulator";

        /// <summary>
        /// The name of the service.  Used in installing the service and in log messages.
        /// </summary>
        public new const String ServiceName = "Market Simulator";

        /// <summary>
        /// The parameters that control the heuristics of the simulator.
        /// </summary>
        private static SimulatorParameters simulatorParametersField;

        /// <summary>
        /// The collection of service hosts that can be started and stopped by this service.
        /// </summary>
        private List<ServiceHost> serviceHosts;

        /// <summary>
        /// Initializes static members of the <see cref="MarketSimulator"/> class.
        /// </summary>
        static MarketSimulator()
        {
            // These settings control the simulator.
            MarketSimulator.simulatorParametersField = new SimulatorParameters();
            MarketSimulator.simulatorParametersField.EquityFrequency = PriceSimulator.EquityFrequency;
            MarketSimulator.simulatorParametersField.ExecutionFrequency = BrokerSimulator.ExecutionFrequency;
            MarketSimulator.simulatorParametersField.IsExchangeSimulatorRunning = false;
            MarketSimulator.simulatorParametersField.IsPriceSimulatorRunning = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketSimulator"/> class.
        /// </summary>
        public MarketSimulator()
        {
            // Initialize the object.
            base.ServiceName = MarketSimulator.ServiceName;
        }

        /// <summary>
        /// Gets or sets the thread safe parameters that control the simulations.
        /// </summary>
        public static SimulatorParameters SimulatorParameters
        {
            get
            {
                return MarketSimulator.simulatorParametersField;
            }

            set
            {
                // This starts the broker simulation when it isn't running.
                if (!MarketSimulator.SimulatorParameters.IsExchangeSimulatorRunning && value.IsExchangeSimulatorRunning)
                {
                    BrokerSimulator.Start();
                }

                // This shuts down the broker simulation when it is running.
                if (MarketSimulator.SimulatorParameters.IsExchangeSimulatorRunning && !value.IsExchangeSimulatorRunning)
                {
                    BrokerSimulator.Stop();
                }

                // This starts the price simulation when it isn't running.
                if (!MarketSimulator.SimulatorParameters.IsPriceSimulatorRunning && value.IsPriceSimulatorRunning)
                {
                    PriceSimulator.Start();
                }

                // This shuts down the price simulation when it is running.
                if (MarketSimulator.SimulatorParameters.IsPriceSimulatorRunning && !value.IsPriceSimulatorRunning)
                {
                    PriceSimulator.Stop();
                }

                // The saved parameters reflect the state of the simulation.
                MarketSimulator.simulatorParametersField = value;
                PriceSimulator.EquityFrequency = MarketSimulator.SimulatorParameters.EquityFrequency;
                BrokerSimulator.ExecutionFrequency = MarketSimulator.SimulatorParameters.ExecutionFrequency;
            }
        }

        /// <summary>
        /// Start the service.
        /// </summary>
        /// <param name="args">Command line parameters.</param>
        protected override void OnStart(String[] args)
        {
            try
            {
                // This Windows Service can host one or many WCF Endpoints.  They are started and stopped as a unit.
                this.serviceHosts = new List<ServiceHost>();
                this.serviceHosts.Add(new ServiceHost(typeof(MarketService), new Uri[] { }));

                // Start each of the WCF Web Services hosted by this Windows Service.
                foreach (ServiceHost serviceHost in this.serviceHosts)
                {
                    serviceHost.Open();
                }
            }
            catch (Exception exception)
            {
                // Any problems initializing should be sent to the Event Log.
                EventLog.WriteEntry(MarketSimulator.Source, String.Format("{0}: {1}", exception.Message, exception.StackTrace), EventLogEntryType.Information);
            }

            // Log the start of the service.
            try
            {
                EventLog.WriteEntry(
                    MarketSimulator.Source,
                    String.Format(Simulator.Properties.Resources.ServiceStarted, MarketSimulator.ServiceName),
                    EventLogEntryType.Information);
            }
            catch (SecurityException)
            {
            }
        }

        /// <summary>
        /// Called when the service is stopped.
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                // This shuts down the broker simulation when it is running.
                if (MarketSimulator.SimulatorParameters.IsExchangeSimulatorRunning)
                {
                    BrokerSimulator.Stop();
                }

                // This shuts down the price simulation when it is running.
                if (MarketSimulator.SimulatorParameters.IsPriceSimulatorRunning)
                {
                    PriceSimulator.Stop();
                }

                // Shut down each of the Web Services hosted by this Windows Service.
                foreach (ServiceHost serviceHost in this.serviceHosts)
                {
                    serviceHost.Close();
                }
            }
            catch (Exception exception)
            {
                // Any problems initializing should be sent to the Event Log.
                EventLog.WriteEntry(MarketSimulator.Source, String.Format("{0}: {1}", exception.Message, exception.StackTrace), EventLogEntryType.Information);
            }

            // Log the end of the service.
            try
            {
                EventLog.WriteEntry(
                    MarketSimulator.Source,
                    String.Format(Simulator.Properties.Resources.ServiceStopping, MarketSimulator.ServiceName),
                    EventLogEntryType.Information);
            }
            catch (SecurityException)
            {
            }
        }

        /// <summary>
        /// The main entry point for the service.
        /// </summary>
        private static void Main()
        {
#if START_SERVICE
            // This will run the project as an executable rather than as a service when debugging.
            MarketSimulator service = new MarketSimulator();
            service.OnStart(new String[] { });
            Console.Write("Hit any key to exit {0}.", MarketSimulator.ServiceName);
            Console.ReadKey();
            Console.WriteLine();
            service.Stop();
#else
            // This will run the Web Service as Windows Service.
            ServiceBase.Run(new ServiceBase[] { new MarketSimulator() });
#endif
        }
    }
}
