namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Data;
	using System.Threading;
	using System.Windows;

	/// <summary>
	/// Represents a list of TimeUnitItems that is bound to the client data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class TimeUnitCollection : ObservableCollection<TimeUnitItem>, IDisposable
	{

		/// <summary>
		/// Creates a list of TimeUnitItems that is bound to the data model.
		/// </summary>
		public TimeUnitCollection()
		{

			// The list is bound to the data model through these event handlers.
			DataModel.TimeUnit.TimeUnitRowChanged += this.OnTimeUnitRowChanged;
			DataModel.TimeUnit.TimeUnitRowDeleted += this.OnTimeUnitRowDeleted;

			// Create a sorted list from the data model.
			foreach (DataModel.TimeUnitRow timeUnitRow in DataModel.TimeUnit)
				this.Insert(~this.BinarySearch(timeUnitItem => timeUnitItem.TimeUnitCode, timeUnitRow.TimeUnitCode), new TimeUnitItem(timeUnitRow));

		}

		/// <summary>
		/// Finalize this instance of the SideList class.
		/// </summary>
		~TimeUnitCollection()
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
				DataModel.TimeUnit.TimeUnitRowChanging -= this.OnTimeUnitRowChanged;
				DataModel.TimeUnit.TimeUnitRowDeleting -= this.OnTimeUnitRowDeleted;
			}

		}

		/// <summary>
		/// Handles a change to a TimeUnit record in the data model.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnTimeUnitRowChanged(Object sender, DataModel.TimeUnitRowChangeEventArgs e)
		{

			// We're only interested in additions and changes in this handler.
			if (e.Action == DataRowAction.Add || e.Action == DataRowAction.Change)
			{

				// If the item doesn't exist, it is added.  If it exists, it's updated.
				Int32 index = this.BinarySearch(timeUnitItem => timeUnitItem.TimeUnitCode, e.Row.TimeUnitCode);
				if (index < 0)
					this.Insert(~index, new TimeUnitItem(e.Row));
				else
					this[index].Copy(e.Row);

			}

		}

		/// <summary>
		/// Handles a change to a TimeUnit record in the data model.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnTimeUnitRowDeleted(Object sender, DataModel.TimeUnitRowChangeEventArgs e)
		{

			// This will delete the item from the list when it is deleted from the data model.
			if (e.Action == DataRowAction.Delete)
			{
				Int32 index = this.BinarySearch(timeUnitItem => timeUnitItem.TimeUnitCode, e.Row.TimeUnitCode);
				if (index >= 0)
					this.RemoveAt(index);
			}

		}

	}

}
