namespace Teraque.AssetNetwork.Windows.Controls
{

	using System;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;

	/// <summary>
	/// A ComboBox used to select a Side (buy, sell, buy cover, sell short) for an order.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class SideComboBox : ComboBox
	{

		/// <summary>
		/// An ordered view of the SideItems.
		/// </summary>
		static ListCollectionView listCollectionView;

		/// <summary>
		/// A collection of side items.
		/// </summary>
		static SideCollection sideCollection = new SideCollection();

		/// <summary>
		/// Identifies the SideItems dependency property key.
		/// </summary>
		static readonly DependencyPropertyKey sideItemsPropertyKey = DependencyProperty.RegisterReadOnly(
				"SideItems",
				typeof(ListCollectionView),
				typeof(SideComboBox),
				new FrameworkPropertyMetadata());

		/// <summary>
		/// Identifies the SideItems dependency property.
		/// </summary>
		public readonly static DependencyProperty SideItemsProperty;

		/// <summary>
		/// Initialize the SideBox class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static SideComboBox()
		{

			// Initialize the read-only properties using their keys.
			SideComboBox.SideItemsProperty = SideComboBox.sideItemsPropertyKey.DependencyProperty;

			// This creates an association with an implicit default style in the themes.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(SideComboBox), new FrameworkPropertyMetadata(typeof(SideComboBox)));

			// This will order the items in the drop down list alphabetically.
			SideComboBox.listCollectionView = CollectionViewSource.GetDefaultView(SideComboBox.sideCollection) as ListCollectionView;
			SideComboBox.listCollectionView.SortDescriptions.Add(new SortDescription("SortOrder", ListSortDirection.Ascending));

		}

		/// <summary>
		/// Initializes a new instance of the SideBox class.
		/// </summary>
		public SideComboBox()
		{

			// Provide the combobox with a set of static side items.  The collection of side items can be changed by the database and those changes will be
			// reflected in the list, but there is only one list needed for all the controls that consume this combobox, so we use a static list, which is not to
			// say the items in the list are static.
			this.SetValue(SideComboBox.sideItemsPropertyKey, SideComboBox.listCollectionView);

		}

		/// <summary> 
		/// Gets the collection of side items.
		/// </summary>
		public ListCollectionView SideItems
		{
			get
			{
				return this.GetValue(SideComboBox.SideItemsProperty) as ListCollectionView;
			}
		}

	}

}
