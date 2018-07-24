namespace Teraque
{

	using System;
	using System.Collections.ObjectModel;
	using System.Windows;
	using Teraque.Windows.Controls;

	/// <summary>
	/// Contains the ItemsSource associated with a routed event.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ItemsSourceEventArgs : RoutedEventArgs
	{

		/// <summary>
		/// The GadgetBar associated with this event.
		/// </summary>
		public ObservableCollection<FrameworkElement> Items { get; private set; }

		/// <summary>
		/// Initializes a new instance of the ItemsSourceChangedEventArgs class, using the supplied routed event identifier.
		/// </summary>
		/// <param name="routedEvent">The routed event identifier for this instance of the ItemsSourceChangedEventArgs class.</param>
		public ItemsSourceEventArgs(RoutedEvent routedEvent) : base(routedEvent) { }

		/// <summary>
		/// Initializes a new instance of the ItemsSourceChangedEventArgs class, using the supplied routed event identifier, and a GadgetBar.
		/// </summary>
		/// <param name="routedEvent">The routed event identifier for this instance of the ItemsSourceChangedEventArgs class.</param>
		/// <param name="gadgetBar">The Explorerbar for this instance of the ItemsSourceChangedEventArgs class.</param>
		public ItemsSourceEventArgs(RoutedEvent routedEvent, ObservableCollection<FrameworkElement> items)
			: base(routedEvent)
		{

			// Initialize the object
			this.Items = items;

		}

	}

}
