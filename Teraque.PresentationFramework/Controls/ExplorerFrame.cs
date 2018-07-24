namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Configuration;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.IO;
	using System.Runtime.InteropServices;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Controls.Primitives;
	using System.Windows.Data;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Navigation;
	using Teraque.Properties;
	using Teraque.Windows;
	using Teraque.Windows.Data;
	using Teraque.Windows.Controls.Primitives;
	using Teraque.Windows.Input;
	using Teraque.Windows.Navigation;

	/// <summary>
	/// A window used to navigate through generic objects in a hierarchy.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ExplorerFrame : Frame
	{

		/// <summary>
		/// Identifies the CommandTarget dependency property.
		/// </summary>
		public readonly static DependencyProperty CommandTargetProperty;

		/// <summary>
		/// Identifies the CommandTarget dependency property.
		/// </summary>
		static DependencyPropertyKey commandTargetPropertyKey = DependencyProperty.RegisterReadOnly(
			"CommandTarget",
			typeof(IInputElement),
			typeof(ExplorerFrame),
			new FrameworkPropertyMetadata());

		/// <summary>
		/// The name of the Content Presenter part.
		/// </summary>
		const String contentPresenterPartName = "PART_NavWinCP";

		/// <summary>
		/// The minimum height allowed in for the client area of the main window frame.
		/// </summary>
		const Double defaultClientMinHeight = 170.0;

		/// <summary>
		/// The minimum height allowed for the detail pane before it disappears.
		/// </summary>
		const Double defaultDetailMinHeight = 53.0;

		/// <summary>
		/// This provides a collection of Detail items that can be accessed either directly or indirectly through the DetailBarItemsSource property.
		/// </summary>
		ViewableCollection detailBarItems;

		/// <summary>
		/// Identifies the DetailBarItemsSource dependency property.key.
		/// </summary>
		public static readonly DependencyProperty DetailBarItemsSourceProperty = DependencyProperty.Register(
			"DetailBarItemsSource",
			typeof(IEnumerable),
			typeof(ExplorerFrame),
			new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ExplorerFrame.OnDetailBarItemsSourcePropertyChanged)));

		/// <summary>
		/// Occurs when a child requires a new set of items in the DetailBar.
		/// </summary>
		public static readonly RoutedEvent DetailBarChangedEvent = EventManager.RegisterRoutedEvent(
			"DetailBarChanged",
			RoutingStrategy.Bubble,
			typeof(EventHandler<ItemsSourceEventArgs>),
			typeof(ExplorerFrame));

		/// <summary>
		/// The name of the DetailBar part.
		/// </summary>
		const String detailBarPartName = "PART_DetailBar";

		/// <summary>
		/// Identifies the DetailHeight dependency property.
		/// </summary>
		public readonly static DependencyProperty DetailHeightProperty = DependencyProperty.Register(
			"DetailHeight",
			typeof(GridLength),
			typeof(ExplorerFrame),
			new FrameworkPropertyMetadata(new GridLength(ExplorerFrame.defaultDetailMinHeight), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		/// <summary>
		/// Identifies the DetailMaxHeight dependency property.
		/// </summary>
		public readonly static DependencyProperty DetailMaxHeightProperty;

		/// <summary>
		/// Identifies the DetailMaxHeight dependency property.key.
		/// </summary>
		static DependencyPropertyKey detailMaxHeightPropertyKey = DependencyProperty.RegisterReadOnly(
			"DetailMaxHeight",
			typeof(Double),
			typeof(ExplorerFrame),
			new FrameworkPropertyMetadata(Double.MaxValue));

		/// <summary>
		/// Identifies the DetailMinHeight dependency property.
		/// </summary>
		public readonly static DependencyProperty DetailMinHeightProperty;

		/// <summary>
		/// Identifies the DetailMinHeight dependency property.key.
		/// </summary>
		static DependencyPropertyKey detailMinHeightPropertyKey = DependencyProperty.RegisterReadOnly(
			"DetailMinHeight",
			typeof(Double),
			typeof(ExplorerFrame),
			new FrameworkPropertyMetadata(ExplorerFrame.defaultDetailMinHeight));

		/// <summary>
		/// The name of the ExplorerBar part.
		/// </summary>
		const String explorerBarPartName = "PART_ExplorerBar";

		/// <summary>
		/// The external processes launched by this frame.
		/// </summary>
		Dictionary<String, Process> externalProcesses = new Dictionary<String, Process>();

		/// <summary>
		/// Occurs when a child requires a new set of items in the GadgetBar.
		/// </summary>
		public static readonly RoutedEvent GadgetBarChangedEvent = EventManager.RegisterRoutedEvent(
			"GadgetBarChanged",
			RoutingStrategy.Bubble,
			typeof(EventHandler<ItemsSourceEventArgs>),
			typeof(ExplorerFrame));

		/// <summary>
		/// This provides a collection of GadgetBar items that can be accessed either directly or indirectly through the GadgetBarItemsSource property.
		/// </summary>
		ViewableCollection gadgetBarItems;

		/// <summary>
		/// Identifies the GadgetBarItemsSource dependency property.key.
		/// </summary>
		public static readonly DependencyProperty GadgetBarItemsSourceProperty = DependencyProperty.Register(
			"GadgetBarItemsSource",
			typeof(IEnumerable),
			typeof(ExplorerFrame),
			new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ExplorerFrame.OnGadgetBarItemsSourcePropertyChanged)));

		/// <summary>
		/// Identifies the IsDetailVisible dependency property.
		/// </summary>
		public readonly static DependencyProperty IsDetailVisibleProperty = DependencyProperty.Register(
			"IsDetailVisible",
			typeof(Boolean),
			typeof(ExplorerFrame),
			new FrameworkPropertyMetadata(ExplorerFrame.OnIsDetailVisiblePropertyChanged));

		/// <summary>
		/// Identifies the IsLibraryVisible dependency property.
		/// </summary>
		public readonly static DependencyProperty IsLibraryVisibleProperty = DependencyProperty.Register(
			"IsLibraryVisible",
			typeof(Boolean),
			typeof(ExplorerFrame));

		/// <summary>
		/// Identifies the IsMenuVisible dependency property.
		/// </summary>
		public readonly static DependencyProperty IsMenuVisibleProperty = DependencyProperty.Register(
			"IsMenuVisible",
			typeof(Boolean),
			typeof(ExplorerFrame));

		/// <summary>
		/// Identifies the IsNavigationVisible dependency property.
		/// </summary>
		public readonly static DependencyProperty IsNavigationVisibleProperty = DependencyProperty.Register(
			"IsNavigationVisible",
			typeof(Boolean),
			typeof(ExplorerFrame),
			new FrameworkPropertyMetadata(ExplorerFrame.OnIsNavigationVisiblePropertyChanged));

		/// <summary>
		/// Identifies the IsPreviewVisible dependency property.
		/// </summary>
		public readonly static DependencyProperty IsPreviewVisibleProperty = DependencyProperty.Register(
			"IsPreviewVisible",
			typeof(Boolean),
			typeof(ExplorerFrame),
			new FrameworkPropertyMetadata(ExplorerFrame.OnIsPreviewVisiblePropertyChanged));

		/// <summary>
		/// The name of the navigator part.
		/// </summary>
		const String navigatorPartName = "PART_Navigator";

		/// <summary>
		/// Identifies the NavigationWidth dependency property.
		/// </summary>
		public readonly static DependencyProperty NavigationWidthProperty = DependencyProperty.Register(
			"NavigationWidth",
			typeof(GridLength),
			typeof(ExplorerFrame));

		/// <summary>
		/// Identifies the OpenItem event.
		/// </summary>
		public static readonly RoutedEvent OpenItemEvent = EventManager.RegisterRoutedEvent(
			"OpenItem",
			RoutingStrategy.Bubble,
			typeof(RoutedEventHandler),
			typeof(ExplorerFrame));

		/// <summary>
		/// The name of the preview part.
		/// </summary>
		const String previewPartName = "PART_Preview";

		/// <summary>
		/// Identifies the PreviewWidth dependency property.
		/// </summary>
		public readonly static DependencyProperty PreviewWidthProperty = DependencyProperty.Register(
			"PreviewWidth",
			typeof(GridLength),
			typeof(ExplorerFrame));

		/// <summary>
		/// Identifies the SelectedItem dependency property.
		/// </summary>
		public static readonly DependencyProperty SelectedItemProperty;

		/// <summary>
		/// Identifies the SelectedItem dependency property key.
		/// </summary>
		static readonly DependencyPropertyKey SelectedItemPropertyKey = DependencyProperty.RegisterReadOnly(
			"SelectedItem",
			typeof(Object),
			typeof(ExplorerFrame),
			new FrameworkPropertyMetadata(null));

		/// <summary>
		/// Identifies the Source dependency property.
		/// </summary>
		public new static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
			"Source",
			typeof(Uri),
			typeof(ExplorerFrame),
			new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ExplorerFrame.OnSourcePropertyChanged)));

		// This table is used to map a command to change the view into a view value.
		static Dictionary<RoutedCommand, Int32> viewCommandMap = new Dictionary<RoutedCommand, Int32>(){
				{Commands.ViewContent, 0},
				{Commands.ViewDetails, 2},
				{Commands.ViewExtraLargeIcons, 79},
				{Commands.ViewLargeIcons, 51},
				{Commands.ViewSimpleList, 3},
				{Commands.ViewMediumIcons, 31},
				{Commands.ViewSmallIcons, 4},
				{Commands.ViewTiles, 1}};

		/// <summary>
		/// Identifies the ViewValue dependency property.
		/// </summary>
		public readonly static DependencyProperty ViewValueProperty = DependencyProperty.Register(
			"ViewValue",
			typeof(Int32),
			typeof(ExplorerFrame));

		/// <summary>
		/// Initialize the ExplorerFrame class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static ExplorerFrame()
		{

			// This allows the templates in resource files to be properly associated with this new class.  Without this override, the type of the base class would
			// be used as the key in any lookup involving resources dictionaries.
			ExplorerFrame.DefaultStyleKeyProperty.OverrideMetadata(typeof(ExplorerFrame), new FrameworkPropertyMetadata(typeof(ExplorerFrame)));

			// The ContentControl is a TabStop by default.  This is a frame window and, while it is focusable, it will want to give the focus immediately to its
			// content.  There's no need for the tab to stop here.
			ExplorerFrame.FocusableProperty.OverrideMetadata(typeof(ExplorerFrame), new FrameworkPropertyMetadata(true));
			ExplorerFrame.IsTabStopProperty.OverrideMetadata(typeof(ExplorerFrame), new FrameworkPropertyMetadata(false));

			// This is a complex control and will manage it's own focus scope.
			FocusManager.IsFocusScopeProperty.OverrideMetadata(typeof(ExplorerFrame), new FrameworkPropertyMetadata(true));

			// The key(s) are initialized here to avoid the forward reference problems that can creep in when you move fields around.
			ExplorerFrame.CommandTargetProperty = ExplorerFrame.commandTargetPropertyKey.DependencyProperty;
			ExplorerFrame.DetailMaxHeightProperty = ExplorerFrame.detailMaxHeightPropertyKey.DependencyProperty;
			ExplorerFrame.DetailMinHeightProperty = ExplorerFrame.detailMinHeightPropertyKey.DependencyProperty;
			ExplorerFrame.SelectedItemProperty = ExplorerFrame.SelectedItemPropertyKey.DependencyProperty;

		}

		/// <summary>
		/// Initializes a new instance of ExplorerFrame class.
		/// </summary>
		public ExplorerFrame()
		{
			// When the application exists, we will kill all the processes that were spawned from this frame.  Note that we need to make sure the application is
			// running because the license manager can call this initializer curing the compilation phase to make sure the ExplorerFrame is licensed.  Without
			// this check, the attempt to access the current application causes and exception that prevents the library from compiling.
			if (Application.Current != null)
				Application.Current.MainWindow.Closing += new CancelEventHandler(OnMainWindowClosing);

			// These command handlers will switch the current view.
			this.CommandBindings.Add(new CommandBinding(Commands.OpenItem, new ExecutedRoutedEventHandler(this.OnOpenItem)));
			this.CommandBindings.Add(new CommandBinding(Commands.ViewContent, new ExecutedRoutedEventHandler(this.OnViewCommand)));
			this.CommandBindings.Add(new CommandBinding(Commands.ViewDetails, new ExecutedRoutedEventHandler(this.OnViewCommand)));
			this.CommandBindings.Add(new CommandBinding(Commands.ViewExtraLargeIcons, new ExecutedRoutedEventHandler(this.OnViewCommand)));
			this.CommandBindings.Add(new CommandBinding(Commands.ViewLargeIcons, new ExecutedRoutedEventHandler(this.OnViewCommand)));
			this.CommandBindings.Add(new CommandBinding(Commands.ViewSimpleList, new ExecutedRoutedEventHandler(this.OnViewCommand)));
			this.CommandBindings.Add(new CommandBinding(Commands.ViewMediumIcons, new ExecutedRoutedEventHandler(this.OnViewCommand)));
			this.CommandBindings.Add(new CommandBinding(Commands.ViewSmallIcons, new ExecutedRoutedEventHandler(this.OnViewCommand)));
			this.CommandBindings.Add(new CommandBinding(Commands.ViewTiles, new ExecutedRoutedEventHandler(this.OnViewCommand)));

			// These event handlers will place a set of framework elements -- managed by a child -- in the frame.  For example, a child content page can send a 
			// list of tool bar items to be displayed in the toolbar when a special element is selected in the child page.
			this.AddHandler(ExplorerFrame.DetailBarChangedEvent, new EventHandler<ItemsSourceEventArgs>(this.OnDetailBarItemsSourceChanged));
			this.AddHandler(ExplorerFrame.GadgetBarChangedEvent, new EventHandler<ItemsSourceEventArgs>(this.OnGadgetBarItemsSourceChanged));

			// A funny thing happened on the way to the Frame class.  The InheritanceBehavior was set to SkipToAppNow.  This had the effect of stifling every 
			// attempt by child visual elements to bind to their parent.  Setting this back to the default, which is what it had down at the ContentControl class,
			// allows binding to work again.  As of this writing, I've not been able to figure out what the intent of changing the InheritanceBehevoir for the Frame
			// class is.
			base.InheritanceBehavior = InheritanceBehavior.Default;

			// This specifies that the frame will maintain its own journal.
			this.JournalOwnership = JournalOwnership.OwnsJournal;

			// This frame will automatically connect and disconnect itself to the properties of a parent ExplorerWindow in order to inherit a data context and a
			// path which reflects the currently selected item in the outer frame.  The direction goes both ways, so if an item is selected from inside this frame
			// it should be displayed in the path of the outer frame.
			this.Loaded += new RoutedEventHandler(this.OnLoaded);
			this.Unloaded += new RoutedEventHandler(this.OnUnloaded);

			// When the size of the frame changes we need to adjust the settings on the grid to make sure that everything important stays visible.
			this.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);

			// The frame needs to be reformatted when the settings are restored to the factory values.
			Properties.Settings.Default.SettingsLoaded += new SettingsLoadedEventHandler(this.OnSettingsLoaded);

			// The 'NavigationService' provides some services that are of questionable value.  One of the most offensive is the desire of the NavigationService to
			// scroll new content to the top of the viewer.  There are several problems with the assumption that the new content wants to be scrolled to the top but
			// the biggest problem has to do with the brain-damaged commanding system.  As commands tunnel into the application, they will follow the FocusScope
			// down through each level in order to find the target.  This frame control has several items that can act as input elements: the toolbar, the navigator
			// and the content presenter.  If the input focus is in the navigator, for instance, then the navigator gets the command to scroll to the top from the
			// NavigationServices.  This causes more issues than it solves, so the command is intercepted and ignored.
			this.AddHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(this.OnExecutedRoutedEvent));

			// These command bindings connect the commands up to the handlers.
			this.CommandBindings.Add(new CommandBinding(Commands.ViewDetailPane, this.OnViewDetailPane));
			this.CommandBindings.Add(new CommandBinding(Commands.ViewLibraryPane, this.OnViewLibraryPane));
			this.CommandBindings.Add(new CommandBinding(Commands.ViewNavigationPane, this.OnViewNavigationPane));
			this.CommandBindings.Add(new CommandBinding(Commands.ViewPreviewPane, this.OnViewPreviewPane));

			// Bind the visibility of the details pane to the settings.
			Binding detailHeightBinding = new Binding();
			detailHeightBinding.Mode = BindingMode.TwoWay;
			detailHeightBinding.Path = new PropertyPath("DetailHeight");
			detailHeightBinding.Source = Settings.Default;
			BindingOperations.SetBinding(this, ExplorerFrame.DetailHeightProperty, detailHeightBinding);

			// Bind the visibility of the details pane to the settings.
			Binding detailVisibilityBinding = new Binding();
			detailVisibilityBinding.Mode = BindingMode.TwoWay;
			detailVisibilityBinding.Path = new PropertyPath("IsDetailVisible");
			detailVisibilityBinding.Source = Settings.Default;
			BindingOperations.SetBinding(this, ExplorerFrame.IsDetailVisibleProperty, detailVisibilityBinding);

			// Bind the visibility of the menu library pane to the settings.
			Binding libraryVisibilityBinding = new Binding();
			libraryVisibilityBinding.Mode = BindingMode.TwoWay;
			libraryVisibilityBinding.Path = new PropertyPath("IsLibraryVisible");
			libraryVisibilityBinding.Source = Settings.Default;
			BindingOperations.SetBinding(this, ExplorerFrame.IsLibraryVisibleProperty, libraryVisibilityBinding);

			// Bind the visibility of the navigation pane to the settings.
			Binding isNavigationVisibleBinding = new Binding();
			isNavigationVisibleBinding.Mode = BindingMode.TwoWay;
			isNavigationVisibleBinding.Path = new PropertyPath("IsNavigationVisible");
			isNavigationVisibleBinding.Source = Settings.Default;
			BindingOperations.SetBinding(this, ExplorerFrame.IsNavigationVisibleProperty, isNavigationVisibleBinding);

			// Bind the visibility of the preview pane to the settings.
			Binding previewVisibilityBinding = new Binding();
			previewVisibilityBinding.Mode = BindingMode.TwoWay;
			previewVisibilityBinding.Path = new PropertyPath("IsPreviewVisible");
			previewVisibilityBinding.Source = Settings.Default;
			BindingOperations.SetBinding(this, ExplorerFrame.IsPreviewVisibleProperty, previewVisibilityBinding);

			// This collection holds the detail items until they're ready to be associated with a control.
			this.detailBarItems = new ViewableCollection();

			// This collection holds the toolbar items until they're ready to be associated with a control.
			this.gadgetBarItems = new ViewableCollection();

			// This will force the DetailHeight property to be aligned with the IsDetailVisible property.
			ExplorerFrame.OnIsDetailVisiblePropertyChanged(
				this,
				new DependencyPropertyChangedEventArgs(ExplorerFrame.IsDetailVisibleProperty, false, this.IsDetailVisible));

			// This will force the NavigationWidth property to be aligned with the IsNavigationVisible property.
			ExplorerFrame.OnIsNavigationVisiblePropertyChanged(
				this,
				new DependencyPropertyChangedEventArgs(ExplorerFrame.IsNavigationVisibleProperty, false, this.IsNavigationVisible));

			// This will force the PreviewWidth property to be aligned with the IsPreviewVisible property.
			ExplorerFrame.OnIsPreviewVisiblePropertyChanged(
				this,
				new DependencyPropertyChangedEventArgs(ExplorerFrame.IsPreviewVisibleProperty, false, this.IsPreviewVisible));

		}

		/// <summary>
		/// Gets or sets the user visibility of the main menu in the frame.  This is a dependency property.
		/// </summary>
		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)this.GetValue(ExplorerFrame.CommandTargetProperty);
			}
		}

		/// <summary>
		/// Gets the collection used to generate the content of the control.
		/// </summary>
		public ViewableCollection DetailBarItems
		{
			get
			{
				return this.detailBarItems;
			}
		}

		/// <summary>
		/// Gets or sets the visibility of the Detail pane.  This is a dependency property.
		/// </summary>
		public IEnumerable DetailBarItemsSource
		{
			get
			{
				return (IEnumerable)this.GetValue(ExplorerFrame.DetailBarItemsSourceProperty);
			}
			set
			{
				this.SetValue(ExplorerFrame.DetailBarItemsSourceProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the visibility of the Detail pane.  This is a dependency property.
		/// </summary>
		public GridLength DetailHeight
		{
			get
			{
				return (GridLength)this.GetValue(ExplorerFrame.DetailHeightProperty);
			}
			set
			{
				this.SetValue(ExplorerFrame.DetailHeightProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the the maximum height of the detail pane.
		/// </summary>
		public Double DetailMaxHeight
		{
			get
			{
				return (Double)this.GetValue(ExplorerFrame.DetailMaxHeightProperty);
			}
		}

		/// <summary>
		/// Gets or sets the the minimum height of the detail pane.
		/// </summary>
		public Double DetailMinHeight
		{
			get
			{
				return (Double)this.GetValue(ExplorerFrame.DetailMinHeightProperty);
			}
		}

		/// <summary>
		/// Gets the collection used to generate the content of the control.
		/// </summary>
		public ViewableCollection GadgetBarItems
		{
			get
			{
				return this.gadgetBarItems;
			}
		}

		/// <summary>
		/// Gets or sets the visibility of the Detail pane.  This is a dependency property.
		/// </summary>
		public IEnumerable GadgetBarItemsSource
		{
			get
			{
				return (IEnumerable)this.GetValue(ExplorerFrame.GadgetBarItemsSourceProperty);
			}
			set
			{
				this.SetValue(ExplorerFrame.GadgetBarItemsSourceProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the user visibility of the Details pane.  This is a dependency property.
		/// </summary>
		public Boolean IsDetailVisible
		{
			get
			{
				return (Boolean)this.GetValue(ExplorerFrame.IsDetailVisibleProperty);
			}
			set
			{
				this.SetValue(ExplorerFrame.IsDetailVisibleProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the visibility of the Library pane.  This is a dependency property.
		/// </summary>
		public Boolean IsLibraryVisible
		{
			get
			{
				return (Boolean)this.GetValue(ExplorerFrame.IsLibraryVisibleProperty);
			}
			set
			{
				this.SetValue(ExplorerFrame.IsLibraryVisibleProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the visibility of the Menu pane.  This is a dependency property.
		/// </summary>
		public Boolean IsMenuVisible
		{
			get
			{
				return (Boolean)this.GetValue(ExplorerFrame.IsMenuVisibleProperty);
			}
			set
			{
				this.SetValue(ExplorerFrame.IsMenuVisibleProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the visibility of the Navigation pane.  This is a dependency property.
		/// </summary>
		public Boolean IsNavigationVisible
		{
			get
			{
				return (Boolean)this.GetValue(ExplorerFrame.IsNavigationVisibleProperty);
			}
			set
			{
				this.SetValue(ExplorerFrame.IsNavigationVisibleProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the visibility of the Preview pane.  This is a dependency property.
		/// </summary>
		public Boolean IsPreviewVisible
		{
			get
			{
				return (Boolean)this.GetValue(ExplorerFrame.IsPreviewVisibleProperty);
			}
			set
			{
				this.SetValue(ExplorerFrame.IsPreviewVisibleProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the visibility of the Navigation pane.  This is a dependency property.
		/// </summary>
		public GridLength NavigationWidth
		{
			get
			{
				return (GridLength)this.GetValue(ExplorerFrame.NavigationWidthProperty);
			}
			set
			{
				this.SetValue(ExplorerFrame.NavigationWidthProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the interpreted URI of the currently selected item.
		/// </summary>
		public new Uri Source
		{
			get
			{
				return this.GetValue(ExplorerFrame.SourceProperty) as Uri;
			}
			set
			{
				this.SetValue(ExplorerFrame.SourceProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the visibility of the Preview pane.  This is a dependency property.
		/// </summary>
		public GridLength PreviewWidth
		{
			get
			{
				return (GridLength)this.GetValue(ExplorerFrame.PreviewWidthProperty);
			}
			set
			{
				this.SetValue(ExplorerFrame.PreviewWidthProperty, value);
			}
		}

		/// <summary>
		/// Gets the selected item in a TreeFrame.
		/// </summary>
		public Object SelectedItem
		{
			get
			{
				return this.GetValue(ExplorerFrame.SelectedItemProperty);
			}
		}

		/// <summary>
		/// Gets or sets the ViewValue which controls which view is displayed and the size of the icons.
		/// </summary>
		public Int32 ViewValue
		{
			get
			{
				return (Int32)this.GetValue(ExplorerFrame.ViewValueProperty);
			}
			set
			{
				this.SetValue(ExplorerFrame.ViewValueProperty, value);
			}
		}

		/// <summary>
		/// <summary>
		/// Invoked when there is a new set of items for the DetailBar.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="gadgetBarRoutedEventArgs">The routed event data.</param>
		void OnDetailBarItemsSourceChanged(Object sender, ItemsSourceEventArgs gadgetBarRoutedEventArgs)
		{

			// If items were placed manually in the DetailBar (i.e. using the Items control instead of the ItemsSource), then we need to clean out the direct list
			// before installing an indirect one.
			if (this.DetailBarItemsSource == null && this.detailBarItems.Count != 0)
				this.detailBarItems.Clear();

			// The DetailBar will now get its logical children from this indirect list.  The beauty of this architecture is that a child element buried deep in the
			// logical hierarchy can manipulate the items in the frame, adding, deleting and moving items without having to get the frame involved in keeping track
			// of those items.
			this.DetailBarItemsSource = gadgetBarRoutedEventArgs.Items;

		}

		/// Formats the frame.
		/// </summary>
		void FormatFrame()
		{

			// This will set the limits on the size of the detail bar.  If there isn't enough room on the screen, the detail bar is considered disposable.  Also, 
			// the user can elect to have it removed manually.  Note that the actual height of the detail part is preserved even when the limits are in place, so
			// that if the room becomes available and the limits change, the detail bar will attempt to expand until it can fill the area it was originally
			// allocated.
			Double maxValue = Math.Max(0.0, this.ActualHeight - ExplorerFrame.defaultClientMinHeight);
			if (!this.IsDetailVisible || maxValue <= ExplorerFrame.defaultDetailMinHeight)
			{
				this.SetValue(ExplorerFrame.detailMinHeightPropertyKey, 0.0);
				this.SetValue(ExplorerFrame.detailMaxHeightPropertyKey, 0.0);
			}
			else
			{
				this.SetValue(ExplorerFrame.detailMaxHeightPropertyKey, maxValue);
				this.SetValue(ExplorerFrame.detailMinHeightPropertyKey, ExplorerFrame.defaultDetailMinHeight);
			}

		}

		/// <summary>
		/// Sets the foreground window.
		/// </summary>
		/// <param name="hWnd">The handle of the window to be moved to the foreground.</param>
		/// <returns>The window moved to the foreground.</returns>
		[DllImportAttribute("User32.dll")]
		static extern IntPtr SetForegroundWindow(IntPtr hWnd);

		/// <summary>
		/// Called when the Content property changes.
		/// </summary>
		/// <param name="oldContent">The old value of the Content property.</param>
		/// <param name="newContent">The new value of the Content property.</param>
		protected override void OnContentChanged(Object oldContent, Object newContent)
		{

			// This will provide a value for the 'CommandTarget' property.  This property can be used by menu items and other command invokers to target the content
			// of this frame when invoking a command.  As the navigator changes from one page to another, this is the only way the commands will have of finding
			// a target inside the new page.
			this.SetValue(ExplorerFrame.commandTargetPropertyKey, newContent as IInputElement);

			// The keyboard focus needs to be managed as the pages are swapped in and out of the frame.  This will install event handlers that watch the pages as
			// the load and unload.  Note that while the content has changed by this time, it is not necessarily loaded and ready for viewing which is when we
			// want to manage the focus.  At the time this event is triggered the content element is not ready for prime time.
			FrameworkElement frameworkElement = newContent as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.Loaded += new RoutedEventHandler(this.OnContentLoaded);
				frameworkElement.Unloaded += new RoutedEventHandler(this.OnContentUnloaded);
			}

		}

		/// <summary>
		/// Occurs when the element is laid out, rendered, and ready for interaction.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnContentLoaded(Object sender, RoutedEventArgs e)
		{

			// Extract the specific argument from the generic event arguments.
			FrameworkElement frameworkElement = sender as FrameworkElement;

			// This handler is only used once and then disconnected.
			frameworkElement.Loaded -= new RoutedEventHandler(this.OnContentLoaded);

			// When this frame has the focus and the current focus scope has not been set, then the keyboard focus will be given to the newly loaded content.  The
			// focus scope is empty on application initialization and whenever content is unloaded while the content contains the keyboard focus.  This happens when
			// you click on an item in the content page in order to navigate.  At all other times we want the focus to stay where it is.  This is especially true of
			// the tree view and the breadcrumb control as it is very annoying to have the keyboard focus jump to the content window when you want to navigate to
			// the next level of some hierarchy.
			IInputElement focusedElement = sender as IInputElement; ;
			DependencyObject focusScope = FocusManager.GetFocusScope(this);
			if (this.IsKeyboardFocusWithin && FocusManager.GetFocusedElement(focusScope) == null)
				FocusManager.SetFocusedElement(this, focusedElement);

		}

		/// <summary>
		/// Occurs when the element is removed from within an element tree of loaded elements.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="routedEventArgs">The event data.</param>
		void OnContentUnloaded(Object sender, RoutedEventArgs routedEventArgs)
		{

			// Extract the specific argument from the generic event arguments.
			FrameworkElement frameworkElement = sender as FrameworkElement;

			// This handler is only used once and then disconnected.
			frameworkElement.Unloaded -= new RoutedEventHandler(OnContentUnloaded);

			// This will clear the focus scope of a focused item when it was the content control that held the keyboard focus.  This is useful when swapping pages
			// because it gives a clear indication when the next one is loaded that the content page had the focus at one time and should get it back again with the
			// new page.  This indication is also helpful in keeping the focus from changing when the tree view is used for navigation.  We don't want the focus
			// jumping into the content window when using the tree view as it is very likely the user wants to continue navigating in the tree until they decide to
			// move the focus on their own into the content page.  Oh, and this is the way Windows Explorer handles the focus change.
			DependencyObject focusScope = FocusManager.GetFocusScope(this);
			IInputElement focusedElement = FocusManager.GetFocusedElement(focusScope);
			if (focusedElement != null && !this.IsAncestorOf((DependencyObject)focusedElement))
				FocusManager.SetFocusedElement(focusScope, null);

		}

		/// <summary>
		/// Called when the DetailBarItemsSource property changes.
		/// </summary>
		/// <param name="oldValue">Old value of the DetailBarItemsSource property.</param>
		/// <param name="newValue">New value of the DetailBarItemsSource property.</param>
		protected virtual void OnDetailBarItemsSourcePropertyChanged(IEnumerable oldValue, IEnumerable newValue)
		{
		}

		/// <summary>
		/// Invoked when the effective property value of the DetailBarItemsSource property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnDetailBarItemsSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the generic event arguments.
			ExplorerFrame explorerFrame = dependencyObject as ExplorerFrame;
			IEnumerable oldValue = dependencyPropertyChangedEventArgs.OldValue as IEnumerable;
			IEnumerable newValue = dependencyPropertyChangedEventArgs.NewValue as IEnumerable;

			// This will effectively bind the given collection to the main menu of the TreeFrame.
			explorerFrame.detailBarItems.ItemsSource = newValue;

			// This will make the TreeFrame appear to work like an ItemsControl by providing a virtual method for handling a change to the data source.
			explorerFrame.OnDetailBarItemsSourcePropertyChanged(oldValue, newValue);

		}

		/// <summary>
		/// Invoked when an command is executed.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="executedRoutedEventArgs">The Executed routed event data.</param>
		void OnExecutedRoutedEvent(Object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
		{

			// This will throw away the ScrollToTopCommand that is generated by the navigation services when it navigates to a new page.  For reasons that are not
			// clear, the NavigationService thinks that it's doing us a favor by sending this out.  This will prevent the command from executing anywhere in the
			// ExplorerFrame scope.  Note that this command does not seem to inhibit the user commands from properly scrolling to the home position.
			if (executedRoutedEventArgs.Command == ScrollBar.ScrollToTopCommand)
				executedRoutedEventArgs.Handled = true;

		}

		/// <summary>
		/// Invoked when the application exits.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="executedRoutedEventArgs">The Executed routed event data.</param>
		void OnMainWindowClosing(object sender, CancelEventArgs e)
		{

			// This will remove all the processes that have been launched from this frame.
			Process[] processes = new Process[this.externalProcesses.Values.Count];
			this.externalProcesses.Values.CopyTo(processes, 0);
			foreach (Process process in processes)
					process.Kill();

		}

		/// <summary>
		/// Invoked when there is a new set of items for the GadgetBar.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="gadgetBarRoutedEventArgs">The routed event data.</param>
		void OnGadgetBarItemsSourceChanged(Object sender, ItemsSourceEventArgs gadgetBarRoutedEventArgs)
		{

			// If items were placed manually in the GadgetBar (i.e. using the Items control instead of the ItemsSource), then we need to clean out the direct list
			// before installing an indirect one.
			if (this.GadgetBarItemsSource == null && this.gadgetBarItems.Count != 0)
				this.gadgetBarItems.Clear();

			// The GadgetBar will now get its logical children from this indirect list.  The beauty of this architecture is that a child element buried deep in the
			// logical hierarchy can manipulate the items in the frame, adding, deleting and moving items without having to get the frame involved in keeping track
			// of those items.
			this.GadgetBarItemsSource = gadgetBarRoutedEventArgs.Items;

		}

		/// <summary>
		/// Called when the GadgetBarItemsSource property changes.
		/// </summary>
		/// <param name="oldValue">Old value of the GadgetBarItemsSource property.</param>
		/// <param name="newValue">New value of the GadgetBarItemsSource property.</param>
		protected virtual void OnGadgetBarItemsSourcePropertyChanged(IEnumerable oldValue, IEnumerable newValue) { }

		/// <summary>
		/// Invoked when the effective property value of the GadgetBarItemsSource property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnGadgetBarItemsSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the generic event arguments.
			ExplorerFrame explorerFrame = dependencyObject as ExplorerFrame;
			IEnumerable oldValue = dependencyPropertyChangedEventArgs.OldValue as IEnumerable;
			IEnumerable newValue = dependencyPropertyChangedEventArgs.NewValue as IEnumerable;

			// This will effectively bind the given collection to the main menu of the TreeFrame.
			explorerFrame.gadgetBarItems.ItemsSource = newValue;

			// This will make the TreeFrame appear to work like an ItemsControl by providing a virtual method for handling a change to the data source.
			explorerFrame.OnGadgetBarItemsSourcePropertyChanged(oldValue, newValue);

		}

		/// <summary>
		/// Invoked when the effective property value of the IsDetailVisible property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnIsDetailVisiblePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the specific arguments from the generic event arguments.
			ExplorerFrame explorerFrame = dependencyObject as ExplorerFrame;

			// The logic for adjusting the frame for the Details Row is the same as adjusting it for a different window size so a common routine for both will
			// adjust all the elements of the client area.
			explorerFrame.FormatFrame();

		}

		/// <summary>
		/// Invoked when the effective property value of the IsNavigationVisible property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnIsNavigationVisiblePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the specific arguments from the generic event arguments.
			ExplorerFrame explorerFrame = dependencyObject as ExplorerFrame;

			// In order to save the width of the navigation pane when it is hidden, the binding to the user interface is disconnected from the settings.  This has
			// the pleasant side effect that when the binding to the settings is reestablished, the width of the column is restored, even when the application has
			// been shut down.
			if ((Boolean)dependencyPropertyChangedEventArgs.NewValue)
			{
				Binding navigationWidthBinding = new Binding();
				navigationWidthBinding.Mode = BindingMode.TwoWay;
				navigationWidthBinding.Path = new PropertyPath("NavigationWidth");
				navigationWidthBinding.Source = Settings.Default;
				BindingOperations.SetBinding(explorerFrame, ExplorerFrame.NavigationWidthProperty, navigationWidthBinding);
			}
			else
			{
				BindingOperations.ClearBinding(explorerFrame, ExplorerFrame.NavigationWidthProperty);
				explorerFrame.NavigationWidth = new GridLength(0.0);
			}

		}

		/// <summary>
		/// Invoked when the effective property value of the IsPreviewVisible property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnIsPreviewVisiblePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the specific arguments from the generic event arguments.
			ExplorerFrame explorerFrame = dependencyObject as ExplorerFrame;

			// In order to save the width of the preview pane when it is hidden, the binding to the user interface is disconnected from the settings.  This has the
			// pleasant side effect that when the binding to the settings is reestablished, the width of the column is restored, even when the application has been
			// shut down.
			if ((Boolean)dependencyPropertyChangedEventArgs.NewValue)
			{
				Binding previewWidthBinding = new Binding();
				previewWidthBinding.Mode = BindingMode.TwoWay;
				previewWidthBinding.Path = new PropertyPath("PreviewWidth");
				previewWidthBinding.Source = Settings.Default;
				BindingOperations.SetBinding(explorerFrame, ExplorerFrame.PreviewWidthProperty, previewWidthBinding);
			}
			else
			{
				BindingOperations.ClearBinding(explorerFrame, ExplorerFrame.PreviewWidthProperty);
				explorerFrame.PreviewWidth = new GridLength(0.0);
			}

		}

		/// <summary>
		/// Occurs when the element is laid out, rendered, and ready for interaction.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnLoaded(Object sender, RoutedEventArgs e)
		{

			// Bind the visibility of the menu pane to the settings.
			Binding parentIsMenuVisibleBinding = new Binding();
			parentIsMenuVisibleBinding.Mode = BindingMode.TwoWay;
			parentIsMenuVisibleBinding.Path = new PropertyPath("IsMenuVisible");
			parentIsMenuVisibleBinding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ExplorerWindow), 1);
			BindingOperations.SetBinding(this, ExplorerFrame.IsMenuVisibleProperty, parentIsMenuVisibleBinding);

			// A frame of this type will automatically bind itself to the data context of an ExplorerWindow if it has one as a parent.
			Binding dataContextBinding = new Binding();
			dataContextBinding.Path = new PropertyPath("DataContext");
			dataContextBinding.Source = VisualTreeExtensions.FindAncestor<ExplorerWindow>(this);
			dataContextBinding.Mode = BindingMode.OneWay;
			BindingOperations.SetBinding(this, ExplorerFrame.DataContextProperty, dataContextBinding);

			// The path of this object is automatically bound to the path of the parent ExplorerWindow if such a parent is available.  Since the binding is
			// bi-directional, updating the 'Source' variable in the frame will cause this control to navigate to an object.  Conversely, navigating to an object in
			// this frame will cause the path displayed by the outer frame to show the currently selected path.
			Binding pathBinding = new Binding();
			pathBinding.Path = new PropertyPath("Source");
			pathBinding.Source = VisualTreeExtensions.FindAncestor<ExplorerWindow>(this);
			pathBinding.Mode = BindingMode.TwoWay;
			BindingOperations.SetBinding(this, ExplorerFrame.SourceProperty, pathBinding);

		}

		/// <summary>
		/// Handles opening up an item.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnOpenItem(Object sender, RoutedEventArgs routedEventArgs)
		{

			// This is a RoutedCommand handler, so we can promote the arguments.
			ExecutedRoutedEventArgs executedRoutedEventArgs = routedEventArgs as ExecutedRoutedEventArgs;

			// Extract from the command arguments the item that is to be opened.
			IExplorerItem iExplorerItem = executedRoutedEventArgs.Parameter as IExplorerItem;

			// Containers (folders) are opened directly.  Other content needs to be examined further to figure out how it is to be opened.
			if (iExplorerItem.IsContainer)
			{
				this.Source = ExplorerHelper.GenerateSource(iExplorerItem);
			}
			else
			{

				// If there is no viewer associated with the item then we'll use the shell to open it.
				if (iExplorerItem.Viewer == null)
				{

					// This will create a file name for the selected item which acts as a unique identifier for the process.  If a process is already running with
					// this name, it is made active and brought to the foreground.  If no process is yet running, we'll write the object's data to disk and then
					// start it.
					String path = Path.Combine(Path.GetTempPath(), iExplorerItem.Name);

					// This will see if we have already started the process and make it active if we have.  Otherwise, we'll ask the shell to start up a new process
					// for us.
					Process process;
					if (this.externalProcesses.TryGetValue(path, out process))
					{
						ExplorerFrame.SetForegroundWindow(process.MainWindowHandle);
					}
					else
					{

						// If the file exists (it was left over from a previous, aborted run), then remove it first.
						FileInfo fileInfo = new FileInfo(path);
						if (fileInfo.Exists)
							try
							{
								fileInfo.Delete();
							}
							catch { }

						// Write the object's data to a local storage file.
						using (FileStream fileStream = new FileStream(path, FileMode.Create))
						{
							Byte[] buffer = iExplorerItem.Data;
							fileStream.Write(buffer, 0, buffer.Length);
						}

						// Start the process using the shell (which will figure out what program is used to run an object of this type).  Note that we attach a
						// callback which will tell us when the process has ended.
						process = new Process();
						process.StartInfo.FileName = path;
						process.EnableRaisingEvents = true;
						process.Exited += this.OnProcessExited;
						process.Start();

						// This data structure keeps track of all the processes launched and is used to clean up any leftover processes when the ExplorerFrame
						// object is destroyed.
						this.externalProcesses.Add(path, process);

					}

				}
				else
				{

					// If the object has a viewer associated with it, then the viewer is called directly.
					this.Source = ExplorerHelper.GenerateSource(iExplorerItem);

				}

			}

		}

		/// <summary>
		/// Handles the completion of an external process.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="eventArgs">The event data.</param>
		void OnProcessExited(Object sender, EventArgs eventArgs)
		{

			// Extract the process that has terminated from the generic event arguments.
			Process process = sender as Process;

			// This will delete the file from temporary storage.
			String path = process.StartInfo.FileName;
			FileInfo fileInfo = new FileInfo(path);
			fileInfo.Delete();

			// This will remove the process from the internal data structure so we no longer track it.
			this.externalProcesses.Remove(path);

		}

		/// <summary>
		/// Invoked when the effective property value of the Source property changes.
		/// </summary>
		/// <param name="oldUri">The old source value.</param>
		/// <param name="newUri">The new source value.</param>
		protected virtual void OnSourceChanged(Uri oldUri, Uri newUri) { }

		/// <summary>
		/// Invoked when the effective property value of the Source property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this property.
		/// </param>
		static void OnSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the specific arguments from the generic event arguments.
			ExplorerFrame explorerFrame = dependencyObject as ExplorerFrame;
			Uri newUri = dependencyPropertyChangedEventArgs.NewValue as Uri;
			Uri oldUri = dependencyPropertyChangedEventArgs.OldValue as Uri;

			// The Source property will be momentarily disconnected from the host when the page containing this frame is unloaded.  To distinguish between a 
			// momentary interruption and the valid application of a new path, we check whether the property is bound or not.  When attached to the parent, we will
			// set the data context of this frame to the selected path but other wise it is ignored.
			if (BindingOperations.IsDataBound(explorerFrame, ExplorerFrame.SourceProperty))
			{

				// The root item is used as the starting point to find the item selected by the path.
				IExplorerItem rootItem = explorerFrame.DataContext as IExplorerItem;
				IExplorerItem selectedItem = ExplorerHelper.FindExplorerItem(rootItem, newUri);

				// This will find the newly selected path in the data context of this frame (which is bound to the parent ExplorerWindow frame) and use that item 
				// as the selected value for this frame.
				explorerFrame.SetValue(ExplorerFrame.SelectedItemPropertyKey, selectedItem);

				// If an explorer item is available from the data context that matches the new path then it will be used as the source for this frame.  This will
				// effectively navigate the inner frame to the page selected by the path.
				if (selectedItem != null)
				{
					Frame frame = explorerFrame as Frame;
					frame.Source = selectedItem.Viewer;
				}

				// This virtual method is intended to allow subclasses the ability to act on this event.
				explorerFrame.OnSourceChanged(oldUri, newUri);

			}

		}

		/// <summary>
		/// Invoked when the persistent application settings have been loaded.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event data.</param>
		void OnSettingsLoaded(Object sender, System.Configuration.SettingsLoadedEventArgs e)
		{

			// This will re-bind the DetailHeight to the settings when the settings are restored to their factor default.
			this.FormatFrame();

		}

		/// <summary>
		/// Occurs when either the ActualHeight or the ActualWidth properties change value on this element.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="sizeChangedEventArgs">The event data.</param>
		void OnSizeChanged(Object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{

			// Format the frame to adjust to the new size.
			this.FormatFrame();

		}

		/// <summary>
		/// Occurs when the element is removed from within an element tree of loaded elements.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnUnloaded(Object sender, RoutedEventArgs e)
		{

			// When the frame is no longer connected to the parent we want to disconnect the bindings to the host frame.  There is no need to update
			// this window when it's not visible or waiting to be garbage collected.
			BindingOperations.ClearBinding(this, ExplorerFrame.IsMenuVisibleProperty);
			BindingOperations.ClearBinding(this, ExplorerFrame.DataContextProperty);
			BindingOperations.ClearBinding(this, ExplorerFrame.SourceProperty);

		}

		/// <summary>
		/// Handles a command change the viewing mode.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="routedEventArgs">The routed event data.</param>
		void OnViewCommand(Object sender, RoutedEventArgs routedEventArgs)
		{

			// Extract the specific arguments from the generic event arguments.
			ExecutedRoutedEventArgs executedRoutedEventArgs = routedEventArgs as ExecutedRoutedEventArgs;

			// This will map the routed command into a value for the view.  That single value will be decoded by the directory viewer into a specific view and
			// magnification factor for that view.
			this.ViewValue = ExplorerFrame.viewCommandMap[executedRoutedEventArgs.Command as RoutedCommand];

		}

		/// <summary>
		/// Hides or displays the Detail Pane.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="executedRoutedEventArgs">The event data.</param>
		void OnViewDetailPane(Object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
		{

			// Toggle the visible state of the detail pane.
			this.IsDetailVisible = !this.IsDetailVisible;

		}

		/// <summary>
		/// Hides or displays the Library Pane.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="executedRoutedEventArgs">The event data.</param>
		void OnViewLibraryPane(Object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
		{

			// Toggle the visible state of the library pane.
			this.IsLibraryVisible = !this.IsLibraryVisible;

		}

		/// <summary>
		/// Hides or displays the Navigation Pane.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="executedRoutedEventArgs">The event data.</param>
		void OnViewNavigationPane(Object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
		{

			// Toggle the visible state of the navigation pane.
			this.IsNavigationVisible = !this.IsNavigationVisible;

		}

		/// <summary>
		/// Hides or displays the Preview Pane.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="executedRoutedEventArgs">The event data.</param>
		void OnViewPreviewPane(Object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
		{

			// Toggle the visible state of the preview pane.
			this.IsPreviewVisible = !this.IsPreviewVisible;

		}

		/// <summary>
		/// Returns the string representation of a ExplorerFrame object.
		/// </summary>
		/// <returns>A string that represents the control.</returns>
		public override String ToString()
		{

			// This will provide a little information about the data context if it's available.
			ICollection iCollection = this.DataContext as ICollection;
			String typeName = this.GetType().ToString();
			return iCollection == null ? String.Format(CultureInfo.InvariantCulture, "{0}", typeName) :
				String.Format(CultureInfo.InvariantCulture, "{0}: {1} Items", typeName, iCollection.Count);

		}

		/// <summary>
		/// Gets or sets the uniform resource identifier (URI) of the actual content currently being navigated to.
		/// </summary>
		public Uri ActualSource
		{
			get
			{
				return ((Frame)this).Source;
			}
			set
			{
				((Frame)this).Source = value;
			}
		}

	}

}
