namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections;
	using System.Diagnostics.CodeAnalysis;
	using System.Resources;
	using System.Windows;
	using System.Windows.Data;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Markup;
	using Teraque.Windows;
	using Teraque.Windows.Input;

	/// <summary>
	/// A page that can display data in multiple views.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class ViewPage : ExplorerPage
	{

		/// <summary>
		/// The Content View.
		/// </summary>
		ContentView contentView;

		/// <summary>
		/// The Details View.
		/// </summary>
		DetailsView detailsView;

		/// <summary>
		/// The Extra Large Icons View.
		/// </summary>
		ExtraLargeIconsView extraLargeIconsView;

		/// <summary>
		/// Identifies the IconSize dependency property.
		/// </summary>
		public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
			"IconSize",
			typeof(Double),
			typeof(ViewPage));

		/// <summary>
		/// Identifies the ItemWidth dependency property.
		/// </summary>
		public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(
			"ItemWidth",
			typeof(Double),
			typeof(ViewPage));

		/// <summary>
		/// The Large Icons View.
		/// </summary>
		LargeIconsView largeIconsView;

		/// <summary>
		/// This is the list box that hosts all the views.
		/// </summary>
		ListBoxView listBoxView;

		/// <summary>
		/// The Medium Icons View.
		/// </summary>
		MediumIconsView mediumIconsView;

		/// <summary>
		/// Identifies the Source dependency property.
		/// </summary>
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
			"Source",
			typeof(Uri),
			typeof(ViewPage),
			new FrameworkPropertyMetadata(null, ViewPage.OnSourcePropertyChanged));

		/// <summary>
		/// The List View.
		/// </summary>
		SimpleListView simpleListView;

		/// <summary>
		/// The Small Icons View.
		/// </summary>
		SmallIconsView smallIconsView;

		/// <summary>
		/// The Tiles View.
		/// </summary>
		TilesView tilesView;

		/// <summary>
		/// Identifies the ViewValue dependency property.
		/// </summary>
		public readonly static DependencyProperty ViewValueProperty = DependencyProperty.Register(
			"ViewValue",
			typeof(Int32),
			typeof(ViewPage),
			new FrameworkPropertyMetadata(Int32.MinValue, ViewPage.OnViewValuePropertyChanged));

		/// <summary>
		/// Maps a ViewValue (a position on a slider) to a set of attributes for the view.
		/// </summary>
		Range[] viewValueRangeMap;
		
		/// <summary>
		/// Initializes the ViewPage class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static ViewPage()
		{

			// This allows the templates in resource files to be properly associated with this new class.  Without this override, the type of the base class would
			// be used as the key in any lookup involving resources dictionaries.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewPage), new FrameworkPropertyMetadata(typeof(ViewPage)));

			// This is a complex control and will manage it's own focus scope.
			FocusManager.IsFocusScopeProperty.OverrideMetadata(typeof(ViewPage), new FrameworkPropertyMetadata(true));

		}

		/// <summary>
		/// Initializes a new instance of the ViewPage class.
		/// </summary>
		public ViewPage()
		{

			// The content for this class is a list box that is capable of showing several views of the same data.
			this.listBoxView = new ListBoxView();
			this.Content = this.listBoxView;

			// The immutable command bindings.
			this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, this.OnOpenItem));

			// These are the various views supported by this items control (which is also a page).
			this.contentView = new ContentView();
			this.detailsView = XamlReader.Load(
				Application.GetResourceStream(
				new Uri("/Teraque.PresentationFramework;component/Controls/Views/DetailsView.xaml", UriKind.Relative)).Stream) as DetailsView;
			this.extraLargeIconsView = new ExtraLargeIconsView();
			this.largeIconsView = new LargeIconsView();
			this.simpleListView = new SimpleListView();
			this.mediumIconsView = new MediumIconsView();
			this.smallIconsView = new SmallIconsView();
			this.tilesView = new TilesView();

			// The icon size and the width of the cell that holds the item are controlled by the view model.  That is, no matter where an item is in the views or
			// how many items there are, they will all get their icon size and the width of the cell from the properties of this page.
			this.BindIconsViewProperties(this.smallIconsView);
			this.BindIconsViewProperties(this.mediumIconsView);
			this.BindIconsViewProperties(this.largeIconsView);
			this.BindIconsViewProperties(this.extraLargeIconsView);

			// This table drives the view used to present the items and the attributes of the items in the display.
			this.viewValueRangeMap = new Range[] {
				new Range(this.contentView, Double.NaN, Double.NaN), new Range(this.tilesView, Double.NaN, Double.NaN),
				new Range(this.detailsView, Double.NaN, Double.NaN), new Range(this.simpleListView, Double.NaN, Double.NaN),
				new Range(this.smallIconsView, 16.0, 230.0), new Range(this.mediumIconsView, 17.0, 230.0), new Range(this.mediumIconsView, 18.0, 230.0),
				new Range(this.mediumIconsView, 19.0, 230.0), new Range(this.mediumIconsView, 20.0, 230.0),	new Range(this.mediumIconsView, 21.0, 230.0),
				new Range(this.mediumIconsView, 22.0, 230.0), new Range(this.mediumIconsView, 23.0, 230.0),	new Range(this.mediumIconsView, 24.0, 230.0),
				new Range(this.mediumIconsView, 25.0, 230.0), new Range(this.mediumIconsView, 26.0, 230.0), new Range(this.mediumIconsView, 27.0, 230.0),
				new Range(this.mediumIconsView, 28.0, 230.0), new Range(this.mediumIconsView, 29.0, 230.0), new Range(this.mediumIconsView, 30.0, 230.0),
				new Range(this.mediumIconsView, 31.0, 230.0), new Range(this.mediumIconsView, 32.0, 230.0), new Range(this.largeIconsView, 33.0, 74.0),
				new Range(this.largeIconsView, 34.0, 74.0), new Range(this.largeIconsView, 35.0, 74.0), new Range(this.largeIconsView, 37.0, 74.0),
				new Range(this.largeIconsView, 38.0, 74.0), new Range(this.largeIconsView, 39.0, 74.0), new Range(this.largeIconsView, 41.0, 74.0),
				new Range(this.largeIconsView, 42.0, 74.0), new Range(this.largeIconsView, 44.0, 74.0), new Range(this.largeIconsView, 45.0, 74.0),
				new Range(this.largeIconsView, 48.0, 74.0), new Range(this.extraLargeIconsView, 49.0, 74.0), new Range(this.extraLargeIconsView, 50.0, 74.0),
				new Range(this.extraLargeIconsView, 52.0, 74.0), new Range(this.extraLargeIconsView, 54.0, 74.0), new Range(this.extraLargeIconsView, 56.0, 74.0),
				new Range(this.extraLargeIconsView, 58.0, 74.0), new Range(this.extraLargeIconsView, 60.0, 74.0), new Range(this.extraLargeIconsView, 62.0, 74.0),
				new Range(this.extraLargeIconsView, 64.0, 74.0), new Range(this.extraLargeIconsView, 67.0, 74.0), new Range(this.extraLargeIconsView, 69.0, 77.0),
				new Range(this.extraLargeIconsView, 72.0, 80.0), new Range(this.extraLargeIconsView, 74.0, 82.0), new Range(this.extraLargeIconsView, 77.0, 85.0),
				new Range(this.extraLargeIconsView, 80.0, 88.0), new Range(this.extraLargeIconsView, 83.0, 92.0), new Range(this.extraLargeIconsView, 86.0, 95.0),
				new Range(this.extraLargeIconsView, 89.0, 98.0), new Range(this.extraLargeIconsView, 92.0, 101.0), new Range(this.extraLargeIconsView, 95.0, 104.0),
				new Range(this.extraLargeIconsView, 99.0, 108.0), new Range(this.extraLargeIconsView, 102.0, 111.0),
				new Range(this.extraLargeIconsView, 106.0, 115.0), new Range(this.extraLargeIconsView, 110.0, 120.0),
				new Range(this.extraLargeIconsView, 114.0, 124.0), new Range(this.extraLargeIconsView, 118.0, 128.0),
				new Range(this.extraLargeIconsView, 122.0, 132.0), new Range(this.extraLargeIconsView, 126.0, 136.0),
				new Range(this.extraLargeIconsView, 131.0, 141.0), new Range(this.extraLargeIconsView, 136.0, 147.0),
				new Range(this.extraLargeIconsView, 140.0, 151.0), new Range(this.extraLargeIconsView, 145.0, 156.0),
				new Range(this.extraLargeIconsView, 151.0, 162.0), new Range(this.extraLargeIconsView, 156.0, 167.0),
				new Range(this.extraLargeIconsView, 162.0, 173.0), new Range(this.extraLargeIconsView, 168.0, 180.0),
				new Range(this.extraLargeIconsView, 174.0, 186.0), new Range(this.extraLargeIconsView, 180.0, 192.0),
				new Range(this.extraLargeIconsView, 186.0, 198.0), new Range(this.extraLargeIconsView, 193.0, 206.0),
				new Range(this.extraLargeIconsView, 200.0, 213.0), new Range(this.extraLargeIconsView, 207.0, 220.0),
				new Range(this.extraLargeIconsView, 215.0, 228.0), new Range(this.extraLargeIconsView, 223.0, 237.0),
				new Range(this.extraLargeIconsView, 231.0, 245.0), new Range(this.extraLargeIconsView, 239.0, 253.0),
				new Range(this.extraLargeIconsView, 248.0, 263.0), new Range(this.extraLargeIconsView, 256.0, 271.0)};

			// This will initialize the view settings.  Note that the value is different than the default and is an explicit command to set the view and all the
			// other aspects of the view model.
			this.ViewValue = 0;

			// Since this is a page it is expected to load and unload from memory at any time and handle itself accordingly. These event handlers will take care of
			// binding to the frame application when loaded and unbinding when not part of the application anymore.
			this.Loaded += new RoutedEventHandler(this.OnLoaded);
			this.Unloaded += new RoutedEventHandler(this.OnUnloaded);

		}

		/// <summary>
		/// Gets or sets the size of the icon in this view.
		/// </summary>
		public Double IconSize
		{
			get
			{
				return (Double)this.GetValue(ViewPage.IconSizeProperty);
			}
			set
			{
				this.SetValue(ViewPage.IconSizeProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets width of an item in this view.
		/// </summary>
		public Double ItemWidth
		{
			get
			{
				return (Double)this.GetValue(ViewPage.ItemWidthProperty);
			}
			set
			{
				this.SetValue(ViewPage.ItemWidthProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the uniform resource identifier (URI) of the current content.
		/// </summary>
		public Uri Source
		{
			get
			{
				return (Uri)this.GetValue(ViewPage.SourceProperty);
			}
			set
			{
				this.SetValue(ViewPage.SourceProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value that determines what kind of a view is used and at what magnification the icons are shown.
		/// </summary>
		public Int32 ViewValue
		{
			get
			{
				return (Int32)this.GetValue(ViewPage.ViewValueProperty);
			}
			set
			{
				this.SetValue(ViewPage.ViewValueProperty, value);
			}
		}

		/// <summary>
		/// Binds the view to the model view settings in this control.
		/// </summary>
		/// <param name="iconsView">The icon-based view to be bound.</param>
		void BindIconsViewProperties(IconsView iconsView)
		{

			// This will bind the icon size to the properties of this control.  In this way a single setting will control the size of all the icons in the view.
			Binding iconSizeBinding = new Binding();
			iconSizeBinding.Path = new PropertyPath("IconSize");
			iconSizeBinding.Source = this;
			BindingOperations.SetBinding(iconsView, IconsView.IconSizeProperty, iconSizeBinding);

			// This will bind the item width to the properties of this control.  In this way a single setting will control the width of all the items in the view.
			Binding itemWidthBinding = new Binding();
			itemWidthBinding.Path = new PropertyPath("ItemWidth");
			itemWidthBinding.Source = this;
			BindingOperations.SetBinding(iconsView, IconsView.ItemWidthProperty, itemWidthBinding);

		}

		/// <summary>
		/// Invoked when a GotKeyboardFocus attached event reaches this element in its route.
		/// </summary>
		/// <param name="e">The KeyboardFocusChangedEventArgs that contains the event data.</param>
		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// This allows the ViewPage to receive the focus and then give it to the list box.  A consumer of this control doesn't need to know anything about the 
			// internal structure of the control.  This code is only used on initialization as the keyboard navigation will find the tab stops by digging into the 
			// control once it has been loaded and initialized.
			if (e.NewFocus == this)
			{
				DependencyObject focusScope = FocusManager.GetFocusScope(this);
				IInputElement focusedElement = FocusManager.GetFocusedElement(focusScope);
				if (focusedElement == null)
					FocusManager.SetFocusedElement(focusScope, this.listBoxView);
			}

		}

		/// <summary>
		/// Occurs when the element is laid out, rendered, and ready for interaction.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="routedEventArgs">The event data.</param>
		void OnLoaded(Object sender, RoutedEventArgs routedEventArgs)
		{

			// This object will automatically bind itself to a parent TreeFrame when one is available.
			ExplorerFrame explorerFrame = VisualTreeExtensions.FindAncestor<ExplorerFrame>(this);
			if (explorerFrame != null)
			{

				// The children of a Frame do not automatically inherit the data context of the parent window.  This is likely due to the fact that pages are not
				// naturally kept alive when the navigation moves away.  So any data binding operation must be established or re-established when the page is
				// loaded and must be cleared when the page is unloaded.  This will bind this page to the context of the parent frame (for now).
				Binding dataContextBinding = new Binding();
				dataContextBinding.Path = new PropertyPath("DataContext");
				dataContextBinding.Source = explorerFrame;
				dataContextBinding.Mode = BindingMode.OneWay;
				BindingOperations.SetBinding(this, ViewPage.DataContextProperty, dataContextBinding);

				// The Source property binding allows a change to the property to propogate up to the container.
				Binding sourceBinding = new Binding();
				sourceBinding.Path = new PropertyPath("Source");
				sourceBinding.Source = explorerFrame;
				sourceBinding.Mode = BindingMode.TwoWay;
				BindingOperations.SetBinding(this, ViewPage.SourceProperty, sourceBinding);

				// This value indicates what kind of view is used to display the items.
				Binding viewValueBinding = new Binding();
				viewValueBinding.Path = new PropertyPath("ViewValue");
				viewValueBinding.Source = explorerFrame;
				viewValueBinding.Mode = BindingMode.TwoWay;
				BindingOperations.SetBinding(this, ViewPage.ViewValueProperty, viewValueBinding);

			}

		}

		/// <summary>
		/// Invoked when the effective property value of the Source property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this property.
		/// </param>
		private static void OnSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will disable the navigation when the source URI is changed retroactively to the page navigation.  That is, when the journal, forward button, 
			// backward button change the current page, the source URI is set after the fact in order to reflect the location of the selected page.  Conversly, 
			// when a breadcrumb control or tree view changes the source URI, that should be taken as an instruction to navigate to the selected page.
			ViewPage viewPage = dependencyObject as ViewPage;
			Uri newSource = dependencyPropertyChangedEventArgs.NewValue as Uri;

			// When the source URI has changed we need a new data context for the new source URI.  This effectively chooses the items that appear in any one of the 
			// views provided by this class by selecting the IExplorerItem as the data context for the page.
			if (newSource != null)
			{
				IExplorerItem rootItem = viewPage.DataContext as IExplorerItem;
				IExplorerItem iExplorerItem = ExplorerHelper.FindExplorerItem(rootItem, newSource);
				if (iExplorerItem != null)
				{
					IExplorerItem parentItem = iExplorerItem;
					CompositeCollection compositCollection = new CompositeCollection();
					compositCollection.Add(new CollectionContainer() { Collection = parentItem as IEnumerable });
					compositCollection.Add(new CollectionContainer() { Collection = parentItem.Leaves });
					viewPage.listBoxView.ItemsSource = compositCollection;
				}
			}

		}

		/// <summary>
		/// Opens the currently selected item in the view.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="routedEventArgs">The event data.</param>
		void OnOpenItem(Object sender, RoutedEventArgs routedEventArgs)
		{

			// This will take the currently selected item, if it exists, and open it in a view.  Note that the opening isn't explicit, but is handled through a long
			// chain of binding where the application frame will be notified of a source URI change and then navigate to the new source URI.  In this manner setting
			// the source URI down here will fully integrate the new page into the browser journal, the breadcrumb bar and the navigator tree view.
			IExplorerItem iExplorerItem = this.listBoxView.SelectedItem as IExplorerItem;
			if (iExplorerItem != null)
				Commands.OpenItem.Execute(iExplorerItem, null);

		}

		/// <summary>
		/// Occurs when the element is removed from within an element tree of loaded elements.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="routedEventArgs">The event data.</param>
		void OnUnloaded(Object sender, RoutedEventArgs routedEventArgs)
		{

			// When this control is unloaded from its parent container the binding to that parent must be cleared or errors will be generated before if the item isn't
			// immediately garbage collected.
			if (BindingOperations.IsDataBound(this, ViewPage.DataContextProperty))
				BindingOperations.ClearBinding(this, ViewPage.DataContextProperty);
			if (BindingOperations.IsDataBound(this, ViewPage.SourceProperty))
				BindingOperations.ClearBinding(this, ViewPage.SourceProperty);
			if (BindingOperations.IsDataBound(this, ViewPage.ViewValueProperty))
				BindingOperations.ClearBinding(this, ViewPage.ViewValueProperty);

		}

		/// <summary>
		/// Invoked when the effective property value of the ViewMode property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnViewValuePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the ViewPage and the new view mode from the property change event arguments.
			ViewPage viewPage = dependencyObject as ViewPage;
			Int32 viewValue = (Int32)dependencyPropertyChangedEventArgs.NewValue;

			// This will adjust the parameters that control the view: the view itself, the size of the icons, the width of the borders around the items, etc.  Each
			// of the individual items is bound to these values which are common for the entire view.  In that way we only need to change the value once for it
			// to propagate to every item in the view.
			if (viewValue != Int32.MinValue)
			{
				Range range = viewPage.viewValueRangeMap[viewValue];
				viewPage.listBoxView.View = range.ViewBase;
				viewPage.IconSize = range.IconSize;
				viewPage.ItemWidth = range.ItemWidth;
			}

		}

		/// <summary>
		/// Used to store the ranges of slider values used to snap the slider into place.
		/// </summary>
		struct Range
		{

			/// <summary>
			/// The view associated with the range.
			/// </summary>
			public ViewBase ViewBase;

			/// <summary>
			/// The size of the icons in Pixels in the given range.
			/// </summary>
			public Double IconSize;

			/// <summary>
			/// The width of the item in the panel.
			/// </summary>
			public Double ItemWidth;

			/// <summary>
			/// Initializes a new instance of the Range structure.
			/// </summary>
			/// <param name="viewBase">The view associated with the view value.</param>
			/// <param name="iconSize">The size of the images in the view.</param>
			/// <param name="itemWidth">The size of the border around the item.</param>
			public Range(ViewBase viewBase, Double iconSize, Double itemWidth)
			{

				// Initialize the object.
				this.ViewBase = viewBase;
				this.IconSize = iconSize;
				this.ItemWidth = itemWidth;

			}

		}

	}

}
