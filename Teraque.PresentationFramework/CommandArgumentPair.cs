namespace Teraque.Windows
{

	using System;

    /// <summary>
	/// Represents a generic pair of event handlers and their arguments.
	/// </summary>
	public class CommandArgumentPair
	{

		/// <summary>
		/// The generic event handler.
		/// </summary>
		GenericEventHandler genericEventHandlerField;

		/// <summary>
		/// The generic event arguments.
		/// </summary>
		GenericEventArgs genericEventArgsField;

		/// <summary>
		/// Create a description of a method that should be invoked with the specified arguments.
		/// </summary>
		/// <param name="argumentDelegate">The method that should be invoked.</param>
		/// <param name="arguments">The arguments of the method invokation.</param>
		public CommandArgumentPair(GenericEventHandler genericEventHandler, GenericEventArgs genericEventArgs)
		{

			// Initialize the object
			this.genericEventHandlerField = genericEventHandler;
			this.genericEventArgsField = genericEventArgs;

		}

		/// <summary>
		/// Create a description of a method that should be invoked with the specified arguments.
		/// </summary>
		/// <param name="argumentDelegate">The method that should be invoked.</param>
		/// <param name="arguments">The arguments of the method invokation.</param>
		public CommandArgumentPair(GenericEventHandler genericEventHandler, params Object[] arguments)
		{

			// Initialize the object
			this.genericEventHandlerField = genericEventHandler;
			this.genericEventArgsField = new GenericEventArgs(arguments);

		}

		/// <summary>
		/// Gets the generic event handler.
		/// </summary>
		public GenericEventHandler GenericEventHandler
		{
			get
			{
				return this.genericEventHandlerField;
			}
		}

		/// <summary>
		/// Get the generic event arguments.
		/// </summary>
		public GenericEventArgs GenericEventArgs
		{
			get
			{
				return this.genericEventArgsField;
			}
		}

	}

}
