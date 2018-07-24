namespace Teraque.Tools
{

	using System;
	using System.Windows;
	using System.Windows.Input;

	/// <summary>
	/// Commands for the application.
	/// </summary>
	public sealed class Commands
	{

		/// <summary>
		/// Initializes a new instance of the Commands class. 
		/// </summary>
		Commands() { }

		/// <summary>
		/// Open a file.
		/// </summary>
		public static readonly RoutedCommand OpenFile = new RoutedCommand("OpenFile", typeof(Commands));

	}
}
