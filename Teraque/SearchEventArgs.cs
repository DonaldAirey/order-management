namespace Teraque
{

	using System;
	using System.Windows;

	/// <summary>
	/// Search for text.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class SearchRoutedEventArgs : RoutedEventArgs
	{

		/// <summary>
		/// The text for which to search.
		/// </summary>
		public String Text { get; private set; }

		/// <summary>
		/// Initializes a new instance of the SearchRoutedEventArgs class, using the supplied routed event identifier.
		/// </summary>
		/// <param name="routedEvent">The routed event identifier for this instance of the GadgetBarEventArgs class.</param>
		public SearchRoutedEventArgs(RoutedEvent routedEvent)
			: base(routedEvent)
		{

			// Initialize the object
			this.Text = String.Empty;

		}

		/// <summary>
		/// Initializes a new instance of the SearchRoutedEventArgs class.
		/// </summary>
		/// <param name="routedEvent">The routed event identifier for this instance of the SearchRoutedEventArgs class.</param>
		/// <param name="text">The text for this instance of the SearchRoutedEventArgs class.</param>
		public SearchRoutedEventArgs(RoutedEvent routedEvent, String text)
			: base(routedEvent)
		{

			// Initialize the object
			this.Text = text;

		}

	}

}
