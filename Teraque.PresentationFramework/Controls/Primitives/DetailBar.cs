namespace Teraque.Windows.Controls.Primitives
{

	using System;
	using System.Globalization;
	using System.Collections.ObjectModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Collections;
	using System.ComponentModel;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Controls.Primitives;
	using System.Windows.Markup;
	using System.Windows.Media;
	using Teraque.Windows.Data;

	/// <summary>
	/// Represents a control that displays metadata information about an item in a wrappable window.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[ContentProperty("Items")]
	public class DetailBar : StatusBar
	{

		/// <summary>
		/// The gap between the metadata key and the value.
		/// </summary>
		const Double centeredMetadataGap = 5.0;

		/// <summary>
		/// A collection of metadata common to all users of the DetailBar (e.g. Name of the item, item type).
		/// </summary>
		ObservableCollection<FrameworkElement> commonMetadata = new ObservableCollection<FrameworkElement>();

		/// <summary>
		/// Identifies the HasItems dependency property.
		/// </summary>
		public new static readonly DependencyProperty HasItemsProperty;

		/// <summary>
		/// Identifies the HasItems dependency property.key.
		/// </summary>
		static readonly DependencyPropertyKey hasItemsPropertyKey = DependencyProperty.RegisterReadOnly(
			"HasItems",
			typeof(Boolean),
			typeof(DetailBar),
			new FrameworkPropertyMetadata(false));

		/// <summary>
		/// Identifies the Icon dependency property.
		/// </summary>
		public static readonly DependencyProperty IconProperty;

		/// <summary>
		/// Identifies the Icon dependency property.key.
		/// </summary>
		static readonly DependencyPropertyKey iconPropertyKey = DependencyProperty.RegisterReadOnly(
			"Icon",
			typeof(ImageSource),
			typeof(DetailBar),
			new FrameworkPropertyMetadata());

		/// <summary>
		/// An infinite size used for measuring visual elements.
		/// </summary>
		static Size infiniteSize = new Size(Double.PositiveInfinity, Double.PositiveInfinity);

		/// <summary>
		/// This provides a collection that can be accessed directly.
		/// </summary>
		ViewableCollection items;

		/// <summary>
		/// Identifies the ItemsSource dependency property.
		/// </summary>
		public new static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
			"ItemsSource",
			typeof(IEnumerable),
			typeof(DetailBar),
			new FrameworkPropertyMetadata(DetailBar.OnItemsSourcePropertyChanged));

		/// <summary>
		/// Brush used to color the labels in the metadata area.
		/// </summary>
		static SolidColorBrush labelBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x5A, 0x67, 0x79));

		/// <summary>
		/// When the details area grows larger than this value, the large image is used.
		/// </summary>
		const Double minLargeImageHeight = 56.0;

		/// <summary>
		/// Typeface used for text in the metadata area.
		/// </summary>
		static Typeface typeface = new Typeface("Segoe UI");

		/// <summary>
		/// The color used to paint the value of a metadata item.
		/// </summary>
		static SolidColorBrush valueBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x39, 0x5B));

		/// <summary>
		/// Initializes the DetailBar class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static DetailBar()
		{

			// This allows us to provide styling for the DetailBar in the template that are distinct from the base class.
			DefaultStyleKeyProperty.OverrideMetadata(typeof(DetailBar), new FrameworkPropertyMetadata(typeof(DetailBar)));

			// These keys provides public access to read-only dependency objects.  Since the compiler doesn't check dependancies on initializers, we must force them
			// to be assigned after the fields are initialized.
			DetailBar.HasItemsProperty = DetailBar.hasItemsPropertyKey.DependencyProperty;
			DetailBar.IconProperty = DetailBar.iconPropertyKey.DependencyProperty;

		}

		/// <summary>
		/// Initializes a new instance of the DetailBar class.
		/// </summary>
		public DetailBar()
		{
			// The data context drives the information that is presented in this control.
			this.DataContextChanged += new DependencyPropertyChangedEventHandler(OnDataContextChanged);

			// When the size of the frame changes we need to adjust the settings on the grid to make sure that everything important stays visible.
			this.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);

			// This is the collection of items that appear in the metadata area.
			this.items = new ViewableCollection();

			// The actual detail area is composed of two collections.  One is basically a 'system' area where metadata common to all objects is displayed, such as
			// the name of the element and the element's type.  The second part is filled in with data provided by the consumer of the DetailBar.  In most
			// scenarios, this is the ExplorerFrame window which, in turn is fed metadata by an embedded page.  However, the DetailBar doesn't want to make a
			// distinction between the two lists when presenting the information in the WrapPanel: both collections are aggregated and treated as a single list.
			CompositeCollection compositCollection = new CompositeCollection();
			compositCollection.Add(new CollectionContainer() { Collection = this.commonMetadata });
			compositCollection.Add(new CollectionContainer() { Collection = this.items });
			base.ItemsSource = compositCollection;

		}

		/// <summary>
		/// Gets or sets a collection used to generate the content of the ItemsControl.
		/// </summary>
		public ImageSource Icon
		{
			get
			{
				return this.GetValue(DetailBar.IconProperty) as ImageSource;
			}
		}

		/// <summary>
		/// Gets the collection used to generate the content of the control.
		/// </summary>
		public new ViewableCollection Items
		{
			get
			{
				return this.items;
			}
		}

		/// <summary>
		/// Gets or sets a collection used to generate the content of the ItemsControl.
		/// </summary>
		public new IEnumerable ItemsSource
		{
			get
			{
				return this.GetValue(DetailBar.ItemsSourceProperty) as IEnumerable;
			}
			set
			{
				this.SetValue(DetailBar.ItemsSourceProperty, value);
			}
		}

		/// <summary>
		/// Creates a value block for presenting percent values.
		/// </summary>
		/// <param name="propertyName">The name of the property to be bound to the value.</param>
		/// <returns>A visual element that will display the bound value as a percent.</returns>
		public static FrameworkElement CreateDateTimeMetadataValue(String propertyName)
		{

			// This will create a presenter for the percent metadata values and bind it to the given property.
			DateTimeBlock dateTimeBlock = new DateTimeBlock() { Foreground = DetailBar.valueBrush, FontSize = 12.0, TextAlignment = TextAlignment.Left };
			BindingOperations.SetBinding(dateTimeBlock, DateTimeBlock.ValueProperty, new Binding() { Path = new PropertyPath(propertyName) });
			return dateTimeBlock;

		}

		/// <summary>
		/// Creates a value block for presenting System.Double values.
		/// </summary>
		/// <param name="propertyName">The name of the property to be bound to the value.</param>
		/// <returns>A visual element that will display the bound value as an integer.</returns>
		public static FrameworkElement CreateDoubleMetadataValue(String propertyName)
		{

			// This will create a presenter for the System.Double metadata values and bind it to the given property.
			DoubleBlock int32Block = new DoubleBlock() { Foreground = DetailBar.valueBrush, FontSize = 12.0, TextAlignment = TextAlignment.Left };
			BindingOperations.SetBinding(int32Block, DoubleBlock.ValueProperty, new Binding() { Path = new PropertyPath(propertyName) });
			return int32Block;

		}

		/// <summary>
		/// Creates a value block for presenting System.Int32 values.
		/// </summary>
		/// <param name="propertyName">The name of the property to be bound to the value.</param>
		/// <returns>A visual element that will display the bound value as an integer.</returns>
		public static FrameworkElement CreateInt32MetadataValue(String propertyName)
		{

			// This will create a presenter for the System.Int32 metadata values and bind it to the given property.
			Int32Block int32Block = new Int32Block() { Foreground = DetailBar.valueBrush, FontSize = 12.0, TextAlignment = TextAlignment.Left };
			BindingOperations.SetBinding(int32Block, Int32Block.ValueProperty, new Binding() { Path = new PropertyPath(propertyName) });
			return int32Block;

		}

		/// <summary>
		/// Creates a metadata element
		/// </summary>
		/// <param name="key">The key (name) of the metadata item.</param>
		/// <param name="dataContext">The data context for the element that dislays the metadata pair.</param>
		/// <param name="valueElement">The presenter for the metadata.</param>
		/// <returns>A metadata object presenter that constinsts of a key (description) and a value.</returns>
		public static FrameworkElement CreateMetadataElement(String key, Object dataContext, FrameworkElement valueElement)
		{

			// Validate the parameters.
			if (valueElement == null)
				throw new ArgumentNullException("valueElement");

			// The main idea here is to create a visual element that can align along the break between the key and the value of the metadata.  That is, when we're
			// done creating this element, if you were to center it in a panel along with other, similarly created items, then they would all appear to line up on
			// the break between the key and the value.  Note that all of these containers are given a data context so that the value block can have something to
			// bind to.  The general idea is to give the Metadata container a data structure with properties that can be bound to the value blocks.  In this way,
			// just updating the property of that data structure will display that value in the DetailBar.
			StackPanel stackPanel = new StackPanel()
			{
				DataContext = dataContext,
				Orientation = Orientation.Horizontal,
				HorizontalAlignment = HorizontalAlignment.Center,
				Margin = new Thickness(5, 1, 5, 1)
			};

			// The label for the metadata pair is constructed using a predefined gap between the label and the value.  It is aligned to the right, the value will 
			// be aligned to the left, so they appear to be centered on the break.
			TextBlock labelBlock = new TextBlock()
			{
				FontFamily = DetailBar.typeface.FontFamily,
				FontSize = 12.0,
				Foreground = DetailBar.labelBrush,
				Padding = new Thickness(0, 0, DetailBar.centeredMetadataGap, 0),
				Text = key,
				TextAlignment = TextAlignment.Right
			};

			// The panel consists of a label and a value that presents the value associated with that label.
			stackPanel.Children.Add(labelBlock);
			stackPanel.Children.Add(valueElement);

			// This will make the width (and height) of the two text blocks equal to the maximum of either the key or the value.  That way, when they are placed in
			// the StackPanel, and the StackPanel is place in another panel, they will line up along the break between the two TextBlocks.  Note that the padding
			// has been included with the font metrics to calculate the complete width of the control.
			labelBlock.Measure(DetailBar.infiniteSize);
			valueElement.Measure(DetailBar.infiniteSize);
			labelBlock.Width = valueElement.Width = Math.Max(labelBlock.DesiredSize.Width + DetailBar.centeredMetadataGap, valueElement.DesiredSize.Width);
			labelBlock.Height = valueElement.Height = Math.Max(labelBlock.DesiredSize.Height, valueElement.DesiredSize.Height);

			// This control consists of a label/value pair.
			return stackPanel;

		}

		/// <summary>
		/// Creates the metadata text.
		/// </summary>
		/// <param name="text">The text to be displayed.</param>
		/// <param name="foreground">The brush to use for the text.</param>
		/// <param name="fontSize">The font size of the text.</param>
		/// <param name="padding">The margins around the text block.</param>
		/// <returns>An element containing the text that can be displayed in the metadata area of the DetailBar.</returns>
		public static TextBlock CreateMetadataText(String text, Brush foreground, Double fontSize, Thickness padding, TextAlignment textAlignment)
		{

			// Create a TextBlock to display the given text.
			return new TextBlock()
			{
				FontFamily = DetailBar.typeface.FontFamily,
				FontSize = fontSize,
				Foreground = foreground,
				Padding = padding,
				Text = text,
				TextAlignment = textAlignment
			};

		}

		/// <summary>
		/// Creates a value block for presenting percent values.
		/// </summary>
		/// <param name="propertyName">The name of the property to be bound to the value.</param>
		/// <returns>A visual element that will display the bound value as a percent.</returns>
		public static FrameworkElement CreatePercentMetadataValue(String propertyName)
		{

			// This will create a presenter for the percent metadata values and bind it to the given property.
			PercentControl percentControl = new PercentControl() {
				Foreground = DetailBar.valueBrush,
				FontSize = 12.0,
				HorizontalContentAlignment = HorizontalAlignment.Left };
			BindingOperations.SetBinding(percentControl, PercentControl.PercentProperty, new Binding() { Path = new PropertyPath(propertyName) });
			return percentControl;

		}

		/// <summary>
		/// Creates a value block for presenting the progress of a value.
		/// </summary>
		/// <param name="propertyName">The name of the property to be bound to the value.</param>
		/// <returns>A visual element that will display the bound value as a percent.</returns>
		public static FrameworkElement CreateProgressMetadataValue(String propertyName, Double minimum, Double maximum)
		{

			// The metadata methods will create a stack panel that is centered on the break between the elements.  To do this, it sets both controls in the stack
			// panel (the label presenter and the value presenter) to the same size.  In order to allow the progress bar to have a height that is independant of the
			// maximum height of the controls in the metadata stack panel, it will be presented inside a grid.
			Grid grid = new Grid();

			// This will create a presenter for the progress of a values and bind it to the given property.
			ProgressBar progressBar = new ProgressBar();
			progressBar.Minimum = minimum;
			progressBar.Maximum = maximum;
			progressBar.Width = 100.0;
			progressBar.Height = 12.0;
			progressBar.Orientation = Orientation.Horizontal;
			progressBar.VerticalAlignment = VerticalAlignment.Center;
			BindingOperations.SetBinding(progressBar, ProgressBar.ValueProperty, new Binding() { Path = new PropertyPath(propertyName) });

			// Adding the progress bar to the stack panel allows it to have it's own height.  Otherwise the progress bar is fixed to be the maximum height of 
			// either the label presenter or the value presenter.
			grid.Children.Add(progressBar);

			// This grid is used to present the progress.
			return grid;

		}

		/// <summary>
		/// Determines if the specified item is (or is eligible to be) its own container.
		/// </summary>
		/// <param name="item">The specified object to evaluate.</param>
		/// <returns>Returns true if the item is (or is eligible to be) its own container; otherwise, false.</returns>
		protected override Boolean IsItemItsOwnContainerOverride(Object item)
		{

			// The DetailBar doesn't really discriminate as all items end up in the WrapPanel.  If an item is a FrameworkElement, it can play in the DetailBar.
			return item is FrameworkElement ? true : base.IsItemItsOwnContainerOverride(item);

		}

		/// <summary>
		/// Handles the changing of the data context.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event data.</param>
		void OnDataContextChanged(Object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// If the data context for this item is an ExplorerItem, we can set the common elements that appear in the DetailBar for all ExplorerItems.
			IExplorerItem iExplorerItem = dependencyPropertyChangedEventArgs.NewValue as IExplorerItem;
			if (iExplorerItem != null)
			{

				// This will set the image according to how much space is available in the DetailBar.
				this.SetImage();

				// Clear out whatever was previously in the DetailBar and present the item's name and type.
				this.commonMetadata.Clear();
				this.commonMetadata.Add(DetailBar.CreateMetadataText(iExplorerItem.Name, Brushes.Black, 13.33, new Thickness(0), TextAlignment.Left));
				this.commonMetadata.Add(
					DetailBar.CreateMetadataText(iExplorerItem.TypeDescription, DetailBar.labelBrush, 12.0, new Thickness(0, 1, 0, 1), TextAlignment.Left));

			}

		}

		/// <summary>
		/// Invoked when the effective property value of the ItemsSource property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnItemsSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the event arguments.
			DetailBar detailBar = (DetailBar)dependencyObject;
			IEnumerable newValue = (IEnumerable)dependencyPropertyChangedEventArgs.NewValue;

			// The ItemsSource can be cleared by setting the property to 'null' (and providing it's not data bound).  Otherwise this handler will set the
			// source of the internal collection to use the supplied collection.
			if (dependencyPropertyChangedEventArgs.NewValue == null && !BindingOperations.IsDataBound(dependencyObject, DetailBar.ItemsSourceProperty))
				detailBar.items.ClearItemsSource();
			else
				detailBar.items.ItemsSource = newValue;

		}

		/// <summary>
		/// Occurs when either the ActualHeight or the ActualWidth properties change value on this element.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="sizeChangedEventArgs">The event data.</param>
		void OnSizeChanged(Object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{

			// Format the image when the size has changed.
			this.SetImage();

		}

		/// <summary>
		/// Determines the proper image for the control based on how much room there is to display an icon.
		/// </summary>
		void SetImage()
		{

			// Select the image based on how much space is available.  Small Icons look terrible when they are magnified and large icons look terrible when they are
			// compressed.  This algorithm chooses the best looking image given how large it will be in the detail bar.
			IExplorerItem iExplorerItem = this.DataContext as IExplorerItem;
			if (iExplorerItem != null)
				this.SetValue(
					DetailBar.iconPropertyKey,
					this.ActualHeight > DetailBar.minLargeImageHeight ? iExplorerItem.ExtraLargeImageSource : iExplorerItem.LargeImageSource);

		}

	}

}
