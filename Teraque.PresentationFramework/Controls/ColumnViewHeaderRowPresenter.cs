namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections.Generic;		
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media.Animation;

	/// <summary>
	/// Represents an Object that is used to define the layout of a row of column headers.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[StyleTypedProperty(Property = "ColumnHeaderContainerStyle", StyleTargetType = typeof(ColumnViewColumnHeader))]
	public class ColumnViewHeaderRowPresenter : ColumnViewRowPresenterBase
	{

		/// <summary>
		/// Column dragging states.
		/// </summary>
		enum DraggingState { None, PreDrag, Dragging };

		/// <summary>
		/// Identifies the AllowsColumnReorder dependency property.
		/// </summary>
		public static readonly DependencyProperty AllowsColumnReorderProperty = ColumnView.AllowsColumnReorderProperty.AddOwner(typeof(ColumnViewHeaderRowPresenter));

		/// <summary>
		/// Identifies the ColumnHeaderContainerStyle dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnHeaderContainerStyleProperty = ColumnView.ColumnHeaderContainerStyleProperty.AddOwner(
			typeof(ColumnViewHeaderRowPresenter),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(ColumnViewHeaderRowPresenter.OnPropertyChanged)));

		/// <summary>
		/// Identifies the ColumnHeaderContextMenu dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnHeaderContextMenuProperty = ColumnView.ColumnHeaderContextMenuProperty.AddOwner(
			typeof(ColumnViewHeaderRowPresenter),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(ColumnViewHeaderRowPresenter.OnPropertyChanged)));

		/// <summary>
		/// Identifies the ColumnHeaderHorizontalAlignment dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnHeaderHorizontalAlignmentProperty = ColumnView.ColumnHeaderHorizontalAlignmentProperty.AddOwner(
			typeof(ColumnViewHeaderRowPresenter),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(ColumnViewHeaderRowPresenter.OnPropertyChanged)));

		/// <summary>
		/// Identifies the ColumnHeaderStringFormat dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnHeaderStringFormatProperty = ColumnView.ColumnHeaderStringFormatProperty.AddOwner(
			typeof(ColumnViewHeaderRowPresenter),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(ColumnViewHeaderRowPresenter.OnPropertyChanged)));

		/// <summary>
		/// Identifies the ColumnHeaderTemplate dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnHeaderTemplateProperty = ColumnView.ColumnHeaderTemplateProperty.AddOwner(
			typeof(ColumnViewHeaderRowPresenter),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(ColumnViewHeaderRowPresenter.OnPropertyChanged)));

		/// <summary>
		/// Identifies the ColumnHeaderTemplateSelector dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnHeaderTemplateSelectorProperty = ColumnView.ColumnHeaderTemplateSelectorProperty.AddOwner(
			typeof(ColumnViewHeaderRowPresenter),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(ColumnViewHeaderRowPresenter.OnPropertyChanged)));

		/// <summary>
		/// Identifies the ColumnHeaderToolTip dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnHeaderToolTipProperty = ColumnView.ColumnHeaderToolTipProperty.AddOwner(
			typeof(ColumnViewHeaderRowPresenter),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(ColumnViewHeaderRowPresenter.OnPropertyChanged)));

		/// <summary>
		/// Identifies the ColumnHeaderVerticalAlignment dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnHeaderVerticalAlignmentProperty = ColumnView.ColumnHeaderVerticalAlignmentProperty.AddOwner(
			typeof(ColumnViewHeaderRowPresenter),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(ColumnViewHeaderRowPresenter.OnPropertyChanged)));

		/// <summary>
		/// The amount of time the animation will take to move a column heder.
		/// </summary>
		static Duration columnMovementDuration = new Duration(TimeSpan.FromMilliseconds(250));

		/// <summary>
		/// Maps the ColumnViewColumn property to the header's property.
		/// </summary>
		static Dictionary<DependencyProperty, DependencyProperty> columnPropertyTargetMap = new Dictionary<DependencyProperty, DependencyProperty>()
		{
			{ColumnViewColumn.HeaderContainerStyleProperty, FrameworkElement.StyleProperty},
			{ColumnViewColumn.HeaderContextMenuProperty, FrameworkElement.ContextMenuProperty},
			{ColumnViewColumn.HeaderHorizontalAlignmentProperty, HeaderedItemsControl.HorizontalContentAlignmentProperty},
			{ColumnViewColumn.HeaderStringFormatProperty, HeaderedItemsControl.HeaderStringFormatProperty},
			{ColumnViewColumn.HeaderTemplateProperty, HeaderedItemsControl.HeaderTemplateProperty},
			{ColumnViewColumn.HeaderTemplateSelectorProperty, HeaderedItemsControl.HeaderTemplateSelectorProperty},
			{ColumnViewColumn.HeaderToolTipProperty, FrameworkElement.ToolTipProperty},
			{ColumnViewColumn.HeaderVerticalAlignmentProperty, HeaderedItemsControl.VerticalContentAlignmentProperty}
		};

		/// <summary>
		/// Maps the ColumnViewHeaderRowPresenter property to the header's property.
		/// </summary>
		static Dictionary<DependencyProperty, DependencyProperty> columnViewTargetPropertyMap = new Dictionary<DependencyProperty, DependencyProperty>()
		{
			{ColumnViewHeaderRowPresenter.ColumnHeaderContainerStyleProperty, FrameworkElement.StyleProperty},
			{ColumnViewHeaderRowPresenter.ColumnHeaderContextMenuProperty, FrameworkElement.ContextMenuProperty},
			{ColumnViewHeaderRowPresenter.ColumnHeaderHorizontalAlignmentProperty, HeaderedItemsControl.HorizontalContentAlignmentProperty},
			{ColumnViewHeaderRowPresenter.ColumnHeaderStringFormatProperty, HeaderedItemsControl.HeaderStringFormatProperty},
			{ColumnViewHeaderRowPresenter.ColumnHeaderTemplateProperty, HeaderedItemsControl.HeaderTemplateProperty},
			{ColumnViewHeaderRowPresenter.ColumnHeaderTemplateSelectorProperty, HeaderedItemsControl.HeaderTemplateSelectorProperty},
			{ColumnViewHeaderRowPresenter.ColumnHeaderToolTipProperty, FrameworkElement.ToolTipProperty},
			{ColumnViewHeaderRowPresenter.ColumnHeaderVerticalAlignmentProperty, HeaderedItemsControl.VerticalContentAlignmentProperty}
		};

		/// <summary>
		/// The current position of the mouse during a column drag-and-drop operation.
		/// </summary>
		Point currentPosition;

		/// <summary>
		/// The index of the destination column during a column drag-and-drop operation.
		/// </summary>
		Int32 destinationColumnIndex;

		/// <summary>
		/// The column being dragged during a drag-and-drop opration.
		/// </summary>
		ColumnViewColumnHeader draggingColumnHeader;

		/// <summary>
		/// The state of dragging a column to a new destination.
		/// </summary>
		DraggingState draggingState = DraggingState.None;

		/// <summary>
		/// This is the primary source of scrolling instructions.
		/// </summary>
		ScrollViewer mainScrollViewer;

		/// <summary>
		/// This is how far the mouse must move before we'll start a drag-and-drop operation.
		/// </summary>
		const Double mouseMoveThreshold = 4.0;

		/// <summary>
		/// The original placement of the dragging column in the Z order.  The dragging column is dropped back to this possition after dragging.
		/// </summary>
		Int32 originalDraggingHeaderZIndex;

		/// <summary>
		/// This column fills the viewer up from the rightmost column to the edge of the viewer.
		/// </summary>
		ColumnViewColumnHeader paddingHeader = ColumnViewHeaderRowPresenter.CreatePaddingHeader();

		/// <summary>
		/// These properties when changed require the columns to be rearranged.
		/// </summary>
		static HashSet<DependencyProperty> rearrangingProperties = new HashSet<DependencyProperty>()
		{
			FrameworkElement.StyleProperty,
			HeaderedItemsControl.HeaderTemplateProperty,
			HeaderedItemsControl.HeaderTemplateSelectorProperty,
			HeaderedItemsControl.HeaderStringFormatProperty
		};

		/// <summary>
		/// This is the initial position, in the coordinates of the header control, of the mouse at the start of a drag-and-drop operations.
		/// </summary>
		Point relativeStartPosition;

		/// <summary>
		/// This is the column that is being dragged during a drag-and-drop operation.
		/// </summary>
		Int32 startColumnIndex;

		/// <summary>
		/// The starting position, in the coordinates of this control, for a drag-and-drop operation.
		/// </summary>
		Point startPosition;

		/// <summary>
		/// Maps the ColumnViewColumn property to the header's property.
		/// </summary>
		static Dictionary<DependencyProperty, DependencyProperty> targetColumnPropertyMap = new Dictionary<DependencyProperty, DependencyProperty>()
		{
			{FrameworkElement.ContextMenuProperty, ColumnViewColumn.HeaderContextMenuProperty},
			{FrameworkElement.StyleProperty, ColumnViewColumn.HeaderContainerStyleProperty},
			{FrameworkElement.ToolTipProperty, ColumnViewColumn.HeaderToolTipProperty},
			{HeaderedItemsControl.HeaderTemplateProperty, ColumnViewColumn.HeaderTemplateProperty},
			{HeaderedItemsControl.HeaderTemplateSelectorProperty, ColumnViewColumn.HeaderTemplateSelectorProperty},
			{HeaderedItemsControl.HeaderStringFormatProperty, ColumnViewColumn.HeaderStringFormatProperty},
			{HeaderedItemsControl.HorizontalContentAlignmentProperty, ColumnViewColumn.HeaderHorizontalAlignmentProperty},
			{HeaderedItemsControl.VerticalContentAlignmentProperty, ColumnViewColumn.HeaderVerticalAlignmentProperty}
		};

		/// <summary>
		/// Maps the ColumnViewHeaderRowPresenter property to the header's property.
		/// </summary>
		static Dictionary<DependencyProperty, DependencyProperty> targetColumnViewPropertyMap = new Dictionary<DependencyProperty, DependencyProperty>()
		{
			{FrameworkElement.ContextMenuProperty, ColumnViewHeaderRowPresenter.ColumnHeaderContextMenuProperty},
			{FrameworkElement.StyleProperty, ColumnViewHeaderRowPresenter.ColumnHeaderContainerStyleProperty},
			{FrameworkElement.ToolTipProperty, ColumnViewHeaderRowPresenter.ColumnHeaderToolTipProperty},
			{HeaderedItemsControl.HorizontalContentAlignmentProperty, ColumnViewHeaderRowPresenter.ColumnHeaderHorizontalAlignmentProperty},
			{HeaderedItemsControl.HeaderTemplateProperty, ColumnViewHeaderRowPresenter.ColumnHeaderTemplateProperty},
			{HeaderedItemsControl.HeaderTemplateSelectorProperty, ColumnViewHeaderRowPresenter.ColumnHeaderTemplateSelectorProperty},
			{HeaderedItemsControl.HeaderStringFormatProperty, ColumnViewHeaderRowPresenter.ColumnHeaderStringFormatProperty},
			{HeaderedItemsControl.VerticalContentAlignmentProperty, ColumnViewHeaderRowPresenter.ColumnHeaderVerticalAlignmentProperty}
		};

		/// <summary>
		/// Initialize the ColumnViewHeaderRowPresenter class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static ColumnViewHeaderRowPresenter()
		{

			// The canvas normally wants to center itself in the parent, this will force it to align on the left edge.
			FrameworkElement.HorizontalAlignmentProperty.OverrideMetadata(
				typeof(ColumnViewHeaderRowPresenter),
				new FrameworkPropertyMetadata(HorizontalAlignment.Left));

		}

		/// <summary>
		/// Initializes a new instance of the ColumnViewHeaderRowPresenter class.
		/// </summary>
		public ColumnViewHeaderRowPresenter()
		{

			// This child is always part of the control.  The padding header fills out the control from the last defined column to the edge of the control.
			this.Children.Add(this.paddingHeader);

		}

		/// <summary>
		/// Arrange the columns on the canvas.
		/// </summary>
		protected void ArrangeColumns()
		{

			// This keeps track of the total size needed to hold this row.  Note that if the viewport hasn't been measured yet then we'll assume 0.0.  This will
			// keep the width of the padding header at a reasonable value until the viewport has been established.  This is only really a problem with the
			// Visual Studio designer.
			Double viewportWidth =
				this.mainScrollViewer == null || this.mainScrollViewer.ViewportWidth == Double.PositiveInfinity ?
				0.0 :
				this.mainScrollViewer.ViewportWidth;

			// This keeps track of the space used by each of the grid headers as we figure out where they sit in the canvas.
			Size size = new Size();

			// The columns are arranged so that the left column overlaps the right column.  This is done so that the thumb from the left column sits on top of the
			// header to the right.  This makes it easy to grab on to the thumb and drag it to resize the column.
			Int32 zIndex = 0;

			// If arranging the columns changes the width of any header, then we'll need to invalidate the measurements and ask the headers to reposition 
			// themselves.
			Boolean isWidthChanged = false;

			// This will figure out the placement and width of each of the headers.
			foreach (ColumnViewColumnHeader columnViewColumnHeader in this.Children)
			{

				// The padding column fills in all the space from the rightmost column to the egde of the parent.  All the other columns have their sizes set by the
				// column.
				if (columnViewColumnHeader.Role == ColumnViewColumnHeaderRole.Padding)
				{

					// This will set the padding column to fill all available space from the rightmost column header to the edge of the page.  Note that the ZIndex 
					// is only set when we have columns.  This method is called when any of the properties that effect the layout have changed and it's possible to
					// set those properties before the column is set.
					Double paddingWidth = viewportWidth < (size.Width + this.Margin.Left) ? 0.0 : viewportWidth - (size.Width + this.Margin.Left);
					if (this.paddingHeader.Width != paddingWidth)
					{
						this.paddingHeader.Width = paddingWidth;
						isWidthChanged = true;
					}
					Canvas.SetLeft(this.paddingHeader, size.Width);
					Canvas.SetTop(this.paddingHeader, 0.0);
					if (this.Columns != null)
						Canvas.SetZIndex(this.paddingHeader, this.Columns.Count - zIndex++);

				}
				else
				{

					// This will figure out the width of the header and place it in the proper place, horizontally, vertically and Z order, in the canvas.  As 
					// noted above, each header is designed to sit above the column to the right in the Z order so that the thumb can overlap the neighbor to the
					// right and allow the user to easily drag the columns around.
					ColumnViewColumn columnViewColumn = columnViewColumnHeader.Column;
					if (columnViewColumnHeader.Width != columnViewColumn.Width)
					{
						columnViewColumnHeader.Width = columnViewColumn.Width;
						isWidthChanged = true;
					}
					Canvas.SetLeft(columnViewColumnHeader, size.Width);
					Canvas.SetTop(columnViewColumnHeader, 0.0);
					Canvas.SetZIndex(columnViewColumnHeader, this.Columns.Count - zIndex++);
					size.Width += columnViewColumn.Width;

				}

			}

			// If the width of any item has changed then we need to remeasure all the headers in this presenter.
			if (isWidthChanged)
				this.InvalidateMeasure();

		}

		/// <summary>
		/// Positions child elements and determines a size for a FrameworkElement derived class.
		/// </summary>
		/// <param name="arrangeSize">The final area within the parent that this element should use to arrange itself and its children.</param>
		/// <returns>The actual size used.</returns>
		protected override Size ArrangeOverride(Size arrangeSize)
		{

			// Position each of the child elements using the maximum size of the row for the height.  This allows the items to be vertically aligned inside the
			// space allotted for the column header.
			foreach (UIElement element in base.InternalChildren)
			{

				// If the attached property has been set for the left side of the control, then use it.  Otherwise look to see if there's an attached property for
				// the right side and use it.  Same handling of defaults goes for the top and bottom.
				Double left = Canvas.GetLeft(element);
				Double right = Canvas.GetRight(element);
				Double top = Canvas.GetTop(element);
				Double bottom = Canvas.GetBottom(element);
				Point point = new Point() {
					X = Double.IsNaN(left) ? Double.IsNaN(right) ? 0.0 : arrangeSize.Width - element.DesiredSize.Width - right : left,
					Y = Double.IsNaN(top) ? Double.IsNaN(bottom) ? 0.0 : arrangeSize.Height - element.DesiredSize.Height - bottom : top};

				// We'll place the control at the location specified by the attached properties.  The header will be sized based by the width measured (which is, 
				// in turn, derived from the column that defines this header) and the total size allocated for this header, which is derived from the height of the
				// largest header.  By giving the control more space than it asks for to arrange itself, we allow it to align itself vertically, much the same way a
				// resident of a grid can align itself in a row or column that is larger than the control.
				element.Arrange(new Rect(point, new Size(element.DesiredSize.Width, arrangeSize.Height)));

			}

			// There is no change to the size after arranging.
			return arrangeSize;

		}

		/// <summary>
		/// Build the visual element for the header and insert it into the list of visual children.
		/// </summary>
		/// <param name="columnViewColumn">The ColumnViewColumn to be created and inserted.</param>
		/// <returns>The visual element representing the logical ColumnViewColumn.</returns>
		ColumnViewColumnHeader BuildHeader(ColumnViewColumn columnViewColumn)
		{

			// If the header is a logical element, then create a wrapper for it.  If it is already a ColumnViewColumnHeader, then remove it from any visual parents
			// before trying to use it here.
			ColumnViewColumnHeader columnViewColumnHeader = columnViewColumn.Header as ColumnViewColumnHeader;
			if (columnViewColumnHeader == null)
				columnViewColumnHeader = this.CreateHeader(columnViewColumn);

			// This will initialize the new header.  Associate the column with the control that displays that column, install the content of the header and then
			// apply all the properties from the column or the view (there is a precendence to they way the properties are applied: column settings override
			// view settings).
			columnViewColumnHeader.SetValue(ColumnViewColumnHeader.columnPropertyKey, columnViewColumn);
			columnViewColumnHeader.Header = columnViewColumn.Header;
			columnViewColumnHeader.ItemsSource = columnViewColumn.Filters;
			foreach (DependencyProperty dependencyProperty in ColumnViewHeaderRowPresenter.columnPropertyTargetMap.Keys)
				this.UpdateHeaderProperty(columnViewColumnHeader, dependencyProperty);

			// A Weak Event Handler is required to keep us informed about changes to the column.  It is weak because the base class uses weak events to communicate 
			// with the rows as well as the header when changes are made to the view.  There is no measurable performance penalty for using a weak pattern here, so
			// that's how we'll keep abreast of the changes to the column.  The listner should be removed when the header is removed, but being that it's a weak
			// listener pattern, it really doesn't matter.
			PropertyChangedEventManager.AddListener(columnViewColumn, this, String.Empty);

			// This is the visual element to be used for the header.
			return columnViewColumnHeader;

		}

		/// <summary>
		/// Create the visual element for the header.
		/// </summary>
		/// <param name="columnViewColumn">The ColumnViewColumn to be created.</param>
		/// <returns>The visual element representing the logical ColumnViewColumn.</returns>
		protected virtual ColumnViewColumnHeader CreateHeader(ColumnViewColumn columnViewColumn)
		{

			// This is the default column that is created for this presenter.  Inheriting classes can override this for customer header.
			return new ColumnViewColumnHeader();

		}

		/// <summary>
		/// Creates a column that is used for padding.
		/// </summary>
		/// <returns>A padding column.</returns>
		static ColumnViewColumnHeader CreatePaddingHeader()
		{

			// This column fills out the space between the last column and the edge of the control.
			ColumnViewColumnHeader columnViewColumnHeader = new ColumnViewColumnHeader();
			columnViewColumnHeader.Role = ColumnViewColumnHeaderRole.Padding;
			return columnViewColumnHeader;

		}

		/// <summary>
		/// Find the index of the column that contains the given point.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns>The index of the column that conains the point or -1 if there are no columns.</returns>
		Int32 FindIndex(Point point)
		{

			// This will determine to which column a given point belongs by creating an area of infinite height bounded by the edges of the column.
			Rect rect = new Rect(0.0, Double.NegativeInfinity, 0.0, Double.PositiveInfinity);
			for (Int32 columnIndex = 0; columnIndex < this.Columns.Count; columnIndex++)
			{
				ColumnViewColumn columnViewColumn = this.Columns[columnIndex];
				rect.Width = columnViewColumn.Width;
				if (rect.Contains(point))
					return columnIndex;
				rect.X += rect.Width;
			}

			// If the point is not within the bounds of the columns, return the terminating columns.  Note that the padding column is not included in the explicit 
			// column collection so a point that is off the right edge of the header will return an index between the last explicit column and the padding column.
			return point.X < 0.0 ? 0 : this.Columns.Count;

		}

		/// <summary>
		/// Gets the visual header from the column definition.
		/// </summary>
		/// <param name="columnViewColumn">The ColumnViewColumn used to create the header.</param>
		/// <returns>The visual header associated with the ColumnViewColumn.</returns>
		protected ColumnViewColumnHeader GetHeaderFromColumn(ColumnViewColumn columnViewColumn)
		{

			// Scan all the children and return the element associated with the column.
			foreach (ColumnViewColumnHeader columnViewColumnHeader in this.Children)
				if (columnViewColumnHeader.Column == columnViewColumn)
					return columnViewColumnHeader;

			// We should never reach this point as we always generate a visible element from a column definition.
			throw new InvalidOperationException();

		}

		/// <summary>
		/// Measures the child elements of a Canvas in anticipation of arranging them during the ArrangeOverride pass.
		/// </summary>
		/// <param name="constraint">An upper limit Size that should not be exceeded.</param>
		/// <returns>A Size that represents the size that is required to arrange child content.</returns>
		protected override Size MeasureOverride(Size constraint)
		{

			// Allow the canvas to measure all the child elements in their current arrangement.
			base.MeasureOverride(constraint);

			// The horizontal size of this header is fixed based on the columns but the height is autosized.  That means that we're going to ask each header how big
			// it wants to be and set the height of the header row to be the maximum height of all the children.
			constraint = new Size();
			foreach (ColumnViewColumnHeader columnViewColumnHeader in this.Children)
				if (columnViewColumnHeader.Role != ColumnViewColumnHeaderRole.Padding)
				{
					constraint.Height = Math.Max(columnViewColumnHeader.DesiredSize.Height, constraint.Height);
					constraint.Width += columnViewColumnHeader.Column.Width;
				}

			// The padding header has no content so it can't resize itself properly.  This will force the padding header to use the maximum height of all its
			// siblings.
			this.paddingHeader.Height = constraint.Height;

			// This is the desired size of the canvas.
			return constraint;

		}

		/// <summary>
		/// Handles a change to the column collection associated with this presenter.
		/// </summary>
		/// <param name="notifyCollectionChangedEventArgs">The Object that originated the event.</param>
		/// <param name="notifyCollectionChangedEventArgs">Provides data for the CollectionChanged event.</param>
		protected override void OnColumnCollectionChanged(Object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{

			Int32 columnIndex;

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (notifyCollectionChangedEventArgs == null)
				throw new ArgumentNullException("notifyCollectionChangedEventArgs");

			// This will reconcile the changes in the master column list with the visual elements used to represent the columns in this presenter.
			switch (notifyCollectionChangedEventArgs.Action)
			{

			case NotifyCollectionChangedAction.Add:

				// A starting index of '-1' indicates that we're adding, not inserting.  Otherwise, we insert the new elements at the index given to us by the event
				// arguments.  Note that the padding column is always going to be the first element and, therefore, at the bottom of the Z order.  This is done so that
				// the thumb control used for resizing can overlap the control to the right.  In order to overlap the control to the right, the control to the left
				// must sit on top of the control to the right in the Z order, so the rightmost column would need to be at the bottom of the pile.
				columnIndex = notifyCollectionChangedEventArgs.NewStartingIndex == -1 ? 0 : notifyCollectionChangedEventArgs.NewStartingIndex;
				foreach (ColumnViewColumn columnViewColumn in notifyCollectionChangedEventArgs.NewItems)
					if (columnViewColumn.IsVisible)
						this.Children.Insert(columnIndex++, this.BuildHeader(columnViewColumn));
				break;

			case NotifyCollectionChangedAction.Move:

				// Columns that appear after the visible count in the collection are hidden items.  They exist only as a pool of potential columns.  The items below
				// the 'VisibleCount' are actually visible in this view and appear in the same order in which they appear in this control.  As items are moved in
				// and out of the visible range, we will create or destroy the visual elements used to show those columns in the viewer.
				Int32 oldIndex = notifyCollectionChangedEventArgs.OldStartingIndex;
				Int32 newIndex = notifyCollectionChangedEventArgs.NewStartingIndex;
				ColumnViewColumn movedColumn = this.Columns[newIndex];
				if (this.Children.Count - 1 == this.Columns.VisibleCount)
				{

					// At this point, the collection hasn't changed.  We are just moving columns around.
					ColumnViewColumnHeader columnViewColumnHeader = this.Children[oldIndex] as ColumnViewColumnHeader;
					this.Children.RemoveAt(notifyCollectionChangedEventArgs.OldStartingIndex);
					this.Children.Insert(notifyCollectionChangedEventArgs.NewStartingIndex, columnViewColumnHeader);
					this.InvalidateArrange();

				}
				else
				{

					// This will hide the column by destroying the visual elements used to display the column.
					if (oldIndex < this.Children.Count - 1)
					{
						this.RemoveHeader(movedColumn);
						this.InvalidateMeasure();
					}

					// This will create a visual element to display the now visible column.
					if (movedColumn.IsVisible)
					{
						this.Children.Insert(newIndex, this.BuildHeader(movedColumn));
						this.InvalidateMeasure();
					}

				}

				break;

			case NotifyCollectionChangedAction.Remove:

				// This will remove the elements at the given index.
				columnIndex = notifyCollectionChangedEventArgs.OldStartingIndex;
				foreach (ColumnViewColumn columnViewColumn in notifyCollectionChangedEventArgs.OldItems)
					this.RemoveHeader(columnViewColumn);
				break;

			case NotifyCollectionChangedAction.Reset:

				// Remove any column header except for the predefined ones.
				columnIndex = 0;
				while (this.Children.Count != 1)
					this.RemoveHeader(0);
				break;

			}

			// After any change is made to the collection we need to re-arrange the children in on the canvas.
			this.ArrangeColumns();

		}

		/// <summary>
		/// Handles a change to a property of the column when those changes originate from the ColumnViewColumn.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="propertyChangedEventArgs">The event data.</param>
		protected override void OnColumnPropertyChanged(Object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (propertyChangedEventArgs == null)
				throw new ArgumentNullException("propertyChangedEventArgs");

			// Extract the column that created this event from the arguments.
			ColumnViewColumn columnViewColumn = sender as ColumnViewColumn;

			// This is the visual element used to display the column that was changed.
			ColumnViewColumnHeader columnViewColumnHeader = this.GetHeaderFromColumn(columnViewColumn);

			// This will reconcile the properties of the header with the properties of the ColumnViewColumn.
			switch (propertyChangedEventArgs.PropertyName)
			{

			case "Header":

				// Set the content.
				columnViewColumnHeader.Header = columnViewColumnHeader.Column.Header;
				this.ArrangeColumns();
				break;

			case "HeaderContainerStyle":

				// HeaderContainerStyle
				this.UpdateHeaderProperty(columnViewColumnHeader, ColumnViewColumn.HeaderContainerStyleProperty);
				this.ArrangeColumns();
				break;

			case "HeaderContextMenu":

				// HeaderContextMenu
				this.UpdateHeaderProperty(columnViewColumnHeader, ColumnViewColumn.HeaderContextMenuProperty);
				break;

			case "HeaderHorizontalAlignment":

				// HeaderHorizontalAlignment
				this.UpdateHeaderProperty(columnViewColumnHeader, ColumnViewColumn.HeaderHorizontalAlignmentProperty);
				break;

			case "HeaderStringFormat":

				// HeaderStringFormat
				this.UpdateHeaderProperty(columnViewColumnHeader, ColumnViewColumn.HeaderStringFormatProperty);
				this.ArrangeColumns();
				break;

			case "HeaderTemplate":

				// HeaderTemplate
				this.UpdateHeaderProperty(columnViewColumnHeader, ColumnViewColumn.HeaderTemplateProperty);
				this.ArrangeColumns();
				break;

			case "HeaderTemplateSelector":

				// HeaderTemplateSelector
				this.UpdateHeaderProperty(columnViewColumnHeader, ColumnViewColumn.HeaderTemplateSelectorProperty);
				this.ArrangeColumns();
				break;

			case "HeaderToolTip":

				// HeaderToolTop
				this.UpdateHeaderProperty(columnViewColumnHeader, ColumnViewColumn.HeaderToolTipProperty);
				break;

			case "HeaderVerticalAlignment":

				// HeaderVerticalAlignment
				this.UpdateHeaderProperty(columnViewColumnHeader, ColumnViewColumn.HeaderVerticalAlignmentProperty);
				break;

			case "SortDirection":

				// Update the sort direction on the header control to agree with the ColumnViewColumn.
				columnViewColumnHeader.UpdateSortDirection();
				break;

			case "Width":
			case "MinWidth":

				// This will handle a change to the dimensions of the column.  Note that we have to invalidate the measurement of the control so that we'll force
				// the header to lay itself out again with the new width.  The ArrangeColumns will not do that automatically.
				this.ArrangeColumns();
				break;

			}

		}

		/// <summary>
		/// Occurs when changes are detected to the scroll position, extent, or viewport size.
		/// </summary>
		/// <param name="sender">The obje</param>
		/// <param name="scrollChangedEventArgs"></param>
		void OnMasterScrollChanged(Object sender, ScrollChangedEventArgs scrollChangedEventArgs)
		{

			// This header presenter is ususally shrowded in a ScrollViewer to handle the scrolling.  When the master scroll viewer (which provides the 
			// instructions for scrolling, as opposed to the parent ScrollViewer which is simply a panel that moves behind the master viewport) tells us to scroll,
			// we'll change the offset of the parent viewer (if it exists).
			ScrollViewer scrollViewer = this.Parent as ScrollViewer;
			if (scrollViewer != null && this.mainScrollViewer == scrollChangedEventArgs.OriginalSource)
				scrollViewer.ScrollToHorizontalOffset(scrollChangedEventArgs.HorizontalOffset);

			// If the dimensions of the viewport have changed, then the padding column needs to be adjusted as it sits in the virtual space between the rightmost
			// column and the edge of the viewport.
			if (scrollChangedEventArgs.ViewportWidthChange != 0.0)
				this.ArrangeColumns();

		}

		/// <summary>
		/// Invoked when an unhandled MouseLeftButtonDown routed event is raised on this element.
		/// </summary>
		/// <param name="e">The MouseButtonEventArgs that contains the event data. The event data reports that the left mouse button was pressed.</param>
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// If the header allows reordering then we'll set the variables that keep track of where we started this drag-and-drop operation and who's being
			// dragged.  Note that we don't really begin dragging until the mouse has moved a small amount with the button still down.  This is to keep
			// false starts -- like a spurrious mouse click -- from reordering the columns.
			if (this.AllowsColumnReorder)
			{
				this.draggingState = DraggingState.PreDrag;
				this.startPosition = e.GetPosition(this);
				this.relativeStartPosition = e.GetPosition(e.Source as IInputElement);
			}

			// We handled the button.
			e.Handled = true;

		}

		/// <summary>
		/// Invoked when an unhandled MouseLeftButtonUp routed event reaches an element in its route that is derived from this class.
		/// </summary>
		/// <param name="e">The MouseButtonEventArgs that contains the event data. The event data reports that the left mouse button was released.</param>
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			//  If we just started dragging but haven't passed the mouse movement threshold, then stop.  If we've been dragging a header to a new location then 
			//  complete the drag-and-drop operation.
			switch (draggingState)
			{

			case DraggingState.PreDrag:

				// If we just started dragging but never moved the mouse enough, then we write the gesture off as spurious.
				this.draggingState = DraggingState.None;
				break;

			case DraggingState.Dragging:

				// This will reset the housekeeping variables that track the drag operation.  Note that dereferencing the animation used to move the cells around
				// will allow the animations to be garbage collected.
				Canvas.SetZIndex(this.draggingColumnHeader, this.originalDraggingHeaderZIndex);
				this.draggingColumnHeader.Role = ColumnViewColumnHeaderRole.Normal;
				this.draggingState = DraggingState.None;
				foreach (ColumnViewColumnHeader columnViewColumnHeader in this.Children)
					if (columnViewColumnHeader.Role != ColumnViewColumnHeaderRole.Padding)
						columnViewColumnHeader.BeginAnimation(Canvas.LeftProperty, null);

				// This does the actual work of moving the column.
				Int32 newIndex = this.startColumnIndex >= this.destinationColumnIndex ? this.destinationColumnIndex : this.destinationColumnIndex - 1;
				this.Columns.Move(this.startColumnIndex, newIndex);
				this.ArrangeColumns();

				break;

			}

			// We handled the button.
			e.Handled = true;

		}

		/// <summary>
		/// Invoked when an unhandled Mouse.MouseMove attached event reaches an element in its route that is derived from this class.
		/// </summary>
		/// <param name="e">The MouseEventArgs that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// When the mouse button is pressed we'll look to see if a drag-and-drop operation is underway.
			if (e.LeftButton == MouseButtonState.Pressed)
			{

				// This is the current position of the mouse in the presenter control's coorindate system.
				this.currentPosition = e.GetPosition(this);

				// There are two states that are of interest when the mouse is moving: the state where we wait for a certain distance to be dragged before starting 
				// a drag-and-drop operation, and the actual dragging of a column.
				switch (this.draggingState)
				{

				case DraggingState.PreDrag:

					// This will wait until the user has held the mouse down and moved the cursor a certian distance.  It is designed to prevent spurious mouse 
					// clicks from moving columns around.  Once the threshold has been achieved, we can start the drag-and-drop operation.  Note that the selected  
					// column is moved to the bottom of the Z Index so it will appear to slide under all the other columns.  This is a very cool effect when the 
					// normal columns have a transparent background.  Of course, the draggin column will be completely obscured if you change the default header 
					// template to have an opaque background.
					if (Math.Abs(this.currentPosition.X - this.startPosition.X) > ColumnViewHeaderRowPresenter.mouseMoveThreshold)
					{
						this.draggingColumnHeader = e.Source as ColumnViewColumnHeader;
						this.draggingColumnHeader.Role = ColumnViewColumnHeaderRole.Dragging;
						this.destinationColumnIndex = this.startColumnIndex = this.FindIndex(this.startPosition);
						this.originalDraggingHeaderZIndex = Canvas.GetZIndex(this.draggingColumnHeader);
						Canvas.SetZIndex(this.draggingColumnHeader, Int32.MinValue);
						this.draggingState = DraggingState.Dragging;
					}

					break;

				case DraggingState.Dragging:

					// This is the index of the column that is the current destination for the drag-and-drop operation.
					Int32 currentColumnIndex = this.FindIndex(this.currentPosition);

					// When the proposed destination column has changed (and is not the original index of the column being dragged) then animate the cells to
					// re-order themselves in order to make a room in the header for the dragged column.
					if (currentColumnIndex != this.destinationColumnIndex && currentColumnIndex != this.startColumnIndex)
					{

						// This will open up a gap in the heading where the dragged column can be dropped.  The 'destination' variable is used to set the
						// destination for the animation.  It is designed to open up a gap in the header where the dragged column can be dropped.
						Double destination = 0.0;

						// This will cycle through all the children re-arranging them to produce a gap.  Note that in this form the loop will also examine the
						// padding header because it's index can also be the destination for an operation.
						for (Int32 columnIndex = 0; columnIndex < this.Columns.VisibleCount; columnIndex++)
						{

							// This column will be animated to move into a new location if it's not already at the proper location.
							ColumnViewColumnHeader columnViewColumnHeader = this.Children[columnIndex] as ColumnViewColumnHeader;

							// The column being dragged occupies it's own space so it doesn't need to counted here.
							if (columnIndex == this.startColumnIndex)
								continue;

							// This will produce a gap at the target location big enough to hold the dragged column.
							if (columnIndex == currentColumnIndex)
								destination += this.draggingColumnHeader.Column.Width;

							// If the current location of the column doesn't agree with the location of the new configuration, then animated it to find its now
							// place.
							if (Canvas.GetLeft(columnViewColumnHeader) != destination)
							{
								DoubleAnimation doubleAnimation = new DoubleAnimation();
								doubleAnimation.From = Canvas.GetLeft(columnViewColumnHeader);
								doubleAnimation.To = destination;
								doubleAnimation.Duration = ColumnViewHeaderRowPresenter.columnMovementDuration;
								columnViewColumnHeader.BeginAnimation(Canvas.LeftProperty, doubleAnimation);
							}

							// The destination moves along with each column except for the places where the gap was opened or the dragged column was ignored.  Thus,
							// it becomes the new value for the left side of the next column in the header.
							destination += columnViewColumnHeader.Column.Width;

						}

						// At this point the destination column is recorded.  When the mouse button is lifed, we'll look at this value to see what position has been
						// selected by the user's mouse.
						this.destinationColumnIndex = currentColumnIndex;

					}

					// This will calculate the left edge of the dragging header based on the starting position of the cursor with respect to the left edge of the
					// header being dragged (this.relativeStartPosition.X) and the current location of the cursor with respect to the left edge of the header bar.
					Canvas.SetLeft(this.draggingColumnHeader, this.currentPosition.X - this.relativeStartPosition.X);

					break;

				}

			}

			// We handled the button.
			e.Handled = true;

		}

		/// <summary>
		/// Handles a change to the dependency properties.
		/// </summary>
		/// <param name="dependencyObject">The Object that originated the event.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event arguments.</param>
		static void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the target Object from the event arguments.
			ColumnViewHeaderRowPresenter columnViewHeaderRowPresenter = dependencyObject as ColumnViewHeaderRowPresenter;

			// The properties of the presenter aren't applicable to the actual control where the heading is displayed.  In order to map the changes, we need to find
			// out what framework property is being changed.  In order to enforce the precendence of column properties before presenter properties, we'll have to get
			// the equivalent ColumnViewColumn property also.
			DependencyProperty targetProperty = ColumnViewHeaderRowPresenter.columnViewTargetPropertyMap[dependencyPropertyChangedEventArgs.Property];
			DependencyProperty columnProperty = ColumnViewHeaderRowPresenter.targetColumnPropertyMap[targetProperty];

			// Property changes made to the presenter are reflected in all the children (as opposed to changes to the column property which effect only the column
			// modified).  The proprties have a precedence where column properties override the presenter properties.
			for (Int32 childIndex = 0; childIndex < columnViewHeaderRowPresenter.Children.Count; childIndex++)
			{

				// This is the child who's property will be reconciled to the presenter's property.
				ColumnViewColumnHeader columnViewColumnHeader = columnViewHeaderRowPresenter.Children[childIndex] as ColumnViewColumnHeader;

				// This will update the actual visual element with the property change.  We examine the precendence of the properties as we select a new value for
				// the visual element's property.  Column properties take precendence over presenter properties.
				Object value = columnViewHeaderRowPresenter.GetValue(dependencyPropertyChangedEventArgs.Property);
				if (columnViewColumnHeader.Role != ColumnViewColumnHeaderRole.Padding)
					value = value == null ? columnViewColumnHeader.Column.GetValue(columnProperty) : value;

				// Set or clear the final target property.
				if (value == null)
					columnViewColumnHeader.ClearValue(targetProperty);
				else
					columnViewColumnHeader.SetValue(targetProperty, value);

				// There is a bug in the WCF content controls where the content will not pick up on the new StringFormat if the content is set before the string 
				// format.  This will re-initialize the content control after the string format has been changed in order force the new format to be recognized by 
				// the content.
				if (targetProperty == HeaderedItemsControl.HeaderStringFormatProperty)
				{
					Object header = columnViewColumnHeader.Header;
					columnViewColumnHeader.Header = null;
					columnViewColumnHeader.Header = header;
				}

			}

			// If the property modified can change the appearance of the header then we will rearrange the columns.
			if (ColumnViewHeaderRowPresenter.rearrangingProperties.Contains(targetProperty))
				columnViewHeaderRowPresenter.ArrangeColumns();

		}

		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code or internal processes call ApplyTemplate.
		/// </summary>
		protected override void OnVisualParentChanged(DependencyObject oldParent)
		{

			// This will hook us into the scroll viewer so we can keep track of changes to the viewport.
			this.mainScrollViewer = this.TemplatedParent as ScrollViewer;
			if (this.mainScrollViewer != null)
				this.mainScrollViewer.ScrollChanged += this.OnMasterScrollChanged;

			// Allow the base class to handle the reset of parent change event.
			base.OnVisualParentChanged(oldParent);

		}

		/// <summary>
		/// Remove the header.
		/// </summary>
		/// <param name="columnIndex">The index of the column header to be removed from the presenter.</param>
		void RemoveHeader(Int32 columnIndex)
		{

			// It is good housecleaning to remove the weak listner, but not essential.  The column can still be garbage collected.
			ColumnViewColumnHeader columnViewColumnHeader = this.Children[columnIndex] as ColumnViewColumnHeader;
			PropertyChangedEventManager.RemoveListener(columnViewColumnHeader.Column, this, String.Empty);

			// Remove the visual header from the collection and unhook it from the keyboard events.
			this.Children.RemoveAt(columnIndex);

		}

		/// <summary>
		/// Remove the header.
		/// </summary>
		/// <param name="columnIndex">The index of the column header to be removed from the presenter.</param>
		void RemoveHeader(ColumnViewColumn columnViewColumn)
		{

			// It is good housecleaning to remove the weak listner, but not essential.  The column can still be garbage collected.
			ColumnViewColumnHeader columnViewColumnHeader = this.GetHeaderFromColumn(columnViewColumn);
			PropertyChangedEventManager.RemoveListener(columnViewColumnHeader.Column, this, String.Empty);

			// Remove the visual header from the collection and unhook it from the keyboard events.
			columnViewColumnHeader.Header = null;
			this.Children.Remove(columnViewColumnHeader);

		}

		/// <summary>
		/// Updates the header property when a column property has changed.
		/// </summary>
		/// <param name="columnViewColumnHeader">The column header that has changed.</param>
		/// <param name="columnProperty">The property on the ColumnViewColumn that has changed.</param>
		void UpdateHeaderProperty(ColumnViewColumnHeader columnViewColumnHeader, DependencyProperty columnProperty)
		{

			// The properties of the column aren't applicable to the actual control where the heading is displayed.  In order to map the changes, we need to find
			// out what framework property is being changed.  In order to enforce the precendence of column properties before presenter properties, we'll have to get
			// the equivalent ColumnView property also.
			DependencyProperty targetProperty = ColumnViewHeaderRowPresenter.columnPropertyTargetMap[columnProperty];
			DependencyProperty columnViewProperty = ColumnViewHeaderRowPresenter.targetColumnViewPropertyMap[targetProperty];

			// This will evaluate the new value for the property based on an ordering where the ColumnViewColumn properties take precendence over the ColumnView
			// properties.
			ColumnViewColumn columnViewColumn = columnViewColumnHeader.Column;
			Object value = columnViewColumn.GetValue(columnProperty);
			value = value == null ? this.GetValue(columnViewProperty) : value;
			if (value == null)
				columnViewColumnHeader.ClearValue(targetProperty);
			else
				columnViewColumnHeader.SetValue(targetProperty, value);

			// There is a bug in the WCF content controls where the content will not pick up on the new StringFormat if the content is set before the string 
			// format.  This will re-initialize the content control after the string format has been changed in order force the new format to be recognized by the 
			// content.
			if (targetProperty == HeaderedItemsControl.HeaderStringFormatProperty)
			{
				Object header = columnViewColumnHeader.Header;
				columnViewColumnHeader.Header = null;
				columnViewColumnHeader.Header = header;
			}

		}

		/// <summary>
		/// Gets or sets a value that indicates whether columns can change positions.
		/// </summary>
		public Boolean AllowsColumnReorder
		{
			get
			{
				return (Boolean)this.GetValue(AllowsColumnReorderProperty);
			}
			set
			{
				this.SetValue(AllowsColumnReorderProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the Style to use for the column headers.
		/// </summary>
		public Style ColumnHeaderContainerStyle
		{
			get
			{
				return (Style)this.GetValue(ColumnHeaderContainerStyleProperty);
			}
			set
			{
				this.SetValue(ColumnHeaderContainerStyleProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a ContextMenu for the column headers.
		/// </summary>
		public ContextMenu ColumnHeaderContextMenu
		{
			get
			{
				return (ContextMenu)this.GetValue(ColumnHeaderContextMenuProperty);
			}
			set
			{
				this.SetValue(ColumnHeaderContextMenuProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a HorizontalAlignment for the column headers.
		/// </summary>
		public HorizontalAlignment ColumnHeaderHorizontalAlignment
		{
			get
			{
				return (HorizontalAlignment)this.GetValue(ColumnHeaderHorizontalAlignmentProperty);
			}
			set
			{
				this.SetValue(ColumnHeaderHorizontalAlignmentProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a composite string that specifies how to format the column headers if they are displayed as strings.
		/// </summary>
		public String ColumnHeaderStringFormat
		{
			get
			{
				return (String)this.GetValue(ColumnHeaderStringFormatProperty);
			}
			set
			{
				this.SetValue(ColumnHeaderStringFormatProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the template to use to display the column headers.
		/// </summary>
		public DataTemplate ColumnHeaderTemplate
		{
			get
			{
				return (DataTemplate)this.GetValue(ColumnHeaderTemplateProperty);
			}
			set
			{
				this.SetValue(ColumnHeaderTemplateProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a DataTemplateSelector that provides logic that selects the data template to use to display a column header.
		/// </summary>
		public DataTemplateSelector ColumnHeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)this.GetValue(ColumnHeaderTemplateSelectorProperty);
			}
			set
			{
				this.SetValue(ColumnHeaderTemplateSelectorProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the content for a tooltip for the column header row.
		/// </summary>
		public Object ColumnHeaderToolTip
		{
			get
			{
				return this.GetValue(ColumnHeaderToolTipProperty);
			}
			set
			{
				this.SetValue(ColumnHeaderToolTipProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a VerticalAlignment for the column headers.
		/// </summary>
		public VerticalAlignment ColumnHeaderVerticalAlignment
		{
			get
			{
				return (VerticalAlignment)this.GetValue(ColumnHeaderVerticalAlignmentProperty);
			}
			set
			{
				this.SetValue(ColumnHeaderVerticalAlignmentProperty, value);
			}
		}

	}

}