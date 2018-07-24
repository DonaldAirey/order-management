namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Data;
	using Teraque.Windows.Controls;
	using Teraque.Windows.Input;

	/// <summary>
	/// Interaction logic for ColumnViewContextMenu.xaml
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ColumnViewContextMenu : ContextMenu
	{

		/// <summary>
		/// Initialize a new instance of the ColumnViewContextMenu class.
		/// </summary>
		public ColumnViewContextMenu()
		{

			// These command bindings will pick up commands from the context menu and execute them.
			this.CommandBindings.Add(new CommandBinding(Commands.More, this.OnMore));

			// The context menu is dynamic and will recalculate its contents each time it is opened depending on the context.
			this.Opened += this.OnOpened;

		}

		/// <summary>
		/// Occurs when a particular instance of a context menu opens.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnOpened(Object sender, RoutedEventArgs e)
		{

			// Extract the context menu from the generic event arguments.
			ContextMenu contextMenu = sender as ContextMenu;

			// We're going to clean out the context menu and re-evaluate the contents based on the context where the menu was opened.
			contextMenu.Items.Clear();

			// This is the header that opened the context menu.  From it, we need the ListView that hosts the ColumnView (that contains all the columns that we're
			// trying to manage with this context menu).
			ColumnViewColumnHeader columnViewColumnHeader = contextMenu.PlacementTarget as ColumnViewColumnHeader;
			ListView listView = VisualTreeExtensions.FindAncestor<ListView>(columnViewColumnHeader);
			ColumnView columnView = listView.View as ColumnView;

			// The task now is to fill in the menu items.  We don't allow the user to resize a padding column, but will allow them to resize all the columns.
			if (columnViewColumnHeader.Role != ColumnViewColumnHeaderRole.Padding)
				this.Items.Add(new MenuItem() { Command = Commands.FitColumn, CommandTarget = columnViewColumnHeader, Header = "Size Column to Fit" });
			this.Items.Add(new MenuItem() { Command = Commands.FitAllColumns, CommandTarget = listView, Header = "Size All Columns to Fit" });
			this.Items.Add(new Separator());

			// This will order the fixed menu items by their ordinal.  An 'ordinal' is a numeric value that the designer can assign to the fixed menu items to
			// determine the order (or even if they'll appear) as a fixed menu item (that is, a quick way to add or remove a column, as opposed to selecting the
			// 'More...' button and using the list box to select an column).
			SortedList<Int32, ColumnViewColumn> commonColumns = new SortedList<Int32, ColumnViewColumn>();
			foreach (ColumnViewColumn columnViewColumn in columnView.Columns)
				if (columnViewColumn.Ordinal.HasValue && !commonColumns.ContainsKey(columnViewColumn.Ordinal.Value))
					commonColumns.Add(columnViewColumn.Ordinal.Value, columnViewColumn);

			// This will create a dedicated menu item for each of the columns that has been assigned an 'ordinal'.  This allows the user to quickly add or remove
			// frequently used columns.  Note that we attempt to format the column headers the way they would appear in the actual column header.
			foreach (KeyValuePair<Int32, ColumnViewColumn> keyValuePair in commonColumns)
			{
				ColumnViewColumn columnViewColumn = keyValuePair.Value;
				MenuItem menuItem = new MenuItem();
				Binding isVisibleBinding = new Binding() { Path = new PropertyPath(ColumnViewColumn.IsVisibleProperty) };
				isVisibleBinding.Source = columnViewColumn;
				BindingOperations.SetBinding(menuItem, MenuItem.IsCheckedProperty, isVisibleBinding);
				menuItem.Header = columnViewColumn.Description;
				menuItem.IsCheckable = true;
				this.Items.Add(menuItem);
			}

			// Place a separator in the context menu if there is a section above it with dedicated menu items for the column visibility settings.
			if (commonColumns.Count != 0)
				this.Items.Add(new Separator());

			// Finally, a general purpose dialog box for managing the column set.
			this.Items.Add(new MenuItem() { Command = Commands.More, Header = "More..." });

		}

		/// <summary>
		/// Presents the user with a dialog box for managing the columns.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event data.</param>
		void OnMore(Object sender, ExecutedRoutedEventArgs e)
		{

			// This will extract the context menu that generated this event from the generic arguments.
			ContextMenu contextMenu = sender as ContextMenu;

			// From the context menu, we need the target that orignated the event and from that we can finally get the column set on which we want to operate.
			ColumnViewColumnHeader columnViewColumnHeader = contextMenu.PlacementTarget as ColumnViewColumnHeader;
			ListView listView = VisualTreeExtensions.FindAncestor<ListView>(columnViewColumnHeader);
			ColumnView columnView = listView.View as ColumnView;

			// This dialog box is used to add, remove, move or resize the columns in the view.  It can't operate directly on the set of columns because those are
			// linked dynamically to the view, so we'll create a shallow clone of the values.
			ColumnViewChooseDetail columnViewChooseDetail = new ColumnViewChooseDetail();
			foreach (ColumnViewColumn columnViewColumn in columnView.Columns)
				columnViewChooseDetail.ListBox.Items.Add(new ColumnDescription(columnViewColumn));

			// Present the user with the chance to manage the columns.  If the OK key is hit then copy the values out of the shallow copy and into the live column
			// set where they'll update the view.
			if (columnViewChooseDetail.ShowDialog() == true)
				foreach (ColumnDescription columnDescription in columnViewChooseDetail.ListBox.Items)
				{
					ColumnViewColumn columnViewColumn = columnDescription.Column;
					columnViewColumn.IsVisible = columnDescription.IsVisible;
					columnViewColumn.Width = columnDescription.Width;
				}

		}

	}

}
