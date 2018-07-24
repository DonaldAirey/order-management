namespace Teraque.AssetNetwork.Windows
{

    using System;
    using System.Collections.ObjectModel;
    using System.Data;

	/// <summary>
	/// Represents a collection of BlotterItem elements.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class BlotterCollection : ObservableCollection<BlotterItem>, IDisposable
	{

		/// <summary>
		/// Creates a list of BlotterItems that is bound to the data model.
		/// </summary>
		public BlotterCollection()
		{

			// The list is bound to the data model through these event handlers.
			DataModel.Blotter.BlotterRowChanged += this.OnBlotterRowChanged;
			DataModel.Blotter.BlotterRowDeleted += this.OnBlotterRowDeleted;
			DataModel.Entity.EntityRowChanged += this.OnEntityRowChanged;

			// Create a sorted list from the data model.
			foreach (DataModel.BlotterRow blotterRow in DataModel.Blotter)
				this.Insert(~this.BinarySearch(blotterItem => blotterItem.BlotterId, blotterRow.BlotterId), new BlotterItem(blotterRow));

		}

		/// <summary>
		/// Finalize this instance of the BlotterList class.
		/// </summary>
		~BlotterCollection()
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
				DataModel.Blotter.BlotterRowChanged -= this.OnBlotterRowChanged;
				DataModel.Blotter.BlotterRowDeleted -= this.OnBlotterRowDeleted;
				DataModel.Entity.EntityRowChanged -= this.OnEntityRowChanged;
			}

		}

		/// <summary>
		/// Handles a change to a Blotter record in the data model.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="blotterRowChangeEventArgs">The event arguments.</param>
		private void OnBlotterRowChanged(Object sender, DataModel.BlotterRowChangeEventArgs blotterRowChangeEventArgs)
		{

			// We're only interested in additions and changes in this handler.
			if (blotterRowChangeEventArgs.Action == DataRowAction.Add || blotterRowChangeEventArgs.Action == DataRowAction.Change)
			{

				// If the item doesn't exist, it is added.  If it exists, it's updated.
				Int32 index = this.BinarySearch(blotterItem => blotterItem.BlotterId, blotterRowChangeEventArgs.Row.BlotterId);
				if (index < 0)
					this.Insert(~index, new BlotterItem(blotterRowChangeEventArgs.Row));
				else
					this[index].Copy(blotterRowChangeEventArgs.Row);

			}

		}

		/// <summary>
		/// Handles a change to a Blotter record in the data model.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="blotterRowChangeEventArgs">The event arguments.</param>
		private void OnBlotterRowDeleted(Object sender, DataModel.BlotterRowChangeEventArgs blotterRowChangeEventArgs)
		{

			// This will delete the item from the list when it is deleted from the data model.
			if (blotterRowChangeEventArgs.Action == DataRowAction.Delete)
			{
				Int32 index = this.BinarySearch(blotterItem => blotterItem.BlotterId, blotterRowChangeEventArgs.Row.BlotterId);
				if (index >= 0)
					this.RemoveAt(index);
			}

		}

		/// <summary>
		/// Handles a change to a Entity record in the data model.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="entityRowChangeEventArgs">The event arguments.</param>
		private void OnEntityRowChanged(Object sender, DataModel.EntityRowChangeEventArgs entityRowChangeEventArgs)
		{

			// Any changes in the Entity table will be copied to only the blotters in this list.
			if (entityRowChangeEventArgs.Action == DataRowAction.Change)
				foreach (DataModel.BlotterRow blotterRow in entityRowChangeEventArgs.Row.GetBlotterRows())
				{
					Int32 index = this.BinarySearch(blotterItem => blotterItem.BlotterId, blotterRow.BlotterId);
					if (index >= 0)
						this[index].Copy(blotterRow);
				}

		}

	}

}
