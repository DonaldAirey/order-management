namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Data;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Input;
	using System.Windows.Markup;
	using Teraque.Windows;
	using Teraque.Windows.Controls;
	using Teraque.Windows.Input;

	/// <summary>
	/// The specific implementation of the generic WorkingOrderView&lt;Type&gt; class.
	/// </summary>
	public class WorkingOrderView : WorkingOrderView<WorkingOrder> { }

	/// <summary>
	/// Interaction logic for WorkingOrderView.xaml
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class WorkingOrderView<TType> : ListViewport, IDisposable where TType : WorkingOrder
	{

		/// <summary>
		/// The unique identifier of the blotter that contains the working orders.
		/// </summary>
		Guid blotterIdField;

		/// <summary>
		/// The columns based view of the items.
		/// </summary>
		ColumnView columnViewField;

		/// <summary>
		/// A set of groups used to filter the view.  Each group contains a set of predicates, any one of which must be satisified for the group to be viewed.
		/// </summary>
		Dictionary<String, ObservableCollection<Predicate<TType>>> filterGroups = new Dictionary<String, ObservableCollection<Predicate<TType>>>();

		/// <summary>
		/// Maps the name of the FilterItem to the predicate that handles that filter.
		/// </summary>
		static Dictionary<String, Predicate<TType>> filterDelegateMap = new Dictionary<String, Predicate<TType>>()
		{
			{ "FilterStatusNew", (TType w) => {return w.StatusCode == StatusCode.New;}},
			{ "FilterStatusActive", (TType w) => {return w.StatusCode == StatusCode.Active;}},
			{ "FilterStatusPartiallyFilled", (TType w) => {return w.StatusCode == StatusCode.PartiallyFilled;}},
			{ "FilterStatusFilled", (TType w) => {return w.StatusCode == StatusCode.Filled;}},
			{ "FilterSymbolAtoG", (TType w) => {return WorkingOrderView<TType>.CharacterRangeTest(w.Symbol, 'A', 'G');}},
			{ "FilterSymbolHtoM", (TType w) => {return WorkingOrderView<TType>.CharacterRangeTest(w.Symbol, 'H', 'M');}},
			{ "FilterSymbolNtoR", (TType w) => {return WorkingOrderView<TType>.CharacterRangeTest(w.Symbol, 'N', 'R');}},
			{ "FilterSymbolStoZ", (TType w) => {return WorkingOrderView<TType>.CharacterRangeTest(w.Symbol, 'S', 'Z');}},
			{ "FilterSideBuy", (TType w) => {return w.SideCode == SideCode.Buy;}},
			{ "FilterSideBuyCover", (TType w) => {return w.SideCode == SideCode.BuyCover;}},
			{ "FilterSideSell", (TType w) => {return w.SideCode == SideCode.Sell;}},
			{ "FilterSideSellShort", (TType w) => {return w.SideCode == SideCode.SellShort;}},
			{ "FilterSourceOrderQuantity0to1000", (TType w) => {return WorkingOrderView<TType>.DecimalRangeTest(w.SourceOrderQuantity, 0.0M, 1000.0M);}},
			{ "FilterSourceOrderQuantity1000to10000", (TType w) => {return WorkingOrderView<TType>.DecimalRangeTest(w.SourceOrderQuantity, 1000.0M, 10000.0M);}},
			{ "FilterSourceOrderQuantity10000to100000", (TType w) => {return WorkingOrderView<TType>.DecimalRangeTest(w.SourceOrderQuantity, 10000.0M, 100000.0M);}},
			{ "FilterSourceOrderQuantityAbove100000", (TType w) => {return WorkingOrderView<TType>.DecimalRangeTest(w.SourceOrderQuantity, 100000.0M, Decimal.MaxValue);}},
			{ "FilterMarketValue0to100000", (TType w) => {return WorkingOrderView<TType>.DecimalRangeTest(w.MarketValue, 0.0M, 100000.0M);}},
			{ "FilterMarketValue100000to500000", (TType w) => {return WorkingOrderView<TType>.DecimalRangeTest(w.MarketValue, 100000.0M, 500000.0M);}},
			{ "FilterMarketValue500000to1000000", (TType w) => {return WorkingOrderView<TType>.DecimalRangeTest(w.MarketValue, 500000.0M, 1000000.0M);}},
			{ "FilterMarketValueAbove1000000", (TType w) => {return WorkingOrderView<TType>.DecimalRangeTest(w.MarketValue, 1000000.0M, Decimal.MaxValue);}},
		};

		/// <summary>
		/// This is the list of orders assigned to this blotter.
		/// </summary>
		WorkingOrderCollection<TType> workingOrderCollection;

		/// <summary>
		/// Initializes the WorkingOrderView class.
		/// </summary>
		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"), SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static WorkingOrderView()
		{

			// This is a complex control and will manage it's own focus scope.
			FocusManager.IsFocusScopeProperty.OverrideMetadata(typeof(WorkingOrderView<TType>), new FrameworkPropertyMetadata(true));

		}

		/// <summary>
		/// Initializes a new instance of the WorkingOrderView class.
		/// </summary>
		public WorkingOrderView()
		{

			// Installing the vent handlers will only confuse the design surface.
			if (!DesignerProperties.GetIsInDesignMode(this))
			{

				// This will initialize and destroy the managed resources of this window.
				this.Loaded += this.OnLoaded;
				this.Unloaded += this.OnUnloaded;

				// This will add a handler for the AutoSize event that is generated from the column headers.
				this.AddHandler(ColumnViewColumnHeader.ClickEvent, new RoutedEventHandler(this.OnHeaderClick));

			}

			// These bindings handle the column commands.
			this.CommandBindings.Add(new CommandBinding(Commands.FitColumn, this.OnFitColumn));
			this.CommandBindings.Add(new CommandBinding(Commands.FitAllColumns, this.OnFitAllColumns));
			this.CommandBindings.Add(new CommandBinding(Commands.Filter, this.OnFilterColumn));

			// These are the various views supported by this items control (which is also a page).
			ViewContainer viewContainer = XamlReader.Load(
				Application.GetResourceStream(new Uri("/Teraque.AssetNetwork.Blotter;component/Views/ColumnView.xaml", UriKind.RelativeOrAbsolute)).Stream) as ViewContainer;
			this.columnViewField = viewContainer.View as ColumnView;
			this.View = this.columnViewField;

		}

		/// <summary>
		/// Finalize this instance of the WorkingOrderView class.
		/// </summary>
		~WorkingOrderView()
		{

			// This will dispose of the unmanaged resources.
			this.Dispose(false);

		}

		/// <summary>
		/// The ColumnView used for this view.
		/// </summary>
		protected ColumnView ColumnView
		{
			get
			{
				return this.columnViewField;
			}
			set
			{
				this.columnViewField = value;
			}
		}

		/// <summary>
		/// Gets the collection of WorkingOrders.
		/// </summary>
		public WorkingOrderCollection<TType> WorkingOrders
		{
			get
			{
				return this.workingOrderCollection;
			}
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
			foreach (Object item in this.Items)
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
		protected static Boolean CharacterRangeTest(String value, Char start, Char end)
		{

			// Validate the parameters.
			if (value == null)
				throw new ArgumentNullException("value");

			// Validate the range.
			Char firstChar = Char.ToUpper(value[0], CultureInfo.InvariantCulture);
			return start <= firstChar && firstChar <= end;

		}

		/// <summary>
		/// Tests to see if the given System.Decimal value falls within the given range.
		/// </summary>
		/// <param name="value">The value to be tested.</param>
		/// <param name="start">The starting value of the range.</param>
		/// <param name="end">The ending value of the range.</param>
		/// <returns>true if the given value falls within the given range, false if not.</returns>
		protected static Boolean DecimalRangeTest(Decimal value, Decimal start, Decimal end)
		{
			return start <= value && value < end;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{

			// Call the virtual method to allow derived classes to clean up resources.
			this.Dispose(true);

			// Since we took care of cleaning up the resources, there is no need to call the finalizer.
			GC.SuppressFinalize(this);

		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing">true to indicate that the object is being disposed, false to indicate that the object is being finalized.</param>
		protected virtual void Dispose(Boolean disposing)
		{

			// This will remove this object from the data model events.
			if (disposing)
			{

				// The WorkingOrderCollection links itself into the data model.  In order to properly release all references so the collection can be garbage
				// collected, we need to dispose of the managed resources used by the collection (this basically removes the event handlers that tie the collection
				// to the data model).
				this.ItemsSource = null;
				if (this.workingOrderCollection != null)
				{
					this.workingOrderCollection.Dispose();
					this.workingOrderCollection = null;
				}

			}

		}

		/// <summary>
		/// Merge the filters.
		/// </summary>
		/// <param name="filterDelegateMap">A set of delegates that are used to filter the view.</param>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		protected static void MergeFilters(Dictionary<String, Predicate<TType>> filterDelegateMap)
		{

			// Validate the parameters.
			if (filterDelegateMap == null)
				throw new ArgumentNullException("filterDelegateMap");

			// Merge the given set of delegates with the existing set.
			foreach (KeyValuePair<String, Predicate<TType>> keyValuePair in filterDelegateMap)
				WorkingOrderView<TType>.filterDelegateMap.Add(keyValuePair.Key, keyValuePair.Value);

		}

		/// <summary>
		/// Handles a filter command.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="routedEventArgs">The event data.</param>
		void OnFilterColumn(Object sender, RoutedEventArgs routedEventArgs)
		{

			// The filters consist of a group of delegates.  The groups are used to collect filters that should be considered as inclusive.  That is, if a given 
			// record passes any one of the filters in a group, it is included in that group, but a given record must pass through all groups before it will be
			// include in the view.
			FilterItem filterItem = routedEventArgs.OriginalSource as FilterItem;
			Predicate<TType> filterHandler = WorkingOrderView<TType>.filterDelegateMap[filterItem.Name];

			// If the filter has been selected, then include it in a filter group, if it has been deselected, then remove it from the filter group.
			if (filterItem.IsChecked == true)
			{

				// This will create a group if one doesn't already exist or add the filter to an existing group.  Note that we initialize the filter group with a 
				// filter before adding it.  This will keep the filter from performing an unnessary filtering pass on the data with an empty filter group.
				ObservableCollection<Predicate<TType>> filterGroup;
				if (!this.filterGroups.TryGetValue(filterItem.GroupName, out filterGroup))
				{

					// The 'GroupName' acts like a set of parenthesis in an expression.  All items in a filter group act like a logical 'Or' statement; if an item
					// passes any single filter in the group then the item has passed that group's filter.  The View doesn't have any native logic for symbolic
					// grouping, so we'll use the 'filterGroups' dictionary to organize our groups based on XAML tags.
					filterGroup = new ObservableCollection<Predicate<TType>>();
					this.filterGroups.Add(filterItem.GroupName, filterGroup);
					filterGroup.Add(filterHandler);
					this.workingOrderCollection.View.Filters.Add(filterGroup);

				}
				else
				{

					// If a group already exists for this filter, then simply add it to the group.
					filterGroup.Add(filterHandler);

				}

			}
			else
			{

				// This will remove the filter from the group.
				ObservableCollection<Predicate<TType>> filterGroup = this.filterGroups[filterItem.GroupName];

				// If this is the last item in the filter group, then we're going to remove the entire group from the filter.  Note that we don't simply remove the
				// filter first and then check to see if the group is empty.  This would cause two passes through the filtering logic instead of one.  Each time the
				// filter or the filter group is modified, a filter pass is generated.
				if (filterGroup.Count == 1)
				{

					// If this is the last item in the group, then we're going to remove the group first, then remove the filter from the group (which is probably 
					// unnecessary since garbage collection will get them both, but it feels more symetrical this way).
					this.workingOrderCollection.View.Filters.Remove(filterGroup);
					filterGroup.Remove(filterHandler);

					// Remove the symbolic reference for this group from the dictionary.
					this.filterGroups.Remove(filterItem.GroupName);

				}
				else
				{

					// If there are other filters in this group, then just remove the specified filter.
					filterGroup.Remove(filterHandler);

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
			foreach (ColumnViewColumn columnViewcolumn in this.columnViewField.Columns)
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
			using (this.workingOrderCollection.View.DeferRefresh())
			{

				// Clear out the existing sort order from the view.
				this.workingOrderCollection.View.SortDescriptions.Clear();

				// Clear the sort order for the other columns.
				foreach (ColumnViewColumn siblingColumn in this.columnViewField.Columns)
					if (siblingColumn != columnViewColumn)
						siblingColumn.SortDirection = SortDirection.None;

				// If a DisplayMemberBinding exists for this column, we'll use that binding to find the property used to populate this column.
				if (columnViewColumn.SortPath != null)
				{

					// This will toggle the sort order for this column (or select Ascending if the sort order hasn't been set) and set the corresonding sort order in
					// the collection view.
					columnViewColumn.SortDirection = columnViewColumn.SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
					ListSortDirection listSortDirection =
						columnViewColumn.SortDirection == SortDirection.Descending ?
						ListSortDirection.Descending :
						ListSortDirection.Ascending;
					this.workingOrderCollection.View.SortDescriptions.Add(new SortDescription(columnViewColumn.SortPath, listSortDirection));

				}

			}

		}

		/// <summary>
		/// Creates the collection of &lt;TType&gt; used by the view.
		/// </summary>
		/// <param name="blotterId">The blotter from which to construct the collection.</param>
		/// <returns>A collection of &lt;TType&gt; View Model records belonging to the given blotter.</returns>
		protected virtual WorkingOrderCollection<TType> CreateCollectionCore(Guid blotterId)
		{

			// This is used to construct a View Model collection for the viewer.  It can be overridden in a descenant to add additional properties to the View 
			// Model elements.
			return new WorkingOrderCollection<TType>(blotterId);

		}
	
		/// <summary>
		/// Occurs when the element is laid out, rendered, and ready for interaction.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnLoaded(Object sender, RoutedEventArgs e)
		{

			// The view will be based on the blotter currently selected in the frame.
			AssetNetworkItem assetNetworkItem = this.DataContext as AssetNetworkItem;
			this.blotterIdField = assetNetworkItem.EntityId;

			// This provides the collection of WorkingOrders and a view of that collection to the ListView.
			this.workingOrderCollection = this.CreateCollectionCore(blotterIdField);
			this.ItemsSource = this.workingOrderCollection.View;

		}

		/// <summary>
		/// Occurs when the element is removed from within an element tree of loaded elements.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnUnloaded(Object sender, RoutedEventArgs e)
		{

			// Dispose of the managed resources.
			this.Dispose();

		}

	}

}
