namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections;
	using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Input;
	using System.Windows.Navigation;
	using System.Windows.Media.Animation;

	/// <summary>
	/// Allows a user to select from a hierarchy of items.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class Navigator : TreeView
	{

		/// <summary>
		/// Identifies the ExpanderOpacity dependency property key.
		/// </summary>
		public static readonly DependencyProperty ExpanderOpacityProperty = DependencyProperty.Register(
			"ExpanderOpacity",
			typeof(Double),
			typeof(Navigator),
			new FrameworkPropertyMetadata(0.0));

		/// <summary>
		/// Identifies the Indent dependency property.
		/// </summary>
		public static readonly DependencyProperty IndentProperty = DependencyProperty.Register(
			"Indent",
			typeof(Double),
			typeof(Navigator),
			new FrameworkPropertyMetadata(10.0));

		/// <summary>
		/// Identifies the IsInViewWhenSelected attached dependency property.
		/// </summary>
		public static readonly DependencyProperty IsInViewWhenSelectedProperty = DependencyProperty.RegisterAttached(
			"IsInViewWhenSelected",
			typeof(Boolean),
			typeof(Navigator),
			new FrameworkPropertyMetadata(false, Navigator.OnIsInViewWhenSelectedChanged));

		/// <summary>
		/// Identifies the Source dependency property.
		/// </summary>
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
			"Source",
			typeof(Uri),
			typeof(Navigator),
			new PropertyMetadata(Navigator.OnSourcePropertyChanged));

		/// <summary>
		/// Initialize the Navigator class.
		/// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static Navigator()
		{

			// This allows the templates in resource files to be properly associated with this new class.  Without this override, the type of the base class would 
			// be used as the key in any lookup involving resources dictionaries.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Navigator), new FrameworkPropertyMetadata(typeof(Navigator)));

			// The horizontal and vertical scroll bars are automatic for this control class.
			ScrollViewer.HorizontalScrollBarVisibilityProperty.OverrideMetadata(typeof(Navigator), new FrameworkPropertyMetadata(ScrollBarVisibility.Auto));
			ScrollViewer.VerticalScrollBarVisibilityProperty.OverrideMetadata(typeof(Navigator), new FrameworkPropertyMetadata(ScrollBarVisibility.Auto));

		}

		/// <summary>
		/// Gets the opacity for the expander buttons on the child TreeViewItem elements.
		/// </summary>
		public Double ExpanderOpacity
		{
			get
			{
				return (Double)this.GetValue(Navigator.ExpanderOpacityProperty);
			}
		}

		/// <summary>
		/// Gets or sets the amount of space that each child is indented from the parent.
		/// </summary>
		public Double Indent
		{
			get
			{
				return (Double)this.GetValue(Navigator.IndentProperty);
			}
			set
			{
				this.SetValue(Navigator.IndentProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the uniform resource identifier (URI) of the current content addressed by the Navigator.
		/// </summary>
		public Uri Source
		{
			get
			{
				return GetValue(Navigator.SourceProperty) as Uri;
			}
			set
			{
				SetValue(Navigator.SourceProperty, value);
			}
		}

		/// <summary>
		/// Creates a NavigatorItem to use to display content.
		/// </summary>
		/// <returns>A new TreeViewItem to use as a container for content.</returns>
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new NavigatorItem();
		}

		/// <summary>
		/// Gets an indication whether the given object should scroll into view when selected.
		/// </summary>
		/// <param name="dependencyObject">The element from which the property value is read.</param>
		/// <returns>The IsInViewWhenSelected property value for the element.</returns>
		public static Boolean GetIsInViewWhenSelected(DependencyObject dependencyObject)
		{

			// Validate the parameters.
			if (dependencyObject == null)
				throw new ArgumentNullException("dependencyObject");

			// This value indicates that the item was in the view when it was selected.
			return (Boolean)dependencyObject.GetValue(Navigator.IsInViewWhenSelectedProperty);

		}

		/// <summary>
		/// Determines whether the specified item is its own container or can be its own container.
		/// </summary>
		/// <param name="item">The object to evaluate.</param>
		/// <returns>true if item is a NavigatorItem; otherwise, false.</returns>
		protected override Boolean IsItemItsOwnContainerOverride(Object item)
		{
			return item is NavigatorItem;
		}

		/// <summary>
		/// Navigate to the given source URI.
		/// </summary>
		/// <param name="source">A fully qualified description of an item in the hierarchy.</param>
		void NavigateTo(Uri source)
		{

			// When we have a valid data context and a source, this will find the item in the hiearchy given the source and select it, optionally opening up its 
			// parent if one exists.
			if (this.DataContext != null && source != null)
			{
				IExplorerItem iExplorerItem = ExplorerHelper.FindExplorerItem(this.DataContext as IExplorerItem, source);
				if (iExplorerItem != null)
				{
					IExplorerItem parent = iExplorerItem.Parent;
					if (parent != null)
						parent.IsExpanded = true;
					iExplorerItem.IsSelected = true;
				}
			}

		}

		/// <summary>
		/// Invoked when the effective property value of the IsInViewWhenSelected property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnIsInViewWhenSelectedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the specific arguments from the generic event arguments.
			TreeViewItem treeViewItem = dependencyObject as TreeViewItem;
			Boolean isInViewWhenSelected = (Boolean)dependencyPropertyChangedEventArgs.NewValue;

			// This will install and uninstall the handler for the TreeViewItem that has the attached property.  The event handler does the hard work of bringing
			// the item into view when selected.  This attached property handler provides the connection to the TreeViewItem no matter where it may be in the
			// hierarchy.  That is, no matter how deeply buried the TreeViewItem node is, the MVVM patter will find it when selected and bring it into view.  This
			// is a pretty amazing coding practice if you stop and think about how much code this would have been procedurally.
			if (treeViewItem != null)
				if (isInViewWhenSelected)
					treeViewItem.Selected += Navigator.OnTreeViewItemSelected;
				else
					treeViewItem.Selected -= Navigator.OnTreeViewItemSelected;

		}

		/// <summary>
		/// Called when the ItemsSource property changes.
		/// </summary>
		/// <param name="oldValue">Old value of the ItemsSource property.</param>
		/// <param name="newValue">New value of the ItemsSource property.</param>
		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{

			// The 'Source' property addresses an item in a hierarchy.  If the hierarchy changes then the current source URI needs to be evaluated again.
			this.NavigateTo(this.Source);

		}

		/// <summary>
		/// Invoked when the effective property value of the Source property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this property.</param>
		static void OnSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Navigate to the new source URI when the property changes.
			Navigator navigator = dependencyObject as Navigator;
			navigator.NavigateTo(dependencyPropertyChangedEventArgs.NewValue as Uri);

		}

		/// <summary>
		/// Provides class handling for the KeyDown event for a Navigator.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// This will open the item that generated the event.
			if (e.Key == Key.Enter)
				this.OpenItem(e.OriginalSource as FrameworkElement);

			// The base class does a good job with the rest of the navigation keys.
			base.OnPreviewKeyDown(e);

		}

		/// <summary>
		/// Invoked when an unhandled Mouse.PreviewMouseDown attached routed event reaches this element in its route that is derived from this class.
		/// </summary>
		/// <param name="e">The MouseButtonEventArgs that contains the event data. The event data reports that one or more mouse buttons were pressed.</param>
		protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// A single click will open up the selected item.
			if (e.ClickCount == 1 && e.ButtonState == MouseButtonState.Pressed)
				OpenItem(e.OriginalSource as FrameworkElement);

			// Allow the base class to handle the rest of the event.
			base.OnPreviewMouseDown(e);

		}

		/// <summary>
		/// Invoked whenever the effective value of any dependency property on this FrameworkElement has been updated.
		/// </summary>
		/// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{

			// If either the keyboard focus is moved into the navigator or the mouse is over it the expander buttons on the TreeViewItems will light up.  That is,
			// the opacity of the expander buttons is controlled by these two properties.  This does not directly control the expander buttons but provides a
			// property that they can bind to through the visual tree hierarchy.  It also saves on Storyboards as only one animation needs to be created for all 
			// the buttons that might appear in a heavily populated navigation hierarchy.
			Boolean turnOn = false;
			Boolean turnOff = false;

			// Evaluate whether the focus change will make the expander buttons visible or not.
			if (e.Property == UIElement.IsKeyboardFocusWithinProperty)
			{
				turnOn = (Boolean)e.NewValue;
				turnOff = !(Boolean)e.NewValue && !this.IsMouseOver;
			}

			// Evaluate wheter the mouse hovering over the control will make the expander buttons visible or not.
			if (e.Property == UIElement.IsMouseOverProperty)
			{
				turnOn = (Boolean)e.NewValue;
				turnOff = !(Boolean)e.NewValue && !this.IsKeyboardFocusWithin;
			}

			// This animation will make the expander buttons visible.
			if (turnOn)
				this.BeginAnimation(Navigator.ExpanderOpacityProperty, new DoubleAnimation(1.0, TimeSpan.FromMilliseconds(250)));

			// This animation will make the expander buttons invisible.
			if (turnOff)
				this.BeginAnimation(Navigator.ExpanderOpacityProperty, new DoubleAnimation(0.0, TimeSpan.FromMilliseconds(1000)));

			// Allow the base class to handle the rest of the property changes.
			base.OnPropertyChanged(e);

		}

		/// <summary>
		/// Invoked when the TreeViewItem is selected.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="routedEventArgs">The event data.</param>
		static void OnTreeViewItemSelected(Object sender, RoutedEventArgs routedEventArgs)
		{

			// The TreeView will pass this event on to every item in the hierarchy from the item that generated it up to the root item.  We don't want to bring all
			// these items into view, just the item that generated the event (that is, the original source).
			if (Object.ReferenceEquals(sender, routedEventArgs.OriginalSource))
			{
				TreeViewItem item = routedEventArgs.OriginalSource as TreeViewItem;
				if (item != null)
					item.BringIntoView();
			}

		}

		/// <summary>
		/// Opens the given item.
		/// </summary>
		/// <param name="frameworkElement">The selected element to be opened.</param>
		void OpenItem(FrameworkElement frameworkElement)
		{

			// This will open a user interface element that has an IExplorerItem associated with it (as long as the element selectes is not the root element).
			if (frameworkElement != null && frameworkElement.DataContext != this.DataContext)
			{
				IExplorerItem iExplorerItem = frameworkElement.DataContext as IExplorerItem;
				if (iExplorerItem != null)
				{
					Uri source = ExplorerHelper.GenerateSource(iExplorerItem);
					if (this.Source != source)
						this.Source = source;
				}
			}

		}

		/// <summary>
		/// Sets whether the given object will scroll into view automatically when selected.
		/// </summary>
		/// <param name="dependencyObject">The element from which the property value is read.</param>
		/// <param name="value">An indication of whether or not the given object is scrolled into view when selected.</param>
		public static void SetIsInViewWhenSelected(DependencyObject dependencyObject, Boolean value)
		{

			// Validate the parameters.
			if (dependencyObject == null)
				throw new ArgumentNullException("dependencyObject");

			// This indicates that the item was in the view when it was selected.
			dependencyObject.SetValue(IsInViewWhenSelectedProperty, value);

		}

	}

}
