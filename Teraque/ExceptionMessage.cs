namespace Teraque
{

	using System;
	using System.Globalization;

	/// <summary>
	/// Formats messages that appear in exceptions.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public sealed class ExceptionMessage
	{

		/// <summary>
		/// Initializes a new instance of ExceptionMessage class.
		/// </summary>
		ExceptionMessage() { }

		/// <summary>
		/// Formats an exception message according the the invariant culture.
		/// </summary>
		/// <param name="message">The format of the message.</param>
		/// <param name="arguments">An optional list of parameters.</param>
		/// <returns>The formatted string targeting the invariant culture.</returns>
		public static String Format(String message, params Object[] arguments)
		{

			// This insures that the rules for formatting a string for a common culture is applied.  Exception messages are intended to be universal and should not
			// be formatted for a local culture.
			return String.Format(CultureInfo.InvariantCulture, message, arguments);

		}

	}
}
