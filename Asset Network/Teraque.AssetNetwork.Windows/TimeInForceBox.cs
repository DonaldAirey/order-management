namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;

	/// <summary>
	/// A ComboBox used to select a TimeInForce (DAY, GTC, CLO) for an order.
	/// </summary>
	public class TimeInForceBox : ComboBox
	{

		/// <summary>
		/// An ordered view of the timeInForce codes.
		/// </summary>
		static ListCollectionView listCollectionView;

		/// <summary>
		/// A collection of TimeInForce items.
		/// </summary>
		static TimeInForceCollection timeInForceCollection = new TimeInForceCollection();

		/// <summary>
		/// Identifies the TimeInForceItems dependency property key.
		/// </summary>
		static readonly DependencyPropertyKey timeInForceItemsPropertyKey = DependencyProperty.RegisterReadOnly(
				"TimeInForceItems",
				typeof(ListCollectionView),
				typeof(TimeInForceBox),
				new FrameworkPropertyMetadata());

		/// <summary>
		/// Identifies the TimeInForceItems dependency property.
		/// </summary>
		public readonly static DependencyProperty TimeInForceItemsProperty;

		/// <summary>
		/// Initialize the TimeInForceBox class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static TimeInForceBox()
		{

			// Initialize the read-only properties using their keys.
			TimeInForceBox.TimeInForceItemsProperty = TimeInForceBox.timeInForceItemsPropertyKey.DependencyProperty;

			// This creates an association with an implicit default style in the themes.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeInForceBox), new FrameworkPropertyMetadata(typeof(TimeInForceBox)));

			// This will order the items in the drop down list alphabetically.
			TimeInForceBox.listCollectionView = CollectionViewSource.GetDefaultView(TimeInForceBox.timeInForceCollection) as ListCollectionView;
			TimeInForceBox.listCollectionView.SortDescriptions.Add(new SortDescription("SortOrder", ListSortDirection.Ascending));

		}

		/// <summary>
		/// Initializes a new instance of the TimeInForceBox class.
		/// </summary>
		public TimeInForceBox()
		{

			// Provide the combobox with a set of static timeInForce items.  The collection of timeInForce items can be changed by the database and those changes 
			// will be reflected in the list, but there is only one list needed for all the controls that consume this combobox, so we use a static list, which is
			// not to say the items in the list are static.
			this.SetValue(TimeInForceBox.timeInForceItemsPropertyKey, TimeInForceBox.listCollectionView);

		}

		/// <summary> 
		/// Gets the collection of timeInForce items.
		/// </summary>
		public ListCollectionView TimeInForceItems
		{
			get
			{
				return this.GetValue(TimeInForceBox.TimeInForceItemsProperty) as ListCollectionView;
			}
		}

	}

}
