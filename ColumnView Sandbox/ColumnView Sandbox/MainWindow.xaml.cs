namespace Teraque.Windows
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Data;
	using System.Windows.Media.Imaging;
	using Teraque.Windows.Controls;
	using Teraque.Windows.Input;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class MainWindow : Window
	{

		/// <summary>
		/// Keeps track of the original order of columns in the header so they can be restored to the original position.
		/// </summary>
		Dictionary<ColumnViewColumn, Int32> columnOrderMap = new Dictionary<ColumnViewColumn, Int32>();

		/// <summary>
		/// A collection of consumers.
		/// </summary>
		ConsumerCollection consumerCollection = new ConsumerCollection();

		/// <summary>
		/// A set of groups used to filter the view.  Each group contains a set of predicates, any one of which must be satisified for the group to be viewed.
		/// </summary>
		Dictionary<String, ObservableCollection<Predicate<Consumer>>> filterGroups = new Dictionary<String, ObservableCollection<Predicate<Consumer>>>();

		/// <summary>
		/// Maps the name of the FilterItem to the predicate that handles that filter.
		/// </summary>
		static Dictionary<String, Predicate<Consumer>> filterDelegateMap = new Dictionary<String, Predicate<Consumer>>()
		{
			{ "FilterFirstNameAtoI", (Consumer c) => {return MainWindow.CharacterRangeTest(c.FirstName, 'A', 'I');}},
			{ "FilterFirstNameJtoP", (Consumer c) => {return MainWindow.CharacterRangeTest(c.FirstName, 'J', 'P');}},
			{ "FilterFirstNameRtoZ", (Consumer c) => {return MainWindow.CharacterRangeTest(c.FirstName, 'R', 'Z');}},
			{ "FilterLastNameAtoG", (Consumer c) => {return MainWindow.CharacterRangeTest(c.LastName, 'A', 'G');}},
			{ "FilterLastNameHtoM", (Consumer c) => {return MainWindow.CharacterRangeTest(c.LastName, 'H', 'M');}},
			{ "FilterLastNameNtoR", (Consumer c) => {return MainWindow.CharacterRangeTest(c.LastName, 'N', 'R');}},
			{ "FilterLastNameStoZ", (Consumer c) => {return MainWindow.CharacterRangeTest(c.LastName, 'S', 'Z');}},
		};

		/// <summary>
		/// A sandbox for the ColumnView class.
		/// </summary>
		public MainWindow()
		{

			// The IDE managed components are initialize here.
			InitializeComponent();

			// The collection of consumers provides a run-time view model for the application.
			this.DataContext = this.consumerCollection;

			// This will add a handler for the AutoSize event that is generated from the column headers.
			this.AddHandler(ColumnViewColumnHeader.ClickEvent, new RoutedEventHandler(this.OnHeaderClick));

			// This sucks, but it's the only way to have the context menus find the command target.  Long story short: the Context Menu is not part of the visual
			// tree, so when it sends a command up the visual tree, it gets to the top of it's pop-up and can't find any parent.  So then the commanding looks for a
			// focus scope.  By default, an application has none.  By creating one here, the Context Menu command chain can find a target for the commands.
			FocusManager.SetIsFocusScope(this, true);
			FocusManager.SetFocusedElement(this, this.ListView);

		}

		/// <summary>
		/// Sizes the column to fit the largest cell.
		/// </summary>
		/// <param name="columnViewColumn">The column to be autosized.</param>
		void AutoSizeColumn(ColumnViewColumn columnViewColumn)
		{

			// This will create the same element that is used by the ColumnView templates.
			FrameworkElement frameworkElement = ColumnViewRowPresenter.CreateCell(columnViewColumn);

			// Now that we have the destination element we're going to pump all of the content through it and measure the maximum width of the columns.
			Double maxWidth = Double.MinValue;
			foreach (Object item in this.ListView.Items)
			{
				frameworkElement.DataContext = item;
				frameworkElement.UpdateLayout();
				frameworkElement.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
				maxWidth = Math.Max(frameworkElement.DesiredSize.Width, maxWidth);
			}

			// This will set the column to the maximum width calculated above.
			columnViewColumn.Width = maxWidth;

		}

		/// <summary>
		/// Tests to see if a string starts with the given range of characters.
		/// </summary>
		/// <param name="value">The string to be tested.</param>
		/// <param name="start">The starting character.</param>
		/// <param name="end">The ending character.</param>
		/// <returns>true if the given string starts with a character range, false if not.</returns>
		static Boolean CharacterRangeTest(String value, Char start, Char end)
		{
			Char firstChar = Char.ToUpper(value[0]);
			return start <= firstChar && firstChar <= end;
		}

		/// <summary>
		/// Changes the name of a city on an item.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnChangeCityClick(Object sender, RoutedEventArgs e)
		{
			IEditableCollectionView iEditableCollectionView = this.ListView.Items as IEditableCollectionView;
			iEditableCollectionView.EditItem(this.consumerCollection[3]);
			this.consumerCollection[3].City = "Aberdene";
			iEditableCollectionView.CommitEdit();
		}

		/// <summary>
		/// Changes the header.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnChangeHeaderClick(Object sender, RoutedEventArgs e)
		{
			this.ColumnView.Columns[4].Header = this.ColumnView.Columns[4].Header as String == "State" ? "Province" : "State";
		}

		/// <summary>
		/// Changes the horizontal alignment.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnChangeHorizontalAlignmentClick(Object sender, RoutedEventArgs e)
		{
			if (this.ColumnView.ColumnHeaderHorizontalAlignment == HorizontalAlignment.Right)
				this.ColumnView.ClearValue(ColumnView.ColumnHeaderHorizontalAlignmentProperty);
			else
				this.ColumnView.SetValue(ColumnView.ColumnHeaderHorizontalAlignmentProperty, HorizontalAlignment.Right);
		}

		/// <summary>
		/// Changes the binding on a column.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnChangeHeaderBindingClick(Object sender, RoutedEventArgs e)
		{
			Binding binding = this.ColumnView.Columns[4].DisplayMemberBinding as Binding;
			this.ColumnView.Columns[4].DisplayMemberBinding = new Binding(binding.Path.Path == "City" ? null : "City");
		}

		/// <summary>
		/// Changes the string format on a column.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnChangeStringFormat(Object sender, RoutedEventArgs e)
		{
			this.ColumnView.Columns[3].HeaderStringFormat = this.ColumnView.Columns[3].HeaderStringFormat == "dddd" ? "MMM" : "dddd";
		}

		/// <summary>
		/// Handles a filter command.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="routedEventArgs">The event data.</param>
		void OnFilter(Object sender, RoutedEventArgs routedEventArgs)
		{

			// The filters consist of a group of delegates.  The groups are used to collect filters that should be considered as inclusive.  That is, if a given 
			// record passes any one of the filters in a group, it is included in that group, but a given record must pass through all groups before it will be
			// include in the view.
			FilterItem filterItem = routedEventArgs.OriginalSource as FilterItem;
			Predicate<Consumer> filterHandler = MainWindow.filterDelegateMap[filterItem.Name];

			// If the filter has been selected, then include it in a filter group, if it has been deselected, then remove it from the filter group.
			if (filterItem.IsChecked == true)
			{

				// This will create a group if one doesn't already exist.
				ObservableCollection<Predicate<Consumer>> filterGroup;
				if (!this.filterGroups.TryGetValue(filterItem.GroupName, out filterGroup))
				{
					filterGroup = new ObservableCollection<Predicate<Consumer>>();
					this.filterGroups.Add(filterItem.GroupName, filterGroup);
					this.consumerCollection.View.Filters.Add(filterGroup);
				}

				// Add the selected filter to the group.  All records that pass this filter will be included in the group.
				filterGroup.Add(filterHandler);

			}
			else
			{

				// This will remove the filter from the group.
				ObservableCollection<Predicate<Consumer>> filterGroup = this.filterGroups[filterItem.GroupName];
				filterGroup.Remove(filterHandler);

				// If the group is empty, this will clean it out of the filter.
				if (filterGroup.Count == 0)
				{
					this.consumerCollection.View.Filters.Remove(filterGroup);
					this.filterGroups.Remove(filterItem.GroupName);
				}

			}

		}

		/// <summary>
		/// Autosizes a single column.
		/// </summary>
		/// <param name="sender">The Object that generated the routed command.</param>
		/// <param name="e">The executed routed event data.</param>
		void OnFitColumn(Object sender, ExecutedRoutedEventArgs e)
		{

			/// This will extract from the event the column header that was the original source of the event and resize the column.
			ColumnViewColumnHeader columnViewColumnHeader = e.OriginalSource as ColumnViewColumnHeader;
			if (columnViewColumnHeader != null)
				if (columnViewColumnHeader.Role != ColumnViewColumnHeaderRole.Padding)
					this.AutoSizeColumn(columnViewColumnHeader.Column);

		}

		/// <summary>
		/// Autosizes all columns.
		/// </summary>
		/// <param name="sender">The Object that generated the routed command.</param>
		/// <param name="e">The executed routed event data.</param>
		void OnFitAllColumns(Object sender, ExecutedRoutedEventArgs e)
		{

			// This will resize every column in the view.
			foreach (ColumnViewColumn columnViewcolumn in this.ColumnView.Columns)
				this.AutoSizeColumn(columnViewcolumn);

		}

		/// <summary>
		/// Handles a routed command to sort the column.
		/// </summary>
		/// <param name="sender">The Object that generated the event.</param>
		/// <param name="routedEventArgs">The routed event arguments.</param>
		void OnHeaderClick(Object sender, RoutedEventArgs routedEventArgs)
		{

			// Extract from the command arguments the column header that generated a request to be autosized and then resize the column.
			ColumnViewColumnHeader columnViewColumnHeader = routedEventArgs.OriginalSource as ColumnViewColumnHeader;
			ColumnViewColumn columnViewColumn = columnViewColumnHeader.Column;

			// This will prevent each change we make to the view from triggering an update of the viewed data.
			using (this.consumerCollection.View.DeferRefresh())
			{

				// Clear out the existing sort order from the view.
				this.consumerCollection.View.SortDescriptions.Clear();

				// Clear the sort order for the other columns.
				foreach (ColumnViewColumn siblingColumn in this.ColumnView.Columns)
					if (siblingColumn != columnViewColumn)
						siblingColumn.SortDirection = SortDirection.None;

				// If a DisplayMemberBinding exists for this column, we'll use that binding to find the property used to populate this column.
				Binding binding = columnViewColumn.DisplayMemberBinding as Binding;
				if (binding != null)
				{

					// This will toggle the sort order for this column (or select Ascending if the sort order hasn't been set) and set the corresonding sort order in
					// the collection view.
					columnViewColumn.SortDirection = columnViewColumn.SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
					ListSortDirection listSortDirection =
						columnViewColumn.SortDirection == SortDirection.Descending ?
						ListSortDirection.Descending :
						ListSortDirection.Ascending;
					this.consumerCollection.View.SortDescriptions.Add(new SortDescription(binding.Path.Path, listSortDirection));

				}

			}

		}

		/// <summary>
		/// Sets the style for all columns.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnSetAllColumnsStyleClick(Object sender, RoutedEventArgs e)
		{
			this.ColumnView.ColumnHeaderContainerStyle = this.ColumnView.ColumnHeaderContainerStyle == null ? this.Resources["columnHeaderStyle"] as Style : null;
		}

		/// <summary>
		/// Sets the style for a single column.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnSetColumnStyleClick(Object sender, RoutedEventArgs e)
		{
			this.ColumnView.Columns[4].HeaderContainerStyle =
				this.ColumnView.Columns[4].HeaderContainerStyle == null ?
				this.Resources["columnHeaderStyle"] as Style :
				null;
		}

	}

}
