namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;

	/// <summary>
	/// A generic collection of Debt Working Orders.
	/// </summary>
	/// <remarks>This generic class is primarily for inheritance.</remarks>
	public class DebtWorkingOrderCollection<TType> : WorkingOrderCollection<TType> where TType : DebtWorkingOrder
	{

		/// <summary>
		/// Initializes a new instance of the WorkingOrderCollection class.
		/// </summary>
		public DebtWorkingOrderCollection(Guid blotterId) : base(blotterId)
		{

			// Link the collection into the data model.
			DataModel.Debt.DebtRowChanged += this.OnDebtRowChanged;
		
		}

		/// <summary>
		/// Creates a new instance of the TType row.
		/// </summary>
		/// <param name="workingOrderRow">The data model record that is the source of information for the new row.</param>
		/// <returns>A model view version of the data model record.</returns>
		protected override TType CreateInstanceCore(DataModel.WorkingOrderRow workingOrderRow)
		{

			// Create a model view version of the data model working order row.
			return new DebtWorkingOrder(workingOrderRow) as TType;

		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing">true to indicate that the object is being disposed, false to indicate that the object is being finalized.</param>
		protected override void Dispose(bool disposing)
		{

			// This will remove this object from the data model events.
			if (disposing)
			{
				DataModel.Debt.DebtRowChanged -= this.OnDebtRowChanged;
			}

			// The base class has additional items to release.
			base.Dispose(disposing);

		}

		/// <summary>
		/// Handles a change to the Debt row.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="debtRowChangeEventArgs">The event arguments.</param>
		protected virtual void OnDebtRowChanged(Object sender, DataModel.DebtRowChangeEventArgs debtRowChangeEventArgs)
		{

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (debtRowChangeEventArgs == null)
				throw new ArgumentNullException("debtRowChangeEventArgs");

			// We're only interested in changes that affect the WorkingOrder records in this blotter.
			if (debtRowChangeEventArgs.Action == DataRowAction.Add || debtRowChangeEventArgs.Action == DataRowAction.Change)
			{
				DataModel.DebtRow debtRow = debtRowChangeEventArgs.Row;
				foreach (DataModel.WorkingOrderRow workingOrderRow in
					debtRow.SecurityRowByFK_Security_Debt_DebtId.GetWorkingOrderRowsByFK_Security_WorkingOrder_SecurityId())
				{
					if (this.BlotterIdSet.Contains(workingOrderRow.BlotterId))
					{
						Int32 index = this.BinarySearch(order => order.WorkingOrderId, workingOrderRow.WorkingOrderId);
						if (index >= 0)
						{
							TType workingOrder = this[index];
							this.View.EditItem(workingOrder);
							workingOrder.MaturityDate = debtRow.MaturityDate;
							this.View.CommitEdit();
						}
					}
				}
			}

		}

		/// <summary>
		/// Handles a change to the WorkingOrder row.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="workingOrderRowChangeEventArgs">The event arguments.</param>
		protected override void OnWorkingOrderRowChanged(Object sender, DataModel.WorkingOrderRowChangeEventArgs workingOrderRowChangeEventArgs)
		{

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (workingOrderRowChangeEventArgs == null)
				throw new ArgumentNullException("workingOrderRowChangeEventArgs");

			// If the new working order belongs to this blotter or one of its descendants, then add it in the proper place.
			if (workingOrderRowChangeEventArgs.Action == DataRowAction.Add)
			{
				DataModel.WorkingOrderRow workingOrderRow = workingOrderRowChangeEventArgs.Row;
				if (this.BlotterIdSet.Contains(workingOrderRow.BlotterId))
				{
					Int32 index = this.BinarySearch(order => order.WorkingOrderId, workingOrderRow.WorkingOrderId);
					this.Insert(~index, this.CreateInstanceCore(workingOrderRowChangeEventArgs.Row));
				}
			}

			// This will copy the modified elements from the data model into the collection and commit the changes.
			if (workingOrderRowChangeEventArgs.Action == DataRowAction.Change)
			{
				DataModel.WorkingOrderRow workingOrderRow = workingOrderRowChangeEventArgs.Row;
				if (this.BlotterIdSet.Contains(workingOrderRow.BlotterId))
				{
					Int32 index = this.BinarySearch(order => order.WorkingOrderId, workingOrderRow.WorkingOrderId);
					TType debtWorkingOrder = this[index];
					this.View.EditItem(debtWorkingOrder);
					debtWorkingOrder.IsActive = workingOrderRow.StatusRow.StatusCode != StatusCode.Filled;
					debtWorkingOrder.BlotterName = workingOrderRow.BlotterRow.EntityRow.Name;
					debtWorkingOrder.CreatedBy = workingOrderRow.UserRowByFK_User_WorkingOrder_CreatedUserId.EntityRow.Name;
					debtWorkingOrder.CreatedTime = workingOrderRow.CreatedTime;
					debtWorkingOrder.ModifiedBy = workingOrderRow.UserRowByFK_User_WorkingOrder_ModifiedUserId.EntityRow.Name;
					debtWorkingOrder.ModifiedTime = workingOrderRow.ModifiedTime;
					debtWorkingOrder.SecurityName = workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SecurityId.EntityRow.Name;
					debtWorkingOrder.SettlementDate = workingOrderRow.SettlementDate;
					debtWorkingOrder.SideCode = workingOrderRow.SideRow.SideCode;
					debtWorkingOrder.SideMnemonic = workingOrderRow.SideRow.Mnemonic;
					debtWorkingOrder.StatusCode = workingOrderRow.StatusRow.StatusCode;
					debtWorkingOrder.Symbol = workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SecurityId.Symbol;
					debtWorkingOrder.TradeDate = workingOrderRow.TradeDate;
					debtWorkingOrder.WorkingOrderId = workingOrderRow.WorkingOrderId;
					this.View.CommitEdit();
				}

			}
		}

	}

}
