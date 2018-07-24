namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Data;

	/// <summary>
	/// Business rules for Source Orders.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal class WorkingOrderService
	{

		/// <summary>
		/// Handles a change to a Working Order.
		/// </summary>
		/// <param name="workingOrderRow">The working order that has changed.</param>
		delegate void WorkingOrderRowHandler(DataModel.WorkingOrderRow workingOrderRow);

		/// <summary>
		/// Maps an action to a handler for that action.
		/// </summary>
		static Dictionary<DataRowAction, WorkingOrderRowHandler> workingOrderHandlerMap = new Dictionary<DataRowAction, WorkingOrderRowHandler>()
		{
			{DataRowAction.Add, WorkingOrderService.OnDoNothing},
			{DataRowAction.Change, WorkingOrderService.OnDoNothing},
			{DataRowAction.Commit, WorkingOrderService.OnDoNothing},
			{DataRowAction.Delete, WorkingOrderService.OnDoNothing},
			{DataRowAction.Rollback, WorkingOrderService.OnDoNothing}
		};

		/// <summary>
		/// Gets the sum total of all the destination orders.
		/// </summary>
		/// <param name="dataModelTransaction">The data model transaction.</param>
		/// <param name="workingOrderRow">The working order row.</param>
		/// <returns>The aggregate quantity of destination orders for the specified working order.</returns>
		internal static Decimal GetDestinationOrderQuantity(DataModelTransaction dataModelTransaction, DataModel.WorkingOrderRow workingOrderRow)
		{

			// Aggregate the destination orders.
			Decimal destinationOrderQuantity = 0.0m;
			foreach (DataModel.DestinationOrderRow destinationOrderRow in workingOrderRow.GetDestinationOrderRows())
			{
				destinationOrderRow.AcquireReaderLock(dataModelTransaction);
				destinationOrderQuantity += destinationOrderRow.OrderedQuantity;
			}
			return destinationOrderQuantity;

		}

		/// <summary>
		/// Gets the sum total of all the executions.
		/// </summary>
		/// <param name="dataModelTransaction">The data model transaction.</param>
		/// <param name="workingOrderRow">The working order row.</param>
		/// <returns>The aggregate quantity of executed orders for the specified working order.</returns>
		internal static Decimal GetExecutionQuantity(DataModelTransaction dataModelTransaction, DataModel.WorkingOrderRow workingOrderRow)
		{

			// Aggregate the executed orders.
			Decimal executionQuantity = 0.0m;
			foreach (DataModel.DestinationOrderRow destinationOrderRow in workingOrderRow.GetDestinationOrderRows())
			{
				destinationOrderRow.AcquireReaderLock(dataModelTransaction);
				if (destinationOrderRow.StatusCode != StatusCode.Canceled)
					foreach (DataModel.ExecutionRow executionRow in destinationOrderRow.GetExecutionRows())
					{
						executionRow.AcquireReaderLock(dataModelTransaction);
						executionQuantity += executionRow.ExecutionQuantity;
					}
			}

			// This is the aggregate quantity executed for the given working order.
			return executionQuantity;

		}

		/// <summary>
		/// The sum total of the quantities of all the source orders in a given working order.
		/// </summary>
		/// <param name="dataModelTransaction"></param>
		/// <param name="workingOrderRow">A working order row.</param>
		/// <returns>The total quantity of all the source orders associated with the working order.</returns>
		internal static Decimal GetSourceOrderQuantity(DataModelTransaction dataModelTransaction, DataModel.WorkingOrderRow workingOrderRow)
		{

			// This will aggregate all the source order quantities.  Note that the rows are kept locked for the duration of the transaction.  This guarantees
			// the integrity of the aggregate values.
			Decimal sourceOrderQuantity = 0.0m;
			foreach (DataModel.SourceOrderRow sourceOrderRow in workingOrderRow.GetSourceOrderRows())
			{
				try
				{
					sourceOrderRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
					sourceOrderQuantity += sourceOrderRow.OrderedQuantity;
				}
				finally
				{
					sourceOrderRow.ReleaseLock(dataModelTransaction.TransactionId);
				}
			}

			// This is the sum total of all the source orders in the given working order.
			return sourceOrderQuantity;

		}

		/// <summary>
		/// Do nothing.
		/// </summary>
		/// <param name="workingOrderRow">The source order row that was changed.</param>
		static void OnDoNothing(DataModel.WorkingOrderRow workingOrderRow) { }

		/// <summary>
		/// Handler for validating Source Order records.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		internal static void OnWorkingOrderRowValidate(Object sender, DataModel.WorkingOrderRowChangeEventArgs e)
		{

			// Use the map to call the proper handler for the action.
			WorkingOrderService.workingOrderHandlerMap[e.Action](e.Row);

		}

		/// <summary>
		/// Updates the status of the Working Order.
		/// </summary>
		/// <param name="workingOrderRow">The Working Order to be modified.</param>
		/// <param name="statusCode">The new status of the Working Order.</param>
		internal static void UpdateWorkingOrderStatus(DataModel.WorkingOrderRow workingOrderRow, StatusCode statusCode)
		{

			// This will update only the status on the working order.
			DataModel.TenantDataModel.UpdateWorkingOrder(
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				workingOrderRow.RowVersion,
				null,
				null,
				null, 
				null,
				null,
				statusCode,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				new Object[] { workingOrderRow.WorkingOrderId });

		}


	}

}
