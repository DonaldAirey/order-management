namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Globalization;
	using System.Windows;

	/// <summary>
	/// A collection of ColumnViewColumns.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ColumnViewColumnCollection : ObservableCollection<ColumnViewColumn>
	{

		/// <summary>
		/// The number of visible columns in the collection.
		/// </summary>
		Int32 visibleCount = 0;

		/// <summary>
		/// Removes all items from the collection.
		/// </summary>
		protected override void ClearItems()
		{

			// Remove the event handlers from each of the items before they're removed from the collection.  Failure to do so will leave resources hanging around
			// after they are no longer needed.
			foreach (ColumnViewColumn columnViewColumn in this)
				columnViewColumn.PropertyChanged -= this.OnItemPropertyChanged;

			// Allow the base class to finish clearing the collection.
			base.ClearItems();

		}

		/// <summary>
		/// Inserts an item into the collection at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="item">The object to insert.</param>
		protected override void InsertItem(Int32 index, ColumnViewColumn item)
		{

			// Validate the parameters.
			if (item == null)
				throw new ArgumentNullException("item");

			// The items in this collection will change position based on whether they are visible or not.  This makes it possible to have a view based on the
			// visible items and leave the hidden ones around so they can be managed and added to the view as needed.  When the 'IsVisible' property of a ColumnViewColumn
			// is changed, the collection is changed also.
			item.PropertyChanged += this.OnItemPropertyChanged;

			// Insert the visible items sequentially at the start of the list.  The hidden items are added alphabetically after the visible items.
			if (item.IsVisible)
				base.InsertItem(this.visibleCount++, item);
			else
			{
				index = this.BinarySearch(item);
				if (index < 0)
					base.InsertItem(~index, item);
			}

		}

		/// <summary>
		/// Handles a change to the properties of the items in the collection.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="propertyChangedEventArgs">The property change event data.</param>
		void OnItemPropertyChanged(Object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{

			// Changing the visiblity of the item will change its position in the list and the number of visible items in the collection.  These changes are
			// propogated to the consumers of this collection as move operations.  A presenter which is based on this collection will then need to reconcile the
			// visible objects with the object in this collection.  For exmple, setting a hidden item's 'IsVisible' property to true will move it into the area of
			// the list with the other visible objects.  The header presenter must create a header for this object and place it in the control at the equivalent
			// position to it's place in the list.
			if (propertyChangedEventArgs.PropertyName == "IsVisible")
			{

				// This will extract the ColumnViewColumn that generated this event from the generic parameters.
				ColumnViewColumn columnViewColumn = sender as ColumnViewColumn;

				// We're going to move this item.  Get the old index from the current list.
				Int32 oldIndex = this.IndexOf(columnViewColumn);

				// Now, calculate where the item should be in the visible list.  If the column has an 'ordinal' value associated with it, that ordinal value
				// determines its location.  Otherwise it is appended at the end of the visible columns.
				Int32 ordinalIndex = this.visibleCount;
				if (columnViewColumn.Ordinal.HasValue)
				{
					for (Int32 index = 0; index < this.visibleCount; index++)
						if (!this[index].Ordinal.HasValue || this[index].Ordinal > columnViewColumn.Ordinal)
						{
							ordinalIndex = index;
							break;
						}
				}

				// Hidden columns are ordered alphabetically after the visible ones.  The binary search only looks at the hidden columns, so if we're making this
				// column visible, we'll used the predefined ordinal for its location.  If it's not visible, we'll sort it alphabetically after the visible columns.  Note that
 				// we need to adjust the new index when we open up a hold in the list for reasons I don't clearly understand.  The 'Move' method should be smart 
 				// enough to work ith absolute index coordinates.
				Int32 newIndex = columnViewColumn.IsVisible ? ordinalIndex : ~this.BinarySearch(columnViewColumn);
				if (oldIndex < newIndex)
					newIndex--;

				// This will adjust the count of visible items based on whether we're becoming visible or being hidden.
				this.visibleCount = columnViewColumn.IsVisible ? this.visibleCount + 1 : this.visibleCount - 1;

				// Moving the item will either cause it to become visible or invisible based on whether it appears before or after the 'visibleCount'.
				this.MoveItem(oldIndex, newIndex);

			}

		}

		/// <summary>
		/// Removes the item at the specified index of the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		protected override void RemoveItem(int index)
		{

			// Columns must have the property changed event handler removed from them before leaving the collection.
			ColumnViewColumn columnViewColumn = this[index];
			columnViewColumn.PropertyChanged -= this.OnItemPropertyChanged;
			base.RemoveItem(index);

		}

		/// <summary>
		/// Replaces the element at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to replace.</param>
		/// <param name="item">The new value for the element at the specified index.</param>
		protected override void SetItem(int index, ColumnViewColumn item)
		{

			// Validate the parameters.
			if (item == null)
				throw new ArgumentNullException("item");

			// Columns must have the property changed event handler removed from them before leaving the collection.  New columns must be added to the event 
			// handler.
			ColumnViewColumn columnViewColumn = this[index];
			columnViewColumn.PropertyChanged -= this.OnItemPropertyChanged;
			item.PropertyChanged += this.OnItemPropertyChanged;
			base.SetItem(index, item);

		}

		/// <summary>
		/// Uses a binary search algorithm to locate a specific element in the ColumnViewColumnCollection.
		/// </summary>
		/// <param name="columnViewColumn">The Object to locate.</param>
		/// <returns>
		/// The zero-based index of item in the list if found; otherwise, a negative number that is the bitwise complement of the index of the next element that is
		/// larger than item or, if there is no larger element, the bitwise complement of Count.
		/// </returns>
		Int32 BinarySearch(ColumnViewColumn columnViewColumn)
		{

			// The binary search will compare the headers.  Since Headers are objects, we need to make sure we convert to a comparable object first.
			String key = columnViewColumn.Description;

			// This is a standard binary search, ripped from the .NET code by Reflector.  The only real addition is the use of the generic comparer that makes use
			// of the selector and the type of the key.
			Int32 low = this.visibleCount;
			Int32 high = this.Count - 1;
			while (low <= high)
			{
				Int32 mid = low + (high - low >> 1);
				ColumnViewColumn midColumn = this[mid];
				String midKey = midColumn.Description;
				Int32 compare = String.Compare(midKey, key, StringComparison.OrdinalIgnoreCase);
				if (compare == 0)
					return mid;
				else
				{
					if (compare < 0)
						low = mid + 1;
					else
						high = mid - 1;
				}
			}

			// If the item isn't found using a binary search, then the completement of the 'low' variable indicates where in the list the item would go if it were
			// part of the sorted list.
			return ~low;

		}

		/// <summary>
		/// Gets the index of the last visible item in this collection.
		/// </summary>
		public Int32 VisibleCount
		{
			get
			{
				return this.visibleCount;
			}
		}

	}

}
