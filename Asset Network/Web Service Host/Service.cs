namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Diagnostics;
	using System.ServiceProcess;
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using System.ServiceModel.Description;

	/// <summary>
	/// Windows Service that provides a shared, in-memory data model.
	/// </summary>
	public partial class Service : ServiceBase
	{

		// The WebServiceHostInstaller doesn't have access to the application configuration or settings file.  These public constants allow it to use this class to 
		// provide the constant values when installing.  Since these are shared values, this appeared to be the most visible place to put them.
		public const String serviceName = "Web Service Host";
		public const String source = "Web Service Host";

		/// <summary>
		/// The collection of service hosts that can be started and stopped by this service.
		/// </summary>
		private List<ServiceHost> serviceHosts;

		/// <summary>
		/// Initialize a new instance of the Service class.
		/// </summary>
		public Service()
		{

			// Initialize the object.
			this.ServiceName = Service.serviceName;

		}

		/// <summary>
		/// The main entry point for the service.
		/// </summary>
		static void Main()
		{

#if DEBUG_SERVICE
			// This will run the project as an executable rather than as a service when debugging.
			Service service = new Service();
			service.OnStart(new string[] { });
			Console.Write("Hit any key to exit {0}.", Service.serviceName);
			Console.ReadKey();
			Console.WriteLine();
			service.Stop();
#else
			// This will run the Web Service as Windows Service.
			ServiceBase.Run(new ServiceBase[] { new Service() });
#endif

		}

		/// <summary>
		/// Start the service.
		/// </summary>
		/// <param name="args">Command line parameters.</param>
		protected override void OnStart(string[] args)
		{

			try
			{

#if !DEBUG_SERVICE
				// Starting up the database may sometimes take more than 30 seconds.  This will ask the service manager for some extra time to insure that
				// the web service can read all the data from the database and construct the in memory data model.
				this.RequestAdditionalTime(60000);
#endif

				// This Windows Service can host one or many WCF Endpoints.  They are started and stopped as a unit.
				this.serviceHosts = new List<ServiceHost>();
				this.serviceHosts.Add(new ServiceHost(typeof(DataModel), new Uri[] { }));
				this.serviceHosts.Add(new ServiceHost(typeof(WebService), new Uri[] { }));

				// Start each of the WCF Web Services hosted by this Windows Service.
				foreach (ServiceHost serviceHost in this.serviceHosts)
					serviceHost.Open();

			}
			catch (Exception exception)
			{

				// Any problems initializing should be sent to the Event Log.
				EventLog.WriteEntry(Service.source, string.Format("{0}: {1}", exception.Message, exception.StackTrace), EventLogEntryType.Error);

			}

			// Log the start of the service.
			EventLog.WriteEntry(Service.source, String.Format("Web Service {0} is starting", Service.serviceName), EventLogEntryType.Information);

		}

		/// <summary>
		/// Called when the service is stopped.
		/// </summary>
		protected override void OnStop()
		{

			try
			{

				// Shut down each of the Web Services hosted by this Windows Service.
				foreach(ServiceHost serviceHost in this.serviceHosts)
					serviceHost.Close();

			}
			catch(Exception exception)
			{

				// Any problems initializing should be sent to the Event Log.
				EventLog.WriteEntry(
					Service.source,
					string.Format("{0}: {1}", exception.Message, exception.StackTrace),
					EventLogEntryType.Information);

			}

			// Log the end of the service.
			EventLog.WriteEntry(
				Service.source,
				string.Format("Web Service {0} is stopping", Service.serviceName),
				EventLogEntryType.Information);

		}

	}

}
