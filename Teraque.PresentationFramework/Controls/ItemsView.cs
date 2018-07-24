namespace Teraque.Windows.Controls
{

	using System;
	using System.ComponentModel;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Input;

	/// <summary>
	/// A generic view used to display items.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public abstract class ItemsView : ViewBase
	{

		/// <summary>
		/// Removes all bindings and styling that are set for an item.
		/// </summary>
		/// <param name="item"></param>
		protected override void ClearItem(ListViewItem item)
		{

            // Validate parameters
            if (item == null)
                throw new ArgumentNullException("item");

            // The actual binding and unbinding is done during the 'Load' and 'Unload' event handlers.  This will simply clean up those handlers when the object is
			// cleared.
			item.Loaded -= new RoutedEventHandler(this.OnItemLoaded);
			item.Unloaded -= new RoutedEventHandler(this.OnItemUnloaded);

			// Allow the base class to handle the rest of the clearing.
			base.ClearItem(item);

		}

		/// <summary>
		/// Occurs when the element is laid out, rendered, and ready for interaction.
		/// </summary>
		/// <param name="listViewItem">The item that has been loaded into the view.</param>
		protected virtual void OnItemLoaded(ListViewItem listViewItem) { }

		/// <summary>
		/// Occurs when the element is laid out, rendered, and ready for interaction.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnItemLoaded(Object sender, RoutedEventArgs e)
		{

			// Extract the ListViewItem that originated the event.
			ListViewItem listViewItem = sender as ListViewItem;

			// This will catch a notification from the control that it has been double clicked turn it into a command that's passed up the visual tree.
			listViewItem.MouseDoubleClick += new MouseButtonEventHandler(this.OnItemMouseDoubleClick);

			// This gives descendant classes a chance to initialize bindings that require the item to be loaded.
			this.OnItemLoaded(listViewItem);

		}

		/// <summary>
		/// Occurs when a mouse button is clicked two or more times.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="mouseButtonEventArgs">The event data.</param>
		void OnItemMouseDoubleClick(Object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{

			// This will execute the 'OpenItem' command.
			ApplicationCommands.Open.Execute(null, null);

		}

		/// <summary>
		/// Occurs when the element is removed from within an element tree of loaded elements. 
		/// </summary>
		/// <param name="listViewItem">The item that has been removed from the view.</param>
		protected virtual void OnItemUnloaded(ListViewItem listViewItem) { }

		/// <summary>
		/// Occurs when the element is removed from within an element tree of loaded elements. 
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnItemUnloaded(Object sender, RoutedEventArgs e)
		{

			// Extract the item from the generic event arguments.
			ListViewItem listViewItem = sender as ListViewItem;

			// This will remove the double click event handler from the item now that it is no longer needed.
			listViewItem.MouseDoubleClick -= new MouseButtonEventHandler(this.OnItemMouseDoubleClick);

			// This gives descendant classes a chance to remove bindings.
			this.OnItemUnloaded(listViewItem);

		}

		/// <summary>
		/// Prepares an item in the view for display, by setting bindings and styles.
		/// </summary>
		/// <param name="item"></param>
		protected override void PrepareItem(ListViewItem item)
		{

            // Validate parameters
            if (item == null)
                throw new ArgumentNullException("item");

            // The part to which we want to bind is part of the template which hasn't been loaded at this point.  This will install handlers that will notify this
			// class when the ListViewItem has loaded the template and is ready to be bound.
			item.Loaded += new RoutedEventHandler(this.OnItemLoaded);
			item.Unloaded += new RoutedEventHandler(this.OnItemUnloaded);

			// Allow the base class to handle the rest of the preparation.
			base.PrepareItem(item);

		}

	}

}
