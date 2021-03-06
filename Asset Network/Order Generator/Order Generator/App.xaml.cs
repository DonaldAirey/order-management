﻿namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Windows;
	using System.Windows.Threading;
	using Teraque;
	using Teraque.AssetNetwork.Properties;

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

			// This will catch all unhandled exceptions including the ones that can be generated by the main window having trouble initializing.  The original
			// version was placed in the MainWindow but failed to catch XAML errors when the frame couldn't be loaded, particularly due to licensing errors, 
			// because the event handlers are not installed when the main window calls its constructors.
#if !DEBUG
			this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(this.OnDispatcherUnhandledException);
#endif

			// This handler will deal with the special case where the tenant has been unloaded.  Nearly all other communication errors will be retried periodically 
			// until the communication recovers, but this event occurs when the server has been intensionally shut down.  We'll terminate the application to give
			// the server a chance to recover.
			DataModel.TenantNotLoaded += this.OnTenantNotLoaded;

			// This starts the thread that reconciles the client data model with the server data model.
			DataModel.IsReading = true;

			// The base class will handle the rest of the startup event.
			base.OnStartup(e);

		}

		/// <summary>
		/// Handles the tenant being unloaded.
		/// </summary>
		/// <param name="sender">The object that invoked this event.</param>
		/// <param name="tenantName">The name of the tenant that couldn't be accessed on the server.</param>
		void OnTenantNotLoaded(Object sender, String tenantName)
		{

			// If the tenant has been unloaded then it is probably for a very good reason, such as a catestrophic database error.  In this scenario we're going to 
			// give the user a message to come back later before exiting.  It is best the server doesn't have dozens of zombie workstations polling it for data
			// while it is trying to reboot.
			this.Dispatcher.Invoke(new Action(() =>
			{
				MessageBox.Show(
					String.Format(Explorer.Properties.Resources.TenantNotLoaded, tenantName),
					Explorer.Properties.Resources.Title,
					MessageBoxButton.OK,
					MessageBoxImage.Error);
				this.Shutdown();
			}));

		}

	}

}
