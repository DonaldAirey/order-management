namespace Teraque
{

	using Teraque.Properties;
	using System;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;

	/// <summary>
	/// Provides interaction with a specific Window event log.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public sealed class Log
	{

		/// <summary>
		/// The persistent event log.
		/// </summary>
		static EventLog eventLog = InitializeEventLog();

		/// <summary>
		/// Initializes a new instance of the Log class.
		/// </summary>
		Log()
		{
		}

		/// <summary>
		/// Initializes an EventLog object for this type.
		/// </summary>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		static EventLog InitializeEventLog()
		{

			// Create an event log specifically for the Log and Source described in the configuration file.
			EventLog eventLog = new EventLog();
			eventLog.Log = Settings.Default.EventLogLogName;
			eventLog.Source = Settings.Default.EventLogSource == null ? Process.GetCurrentProcess().MainModule.ModuleName : Settings.Default.EventLogSource;
			return eventLog;

		}

		/// <summary>
		/// Logs an error event.
		/// </summary>
		/// <param name="format">The format of the string to write to the event log.</param>
		/// <param name="parameters">The optional parameters for the message.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public static void Error(String format, params Object[] parameters)
		{

			try
			{

				// Create a log entry using the formatted text and give it a log type indicating an error.
				Log.eventLog.WriteEntry(String.Format(CultureInfo.InvariantCulture, format, parameters), EventLogEntryType.Error);

			}
			catch
			{

				// It is critical that writing to the event log doesn't generate an exception.  This can lead to an ugly chain reaction.

			}

		}

		/// <summary>
		/// Logs an error event.
		/// </summary>
		/// <param name="eventId">The application-specific identifier for the event.</param>
		/// <param name="format">The format of the string to write to the event log.</param>
		/// <param name="parameters">The optional parameters for the message.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public static void Error(Int32 eventId, String format, params Object[] parameters)
		{

			try
			{

				// Create a log entry using the formatted text and give it a log type indicating an error.
				Log.eventLog.WriteEntry(String.Format(CultureInfo.InvariantCulture, format, parameters), EventLogEntryType.Error, eventId);

			}
			catch
			{

				// It is critical that writing to the event log doesn't generate an exception.  This can lead to an ugly chain reaction.

			}

		}

		/// <summary>
		/// Logs an information event.
		/// </summary>
		/// <param name="format">The format of the string to write to the event log.</param>
		/// <param name="parameters">The optional parameters for the message.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public static void Information(String format, params Object[] parameters)
		{

			try
			{

				// Create a log entry using the formatted text and give it a log type indicating an information.
				Log.eventLog.WriteEntry(String.Format(CultureInfo.InvariantCulture, format, parameters), EventLogEntryType.Information);

			}
			catch
			{

				// It is critical that writing to the event log doesn't generate an exception.  This can lead to an ugly chain reaction.

			}

		}

		/// <summary>
		/// Logs an information event.
		/// </summary>
		/// <param name="eventId">The application-specific identifier for the event.</param>
		/// <param name="format">The format of the string to write to the event log.</param>
		/// <param name="parameters">The optional parameters for the message.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public static void Information(Int32 eventId, String format, params Object[] parameters)
		{

			try
			{

				// Create a log entry using the formatted text and give it a log type indicating an information.
				Log.eventLog.WriteEntry(String.Format(CultureInfo.InvariantCulture, format, parameters), EventLogEntryType.Information, eventId);

			}
			catch
			{

				// It is critical that writing to the event log doesn't generate an exception.  This can lead to an ugly chain reaction.

			}

		}

		/// <summary>
		/// Logs an warning event.
		/// </summary>
		/// <param name="format">The format of the string to write to the event log.</param>
		/// <param name="parameters">The optional parameters for the message.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public static void Warning(String format, params Object[] parameters)
		{

			try
			{

				// Create a log entry using the formatted text and give it a log type indicating an warning.
				Log.eventLog.WriteEntry(String.Format(CultureInfo.InvariantCulture, format, parameters), EventLogEntryType.Warning);

			}
			catch
			{

				// It is critical that writing to the event log doesn't generate an exception.  This can lead to an ugly chain reaction.

			}

		}

		/// <summary>
		/// Logs an warning event.
		/// </summary>
		/// <param name="eventId">The application-specific identifier for the event.</param>
		/// <param name="format">The format of the string to write to the event log.</param>
		/// <param name="parameters">The optional parameters for the message.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public static void Warning(Int32 eventId, String format, params Object[] parameters)
		{

			try
			{

				// Create a log entry using the formatted text and give it a log type indicating an warning.
				Log.eventLog.WriteEntry(String.Format(CultureInfo.InvariantCulture, format, parameters), EventLogEntryType.Warning, eventId);

			}
			catch
			{

				// It is critical that writing to the event log doesn't generate an exception.  This can lead to an ugly chain reaction.

			}

		}

	}

}
