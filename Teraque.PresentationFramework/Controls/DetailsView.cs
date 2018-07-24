namespace Teraque.Windows.Controls
{

	using System;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;

	/// <summary>
	/// A view used to display items in a columnar format with special handling for the first column.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class DetailsView : ColumnView
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

			// The actual binding and unbinding is done during the 'Load' and 'Unload' event handlers.  This will simply clean up those handlers when the Object is
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
		/// <param name="sender">The Object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnItemLoaded(Object sender, RoutedEventArgs e)
		{

			// Extract the ListViewItem that originated the event.
			ListViewItem listViewItem = sender as ListViewItem;

			// This will install event handlers for user intput for each item in the list.
			listViewItem.MouseDoubleClick += this.OnItemMouseDoubleClick;
			listViewItem.KeyDown += this.OnItemKeyDown;

			// This gives descendant classes a chance to initialize bindings that require the item to be loaded.
			this.OnItemLoaded(listViewItem);

		}

		/// <summary>
		/// Occurs when a key is pressed on an item in the list.
		/// </summary>
		/// <param name="sender">The Object where the event handler is attached.</param>
		/// <param name="mouseButtonEventArgs">The event data.</param>
		void OnItemKeyDown(object sender, KeyEventArgs keyEventArgs)
		{

			// This will execute the 'OpenItem' command when the enter key is pressed.
			if (keyEventArgs.Key == Key.Enter)
				ApplicationCommands.Open.Execute(null, null);

			// We've handled the key.
			keyEventArgs.Handled = true;

		}

		/// <summary>
		/// Occurs when a mouse button is clicked two or more times on an item in the list.
		/// </summary>
		/// <param name="sender">The Object where the event handler is attached.</param>
		/// <param name="mouseButtonEventArgs">The event data.</param>
		void OnItemMouseDoubleClick(Object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{

			// This will execute the 'OpenItem' command.
			ApplicationCommands.Open.Execute(null, null);

			// We've handled the gesture.
			mouseButtonEventArgs.Handled = true;

		}

		/// <summary>
		/// Occurs when the element is removed from within an element tree of loaded elements. 
		/// </summary>
		/// <param name="listViewItem">The item that has been removed from the view.</param>
		protected virtual void OnItemUnloaded(ListViewItem listViewItem) { }

		/// <summary>
		/// Occurs when the element is removed from within an element tree of loaded elements. 
		/// </summary>
		/// <param name="sender">The Object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnItemUnloaded(Object sender, RoutedEventArgs e)
		{

			// Extract the item from the generic event arguments.
			ListViewItem listViewItem = sender as ListViewItem;

			// This will remove the handlers before unloading the item from the list.
			listViewItem.MouseDoubleClick -= this.OnItemMouseDoubleClick;
			listViewItem.KeyDown -= this.OnItemKeyDown;

			// This gives descendant classes a chance to remove bindings.
			this.OnItemUnloaded(listViewItem);

		}

		/// <summary>
		/// Prepares an item in the view for display, by setting bindings and styles.
		/// </summary>
		/// <param name="item">The item to prepare.</param>
		protected override void PrepareItem(ListViewItem item)
		{

            // Validate parameters
            if (item == null)
                throw new ArgumentNullException("item");

            // The part to which we want to bind is part of the template which hasn't been loaded at this point.  This will install handlers that will notify this n
			// class whethe ListViewItem has loaded the template and is ready to be bound.
			item.Loaded += new RoutedEventHandler(this.OnItemLoaded);
			item.Unloaded += new RoutedEventHandler(this.OnItemUnloaded);

			// Allow the base class to handle the rest of the preparation.
			base.PrepareItem(item);

		}

		/// <summary>
		/// Gets the reference for the default style for the DetailsView. (Overrides ViewBase.DefaultStyleKey.)
		/// </summary>
		protected override Object DefaultStyleKey
		{
			get
			{
				return new ComponentResourceKey(typeof(DetailsView), "DetailsViewStyle");
			}
		}

		/// <summary>
		/// Gets the reference to the default style for the container of the data items in the DetailsView. (Overrides ViewBase.ItemContainerDefaultStyleKey.)
		/// </summary>
		protected override Object ItemContainerDefaultStyleKey
		{
			get
			{
				return new ComponentResourceKey(typeof(DetailsView), "DetailsViewItemContainerStyle");
			}
		}

	}

}
