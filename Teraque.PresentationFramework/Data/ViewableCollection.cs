namespace Teraque.Windows.Data
{

	using System;
	using System.Collections;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Data;
	using Teraque.Properties;

	/// <summary>
	/// Holds the list of items that represent the content of a control.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.ComponentModel.IEditableCollectionView")]
	[SuppressMessage("Microsoft.Design", "CA1039:ListsAreStronglyTyped")]
	[SuppressMessage("Microsoft.Design", "CA1035:ICollectionImplementationsHaveStronglyTypedMembers")]
	public sealed class ViewableCollection :
		CollectionView,
		IList,
		IEditableCollectionView,
		IWeakEventListener
	{

		/// <summary>
		/// This is the current view of the collection and can refer to a direct collection or an indirect source for the collection.
		/// </summary>
		ICollectionView currentView;

		/// <summary>
		/// Contains the data added directly to this view.
		/// </summary>
		ObservableCollection<Object> items;

		/// <summary>
		/// The external source of data when not using the implicit (direct) collection.
		/// </summary>
		IEnumerable itemsSource;

		/// <summary>
		/// Handles a change to the collection.
		/// </summary>
		public new event NotifyCollectionChangedEventHandler CollectionChanged;

		/// <summary>
		/// Initializes a new instance of the ViewableCollection class.
		/// </summary>
		public ViewableCollection() : base(new ObservableCollection<Object>())
		{

			// Initialize the object.
			this.items = base.SourceCollection as ObservableCollection<Object>;

			// The direct collection provides the view for this collection by default.
			this.ICollectionView = CollectionViewSource.GetDefaultView(this.items);

			base.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnBaseCollectionChanged);

		}

		void OnBaseCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.CollectionChanged != null)
				this.CollectionChanged(sender, e);
		}

		/// <summary>
		/// Gets a value that indicates whether this collection view supports filtering.
		/// </summary>
		public override Boolean CanFilter
		{
			get
			{
				return this.currentView.CanFilter;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether this collection view supports grouping.
		/// </summary>
		public override Boolean CanGroup
		{
			get
			{
				return this.currentView.CanGroup;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether this collection view supports sorting.
		/// </summary>
		public override Boolean CanSort
		{
			get
			{
				return this.currentView.CanSort;
			}
		}

		/// <summary>
		/// Gets the number of records in the collection.
		/// </summary>
		public override Int32 Count
		{
			get
			{

				// See if there is a CollectionView class that can be used to return the number of items in the collection.
				CollectionView collectionView = this.currentView as CollectionView;
				if (collectionView != null)
					return collectionView.Count;

				// At this point there is no common base class that can be used to extract the number of elements.
				return 0;

			}
		}

		/// <summary>
		/// Gets the current item in the view.
		/// </summary>
		public override Object CurrentItem
		{
			get
			{
				return this.currentView.CurrentItem;
			}
		}

		/// <summary>
		/// Gets the ordinal position of the current item within the view.
		/// </summary>
		public override Int32 CurrentPosition
		{
			get
			{
				return this.currentView.CurrentPosition;
			}
		}

		/// <summary>
		/// Gets or sets a callback used to determine if an item is suitable for inclusion in the view.
		/// </summary>
		public override Predicate<Object> Filter
		{
			get
			{
				return this.currentView.Filter;
			}
			set
			{
				this.currentView.Filter = value;
			}
		}

		/// <summary>
		/// Gets a collection of GroupDescription objects that describes how the items in the collection are grouped in the view.
		/// </summary>
		public override ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return this.currentView.GroupDescriptions;
			}
		}

		/// <summary>
		/// Gets the top-level groups that are constructed according to the GroupDescriptions.
		/// </summary>
		public override ReadOnlyObservableCollection<Object> Groups
		{
			get
			{
				return this.currentView.Groups;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the current item of the view is beyond the end of the collection.
		/// </summary>
		public override Boolean IsCurrentAfterLast
		{
			get
			{
				return this.currentView.IsCurrentAfterLast;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the current item of the view is beyond the beginning of the collection.
		/// </summary>
		public override Boolean IsCurrentBeforeFirst
		{
			get
			{
				return this.currentView.IsCurrentBeforeFirst;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the resulting (filtered) view is empty.
		/// </summary>
		public override Boolean IsEmpty
		{
			get
			{
				return this.currentView.IsEmpty;
			}
		}

		/// <summary>
		/// Gets or sets the item at the given zero-based index.
		/// </summary>
		/// <param name="index">The zero-based index of the item.</param>
		/// <returns>The object retrieved or the object that is being set to the specified index.</returns>
		public Object this[Int32 index]
		{

			get
			{
				return this.GetItemAt(index);
			}

			set
			{

				// Throw an exception when using the indirect collection.
				if (this.itemsSource != null)
					throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.ItemsSourceInUse));

				// This insures that the index is within the array bounds.
				if (index < 0 || index >= this.items.Count)
					throw new ArgumentOutOfRangeException("index");

				// Set the item at the given index to the given value.
				this.items[index] = value;

			}

		}

		/// <summary>
		/// Gets or sets the items source collection.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal IEnumerable ItemsSource
		{

			get
			{
				return this.itemsSource;
			}

			set
			{

				// Don't allow an items source to be set if there's an active and populated direct collection.
				if (this.itemsSource == null && this.items.Count > 0)
					throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.CannotUseItemsSource));

				// This switches the mode of the view to be indirect.  The collection becomes essentially a read-only collection.
				this.itemsSource = value;

				// The default view on the indirect collection now becomes the public face of this object.
				this.ICollectionView = CollectionViewSource.GetDefaultView(this.itemsSource);

				// Changing the source of the collection needs to trigger a 'CollectionChanged' event so consumers know that something fundamental has changed.
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

				// The collection needs to be refreshed when the source has changed.
				this.Refresh();

			}

		}

		/// <summary>
		/// Gets a collection of SortDescription objects that describe how the items in the collection are sorted in the view. 
		/// </summary>
		public override SortDescriptionCollection SortDescriptions
		{
			get
			{
				return this.currentView.SortDescriptions;
			}
		}

		/// <summary>
		/// Gets the unsorted and unfiltered collection that underlies this collection view.
		/// </summary>
		public override IEnumerable SourceCollection
		{
			get
			{
				return this.itemsSource == null ? this.items : this.itemsSource;
			}
		}

		/// <summary>
		/// This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.
		/// </summary>
		Boolean ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.
		/// </summary>
		Object ICollection.SyncRoot
		{
			get
			{
				if (this.itemsSource != null)
					throw new NotSupportedException(ExceptionMessage.Format(ExceptionMessages.ItemCollectionShouldUseInnerSyncRoot));
				return this.items;
			}
		}

		/// <summary>
		/// This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.
		/// </summary>
		Boolean IList.IsFixedSize
		{
			get
			{
				return this.itemsSource != null;
			}
		}

		/// <summary>
		/// This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.
		/// </summary>
		Boolean IList.IsReadOnly
		{
			get
			{
				return this.itemsSource != null;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether a new item can be added to the collection.
		/// </summary>
		Boolean IEditableCollectionView.CanAddNew
		{
			get
			{
				IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
				return iEditableCollectionView != null && iEditableCollectionView.CanAddNew;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the collection view can discard pending changes and restore the original values of an edited object.
		/// </summary>
		Boolean IEditableCollectionView.CanCancelEdit
		{
			get
			{
				IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
				return iEditableCollectionView != null && iEditableCollectionView.CanCancelEdit;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether an item can be removed from the collection.
		/// </summary>
		Boolean IEditableCollectionView.CanRemove
		{
			get
			{
				IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
				return iEditableCollectionView != null && iEditableCollectionView.CanRemove;
			}
		}

		/// <summary>
		/// Gets the item that is being added during the current add transaction.
		/// </summary>
		Object IEditableCollectionView.CurrentAddItem
		{
			get
			{
				IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
				return iEditableCollectionView == null ? null : iEditableCollectionView.CurrentAddItem;
			}
		}

		/// <summary>
		/// Gets the item in the collection that is being edited.
		/// </summary>
		Object IEditableCollectionView.CurrentEditItem
		{
			get
			{
				IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
				return iEditableCollectionView == null ? null : iEditableCollectionView.CurrentEditItem;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether an add transaction is in progress.
		/// </summary>
		Boolean IEditableCollectionView.IsAddingNew
		{
			get
			{
				IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
				return iEditableCollectionView != null && iEditableCollectionView.IsAddingNew;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether an edit transaction is in progress.
		/// </summary>
		Boolean IEditableCollectionView.IsEditingItem
		{
			get
			{
				IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
				return iEditableCollectionView != null && iEditableCollectionView.IsEditingItem;
			}
		}

		/// <summary>
		/// Gets or sets the position of the new item placeholder in the collection view.
		/// </summary>
		NewItemPlaceholderPosition IEditableCollectionView.NewItemPlaceholderPosition
		{
			get
			{
				IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
				return iEditableCollectionView == null ? NewItemPlaceholderPosition.None : iEditableCollectionView.NewItemPlaceholderPosition;
			}
			set
			{
				IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
				if (iEditableCollectionView == null)
					throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.MemberNotAllowedForView, "NewItemPlaceholderPosition"));
				iEditableCollectionView.NewItemPlaceholderPosition = value;
			}
		}

		/// <summary>
		/// Gets or sets the culture information to use during sorting.
		/// </summary>
		[TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
		public override CultureInfo Culture
		{
			get
			{
				return this.currentView.Culture;
			}
			set
			{
				this.currentView.Culture = value;
			}
		}

		/// <summary>
		/// Adds an item to the IList.
		/// </summary>
		/// <param name="value">The object to add to the IList.</param>
		/// <returns>The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the collection.</returns>
		public Int32 Add(Object value)
		{

			// Throw an exception when using the indirect collection.
			if (this.itemsSource != null)
				throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.ItemsSourceInUse));

			// This will add the item to the direct collection managed by this wrapper.
			this.items.Add(value);
			return this.items.Count - 1;

		}

		/// <summary>
		/// Clears the collection and releases the references on all items currently in the collection.
		/// </summary>
		public void Clear()
		{

			// This operation is illegal in the indirect mode.
			if (this.itemsSource != null)
				throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.ItemsSourceInUse));

			// As long as the above conditions are met it's alright to clear out the base collection.
			if (this.items != null)
				this.items.Clear();

		}

		/// <summary>
		/// Resets the collection to use the direct mode.
		/// </summary>
		internal void ClearItemsSource()
		{

			// When the source for the view is changed back to the direct mode the external collection must be unhooked from the view and the internal view needs to
			// be hooked back in.
			if (this.itemsSource != null)
			{
				this.itemsSource = null;
				this.ICollectionView = CollectionViewSource.GetDefaultView(this.items);
			}

		}

		/// <summary>
		/// Returns a value that indicates whether the specified item is in this view.
		/// </summary>
		/// <param name="item">The object to check.</param>
		/// <returns>true to indicate that the item belongs to this collection and passes the active filter; otherwise, false.</returns>
		public override Boolean Contains(Object item)
		{

			// This operation can't be accomplished when waiting for a refresh.

			// As long as the above conditions have been met, the current view can be queried for the item.
			return this.currentView.Contains(item);

		}

		/// <summary>
		/// Copies the elements of the collection to an array, starting at a particular array index.
		/// </summary>
		/// <param name="array">The destination array to copy to.</param>
		/// <param name="index">The zero-based index in the destination array.</param>
		public void CopyTo(Array array, Int32 index)
		{

			// Don't allow an empty array to be copied.
			if (array == null)
				throw new ArgumentNullException("array");

			// Only 1 dimensional (vector) arrays can be copied.
			if (array.Rank > 1)
				throw new ArgumentException(ExceptionMessage.Format(ExceptionMessages.BadTargetArray));

			// Insure that the start index is valid.
			if (index < 0)
				throw new ArgumentOutOfRangeException("index");

			// The operation can't be done while waiting for a refresh.

			// An indexable collection can be copied directly.  An IEnumerable collection requires a little more work to construct and use an iterator.
			ICollection iCollection = this.currentView as ICollection;
			if (iCollection != null)
				iCollection.CopyTo(array, index);
			else
			{

				// This will enumerate through the collection when it's based on IEnumerable and copy elements into the destination array.
				IEnumerable iEnumerable = this.currentView as IEnumerable;
				IEnumerator enumerator = iEnumerable.GetEnumerator();
				IList iList = array as IList;
				while (enumerator.MoveNext())
				{

					// There's no way to perform this check outside of the enumerator as we don't know how many items are in the IEnumerable collection.
					if (index >= array.Length)
						throw new ArgumentException(ExceptionMessage.Format(ExceptionMessages.CopyToNotEnoughSpace));

					// This will copy the next element out of the current collection's view and into the given array.
					iList[index++] = enumerator.Current;

				}
			}

		}

		/// <summary>
		/// Enters a defer cycle that you can use to merge changes to the view and delay automatic refresh.
		/// </summary>
		/// <returns>An IDisposable object that you can use to dispose of the calling object.</returns>
		public override IDisposable DeferRefresh()
		{

			// Allow the current view to handle the deferrals.
			return this.currentView.DeferRefresh();

		}

		/// <summary>
		/// Returns an object that you can use to enumerate the items in the view.
		/// </summary>
		/// <returns>An IEnumerator object that you can use to enumerate the items in the view.</returns>
		protected override IEnumerator GetEnumerator()
		{

			// Use the current collection if it has been initialized with a view otherwise an empty enumerator will do.
			return this.currentView.GetEnumerator();

		}

		/// <summary>
		/// Returns the item at the specified zero-based index in this view.
		/// </summary>
		/// <param name="index">The zero-based index at which the item is located.</param>
		/// <returns>The item at the specified zero-based index in this view.</returns>
		public override Object GetItemAt(Int32 index)
		{

			// Make sure the index is within range.  For performance reasons there is no check for the upper bounds of an IEnumerable collection.
			if (index < 0)
				throw new ArgumentOutOfRangeException("index");

			// See if we can extract an item from a collection view.
			CollectionView collectionView = this.currentView as CollectionView;
			if (collectionView != null)
				return collectionView.GetItemAt(index);

			// See if there is a ViewableCollection class that can be used to return the number of items in the collection.
			ViewableCollection viewableCollection = this.currentView as ViewableCollection;
			if (viewableCollection != null)
				return viewableCollection.GetItemAt(index);

			// At this point, we can't extract an item from the current collection view.
			return null;

		}

		/// <summary>
		/// Hooke the CollectionView into the current view.
		/// </summary>
		/// <param name="iCollectionView">The collection that is to become the underlying connection for this view.</param>
		void HookCollectionView(ICollectionView iCollectionView)
		{

			// Hook the class into the weak listeners.  The event handler will rebroadcast the events in the underlying collections to listeners of this object as 
			// if they were generated from this class.
			CollectionChangedEventManager.AddListener(iCollectionView, this);
			CurrentChangingEventManager.AddListener(iCollectionView, this);
			CurrentChangedEventManager.AddListener(iCollectionView, this);

			// If the collection view has an opportunity to notify listeners when the property has changed then add it to the weak listener.
			INotifyPropertyChanged iNotifyPropertyChanged = this.currentView as INotifyPropertyChanged;
			if (iNotifyPropertyChanged != null)
				PropertyChangedEventManager.AddListener(iNotifyPropertyChanged, this, String.Empty);

		}

		/// <summary>
		/// Returns the index in this collection where the specified item is located.
		/// </summary>
		/// <param name="value">The object to look for in the collection.</param>
		/// <returns>The index of the item in the collection, or -1 if the item does not exist in the collection.</returns>
		public override Int32 IndexOf(Object value)
		{

			// As long as the conditions above are met then use the current collection to return the item.
			CollectionView collectionView = this.currentView as CollectionView;
			if (collectionView != null)
				return collectionView.IndexOf(value);

			// If there's no base class that can return an 'IndexOf' operation, then the item can't be found in this collection.
			return -1;

		}

		/// <summary>
		/// Inserts an element into the collection at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which to insert the item.</param>
		/// <param name="value">The item to insert.</param>
		public void Insert(Int32 index, Object value)
		{

			// Throw an exception when using the indirect collection.
			if (this.itemsSource != null)
				throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.ItemsSourceInUse));

			// Insert the item into the direct collection.
			this.items.Insert(index, value);
			this.Refresh();

		}

		/// <summary>
		/// Sets the specified item in the collection as the CurrentItem.
		/// </summary>
		/// <param name="item">The item to set as the CurrentItem.</param>
		/// <returns>true to indicate that the resulting CurrentItem is an item within the view; otherwise, false.</returns>
		public override Boolean MoveCurrentTo(Object item)
		{

			// Move the current item in the current view to the given item.
			return this.currentView.MoveCurrentTo(item);

		}

		/// <summary>
		/// Sets the first item in the view as the CurrentItem.
		/// </summary>
		/// <returns>true to indicate that the resulting CurrentItem is an item within the view; otherwise, false.</returns>
		public override Boolean MoveCurrentToFirst()
		{

			// Move the current item in the current view to the first record.
			return this.currentView.MoveCurrentToFirst();

		}

		/// <summary>
		/// Sets the last item in the view as the CurrentItem.
		/// </summary>
		/// <returns>true to indicate that the resulting CurrentItem is an item within the view; otherwise, false.</returns>
		public override Boolean MoveCurrentToLast()
		{

			// Move the current item in the current view to the last record.
			return this.currentView.MoveCurrentToLast();

		}

		/// <summary>
		/// Sets the item after the CurrentItem in the view as the CurrentItem.
		/// </summary>
		/// <returns>true to indicate that the resulting CurrentItem is an item within the view; otherwise, false.</returns>
		public override Boolean MoveCurrentToNext()
		{

			// Move the current item in the current view to the record after the current record.
			return this.currentView.MoveCurrentToNext();

		}

		/// <summary>
		/// Sets the item at the specified index to be the CurrentItem in the view.
		/// </summary>
		/// <param name="position">The zero-based index of the item to set as the CurrentItem.</param>
		/// <returns>true to indicate that the resulting CurrentItem is an item within the view; otherwise, false.</returns>
		public override Boolean MoveCurrentToPosition(Int32 position)
		{

			// Move the current item in the current view to the absolute position given.
			return this.currentView.MoveCurrentToPosition(position);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override Boolean MoveCurrentToPrevious()
		{

			// Move the current item in the current view to the absolute position given.
			return this.currentView.MoveCurrentToPrevious();

		}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="notifyCollectionChangedEventArgs"></param>
		//protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		//{

		//    if (this.CollectionChanged != null)
		//        this.CollectionChanged(this, notifyCollectionChangedEventArgs);

		//    base.OnCollectionChanged(notifyCollectionChangedEventArgs);

		//}

		/// <summary>
		/// Returns a value that indicates whether the specified item belongs to this view.
		/// </summary>
		/// <param name="item">The object to test.</param>
		/// <returns>true to indicate that the specified item belongs to this view or there is no filter set on this collection view; otherwise, false.</returns>
		public override Boolean PassesFilter(Object item)
		{

			// Test whether the value in the inner collection can pass the filters placed on it.
			CollectionView collectionView = this.currentView as CollectionView;
			if (collectionView != null)
				return collectionView.PassesFilter(item);

			// See if there is a ViewableCollection class that can be used to return the number of items in the collection.
			ViewableCollection viewableCollection = this.currentView as ViewableCollection;
			if (viewableCollection != null)
				return viewableCollection.PassesFilter(item);

			// If there is no class available to check the filter, then the filter operation will fail.
			return false;

		}

		/// <summary>
		/// Re-creates the view.
		/// </summary>
		public override void Refresh()
		{

			// The refresh logic is handled by the current view.
			this.currentView.Refresh();

		}

		/// <summary>
		/// Removes the specified item reference from the collection or view.
		/// </summary>
		/// <param name="value">The object to remove.</param>
		public void Remove(Object value)
		{

			// Throw an exception when using the indirect collection.
			if (this.itemsSource != null)
				throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.ItemsSourceInUse));

			// Remove the item from the inner collection and refresh the view.
			this.items.Remove(value);

		}

		/// <summary>
		/// Removes the item at the specified index of the collection or view.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		public void RemoveAt(Int32 index)
		{

			// Throw an exception when using the indirect collection.
			if (this.itemsSource != null)
				throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.ItemsSourceInUse));

			// Remove the item from the inner collection and update the view.
			this.items.RemoveAt(index);

		}

		/// <summary>
		/// Gets or sets the current CollectionView for this object.
		/// </summary>
		ICollectionView ICollectionView
		{

			set
			{

				// This will prevent redundant efforts to set the collection to use a given view.
				if (this.currentView != value)
				{

					// This will unhook the previous collection and eliminate any deferred refreshes.
					if (this.currentView != null)
						using (this.currentView.DeferRefresh())
							this.UnhookCollectionView(this.currentView);

					// The new value becomes the current view.
					this.currentView = value;

					// This will hook the new view into the event listeners and defer any refresh until the operation is completed.
					if (this.currentView != null)
						using (this.currentView.DeferRefresh())
							this.HookCollectionView(this.currentView);

				}

			}

		}

		/// <summary>
		/// Adds a new item to the collection.
		/// </summary>
		/// <returns></returns>
		Object IEditableCollectionView.AddNew()
		{
			IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
			if (iEditableCollectionView == null)
				throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.MemberNotAllowedForView, "AddNew"));
			return iEditableCollectionView.AddNew();
		}

		/// <summary>
		/// Ends the edit transaction and, if possible, restores the original value to the item.
		/// </summary>
		void IEditableCollectionView.CancelEdit()
		{
			IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
			if (iEditableCollectionView == null)
				throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.MemberNotAllowedForView, "CancelEdit"));
			iEditableCollectionView.CancelEdit();
		}

		/// <summary>
		/// Ends the add transaction and discards the pending new item.
		/// </summary>
		void IEditableCollectionView.CancelNew()
		{
			IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
			if (iEditableCollectionView == null)
				throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.MemberNotAllowedForView, "CancelNew"));
			iEditableCollectionView.CancelNew();
		}

		/// <summary>
		/// Ends the edit transaction and saves the pending changes.
		/// </summary>
		void IEditableCollectionView.CommitEdit()
		{
			IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
			if (iEditableCollectionView == null)
				throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.MemberNotAllowedForView, "CommitEdit"));
			iEditableCollectionView.CommitEdit();
		}

		/// <summary>
		/// Ends the add transaction and saves the pending new item.
		/// </summary>
		void IEditableCollectionView.CommitNew()
		{
			IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
			if (iEditableCollectionView == null)
				throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.MemberNotAllowedForView, "CommitNew"));
			iEditableCollectionView.CommitNew();
		}

		/// <summary>
		/// Begins an edit transaction of the specified item.
		/// </summary>
		/// <param name="item">The item to edit.</param>
		void IEditableCollectionView.EditItem(Object item)
		{
			IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
			if (iEditableCollectionView == null)
				throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.MemberNotAllowedForView, "EditItem"));
			iEditableCollectionView.EditItem(item);
		}

		/// <summary>
		/// Removes the specified item from the collection.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		void IEditableCollectionView.Remove(Object item)
		{
			IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
			if (iEditableCollectionView == null)
				throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.MemberNotAllowedForView, "Remove"));
			iEditableCollectionView.Remove(item);
		}

		/// <summary>
		/// Removes the item at the specified position from the collection.
		/// </summary>
		/// <param name="index">The position of the item to remove.</param>
		void IEditableCollectionView.RemoveAt(Int32 index)
		{
			IEditableCollectionView iEditableCollectionView = this.currentView as IEditableCollectionView;
			if (iEditableCollectionView == null)
				throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.MemberNotAllowedForView, "RemoveAt"));
			iEditableCollectionView.RemoveAt(index);
		}

		/// <summary>
		/// Receives events from the centralized event manager.
		/// </summary>
		/// <param name="managerType">The type of the WeakEventManager calling this method.</param>
		/// <param name="sender">Object that originated the event.</param>
		/// <param name="eventArgs">Event data.</param>
		/// <returns>true if the listener handled the event. It is considered an error by the WeakEventManager handling in WPF to register a listener for an event
		/// that the listener does not handle. Regardless, the method should return false if it receives an event that it does not recognize or handle.</returns>
		Boolean IWeakEventListener.ReceiveWeakEvent(Type managerType, Object sender, EventArgs eventArgs)
		{

			// Turn the weak event into a proper PropertyChanged notification.
			if (managerType == typeof(PropertyChangedEventManager))
				this.OnPropertyChanged(eventArgs as PropertyChangedEventArgs);

			// This will handle a change made to the collection.
			if (managerType == typeof(CollectionChangedEventManager))
				base.OnCollectionChanged(eventArgs as NotifyCollectionChangedEventArgs);

			// Turn the weak event into a proper CurrentChanging event.
			if (managerType == typeof(CurrentChangingEventManager))
				this.OnCurrentChanging(eventArgs as CurrentChangingEventArgs);

			// Turn the weak event into a proper CurrentChanged event.
			if (managerType == typeof(CurrentChangedEventManager))
				this.OnCurrentChanged();

			// By virtue of the fact that handlers are provided for all the event types to which we've subscribed, the only possible response is to indicate that
			// we've handled the event.
			return true;

		}

		/// <summary>
		/// Unhooks the current view from the weak listeners.
		/// </summary>
		/// <param name="iCollectionView">The view to be removed from the hooks.</param>
		void UnhookCollectionView(ICollectionView iCollectionView)
		{

			// The events of the given CollectionView are no longer of any interest to the public view of this object.
			CollectionChangedEventManager.RemoveListener(iCollectionView, this);
			CurrentChangingEventManager.RemoveListener(iCollectionView, this);
			CurrentChangedEventManager.RemoveListener(iCollectionView, this);

			// If the collection view had an opportunity to notify listeners when the property has changed then remove it to the weak listener.
			INotifyPropertyChanged iNotifyPropertyChanged = this.currentView as INotifyPropertyChanged;
			if (iNotifyPropertyChanged != null)
				PropertyChangedEventManager.RemoveListener(iNotifyPropertyChanged, this, String.Empty);

		}

	}

}