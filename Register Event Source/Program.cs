namespace Teraque
{

	using System;

	/// <summary>
	/// Creates an Event Log and an Event Source.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	class Program
	{

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(String[] args)
		{

			// Get the settings for the event log and the source name from the configuration file.
			String logName = Teraque.Properties.Settings.Default.EventLogLog;
			String[] sourceList = Teraque.Properties.Settings.Default.EventLogSource.Split('|');
			Boolean deleteLog = Teraque.RegisterEventSource.Properties.Settings.Default.DeleteLog;
			Boolean createSources = Teraque.RegisterEventSource.Properties.Settings.Default.CreateSources;

			// Delete the sources if they exist.  If the source is moved from one log to another, the user should be warned that they need to reboot the computer.
			Boolean isSourceMoved = false;
			foreach(String source in sourceList)
			{
				String oldLogName = System.Diagnostics.EventLog.LogNameFromSourceName(source, ".");
				if (System.Diagnostics.EventLog.SourceExists(source))
					System.Diagnostics.EventLog.DeleteEventSource(source);
				if (oldLogName != String.Empty && oldLogName != logName)
					isSourceMoved = true;
			}

			// This will clean out the entire log if the feature is selected.
			if (deleteLog)
			{

				// Delete each of the sources that match the name of the source in the configuration file.
				foreach (String source in sourceList)
				{
					String oldLogName = System.Diagnostics.EventLog.LogNameFromSourceName(source, ".");
					if (System.Diagnostics.EventLog.SourceExists(source))
						System.Diagnostics.EventLog.DeleteEventSource(source);
				}

				// This will delete the actual file where the log is kept.
				if (System.Diagnostics.EventLog.Exists(logName))
					System.Diagnostics.EventLog.Delete(logName);

			}

			// Create each of the sources specified in the configuration file (there should only be one).
			if (createSources)
				foreach(String source in sourceList)
					System.Diagnostics.EventLog.CreateEventSource(source, logName);

			// Put up a helpful message about moving sources mapped to other logs.
			if (isSourceMoved)
			{
				Console.WriteLine("You have moved a source that was already mapped to another log.");
				Console.WriteLine("You must reboot the computer for the changes to take effect.");
				Console.WriteLine("Press 'Enter' key.");
				Console.ReadLine();
			}

		}

	}

}
