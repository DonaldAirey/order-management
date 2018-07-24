namespace Teraque.AssetNetwork.Windows.Controls
{

	using System;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;

	/// <summary>
	/// A ComboBox used to select a Blotter.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class BlotterComboBox : ComboBox
	{

		/// <summary>
		/// The collection of blotters in this control.
		/// </summary>
		static BlotterCollection blotterCollection = new BlotterCollection();

		/// <summary>
		/// Identifies the BlotterId dependency property.
		/// </summary>
		public static readonly DependencyProperty BlotterIdProperty = DependencyProperty.Register(
			"BlotterId",
			typeof(Guid),
			typeof(BlotterComboBox),
			new FrameworkPropertyMetadata(Guid.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(BlotterComboBox.OnBlotterIdPropertyChanged)));

		/// <summary>
		/// Identifies the BlotterItems dependency property key.
		/// </summary>
		static readonly DependencyPropertyKey blotterItemsPropertyKey = DependencyProperty.RegisterReadOnly(
				"BlotterItems",
				typeof(ListCollectionView),
				typeof(BlotterComboBox),
				new FrameworkPropertyMetadata());

		/// <summary>
		/// Identifies the BlotterItems dependency property.
		/// </summary>
		public readonly static DependencyProperty BlotterItemsProperty;

		/// <summary>
		/// An ordered view of the blotters.
		/// </summary>
		static ListCollectionView listCollectionView;

		/// <summary>
		/// Initialize the BlotterBox class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static BlotterComboBox()
		{

			// Initialize the read-only properties using their keys.
			BlotterComboBox.BlotterItemsProperty = BlotterComboBox.blotterItemsPropertyKey.DependencyProperty;

			// This creates an association with an implicit default style in the themes.
			BlotterComboBox.DefaultStyleKeyProperty.OverrideMetadata(typeof(BlotterComboBox), new FrameworkPropertyMetadata(typeof(BlotterComboBox)));

			// This will order the items in the drop down list alphabetically.
			BlotterComboBox.listCollectionView = CollectionViewSource.GetDefaultView(BlotterComboBox.blotterCollection) as ListCollectionView;
			BlotterComboBox.listCollectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

		}

		/// <summary>
		/// Initializes a new instance of the BlotterBox class.
		/// </summary>
		public BlotterComboBox()
		{

			// Provide the combobox with a set of static blotter items.  The collection of blotter items can be changed by the database and those changes will be
			// reflected in the list, but there is only one list needed for all the controls that consume this combobox, so we use a static list, which is not to
			// say the items in the list are static.
			this.SetValue(BlotterComboBox.blotterItemsPropertyKey, BlotterComboBox.listCollectionView);

			// Normally this would be a Metadata override, but the logic (which I considered flawed) for this property watches for changes to the default value to 
			// set the path to the property.  Therefore, overriding a default metadata will not actually set the property because the property never changes.  The 
			// same is true of the SelectedValuePath property.
			this.SetValue(BlotterComboBox.DisplayMemberPathProperty, "Name");
			this.SetValue(BlotterComboBox.SelectedValuePathProperty, "BlotterId");

			// This sets up a default binding for the ItemsSource property to the collection of blotters.  It can be overridden in the XAML, but practically there is
			// no reason to ever override the default collection.
			Binding itemsSourceBinding = new Binding("BlotterItems");
			itemsSourceBinding.RelativeSource = new RelativeSource(RelativeSourceMode.Self);
			BindingOperations.SetBinding(this, BlotterComboBox.ItemsSourceProperty, itemsSourceBinding);

			// This allows us to listen to changes to the underlying list and select a default item when data finally appears.
			BlotterComboBox.blotterCollection.CollectionChanged += this.OnBlotterCollectionChanged;

		}

		/// <summary>
		/// Handles a change to the collection of blotters.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event data.</param>
		void OnBlotterCollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
		{

			// This will select the first item in the list when there are no selections and the list of blotters has been udpated.
			if (e.Action == NotifyCollectionChangedAction.Add)
				if (this.SelectedIndex == -1)
					this.SelectedIndex = 0;

		}

		/// <summary>
		/// Handles a change to the BlotterId property.
		/// </summary>
		/// <param name="dependencyObject">The Object that originated the event.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event arguments.</param>
		static void OnBlotterIdPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will select the item with the given blotter id (if it exists in the collection of items).
			BlotterComboBox blotterComboBox = dependencyObject as BlotterComboBox;
			Guid blotterId = (Guid)dependencyPropertyChangedEventArgs.NewValue;
			blotterComboBox.SelectedValue = blotterId;

		}

		/// <summary>
		/// Responds to a ComboBox selection change by raising a SelectionChanged event. 
		/// </summary>
		/// <param name="e">Provides data for SelectionChangedEventArgs.</param>
		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{

			// Allow the base class to handle the change.
			base.OnSelectionChanged(e);

			// This reconciles the BlotterId property to the SelectedValue property.
			this.BlotterId = (Guid)this.SelectedValue;

		}

		/// <summary>
		/// Gets or sets the currently selected blotter.
		/// </summary>
		public Guid BlotterId
		{
			get
			{
				return (Guid)this.GetValue(BlotterComboBox.BlotterIdProperty);
			}
			set
			{
				this.SetValue(BlotterComboBox.BlotterIdProperty, value);
			}
		}

		/// <summary> 
		/// Gets the read-only collection of blotter items.
		/// </summary>
		public ListCollectionView BlotterItems
		{
			get
			{
				return this.GetValue(BlotterComboBox.BlotterItemsProperty) as ListCollectionView;
			}
		}

	}

}
