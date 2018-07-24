namespace Teraque.Windows
{

	using System;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.Windows.Data;

	/// <summary>
	/// A View that provides sorting, filtering and the evaluation of the Odd/Even property for the MVVM.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class ConsumerView : ListCollectionView
	{

		/// <summary>
		/// This provides the ability to have multiple filters on a view.
		/// </summary>
		ObservableCollection<ObservableCollection<Predicate<Consumer>>> filterList = new ObservableCollection<ObservableCollection<Predicate<Consumer>>>();

		/// <summary>
		/// Initialize a new instance of the ConsumerView class.
		/// </summary>
		/// <param name="consumerCollection">The raw collection of consumers.</param>
		public ConsumerView(ConsumerCollection consumerCollection)
			: base(consumerCollection)
		{

			// This will evaluation several filters to see if a consumer should be in the view.
			this.Filter = this.ProcessFilterList;

			// This lets us know that a new filter has been added to the collection so we can refresh the view.
			this.filterList.CollectionChanged += this.OnFilterListChanged;

		}


		/// <summary>
		/// Occurs when an item is added, removed, changed, moved, or the entire list is refreshed.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="notifyCollectionChangedEventArgs">Information about the event.</param>
		void OnFilterListChanged(Object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{

			switch (notifyCollectionChangedEventArgs.Action)
			{
			case NotifyCollectionChangedAction.Add:

				// Each group that is added to the collection needs to refresh the view when the collection changes.
				foreach (ObservableCollection<Predicate<Consumer>> filterGroup in notifyCollectionChangedEventArgs.NewItems)
					filterGroup.CollectionChanged += this.OnFilterGroupCollectionChanged;
				break;

			case NotifyCollectionChangedAction.Remove:

				// This will extract the refresh event from the group when it is pulled out of the filter.
				foreach (ObservableCollection<Predicate<Consumer>> filterGroup in notifyCollectionChangedEventArgs.OldItems)
					filterGroup.CollectionChanged -= this.OnFilterGroupCollectionChanged;
				break;

			}

			// Refresh the view after any of the groups have been added or removed.
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
		/// Raises the CollectionChanged event.
		/// </summary>
		/// <param name="args">The NotifyCollectionChangedEventArgs object to pass to the event handler.</param>
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{

			// The base class has significant processing to do on this event.
			base.OnCollectionChanged(args);

			// This class will update the odd/even property of the consumers in the collection.  This is the only place that knows about the final order of the
			// records, so this is the only place a function like this can be placed.
			if (this.MoveCurrentToFirst())
				do
				{
					Consumer consumer = this.CurrentItem as Consumer;
					consumer.IsEven = this.CurrentPosition % 2 == 0 ? true : false;
				} while (this.MoveCurrentToNext());

		}

		/// <summary>
		/// Determine whether an item is suitable for inclusion in the view.
		/// </summary>
		/// <param name="item">The item to be tested.</param>
		/// <returns>true if the item is to be include in the view, false if not.</returns>
		Boolean ProcessFilterList(Object item)
		{

			// Only consumers can be tested against the strongly typed filters.
			Consumer consumer = item as Consumer;
			if (consumer == null)
				return false;

			// If no filters are active then every item is available.  Otherwise the filters act like a logical 'Or' by including any item that passes any of the
			// filters.
			if (this.filterList.Count == 0)
				return true;

			// This will test the item against each of the filters.  If it passes any one of the filters it can be included in the view.  In this way the combined
			// filters act like a logical 'Or' by include any of the rows that pass this filter or that filter or any other filter.
			foreach (ObservableCollection<Predicate<Consumer>> filterGroup in this.filterList)
			{

				if (filterGroup.Count == 0)
					continue;

				Boolean passedFilterGroup = false;
				foreach (Predicate<Consumer> filter in filterGroup)
					if (filter(consumer))
					{
						passedFilterGroup = true;
						break;
					}

				if (!passedFilterGroup)
					return false;

			}

			// At this point the row has not passed any of the filteres defined for this view.
			return true;

		}

		/// <summary>
		/// A collection of filter groups.
		/// </summary>
		public ObservableCollection<ObservableCollection<Predicate<Consumer>>> Filters
		{
			get
			{
				return this.filterList;
			}
		}

	}

}
