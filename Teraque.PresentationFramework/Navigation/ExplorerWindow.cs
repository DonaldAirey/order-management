namespace Teraque.Windows.Navigation
{

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Configuration;
	using System.Diagnostics.CodeAnalysis;
	using System.Text;
	using System.Windows;
	using System.Windows.Interop;
	using System.Windows.Controls;
	using System.Windows.Controls.Primitives;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Markup;
	using System.Windows.Media;
	using System.Windows.Navigation;
	using System.Windows.Threading;
	using Teraque.Properties;
	using Teraque.Windows.Controls;
	using Teraque.Windows.Data;
	using Teraque.Windows.Input;

	/// <summary>
	/// A window frame based on the Microsoft Explorer model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[TemplatePart(Name = ExplorerWindow.breadcrumbBarPartName, Type = typeof(BreadcrumbBar))]
	[TemplatePart(Name = ExplorerWindow.browserPanelPartName, Type = typeof(Panel))]
	[TemplatePart(Name = ExplorerWindow.contentGridPartName, Type = typeof(Grid))]
	[TemplatePart(Name = ExplorerWindow.contentPresenterPartName, Type = typeof(ContentPresenter))]
	[TemplatePart(Name = ExplorerWindow.gadgetBarPartName, Type = typeof(GadgetBar))]
	[TemplatePart(Name = ExplorerWindow.searchBoxPartName, Type = typeof(SearchBox))]
	[TemplatePart(Name = ExplorerWindow.statusBarPartName, Type = typeof(StatusBar))]
	[TemplatePart(Name = ExplorerWindow.travelPanelPartName, Type = typeof(Panel))]
	[ContentProperty("Items")]
	[SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
	public class ExplorerWindow : NavigationWindow
	{

		/// <summary>
		/// Various states of the menu.
		/// </summary>
		enum MenuState { Open, ToggledOpen, ToggledClosed };

		/// <summary>
		/// The Breadcrumb Bar is used to navigate the hierarchy.
		/// </summary>
		BreadcrumbBar breadcrumbBar;

		/// <summary>
		/// Identifies the BreadcrumbMinWidth dependency property.
		/// </summary>
		public readonly static DependencyProperty BreadcrumbMinWidthProperty;

		/// <summary>
		/// Identifies the BreadcrumbMinWidth dependency property.key.
		/// </summary>
		static DependencyPropertyKey breadcrumbMinWidthPropertyKey = DependencyProperty.RegisterReadOnly(
			"BreadcrumbMinWidth",
			typeof(Double),
			typeof(ExplorerWindow),
			new FrameworkPropertyMetadata(ExplorerWindow.defaultBreadcrumbMinWidth));

		/// <summary>
		/// The name of the BreadcrumbBar part.
		/// </summary>
		const String breadcrumbBarPartName = "PART_BreadcrumbBar";

		/// <summary>
		/// The panel on the frame window where the browsing controls are displayed.
		/// </summary>
		Panel browserPanel;

		/// <summary>
		/// The name of the BrowserBar part.
		/// </summary>
		const String browserPanelPartName = "PART_BrowserPanel";

		/// <summary>
		/// Identifies the CommandTarget dependency property.
		/// </summary>
		public readonly static DependencyProperty CommandTargetProperty;

		/// <summary>
		/// The name of the ContentGrid part.
		/// </summary>
		const String contentGridPartName = "PART_ContentGrid";

		/// <summary>
		/// The main client area of this window.
		/// </summary>
		Grid contentGrid;

		/// <summary>
		/// The name of the ContentPresenter part.
		/// </summary>
		const String contentPresenterPartName = "PART_NavWinCP";

		/// <summary>
		/// The part of the frame where the navigated content is presented.
		/// </summary>
		ContentPresenter contentPresenter;

		/// <summary>
		/// Identifies the CommandTarget dependency property.
		/// </summary>
		static DependencyPropertyKey commandTargetPropertyKey = DependencyProperty.RegisterReadOnly(
			"CommandTarget",
			typeof(IInputElement),
			typeof(ExplorerWindow),
			new FrameworkPropertyMetadata());

		/// <summary>
		/// The minimum width allowed for the breadcrumb bar.
		/// </summary>
		const Double defaultBreadcrumbMinWidth = 234.0;

		/// <summary>
		/// The minimum width allowed for the search panel.
		/// </summary>
		const Double defaultSearchMinWidth = 104.0;

		/// <summary>
		/// The System.Windows.Controls.Menu on the frame window where the menu items are displayed.
		/// </summary>
		GadgetBar gadgetBar;

		/// <summary>
		/// The name of the GadgetBar part.
		/// </summary>
		const String gadgetBarPartName = "PART_GadgetBar";

		/// <summary>
		/// Identifies the HasItems dependency property.
		/// </summary>
		public static readonly DependencyProperty HasItemsProperty;

		/// <summary>
		/// Identifies the HasItems dependency property.key.
		/// </summary>
		internal static readonly DependencyPropertyKey hasItemsPropertyKey = DependencyProperty.RegisterReadOnly(
			"HasItems",
			typeof(Boolean),
			typeof(ExplorerWindow),
			new FrameworkPropertyMetadata(false));

		/// <summary>
		/// Prevents navigation to a page when setting the path.
		/// </summary>
		Boolean isNavigationInhibited;

		/// <summary>
		/// This provides a collection that can be accessed either directly through the view or indirectly through a IEnumerable collection.
		/// </summary>
		ViewableCollection items;

		/// <summary>
		/// A 'logical' state of the keyboard focus in the menu bar.  Used primarily to handle the 'Alt' key toggling.
		/// </summary>
		Boolean isMenuFocused;

		/// <summary>
		/// Identifies the IsMenuVisible dependency property.
		/// </summary>
		public readonly static DependencyProperty IsMenuVisibleProperty = DependencyProperty.Register(
			"IsMenuVisible",
			typeof(Boolean),
			typeof(ExplorerWindow),
			new FrameworkPropertyMetadata(false, ExplorerWindow.OnIsMenuVisiblePropertyChanged));

		/// <summary>
		/// Used to defer setting the path on initialization.
		/// </summary>
		Boolean isSourceDeferred;

		/// <summary>
		/// Used to prevent recursion during the operation that repairs the FocusScope.
		/// </summary>
		Boolean isRepairingFocus;

		/// <summary>
		/// Identifies the IsSearching dependency property.
		/// </summary>
		public readonly static DependencyProperty IsSearchingProperty = DependencyProperty.Register(
			"IsSearching",
			typeof(Boolean),
			typeof(ExplorerWindow),
			new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		/// <summary>
		/// Identifies the IsStatusVisible dependency property.
		/// </summary>
		public readonly static DependencyProperty IsStatusVisibleProperty = DependencyProperty.Register(
			"IsStatusVisible",
			typeof(Boolean),
			typeof(ExplorerWindow));

		/// <summary>
		/// Identifies the ItemsSource dependency property.
		/// </summary>
		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
			"ItemsSource",
			typeof(IEnumerable),
			typeof(ExplorerWindow),
			new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ExplorerWindow.OnItemsSourcePropertyChanged)));

		/// <summary>
		/// This provides a collection of Menu items that can be accessed either directly or indirectly through the MenuItemsSource property.
		/// </summary>
		ViewableCollection menuItems;

		/// <summary>
		/// Identifies the MenuItemsSource dependency property.key.
		/// </summary>
		public static readonly DependencyProperty MenuItemsSourceProperty = DependencyProperty.Register(
			"MenuItemsSource",
			typeof(IEnumerable),
			typeof(ExplorerWindow),
			new FrameworkPropertyMetadata(null, ExplorerWindow.OnMenuItemsSourcePropertyChanged));

		/// <summary>
		/// The current state of the menu.
		/// </summary>
		MenuState menuState;

		/// <summary>
		/// This table routes Win32 messages to their respective handlers.
		/// </summary>
		Dictionary<Int32, NativeMethods.MessageHandler> messageTable;

		/// <summary>
		/// Used to fix a bug in WPF when updating the system icons fonts.
		/// </summary>
		static NativeMethods.SubclassProc notificationWindowCallback;

		/// <summary>
		/// The hard-coded name of the window that handles the notification for the resource events.
		/// </summary>
		const String notificationWindowName = "SystemResourceNotifyWindow";

		/// <summary>
		/// Identifies the OriginalUri attached dependency property.
		/// </summary>
		static readonly DependencyProperty originalUriProperty = DependencyProperty.RegisterAttached(
			"OriginalUri",
			typeof(Uri),
			typeof(ExplorerWindow));

		/// <summary>
		/// Contains the input element that had the keyboard focus before the main menu acquired it.
		/// </summary>
		IInputElement preMenuFocusElement;

		/// <summary>
		/// The character used to separate elements of the path.
		/// </summary>
		const Char separatorCharacter = '/';

		/// <summary>
		/// Identifies the Source dependency property.
		/// </summary>
		public new static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
			"Source",
			typeof(Uri),
			typeof(ExplorerWindow),
			new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ExplorerWindow.OnSourcePropertyChanged)));

		/// <summary>
		/// Used by the user to enter search text and control the process of searching.
		/// </summary>
		SearchBox searchBox;

		/// <summary>
		/// The name of the SearchBox part.
		/// </summary>
		const String searchBoxPartName = "PART_SearchBox";

		/// <summary>
		/// Identifies the SearchMaxWidth dependency property.
		/// </summary>
		public readonly static DependencyProperty SearchMaxWidthProperty;

		/// <summary>
		/// Identifies the searchMaxWidthPropertyKey dependency property.
		/// </summary>
		static DependencyPropertyKey searchMaxWidthPropertyKey = DependencyProperty.RegisterReadOnly(
			"SearchMaxWidth",
			typeof(Double),
			typeof(ExplorerWindow),
			new FrameworkPropertyMetadata(Double.MaxValue));

		/// <summary>
		/// Identifies the SearchMinWidth dependency property.
		/// </summary>
		public readonly static DependencyProperty SearchMinWidthProperty;

		/// <summary>
		/// Identifies the SearchMinWidth dependency property.key.
		/// </summary>
		static DependencyPropertyKey searchMinWidthPropertyKey = DependencyProperty.RegisterReadOnly(
			"SearchMinWidth",
			typeof(Double),
			typeof(ExplorerWindow),
			new FrameworkPropertyMetadata(ExplorerWindow.defaultSearchMinWidth));

		/// <summary>
		/// Identifies the NavigationWidth dependency property.
		/// </summary>
		public readonly static DependencyProperty SearchWidthProperty = DependencyProperty.Register(
			"SearchWidth",
			typeof(GridLength),
			typeof(ExplorerWindow));

		/// <summary>
		/// The System.Windows.Controls.ContentControl on the frame window where the status is displayed.
		/// </summary>
		StatusBar statusBar;

		/// <summary>
		/// The name of the Status Bar part.
		/// </summary>
		const String statusBarPartName = "PART_StatusBar";

		/// <summary>
		/// The panel that contains the travel controls (forward, backward, menu of recently visited places).
		/// </summary>
		Panel travelPanel;

		/// <summary>
		/// The name of the TravelPanel part.
		/// </summary>
		const String travelPanelPartName = "PART_TravelPanel";

		/// <summary>
		/// Identifies the UriMapper dependency property.key.
		/// </summary>
		internal static readonly DependencyProperty UriMapperProperty = DependencyProperty.Register(
			"UriMapper",
			typeof(UriMapper),
			typeof(ExplorerWindow),
			new FrameworkPropertyMetadata(null, ExplorerWindow.OnUriMapperPropertyChanged));

		/// <summary>
		/// Initializes the ExplorerWindow class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static ExplorerWindow()
		{

			// This allows the templates in resource files to be properly associated with this new class.  Without this override, the type of the base class would
			// be used as the key in any lookup involving resources dictionaries.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ExplorerWindow), new FrameworkPropertyMetadata(typeof(ExplorerWindow)));

			// These read-only keys are assigned here to avoid any referential errors that might occur if they were declared in-line.
			ExplorerWindow.HasItemsProperty = ExplorerWindow.hasItemsPropertyKey.DependencyProperty;

			// There is a bug with WPF.  There is currently no dynamic notification when the system metrics for the Icon font family are changed.  This callback
			// will fix the problem by forcing the update notification.
			ExplorerWindow.notificationWindowCallback = new NativeMethods.SubclassProc(ExplorerWindow.NotificationWindowCallback);

			// These properties must be initialized here to prevent errors that can occur by rearranging the order of the fields.
			ExplorerWindow.CommandTargetProperty = ExplorerWindow.commandTargetPropertyKey.DependencyProperty;
			ExplorerWindow.BreadcrumbMinWidthProperty = ExplorerWindow.breadcrumbMinWidthPropertyKey.DependencyProperty;
			ExplorerWindow.SearchMaxWidthProperty = ExplorerWindow.searchMaxWidthPropertyKey.DependencyProperty;
			ExplorerWindow.SearchMinWidthProperty = ExplorerWindow.searchMinWidthPropertyKey.DependencyProperty;

			// This will trap all the Hyperlink events and force them to use the UriMapper to resolve addresses.  Without this the base class will handle the events
			// and try to navigate to the raw url which is very bad if you are using uri mapping.
			EventManager.RegisterClassHandler(
				typeof(ExplorerWindow),
				Hyperlink.RequestNavigateEvent,
				new RequestNavigateEventHandler(ExplorerWindow.OnRequestNavigate));

		}

		/// <summary>
		/// Initializes a new instance of the ExplorerWindow class.
		/// </summary>
		public ExplorerWindow()
		{
			// These handlers will paint the background when the Aero Glass is disabled.
			this.Activated += new EventHandler(this.OnActivated);
			this.Deactivated += new EventHandler(this.OnDeactivated);

			// This collection provides the data used to navigate.  In order to look and feel like a normal ItemsControl, the 'HasItems' property is updated when 
			// the items change.
			this.items = new ViewableCollection();
			this.items.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnViewableCollectionChanged);

			// This table is used to sublcass GDI+ messages when managed methods aren't good enough.
			this.messageTable = new Dictionary<int, NativeMethods.MessageHandler>();
			this.messageTable.Add(NativeMethods.DwmCompositionChanged, this.OnDwmCompositionPropertyChanged);

			// This handles the syncrhonization of all the frame components to the currently displayed content.
			this.Navigated += new NavigatedEventHandler(this.OnNavigated);
			this.Navigating += new NavigatingCancelEventHandler(this.OnNavigating);

			// This field provides a collection that holds the menu items that are displayed in the GadgetBar.  This is a holding area where the items are kept
			// until the 'OnApplyTemplate' method finds an instantiated control to host the items.
			this.menuItems = new ViewableCollection();

			// These command bindings are provided because they are common to most all Explorer window frames.  They can always be replaced by an inheriting class 
			// if they are not useful.
			this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, this.OnClose));
			this.CommandBindings.Add(new CommandBinding(Commands.ResetSettings, this.OnResetSettings));
			this.CommandBindings.Add(new CommandBinding(Commands.ViewMenuPane, this.OnViewMenuPane));
			this.CommandBindings.Add(new CommandBinding(Commands.ViewStatusPane, this.OnViewStatusPane));

			// Load the title of the application from the resources.
			this.Title = Properties.Resources.ExplorerWindowTitle;

			// Older versions of Visual Studio do not like to have the boundaries of the window messed with while it is presenting the object on the design
			// surface.  This allows the ExplorerWindow to be managed on the design surface during design time and load the properties from the settings file during
			// run time.
			if (!DesignerProperties.GetIsInDesignMode(this))
			{

				// Bind the Main Window Height property to the settings so it will persist from one instance to the next.
				Binding explorerWindowHeightBinding = new Binding();
				explorerWindowHeightBinding.Mode = BindingMode.TwoWay;
				explorerWindowHeightBinding.Path = new PropertyPath("ExplorerWindowHeight");
				explorerWindowHeightBinding.Source = Settings.Default;
				BindingOperations.SetBinding(this, ExplorerWindow.HeightProperty, explorerWindowHeightBinding);

				// Bind the Main Window Left property to the settings so it will persist from one instance to the next.
				Binding explorerWindowLeftBinding = new Binding();
				explorerWindowLeftBinding.Mode = BindingMode.TwoWay;
				explorerWindowLeftBinding.Path = new PropertyPath("ExplorerWindowLeft");
				explorerWindowLeftBinding.Source = Settings.Default;
				BindingOperations.SetBinding(this, ExplorerWindow.LeftProperty, explorerWindowLeftBinding);

				// Bind the Main Window Top property to the settings so it will persist from one instance to the next.
				Binding explorerWindowTopBinding = new Binding();
				explorerWindowTopBinding.Mode = BindingMode.TwoWay;
				explorerWindowTopBinding.Path = new PropertyPath("ExplorerWindowTop");
				explorerWindowTopBinding.Source = Settings.Default;
				BindingOperations.SetBinding(this, ExplorerWindow.TopProperty, explorerWindowTopBinding);

				// Bind the Main Window Width property to the settings so it will persist from one instance to the next.
				Binding explorerWindowWidthBinding = new Binding();
				explorerWindowWidthBinding.Mode = BindingMode.TwoWay;
				explorerWindowWidthBinding.Path = new PropertyPath("ExplorerWindowWidth");
				explorerWindowWidthBinding.Source = Settings.Default;
				BindingOperations.SetBinding(this, ExplorerWindow.WidthProperty, explorerWindowWidthBinding);

				// Bind the Main Window WindowState property to the settings so it will persist from one instance to the next.
				Binding explorerWindowWindowStateBinding = new Binding();
				explorerWindowWindowStateBinding.Mode = BindingMode.TwoWay;
				explorerWindowWindowStateBinding.Path = new PropertyPath("ExplorerWindowWindowState");
				explorerWindowWindowStateBinding.Source = Settings.Default;
				BindingOperations.SetBinding(this, ExplorerWindow.WindowStateProperty, explorerWindowWindowStateBinding);

				// Bind the IsMenuVisible property to the settings so it will persist from one instance to the next.
				Binding menuPaneVisibilityBinding = new Binding();
				menuPaneVisibilityBinding.Mode = BindingMode.TwoWay;
				menuPaneVisibilityBinding.Path = new PropertyPath("IsMenuVisible");
				menuPaneVisibilityBinding.Source = Settings.Default;
				BindingOperations.SetBinding(this, ExplorerWindow.IsMenuVisibleProperty, menuPaneVisibilityBinding);

				// Bind the IsStatusVisible property to the settings so it will persist from one instance to the next.
				Binding isStatusVisibleBinding = new Binding();
				isStatusVisibleBinding.Mode = BindingMode.TwoWay;
				isStatusVisibleBinding.Path = new PropertyPath("IsStatusVisible");
				isStatusVisibleBinding.Source = Settings.Default;
				BindingOperations.SetBinding(this, ExplorerWindow.IsStatusVisibleProperty, isStatusVisibleBinding);

				// Bind the Main Window Height property to the settings so it will persist from one instance to the next.
				Binding breadcrumbWidthBinding = new Binding();
				breadcrumbWidthBinding.Mode = BindingMode.TwoWay;
				breadcrumbWidthBinding.Path = new PropertyPath("SearchWidth");
				breadcrumbWidthBinding.Source = Settings.Default;
				BindingOperations.SetBinding(this, ExplorerWindow.SearchWidthProperty, breadcrumbWidthBinding);

			}

			// This guarantees that the Menu state that depend on the IsMenuVisible property is aligned with the bound value.  It would be nice if the Dependency
			// Properties had an 'Unset' value that could be used when initializing.  Unfortunately, with Boolean type, you only get 'true' or 'false' for a 
			// default and when the value is bound and the bound value is 'false' then there is no change in the property value.  Hence, the property change
			// notification is never sent and the need to force it here.  This seems like extra work but it is more reliable then trying to get the defaults
			// of interrelated properties to match up properly in the resources.
			ExplorerWindow.OnIsMenuVisiblePropertyChanged(
				this,
				new DependencyPropertyChangedEventArgs(ExplorerWindow.IsMenuVisibleProperty, false, this.IsMenuVisible));

		}

		/// <summary>
		/// Gets the Breadcrumb Bar used to navigate the hierarchy.
		/// </summary>
		public BreadcrumbBar BreadcrumbBar
		{
			get
			{
				if (this.breadcrumbBar == null)
					throw new NotSupportedException(ExceptionMessage.Format(ExceptionMessages.MissingPart, this.GetType(), ExplorerWindow.breadcrumbBarPartName));
				return this.breadcrumbBar;
			}
		}

		/// <summary>
		/// Gets or sets the the maximum height of the detail pane.
		/// </summary>
		public Double BreadcrumbMinWidth
		{
			get
			{
				return (Double)this.GetValue(ExplorerWindow.BreadcrumbMinWidthProperty);
			}
		}

		/// <summary>
		/// Gets or sets the user visibility of the main menu in the frame.  This is a dependency property.
		/// </summary>
		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)this.GetValue(ExplorerWindow.CommandTargetProperty);
			}
		}

		/// <summary>
		/// Gets the grid where the content of this frame window is presented.
		/// </summary>
		protected Grid ContentGrid
		{
			get
			{
				return this.contentGrid;
			}
		}

		/// <summary>
		/// Gets the grid where the content of this frame window is presented.
		/// </summary>
		protected ContentPresenter ContentPresenter
		{
			get
			{
				return this.contentPresenter;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the ItemsControl contains items.
		/// </summary>
		public Boolean HasItems
		{
			get
			{
				return (Boolean)this.GetValue(ExplorerWindow.HasItemsProperty);
			}
		}

		/// <summary>
		/// Gets an indicator of whether Composition is enabled for the current operating environment.
		/// </summary>
		static Boolean IsCompositionEnabled
		{

			get
			{

				try
				{

					// The glass effect is available only in Windows Vista or later.
					if (Environment.OSVersion.Version.Major > 5)
					{
						Boolean compositionEnabled = false;
						if (NativeMethods.DwmIsCompositionEnabled(ref compositionEnabled) == 0)
							return compositionEnabled;
					}

				}
				catch (DllNotFoundException dllNotFoundException)
				{

					// Log the error but otherwise don't bother the user about this error.
					Log.Error(EventId.GeneralAssemblyError, "{0}, {1}", dllNotFoundException.Message, dllNotFoundException.StackTrace);

				}

				// All other versions of windows will not be able to display the glass effect.
				return false;

			}

		}

		/// <summary>
		/// Gets or sets the user visibility of the main menu in the frame.  This is a dependency property.
		/// </summary>
		public Boolean IsMenuVisible
		{
			get
			{
				return (Boolean)this.GetValue(ExplorerWindow.IsMenuVisibleProperty);
			}
			set
			{
				this.SetValue(ExplorerWindow.IsMenuVisibleProperty, value);
			}
		}

		/// <summary>
		/// Gets an indication of whether the control is searching for a value or not.
		/// </summary>
		public Boolean IsSearching
		{
			get
			{
				return (Boolean)this.GetValue(ExplorerWindow.IsSearchingProperty);
			}
			set
			{
				this.SetValue(ExplorerWindow.IsSearchingProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets an indication of whether the status bar is visible or not.  This is a dependency property.
		/// </summary>
		public Boolean IsStatusVisible
		{
			get
			{
				return (Boolean)this.GetValue(ExplorerWindow.IsStatusVisibleProperty);
			}
			set
			{
				this.SetValue(ExplorerWindow.IsStatusVisibleProperty, value);
			}
		}

		/// <summary>
		/// Gets the collection used to generate the content of the control.
		/// </summary>
		public ViewableCollection Items
		{
			get
			{
				return this.items;
			}
		}

		/// <summary>
		/// Gets or sets a collection used to generate the content of the ItemsControl.
		/// </summary>
		public IEnumerable ItemsSource
		{
			get
			{
				return this.GetValue(ExplorerWindow.ItemsSourceProperty) as IEnumerable;
			}
			set
			{
				this.SetValue(ExplorerWindow.ItemsSourceProperty, value);
			}
		}

		/// <summary>
		/// Gets the collection used to generate the content of the control.
		/// </summary>
		public ViewableCollection MenuItems
		{
			get
			{
				return this.menuItems;
			}
		}

		/// <summary>
		/// Gets or sets a collection used to generate the content of the MenuItemsControl.
		/// </summary>
		public IEnumerable MenuItemsSource
		{
			get
			{
				return this.GetValue(ExplorerWindow.MenuItemsSourceProperty) as IEnumerable;
			}
			set
			{
				this.SetValue(ExplorerWindow.MenuItemsSourceProperty, value);
			}
		}

		/// <summary>
		/// Gets the control used to search for text in the application.
		/// </summary>
		public SearchBox SearchBox
		{
			get
			{
				if (this.searchBox == null)
					throw new NotSupportedException(ExceptionMessage.Format(ExceptionMessages.MissingPart, this.GetType(), ExplorerWindow.searchBoxPartName));
				return this.searchBox;
			}
		}

		/// <summary>
		/// Gets or sets the width of the search panel.
		/// </summary>
		public GridLength SearchWidth
		{
			get
			{
				return (GridLength)this.GetValue(ExplorerWindow.SearchWidthProperty);
			}
			set
			{
				this.SetValue(ExplorerWindow.SearchWidthProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the the minimum width of the search box.
		/// </summary>
		public Double SearchMaxWidth
		{
			get
			{
				return (Double)this.GetValue(ExplorerWindow.SearchMaxWidthProperty);
			}
		}

		/// <summary>
		/// Gets or sets the the maximum width of the search box.
		/// </summary>
		public Double SearchMinWidth
		{
			get
			{
				return (Double)this.GetValue(ExplorerWindow.SearchMinWidthProperty);
			}
		}

		/// <summary>
		/// Gets or sets the uniform resource identifier (URI) of the current content, or the URI of new content that is currently being navigated to.
		/// </summary>
		public new Uri Source
		{
			get
			{
				return (Uri)this.GetValue(ExplorerWindow.SourceProperty);
			}
			set
			{
				this.SetValue(ExplorerWindow.SourceProperty, value);
			}
		}

		/// <summary>
		/// Gets the System.Windows.Controls.Primitives.StatusBar on the frame window where the tool items are displayed.
		/// </summary>
		public StatusBar StatusBar
		{
			get
			{
				return this.statusBar;
			}
		}

		/// <summary>
		/// Gets or sets a collection used to generate the content of the ItemsControl.
		/// </summary>
		[Bindable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UriMapper UriMapper
		{
			get
			{
				return this.GetValue(ExplorerWindow.UriMapperProperty) as UriMapper;
			}
			set
			{
				this.SetValue(ExplorerWindow.UriMapperProperty, value);
			}
		}

		/// <summary>
		/// Coerce the value of the path.
		/// </summary>
		/// <param name="dependencyObject">The object on which the property exists.</param>
		/// <param name="value">The new value of the property, prior to any coercion attempt.</param>
		/// <returns>The coerced value (with appropriate type).</returns>
		static Object CoerceSource(DependencyObject dependencyObject, Object value)
		{

			// Extract the generic parameters.
			ExplorerWindow explorerWindow = dependencyObject as ExplorerWindow;
			String path = (value as Uri).OriginalString;

			// The XAML for an ExplorerWindow can install the source, the path or the items in the hierarchy in any order.  This causes problems for a coercion
			// function that depends on all three items.  To get around this, coersion is deferred until after the application has been initialized.
			if (explorerWindow.IsInitialized)
			{

				// The main idea behind here is to break up the path into a series of individual strings (elements) and compare them to the hierarchy attached to this
				// control.  Each time an item of the hierarchy is matched, the loop will dig down into the next level of the hierarchy based on the element that was
				// matched.  If at any level of the hierarchy there doesn't exist a corresponding element in the path, then the overall path is invalid.
				String[] pathElements = path.Split(ExplorerWindow.separatorCharacter);
				ICollection parentCollection = explorerWindow.Items as ICollection;
				for (Int32 index = 1; index < pathElements.Length; index++)
				{

					// Get the current element of the path.
					String pathElement = pathElements[index];

					// The main idea of this loop is to find an item in the current level of the hierarchy that matches the current path element.  When a match is made the
					// algorithm will recurse into  the next level of the hierarchy.
					ICollection previousCollection = parentCollection;
					foreach (Object item in parentCollection)
					{
						IExplorerItem iExplorerItem = item as IExplorerItem;
						if (iExplorerItem != null && iExplorerItem.Name == pathElement)
							parentCollection = iExplorerItem as ICollection;
					}

					// If the parent collection is not changed then we haven't found a match to the current path element extracted from the path.  No match means that
					// the given path is invalid (unless, of course, the control is still initializing.  A 'pass' is given in this condition because the UriMapper may
					// be part of the same XAML that sets up the initial path.  Once the control is initialized, if the path is still not valid, then an exception is 
					// thrown).
					if (previousCollection == parentCollection)
						throw new NotSupportedException(ExceptionMessage.Format(ExceptionMessages.InvalidPath, path));

				}

			}

			// If we reached here then the path is valid.
			return value;

		}

		/// <summary>
		/// Subclasses the WPF Notification Window to provide an event when any of the System Icon Font properties change.
		/// </summary>
		/// <param name="hwnd">A handle to a window associated with the thread specified in the EnumThreadWindows function.</param>
		/// <param name="lParam">The application-defined value given in the EnumThreadWindows function.</param>
		/// <returns>To continue enumeration, the callback function must return TRUE; to stop enumeration, it must return FALSE.</returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		static Boolean EnumerateWindows(IntPtr hwnd, IntPtr lParam)
		{

			// Indicates whether the enumeration of the top level windows should continue.  When the Notification Window is found and subclassed, the enumeration
			// can stop.
			Boolean continueEnumeration = true;

			try
			{

				// Used to capture the name of an enumerated window.
				const Int32 capacity = 128;
				StringBuilder buffer = new StringBuilder(capacity);

				// If the current window handle belongs to the WPF Notification window then it will be subclassed with a message handler that will fix the bug with
				// notification of the Icon Font family properties.  Note that we're performing a hard-coded comparison of the Windows name so we need to make sure
				// the comparison is culture neutural.  This sucks in many ways, but it's better than the bug.
				if (NativeMethods.GetWindowText(hwnd, buffer, buffer.Capacity) > 0)
					if (buffer.ToString().Equals(ExplorerWindow.notificationWindowName, StringComparison.Ordinal))
					{
						NativeMethods.SetWindowSubclass(hwnd, ExplorerWindow.notificationWindowCallback, IntPtr.Zero, IntPtr.Zero);
						continueEnumeration = false;
					}

			}
			catch { }

			// This indicates whether or not the enumeration will continue.
			return continueEnumeration;

		}

		/// <summary>
		/// Extends the area of the glass frame into the client area of the frame window.
		/// </summary>
		/// <param name="thickness">The distance into the client area that the glass effect is extended.</param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		void ExtendGlassIntoClientArea(Thickness thickness)
		{

			try
			{

				// It's possible the background may be set to the  if the Windows 'Basic' mode has been invoked.  In order to insure that the Aero Glass effect will
				// work when we switch back to the 'Glass' mode, the background needs to be transparent.
				this.Background = Brushes.Transparent;

				// Extending the glass into the client area has no managed analog so it must be done using Interop.  The main idea here is to set the background of 
				// the client area of this window so that it is transparent all the way down to the frame.  Unfortunately the managed version of this
				// 'AllowTransparency' only works when all the visual elements of the frame have been removed.  The unmanaged version here works with any
				// WindowStyle.
				HwndSource hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
				hwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;

				// The old GDI+ methods don't recognize the managed data structures so the definition of the frame area where the glass effect is to be shown needs to
				// be converted into something that can be passed.  GDI+ is not DPI aware so the coordinates need to factor in the conversion for the X and Y
				// dimensions.
				NativeMethods.Margins margins = new NativeMethods.Margins();
				Matrix matrix = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
				margins.Bottom = Convert.ToInt32(Math.Round(thickness.Bottom * matrix.M22));
				margins.Left = Convert.ToInt32(Math.Round(thickness.Left * matrix.M11));
				margins.Right = Convert.ToInt32(Math.Round(thickness.Right * matrix.M11));
				margins.Top = Convert.ToInt32(Math.Round(thickness.Top * matrix.M22));

				// Once the client area has been made transparent and the units have been converted to account for the DPI environment, the unmanaged code is called
				// to extend the glass frame into the client area.
				Int32 hResult = NativeMethods.DwmExtendFrameIntoClientArea(hwndSource.Handle, ref margins);
				if (hResult != 0)
					throw new Win32Exception(hResult);

			}
			catch
			{

				// If this method fails for any reason, use the non-Aero settings for the part of the client area that pretends to be the non-client area.
				this.Background = this.IsActive ? SystemColors.GradientActiveCaptionBrush : SystemColors.GradientInactiveCaptionBrush;

			}

		}

		/// <summary>
		/// Format the main screen chrome for the current system conditions.
		/// </summary>
		void FormatFrame()
		{

			// There is no managed way to handle a change to the composition of the frame.  When Aero glass style is enabled the glass is extended into the client
			// area of the screen.  When the glass is disabled, the client area pretends to be the non-client area by using the same color as the frame.  Note that
			// when we fall back to 'Basic' mode we activate handlers that use the system colors to paint the background.
			if (ExplorerWindow.IsCompositionEnabled && this.browserPanel != null)
				this.ExtendGlassIntoClientArea(new Thickness(0.0, this.browserPanel.ActualHeight, 0.0, 0.0));

			// This will set the limits on the size of the detail bar.  If there isn't enough room on the screen, the detail bar is considered disposable.  Also, 
			// the user can elect to have it removed manually.  Note that the actual height of the detail part is preserved even when the limits are in place, so
			// that if the room becomes available and the limits change, the detail bar will attempt to expand until it can fill the area it was originally
			// allocated.
			Double maxValue = Math.Max(0.0, this.browserPanel.ActualWidth - this.travelPanel.ActualWidth - ExplorerWindow.defaultBreadcrumbMinWidth);
			if (maxValue <= ExplorerWindow.defaultSearchMinWidth)
			{
				this.SetValue(ExplorerWindow.searchMinWidthPropertyKey, 0.0);
				this.SetValue(ExplorerWindow.searchMaxWidthPropertyKey, 0.0);
			}
			else
			{
				this.SetValue(ExplorerWindow.searchMaxWidthPropertyKey, maxValue);
				this.SetValue(ExplorerWindow.searchMinWidthPropertyKey, ExplorerWindow.defaultSearchMinWidth);
			}

		}

		/// <summary>
		/// Gets the value of the ExplorerWindow.OriginalUri attached property from the specified FrameworkElement.
		/// </summary>
		/// <param name="dependencyObject">The element from which to read the property value.</param>
		/// <returns>The value of the ExplorerWindow.OriginalUri attached property.</returns>
		static public Uri GetOriginalUri(DependencyObject dependencyObject)
		{

			// Validate the arguments.
			if (dependencyObject == null)
				throw new ArgumentNullException("dependencyObject");

			// This property is used to get the original path used to generate a page.  It is used when reconstituting a page from the journal.
			return dependencyObject.GetValue(ExplorerWindow.originalUriProperty) as Uri;

		}

		/// <summary>
		/// Navigates asynchronously to content that is specified by a uniform resource identifier (URI).
		/// </summary>
		/// <param name="uri">A Uri object initialized with the URI for the desired content.</param>
		/// <returns>true if a navigation is not canceled; otherwise, false. </returns>
		protected new Boolean Navigate(Uri uri)
		{

			// Allow the base class to perform the navigation after mapping the URI.
			return this.Navigate(uri, new ExplorerContentState(uri));

		}

		/// <summary>
		/// Navigates asynchronously to content that is specified by a uniform resource identifier (URI).
		/// </summary>
		/// <param name="uri">A Uri object initialized with the URI for the desired content.</param>
		/// <param name="extraData">An Object that contains data to be used for processing during navigation. </param>
		/// <returns>true if a navigation is not canceled; otherwise, false. </returns>
		public new Boolean Navigate(Uri uri, Object extraData)
		{

			// The Source URI needs to reflect the navigated URI.
			using (new NavigationInhibitor(this))
				this.Source = uri;

			// Allow the base class to perform the navigation after mapping the URI.
			if (this.UriMapper != null)
				uri = this.UriMapper.MapUri(uri);
			return base.Navigate(uri, extraData);

		}

		/// <summary>
		/// Subclasses the default message handler for the WPF Resource Notification Window.
		/// </summary>
		/// <param name="hwnd">The handle to the subclassed window.</param>
		/// <param name="msg">The message being passed.</param>
		/// <param name="wParam">Additional message information. The contents of this parameter depend on the value of uMsg.</param>
		/// <param name="lParam">Additional message information. The contents of this parameter depend on the value of uMsg. </param>
		/// <param name="uIdSubclass">The subclass ID.</param>
		/// <param name="refData">The reference data provided to the SetWindowSubclass function.</param>
		/// <returns>The returned value is specific to the message sent. This value should be ignored.</returns>
		static IntPtr NotificationWindowCallback(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, IntPtr refData)
		{

			// If the incoming message is changing the settings for the non-client area of the window (that includes the system font faces and sizes), then force a
			// message that indicates that the Icon family of metrics have changed.  This will fix the problem where the Icon metrics are not updated dynamically.
			if (msg == NativeMethods.SettingChange && (UInt32)wParam == NativeMethods.SetNonClientMetrics)
				NativeMethods.SendMessage(hwnd, NativeMethods.SettingChange, (IntPtr)NativeMethods.SetIconMetrics, IntPtr.Zero);

			// This will call the next subclass window handler.
			return NativeMethods.DefSubclassProc(hwnd, msg, wParam, lParam);

		}

		/// <summary>
		/// Occurs when a window becomes the foreground window.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains no event data.</param>
		void OnActivated(Object sender, EventArgs e)
		{

			// When DWM Compositioning has been turned off the application needs to provide a background for the non-client area of the frame when it is active.
			if (!ExplorerWindow.IsCompositionEnabled)
				this.Background = SystemColors.GradientActiveCaptionBrush;

		}

		/// <summary>
		/// Called when the template generation for the visual tree is created.
		/// </summary>
		public override void OnApplyTemplate()
		{

			// Install the BreadcrumbBar from the template.
			if (this.breadcrumbBar != null)
			{
				this.breadcrumbBar.Cancel -= new RoutedEventHandler(this.OnCancelSearch);
				this.breadcrumbBar.Recycle -= new RoutedEventHandler(this.OnRecycle);
			}
			this.breadcrumbBar = this.GetTemplateChild(ExplorerWindow.breadcrumbBarPartName) as BreadcrumbBar;
			if (this.breadcrumbBar != null)
			{
				this.breadcrumbBar.Recycle += new RoutedEventHandler(this.OnRecycle);
				this.breadcrumbBar.Cancel += new RoutedEventHandler(this.OnCancelSearch);
			}

			// Install the BrowserBar from the template.
			if (this.browserPanel != null)
				this.browserPanel.SizeChanged -= new SizeChangedEventHandler(this.OnBrowserBarSizePropertyChanged);
			this.browserPanel = this.GetTemplateChild(ExplorerWindow.browserPanelPartName) as Panel;
			if (this.browserPanel != null)
				this.browserPanel.SizeChanged += new SizeChangedEventHandler(this.OnBrowserBarSizePropertyChanged);

			// Where the content of this control is presented.  This is made available so that decorations can be presented over the face of the client area or
			// wipes or fades can be applied.
			this.contentGrid = this.GetTemplateChild(ExplorerWindow.contentGridPartName) as Grid;
			this.contentPresenter = this.GetTemplateChild(ExplorerWindow.contentPresenterPartName) as ContentPresenter;

			// This will hook the main menu into the frame and give it some items to use.  Note that the main menu can be toggled open and closed with the 'Alt'
			// key.  The requires that we capture the mouse so we know when the user has clicked on to something else.  Loss of the mouse focus is the signal to
			// close the main menu again when it's been toggled open.
			if (this.gadgetBar != null)
			{
				this.gadgetBar.GotMouseCapture -= new MouseEventHandler(this.OnGadgetBarGotMouseCapture);
				this.gadgetBar.LostMouseCapture -= new MouseEventHandler(this.OnGadgetBarLostMouseCapture);
				this.gadgetBar.ItemsSource = null;
			}
			this.gadgetBar = this.GetTemplateChild(ExplorerWindow.gadgetBarPartName) as GadgetBar;
			if (this.gadgetBar != null)
			{
				this.gadgetBar.GotMouseCapture += new MouseEventHandler(this.OnGadgetBarGotMouseCapture);
				this.gadgetBar.LostMouseCapture += new MouseEventHandler(this.OnGadgetBarLostMouseCapture);
				this.gadgetBar.ItemsSource = this.menuItems;
			}

			// The search box is where the user enters the text for searching and controls the process of searching.
			if (this.searchBox != null)
				this.searchBox.Search += new EventHandler<SearchRoutedEventArgs>(this.OnSearch);
			this.searchBox = this.GetTemplateChild(ExplorerWindow.searchBoxPartName) as SearchBox;
			if (this.searchBox != null)
				this.searchBox.Search += new EventHandler<SearchRoutedEventArgs>(this.OnSearch);

			// The status bar provides an area where the current status of the application can be displayed.
			this.statusBar = this.GetTemplateChild(ExplorerWindow.statusBarPartName) as StatusBar;

			// This part is used to calculate the amount of space available on the browser bar for a search box.
			this.travelPanel = this.GetTemplateChild(ExplorerWindow.travelPanelPartName) as Panel;

			// Give the base class a crack at applying the template.
			base.OnApplyTemplate();

		}

		/// <summary>
		/// Handles a change to the size of the BrowserBar.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="sizeChangedEventArgs">The event data.</param>
		void OnBrowserBarSizePropertyChanged(Object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{

			// The size of the frame needs to be recalculated when the browser bar size changes in order to keep the Aero Glass effect on the background of the 
			// travel controls.
			this.FormatFrame();

		}

		/// <summary>
		/// Invoked when the current search is canceled.
		/// </summary>
		protected virtual void OnCancelSearch() { }

		/// <summary>
		/// Invoked when the current search should be canceled.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The routed event data.</param>
		void OnCancelSearch(Object sender, RoutedEventArgs e)
		{

			// Refresh the contents of the window.
			this.OnCancelSearch();

		}

		/// <summary>
		/// Handles a request to close the main window of the application.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="executedRoutedEventArgs">The event data.</param>
		void OnClose(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
		{

			// Manually close down the main window.
			this.Close();

		}

		/// <summary>
		/// Called when the Content property changes.
		/// </summary>
		/// <param name="oldContent">A reference to the root of the old content tree.</param>
		/// <param name="newContent">A reference to the root of the new content tree.</param>
		protected override void OnContentChanged(object oldContent, object newContent)
		{

			// This will provide a value for the 'CommandTarget' property.  This property can be used by menu items and other command invokers to target the content
			// of this frame when invoking a command.  As the navigator changes from one page to another, this is the only way the commands will have of finding
			// a target inside the new page.
			this.SetValue(ExplorerWindow.commandTargetPropertyKey, newContent as IInputElement);

			// Moving the keyboard focus around a window that has pluggable content is tricky.  When the content of this frame window changes, we want to link into 
			// the new content so that the focus can be set properly when it appears and then we want to reclaim the focus when it disappears.
			FrameworkElement frameworkElement = newContent as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.Loaded += new RoutedEventHandler(this.OnContentLoaded);
				frameworkElement.Unloaded += new RoutedEventHandler(this.OnContentUnloaded);
			}

		}

		/// <summary>
		/// Handles a new content window being loaded into the ContentPresenter of this frame.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="routedEventArgs">An EventArgs that contains no event data.</param>
		void OnContentLoaded(Object sender, RoutedEventArgs routedEventArgs)
		{

			// When content is successfully loaded into the frame we want to move the focus into that window.  This will disengage the event handler so
			// they don't collect up indefinitely and give the focus to the new content which can then figure out what to do with it.
			FrameworkElement frameworkElement = sender as FrameworkElement;
			frameworkElement.Loaded -= new RoutedEventHandler(OnContentLoaded);
			DependencyObject focusScope = FocusManager.GetFocusScope(this);
			IInputElement focusedElement = FocusManager.GetFocusedElement(focusScope);
			if (focusedElement == null)
				FocusManager.SetFocusedElement(this, sender as IInputElement);

		}

		/// <summary>
		/// Handles the old content window being unloaded from the ContentPresenter of this frame.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="routedEventArgs">An EventArgs that contains no event data.</param>
		void OnContentUnloaded(object sender, RoutedEventArgs routedEventArgs)
		{

			// If the item being unloaded has the current focused item, then the focus scope needs to clear itself of the reference.  If it doesn't, then the
			// framework can have a focus scope that points to a non-visible element which screws up the keyboard navigation as well as the visual feedback.
			FrameworkElement frameworkElement = sender as FrameworkElement;
			frameworkElement.Loaded -= new RoutedEventHandler(this.OnContentUnloaded);
			DependencyObject focusScope = FocusManager.GetFocusScope(this);
			IInputElement focusedElement = FocusManager.GetFocusedElement(focusScope);
			if (focusedElement != null && !this.IsAncestorOf((DependencyObject)focusedElement))
				FocusManager.SetFocusedElement(focusScope, null);

		}

		/// <summary>
		/// Occurs when a window becomes a background window.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains no event data.</param>
		void OnDeactivated(object sender, EventArgs e)
		{

			// When DWM Compositioning has been turned off the application needs to provide a background for the non-client area of the frame when it is not active.
			if (!ExplorerWindow.IsCompositionEnabled)
				this.Background = SystemColors.GradientInactiveCaptionBrush;

		}

		/// <summary>
		/// Handles a change to the Compositioning mode of the Desktop Windows Manager.
		/// </summary>
		/// <param name="hwnd">A handle to the window.</param>
		/// <param name="msg">The window message.</param>
		/// <param name="wParam">This parameter is not used.</param>
		/// <param name="lParam">This parameter is not used.</param>
		/// <param name="handled">An indication of whether the event is handled or not.</param>
		/// <returns>The return code from handling the message.</returns>
		IntPtr OnDwmCompositionPropertyChanged(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref Boolean handled)
		{

			// Calculate the new metrics of the frame.
			this.FormatFrame();

			// When the composition is turned off we need to provide a value for the background since it's no longer transparent.  Note that we likely don't need to
			// check fo a 'Active' status because we can't change the DWM mode for windows inside of this application, but for completeness-sake (if that is a
			// word), we provide for all possibilities here.
			if (!ExplorerWindow.IsCompositionEnabled)
				this.Background = this.IsActive ? SystemColors.GradientActiveCaptionBrush : SystemColors.GradientInactiveCaptionBrush;

			// This indicates that we handled the message.
			handled = true;

			// This indicates that the message was processed.
			return IntPtr.Zero;

		}

		/// <summary>
		/// Handles the GadgetBar entering the 'Menu' mode.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnGadgetBarGotMouseCapture(Object sender, MouseEventArgs e)
		{

			// When the 'Alt' key is pressed and the main menu 'releases' the keyboard focus, the focus should be restored to the element that had it last.  The 
			// only way it can do this is to remember who had the keyboard focus before the main menu got it.
			this.preMenuFocusElement = FocusManager.GetFocusedElement(FocusManager.GetFocusScope(this));

		}

		/// <summary>
		/// Handles the GadgetBar leaving the 'Menu' mode.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnGadgetBarLostMouseCapture(Object sender, MouseEventArgs e)
		{

			// The menu is collapsed whenever the mouse capture is lost.  The exceptions are when the menu is forced open and the 'Alt' key were not used to close 
			// the menu. The logic is that if the 'Alt' key is pressed, when it is released it will collapse the menu, so doing here (when the 'Alt' key is pressed)
			// is premature.
			if (this.menuState == MenuState.ToggledOpen && !Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
			{
				this.menuState = MenuState.ToggledClosed;
				this.IsMenuVisible = false;
			}

			// When the GadgetBar leaves the menu mode the focus is restored to the element that had it before the main menu was given the focus.
			if (FocusManager.GetFocusedElement(FocusManager.GetFocusScope(this)) != null)
				if (this.gadgetBar.IsAncestorOf(FocusManager.GetFocusedElement(FocusManager.GetFocusScope(this)) as DependencyObject))
					FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this), this.preMenuFocusElement);

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

			// This will prevent the focus repair from recursing.  This is possible if the programmer has been sloppy and creates a circular reference with the
			// focus scopes.  To be perfectly honest, I don't remember the exact scenario but I remember this being a problem if I wasn't careful about setting
			// the focused element.
			if (!this.isRepairingFocus)
			{

				try
				{

					// This will prevent the repairing of the FocusScope from being called while it's in the process of repairing a FocusScope chain.
					this.isRepairingFocus = true;

					// The main idea here is to repair the chain of focus scopes from the element with the keyboard focus up to the application focus scope.  Due 
					// to a design oversight (bug), the command manager will direct traffic down the parent focus scope's chain if the parent and the child scope
					// disagree about who has the current logical focus.  Without the code below, a child scope may properly keep track of the logical focus, but
					// when the application attempts to direct a 'OnCanExecute' or a 'OnExecute' command, it will send the command into a black hole (or some random
					// user control that wasn't expecting it). The source of the problem can be traced back to the 'GetParentScopeFocusedElement' in the
					// CommandManager class.
					UIElement currentFocusElement = e.NewFocus as UIElement;
					if (currentFocusElement != null)
					{

						// The general idea will be to navigate up through the focus scopes repairing each one as we find inconsistencies.  This is the scope of the element
						// with the keyboard focus and is used to seed the search.
						DependencyObject focusScope = FocusManager.GetFocusScope(currentFocusElement);

						// This will keep moving up the hierarchy until the application window's focus scope is found.  Note that we also prevent the a focus scope from
						// setting the focused element to itself.
						while (focusScope != e.NewFocus && focusScope != null)
						{
							IInputElement focusedElement = FocusManager.GetFocusedElement(focusScope);
							if (focusedElement != currentFocusElement)
								FocusManager.SetFocusedElement(focusScope, currentFocusElement);
							DependencyObject parent = VisualTreeHelper.GetParent(focusScope);
							focusScope = parent == null ? null : FocusManager.GetFocusScope(parent);
						}
					}

				}
				finally
				{

					// The chain is repaired at this point and we can allow another scope to be fixed.
					this.isRepairingFocus = false;

				}

			}

			// This method has no default implementation. Because an intermediate class in the inheritance might implement this method, we recommend that you call
			// the base implementation in your implementation.
			base.OnGotKeyboardFocus(e);

		}

		/// <summary>
		/// Raises the Initialized event. This method is invoked whenever IsInitialized is set to true internally.
		/// </summary>
		/// <param name="e">The RoutedEventArgs that contains the event data.</param>
		protected override void OnInitialized(EventArgs e)
		{

			// If setting the path was deferred during initialization then complete the operation now.  Note that the Coersion can also be deferred, so now is the 
			// time to insure that the path is defined according to the hierarchy.
			if (this.isSourceDeferred && this.Source != ExplorerWindow.SourceProperty.DefaultMetadata.DefaultValue as Uri)
				this.Navigate(ExplorerWindow.CoerceSource(this, this.Source) as Uri);

			// The default implementation of this virtual method raises the event as described above. Overrides should call the base implementation to preserve 
			// this behavior. If you fail to call the base implementation, not only will you not raise the Initialized event as is generally expected of a 
			// FrameworkElement derived class, but you will also suppress two important style and theme style initialization operations that are implemented by 
			// this base implementation.
			base.OnInitialized(e);

		}

		/// <summary>
		/// Invoked when the effective property value of the MenuItemsSource property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnIsMenuVisiblePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// The menu can be in three states to allow for toggling it with the keyboard.  Those states are: Open, Toggled Open, Toggled Closed.  When the 
			// 'IsMenuVisible' property changes, the state of the toggling feature needs to be aligned to the new visibility of the menu.  If the menu is forced to
			// be visible, then the menu state is clearly 'Open'.  However, if the menu is forced to be closed, then the menu can toggle between being opened and
			// closed.  When it enters the 'Forced Closed' state, the initial state of the toggling will be 'Toggled Closed'.  Hitting the 'Alt' key is the only way
			// to change the state to 'Toggled Open'.  Since we know the 'Alt' key is not pressed here, the only other state it can occupy is 'Toggled Closed'.
			ExplorerWindow explorerWindow = (ExplorerWindow)dependencyObject;
			explorerWindow.menuState = (Boolean)dependencyPropertyChangedEventArgs.NewValue ? MenuState.Open : MenuState.ToggledClosed;

		}

		/// <summary>
		/// Called when the MenuItemsSource property changes.
		/// </summary>
		/// <param name="oldValue">Old value of the MenuItemsSource property.</param>
		/// <param name="newValue">New value of the MenuItemsSource property.</param>
		protected virtual void OnMenuItemsSourcePropertyChanged(IEnumerable oldValue, IEnumerable newValue) { }

		/// <summary>
		/// Invoked when the effective property value of the MenuItemsSource property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnMenuItemsSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the generic event arguments.
			ExplorerWindow explorerWindow = dependencyObject as ExplorerWindow;
			IEnumerable oldValue = dependencyPropertyChangedEventArgs.OldValue as IEnumerable;
			IEnumerable newValue = dependencyPropertyChangedEventArgs.NewValue as IEnumerable;

			// This will effectively bind the given collection to the main menu of the ExplorerWindow.
			explorerWindow.menuItems.ItemsSource = newValue;

			// This will make the ExplorerWindow appear to work like an ItemsControl by providing a virtual method for handling a change to the data source.
			explorerWindow.OnMenuItemsSourcePropertyChanged(oldValue, newValue);

		}

		/// <summary>
		/// Handles the successful navigation to a content page.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="navigationEventArgs">The event data.</param>
		void OnNavigated(Object sender, NavigationEventArgs navigationEventArgs)
		{

			// This control supports URI mapping.  Just like Namespace prefix, a friendly, shorthand URI can be expanded to a much longer, fully specified URI for
			// the purposes of navigating.  Since this mapping is not reflective (knowing the final full path does not get you back to the path used to create
			// the page), we need to store the fully qualified URI in an object that can be 'attached' to the page.  The 'ExplorerContentState' is used to remember
			// the state of the object while it's on the stack and then restore the page when it is viewed again.
			ExplorerContentState explorerContentState = navigationEventArgs.ExtraData as ExplorerContentState;
			DependencyObject content = navigationEventArgs.Content as DependencyObject;

			// At this point a new page has been created and navigated.  If the navigation was the result of a new page being created by calling the 'Navigate'
			// method, then the extra data will be populated with the data context (the path for the most part) of the new page.  If the journal was the source of
			// the navigation, then this value is sadly empty.  Trying to give a data context to a page when it comes from the journal requires some games to be
			// played as there's no clean way to distinguish what kind of journal operation was executed.
			if (explorerContentState == null)
			{

				// The presence of an attached property indicates that the item has been replayed.  For some reason, only forward and back commands are replayed 
				// which leaves us with a problem of how to handle the 'Refresh'.  The designers of this API did not provide a clean way to set the state of a 
				// refreshed window even though they create the content anew, so the only thing left to do at this point is assume that the original path is the 
				// same as the current path.  Navigating forward and backwards is a little cleaner as the state is preserved through the 'replay' operation on the 
				// ExplorerContentState object.  The only complication with going foward or backward is that a Source needs to be displayed.  Setting the Source is 
				// usually associated with navigating to the requested URI unless the navigation is explicitly inhibited.  Failure to inhibit the navigation will  
				// result in the forward stack of the journal being destroyed because it will think that the user entered the path.
				Uri originalUri = ExplorerWindow.GetOriginalUri(content);
				if (originalUri == null)
					ExplorerWindow.SetOriginalUri(content, this.Source);
				else
					using (new NavigationInhibitor(this))
						this.Source = originalUri;

			}
			else
				ExplorerWindow.SetOriginalUri(content, explorerContentState.OriginalUri);

		}

		/// <summary>
		/// Occurs when a new navigation is requested.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="navigatingCancelEventArgs">The data for the event.</param>
		void OnNavigating(Object sender, NavigatingCancelEventArgs navigatingCancelEventArgs)
		{

			// This is a method of attaching the custom information to the journal.  The journal will store this information in the forward and backward stack so
			// they can be extracted again when the page is restored.  The path is the only item needed to restore the state of a page (that may change later to 
			// include the selected items, etc.).  We do not depend on the 'KeepAlive' to restore the original instance of the page, so remember that the 'content'
			// here could be a brand new instance of the destination page and needs to be given the original path used to generate the object as part of its data 
			// context.  Also note that we are sort of looking backwards here.  As we navigate away from a page, we store this custom information in the journal so
			// we can retrieve it later when we navigate back.
			DependencyObject content = this.NavigationService.Content as DependencyObject;
			if (content != null)
				navigatingCancelEventArgs.ContentStateToSave = new ExplorerContentState(ExplorerWindow.GetOriginalUri(content));

		}

		/// <summary>
		/// Invoked when the effective property value of the UriMapper property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this property.
		/// </param>
		static void OnUriMapperPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// During initialization of the XAML, it's possible to set a path before the URI Mapper is defined.  Setting the path without a URI Mapper would
			// typically produce an exception, but because this is all done during the initialization, we don't emit exceptions instead giving the XAML
			// a chance to put everything into place.  Once the URI Mapper has been declared, we can try the path again (if one has been set).
			ExplorerWindow explorerWindow = dependencyObject as ExplorerWindow;
			if (!explorerWindow.IsInitialized)
				explorerWindow.isSourceDeferred = true;

		}

		/// <summary>
		/// Invoked when an unhandled Keyboard.PreviewKeyUp attached event reaches this element in its route.
		/// </summary>
		/// <param name="e">The KeyEventArgs that contains the event data.</param>
		protected override void OnPreviewKeyUp(KeyEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// The main idea here is to handle the 'Alt' key which brings the user's attention to the main menu.  If the menu is hidden, it shows it and moves the
			// focus to it.  If it visible, then just the focus is moved.  If the focus is alredy in the menu, then the menu returns to it's previous state (hidden
			// or inactive) and the keyboard focus returns to the control that had it before the main menu was given the focus.  The 'Alt' key is the gateway to
			// system functions, so that's where we'll find the combination.
			if (e.Key == Key.System)
			{

				// After determining that a system function was requested, see if it's either of the 'Alt' keys.  If so, we want to bring the attention of the user
				// to the main menu or restore the focus to the previous control (the 'Alt' key toggles the state of the keyboard).
				if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt)
				{

					// The focus manager contains the currently focused element and is used to set the new keyboard focus.  It is a horribly brain-damaged concept
					// but we deal with it as best we can.
					DependencyObject focusScope = FocusManager.GetFocusScope(this);

					// The state of the settings effects how the 'Alt' key functions.  If the main menu is forced to be visible because of the settings, then the
					// 'Alt' key acts to move the focus in and out of the menu.  If the main menu is forced to be invisible, then the 'Alt' key will act to toggle
					// the visibility of the main menu.
					switch (this.menuState)
					{
					case MenuState.Open:

						// If the menu is forced to be visible and the keyboard focus is already within it, then restore the keyboard focus to the control that had
						// the focus before the 'Alt' key was hit.  If the main menu is forced to be visible but doesn't have the focus, then remember where the 
						// focus was and move the attention of the user into the main menu.  Note that the actual keyboard focus is not a good indicator of whether
						// the menu has the focus or not.  Because they key combination uses the 'Key Up' trigger, it's possible that the actual keyboard focus can
						// be lost (by the loss of the mouse capture, mainly) by the time we get to processing this event.
						if (this.isMenuFocused)
						{
							FocusManager.SetFocusedElement(focusScope, this.preMenuFocusElement);
							this.isMenuFocused = false;
						}
						else
						{
							this.gadgetBar.Focus();
							this.isMenuFocused = true;
						}
						break;

					case MenuState.ToggledClosed:

						// If the settings have forced the main menu to be invisible until needed, then the 'Alt' key will toggle the visibility of the menu.  This
						// will open when it is toggled closed and give it the focus.
						this.IsMenuVisible = true;
						this.gadgetBar.Focus();
						this.menuState = MenuState.ToggledOpen;
						break;

					case MenuState.ToggledOpen:

						// If the settings have forced the main menu to be invisible until needed, then the 'Alt' key acts to toggle the visiblity of the menu.  
						// This will close the menu and restore the focus to the item that previously had it.
						FocusManager.SetFocusedElement(focusScope, this.preMenuFocusElement);
						this.IsMenuVisible = false;
						this.menuState = MenuState.ToggledClosed;
						break;

					}

					// It is critical that no other control tries to handle the 'Alt' key once we've decieded to use it in this context as this key as it has
					// implications across the application.  In addition, we are mucking around with the keyboard focus which is also at odds with the way
					// some controls will handle the 'Alt' key.
					e.Handled = true;

				}

			}

			// This method has no default implementation. Because an intermediate class in the inheritance might implement this method, we recommend that you call
			// the base implementation in your implementation.
			base.OnPreviewKeyUp(e);

		}

		/// <summary>
		/// Invoked when the contents of this window needs to be refreshed.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The routed event data.</param>
		void OnRecycle(Object sender, RoutedEventArgs e)
		{

			// Refresh the contents of the window.
			this.Refresh();

		}

		/// <summary>
		/// Resets all the user preferences to the factory defaults.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="executedRoutedEventArgs">The event data.</param>
		void OnResetSettings(Object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
		{

			// This will reset the application to the factory defaults.
			ExplorerWindow.ResetSettings();

		}

		/// <summary>
		/// Occurs when a request is made to search the hierarchy.
		/// </summary>
		/// <param name="text">The text that is to be found by a search.</param>
		protected virtual void OnSearch(String text) { }

		/// <summary>
		/// Handles a request to search the hierarchy.
		/// </summary>
		/// <param name="sender">the object that originated the event.</param>
		/// <param name="searchEventArgs">The event data.</param>
		void OnSearch(object sender, SearchRoutedEventArgs searchEventArgs)
		{

			// The virtual method will handle the higher levels of this function.
			this.OnSearch(searchEventArgs.Text);

		}

		/// <summary>
		/// Occurs when the element is laid out, rendered, and ready for interaction.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected override void OnSourceInitialized(EventArgs e)
		{

			// This will install a general purpose message handler for the low-level messages that can't be handled with the managed code.  There are still some
			// Win32 features that are not exposed to the WPF framework, for example, the DWM API.  Until then, it is necessary to roll up one's sleeves and get
			// involved at a very dirty level: the message loop.
			HwndSource hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
			hwndSource.AddHook(new HwndSourceHook(this.WndProc));

			// Because of a bug in the WPF libraries the system resources for the Icon font family are not updated dynamically.  That means that if a user were to
			// change the FontFace or FontSize used for the Icon family of fonts, they would not change in the application dynamically, though the changes would
			// have appeared the next time the application loaded.  To get around this bug, the resource notification window is subclassed here and given the logic
			// required to dynamicall update any controls that have subscribed.
			NativeMethods.EnumThreadWindows(NativeMethods.GetCurrentThreadId(), new NativeMethods.EnumThreadWndProc(ExplorerWindow.EnumerateWindows), IntPtr.Zero);

			// Calculate the new metrics of the frame.
			this.FormatFrame();

		}

		/// <summary>
		/// Invoked when the effective property value of the Source property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this property.</param>
		static void OnSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the specific arguments from the generic event arguments.
			ExplorerWindow explorerWindow = dependencyObject as ExplorerWindow;
			Uri newUri = dependencyPropertyChangedEventArgs.NewValue as Uri;

			// This will disable the navigation when the path is changed retroactively to the page navigation.  That is, when the journal, forward button, backward 
			// button change the current page, the path is set after the fact in order to reflect the location of the selected page.  Conversely, when a breadcrumb
			// control or tree view changes the path, that should be taken as an instruction to navigate to the selected page.
			if (explorerWindow.IsInitialized)
			{
				if (!explorerWindow.isNavigationInhibited)
					explorerWindow.Navigate(newUri);
			}
			else
				explorerWindow.isSourceDeferred = true;

		}

		/// <summary>
		/// Invoked when the effective property value of the MenuItemsSource property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnItemsSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the event arguments.
			ExplorerWindow explorerWindow = (ExplorerWindow)dependencyObject;
			IEnumerable newValue = (IEnumerable)dependencyPropertyChangedEventArgs.NewValue;

			// The ItemsSource can be cleared by setting the property to 'null' (and providing it's not data bound).  Otherwise this handler will set the
			// source of the internal collection to be the new specified value.
			if ((dependencyPropertyChangedEventArgs.NewValue == null) && !BindingOperations.IsDataBound(dependencyObject, ExplorerWindow.ItemsSourceProperty))
				explorerWindow.items.ClearItemsSource();
			else
				explorerWindow.items.ItemsSource = newValue;

		}

		/// <summary>
		/// Invoked when the ViewableCollection has changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="notifyCollectionChangedEventArgs"></param>
		void OnViewableCollectionChanged(Object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{

			// This dependency property will reflect whether the underlying collection contains any items or not.
			this.SetValue(ExplorerWindow.hasItemsPropertyKey, this.items.Count != 0);

		}

		/// <summary>
		/// Hides or displays the Menu Bar.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="executedRoutedEventArgs">The event data.</param>
		void OnViewMenuPane(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
		{

			// Toggle the visible state of the menu bar.
			this.IsMenuVisible = !this.IsMenuVisible;

		}

		/// <summary>
		/// Hides or displays the Status Pane.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="executedRoutedEventArgs">The event data.</param>
		void OnViewStatusPane(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
		{

			// Toggle the visible state of the status pane.
			this.IsStatusVisible = !this.IsStatusVisible;

		}

		/// <summary>
		/// Resets the settings to their default values.
		/// </summary>
		protected static void ResetSettings()
		{

			// This will restore all the user preferences to the factory defaults.  Note that there is a bug with the 'Settings.Reset' function that prevents it
			// from being effective at restoring values that are bound to other properties.  It will leave some bound values such as the Height and Top unchanged 
			// when the Width or Left property has changed.  Restoring the defaults manually here appears to avoid those problems.
			foreach (SettingsProperty settingsProperty in Properties.Settings.Default.Properties)
			{
				TypeConverter typeConverter = TypeDescriptor.GetConverter(settingsProperty.PropertyType);
				Properties.Settings.Default[settingsProperty.Name] = typeConverter.ConvertFromInvariantString(settingsProperty.DefaultValue as String);
			}

		}

		/// <summary>
		/// Sets the value of the ExplorerWindow.OriginalUri attached property to the specified FrameworkElement.
		/// </summary>
		/// <param name="dependencyObject">The element on which to set the Grid ..::.Row  attached property.</param>
		/// <param name="originalUri">The property value to set.</param>
		static public void SetOriginalUri(DependencyObject dependencyObject, Uri originalUri)
		{

			// Validate the argument.
			if (dependencyObject == null)
				throw new ArgumentNullException("dependencyObject");

			// This value can be used to reconstitute a page when it restored from the journal.
			dependencyObject.SetValue(ExplorerWindow.originalUriProperty, originalUri);

		}

		/// <summary>
		/// Handles a windows message.
		/// </summary>
		/// <param name="hwnd">The handle of the window.</param>
		/// <param name="msg">The window message.</param>
		/// <param name="wParam">The generic short parameter.</param>
		/// <param name="lParam">The generic long parameter.</param>
		/// <param name="handled">An indication that the message was handled.</param>
		/// <returns>0 indicates the message was handled.</returns>
		IntPtr WndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref Boolean handled)
		{

			// This uses a hash table to quickly find the handler for a given message and then executes the message with that handler.
			NativeMethods.MessageHandler messageHandler;
			if (this.messageTable.TryGetValue(msg, out messageHandler))
				return messageHandler(hwnd, msg, wParam, lParam, ref handled);

			// This indicates that the message wasn't handled.
			return IntPtr.Zero;

		}

		/// <summary>
		/// Insures that the navigation is properly inhibited during a discrete operation.
		/// </summary>
		class NavigationInhibitor : IDisposable
		{

			/// <summary>
			/// The owner of the 'isNavigationInhibited' flag.
			/// </summary>
			ExplorerWindow explorerWindow;

			/// <summary>
			/// Initializes a new instance of the NavigationInhibitor class.
			/// </summary>
			/// <param name="explorerWindow"></param>
			public NavigationInhibitor(ExplorerWindow explorerWindow)
			{

				// Initialize the object and prevent the journal from being updated by a change to the path.
				this.explorerWindow = explorerWindow;
				this.explorerWindow.isNavigationInhibited = true;

			}

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			public void Dispose()
			{

				// This will allow user initiated navigations to update the journal.
				this.explorerWindow.isNavigationInhibited = false;

			}

		}

		/// <summary>
		/// Handles a request to navigate to a Uri.
		/// </summary>
		/// <param name="sender">The object that sent the event.</param>
		/// <param name="requestNavigateEventArgs">The event data.</param>
		static void OnRequestNavigate(Object sender, RequestNavigateEventArgs requestNavigateEventArgs)
		{

			ExplorerWindow explorerWindow = sender as ExplorerWindow;
			explorerWindow.Navigate(requestNavigateEventArgs.Uri);
			requestNavigateEventArgs.Handled = true;

		}

	}

}
