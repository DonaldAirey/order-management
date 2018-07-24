namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Markup;

	/// <summary>
	/// Represents a view mode that displays data items in columns for a ListView control.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[ContentProperty("Columns")]
	public class ColumnView : ViewBase, IAddChild
	{

		/// <summary>
		/// Identifies the AllowsColumnReorder dependency property.
		/// </summary>
		public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register(
			"ItemContainerStyle",
			typeof(Style),
			typeof(ColumnView));

		/// <summary>
		/// Identifies the AllowsColumnReorder dependency property.
		/// </summary>
		public static readonly DependencyProperty AllowsColumnReorderProperty = DependencyProperty.Register(
			"AllowsColumnReorder",
			typeof(Boolean),
			typeof(ColumnView),
			new FrameworkPropertyMetadata(true));

		/// <summary>
		/// Identifies the ColumnCollection attached property.
		/// </summary>
		public static readonly DependencyProperty ColumnCollectionProperty = DependencyProperty.RegisterAttached(
			"ColumnCollection",
			typeof(ColumnViewColumnCollection),
			typeof(ColumnView));

		/// <summary>
		/// Identifies the ColumnHeaderContainerStyle dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnHeaderContainerStyleProperty = DependencyProperty.Register(
			"ColumnHeaderContainerStyle",
			typeof(Style), 
			typeof(ColumnView));

		/// <summary>
		/// Identifies the ColumnHeaderContext dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnHeaderContextMenuProperty = DependencyProperty.Register(
			"ColumnHeaderContextMenu",
			typeof(ContextMenu),
			typeof(ColumnView));

		/// <summary>
		/// Identifies the ColumnHeaderStringFormat dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnHeaderStringFormatProperty = DependencyProperty.Register(
			"ColumnHeaderStringFormat",
			typeof(String),
			typeof(ColumnView));

		/// <summary>
		/// Identifies the ColumnHeaderTemplate dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnHeaderTemplateProperty = DependencyProperty.Register(
			"ColumnHeaderTemplate",
			typeof(DataTemplate),
			typeof(ColumnView));

		/// <summary>
		/// Identifies the ColumnHeaderTemplateSelector dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnHeaderTemplateSelectorProperty = DependencyProperty.Register(
			"ColumnHeaderTemplateSelector",
			typeof(DataTemplateSelector),
			typeof(ColumnView));

		/// <summary>
		/// Identifies the ColumnHeaderHorizontalAlignment dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnHeaderHorizontalAlignmentProperty = DependencyProperty.Register(
			"ColumnHeaderHorizontalAlignment",
			typeof(Nullable<HorizontalAlignment>),
			typeof(ColumnView),
			new FrameworkPropertyMetadata(null));

		/// <summary>
		/// Identifies the ColumnHeaderToolTip dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnHeaderToolTipProperty = DependencyProperty.Register(
			"ColumnHeaderToolTip",
			typeof(Object),
			typeof(ColumnView));

		/// <summary>
		/// Identifies the ColumnHeaderVerticalAlignment dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnHeaderVerticalAlignmentProperty = DependencyProperty.Register(
			"ColumnHeaderVerticalAlignment",
			typeof(VerticalAlignment),
			typeof(ColumnView),
			new FrameworkPropertyMetadata(VerticalAlignment.Center));

		/// <summary>
		/// The collection of columns for this view.
		/// </summary>
		ColumnViewColumnCollection columns = new ColumnViewColumnCollection();

		/// <summary>
		/// Initialize a new instance of the ColumnView class.
		/// </summary>
		public ColumnView()
		{

			// Each ColumnView gets a default context menu suitable for managing most columns.
			this.ColumnHeaderContextMenu = new ColumnViewContextMenu();

		}

		/// <summary>
		/// Adds a child object.
		/// </summary>
		/// <param name="value">The child object to add.</param>
		public void AddChild(Object value)
		{

			// Validate the parameters.
			if (value == null)
				throw new ArgumentNullException("value");

			// Validate the parameter.
			ColumnViewColumn columnViewColumn = value as ColumnViewColumn;
			if (columnViewColumn == null)
				throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "Illegal type of child {0}", value.GetType()));

			// Add the column definition to the view.
			this.Columns.Add(columnViewColumn);

		}

		/// <summary>
		/// Adds the text content of a node to the object.
		/// </summary>
		/// <param name="text">The text to add to the object.</param>
		public void AddText(String text)
		{

			// We assume any text is the heading for a column and that it's width is calculated automatically.
			this.AddChild(new ColumnViewColumn { Header = text });

		}

		/// <summary>
		/// Removes all bindings and styling that are set for an item.
		/// </summary>
		/// <param name="item">The ListViewItem to remove settings from.</param>
		protected override void ClearItem(ListViewItem item)
		{
			base.ClearValue(ColumnView.ColumnCollectionProperty);
			this.ClearItem(item);
		}

		/// <summary>
		/// Prepares an item in the view for display by binding it to the ColumnCollection property.
		/// </summary>
		/// <param name="item">The item to prepare for display.</param>
		protected override void PrepareItem(ListViewItem item)
		{

			// Validate the parameters.
			if (item == null)
				throw new ArgumentNullException("item");

			// All items in this view are given a copy of the column collection.
			base.PrepareItem(item);
			item.SetValue(ColumnView.ColumnCollectionProperty, this.columns);
			if (this.ItemContainerStyle != null)
				item.SetValue(ListViewItem.StyleProperty, this.ItemContainerStyle);

		}

		/// <summary>
		/// Gets or sets whether columns in a ColumnView can be reordered by a drag-and-drop operation.
		/// </summary>
		public Boolean AllowsColumnReorder
		{
			get
			{
				return (Boolean)this.GetValue(ColumnView.AllowsColumnReorderProperty);
			}
			set
			{
				this.SetValue(ColumnView.AllowsColumnReorderProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the style to apply to column headers.
		/// </summary>
		public Style ColumnHeaderContainerStyle
		{
			get
			{
				return this.GetValue(ColumnView.ColumnHeaderContainerStyleProperty) as Style;
			}
			set
			{
				this.SetValue(ColumnView.ColumnHeaderContainerStyleProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a ContextMenu for the ColumnView.
		/// </summary>
		public ContextMenu ColumnHeaderContextMenu
		{
			get
			{
				return this.GetValue(ColumnView.ColumnHeaderContextMenuProperty) as ContextMenu;
			}
			set
			{
				this.SetValue(ColumnView.ColumnHeaderContextMenuProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a composite string that specifies how to format the column headers of the ColumnView if they are displayed as strings.
		/// </summary>
		public String ColumnHeaderStringFormat
		{
			get
			{
				return this.GetValue(ColumnView.ColumnHeaderStringFormatProperty) as String;
			}
			set
			{
				this.SetValue(ColumnView.ColumnHeaderStringFormatProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a template to use to display the column headers.
		/// </summary>
		public DataTemplate ColumnHeaderTemplate
		{
			get
			{
				return this.GetValue(ColumnView.ColumnHeaderTemplateProperty) as DataTemplate;
			}
			set
			{
				this.SetValue(ColumnView.ColumnHeaderTemplateProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the selector object that provides logic for selecting a template to use for each column header.
		/// </summary>
		public DataTemplateSelector ColumnHeaderTemplateSelector
		{
			get
			{
				return this.GetValue(ColumnView.ColumnHeaderTemplateSelectorProperty) as DataTemplateSelector;
			}
			set
			{
				this.SetValue(ColumnView.ColumnHeaderTemplateSelectorProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the content of a tooltip that appears when the mouse pointer pauses over one of the column headers.
		/// </summary>
		public Object ColumnHeaderToolTip
		{
			get
			{
				return this.GetValue(ColumnView.ColumnHeaderToolTipProperty);
			}
			set
			{
				this.SetValue(ColumnView.ColumnHeaderToolTipProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the selector object that provides logic for selecting a template to use for each column header.
		/// </summary>
		public Nullable<HorizontalAlignment> ColumnHeaderHorizontalAlignment
		{
			get
			{
				return (Nullable<HorizontalAlignment>)this.GetValue(ColumnView.ColumnHeaderHorizontalAlignmentProperty);
			}
			set
			{
				this.SetValue(ColumnView.ColumnHeaderHorizontalAlignmentProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the selector object that provides logic for selecting a template to use for each column header.
		/// </summary>
		public VerticalAlignment ColumnHeaderVerticalAlignment
		{
			get
			{
				return (VerticalAlignment)this.GetValue(ColumnView.ColumnHeaderVerticalAlignmentProperty);
			}
			set
			{
				this.SetValue(ColumnView.ColumnHeaderVerticalAlignmentProperty, value);
			}
		}

		/// <summary>
		/// Gets the collection of ColumnViewColumn objects that is defined for this ColumnView.
		/// </summary>
		public ColumnViewColumnCollection Columns
		{
			get
			{
				return this.columns;
			}
		}

		/// <summary>
		/// Gets the reference for the default style for the ColumnView. (Overrides ViewBase.DefaultStyleKey.)
		/// </summary>
		protected override Object DefaultStyleKey
		{
			get
			{
				return new ComponentResourceKey(typeof(ColumnView), "ColumnViewStyle");
			}
		}

		/// <summary>
		/// Gets the reference to the default style for the container of the data items in the ColumnView. (Overrides ViewBase.ItemContainerDefaultStyleKey.)
		/// </summary>
		protected override Object ItemContainerDefaultStyleKey
		{
			get
			{
				return new ComponentResourceKey(typeof(ColumnView), "ColumnViewItemContainerStyle");
			}
		}

		/// <summary>
		/// Gets or sets the style for the ListViewItem container.
		/// </summary>
		public Style ItemContainerStyle
		{
			get
			{
				return this.GetValue(ColumnView.ItemContainerStyleProperty) as Style;
			}
			set
			{
				this.SetValue(ColumnView.ItemContainerStyleProperty, value);
			}
		}

	}

}