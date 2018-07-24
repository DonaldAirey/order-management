namespace Teraque.Windows.Controls
{

	using System;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Controls.Primitives;
	using System.Windows.Input;
	using System.Windows.Media;
	using Teraque.Windows.Input;

	/// <summary>
	/// Represents a column header for a ColumnViewColumn.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[TemplatePart(Name = ColumnViewColumnHeader.contentElementPartName, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = ColumnViewColumnHeader.filterButtonPartName, Type = typeof(Button))]
	[TemplatePart(Name = ColumnViewColumnHeader.reszieGripperPartName, Type = typeof(Thumb))]
	public class ColumnViewColumnHeader : HeaderedItemsControl
	{

		/// <summary>
		/// Identifies the Click routed event.
		/// </summary>
		public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
			"Click",
			RoutingStrategy.Bubble,
			typeof(RoutedEventHandler),
			typeof(ColumnViewColumnHeader));

		/// <summary>
		/// Identifies the ColumnProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnProperty;

		/// <summary>
		/// Identifies the columnProperty dependency property key.
		/// </summary>
		internal static readonly DependencyPropertyKey columnPropertyKey = DependencyProperty.RegisterReadOnly(
			"Column",
			typeof(ColumnViewColumn),
			typeof(ColumnViewColumnHeader),
			new FrameworkPropertyMetadata(ColumnViewColumnHeader.OnColumnPropertyChanged));

		/// <summary>
		/// The element where the content of the header appears.  This element is clipped when the filter button is visible.
		/// </summary>
		FrameworkElement contentElement;

		/// <summary>
		/// The name of the part where the content is displayed.  Used for clipping when the filter button is visible.
		/// </summary>
		const String contentElementPartName = "PART_Content";

		/// <summary>
		/// This is the element used to open up the filter from the header.
		/// </summary>
		Button filterButton;

		/// <summary>
		/// Name of the open filter part of the header.
		/// </summary>
		const String filterButtonPartName = "PART_FilterButton";

		/// <summary>
		/// Identifies the HasFiltersProperty dependency property key.
		/// </summary>
		public static readonly DependencyProperty HasFiltersProperty;

		/// <summary>
		/// Identifies the HasFilters dependency property.
		/// </summary>
		static readonly DependencyPropertyKey hasFiltersPropertyKey = DependencyProperty.RegisterReadOnly(
			"HasFilters",
			typeof(Boolean),
			typeof(ColumnViewColumnHeader),
			new FrameworkPropertyMetadata(false));

		/// <summary>
		/// Identifies the IsAscendingProperty dependency property key.
		/// </summary>
		public static readonly DependencyProperty IsAscendingProperty;

		/// <summary>
		/// Identifies the IsAscending dependency property.
		/// </summary>
		static readonly DependencyPropertyKey isAscendingPropertyKey = DependencyProperty.RegisterReadOnly(
			"IsAscending",
			typeof(Boolean),
			typeof(ColumnViewColumnHeader),
			new FrameworkPropertyMetadata(false));

		/// <summary>
		/// Identifies the IsDescendingProperty dependency property key.
		/// </summary>
		public static readonly DependencyProperty IsDescendingProperty;

		/// <summary>
		/// Identifies the IsDescending dependency property.
		/// </summary>
		static readonly DependencyPropertyKey isDescendingPropertyKey = DependencyProperty.RegisterReadOnly(
			"IsDescending",
			typeof(Boolean),
			typeof(ColumnViewColumnHeader),
			new FrameworkPropertyMetadata(false));

		/// <summary>
		/// Identifies the IsFilterOpen dependency property.
		/// </summary>
		public static readonly DependencyProperty IsFilterOpenProperty = DependencyProperty.Register(
			"IsFilterOpen",
			typeof(Boolean),
			typeof(ColumnViewColumnHeader));

		/// <summary>
		/// Identifies the IsPressed dependency property.
		/// </summary>
		public static readonly DependencyProperty IsPressedProperty;

		/// <summary>
		/// Identifies the IsPressed dependency property key.
		/// </summary>
		internal static readonly DependencyPropertyKey isPressedPropertyKey = DependencyProperty.RegisterReadOnly(
			"IsPressed",
			typeof(Boolean),
			typeof(ColumnViewColumnHeader),
			new FrameworkPropertyMetadata(false));

		/// <summary>
		/// Identifies the IsSelected dependency property.
		/// </summary>
		public static readonly DependencyProperty IsSelectedProperty;

		/// <summary>
		/// Identifies the IsSelected dependency property key.
		/// </summary>
		internal static readonly DependencyPropertyKey isSelectedPropertyKey = DependencyProperty.RegisterReadOnly(
			"IsSelected",
			typeof(Boolean),
			typeof(ColumnViewColumnHeader),
			new FrameworkPropertyMetadata(false, new PropertyChangedCallback(ColumnViewColumnHeader.OnIsSelectedPropertyChanged)));

		/// <summary>
		/// The resize gripper.
		/// </summary>
		Thumb resizeGripper;

		/// <summary>
		/// Name of the gripper part.
		/// </summary>
		const String reszieGripperPartName = "PART_HeaderGripper";

		/// <summary>
		/// Identifies the RoleProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty RoleProperty;

		/// <summary>
		/// Identifies the RoleProperty dependency property key.
		/// </summary>
		static readonly DependencyPropertyKey rolePropertyKey = DependencyProperty.RegisterReadOnly(
			"Role",
			typeof(ColumnViewColumnHeaderRole),
			typeof(ColumnViewColumnHeader),
			new FrameworkPropertyMetadata(ColumnViewColumnHeaderRole.Normal));

		/// <summary>
		/// The cursor used to resize columns.
		/// </summary>
		static Cursor verticalSplitCursor = new Cursor(
			Application.GetResourceStream(new Uri("/Teraque.PresentationFramework;component/Resources/VerticalSplit.cur", UriKind.Relative)).Stream);

		/// <summary>
		/// Initialize the GidViewColumnHeader class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static ColumnViewColumnHeader()
		{

			// This allows the class to have its own style in the theme.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
				typeof(ColumnViewColumnHeader),
				new FrameworkPropertyMetadata(new ComponentResourceKey(typeof(ColumnViewColumnHeader), "ColumnViewColumnHeaderStyle")));

			// This element is not focuasble and does not support keystrokes.
			UIElement.FocusableProperty.OverrideMetadata(typeof(ColumnViewColumnHeader), new FrameworkPropertyMetadata(false));

			// These properties must be set after the corresonding property key is defined.  This is done here because setting these when the field is declared can 
			// lead to bugs if the fields should be reorderer.
			ColumnViewColumnHeader.ColumnProperty = ColumnViewColumnHeader.columnPropertyKey.DependencyProperty;
			ColumnViewColumnHeader.HasFiltersProperty = ColumnViewColumnHeader.hasFiltersPropertyKey.DependencyProperty;
			ColumnViewColumnHeader.IsAscendingProperty = ColumnViewColumnHeader.isAscendingPropertyKey.DependencyProperty;
			ColumnViewColumnHeader.IsDescendingProperty = ColumnViewColumnHeader.isDescendingPropertyKey.DependencyProperty;
			ColumnViewColumnHeader.IsPressedProperty = ColumnViewColumnHeader.isPressedPropertyKey.DependencyProperty;
			ColumnViewColumnHeader.IsSelectedProperty = ColumnViewColumnHeader.isSelectedPropertyKey.DependencyProperty;
			ColumnViewColumnHeader.RoleProperty = ColumnViewColumnHeader.rolePropertyKey.DependencyProperty;

		}

		/// <summary>
		/// This will clip the content of the header when the filter button is visible.
		/// </summary>
		void ClipContent()
		{

			// If we have both a content part and filter button part, then we can clip the content so that it won't bleed into the filter button when the filter
			// button is visible.  Note the addition of the padding which gives a nice separation between the header and the filter button, even when the header is
			// right justified.
			if (this.contentElement != null && this.filterButton != null)
			{
				RectangleGeometry rectangleGeometry = this.contentElement.Clip as RectangleGeometry;
				Double width = this.filterButton.Visibility == Visibility.Visible ?
					Math.Max(0.0, this.contentElement.ActualWidth - this.filterButton.ActualWidth - this.Padding.Right) :
					this.contentElement.ActualWidth;
				rectangleGeometry.Rect = new Rect(new Size(width, this.contentElement.ActualHeight));
			}
		}

		/// <summary>
		/// Is invoked whenever application code or internal processes call ApplyTemplate.
		/// </summary>
		public override void OnApplyTemplate()
		{

			// Allow the base class to initialize any template objects.
			base.OnApplyTemplate();

			// This will attache the gripper to events that allow the column header to be resized and will give it a cursor that lets the user know when resizing
			// will work.
			this.resizeGripper = this.GetTemplateChild(ColumnViewColumnHeader.reszieGripperPartName) as Thumb;
			if (this.resizeGripper != null)
			{
				this.resizeGripper.DragDelta += new DragDeltaEventHandler(this.OnColumnHeaderResize);
				this.resizeGripper.MouseDown += new MouseButtonEventHandler(this.OnGripperClicked);
				this.resizeGripper.MouseUp += new MouseButtonEventHandler(this.OnGripperClicked);
				this.resizeGripper.MouseDoubleClick += new MouseButtonEventHandler(this.OnGripperDoubleClicked);
				this.resizeGripper.Cursor = ColumnViewColumnHeader.verticalSplitCursor;
			}

			// When the filter button is visible we're going to clip the header so that it doesn't bleed into the filter button.  Since the button, the filter 
			// button and all the controls in the header are transparent (for a visually striking effect), it is necessary to clip rather than just paint over.
			this.contentElement = this.GetTemplateChild(ColumnViewColumnHeader.contentElementPartName) as FrameworkElement;
			if (this.contentElement != null)
			{
				this.contentElement.Clip = new RectangleGeometry(new Rect(new Size(Double.PositiveInfinity, Double.PositiveInfinity)));
				this.contentElement.SizeChanged += (s, e) => { this.ClipContent(); };
			}

			// This will attach logic that will open or close the filter button when that part of the header is pressed.
			this.filterButton = this.GetTemplateChild(ColumnViewColumnHeader.filterButtonPartName) as Button;
			if (this.filterButton != null)
			{
				this.filterButton.Click += this.OnFilterButtonClick;
				this.filterButton.IsVisibleChanged += (s, e) => { this.ClipContent(); };
				this.filterButton.SizeChanged += (s, e) => { this.ClipContent(); };
			}

		}

		/// <summary>
		/// Raises the Click event.
		/// </summary>
		protected virtual void OnClick()
		{

			// This will let listeners know that the column has been clicked.
			this.RaiseEvent(new RoutedEventArgs(ClickEvent, this));

		}

		/// <summary>
		/// Represents a method that will handle the DragDelta routed event of a Thumb control.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnColumnHeaderResize(Object sender, DragDeltaEventArgs e)
		{

			// This will set the columns width to the value selected by the thumb.  This property change will propogate to the header and row presenters where the
			// other column will reposition themselves to account for the new width.
			this.Column.Width = Math.Max(this.ActualWidth + e.HorizontalChange, 0.0);

			// The drag operation was handled.
			e.Handled = true;

		}

		/// <summary>
		/// Handles a change to the Column property.
		/// </summary>
		/// <param name="dependencyObject">The object that originated the event.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The property change event arguments.</param>
		static void OnColumnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the ColumnViewColumnHeader and the property from the generic event arguments.
			ColumnViewColumnHeader columnViewColumnHeader = dependencyObject as ColumnViewColumnHeader;
			ColumnViewColumn columnViewColumn = dependencyPropertyChangedEventArgs.NewValue as ColumnViewColumn;

			// This will force the sorting properties to reflect the sort order of the underlying column.
			columnViewColumnHeader.UpdateSortDirection();

			// This will keep the header property 'HasFilters' reconciled to the same property in the column it represents.
			columnViewColumnHeader.SetValue(ColumnViewColumnHeader.hasFiltersPropertyKey, columnViewColumnHeader.Column.HasFilters);

		}

		/// <summary>
		/// Occurs when the filter button is pressed.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event data.</param>
		void OnFilterButtonClick(object sender, RoutedEventArgs e)
		{

			// Toggle the filter visibility.
			this.IsFilterOpen = !this.IsFilterOpen;

		}

		/// <summary>
		/// Handles a mouse click on the gripper.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnGripperClicked(Object sender, MouseButtonEventArgs e)
		{

			// This will prevent a single click on the gripper from being passed on to the header which is only interested in the single clicks that indicate that 
			// a sorting operation is requested.
			e.Handled = true;

		}

		/// <summary>
		/// Occurs when a mouse button is clicked two or more times. 
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnGripperDoubleClicked(Object sender, MouseButtonEventArgs e)
		{

			// This sends a routed command asking someone above this control to adjust the size to fit the largest value in the column.
			Commands.FitColumn.Execute(null, this);

			// This event has been handled.
			e.Handled = true;

		}

		/// <summary>
		/// Invoked just before the IsKeyboardFocusWithinChanged event is raised by this element.
		/// </summary>
		/// <param name="e">A DependencyPropertyChangedEventArgs that contains the event data.</param>
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{

			// Allow the base class to handle the event.
			base.OnIsKeyboardFocusWithinChanged(e);

			// This will set the IsSelected property do true when the keyboard focus is within the filter drop down.  False when the keyboard focus leaves the
			// filter drop down.
			if (base.IsKeyboardFocusWithin && !this.IsSelected)
				this.SetValue(ColumnViewColumnHeader.isSelectedPropertyKey, true);
			if (!this.IsKeyboardFocusWithin && this.IsSelected)
				this.SetValue(ColumnViewColumnHeader.isSelectedPropertyKey, false);

		}

		/// <summary>
		/// Handles a change to the dependency properties.
		/// </summary>
		/// <param name="dependencyObject">The object that originated the event.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event arguments.</param>
		static void OnIsSelectedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// The filter is closed when the selection is turned off.  The selection is a property that combines the keystrokes and the mouse gestures into a single
			// state the shows the menu header has been selected by the user for some operation.  The visual cues for the header are driven by this property and so
			// is the state of the filter drop down.
			ColumnViewColumnHeader columnViewColumnHeader = dependencyObject as ColumnViewColumnHeader;
			if ((Boolean)dependencyPropertyChangedEventArgs.OldValue)
				if (columnViewColumnHeader.IsFilterOpen)
					columnViewColumnHeader.SetValue(ColumnViewColumnHeader.IsFilterOpenProperty, false);

		}

		/// <summary>
		/// Provides class handling for the LostMouseCapture routed event that occurs when this control is no longer receiving mouse event messages. 
		/// </summary>
		/// <param name="e">The event data for the LostMouseCapture event.</param>
		protected override void OnLostMouseCapture(MouseEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// Release the 'pressed' property.  This is used to give a visual cue as to the header's state.
			if (e.OriginalSource == this)
				this.SetValue(ColumnViewColumnHeader.isPressedPropertyKey, false);

		}

		/// <summary>
		/// Invoked when an unhandled Mouse.MouseEnter attached event is raised on this element.
		/// </summary>
		/// <param name="e">The MouseEventArgs that contains the event data.</param>
		protected override void OnMouseEnter(MouseEventArgs e)
		{

			// Allow the base class to handle the event.
			base.OnMouseEnter(e);

			// This will force the 'IsSelected' property to true when the mouse is over the 'Open Filter' button.
			if (this.IsMouseOver != this.IsSelected)
				this.SetValue(ColumnViewColumnHeader.isSelectedPropertyKey, this.IsMouseOver);

		}

		/// <summary>
		/// Invoked when an unhandled Mouse.MouseLeave attached event is raised on this element.
		/// </summary>
		/// <param name="e">The MouseEventArgs that contains the event data.</param>
		protected override void OnMouseLeave(MouseEventArgs e)
		{

			// Allow the base class to handle the event.
			base.OnMouseLeave(e);

			// This will force the 'IsSelected' property to true when the mouse is over the 'Open Filter' button.
			if (this.IsMouseOver != this.IsSelected)
				this.SetValue(ColumnViewColumnHeader.isSelectedPropertyKey, this.IsMouseOver);

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

			// This will capture the mouse for as long as the mouse button is pressed.
			if (e.ButtonState == MouseButtonState.Pressed)
			{
				this.CaptureMouse();
				if (this.IsMouseCaptured)
				{
					if (e.ButtonState == MouseButtonState.Pressed)
					{
						if (!this.IsPressed)
							this.SetValue(ColumnViewColumnHeader.isPressedPropertyKey, true);
					}
					else
						this.ReleaseMouseCapture();
				}
			}

		}

		/// <summary>
		/// Invoked when an unhandled MouseLeftButtonUp routed event reaches an element in its route that is derived from this class.
		/// </summary>
		/// <param name="e">The MouseButtonEventArgs that contains the event data. The event data reports that the left mouse button was released.</param>
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{

			// Release the mouse capture when the button is release.
			if (this.IsMouseCaptured)
				this.ReleaseMouseCapture();

			// Column headers raise the click event when the mouse button is released.  This allows the consumers of this class to distinguish between a sort
			// gesture and a drag-and-drop gesture.
			if (this.Role == ColumnViewColumnHeaderRole.Normal)
				this.OnClick();

		}

		/// <summary>
		/// Invoked when an unhandled Mouse.MouseMove attached event reaches an element in its route that is derived from this class.
		/// </summary>
		/// <param name="e">The MouseEventArgs that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{

			// Allow the base class to handle the event.
			base.OnMouseMove(e);

			// This will force the 'IsSelected' property to true when the mouse is over the 'Open Filter' button.
			if (this.IsMouseOver != this.IsSelected)
				this.SetValue(ColumnViewColumnHeader.isSelectedPropertyKey, this.IsMouseOver);

		}

		/// <summary>
		/// Updates the sort indicator properties to reflect the sort order selected by the ColumnViewColumn.
		/// </summary>
		internal void UpdateSortDirection()
		{

			// It is far easier for XAML to trigger on Booleans than on enumerations, so we're going to translate the sort order into flags on the header.
			this.SetValue(ColumnViewColumnHeader.isAscendingPropertyKey, this.Column.SortDirection == SortDirection.Ascending);
			this.SetValue(ColumnViewColumnHeader.isDescendingPropertyKey, this.Column.SortDirection == SortDirection.Descending);

		}

		/// <summary>
		/// Gets the ColumnViewColumn that is associated with the ColumnViewColumnHeader.
		/// </summary>
		public ColumnViewColumn Column
		{
			get
			{
				return this.GetValue(ColumnViewColumnHeader.ColumnProperty) as ColumnViewColumn;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the column is sorting its values in ascending order.
		/// </summary>
		public Boolean HasFilters
		{
			get
			{
				return (Boolean)this.GetValue(ColumnViewColumnHeader.HasFiltersProperty);
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the column is sorting its values in ascending order.
		/// </summary>
		public Boolean IsAscending
		{
			get
			{
				return (Boolean)this.GetValue(ColumnViewColumnHeader.IsAscendingProperty);
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the column is sorting its values in descending order.
		/// </summary>
		public Boolean IsDescending
		{
			get
			{
				return (Boolean)this.GetValue(ColumnViewColumnHeader.IsDescendingProperty);
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the filter for this column is visible or not.
		/// </summary>
		public Boolean IsFilterOpen
		{
			get
			{
				return (Boolean)this.GetValue(ColumnViewColumnHeader.IsFilterOpenProperty);
			}
			set
			{
				this.SetValue(ColumnViewColumnHeader.IsFilterOpenProperty, value);
			}
		}

		/// <summary>
		/// Gets a value that indicates whether a ColumnViewColumnHeader is currently activated.
		/// </summary>
		public Boolean IsPressed
		{
			get
			{
				return (Boolean)this.GetValue(ColumnViewColumnHeader.IsPressedProperty);
			}
		}

		/// <summary>
		/// Gets a value that indicates whether a ColumnViewColumnHeader is currently activated.
		/// </summary>
		public Boolean IsSelected
		{
			get
			{
				return (Boolean)this.GetValue(ColumnViewColumnHeader.IsSelectedProperty);
			}
		}

		/// <summary>
		/// Gets the role of a GridViewColumnHeader.
		/// </summary>
		public ColumnViewColumnHeaderRole Role
		{
			get
			{
				return (ColumnViewColumnHeaderRole)this.GetValue(ColumnViewColumnHeader.RoleProperty);
			}
			internal set
			{
				this.SetValue(ColumnViewColumnHeader.rolePropertyKey, value);
			}
		}

	}

}