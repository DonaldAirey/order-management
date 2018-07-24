namespace Teraque
{

	using System;
	using System.Globalization;

	/// <summary>
	/// Event data for progress messages.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class MessageEventArgs : EventArgs
	{

		/// <summary>
		/// The event message.
		/// </summary>
		public String Message { get; private set; }

		/// <summary>
		/// Indicates progress.
		/// </summary>
		public Boolean IsProgressTick { get; private set; }

		/// <summary>
		/// Initializes a new instance of the MessageEventArgs class.
		/// </summary>
		/// <param name="message">The event message.</param>
		/// <param name="arguments">The arguments for constructing the message.</param>
		public MessageEventArgs(String message, params Object[] arguments)
		{

			// Initialize the object.
			this.IsProgressTick = false;
			this.Message = String.Format(CultureInfo.CurrentCulture, message, arguments);

		}

		/// <summary>
		/// Initializes a new instance of the MessageEventArgs class.
		/// </summary>
		/// <param name="isProgressTick">Indicates the event is a tick.</param>
		/// <param name="message">The event message.</param>
		/// <param name="arguments">The arguments for constructing the message.</param>
		public MessageEventArgs(Boolean isProgressTick, String message, params Object[] arguments)
		{

			// Initialize the object.
			this.IsProgressTick = isProgressTick;
			this.Message = String.Format(CultureInfo.CurrentCulture, message, arguments);

		}

	}

}
