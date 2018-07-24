namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Data;
	using System.Threading;
	using System.Windows;
	using Teraque.Windows;

	/// <summary>
	/// Represents a list of SettlementUnitItems that is bound to the client data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class SettlementUnitCollection : ObservableCollection<SettlementUnitItem>, IDisposable
	{

		/// <summary>
		/// Creates a list of SettlementUnitItems that is bound to the data model.
		/// </summary>
		public SettlementUnitCollection()
		{

			// The list is bound to the data model through these event handlers.
			DataModel.SettlementUnit.SettlementUnitRowChanged += this.OnSettlementUnitRowChanged;
			DataModel.SettlementUnit.SettlementUnitRowDeleted += this.OnSettlementUnitRowDeleted;

			// Create a sorted list from the data model.
			foreach (DataModel.SettlementUnitRow settlementUnitRow in DataModel.SettlementUnit)
				this.Insert(
					~this.BinarySearch(settlementUnitItem => settlementUnitItem.SettlementUnitCode, settlementUnitRow.SettlementUnitCode),
					new SettlementUnitItem(settlementUnitRow));

		}

		/// <summary>
		/// Finalize this instance of the SideList class.
		/// </summary>
		~SettlementUnitCollection()
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
				DataModel.SettlementUnit.SettlementUnitRowChanging -= this.OnSettlementUnitRowChanged;
				DataModel.SettlementUnit.SettlementUnitRowDeleting -= this.OnSettlementUnitRowDeleted;
			}

		}

		/// <summary>
		/// Handles a change to a SettlementUnit record in the data model.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		private void OnSettlementUnitRowChanged(Object sender, DataModel.SettlementUnitRowChangeEventArgs e)
		{

			// We're only interested in additions and changes in this handler.
			if (e.Action == DataRowAction.Add || e.Action == DataRowAction.Change)
			{

				// If the item doesn't exist, it is added.  If it exists, it's updated.
				Int32 index = this.BinarySearch(settlementUnitItem => settlementUnitItem.SettlementUnitCode, e.Row.SettlementUnitCode);
				if (index < 0)
					this.Insert(~index, new SettlementUnitItem(e.Row));
				else
					this[index].Copy(e.Row);

			}

		}

		/// <summary>
		/// Handles a change to a SettlementUnit record in the data model.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		private void OnSettlementUnitRowDeleted(Object sender, DataModel.SettlementUnitRowChangeEventArgs e)
		{

			// This will delete the item from the list when it is deleted from the data model.
			if (e.Action == DataRowAction.Delete)
			{
				Int32 index = this.BinarySearch(settlementUnitItem => settlementUnitItem.SettlementUnitCode, e.Row.SettlementUnitCode);
				if (index >= 0)
					this.RemoveAt(index);
			}

		}

	}

}
