namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Windows;
	using System.Windows.Threading;
	using Teraque;
	using Teraque.AssetNetwork.Properties;
	using Teraque.AssetNetwork.WebService;

	/// <summary>
	/// An application to explore and manage assets.
	/// </summary>
	partial class App : Application
	{

		/// <summary>
		/// Raises the Exit event.
		/// </summary>
		/// <param name="e">An ExitEventArgs that contains the event data.</param>
		protected override void OnExit(ExitEventArgs e)
		{

			// Disable the data model updates when we exit.  Without this we will leave an orphaned thread that prevents the application from exiting properly.
			DataModel.IsReading = false;

			// Allow the base class to handle the rest of the application shutdown.
			base.OnExit(e);

		}

		/// <summary>
		/// Raises the Startup event.
		/// </summary>
		/// <param name="e">A StartupEventArgs that contains the event data.</param>
		protected override void OnStartup(StartupEventArgs e)
		{

			// This starts the thread that reconciles the client data model with the server data model.
			DataModel.IsReading = true;

			// The base class will handle the rest of the startup event.
			base.OnStartup(e);

		}

	}

}
