namespace Teraque
{

	using System;

	/// <summary>
	/// Application specific event identifiers for logging.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public sealed class EventId
	{

		/// <summary>
		/// Create a new instance of the Teraque.EventId class.
		/// </summary>
		EventId()
		{
		}

		/// <summary>
		/// Non-specific communication errors.
		/// </summary>
		public const Int32 GeneralCommunicationError = 1000;

		/// <summary>
		/// Access to a given resource was denied due to the credentials offered.
		/// </summary>
		public const Int32 AccessDenied = 1001;

		/// <summary>
		/// Non-specific assembly errors.
		/// </summary>
		public const Int32 GeneralAssemblyError = 2000;

		/// <summary>
		/// A problem with an Interop call.
		/// </summary>
		public const Int32 InteropError = 2001;

	}
}
