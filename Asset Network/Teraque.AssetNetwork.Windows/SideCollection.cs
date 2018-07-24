namespace Teraque.AssetNetwork.Windows
{

    using System;
    using System.Collections.ObjectModel;
    using System.Data;

	/// <summary>
	/// Represents a collection of SideItem elements.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class SideCollection : ObservableCollection<SideItem>, IDisposable
	{

		/// <summary>
		/// Creates a list of SideItems that is bound to the data model.
		/// </summary>
		public SideCollection()
		{

			// The list is bound to the data model through these event handlers.
			DataModel.Side.SideRowChanged += this.OnSideRowChanged;
			DataModel.Side.SideRowDeleted += this.OnSideRowDeleted;

			// Create a sorted list from the data model.
			foreach (DataModel.SideRow sideRow in DataModel.Side)
				this.Insert(~this.BinarySearch(sideItem => sideItem.SideCode, sideRow.SideCode), new SideItem(sideRow));

		}

		/// <summary>
		/// Finalize this instance of the SideList class.
		/// </summary>
		~SideCollection()
		{

			// Call the virtual method to dispose of the resources.  This (recommended) design pattern gives any derived classes a chance to clean up unmanaged 
			// resources even though this base class has none.
			this.Dispose(false);

		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{

			// Call the virtual method to allow derived classes to clean up resources.
			this.Dispose(true);

			// Since we took care of cleaning up the resources, there is no need to call the finalizer.
			GC.SuppressFinalize(this);

		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing">true to indicate that the object is being disposed, false to indicate that the object is being finalized.</param>
		protected virtual void Dispose(Boolean disposing)
		{

			// This will remove this object from the data model events.
			if (disposing)
			{
				DataModel.Side.SideRowChanging -= new DataModel.SideRowChangeEventHandler(this.OnSideRowChanged);
				DataModel.Side.SideRowDeleting -= new DataModel.SideRowChangeEventHandler(this.OnSideRowDeleted);
			}

		}

		/// <summary>
		/// Handles a change to a Side record in the data model.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="sideRowChangeEventArgs">The event arguments.</param>
		private void OnSideRowChanged(Object sender, DataModel.SideRowChangeEventArgs sideRowChangeEventArgs)
		{

			// We're only interested in additions and changes in this handler.
			if (sideRowChangeEventArgs.Action == DataRowAction.Add || sideRowChangeEventArgs.Action == DataRowAction.Change)
			{

				// If the item doesn't exist, it is added.  If it exists, it's updated.
				Int32 index = this.BinarySearch(sideItem => sideItem.SideCode, sideRowChangeEventArgs.Row.SideCode);
				if (index < 0)
					this.Insert(~index, new SideItem(sideRowChangeEventArgs.Row));
				else
					this[index].Copy(sideRowChangeEventArgs.Row);

			}

		}

		/// <summary>
		/// Handles a change to a Side record in the data model.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="sideRowChangeEventArgs">The event arguments.</param>
		private void OnSideRowDeleted(Object sender, DataModel.SideRowChangeEventArgs sideRowChangeEventArgs)
		{

			// This will delete the item from the list when it is deleted from the data model.
			if (sideRowChangeEventArgs.Action == DataRowAction.Delete)
			{
				Int32 index = this.BinarySearch(sideItem => sideItem.SideCode, sideRowChangeEventArgs.Row.SideCode);
				if (index >= 0)
					this.RemoveAt(index);
			}

		}

	}

}
