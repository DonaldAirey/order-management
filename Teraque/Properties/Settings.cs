namespace Teraque.Properties
{

	using System;
	using System.Configuration;
	using System.Windows;

	/// <summary>
	/// Implement the application settings feature in Window Presentation Foundation applications.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public sealed partial class Settings : ApplicationSettingsBase
	{

		/// <summary>
		/// Indicates that the values in the settings file have changed.
		/// </summary>
		Boolean isChanged;

		/// <summary>
		/// Raises the SettingsLoaded event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A SettingsLoadedEventArgs that contains the event data.</param>
		protected override void OnSettingsLoaded(object sender, SettingsLoadedEventArgs e)
		{

			// This will attach the settings to the current process of the comain and watch for the main process to exit.  The general idea is to automatically save
			// the settings when the application that called the settings has terminated.  It should be noted that the typical 'Application.OnExit' can't be used
			// because settings may be accessed in application constructors.
			AppDomain.CurrentDomain.ProcessExit += this.OnProcessExit;

			// Allow the base class to handle the reset of the event.
			base.OnSettingsLoaded(sender, e);

		}

		/// <summary>
		/// Invoked when the main process of the calling application domain has exited.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="eventArgs">The event arguments.</param>
		void OnProcessExit(Object sender, EventArgs eventArgs)
		{

			// Just to be wholesome about this, we make sure that this routine is not available after the process has exited and before the garbage collection has
			// reclaimed it.
			AppDomain.CurrentDomain.ProcessExit -= this.OnProcessExit;

			// It is important to save the settings only when they've been modified.  The Visual Studio designer doesn't like us messing around with the settings
			// file during design time.
			if (this.isChanged)
				this.Save();

		}

		/// <summary>
		/// Raises the SettingChanging event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="settingChangingEventArgs">A SettingChangingEventArgs that contains the event data.</param>
		protected override void OnSettingChanging(Object sender, SettingChangingEventArgs settingChangingEventArgs)
		{

			// This will trigger the 'OnProcessExit' method to save the changes.  There's no need to save the settings file if nothing has changed and
			// there are issues with the designer if you try this while the process is shutting down.
			this.isChanged = true;

			// Allow the base class to handle the reset of the event.
			base.OnSettingChanging(sender, settingChangingEventArgs);

		}

	}

}
