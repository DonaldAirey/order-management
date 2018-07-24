namespace Teraque.AssetNetwork.Windows.Controls
{

	using System;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Data;
	using Teraque.Windows.Controls;

	/// <summary>
	/// A simple sector of a StatusItem.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class StatusSelector : ItemSelector
	{

		/// <summary>
		/// An ordered view of the status codes.
		/// </summary>
		static ListCollectionView listCollectionView;

		/// <summary>
		/// A collection of StatusItems.
		/// </summary>
		static StatusCollection statusCollection = new StatusCollection();
	
		/// <summary>
		/// Identifies the StatusItems dependency property key.
		/// </summary>
		static readonly DependencyPropertyKey statusItemsPropertyKey = DependencyProperty.RegisterReadOnly(
				"StatusItems",
				typeof(ListCollectionView),
				typeof(StatusSelector),
				new FrameworkPropertyMetadata());

		/// <summary>
		/// Identifies the StatusItems dependency property.
		/// </summary>
		public readonly static DependencyProperty StatusItemsProperty;

		/// <summary>
		/// Initialize the StatusBox class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static StatusSelector()
		{

			// Initialize the read-only properties using their keys.
			StatusSelector.StatusItemsProperty = StatusSelector.statusItemsPropertyKey.DependencyProperty;

			// This creates an association with an implicit default style in the themes.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusSelector), new FrameworkPropertyMetadata(typeof(StatusSelector)));

			// This will initialize the ordered, grouped and filtered view of the StatusItem collection.
			StatusSelector.listCollectionView = CollectionViewSource.GetDefaultView(StatusSelector.statusCollection) as ListCollectionView;
			StatusSelector.listCollectionView.SortDescriptions.Add(new SortDescription("SortOrder", ListSortDirection.Ascending));

		}

		/// <summary>
		/// Initializes a new instance of the StatusBox class.
		/// </summary>
		public StatusSelector()
		{

			// Provide the selector with a set of static status items.  The collection of status items can be changed by the database and those changes will be
			// reflected in the list, but there is only one list needed for all the controls that consume this selector, so we use a static list, which is not to
			// say the items in the list are static.
			this.SetValue(StatusSelector.statusItemsPropertyKey, StatusSelector.listCollectionView);

			// By default, the Status selector will bind to it's own collection of status codes.
			Binding itemSourceBinding = new Binding("StatusItems");
			itemSourceBinding.RelativeSource = new RelativeSource(RelativeSourceMode.Self);
			BindingOperations.SetBinding(this, StatusSelector.ItemsSourceProperty, itemSourceBinding);

			// The 'StatusCode' property is used to define the value of this selector.
			this.SelectedValuePath = "StatusCode";
			Binding selectedValueBinding = new Binding("StatusCode");
			BindingOperations.SetBinding(this, StatusSelector.SelectedValueProperty, selectedValueBinding);

		}

		/// <summary> 
		/// Gets the collection of status items.
		/// </summary>
		public ListCollectionView StatusItems
		{
			get
			{
				return this.GetValue(StatusSelector.StatusItemsProperty) as ListCollectionView;
			}
		}

	}

}
