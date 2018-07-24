namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Text;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Controls.Primitives;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Media.Animation;
	using System.Windows.Markup;
	using Teraque.Properties;
	using Teraque.Windows.Controls.Primitives;

	/// <summary>
	/// Represents a control that can be used to present a hierarchical organization of items.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling"), ContentProperty("Items")]
	[TemplatePart(Name = BreadcrumbBar.actionButtonPartName, Type = typeof(Button))]
	[TemplatePart(Name = BreadcrumbBar.autoCompletePartName, Type = typeof(HighlightSelector))]
	[TemplatePart(Name = BreadcrumbBar.gadgetBarPartName, Type = typeof(GadgetBar))]
	[TemplatePart(Name = BreadcrumbBar.glowPartName, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = BreadcrumbBar.historyPartName, Type = typeof(HighlightSelector))]
	[TemplatePart(Name = BreadcrumbBar.indicatorPartName, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = BreadcrumbBar.leafPanelPartName, Type = typeof(Panel))]
	[TemplatePart(Name = BreadcrumbBar.textBoxPartName, Type = typeof(TextBox))]
	[TemplatePart(Name = BreadcrumbBar.trackPartName, Type = typeof(FrameworkElement))]
	public class BreadcrumbBar : ItemsControl
	{

		/// <summary>
		/// The name of the Action Button part.
		/// </summary>
		const String actionButtonPartName = "PART_ActionButton";

		/// <summary>
		/// The button that provides the 'Recycle', 'Go' and 'Stop' actions (depending on the current input mode).
		/// </summary>
		Button actionButton;
	
		/// <summary>
		/// The drop down control that allows the user to select an item to automatically comlete the path.
		/// </summary>
		HighlightSelector autoCompleteSelector;

		/// <summary>
		/// The name of the AutoComplete part.
		/// </summary>
		const String autoCompletePartName = "PART_AutoComplete";

		/// <summary>
		/// The collection of items appears in the panel that allows the user to select a path from the path elements.
		/// </summary>
		ObservableCollection<FrameworkElement> breadcrumbItems;

		/// <summary>
		/// Occurs when the cancel button on the BreadcrumbBar is pressed.
		/// </summary>
		public static readonly RoutedEvent CancelEvent = EventManager.RegisterRoutedEvent(
			"Cancel",
			RoutingStrategy.Bubble,
			typeof(RoutedEventHandler),
			typeof(BreadcrumbBar));

		/// <summary>
		/// The control where the breadcrumb items are presented.
		/// </summary>
		GadgetBar gadgetBar;

		/// <summary>
		/// The name of the GadgetBar part.
		/// </summary>
		const String gadgetBarPartName = "PART_GadgetBar";

		/// <summary>
		/// This is the highlight that moves across the progress bar to indicate that something is happening.
		/// </summary>
		FrameworkElement glow;

		/// <summary>
		/// The name of the Glow part.
		/// </summary>
		const String glowPartName = "PART_Glow";

		/// <summary>
		/// The amount of time the highlight that moves across the indicator bar will wait before repeating the cycle.
		/// </summary>
		const Double highlightPause = 1.0;

		/// <summary>
		/// The speed, in pixels per second, of the highlight that moves across the progress indicator.
		/// </summary>
		const Double highlightSpeed = 900.0;

		/// <summary>
		/// The default maximum number of item in the most recently used list.
		/// </summary>
		const Int32 historyLimit = 19;

		/// <summary>
		/// The name of the persistent list that contains the most recently used paths.
		/// </summary>
		const String historyName = "BreadcrumbBar";

		/// <summary>
		/// The collection of the most recently used paths typed in by the user.
		/// </summary>
		MruCollection<HistoryItem> history;

		/// <summary>
		/// The drop down control that allows the user to select an item that was previously typed in.
		/// </summary>
		HighlightSelector historySelector;

		/// <summary>
		/// The name of the History part.
		/// </summary>
		const String historyPartName = "PART_History";

		/// <summary>
		/// This part indicates the progress that is displayed in the BreadcrumbBar.
		/// </summary>
		FrameworkElement indicator;

		/// <summary>
		/// The name of the Indicator part.
		/// </summary>
		const String indicatorPartName = "PART_Indicator";

		/// <summary>
		/// Identifies the IsAutoCompleteOpen dependency property.
		/// </summary>
		public static readonly DependencyProperty IsAutoCompleteOpenProperty = DependencyProperty.Register(
			"IsAutoCompleteOpen",
			typeof(Boolean),
			typeof(BreadcrumbBar),
			new FrameworkPropertyMetadata(
				false,
				FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
				BreadcrumbBar.OnIsAutoCompleteOpenPropertyChanged));

		/// <summary>
		/// Identifies the IsTextChanged dependency property key.
		/// </summary>
		static readonly DependencyPropertyKey isTextChangedPropertyKey = DependencyProperty.RegisterReadOnly(
			"IsTextChanged",
			typeof(Boolean),
			typeof(BreadcrumbBar),
			new FrameworkPropertyMetadata(false));

		/// <summary>
		/// Identifies the IsTextChanged dependency property.
		/// </summary>
		public readonly static DependencyProperty IsTextChangedProperty;
	
		/// <summary>
		/// Identifies the IsHistoryOpen dependency property.
		/// </summary>
		public static readonly DependencyProperty IsHistoryOpenProperty = DependencyProperty.Register(
			"IsHistoryOpen",
			typeof(Boolean),
			typeof(BreadcrumbBar),
			new FrameworkPropertyMetadata(
				false,
				FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
				new PropertyChangedCallback(BreadcrumbBar.OnIsHistoryOpenPropertyChanged)));

		/// <summary>
		/// Identifies the IsSearching dependency property.key.
		/// </summary>
		public readonly static DependencyProperty IsSearchingProperty = DependencyProperty.Register(
			"IsSearching",
			typeof(Boolean),
			typeof(BreadcrumbBar),
			new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		/// <summary>
		/// Indicates that the TextBox control is being updated from code and not from the user.  It's used to prevent re-entrancy problems.
		/// </summary>
		Boolean isTextBoxUpdating;

		/// <summary>
		/// The last known string that the user typed in the edit box.
		/// </summary>
		String lastKnownText;
	
		/// <summary>
		/// Identifies the LeafItem dependency property.
		/// </summary>
		public readonly static DependencyProperty LeafHeaderProperty;

		/// <summary>
		/// Identifies the LeafItem dependency property key.
		/// </summary>
		static readonly DependencyPropertyKey leafHeaderPropertyKey = DependencyProperty.RegisterReadOnly(
			"LeafHeader",
			typeof(String),
			typeof(BreadcrumbBar),
			new FrameworkPropertyMetadata(null));

		/// <summary>
		/// Identifies the LeafItem dependency property.
		/// </summary>
		public readonly static DependencyProperty LeafImageSourceProperty;

		/// <summary>
		/// The panel on the BreadcrumbBar where the leaf properties are displayed.
		/// </summary>
		Panel leafPanel;

		/// <summary>
		/// The name of the leaf panel part.
		/// </summary>
		const String leafPanelPartName = "PART_LeafPanel";

		/// <summary>
		/// Identifies the LeafItemSource dependency property key.
		/// </summary>
		static readonly DependencyPropertyKey leafImageSourcePropertyKey = DependencyProperty.RegisterReadOnly(
			"LeafImageSource",
			typeof(ImageSource),
			typeof(BreadcrumbBar),
			new FrameworkPropertyMetadata(null));

		/// <summary>
		/// Identifies the MaxHistoryHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty MaxHistoryHeightProperty = DependencyProperty.Register(
			"MaxHistoryHeight",
			typeof(Double),
			typeof(BreadcrumbBar),
			new FrameworkPropertyMetadata(SystemParameters.PrimaryScreenHeight / 3));

		/// <summary>
		/// The character used to separate elements of the path.
		/// </summary>
		const Char pathSeparatorCharacter = '\\';

		/// <summary>
		/// The animation applied to the 'glow' effect that passes across the progress bar.
		/// </summary>
		ThicknessAnimationUsingKeyFrames progressBarAnimation;

		/// <summary>
		/// Identifies the ProgressMaximum dependency property.
		/// </summary>
		public static readonly DependencyProperty ProgressMaximumProperty = DependencyProperty.Register(
			"ProgressMaximum",
			typeof(Double),
			typeof(BreadcrumbBar),
			new FrameworkPropertyMetadata(
				1.0,
				new PropertyChangedCallback(BreadcrumbBar.OnProgressMaximumPropertyChanged),
				new CoerceValueCallback(BreadcrumbBar.CoerceProgressMaximum)),
			new ValidateValueCallback(BreadcrumbBar.IsValidDoubleValue));

		/// <summary>
		/// Identifies the ProgressMinimum dependency property.
		/// </summary>
		public static readonly DependencyProperty ProgressMinimumProperty = DependencyProperty.Register(
			"ProgressMinimum",
			typeof(Double),
			typeof(BreadcrumbBar),
			new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(BreadcrumbBar.OnProgressMinimumPropertyChanged)),
			new ValidateValueCallback(BreadcrumbBar.IsValidDoubleValue));

		/// <summary>
		/// Occurs when the range value changes.
		/// </summary>
		public static readonly RoutedEvent ProgressValueChangedEvent = EventManager.RegisterRoutedEvent(
			"ProgressValueChanged",
			RoutingStrategy.Bubble,
			typeof(RoutedPropertyChangedEventHandler<Double>),
			typeof(BreadcrumbBar));

		/// <summary>
		/// The value of the progress bar.
		/// </summary>
		public static readonly DependencyProperty ProgressValueProperty = DependencyProperty.Register(
			"ProgressValue",
			typeof(Double),
			typeof(BreadcrumbBar),
			new FrameworkPropertyMetadata(
				0.0,
				FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
				new PropertyChangedCallback(BreadcrumbBar.OnValuePropertyChanged),
				new CoerceValueCallback(BreadcrumbBar.CoerceProgressValue)),
			new ValidateValueCallback(BreadcrumbBar.IsValidDoubleValue));

		/// <summary>
		/// Occurs when the Recycle (Refresh) button is pressed.
		/// </summary>
		public static readonly RoutedEvent RecycleEvent = EventManager.RegisterRoutedEvent(
			"Recycle",
			RoutingStrategy.Bubble,
			typeof(RoutedEventHandler),
			typeof(BreadcrumbBar));

		/// <summary>
		/// A menu item separator that divides the overflow items from the children of the root item.
		/// </summary>
		Separator rootSeparator;

		/// <summary>
		/// The character used to separate elements of the path.
		/// </summary>
		const Char separatorCharacter = '/';

		/// <summary>
		/// Identifies the Source dependency property.
		/// </summary>
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
			"Source",
			typeof(Uri),
			typeof(BreadcrumbBar),
			new FrameworkPropertyMetadata(null, BreadcrumbBar.OnSourcePropertyChanged, BreadcrumbBar.CoerceSource));

		/// <summary>
		/// The element where the user enteres and edits the text of the path.
		/// </summary>
		TextBox textBox;

		/// <summary>
		/// The name of the TextBox part.
		/// </summary>
		const String textBoxPartName = "PART_TextBox";

		/// <summary>
		/// The actual width of this control is used to scale the normalized progress indicator.
		/// </summary>
		FrameworkElement track;

		/// <summary>
		/// The name of the TextBox part.
		/// </summary>
		const String trackPartName = "PART_Track";

		/// <summary>
		/// Occurs when the Cancel button is clicked.
		/// </summary>
		public event RoutedEventHandler Cancel
		{
			add
			{
				this.AddHandler(BreadcrumbBar.CancelEvent, value);
			}
			remove
			{
				this.RemoveHandler(BreadcrumbBar.CancelEvent, value);
			}
		}

		/// <summary>
		/// Occurs when the value of the progress bar changes.
		/// </summary>
		public event RoutedPropertyChangedEventHandler<Double> ProgressValueChanged
		{
			add
			{
				this.AddHandler(BreadcrumbBar.ProgressValueChangedEvent, value);
			}
			remove
			{
				this.RemoveHandler(BreadcrumbBar.ProgressValueChangedEvent, value);
			}
		}

		/// <summary>
		/// Occurs when the Recycle (Refresh) button is clicked.
		/// </summary>
		public event RoutedEventHandler Recycle
		{
			add
			{
				this.AddHandler(BreadcrumbBar.RecycleEvent, value);
			}
			remove
			{
				this.RemoveHandler(BreadcrumbBar.RecycleEvent, value);
			}
		}

		/// <summary>
		/// Initializes the BreadcrumbBar class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static BreadcrumbBar()
		{

			// Initialize the read-only properties using their keys.
			BreadcrumbBar.IsTextChangedProperty = BreadcrumbBar.isTextChangedPropertyKey.DependencyProperty;
			BreadcrumbBar.LeafHeaderProperty = BreadcrumbBar.leafHeaderPropertyKey.DependencyProperty;
			BreadcrumbBar.LeafImageSourceProperty = BreadcrumbBar.leafImageSourcePropertyKey.DependencyProperty;

			// This creates an association with an implicit default style in the themes.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(BreadcrumbBar), new FrameworkPropertyMetadata(typeof(BreadcrumbBar)));

			// This is a complex control and will manage it's own focus scope.
			FocusManager.IsFocusScopeProperty.OverrideMetadata(typeof(BreadcrumbBar), new FrameworkPropertyMetadata(true));

			// This class of control can be focused but is not a natural tab stop.  This allows the control to delegate the focus but not actually catch it since
			// there is nothing interesting the BreadcrumbBar can do with user input since it's just a collection of other controls.
			FrameworkElement.FocusableProperty.OverrideMetadata(typeof(BreadcrumbBar), new FrameworkPropertyMetadata(true));
			Control.IsTabStopProperty.OverrideMetadata(typeof(BreadcrumbBar), new FrameworkPropertyMetadata(false));

		}

		/// <summary>
		/// Initializes a new instance of the BreadcrumbBar class.
		/// </summary>
		public BreadcrumbBar()
		{
			// This is the collection of items that's displayed in the GadgetBar that allows the user to select elements of a hierarchical path.
			this.breadcrumbItems = new ObservableCollection<FrameworkElement>();

			// This is a persistent collection of the most recently used paths.  It is used when the history drop down presents the most recently paths that were
			// typed in by the user (not to be confused with paths selected using the mouse).
			this.history = new MruCollection<HistoryItem>(BreadcrumbBar.historyName, BreadcrumbBar.historyLimit);

			// This separator is displayed in the overflow panel when one or more path elements (other than the overflow item) are visible in the BreadcrumbBar.
			this.rootSeparator = new Separator();
			GadgetBar.SetOverflowMode(this.rootSeparator, OverflowMode.Always);

			// This handler is used to detect when the user has clicked outside of the control and is used to dismiss the History and AutoComplete drop down boxes.
			this.AddHandler(Mouse.PreviewMouseDownOutsideCapturedElementEvent, new MouseButtonEventHandler(BreadcrumbBar.OnPreviewMouseDownOutsideCapturedElement));

			// This will handle a child item being selected from the BreadcrumbItem drop down menu and will add another level to the path.
			this.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(this.OnMenuItemClick));

			// This will handle a BreadcrumbItem being pressed on the BreadcrumbBar which will clear the path of any descendant elements.
			this.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.OnButtonClick));

			// This will move the keyboard focus to a new BreadcrumbItem when it appears.
			this.AddHandler(GadgetPanel.ItemsChangedEvent, new RoutedEventHandler(this.OnPanelItemsChanged));

		}

		/// <summary>
		/// Gets the AutoCompleteSelector part for this BreadcrumbBar.
		/// </summary>
		HighlightSelector AutoCompleteSelector
		{
			get
			{
				if (this.autoCompleteSelector == null)
					throw new NotSupportedException(ExceptionMessage.Format(ExceptionMessages.MissingPart, this.GetType(), BreadcrumbBar.autoCompletePartName));
				return this.autoCompleteSelector;
			}
		}

		/// <summary>
		/// Gets the GadgetBar part for this BreadcrumbBar.
		/// </summary>
		GadgetBar GadgetBar
		{
			get
			{
				if (this.gadgetBar == null)
					throw new NotSupportedException(ExceptionMessage.Format(ExceptionMessages.MissingPart, this.GetType(), BreadcrumbBar.gadgetBarPartName));
				return this.gadgetBar;
			}
		}

		/// <summary>
		/// Gets the HistorySelector part for this BreadcrumbBar.
		/// </summary>
		HighlightSelector HistorySelector
		{
			get
			{
				if (this.historySelector == null)
					throw new NotSupportedException(ExceptionMessage.Format(ExceptionMessages.MissingPart, this.GetType(), BreadcrumbBar.historyPartName));
				return this.historySelector;
			}
		}

		/// <summary> 
		/// Indicates whether or not the popup window is open.
		/// </summary>
		public Boolean IsAutoCompleteOpen
		{
			get
			{
				return (Boolean)this.GetValue(BreadcrumbBar.IsAutoCompleteOpenProperty);
			}
			set
			{
				this.SetValue(BreadcrumbBar.IsAutoCompleteOpenProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets whether the BreadcrumbBar is accepting free form text for the path.
		/// </summary>
		Boolean IsEditMode
		{

			get
			{

				// The edit mode is defined by the visibility of the TextBox used for entering free form text.
				return this.TextBox.Visibility == Visibility.Visible;

			}

			set
			{

				// Only execute the code when the property has actually changed.
				if (this.textBox != null && (this.textBox.Visibility == Visibility.Visible) != value)
				{

					// Decide whether to initialize and show the TextBox for free form, or the Gadget bar for breadcrumbs.
					if (value)
					{

						// If the TextBox is used for free-form input from the user, first seed it with the current path and select the current contents.  This
						// means that the first printing character will overwrite the current path.  Obviously the GadgetBar needs to be hidden in order to
						// expose the TextBox.  Finally, once everything has been arranged, the TextBox becomes the focus of keyboard activity.
						this.SetTextBox(this.Source);
						this.TextBox.SelectAll();
						this.TextBox.Visibility = Visibility.Visible;
						this.GadgetBar.Visibility = Visibility.Hidden;
						this.TextBox.Focus();

					}
					else
					{

						// This will have the effect of clearing the 'Go' button and replace it with the normal 'Recycle' button in most templates.
						this.SetValue(BreadcrumbBar.isTextChangedPropertyKey, false);
		
						// When in the breadcrumb mode the TextBox is hidden and the GadgetBar is the focus of the keyboard activity.  Note that in both modes a
						// pre-filter on the keyboard will attempt to intercept certain keys and interpret them befor passing them on to either the TextBox or the
						// GadgetBar.
						this.GadgetBar.Visibility = Visibility.Visible;
						this.TextBox.Visibility = Visibility.Hidden;
						this.GadgetBar.Focus();

					}

				}

			}

		}

		/// <summary> 
		/// Indicates whether or not the popup window is open.
		/// </summary>
		public Boolean IsHistoryOpen
		{
			get
			{
				return (Boolean)this.GetValue(BreadcrumbBar.IsHistoryOpenProperty);
			}
			set
			{
				this.SetValue(BreadcrumbBar.IsHistoryOpenProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets an indication of whether the control is searching for a value or not.
		/// </summary>
		public Boolean IsSearching
		{
			get
			{
				return (Boolean)this.GetValue(BreadcrumbBar.IsSearchingProperty);
			}
			set
			{
				this.SetValue(BreadcrumbBar.IsSearchingProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets an indication of whether the control is searching for a value or not.
		/// </summary>
		public Boolean IsTextChanged
		{
			get
			{
				return (Boolean)this.GetValue(BreadcrumbBar.IsTextChangedProperty);
			}
		}

		/// <summary> 
		/// Indicates whether or not the popup window is open.
		/// </summary>
		public String LeafHeader
		{
			get
			{
				return this.GetValue(BreadcrumbBar.LeafHeaderProperty) as String;
			}
		}

		/// <summary> 
		/// Indicates whether or not the popup window is open.
		/// </summary>
		public ImageSource LeafImageSource
		{
			get
			{
				return this.GetValue(BreadcrumbBar.LeafImageSourceProperty) as ImageSource;
			}
		}

		/// <summary>
		/// The maximum height of the history popup window.
		/// </summary> 
		public Double MaxHistoryHeight
		{
			get
			{
				return (Double)this.GetValue(BreadcrumbBar.MaxHistoryHeightProperty);
			}
			set
			{
				this.SetValue(BreadcrumbBar.MaxHistoryHeightProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the uniform resource identifier (URI) of the current content addressed by the BreadcrumbBar.
		/// </summary>
		public Uri Source
		{
			get
			{
				return this.GetValue(BreadcrumbBar.SourceProperty) as Uri;
			}
			set
			{
				this.SetValue(BreadcrumbBar.SourceProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the highest possible Value of the range element.
		/// </summary>
		public Double ProgressMaximum
		{
			get
			{
				return (Double)this.GetValue(BreadcrumbBar.ProgressMaximumProperty);
			}
			set
			{
				this.SetValue(BreadcrumbBar.ProgressMaximumProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the Minimum possible Value of the range element.
		/// </summary>
		public Double ProgressMinimum
		{
			get
			{
				return (Double)this.GetValue(BreadcrumbBar.ProgressMinimumProperty);
			}
			set
			{
				this.SetValue(BreadcrumbBar.ProgressMinimumProperty, value);
			}
		}

		/// <summary>
		/// Gets the TextBox where free-form version of the path can be entered.
		/// </summary>
		TextBox TextBox
		{
			get
			{

				// This insures that a text box has been created for this control at the time it is used.
				if (this.textBox == null)
					throw new NotSupportedException(ExceptionMessage.Format(ExceptionMessages.MissingPart, this.GetType(), BreadcrumbBar.textBoxPartName));
				return this.textBox;

			}
		}

		/// <summary>
		/// Gets or sets the current setting of the progress range control, which may be coerced.
		/// </summary>
		public Double ProgressValue
		{
			get
			{
				return (Double)this.GetValue(BreadcrumbBar.ProgressValueProperty);
			}
			set
			{
				this.SetValue(BreadcrumbBar.ProgressValueProperty, value);
			}
		}

		/// <summary>
		/// Accepts the path currently displayed in the TextBox.
		/// </summary>
		internal void AcceptSource()
		{

			// This will attempt to set the path.  If the path is illegal then an exception will be thrown.  It is left to a higher level to catch
			// and process the exception.
			this.Source = new Uri(this.TextBox.Text.Replace(BreadcrumbBar.pathSeparatorCharacter, BreadcrumbBar.separatorCharacter), UriKind.RelativeOrAbsolute);

			// If the path is accepted then push it onto the MRU pile.
			this.history.Push(new HistoryItem() { Source = this.Source, ImageSource = this.LeafImageSource });

		}

		/// <summary>
		/// Coerce the maximum value of the range.
		/// </summary>
		/// <param name="dependencyObject">The object on which the property exists.</param>
		/// <param name="value">The new value of the property, prior to any coercion attempt.</param>
		/// <returns>The coerced value (with appropriate type).</returns>
		static Object CoerceProgressMaximum(DependencyObject dependencyObject, Object value)
		{

			// Don't let the maximum exceed the minimum.
			BreadcrumbBar breadcrumbBar = dependencyObject as BreadcrumbBar;
			return Math.Max((Double)value, breadcrumbBar.ProgressMinimum);

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
			BreadcrumbBar breadcrumbBar = dependencyObject as BreadcrumbBar;
			Uri source = value as Uri;

			// If the source has not been set, then there is nothing to cooerce.
			if (value == null)
				return value;

			// The main idea behind here is to break up the path into a series of individual strings (elements) and compare them to the hierarchy attached to this 
			// control.  Each time an item of the hierarchy is matched, the loop will dig down into the next level of the hierarchy based on the element that was 
			// matched.  If at any level of the hierarchy there doesn't exist a corresponding element in the path, then the overall path is invalid.  Note that we 
			// start matching the path on the first item (instead of the zeroth) in the loop below.  This is because the BreadbrumbBar Items do not include the 
			// empty root item.  As it's implied (it would actuall correspond to the 'Items' property if we tried to match the path up against the object 
			// hierarchy), we need to skip over it by seeding the loop with 1.
			String[] sourceElements = source.OriginalString.Split(BreadcrumbBar.separatorCharacter);
			ICollection parentCollection = breadcrumbBar.Items as ICollection;
			for (Int32 index = 1; index < sourceElements.Length; index++)
			{

				// Get the current element of the path.
				String pathElement = sourceElements[index];

				// The main idea of this loop is to find an item in the current level of the hierarchy that matches the current path element.  When a match is made the
				// algorithm will recurse into  the next level of the hierarchy.
				ICollection previousCollection = parentCollection;
				foreach (Object item in parentCollection)
				{
					IExplorerItem iExplorerItem = item as IExplorerItem;
					if (iExplorerItem != null && iExplorerItem.Name == pathElement)
						parentCollection = iExplorerItem as ICollection;
				}

			}

			// If we reached here then the path is valid.
			return value;

		}

		/// <summary>
		/// Coerce the value of the progress indicator.
		/// </summary>
		/// <param name="dependencyObject">The object on which the property exists.</param>
		/// <param name="value">The new value of the property, prior to any coercion attempt.</param>
		/// <returns>The coerced value (with appropriate type).</returns>
		static Object CoerceProgressValue(DependencyObject dependencyObject, Object value)
		{

			// This simply makes sure the value falls between the mininum and maximum values.
			BreadcrumbBar breadcrumbBar = dependencyObject as BreadcrumbBar;
			Double minimum = breadcrumbBar.ProgressMinimum;
			Double maximum = breadcrumbBar.ProgressMaximum;
			Double number = (Double)value;
			return number < minimum ? minimum : number > maximum ? maximum : value;

		}

		/// <summary>
		/// Creates a BreadcrumbItem and populates it.
		/// </summary>
		/// <param name="item">The data context of the new BreadcrumbItem.</param>
		/// <param name="overflowMode">Specifies how GadgetBar items are placed in the main toolbar panel and in the overflow panel.</param>
		/// <returns>A populated BreadcrumbItem that can be placed in the BreadcrumbBar for navigating a hierachical data structure.</returns>
		static BreadcrumbItem CreateBreadcrumb(Object item, OverflowMode overflowMode)
		{

			// Create the BreadcrumbItem and give it a data context.
			BreadcrumbItem breadcrumbItem = new BreadcrumbItem();
			breadcrumbItem.DataContext = item;

			// A new BreadcrumbItem can either have an explicit interface that provides it with separate data for the header and icon or it can use a generic data
			// object that uses the DataTemplates to define the look of the item.
			IExplorerItem iExplorerItem = item as IExplorerItem;
			if (iExplorerItem == null)
				throw new NotSupportedException(ExceptionMessage.Format(ExceptionMessages.MustBeOfType, typeof(IExplorerItem)));
			breadcrumbItem.Header = iExplorerItem.Name;
			breadcrumbItem.Icon = new Image() { Source = iExplorerItem.SmallImageSource };

			// Populate the breadcrumb item with its children.  These child items allow the user to navigate to the next level of the hierarchy.
			// This will populate the new BreadcrumbItem with its children.
			ICollection childCollection = breadcrumbItem.DataContext as ICollection;
			if (childCollection != null)
				foreach (Object childItem in childCollection)
				{

					// This MenuItem is used to navigate to the next level of the hierarchy.
					BreadcrumbItem childMenuItem = new BreadcrumbItem();
					childMenuItem.DataContext = childItem;

					// As with the parent BreadcrumbItem, the child also can either have an explicit interface to provide the content or can rely on a template to
					// provide the visual appearance of the item.
					IExplorerItem iChildExplorerItem = childItem as IExplorerItem;
					if (iChildExplorerItem == null)
						throw new NotSupportedException(ExceptionMessage.Format(ExceptionMessages.MustBeOfType, typeof(IExplorerItem)));
					childMenuItem.Header = iChildExplorerItem.Name;
					childMenuItem.Icon = new Image() { Source = iChildExplorerItem.SmallImageSource };

					// This menu item will be added to the parent's drop down menu and allow the user to navigate to the next level of the hieararchy.
					breadcrumbItem.Items.Add(childMenuItem);

				}

			// Some items always appear in the overflow panel and some are only moved there when there's not enough space.  This makes the creation of the
			// BreadcrumbItems a touch more streamlined because creating and setting the overflow mode can be done on a single line.
			GadgetBar.SetOverflowMode(breadcrumbItem, overflowMode);

			// This BreadcrumbItem is now ready to take its place in the BreadcrumbBar and helps the user navigate through a hierarchical data structure.  The 
			// button part of the BreadcrumbItem will reset the path to that element and the drop down menu will navigate the user to the next part of the 
			// hierarchy.
			return breadcrumbItem;

		}

		/// <summary>
		/// Handles a key event for the BreadcrumbBar.
		/// </summary>
		/// <param name="e">Information about the event.</param>
		void HandleKey(KeyEventArgs e)
		{

			// Handle the key.
			switch (e.Key)
			{

			case Key.Down:

				// If the History Drop Down is open then this key translates into a command to move the highlight and the selection up by one line.
				if (this.IsHistoryOpen)
				{
					this.historySelector.MoveByLine(FocusNavigationDirection.Down);
					e.Handled = true;
				}

				// If the AutoComplete Drop Down is open then this key translates into a command to move the highlight and the selection up by one line.
				if (this.IsAutoCompleteOpen)
				{
					this.AutoCompleteSelector.MoveByLine(FocusNavigationDirection.Down);
					e.Handled = true;
				}

				break;

			case Key.Enter:

				// This key will attempt to change the Source of the BreadcrumbBar to the one provided by the user.
				if (this.IsEditMode)
				{
					this.AcceptSource();
					this.IsEditMode = false;
					this.IsAutoCompleteOpen = false;
					this.IsHistoryOpen = false;
					e.Handled = true;
				}

				break;

			case Key.Escape:

				// This key will close the auto complete and history drop downs if they're open and restore the BreadcrumbBar to editing the text.  If none of the
				// drop down controls are visible, then it will restore the control to breadcrumb mode.
				if (this.IsAutoCompleteOpen || this.IsHistoryOpen)
				{
					this.IsAutoCompleteOpen = false;
					this.IsHistoryOpen = false;
				}
				else
					this.IsEditMode = false;
				e.Handled = true;
				break;

			case Key.F4:

				// This key will toggle the history drop down.  Note that the AutoComplete is never open at the same time as the History.
				this.IsAutoCompleteOpen = false;
				this.IsHistoryOpen = !this.IsHistoryOpen;
				e.Handled = true;
				break;

			case Key.System:

				// Handle the system key.
				switch (e.SystemKey)
				{
				case Key.Down:
				case Key.Up:

					// The Up and Down Arrows, when the 'Alt' key is pressed, serve to toggle the visiblity of the history drop down.
					if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
						this.IsHistoryOpen = !this.IsHistoryOpen;
					e.Handled = true;
					break;

				}

				break;

			case Key.Tab:

				// The Tab key will cycle through the currently available AutoComplete selections.
				if (this.IsAutoCompleteOpen)
				{
					Boolean isShift = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
					this.AutoCompleteSelector.MoveByLine(isShift ? FocusNavigationDirection.Up : FocusNavigationDirection.Down);
					e.Handled = true;
				}

				break;

			case Key.Up:

				// If the History Drop Down is open then this key translates into a command to move the highlight and the selection down by one line.
				if (this.IsHistoryOpen)
				{
					this.historySelector.MoveByLine(FocusNavigationDirection.Up);
					e.Handled = true;
				}

				// If the AutoComplete Drop Down is open then this key translates into a command to move the highlight and the selection down by one line.
				if (this.IsAutoCompleteOpen)
				{
					this.AutoCompleteSelector.MoveByLine(FocusNavigationDirection.Up);
					e.Handled = true;
				}

				break;

			}

		}

		/// <summary>
		/// Validates the effective value of the progress indicator.
		/// </summary>
		/// <param name="value">The value to be validated.</param>
		/// <returns>true if the value was validated; false if the submitted value was invalid.</returns>
		static Boolean IsValidDoubleValue(Object value)
		{

			// This will test the value to see if it's a real Double.
			Double number = (Double)value;
			return !Double.IsNaN(number) && !Double.IsInfinity(number);

		}

		/// <summary> 
		/// Invoked whenever application code or internal processes call ApplyTemplate.
		/// </summary>
		public override void OnApplyTemplate()
		{

			// This button provides the 'Recycle', 'Go' and 'Stop' actions depending on the input mode.
			this.actionButton = this.GetTemplateChild(BreadcrumbBar.actionButtonPartName) as Button;
			
			// The AutoComplete selector is the drop down control that appears under the free form text and provides the user with options for automatically 
			// completing the current segment of the path.  It provides an event handler for when the selection changes, when the navigation has reached the 
			// boundary of the container and an event handler for when the user clicks on an item in the selection box.
			if (this.autoCompleteSelector != null)
			{
				this.autoCompleteSelector.SelectionChanged -= new SelectionChangedEventHandler(this.OnAutoCompleteSelectionChanged);
				this.autoCompleteSelector.SelectionWrapped -= new RoutedEventHandler(this.OnAutoCompleteSelectionWrapped);
				this.autoCompleteSelector.Clicked -= new RoutedEventHandler(this.OnSelectionClicked);
			}
			this.autoCompleteSelector = this.GetTemplateChild(BreadcrumbBar.autoCompletePartName) as HighlightSelector;
			if (this.autoCompleteSelector != null)
			{
				this.autoCompleteSelector.SelectionChanged += new SelectionChangedEventHandler(this.OnAutoCompleteSelectionChanged);
				this.autoCompleteSelector.SelectionWrapped += new RoutedEventHandler(this.OnAutoCompleteSelectionWrapped);
				this.autoCompleteSelector.Clicked += new RoutedEventHandler(this.OnSelectionClicked);
			}

			// The GadgetBar is where the breadcrumbs are displayed.  It allows the user to navigate a path by selecting the next level of the path in menu-like
			// drop down controls.  The source for the items that appear in the GadgetBar come from an internal list.  This list originally was bound to the control
			// using XAML, but since this is an official 'Part' of the control and there was no reason to allow public access to the internal 'breadcrumbItems'
			// list, it seems like the best design is to make the connection and the list private; there's less chance to screw up by making it and no
			// benefit to making it public.
			if (this.gadgetBar != null)
				this.gadgetBar.ItemsSource = null;
			this.gadgetBar = this.GetTemplateChild(BreadcrumbBar.gadgetBarPartName) as GadgetBar;
			if (this.gadgetBar != null)
				this.gadgetBar.ItemsSource = this.breadcrumbItems;

			// This is the highlight that moves across the progress bar to indicate that something is happening.
			this.glow = this.GetTemplateChild(BreadcrumbBar.glowPartName) as FrameworkElement;

			// The History selector is the drop down control that appears when the drow down arrow is clicked and provides the user with a list of the most 
			// recently typed-in paths.  It provides an event handler for when the selection changes and an event handler for when the user clicks on an item in 
			// the  selection box. The source for the history list comes from a Most Recently Used (MRU) list.  The list is preferable to a public 
			// list attached through XAML as there is no reason to expose the history list to the public.
			if (this.historySelector != null)
			{
				this.historySelector.SelectionChanged -= new SelectionChangedEventHandler(this.OnHistorySelectionChanged);
				this.historySelector.Clicked -= new RoutedEventHandler(this.OnSelectionClicked);
				this.historySelector.ItemsSource = null;
			}
			this.historySelector = this.GetTemplateChild(BreadcrumbBar.historyPartName) as HighlightSelector;
			if (this.historySelector != null)
			{
				this.historySelector.SelectionChanged += new SelectionChangedEventHandler(this.OnHistorySelectionChanged);
				this.historySelector.Clicked += new RoutedEventHandler(this.OnSelectionClicked);
				this.historySelector.ItemsSource = this.history;
			}

			// This part shows the progress of some action as a stylized shadow across the background of the control.
			this.indicator = this.GetTemplateChild(BreadcrumbBar.indicatorPartName) as FrameworkElement;

			// The leaf panel is an area of the BreadcrumbBar where the icon and sometimes the name of the root element are kept.  It has a special purpose when
			// clicked or Double clicked as it will change the mode of input.  For example, Double clicking the leaf panel when editing the path will open up the
			// history drop down control.
			this.leafPanel = this.GetTemplateChild(BreadcrumbBar.leafPanelPartName) as Panel;

			// The TextBox is where the user types in the free form path.  This will remove any association to a previous TextBox and link the new one in so it will
			// advise us of any changes to the text.
			if (this.textBox != null)
				this.textBox.TextChanged -= new TextChangedEventHandler(this.OnTextChanged);
			this.textBox = this.GetTemplateChild(BreadcrumbBar.textBoxPartName) as TextBox;
			if (this.textBox != null)
				this.textBox.TextChanged += new TextChangedEventHandler(this.OnTextChanged);

			// The track is used to scale the progress indicator.  When the actual size of the Track changes, the width of the progress indicator also adjust.
			if (this.track != null)
				this.track.SizeChanged -= new SizeChangedEventHandler(this.OnTrackSizeChanged);
			this.track = this.GetTemplateChild(BreadcrumbBar.trackPartName) as FrameworkElement;
			if (this.track != null)
				this.track.SizeChanged += new SizeChangedEventHandler(this.OnTrackSizeChanged);
	
		}

		/// <summary>
		/// Handles the SelectionChanged routed event.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="selectionChangedEventArgs">The event data.</param>
		void OnAutoCompleteSelectionChanged(Object sender, SelectionChangedEventArgs selectionChangedEventArgs)
		{

			// When the selection in the autoComplete drop down window is changed, this will force the TextBox to display the path that was selected.
			foreach (Object item in selectionChangedEventArgs.AddedItems)
			{
				String path = item as String;
				if (path != null)
					this.SetTextBox(path);
			}
			this.TextBox.CaretIndex = this.TextBox.Text.Length;

		}

		/// <summary>
		/// Handles the selected item in a HighlightSelector reaching the boundary of the container.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnAutoCompleteSelectionWrapped(Object sender, RoutedEventArgs e)
		{

			// When the selection control used for automatically completing the user's path gets to the boundary of it's container, reset the text box to the last
			// known selection from the user.  This gives the user the option to select between the suggestions and the stuff they were already typing.
			this.SetTextBox(this.lastKnownText);
			this.TextBox.CaretIndex = this.TextBox.Text.Length;

		}

		/// <summary>
		/// Handles a click of the button part of a BreadcrumbItem.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="routedEventArgs">The event data.</param>
		void OnButtonClick(Object sender, RoutedEventArgs routedEventArgs)
		{

			// This will generate a new path when a breadcrumb item is clicked.  Note that the toggle button in the BreadcrumbBar can also generate a click event 
			// (as could other controls added to the template).  For this reason we need discrimination logic to select a path only the BreadcrumbItems are
			// clicked.
			BreadcrumbItem breadcrumbItem = routedEventArgs.OriginalSource as BreadcrumbItem;
			if (breadcrumbItem != null)
				this.Source = ExplorerHelper.GenerateSource(breadcrumbItem.DataContext as IExplorerItem);

			// The Action button  usually appears to the right side of the BreadcrumbBar and controls the Recycle, Go and Cancel Search actions.  Since one button 
			// can do several things, some logic needs to be applied to figure out which action should be applied.
			if (routedEventArgs.OriginalSource == this.actionButton)
			{

				// If the BreadcrumbBar has been searching, then cancel the search.
				if (this.IsSearching)
				{
					this.IsSearching = false;
					this.RaiseEvent(new RoutedEventArgs(BreadcrumbBar.CancelEvent));
				}
				else
				{

					// If the BreadcrumbBar was editing, then the button means the user wants to accept the current path.
					if (this.IsEditMode)
					{
						this.AcceptSource();
						this.IsEditMode = false;
						this.IsAutoCompleteOpen = false;
						this.IsHistoryOpen = false;
					}
					else
					{

						// In all other situations, the button means Recycle (Refresh) the current contents.
						this.RaiseEvent(new RoutedEventArgs(BreadcrumbBar.RecycleEvent));

					}

				}

			}

		}

		/// <summary>
		/// Handles the SelectionChanged routed event.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnHistorySelectionChanged(Object sender, SelectionChangedEventArgs e)
		{

			// When the selection in the History drop down window is changed, this will force the TextBox to display the path that was selected.
			foreach (Object item in e.AddedItems)
			{
				HistoryItem historyItem = item as HistoryItem;
				if (historyItem != null)
					this.SetTextBox(historyItem.Source);
				this.TextBox.SelectAll();
			}

		}

		/// <summary>
		/// Handles a change to the visibility of the AutoComplete drop down window.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that that tracks changes to the effective value of this property.</param>
		static void OnIsAutoCompleteOpenPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the generic parameters.
			BreadcrumbBar breadcrumbBar = dependencyObject as BreadcrumbBar;
			Boolean newValue = (Boolean)dependencyPropertyChangedEventArgs.NewValue;
			Boolean oldValue = (Boolean)dependencyPropertyChangedEventArgs.OldValue;

			// When the AutoComplete drop down is opened, we close the history and capture the mouse.  Capturing the mouse insures that we get a notification when the user clicks outside of
			// the drop down.  When the drop down is closed, we release the mouse because we're no longer interested in the click event.  Also, the highlighted element is reset so the next
			// time the drop down is opened we don't accidentally select an old item.
			if (newValue)
			{
				breadcrumbBar.IsHistoryOpen = false;
				Mouse.Capture(breadcrumbBar, CaptureMode.SubTree);
			}
			else
			{
				breadcrumbBar.AutoCompleteSelector.HighlightedElement = null;
				Mouse.Capture(null);
			}

		}

		/// <summary>
		/// Handles a change to the visibility of the History drop down window.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that that tracks changes to the effective value of this property.</param>
		static void OnIsHistoryOpenPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the generic parameters.
			BreadcrumbBar breadcrumbBar = dependencyObject as BreadcrumbBar;
			Boolean newValue = (Boolean)dependencyPropertyChangedEventArgs.NewValue;
			Boolean oldValue = (Boolean)dependencyPropertyChangedEventArgs.OldValue;

			// When the History drop down window is opened we want to make sure the AutoComplete is closed as they can't both be open at the same time.  The user is also automatically
			// switched into editing mode and the mouse is captured so we can tell when the user has clicked outside of the control.  It's important to know when the user has clicked
			// outside of the breadcrumb window so we know when to close down the drop down window.  Conversely, when the drop down window is closed down, we can release the mouse capture
			// because we no longer need to be advised when the user has clicked a mouse button.
			if (newValue)
			{
				breadcrumbBar.IsAutoCompleteOpen = false;
				breadcrumbBar.IsEditMode = true;
				Mouse.Capture(breadcrumbBar, CaptureMode.SubTree);
			}
			else
			{
				breadcrumbBar.HistorySelector.HighlightedElement = null;
				Mouse.Capture(null);
			}

		}

		/// <summary>
		/// Called when the Items property changes.
		/// </summary>
		/// <param name="e">The event data for the ItemsChanged event.</param>
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{

			// This will force the path to be re-evaluated with the new hierarchy.
			if (this.Source != null)
				this.UpdateSource();

		}

		/// <summary>
		/// Called when the ItemsSource property changes.
		/// </summary>
		/// <param name="oldValue">Old value of the ItemsSource property.</param>
		/// <param name="newValue">New value of the ItemsSource property.</param>
		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{

			// This will force the path to be re-evaluated with the new hierarchy.
			if (this.Source != null)
				this.UpdateSource();

		}

		/// <summary> 
		/// Invoked when the KeyDown event is received.
		/// </summary> 
		/// <param name="e">Information about the event.</param>
		protected override void OnKeyDown(KeyEventArgs e)
		{

			// Examine the event for any keys that are significant to the BreadcrumbBar.
			this.HandleKey(e);

		}

		/// <summary>
		/// Invoked when an unhandled Keyboard.LostKeyboardFocus attached event reaches this element.
		/// </summary>
		/// <param name="e">The KeyboardFocusChangedEventArgs that contains event data.</param>
		protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// This will insure that the editing mode is turned off when the keyboard focus has been truely lost to another control.
			if (e.NewFocus == null || !(e.NewFocus is Visual) || !this.IsAncestorOf(e.NewFocus as DependencyObject))
			{
				this.IsAutoCompleteOpen = false;
				this.IsHistoryOpen = false;
				this.IsEditMode = false;
			}

			// This method has no default implementation. Because an intermediate class in the inheritance might implement this method, we recommend that you call 
			// the base implementation in your implementation.
			base.OnLostKeyboardFocus(e);

		}

		/// <summary>
		/// Invoked when an unhandled Mouse.LostMouseCapture attached event reaches an this element.
		/// </summary>
		/// <param name="e">The MouseEventArgs that contains event data.</param>
		protected override void OnLostMouseCapture(MouseEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// This control only explicitly captures the mouse when the AutoCopmlete and History drop downs are open.  The idea of capturing the mouse is so that 
			// the drop down controls can be closed when the user clicks outside of them.  This simple scheme is made more complex because certain controls inside
			// the drop down windows can temporarily steal the mouse capture.  For example, the scroll bars will steal the mouse capture while the slider is being
			// moved, and then release it. Because there's no concept of 'Popping' the mouse capture, we need to recapture the mouse in the situation where a child
			// control attempts to release the mouse capture that we were so careful to obtain.
			if (Mouse.Captured == null && this.IsAncestorOf(e.OriginalSource as DependencyObject))
				if (this.IsAutoCompleteOpen || this.IsHistoryOpen)
				{
					Mouse.Capture(this, CaptureMode.SubTree);
					e.Handled = true;
				}

			// This method has no default implementation. Because an intermediate class in the inheritance might implement this method, we recommend that you call 
			// the base implementation in your implementation.
			base.OnLostMouseCapture(e);

		}

		/// <summary>
		/// Handles a selection of the next level of the hierarchy from the drop down menus of a BreadcrumbItem.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="routedEventArgs">The event data.</param>
		void OnMenuItemClick(Object sender, RoutedEventArgs routedEventArgs)
		{

			// This will generate a path to the selected visual element.
			Gadget gadget = routedEventArgs.OriginalSource as Gadget;
			this.Source = ExplorerHelper.GenerateSource(gadget.DataContext as IExplorerItem);

		}

		/// <summary>
		/// Called when the Maximum property changes.
		/// </summary>
		/// <param name="oldMaximum">Old value of the Maximum property.</param>
		/// <param name="newMaximum">New value of the Maximum property.</param>
		protected virtual void OnProgressMaximumChanged(Double oldMaximum, Double newMaximum)
		{
		}

		/// <summary>
		/// Called when the Maximum property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that tracks changes to the effective value of this property.</param>
		static void OnProgressMaximumPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will make sure that the value still falls within the Maximum and Minimum boundaries.
			BreadcrumbBar element = (BreadcrumbBar)dependencyObject;
			element.CoerceValue(BreadcrumbBar.ProgressValueProperty);

			// This allows derrived classes to inspect the changes.
			element.OnProgressMaximumChanged((Double)dependencyPropertyChangedEventArgs.OldValue, (Double)dependencyPropertyChangedEventArgs.NewValue);

		}

		/// <summary>
		/// Called when the Minimum property changes.
		/// </summary>
		/// <param name="oldMinimum">Old value of the Minimum property.</param>
		/// <param name="newMinimum">New value of the Minimum property.</param>
		protected virtual void OnProgressMinimumChanged(Double oldMinimum, Double newMinimum)
		{
		}

		/// <summary>
		/// Called when the Minimum property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that tracks changes to the effective value of this property.</param>
		static void OnProgressMinimumPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will make sure that the value still falls within the Maximum and Minimum boundaries.
			BreadcrumbBar breadcrumbBar = dependencyObject as BreadcrumbBar;
			breadcrumbBar.CoerceValue(BreadcrumbBar.ProgressMaximumProperty);
			breadcrumbBar.CoerceValue(BreadcrumbBar.ProgressValueProperty);

			// This allows derrived classes to inspect the changes.
			breadcrumbBar.OnProgressMinimumChanged((Double)dependencyPropertyChangedEventArgs.OldValue, (Double)dependencyPropertyChangedEventArgs.NewValue);

		}

		/// <summary>
		/// Invoked when an unhandled Mouse.MouseLeave attached event is raised on this element.
		/// </summary>
		/// <param name="e">The MouseEventArgs that contains the event data.</param>
		protected override void OnMouseLeave(MouseEventArgs e)
		{

			// The main idea here is to find the Gadget with the highlight.  The only way to find this item is to query each item for individually for the highlight
			// property (keep in mind that separators and other objects can be part of a BreadcrumbBar).
			Gadget currentHighlight = null;
			foreach (FrameworkElement frameworkElement in this.GadgetBar.PanelItems)
			{
				Gadget gadget = frameworkElement as Gadget;
				if (gadget != null && gadget.IsHighlighted)
					currentHighlight = gadget;
			}

			// if we don't have capture and the mouse left (but the item isn't selected), then we shouldn't have anything selected.
			if (!this.IsMouseCaptured && !this.IsMouseOver && currentHighlight != null && !currentHighlight.IsKeyboardFocused && !currentHighlight.IsSubmenuOpen)
				currentHighlight.SetIsHighlighted();

			// This method has no default implementation. Because an intermediate class in the inheritance might implement this method, we recommend that you call 
			// the base implementation in your implementation.
			base.OnMouseLeave(e);

		}

		/// <summary>
		/// Invoked when the effective property value of the Source property changes.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="routedEventArgs">The event data.</param>
		void OnPanelItemsChanged(Object sender, RoutedEventArgs routedEventArgs)
		{

			// Once a new level of the hierarchy has been added that the keyboard focus should naturally be moved to this new element. This assumption allows the
			// user to naturally move through the breadcrumb bar items.  Without this, the keyboard focus would stay on the current top-level breadcrumb item after
			// a submenu has selected a child forcing the user to hit an extra key to get to the next level of the hierarchy when that's almost always where they
			// want to be.
			GadgetPanel gadgetPanel = routedEventArgs.OriginalSource as GadgetPanel;
			if (gadgetPanel != null && gadgetPanel.Children.Count != 0 && this.IsKeyboardFocusWithin)
			{
				UIElement uiElement = gadgetPanel.Children[gadgetPanel.Children.Count - 1];
				if (uiElement != null && !Selector.GetIsSelected(uiElement))
					Selector.SetIsSelected(uiElement, true);
			}

		}

		/// <summary>
		/// Invoked when the effective property value of the Source property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that tracks changes to the effective value of this property.</param>
		static void OnSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will call the common method to handle a new path.  The same method is called with the underlying hierarchy is changed also.
			BreadcrumbBar breadcrumbBar = dependencyObject as BreadcrumbBar;
			if (dependencyPropertyChangedEventArgs.NewValue != null)
				breadcrumbBar.UpdateSource();
			
		}

		/// <summary>
		/// Invoked when an unhandled Keyboard.PreviewKeyDown attached event reaches an element in its route that is derived from this class.
		/// </summary>
		/// <param name="e">The KeyEventArgs that contains the event data.</param>
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// Examine the event for any keys that are significant to the BreadcrumbBar.
			if (e.OriginalSource == this.TextBox)
				this.HandleKey(e);

		}

		/// <summary>
		/// Invoked when an unhandled Mouse.PreviewMouseDown attached routed event reaches an element in its route.
		/// </summary>
		/// <param name="e">The MouseButtonEventArgs that contains the event data. The event data reports that one or more mouse buttons were pressed.</param>
		protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// Extract the generic arguments.
			Visual originalSource = e.OriginalSource as Visual;

			// These are the two parts of the Breadcrumb Bar that enable editing when hit with the mouse.  The 'LeafPanel' is that part of the BreadcrumbBar where
			// the icon (and sometimes the header) of the last item in the path is displayed.  Hitting it will bring up the edit mode.
			Boolean isBreadcrumbPanelHit = originalSource is BreadcrumbPanel;
			Boolean isLeafPanelHit = this.leafPanel == null ? false : this.leafPanel.IsAncestorOf(originalSource);

			// When in the breadcrumb mode, hitting the panel (anywhere that isn't filled in with breadcrumb items) or the leaf will put the control into a mode
			// where the user can type in a path.
			if ((isBreadcrumbPanelHit || isLeafPanelHit) && !this.IsEditMode)
			{
				this.IsEditMode = true;
				e.Handled = true;
			}

			// When editing, hitting the leaf (the part of the control at the right where the top-level icon is displayed) will close the history drop down, 
			// Double clicking will open it up.  This part is a little confusing as a user.  Double clicking seems a less confusing way to do both actions but
			// this control was designed to be backward compatible with the Windows Explorer Breadcrumb Bar.
			if (isLeafPanelHit && this.IsEditMode)
			{
				if (this.IsHistoryOpen)
					this.IsHistoryOpen = false;
				else
				{
					if (e.ClickCount == 2)
						this.IsHistoryOpen = true;
				}
				e.Handled = true;
			}

			// This method has no default implementation. Because an intermediate class in the inheritance might implement this method, we recommend that you call 
			// the base implementation in your implementation.
			base.OnPreviewMouseDown(e);

		}

		/// <summary>
		/// Invoked when an unhandled PreviewMouseDownOutsideCapturedElement attached event reaches this element.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="mouseButtonEventArgs">The MouseButtonEventArgs that contains the event data. The event data reports that the left mouse button was pressed.</param>
		static void OnPreviewMouseDownOutsideCapturedElement(Object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{

			// This message is only received when the mouse has been captured and the mouse button is clicked outside of the capture area.  Clicking outside
			// the control is a command to dismiss any drop down windows and release the mouse.
			BreadcrumbBar breadcrumbBar = (BreadcrumbBar)sender;
			breadcrumbBar.IsAutoCompleteOpen = false;
			breadcrumbBar.IsHistoryOpen = false;
			Mouse.Capture(null);

		}

		/// <summary>
		/// Invoked when the animation on the glow control has completed.
		/// </summary>
		/// <param name="sender">The object that generated the event.</param>
		/// <param name="eventArgs">The event data.</param>
		void OnProgressAnimationCompleted(Object sender, EventArgs eventArgs)
		{

			// This will pick up the current animation and run it until the field is cleared and the animation is garbage collected.  Note that all the animation
			// control occurs on the same foreground thread so there is no issue with timing.
			this.glow.BeginAnimation(FrameworkElement.MarginProperty, this.progressBarAnimation);

		}

		/// <summary>
		/// Handles the clicking of an item in the HighlightSelector.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnSelectionClicked(Object sender, RoutedEventArgs e)
		{

			// When an item has been clicked in the AutoComplete or History drop downs, the path in the text box is accepted and we exit back to the breadcrumb 
			// mode.
			this.AcceptSource();
			this.IsAutoCompleteOpen = false;
			this.IsHistoryOpen = false;
			this.IsEditMode = false;

		}

		/// <summary>
		/// handle the TextBoxBase.TextChanged routed event.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="textChangedEventArgs">The event data.</param>
		void OnTextChanged(Object sender, TextChangedEventArgs textChangedEventArgs)
		{

			// This is designed to prevent re-entrancy problems that arise when other controls attempt to update the text box.  The processing below is intended to
			// handle the autocomplete logic for the user and causes problems when the Text property is changed through code.
			if (!this.isTextBoxUpdating)
			{

				// Changing the text will always set this flag if its clear.  The flag is generally used by the template to present a 'Go' button which has the same
				// effect as hitting the enter key when new text is presented.
				if (!this.IsTextChanged)
					this.SetValue(BreadcrumbBar.isTextChangedPropertyKey, true);

				// This text is used to restore the text box when cycling through the autocomplete gets to the end and needs to start over again.
				this.lastKnownText = this.TextBox.Text;

				// The main idea behind autocomplete is to bread up the path entered into the text box into the path components and then recurse into the 
				// hierarchical structure following the path elements.  When the last level of the hierarchy is found, the items in that node can be examined to 
				// see if they match the path entered.
				String sourceText = this.TextBox.Text.Replace(BreadcrumbBar.pathSeparatorCharacter, BreadcrumbBar.separatorCharacter);
				String[] sourceElements = sourceText.Split(BreadcrumbBar.separatorCharacter);

				// The 'leafText' is the text for which we'll by suggesting possibilties in the drop down selector.
				String leafText = sourceElements[sourceElements.Length - 1];

				// The starting point for the hierarchy is the items attached to this control.  Each level of the hiearchy will be compared to the elements in the
				// path.  When a match is found, we dig down into the hierarchy one more level.  When this block of code is finished, we'll have found the parent
				// level of the given path or have concluded that the path has no equivalent in the hierarchy.
				IExplorerItem parentCollection = this.DataContext as IExplorerItem;
				if (parentCollection != null)
				{

					// We'll dig down into the hiarchy as each level of the hierarchy is matched to an element of the path.
					for (Int32 index = 1; index < sourceElements.Length - 1; index++)
					{

						// The parent collection can be null because of a casting issue or because there were no matches to path elements at the previous level.
						if (parentCollection == null)
							break;

						// This is the next level of the path to be found in the current level of the hierarchy.
						String pathPart = sourceElements[index];

						// A copy is made of the current level.  If the next level of the hierarchy can't be matched to the 'pathPart', then the loop will exit
						// without finding a parent node from which the autocomplete can suggest matches.
						IExplorerItem previousCollection = parentCollection;
						parentCollection = null;

						// This will perform a case-insensitive search of the current level of the hiearchy looking for a match to the current path element. If a
						// match is found, the logic will continue to dig down into the next level and so on until all the elements of the path have been matched
						// against nodes in the hiearchy.
						foreach (IExplorerItem iExplorerItem in previousCollection.Children)
							if (pathPart.Equals(iExplorerItem.Name, StringComparison.CurrentCultureIgnoreCase))
								parentCollection = iExplorerItem as IExplorerItem;

					}

				}

				// If the hiearchy was matched to the elements of the path then we can try to suggest matches to complete the current 'leafText'.  If not then the
				// autocomplete has no reason to be visible.
				if (parentCollection == null)
				{

					// No parent, no suggestions.
					this.IsAutoCompleteOpen = false;

				}
				else
				{

					// When suggesting new matches the old ones need to be cleared away.
					this.AutoCompleteSelector.Items.Clear();

					// This is where the suggestions are made.  Any item in the leaf node that matches the text in the leaf (the last element of the path) is added
					// to the drop down as a suggestion.  The suggestion drop down is only displayed when we have something useful to suggest to the user.  Note
					// that if there is only one item and that item is a complete match to the text in the path, then there's nothing useful to suggest and the box
					// is hidden.  This is the way the Explorer breadcrumb control handles completions, so the feature is provided here also.
					String lastChildName = null;
					foreach (IExplorerItem iExplorerItem in parentCollection.Children)
					{
						if (iExplorerItem.Name.StartsWith(leafText, StringComparison.CurrentCultureIgnoreCase))
						{
							lastChildName = iExplorerItem.Name;
							Uri uri = ExplorerHelper.GenerateSource(iExplorerItem);
							this.AutoCompleteSelector.Items.Add(uri.OriginalString.Replace(BreadcrumbBar.separatorCharacter, BreadcrumbBar.pathSeparatorCharacter));
						}
					}
					this.IsAutoCompleteOpen = this.AutoCompleteSelector.Items.Count != 0 &&
						(this.AutoCompleteSelector.Items.Count != 1 || !leafText.Equals(lastChildName, StringComparison.CurrentCultureIgnoreCase));
		
				}

			}

		}

		/// <summary>
		/// Handles a change to the size of the tracking bar where the progress indicator is displayed.
		/// </summary>
		/// <param name="sender">The object that orignated the event.</param>
		/// <param name="e">The event data.</param>
		void OnTrackSizeChanged(Object sender, SizeChangedEventArgs e)
		{

			// This will adjust the width of the progress indicator to reflect the new size of the tracking area.
			this.SetProgressBarIndicatorLength();

		}

		/// <summary>
		/// Raises the ValueChanged routed event.
		/// </summary>
		/// <param name="oldValue">Old value of the Value property.</param>
		/// <param name="newValue">New value of the Value property.</param>
		protected virtual void OnValueChanged(Double oldValue, Double newValue)
		{

			// This will bubble the 'Value Changed' event up the element hierarchy and notify any listeners that the value has changed.
			RoutedPropertyChangedEventArgs<Double> e = new RoutedPropertyChangedEventArgs<Double>(oldValue, newValue);
			e.RoutedEvent = ProgressValueChangedEvent;
			base.RaiseEvent(e);

		}

		/// <summary>
		/// Called when the Minimum property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that tracks changes to the effective value of this property.</param>
		static void OnValuePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will effectively change the generic PropertyChanged event into a specific ValueChanged event using the data extracted from the generic event.
			BreadcrumbBar breadcrumbBar = dependencyObject as BreadcrumbBar;
			breadcrumbBar.OnValueChanged((Double)dependencyPropertyChangedEventArgs.OldValue, (Double)dependencyPropertyChangedEventArgs.NewValue);

			// This will adjust the size of the progress indicator within the tracking area.
			breadcrumbBar.SetProgressBarIndicatorLength();

		}

		/// <summary>
		/// Adjust the size of the progress indicator for the tracking area.
		/// </summary>
		void SetProgressBarIndicatorLength()
		{

			// This will scale the indicator to match the new size of the tracking rectangle and adjust the speed at which the highlight moves across the indicator.
			if (this.track != null && this.indicator != null)
			{
				Double normalizedValue = this.ProgressMaximum <= this.ProgressMinimum ? 0.0 : (this.ProgressValue - this.ProgressMinimum) / (this.ProgressMaximum - this.ProgressMinimum);
				this.indicator.Width = normalizedValue * this.track.ActualWidth;
				this.UpdateAnimation();
			}

		}

		/// <summary>
		/// Sets the text box value without triggering any of the user interface events, such as autocomlete.
		/// </summary>
		/// <param name="value">The new text of the control.</param>
		void SetTextBox(String value)
		{

			// This is used by internal processes to set the text in the text box, such as when initializing or reverting back to a known value.  The user interface
			// triggers such as autocomplete are disabled by setting the 'isTextBoxUpdating' flag.
			try
			{
				this.isTextBoxUpdating = true;
				this.TextBox.Text = value;
			}
			finally
			{
				this.isTextBoxUpdating = false;
			}

		}

		/// <summary>
		/// Sets the text box value without triggering any of the user interface events, such as autocomlete.
		/// </summary>
		/// <param name="uri">The new text of the control.</param>
		void SetTextBox(Uri uri)
		{

			// The breadcrumb bar uses the DOS separator character between path elements.
			this.SetTextBox(uri.OriginalString.Replace(BreadcrumbBar.separatorCharacter, BreadcrumbBar.pathSeparatorCharacter));

		}

		/// <summary>
		/// Adjust the animation on the glow highlight that moves across the progress indicator.
		/// </summary>
		void UpdateAnimation()
		{

			// A glow and an indicator parts are needed in order to animate.  If they aren't present, then there's nothing to animate.
			if (this.glow != null && this.indicator != null)
			{

				// The animation is only usefull when the control is visible and there is room for the glow highlight to move.
				if (this.IsVisible && this.glow.Width > 0.0 && this.track.ActualWidth > 0.0)
				{

					// This sets up the animation frames.  The animation is accomplished by moving the margin.  The margin is a Thickness type, so we'll use a
					// Thickness animation to move the highlight across the indicator bar.  This will calculate how fast the highlight will move across the
					// indicator bar and how long it will wait behind the scene until it appears again.
					TimeSpan activeTime = TimeSpan.FromSeconds(this.track.ActualWidth / BreadcrumbBar.highlightSpeed);
					TimeSpan pauseTime = TimeSpan.FromSeconds(this.indicator.ActualWidth / this.track.ActualWidth * BreadcrumbBar.highlightPause);

					// The ThicknessAnimationUsingKeyFrames class is needed for the pause between the end of the highlight movement and the start.  So there are
					// basically two segments to the animation.  The first is where the highlight moves across the indicator and is explicitly created with the
					// frames.  The second is the pause where nothing is moving and that is done implicitly by the difference between the duration of this animation
					// and the time it takes to complete the first frame.  This technique allows for an infinitely repeating pattern that includes a pause between
					// the visible effects.  We could not accomplish the pause with a standard Thickness animation.
					this.progressBarAnimation = new ThicknessAnimationUsingKeyFrames();
					this.progressBarAnimation.Duration = new Duration(activeTime + pauseTime);
					this.progressBarAnimation.Completed += new EventHandler(this.OnProgressAnimationCompleted);
					this.progressBarAnimation.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(), TimeSpan.Zero));
					this.progressBarAnimation.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(this.ActualWidth, 0.0, 0.0, 0.0), activeTime));
					this.progressBarAnimation.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(this.ActualWidth, 0.0, 0.0, 0.0), activeTime + pauseTime));
					this.progressBarAnimation.Freeze();

					// The glow animation will repeat the effect as long as the search operation is in progress.  However, as the size of the breadcrumb bar changes
					// or the progress indicator fills in the space it will cause the animation to update.  This creates a problem when the value behind the
					// progress bar is animated also. Each time the animated value changes, it fires off an event that calls this method and the animation starts
					// all over again with the new metrics for the progress changing the timing of the glow effect.  This creates a problem where the animation 
					// basically can't get started.  To fix the problem we allow the animation to complete before changing the parameters.  Of course, in order to
					// allow it to complete, we need to know whether it's started or not.  This will check to see if the animation has begun and, if it hasn't, it
					// will start the animation.  If an animation is already in progress, the completion routine will pick up the newly created animation for the
					// next cycle and continue to use it until the field is cleared.
					ValueSource valueSource = DependencyPropertyHelper.GetValueSource(this.glow, FrameworkElement.MarginProperty);
					if (!valueSource.IsAnimated)
						this.glow.BeginAnimation(FrameworkElement.MarginProperty, this.progressBarAnimation);

				}
				else
				{

					// When we are no longer animating this control we will set this field to null.  When the current animation is completed it will pick up this
					// value and clear the animated properties on the glow control.
					this.progressBarAnimation = null;

				}

			}

		}

		/// <summary>
		/// Updates the BreadcrumbBar to reflect the current path.
		/// </summary>
		void UpdateSource()
		{

			// Extract the path elements from the current path.
			String[] newSourceElements = this.Source.OriginalString.TrimEnd(BreadcrumbBar.separatorCharacter).Split(BreadcrumbBar.separatorCharacter);

			// The path may have changed from an external sources such as the travel buttons.  Shut down the user interface elements that are used for typing in a
			// path when the path changes from elsewhere.
			this.IsAutoCompleteOpen = false;
			this.IsHistoryOpen = false;
			this.IsEditMode = false;

			// The main idea here is to clear out whatever breadcrumb items were in the GadgetBar and reconstruct a series of BreadcrumbItems that reflect the path 
			// selected.
			this.breadcrumbItems.Clear();
			ICollection parentCollection = this.Items as ICollection;
			for (Int32 pathIndex = 1; pathIndex < newSourceElements.Length; pathIndex++)
			{

				// Cycle through each of the child elements at the current level of the hiearchy looking for a path element who's name matches the current level of
				// the path.  When an item is found it becomes the new parent collection and the recursion continues until each element in the new path has had a
				// visual element created for it.
				foreach (Object item in parentCollection)
				{

					// Extract the data context of the current item in the hierarchy.  It it can be reduced to an IExplorerItem interface and the name of the item
					// matches the current path element, then we can create a visual element to represent it.
					IExplorerItem iExplorerItem = item as IExplorerItem;
					if (iExplorerItem == null || !newSourceElements[pathIndex].Equals(iExplorerItem.Name, StringComparison.CurrentCultureIgnoreCase))
						continue;

					// The child collection of the explorer item is used several times below.  For performance it is extracted here once.
					ICollection iCollection = iExplorerItem as ICollection;

					// The root is handled differently than the other items.  Architecturally, the overflow item acts as a sort of proxy for the root.  It shows up
					// in the same position where the root of the path would be shown, but it's not really a BreadcrumbItem, it's an overflow item.  So special
					// processing is required.  Moreover, when one more more path elements below the root are displayed, an actual BreadcrumbItem is created for the
					// root element and placed in the overflow panel with a separator.
					switch (pathIndex)
					{

					case 1:

						// The child elements of the root are placed in the overflow panel.  This makes the overflow panel appear to be a BreadcrumbItem while at 
						// the same time holding any items that can't fit into the visible BreadcrumbBar.
						ICollection childCollection = iCollection;
						if (childCollection != null)
							foreach (Object childItem in childCollection)
								this.breadcrumbItems.Add(CreateBreadcrumb(childItem, OverflowMode.Always));

						break;

					case 2:

						// A menu items separator and the root BreadcrumbItem are added to the overflow panel when there are path elements to be selected in the
						// BreadcrumbBar.  The root BreadcrumbItem allows the user to reset the BreadcrumbBar and it also provides an area visually separated from the
						// root child items where the other path elements that can't fit into the visual bar will be placed.
						IExplorerItem rootItem = this.Items[0] as IExplorerItem;
						if (rootItem != null)
						{
							this.breadcrumbItems.Insert(0, CreateBreadcrumb(rootItem, OverflowMode.Always));
							this.breadcrumbItems.Insert(1, this.rootSeparator);
						}

						// The overflow panel doesn't require the same handling for the child items as a BreadcrumbItem.  A BreadcrumbItem manages it's own
						// child MenuItems by hiding them when acting as a submenu and showing them acting as a top-level MenuItem.  Marking the selected child item
						// is handled within the class.  In contrast, the overflow panel needs to manage the marking of it's own children.
						foreach (FrameworkElement frameworkElement in this.breadcrumbItems)
						{
							BreadcrumbItem childBreadcrumbItem = frameworkElement as BreadcrumbItem;
							if (childBreadcrumbItem != null && childBreadcrumbItem.DataContext == iExplorerItem)
								childBreadcrumbItem.IsParent = true;
						}

						// This list is used by the BreadcrumbBar panel to manage the list of items that the user sees.  The control has an overflow mechanism that 
						// moves the items that don't fit into an overflow panel.
						this.breadcrumbItems.Add(CreateBreadcrumb(iExplorerItem, OverflowMode.AsNeeded));

						break;

					default:

						// The addition of a path element in any other position after the first two path elements will call on the parent BreadcrumbItem to mark
						// the selected child item that points to the next level of the path.  When used with a standard template, this creates a bold highlight in
						// the drop down menu that indicates the next level of the hierarchy.
						BreadcrumbItem parentItem = this.breadcrumbItems[this.breadcrumbItems.Count - 1] as BreadcrumbItem;
						parentItem.SetSelection(iExplorerItem);

						// This list is used by the BreadcrumbBar panel to manage the list of items that the user sees.  The control has an overflow mechanism that 
						// moves the items that don't fit into an overflow panel.
						this.breadcrumbItems.Add(CreateBreadcrumb(iExplorerItem, OverflowMode.AsNeeded));

						break;

					}

					// This provides a visual indication in the leaf panel element of the path is currently selected in the BreadcrumbBar.
					this.SetValue(BreadcrumbBar.leafImageSourcePropertyKey, iExplorerItem.SmallImageSource);
					this.SetValue(BreadcrumbBar.leafHeaderPropertyKey, pathIndex == 1 ? iExplorerItem.Name : null);

					// We will recurse into the hierarchy until all the elements of the current path are found.  Note that we validate the path before trying to
					// display it so there's no need to check for poorly formed paths.
					parentCollection = iCollection;
					break;

				}

			}

		}

	}

}
