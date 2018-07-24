namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections.ObjectModel;
	using System.Data;

	/// <summary>
	/// Represents a collection of StatusItem elements.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class StatusCollection : ObservableCollection<StatusItem>, IDisposable
	{

		/// <summary>
		/// Creates a list of StatusItems that is bound to the data model.
		/// </summary>
		public StatusCollection()
		{

			// The list is bound to the data model through these event handlers.
			DataModel.Status.StatusRowChanged += this.OnStatusRowChanged;
			DataModel.Status.StatusRowDeleted += this.OnStatusRowDeleted;

			// Create a sorted list from the data model.
			foreach (DataModel.StatusRow statusRow in DataModel.Status)
				this.Insert(~this.BinarySearch(statusItem => statusItem.StatusCode, statusRow.StatusCode), new StatusItem(statusRow));

		}

		/// <summary>
		/// Finalize this instance of the StatusList class.
		/// </summary>
		~StatusCollection()
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
				DataModel.Status.StatusRowChanging -= new DataModel.StatusRowChangeEventHandler(this.OnStatusRowChanged);
				DataModel.Status.StatusRowDeleting -= new DataModel.StatusRowChangeEventHandler(this.OnStatusRowDeleted);
			}

		}

		/// <summary>
		/// Handles a change to a Status record in the data model.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="statusRowChangeEventArgs">The event arguments.</param>
		private void OnStatusRowChanged(Object sender, DataModel.StatusRowChangeEventArgs statusRowChangeEventArgs)
		{

			// We're only interested in additions and changes in this handler.
			if (statusRowChangeEventArgs.Action == DataRowAction.Add || statusRowChangeEventArgs.Action == DataRowAction.Change)
			{

				// If the item doesn't exist, it is added.  If it exists, it's updated.
				Int32 index = this.BinarySearch(statusItem => statusItem.StatusCode, statusRowChangeEventArgs.Row.StatusCode);
				if (index < 0)
					this.Insert(~index, new StatusItem(statusRowChangeEventArgs.Row));
				else
					this[index].Copy(statusRowChangeEventArgs.Row);

			}

		}

		/// <summary>
		/// Handles a change to a Status record in the data model.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="statusRowChangeEventArgs">The event arguments.</param>
		private void OnStatusRowDeleted(Object sender, DataModel.StatusRowChangeEventArgs statusRowChangeEventArgs)
		{

			// This will delete the item from the list when it is deleted from the data model.
			if (statusRowChangeEventArgs.Action == DataRowAction.Delete)
			{
				Int32 index = this.BinarySearch(statusItem => statusItem.StatusCode, statusRowChangeEventArgs.Row.StatusCode);
				if (index >= 0)
					this.RemoveAt(index);
			}

		}

	}

}
