namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows.Data;

	/// <summary>
	/// A View that provides sorting, filtering and the evaluation of the Odd/Even property for the MVVM.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public class WorkingOrderCollectionView<TType> : ListCollectionView, IEnumerable<TType> where TType : WorkingOrder
	{

		/// <summary>
		/// Supports a simple iteration over a WorkingOrder collection.
		/// </summary>
		class WorkingOrderCollectionEnumerator : IEnumerator<TType>
		{

			/// <summary>
			/// The base enumerator.
			/// </summary>
			IEnumerator iEnumerator;

			/// <summary>
			/// Initializes a new instance of the WorkingOrderCollectionEnumerator class.
			/// </summary>
			/// <param name="enumerator"></param>
			public WorkingOrderCollectionEnumerator(IEnumerator enumerator)
			{

				// Initialize the object.
				this.iEnumerator = enumerator;

			}

			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public TType Current
			{
				get { return (TType)this.iEnumerator.Current; }
			}

			/// <summary>
			/// Gets the element in the collection at the current position of the enumerator.
			/// </summary>
			object IEnumerator.Current
			{
				get { return this.iEnumerator.Current; }
			}

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			public void Dispose() { }

			/// <summary>
			/// Advances the enumerator to the next element of the collection.
			/// </summary>
			/// <returns>
			/// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
			/// </returns>
			public bool MoveNext()
			{
				return this.iEnumerator.MoveNext();
			}

			/// <summary>
			/// Sets the enumerator to its initial position, which is before the first element in the collection.
			/// </summary>
			public void Reset()
			{
				this.iEnumerator.Reset();
			}

		}

		/// <summary>
		/// This provides the ability to have multiple filters on a view.
		/// </summary>
		ObservableCollection<ObservableCollection<Predicate<TType>>> filterList = new ObservableCollection<ObservableCollection<Predicate<TType>>>();

		/// <summary>
		/// Initialize a new instance of the WorkingOrderCollectionView class.
		/// </summary>
		/// <param name="workingOrderCollection">The raw collection of workingOrders.</param>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public WorkingOrderCollectionView(WorkingOrderCollection<TType> workingOrderCollection)
			: base(workingOrderCollection)
		{

			// This will evaluation several filters to see if a workingOrder should be in the view.
			this.Filter = this.ProcessFilterList;

			// This lets us know that a new filter has been added to the collection so we can refresh the view.
			this.filterList.CollectionChanged += this.OnFilterListChanged;

		}


		/// <summary>
		/// A collection of filter groups.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public ObservableCollection<ObservableCollection<Predicate<TType>>> Filters
		{
			get
			{
				return this.filterList;
			}
		}

		/// <summary>
		/// Supports a simple iteration over a collection of instances of the TType class.
		/// </summary>
		/// <returns></returns>
		IEnumerator<TType> IEnumerable<TType>.GetEnumerator()
		{
			return new WorkingOrderCollectionEnumerator(base.GetEnumerator());
		}

		/// <summary>
		/// Raises the CollectionChanged event.
		/// </summary>
		/// <param name="args">The NotifyCollectionChangedEventArgs object to pass to the event handler.</param>
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{

			// The base class has significant processing to do on this event.
			base.OnCollectionChanged(args);

			// This class will update the odd/even property of the workingOrders in the collection.  This is the only place that knows about the final order of the
			// records, so this is the only place a function like this can be placed.
			if (this.MoveCurrentToFirst())
				do
				{
					WorkingOrder workingOrder = this.CurrentItem as WorkingOrder;
					if (workingOrder != null)
					{
						this.EditItem(workingOrder);
						workingOrder.IsEven = this.CurrentPosition % 2 == 0 ? true : false;
						this.CommitEdit();
					}
				} while (this.MoveCurrentToNext());

		}

		/// <summary>
		/// Occurs when an item is added, removed, changed, moved, or the entire list is refreshed.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="notifyCollectionChangedEventArgs">Information about the event.</param>
		void OnFilterListChanged(Object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{

			// Handle each of the actions that occur when the filter changes.
			switch (notifyCollectionChangedEventArgs.Action)
			{
			case NotifyCollectionChangedAction.Add:

				// Each group that is added to the collection needs to refresh the view when the collection changes.
				foreach (ObservableCollection<Predicate<TType>> filterGroup in notifyCollectionChangedEventArgs.NewItems)
					filterGroup.CollectionChanged += this.OnFilterGroupCollectionChanged;
				break;

			case NotifyCollectionChangedAction.Remove:

				// This will extract the refresh event from the group when it is pulled out of the filter.
				foreach (ObservableCollection<Predicate<TType>> filterGroup in notifyCollectionChangedEventArgs.OldItems)
					filterGroup.CollectionChanged -= this.OnFilterGroupCollectionChanged;
				break;

			}

			// Refresh the view after any of the filter groups have been added or removed.
			this.Refresh();

		}

		/// <summary>
		/// Handles a change to any one of the filter groups.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="notifyCollectionChangedEventArgs">Information about the event.</param>
		void OnFilterGroupCollectionChanged(Object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{

			// Refresh the view when any of the filter groups contents have changed.
			this.Refresh();

		}

		/// <summary>
		/// Determine whether an item is suitable for inclusion in the view.
		/// </summary>
		/// <param name="item">The item to be tested.</param>
		/// <returns>true if the item is to be include in the view, false if not.</returns>
		Boolean ProcessFilterList(Object item)
		{

			// Only workingOrders can be tested against the strongly typed filters.
			TType workingOrder = (TType)item;
			if (workingOrder == null)
				return false;

			// If no filters are active then every item is available.  Otherwise the filters act like a logical 'Or' by including any item that passes any of the
			// filters.
			if (this.filterList.Count == 0)
				return true;

			// This will test the item against each of the filters.  If it passes any one of the filters it can be included in the view.  In this way the combined
			// filters act like a logical 'Or' by include any of the rows that pass this filter or that filter or any other filter.
			foreach (ObservableCollection<Predicate<TType>> filterGroup in this.filterList)
			{

				// It's possible to add empty filter groups to the list.  If this happens we'll assume that it's permissive until it's filled with filters.  This
				// will prevent the ItemsControls from removing and adding a boatload of items while the filter is being constructed.
				if (filterGroup.Count != 0)
				{

					// An item must pass at least one filter in the group to be visible.
					Boolean passedFilterGroup = false;
					foreach (Predicate<TType> filter in filterGroup)
						if (filter(workingOrder))
						{
							passedFilterGroup = true;
							break;
						}

					// Any item must pass all the filters in a given group to be visible.
					if (!passedFilterGroup)
						return false;

				}

			}

			// At this point the row has not passed any of the filteres defined for this view.
			return true;

		}

	}

}
