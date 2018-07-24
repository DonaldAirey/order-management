namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;

	/// <summary>
	/// A generic collection of Equity Working Orders.
	/// </summary>
	/// <remarks>This generic class is primarily for inheritance.</remarks>
	public class EquityWorkingOrderCollection<TType> : WorkingOrderCollection<TType> where TType : EquityWorkingOrder
	{

		/// <summary>
		/// Initializes a new instance of the WorkingOrderCollection class.
		/// </summary>
		public EquityWorkingOrderCollection(Guid blotterId) : base(blotterId) { }

		/// <summary>
		/// Creates a new instance of the TType row.
		/// </summary>
		/// <param name="workingOrderRow">The data model record that is the source of information for the new row.</param>
		/// <returns>A model view version of the data model record.</returns>
		protected override TType CreateInstanceCore(DataModel.WorkingOrderRow workingOrderRow)
		{

			// Create a model view version of the data model working order row.
			return new EquityWorkingOrder(workingOrderRow) as TType;

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
					TType equityWorkingOrder = this[index];
					this.View.EditItem(equityWorkingOrder);
					equityWorkingOrder.IsActive = workingOrderRow.StatusRow.StatusCode != StatusCode.Filled;
					equityWorkingOrder.BlotterName = workingOrderRow.BlotterRow.EntityRow.Name;
					equityWorkingOrder.CreatedBy = workingOrderRow.UserRowByFK_User_WorkingOrder_CreatedUserId.EntityRow.Name;
					equityWorkingOrder.CreatedTime = workingOrderRow.CreatedTime;
					equityWorkingOrder.IsCrossing = workingOrderRow.CrossingRow.CrossingCode == CrossingCode.AlwaysMatch;
					equityWorkingOrder.IsBrokerMatch = workingOrderRow.IsBrokerMatch;
					equityWorkingOrder.IsHedgeMatch = workingOrderRow.IsHedgeMatch;
					equityWorkingOrder.IsInstitutionMatch = workingOrderRow.IsInstitutionMatch;
					equityWorkingOrder.LimitPrice = workingOrderRow.IsLimitPriceNull() ? 0.0M : workingOrderRow.LimitPrice;
					equityWorkingOrder.ModifiedBy = workingOrderRow.UserRowByFK_User_WorkingOrder_ModifiedUserId.EntityRow.Name;
					equityWorkingOrder.ModifiedTime = workingOrderRow.ModifiedTime;
					equityWorkingOrder.RowVersion = workingOrderRow.RowVersion;
					equityWorkingOrder.OrderTypeMnemonic = workingOrderRow.OrderTypeRow.Mnemonic;
					equityWorkingOrder.OrderTypeCode = workingOrderRow.OrderTypeRow.OrderTypeCode;
					equityWorkingOrder.SecurityName = workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SecurityId.EntityRow.Name;
					equityWorkingOrder.SettlementDate = workingOrderRow.SettlementDate;
					equityWorkingOrder.SideCode = workingOrderRow.SideRow.SideCode;
					equityWorkingOrder.SideMnemonic = workingOrderRow.SideRow.Mnemonic;
					equityWorkingOrder.StatusCode = workingOrderRow.StatusRow.StatusCode;
					equityWorkingOrder.StopPrice = workingOrderRow.IsStopPriceNull() ? 0.0M : workingOrderRow.StopPrice;
					equityWorkingOrder.Symbol = workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SecurityId.Symbol;
					equityWorkingOrder.TimeInForceCode = workingOrderRow.TimeInForceRow.TimeInForceCode;
					equityWorkingOrder.TradeDate = workingOrderRow.TradeDate;
					equityWorkingOrder.WorkingOrderId = workingOrderRow.WorkingOrderId;
					this.View.CommitEdit();
				}

			}

		}

	}

}
