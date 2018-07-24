namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Markup;

	/// <summary>
	/// Represents a column that displays data.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[ContentProperty("Header")]
	[StyleTypedProperty(Property = "HeaderContainerStyle", StyleTargetType = typeof(ColumnViewColumnHeader))]
	public class ColumnViewColumn : DependencyObject, INotifyPropertyChanged
	{

		/// <summary>
		/// The default width of a column.
		/// </summary>
		const Double defaultMinWidth = 80.0;

		/// <summary>
		/// The member in the data context to which the column is bound.
		/// </summary>
		BindingBase displayMemberBinding;

		/// <summary>
		/// Identifies the CellTemplate dependency property.
		/// </summary>
		public static readonly DependencyProperty CellTemplateProperty = DependencyProperty.Register(
			"CellTemplate",
			typeof(DataTemplate),
			typeof(ColumnViewColumn),
			new PropertyMetadata(new PropertyChangedCallback(ColumnViewColumn.OnPropertyChanged)));

		/// <summary>
		/// Identifies the CellTemplateSelector dependency property.
		/// </summary>
		public static readonly DependencyProperty CellTemplateSelectorProperty = DependencyProperty.Register(
			"CellTemplateSelector",
			typeof(DataTemplateSelector),
			typeof(ColumnViewColumn),
			new PropertyMetadata(new PropertyChangedCallback(ColumnViewColumn.OnPropertyChanged)));

		/// <summary>
		/// The list of filters use to select the rows in the view.
		/// </summary>
		ObservableCollection<FilterItem> filterItemCollection = new ObservableCollection<FilterItem>();

		/// <summary>
		/// Identifies the Description dependency property.
		/// </summary>
		public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
			"Description",
			typeof(String),
			typeof(ColumnViewColumn));

		/// <summary>
		/// Identifies the HasFiltersProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty HasFiltersProperty;

		/// <summary>
		/// Identifies the hasFiltersProperty dependency property key.
		/// </summary>
		internal static readonly DependencyPropertyKey hasFiltersPropertyKey = DependencyProperty.RegisterReadOnly(
			"HasFilters",
			typeof(Boolean),
			typeof(ColumnViewColumn),
			new FrameworkPropertyMetadata());

		/// <summary>
		/// Identifies the HeaderContainerStyle dependency property.
		/// </summary>
		public static readonly DependencyProperty HeaderContainerStyleProperty = DependencyProperty.Register(
			"HeaderContainerStyle",
			typeof(Style),
			typeof(ColumnViewColumn),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(ColumnViewColumn.OnPropertyChanged)));

		/// <summary>
		/// Identifies the HeaderContext dependency property.
		/// </summary>
		public static readonly DependencyProperty HeaderContextMenuProperty = DependencyProperty.Register(
			"HeaderContextMenu",
			typeof(ContextMenu),
			typeof(ColumnViewColumn));

		/// <summary>
		/// Identifies the HeaderHorizontalAlignment dependency property.
		/// </summary>
		public static readonly DependencyProperty HeaderHorizontalAlignmentProperty = DependencyProperty.Register(
			"HeaderHorizontalAlignment",
			typeof(HorizontalAlignment),
			typeof(ColumnViewColumn),
			new FrameworkPropertyMetadata(HorizontalAlignment.Left, new PropertyChangedCallback(ColumnViewColumn.OnPropertyChanged)));

		/// <summary>
		/// Identifies the Header dependency property. 
		/// </summary>
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
			"Header",
			typeof(Object),
			typeof(ColumnViewColumn),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(ColumnViewColumn.OnPropertyChanged)));

		/// <summary>
		/// Identifies the HeaderStringFormat dependency property.
		/// </summary>
		public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register(
			"HeaderStringFormat",
			typeof(String),
			typeof(ColumnViewColumn),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(ColumnViewColumn.OnPropertyChanged)));

		/// <summary>
		/// Identifies the HeaderTemplate dependency property.
		/// </summary>
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
			"HeaderTemplate", 
			typeof(DataTemplate),
			typeof(ColumnViewColumn),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(ColumnViewColumn.OnPropertyChanged)));

		/// <summary>
		/// Identifies the HeaderTemplateSelector dependency property.
		/// </summary>
		public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register(
			"HeaderTemplateSelector",
			typeof(DataTemplateSelector),
			typeof(ColumnViewColumn),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(ColumnViewColumn.OnPropertyChanged)));

		/// <summary>
		/// Identifies the HeaderToolTip dependency property.
		/// </summary>
		public static readonly DependencyProperty HeaderToolTipProperty = DependencyProperty.Register(
			"HeaderToolTip",
			typeof(Object),
			typeof(ColumnViewColumn),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(ColumnViewColumn.OnPropertyChanged)));

		/// <summary>
		/// Identifies the HeaderVerticalAlignment dependency property.
		/// </summary>
		public static readonly DependencyProperty HeaderVerticalAlignmentProperty = DependencyProperty.Register(
			"HeaderVerticalAlignment",
			typeof(VerticalAlignment),
			typeof(ColumnViewColumn),
			new FrameworkPropertyMetadata(VerticalAlignment.Center, new PropertyChangedCallback(ColumnViewColumn.OnPropertyChanged)));

		/// <summary>
		/// Identifies the SortPath dependency property.
		/// </summary>
		public static readonly DependencyProperty SortPathProperty = DependencyProperty.Register(
			"SortPath",
			typeof(String),
			typeof(ColumnViewColumn));

		/// <summary>
		/// Used to determine the order of this column when it appears in a fixed list.  Mainly used for context menus.
		/// </summary>
		Nullable<Int32> ordinal;

		/// <summary>
		/// Identifies the IsVisible dependency property.
		/// </summary>
		public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register(
			"IsVisible",
			typeof(Boolean),
			typeof(ColumnViewColumn),
			new FrameworkPropertyMetadata(true, new PropertyChangedCallback(ColumnViewColumn.OnPropertyChanged)));

		/// <summary>
		/// Identifies the MinWidth dependency property.
		/// </summary>
		public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register(
			"MinWidth",
			typeof(Double),
			typeof(ColumnViewColumn),
			new PropertyMetadata(ColumnViewColumn.defaultMinWidth, new PropertyChangedCallback(ColumnViewColumn.OnMinWidthChanged)));

		/// <summary>
		/// Identifies the Width dependency property.
		/// </summary>
		public static readonly DependencyProperty WidthProperty = FrameworkElement.WidthProperty.AddOwner(
			typeof(ColumnViewColumn),
			new PropertyMetadata(
				ColumnViewColumn.defaultMinWidth,
				new PropertyChangedCallback(ColumnViewColumn.OnWidthChanged),
				new CoerceValueCallback(ColumnViewColumn.CoerceWidth)));

		/// <summary>
		/// Identifies the SortDirection dependency property.
		/// </summary>
		public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.Register(
			"SortDirection",
			typeof(SortDirection),
			typeof(ColumnViewColumn),
			new FrameworkPropertyMetadata(SortDirection.None, new PropertyChangedCallback(ColumnViewColumn.OnPropertyChanged)));

		/// <summary>
		/// Occurs when the value of any ColumnViewColumn property changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Initializes the ColumnViewColumn class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static ColumnViewColumn()
		{

			// This must be done in the static constructor to guarantee the DependencyProperty is initialized properly.
			ColumnViewColumn.HasFiltersProperty = ColumnViewColumn.hasFiltersPropertyKey.DependencyProperty;

		}

		/// <summary>
		/// Initializes a new instance of the ColumnViewColumn class.
		/// </summary>
		public ColumnViewColumn()
		{

			// In order to keep the 'HasFilters' property reconciled to the list of filters, we need to monitor the collection for changes.
			this.filterItemCollection.CollectionChanged += this.OnFilterCollectionChanged;

		}

		/// <summary>
		/// Coerce the maximum value of the range.
		/// </summary>
		/// <param name="dependencyObject">The Object on which the property exists.</param>
		/// <param name="value">The new value of the property, prior to any coercion attempt.</param>
		/// <returns>The coerced value (with appropriate type).</returns>
		static Object CoerceWidth(DependencyObject dependencyObject, Object value)
		{

			// Don't let the width exceed the minimum value.
			ColumnViewColumn columnViewColumn = dependencyObject as ColumnViewColumn;
			return Math.Max((Double)value, columnViewColumn.MinWidth);

		}

		/// <summary>
		/// Handles a change to the collection of filters.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event data.</param>
		void OnFilterCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{

			// This will keep the 'HasFilters' property reconcilled to the state of the list.
			this.SetValue(ColumnViewColumn.hasFiltersPropertyKey, this.filterItemCollection.Count != 0);

		}

		/// <summary>
		/// Handles a change to the MinWidth property.
		/// </summary>
		/// <param name="dependencyObject">The Object that originated the event.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event arguments.</param>
		static void OnMinWidthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Don't let the width exceed the minimum value.
			ColumnViewColumn columnViewColumn = dependencyObject as ColumnViewColumn;
			columnViewColumn.CoerceValue(ColumnViewColumn.WidthProperty);

		}

		/// <summary>
		/// Handles a change to a generic property.
		/// </summary>
		/// <param name="dependencyObject">The Object that originated the event.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The property change event arguments.</param>
		static void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will raise the INotifyPropertyChange event for any of the generic properties attached to this handler.
			ColumnViewColumn columnViewColumn = dependencyObject as ColumnViewColumn;
			columnViewColumn.OnPropertyChanged(dependencyPropertyChangedEventArgs.Property.Name);

			// This will provide a default value for the description when a string header is defined.  The designer can always override it with an explicit 
			// description of the column.  The description shows up in the context menu and the 'Choose Column Details' dialog box.
			if (dependencyPropertyChangedEventArgs.Property == ColumnViewColumn.HeaderProperty)
			{
				String headerText = dependencyPropertyChangedEventArgs.NewValue as String;
				if (headerText != null && columnViewColumn.Description == null)
					columnViewColumn.Description = headerText;
			}

		}

		/// <summary>
		/// Raises the INotifyPropertyChanged.PropertyChanged event.
		/// </summary>
		/// <param name="propertyName">The name of the property that has changed.</param>
		protected virtual void OnPropertyChanged(String propertyName)
		{

			// Raise the event for anyone attached to this event handler.
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

		}

		/// <summary>
		/// Handles a change to the Width property.
		/// </summary>
		/// <param name="dependencyObject">The owner of the property.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event arguments.</param>
		static void OnWidthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// If the column width is set to Double.NaN then we need to auto size the column.  Otherwise the column is given a specific width for all rows.
			ColumnViewColumn columnViewColumn = dependencyObject as ColumnViewColumn;
			columnViewColumn.OnPropertyChanged(WidthProperty.Name);

		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "{0} Header={1}", this.GetType(), this.Header);
		}

		/// <summary>
		/// Gets or sets the template to use to display the contents of a column cell.
		/// </summary>
		public DataTemplate CellTemplate
		{
			get
			{
				return this.GetValue(ColumnViewColumn.CellTemplateProperty) as DataTemplate;
			}
			set
			{
				this.SetValue(ColumnViewColumn.CellTemplateProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a DataTemplateSelector that determines the template to use to display cells in a column.
		/// </summary>
		public DataTemplateSelector CellTemplateSelector
		{
			get
			{
				return this.GetValue(ColumnViewColumn.CellTemplateSelectorProperty) as DataTemplateSelector;
			}
			set
			{
				this.SetValue(ColumnViewColumn.CellTemplateSelectorProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the content of the description of the column.
		/// </summary>
		public String Description
		{
			get
			{
				return this.GetValue(ColumnViewColumn.DescriptionProperty) as String;
			}
			set
			{
				this.SetValue(ColumnViewColumn.DescriptionProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the data item to bind to for this column.
		/// </summary>
		public BindingBase DisplayMemberBinding
		{
			get
			{
				return this.displayMemberBinding;
			}
			set
			{
				if (this.displayMemberBinding != value)
				{
					this.displayMemberBinding = value;
					this.OnPropertyChanged("DisplayMemberBinding");
				}
			}
		}

		/// <summary>
		/// Gets the collection of FilterItems use to select the rows that will appear in the view.
		/// </summary>
		public ObservableCollection<FilterItem> Filters
		{
			get
			{
				return this.filterItemCollection;
			}
		}

		/// <summary>
		/// Gets an indication of whether or not the column has any filters defined for it.
		/// </summary>
		public Boolean HasFilters
		{
			get
			{
				return (Boolean)this.GetValue(ColumnViewColumn.HasFiltersProperty);
			}
		}

		/// <summary>
		/// Gets or sets the content of the header of a ColumnViewColumn.
		/// </summary>
		public Object Header
		{
			get
			{
				return this.GetValue(ColumnViewColumn.HeaderProperty);
			}
			set
			{
				this.SetValue(ColumnViewColumn.HeaderProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the style to use for the header of the ColumnViewColumn.
		/// </summary>
		public Style HeaderContainerStyle
		{
			get
			{
				return this.GetValue(ColumnViewColumn.HeaderContainerStyleProperty) as Style;
			}
			set
			{
				this.SetValue(ColumnViewColumn.HeaderContainerStyleProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a ContextMenu for the ColumnViewColumn.
		/// </summary>
		public ContextMenu HeaderContextMenu
		{
			get
			{
				return this.GetValue(HeaderContextMenuProperty) as ContextMenu;
			}
			set
			{
				this.SetValue(HeaderContextMenuProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a composite String that specifies how to format the Header property if it is displayed as a String.
		/// </summary>
		public String HeaderStringFormat
		{
			get
			{
				return this.GetValue(ColumnViewColumn.HeaderStringFormatProperty) as String;
			}
			set
			{
				this.SetValue(ColumnViewColumn.HeaderStringFormatProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the template to use to display the content of the column header. 
		/// </summary>
		public DataTemplate HeaderTemplate
		{
			get
			{
				return this.GetValue(ColumnViewColumn.HeaderTemplateProperty) as DataTemplate;
			}
			set
			{
				this.SetValue(ColumnViewColumn.HeaderTemplateProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the DataTemplateSelector that provides logic to select the template to use to display the column header.
		/// </summary>
		public DataTemplateSelector HeaderTemplateSelector
		{
			get
			{
				return this.GetValue(ColumnViewColumn.HeaderTemplateSelectorProperty) as DataTemplateSelector;
			}
			set
			{
				this.SetValue(ColumnViewColumn.HeaderTemplateSelectorProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the HorizontalAlignment for the column header.
		/// </summary>
		public HorizontalAlignment HeaderHorizontalAlignment
		{
			get
			{
				return (HorizontalAlignment)this.GetValue(ColumnViewColumn.HeaderHorizontalAlignmentProperty);
			}
			set
			{
				this.SetValue(ColumnViewColumn.HeaderHorizontalAlignmentProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the content of a tooltip that appears when the mouse pointer pauses over one of the column headers.
		/// </summary>
		public Object HeaderToolTip
		{
			get
			{
				return this.GetValue(HeaderToolTipProperty);
			}
			set
			{
				this.SetValue(HeaderToolTipProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the VerticalAlignment for the column header.
		/// </summary>
		public VerticalAlignment HeaderVerticalAlignment
		{
			get
			{
				return (VerticalAlignment)this.GetValue(ColumnViewColumn.HeaderVerticalAlignmentProperty);
			}
			set
			{
				this.SetValue(ColumnViewColumn.HeaderVerticalAlignmentProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the minimum width for a column.
		/// </summary>
		public Boolean IsVisible
		{
			get
			{
				return (Boolean)this.GetValue(ColumnViewColumn.IsVisibleProperty);
			}
			set
			{
				this.SetValue(ColumnViewColumn.IsVisibleProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the minimum width for a column.
		/// </summary>
		public Double MinWidth
		{
			get
			{
				return (Double)this.GetValue(ColumnViewColumn.MinWidthProperty);
			}
			set
			{
				this.SetValue(ColumnViewColumn.MinWidthProperty, value);
			}
		}

		/// <summary>
		/// Gets of sets an indication of whether or not the column should appear in the commonly used items in the context menu.
		/// </summary>
		public Nullable<Int32> Ordinal
		{
			get
			{
				return this.ordinal;
			}
			set
			{
				this.ordinal = value;
			}
		}

		/// <summary>
		/// Gets or sets the width of the column. 
		/// </summary>
		public Double Width
		{
			get
			{
				return (Double)this.GetValue(ColumnViewColumn.WidthProperty);
			}
			set
			{
				this.SetValue(ColumnViewColumn.WidthProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the width of the column. 
		/// </summary>
		public SortDirection SortDirection
		{
			get
			{
				return (SortDirection)this.GetValue(ColumnViewColumn.SortDirectionProperty);
			}
			set
			{
				this.SetValue(ColumnViewColumn.SortDirectionProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the width of the column. 
		/// </summary>
		public String SortPath
		{
			get
			{
				return this.GetValue(ColumnViewColumn.SortPathProperty) as String;
			}
			set
			{
				this.SetValue(ColumnViewColumn.SortPathProperty, value);
			}
		}

	}

}
