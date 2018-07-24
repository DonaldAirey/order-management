namespace Teraque
{

	using System;

	/// <summary>
	/// ExceptionEventArgs contains event data describing an exception.
	/// </summary>
	/// <copyright>Copyright © 2010 - 2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ExceptionEventArgs : EventArgs
	{

		/// <summary>
		/// The exception.
		/// </summary>
		public Exception Exception { get; private set; }

		/// <summary>
		/// Initializes a new instance of the ExceptionEventArgs class.
		/// </summary>
		/// <param name="exception">An exception.</param>
		public ExceptionEventArgs(Exception exception)
		{

			// Initialize the object
			this.Exception = exception;

		}

	}

}
