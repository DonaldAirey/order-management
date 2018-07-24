namespace Teraque.Windows.Controls
{

	using Teraque.Properties;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Controls.Primitives;
	using System.Windows.Data;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;

	/// <summary>
	/// A Gadget that allows the user to select a view for the content page.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ViewButton : Gadget
	{

		/// <summary>
		/// An image source for the content view.
		/// </summary>
		static ImageSource contentViewImageSource = new BitmapImage(new Uri("/Teraque.PresentationFramework;component/Resources/Content View.png", UriKind.Relative));

		/// <summary>
		/// An image source for the details view.
		/// </summary>
		static ImageSource detailsViewImageSource = new BitmapImage(new Uri("/Teraque.PresentationFramework;component/Resources/Details View.png", UriKind.Relative));

		/// <summary>
		/// An image source for the extra large icons view.
		/// </summary>
		static ImageSource extraLargeIconsViewImageSource = new BitmapImage(new Uri("/Teraque.PresentationFramework;component/Resources/Extra Large Icons View.png", UriKind.Relative));

		/// <summary>
		/// A table to map the current viewing mode to an image that indicates that mode.
		/// </summary>
		static Dictionary<ViewMode, ImageSource> imageSourceTable;

		/// <summary>
		/// An image source for the large icons view.
		/// </summary>
		static ImageSource largeIconsViewImageSource = new BitmapImage(new Uri("/Teraque.PresentationFramework;component/Resources/Large Icons View.png", UriKind.Relative));

		/// <summary>
		/// An image source for the list view.
		/// </summary>
		static ImageSource simpleListViewImageSource = new BitmapImage(new Uri("/Teraque.PresentationFramework;component/Resources/Simple List View.png", UriKind.Relative));

		/// <summary>
		/// An image source for the medium icons view.
		/// </summary>
		static ImageSource mediumIconsViewImageSource = new BitmapImage(new Uri("/Teraque.PresentationFramework;component/Resources/Medium Icons View.png", UriKind.Relative));

		/// <summary>
		/// An image source for the small icons view.
		/// </summary>
		static ImageSource smallIconsViewImageSource = new BitmapImage(new Uri("/Teraque.PresentationFramework;component/Resources/Small Icons View.png", UriKind.Relative));

		/// <summary>
		/// A table for mapping the current viewing mode to the next viewing mode.
		/// </summary>
		static Dictionary<ViewMode, ViewMode> stateChangeTable = new Dictionary<ViewMode, ViewMode>() {
			{ViewMode.Content, ViewMode.LargeIcons},
			{ViewMode.ExtraLargeIcons, ViewMode.List},
			{ViewMode.LargeIcons, ViewMode.List},
			{ViewMode.MediumIcons, ViewMode.List},
			{ViewMode.SmallIcons, ViewMode.List},
			{ViewMode.List, ViewMode.Details},
			{ViewMode.Details, ViewMode.Tiles},
			{ViewMode.Tiles, ViewMode.Content}};

		/// <summary>
		/// An image source for the tiles view.
		/// </summary>
		static ImageSource tilesViewImageSource = new BitmapImage(new Uri("/Teraque.PresentationFramework;component/Resources/Tiles View.png", UriKind.Relative));

		/// <summary>
		/// Identifies the ViewMode dependency property.
		/// </summary>
		public readonly static DependencyProperty ViewModeProperty = DependencyProperty.Register(
			"ViewMode",
			typeof(ViewMode),
			typeof(ViewButton),
			new FrameworkPropertyMetadata(ViewButton.OnViewModePropertyChanged));

		/// <summary>
		/// A range of values (in device units) used to snap the slider into a selected viewing mode.
		/// </summary>
		static Range[] viewRange = new Range[] {
			new Range(0, 1, ViewMode.Content),
			new Range(1, 2, ViewMode.Tiles),
			new Range(2, 3, ViewMode.Details),
			new Range(3, 4, ViewMode.List),
			new Range(4, 5, ViewMode.SmallIcons),
			new Range(5, 32, ViewMode.MediumIcons),
			new Range(32, 52, ViewMode.LargeIcons),
			new Range(52, 79, ViewMode.ExtraLargeIcons)};

        /// <summary>
        /// Identifies the Value dependency property.
        /// </summary>
        public readonly static DependencyProperty ViewValueProperty = DependencyProperty.Register(
            "ViewValue",
            typeof(Int32),
            typeof(ViewButton),
            new FrameworkPropertyMetadata(Int32.MinValue, ViewButton.OnValuePropertyChanged));

        /// <summary>
		/// A mapping of the view modes to the normalized value of those modes on the slider.
		/// </summary>
		static Dictionary<ViewMode, Int32> viewValues = new Dictionary<ViewMode, Int32>() {
			{ViewMode.Content, 0},
			{ViewMode.Tiles, 1},
			{ViewMode.Details, 2},
			{ViewMode.List, 3},
			{ViewMode.SmallIcons, 4},
			{ViewMode.MediumIcons, 31},
			{ViewMode.LargeIcons, 51},
			{ViewMode.ExtraLargeIcons, 79}};

		/// <summary>
		/// Used to store the ranges of slider values used to snap the slider into place.
		/// </summary>
		struct Range
		{

			/// <summary>
			/// The minimum value of the range.
			/// </summary>
			public Int32 Minimum;

			/// <summary>
			/// The maximum value of the range.
			/// </summary>
			public Int32 Maximum;

			/// <summary>
			/// The view mode associated with the range.
			/// </summary>
			public ViewMode ViewMode;

			/// <summary>
			/// Initializes a new instance of the Range structure.
			/// </summary>
			/// <param name="minimum">The minimum value of a range.</param>
			/// <param name="maximum">The maximum value of a range.</param>
			/// <param name="viewMode">The ViewMode associated with the range.</param>
			public Range(Int32 minimum, Int32 maximum, ViewMode viewMode)
			{

				// Initialize the object.
				this.Minimum = minimum;
				this.Maximum = maximum;
				this.ViewMode = viewMode;

			}

		}

		/// <summary>
		/// Initialize the ViewButton type.
		/// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static ViewButton()
		{

			// This table provides images for each of the view modes that appears on the button.  Initialization here (as opposed to initializing where the field 
			// is declared) guarantees that all of the static variables containing the images have been initialized.
			ViewButton.imageSourceTable = new Dictionary<ViewMode, ImageSource>() {
				{ViewMode.Content, ViewButton.contentViewImageSource},
				{ViewMode.Details, ViewButton.detailsViewImageSource},
				{ViewMode.ExtraLargeIcons, ViewButton.extraLargeIconsViewImageSource},
				{ViewMode.LargeIcons, ViewButton.largeIconsViewImageSource},
				{ViewMode.List, ViewButton.simpleListViewImageSource},
				{ViewMode.MediumIcons, ViewButton.mediumIconsViewImageSource},
				{ViewMode.SmallIcons, ViewButton.smallIconsViewImageSource},
				{ViewMode.Tiles, ViewButton.tilesViewImageSource}};

		}

		/// <summary>
		/// Initializes a new instance of the ViewButton class.
		/// </summary>
		public ViewButton()
		{

			// This will handle the part of the Gadget that acts like a button when it is clicked.
			this.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.OnButtonClick));

			// If the actual value of a property hasn't changed, then the property change handler isn't called.  This is a problem on initialization because if you 
			// try to set the property to an explicit value and that value is the same as the default for that property, then there's no implicit way to call the 
			// property change handler.  This trick will force the button into a known state by explicitly calling the event handler.
			ViewMode viewMode = (ViewMode)ViewButton.ViewModeProperty.DefaultMetadata.DefaultValue;
			ViewButton.OnViewModePropertyChanged(this, new DependencyPropertyChangedEventArgs(ViewButton.ViewModeProperty, null, viewMode));

			// This is the header for the button which is normally invisible when it is parked on the far side of the toolbar.  It become visible on the right side 
			// and when it overflows.
			this.Header = Properties.Resources.Views;

			// This provides the tool tip for the button.
			this.ToolTip = Properties.Resources.ChangeViewToolTip;

		}

		/// <summary>
		/// Gets or sets the magnification.
		/// </summary>
		public Int32 ViewValue
		{
			get
			{
				return (Int32)this.GetValue(ViewButton.ViewValueProperty);
			}
			set
			{
				this.SetValue(ViewButton.ViewValueProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the viewing mode of this button.
		/// </summary>
		ViewMode ViewMode
		{
			get
			{
				return (ViewMode)this.GetValue(ViewButton.ViewModeProperty);
			}
			set
			{
				this.SetValue(ViewButton.ViewModeProperty, value);
			}
		}

		/// <summary>
        /// Determines if the specified item is (or is eligible to be) its own ItemContainer.
		/// </summary>
		/// <param name="item">Specified item.</param>
		/// <returns>true if the item is its own ItemContainer; otherwise, false.</returns>
		protected override bool IsItemItsOwnContainerOverride(object item)
		{

            // The drop down is it's own container.  Without this override, the drop down will act like a MenuItem.
			return item is ViewButtonDropDown;

		}

		/// <summary>
		/// Handles the clicking of the button part of the control.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="routedEventArgs">The event data.</param>
		void OnButtonClick(Object sender, RoutedEventArgs routedEventArgs)
		{

			// If the popup is already open, then clicking the button will close it.  If it is closed, then clicking the button will change the state to the next 
			// view mode.
			if (this.IsSubmenuOpen)
				this.IsSubmenuOpen = false;
			else
			{

				// This will switch to the next mode based on the current mode and snap the slider into position.
				this.ViewMode = ViewButton.stateChangeTable[this.ViewMode];
				this.ViewValue = ViewButton.viewValues[this.ViewMode];

			}

			// That's it: no more handling required.
			routedEventArgs.Handled = true;

		}

		/// <summary>
		/// Responds to the KeyDown event.
		/// </summary>
		/// <param name="e">The event data for the KeyDown event.</param>
		[SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase")]
		protected override void OnKeyDown(KeyEventArgs e)
		{

            // Validate parameters
            if (e == null)
                throw new ArgumentNullException("e");

            // Keyboard navigation is not supported on this drop down, though it would be interesting to try to make the design work.  The slider will get the focus
			// whenever the popup is present and there's no defined way to tab to the other controls in the popup.  In order to be compatible with the Windows
			// Explorer version of this control, the tab key is a signal to close the popup.  In this design there is no ItemsControl to manage the keyboard
			// navigation for tabbing, so we must handle it here.
			if (this.IsSubmenuOpen && e.Key == Key.Tab)
				this.IsSubmenuOpen = false;

			// There is significant processing by the base class on this event.  Preformance will be impacted if this method isn't called.
			base.OnKeyDown(e);

		}

        /// <summary>
        /// Invoked when an unhandled Mouse.LostMouseCapture attached event reaches an element in its route that is derived from this class.
        /// </summary>
        /// <param name="e">The MouseEventArgs that contains event data.</param>
        protected override void OnLostMouseCapture(MouseEventArgs e)
        {

            // Validate parameters
            if (e == null)
                throw new ArgumentNullException("e");

            // Moving the slider to a new position will automatically close the submenu.
            if (e.Source is ViewButtonDropDown)
                this.IsSubmenuOpen = false;

            // This method has no default implementation. Because an intermediate class in the inheritance might implement this method, we recommend that you call 
            // the base implementation in your implementation.
            base.OnLostMouseCapture(e);

        }

        /// <summary>
        /// Invoked when the effective property value of the Value property changes.
        /// </summary>
        /// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
        /// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
        /// property.</param>
        static void OnValuePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {

            // Extract the ViewButton and the new view value from the property change event arguments.
            ViewButton viewButton = dependencyObject as ViewButton;
            Int32 viewValue = (Int32)dependencyPropertyChangedEventArgs.NewValue;

            // This will snap the slider into a distinct position for a viewing mode.  Note that any minimum or maximum can be used for the slider control and the 
            // scaling calculations will adjust.
            foreach (Range range in ViewButton.viewRange)
                if (range.Minimum <= viewValue && viewValue < range.Maximum)
                    viewButton.ViewMode = range.ViewMode;

        }

		/// <summary>
		/// Invoked when the effective property value of the ViewMode property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnViewModePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the ViewButton and the new view mode from the property change event arguments.
			ViewButton viewButton = dependencyObject as ViewButton;
			ViewMode viewMode = (ViewMode)dependencyPropertyChangedEventArgs.NewValue;

			// This uses a table of image sources to match the image on the button with the mode selected.
			viewButton.Icon = new Image() { Source = ViewButton.imageSourceTable[viewMode] };

		}

		/// <summary>
		/// Called when the parent of the visual MenuItem changes.
		/// </summary>
		/// <param name="oldParent">Old value of the parent of the visual, or null if the visual did not have a parent.</param>
		protected override void OnVisualParentChanged(DependencyObject oldParent)
		{

			// This control will automatically bind to properties on a TreeFrame.  However, the control may be moved from one parent to the next, such as when it
			// moves from the overflow panel back to the main tool bar.  To handle the fact that the visual tree can change, this method is need to disconnect
			// the control when it is in limbo and reconnect it when it's found a new home.
			ExplorerFrame explorerFrame = VisualTreeExtensions.FindAncestor<ExplorerFrame>(this);
			if (explorerFrame == null)
			{

				// Disconnect the bindings to the ancestor TreeFrame when the control is floating without a TreeFrame ancestor.
				BindingOperations.ClearBinding(this, ViewButton.ViewValueProperty);
				BindingOperations.ClearBinding(this, ViewButton.ViewModeProperty);

			}
			else
			{

				// This will automatically connect the Value property to the ancestor TreeView when it's found a new home.
				Binding viewValueBinding = new Binding();
				viewValueBinding.Path = new PropertyPath("ViewValue");
				viewValueBinding.Source = explorerFrame;
				viewValueBinding.Mode = BindingMode.TwoWay;
				BindingOperations.SetBinding(this, ViewButton.ViewValueProperty, viewValueBinding);

			}

			// Allow the base class to handle the rest of the event.
			base.OnVisualParentChanged(oldParent);

		}

	}

}
