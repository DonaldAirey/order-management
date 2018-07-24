namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections.ObjectModel;
	using System.Data;

	/// <summary>
	/// Represents a collection of TimeInForceItem elements.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class TimeInForceCollection : ObservableCollection<TimeInForceItem>, IDisposable
	{

		/// <summary>
		/// Creates a list of TimeInForceItems that is bound to the data model.
		/// </summary>
		public TimeInForceCollection()
		{

			// The list is bound to the data model through these event handlers.
			DataModel.TimeInForce.TimeInForceRowChanged += this.OnTimeInForceRowChanged;
			DataModel.TimeInForce.TimeInForceRowDeleted += this.OnTimeInForceRowDeleted;

			// Create a sorted list from the data model.
			foreach (DataModel.TimeInForceRow timeInForceRow in DataModel.TimeInForce)
				this.Insert(~this.BinarySearch(timeInForceItem => timeInForceItem.TimeInForceCode, timeInForceRow.TimeInForceCode), new TimeInForceItem(timeInForceRow));

		}

		/// <summary>
		/// Finalize this instance of the TimeInForceList class.
		/// </summary>
		~TimeInForceCollection()
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
				DataModel.TimeInForce.TimeInForceRowChanging -= new DataModel.TimeInForceRowChangeEventHandler(this.OnTimeInForceRowChanged);
				DataModel.TimeInForce.TimeInForceRowDeleting -= new DataModel.TimeInForceRowChangeEventHandler(this.OnTimeInForceRowDeleted);
			}

		}

		/// <summary>
		/// Handles a change to a TimeInForce record in the data model.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="timeInForceRowChangeEventArgs">The event arguments.</param>
		private void OnTimeInForceRowChanged(Object sender, DataModel.TimeInForceRowChangeEventArgs timeInForceRowChangeEventArgs)
		{

			// We're only interested in additions and changes in this handler.
			if (timeInForceRowChangeEventArgs.Action == DataRowAction.Add || timeInForceRowChangeEventArgs.Action == DataRowAction.Change)
			{

				// If the item doesn't exist, it is added.  If it exists, it's updated.
				Int32 index = this.BinarySearch(timeInForceItem => timeInForceItem.TimeInForceCode, timeInForceRowChangeEventArgs.Row.TimeInForceCode);
				if (index < 0)
					this.Insert(~index, new TimeInForceItem(timeInForceRowChangeEventArgs.Row));
				else
					this[index].Copy(timeInForceRowChangeEventArgs.Row);

			}

		}

		/// <summary>
		/// Handles a change to a TimeInForce record in the data model.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="timeInForceRowChangeEventArgs">The event arguments.</param>
		private void OnTimeInForceRowDeleted(Object sender, DataModel.TimeInForceRowChangeEventArgs timeInForceRowChangeEventArgs)
		{

			// This will delete the item from the list when it is deleted from the data model.
			if (timeInForceRowChangeEventArgs.Action == DataRowAction.Delete)
			{
				Int32 index = this.BinarySearch(timeInForceItem => timeInForceItem.TimeInForceCode, timeInForceRowChangeEventArgs.Row.TimeInForceCode);
				if (index >= 0)
					this.RemoveAt(index);
			}

		}

	}

}
