namespace Teraque.Windows
{

	using System;
	using System.Collections.ObjectModel;

    /// <summary>
	/// Generic arguments for a generic event.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class GenericEventArgs : EventArgs
	{

		/// <summary>
		/// The generic event arguments.
		/// </summary>
		Object[] argumentsField;

		/// <summary>
		/// Creates a new instance of the GenericEventArgs class.
		/// </summary>
		/// <param name="arguments">The generic event arguments.</param>
		public GenericEventArgs(params Object[] arguments)
		{

			// Initialize the object.
			this.argumentsField = arguments;

		}

		/// <summary>
		/// Gets the generic event arguments.
		/// </summary>
		public ReadOnlyCollection<Object> Arguments
		{
			get
			{
				return new ReadOnlyCollection<Object>(this.argumentsField);
			}
		}

	}

}
