namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.ServiceModel;
	using Teraque;

	/// <summary>
	/// Business rules for DestinationOrders.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class DestinationOrderService
    {

		/// <summary>
		/// Handles a change to a Destination Order.
		/// </summary>
		/// <param name="destinationOrderRow">The destination order that has changed.</param>
		delegate void DestinationOrderRowHandler(DataModel.DestinationOrderRow destinationOrderRow);

		/// <summary>
		/// Handles a change to a Working Order.
		/// </summary>
		/// <param name="workingOrderRow">The working order that has changed.</param>
		delegate void WorkingOrderRowHandler(DataModel.WorkingOrderRow workingOrderRow);

		/// <summary>
		/// Maps a DataRowAction to a handler for that action.
		/// </summary>
		static Dictionary<DataRowAction, DestinationOrderRowHandler> destinationOrderHandlerMap = new Dictionary<DataRowAction, DestinationOrderRowHandler>()
		{
			{DataRowAction.Add, DestinationOrderService.OnDestinationOrderAdd},
			{DataRowAction.Change, DestinationOrderService.OnDestinationOrderChange},
			{DataRowAction.Commit, DestinationOrderService.OnDoNothing},
			{DataRowAction.Delete, DestinationOrderService.OnDestinationOrderDelete},
			{DataRowAction.Rollback, DestinationOrderService.OnDoNothing}
		};

		/// <summary>
		/// Handlers for Error StatusCodes.
		/// </summary>
		static Dictionary<StatusCode, WorkingOrderRowHandler> errorChanges = new Dictionary<StatusCode, WorkingOrderRowHandler>()
		{
			{StatusCode.New, DestinationOrderService.OnClearErrorAction},
			{StatusCode.PartiallyFilled, DestinationOrderService.OnClearErrorAction},
			{StatusCode.Canceled, DestinationOrderService.OnClearErrorAction},
			{StatusCode.Deleted, DestinationOrderService.OnClearErrorAction},
			{StatusCode.Filled, DestinationOrderService.OnClearErrorAction}
		};

		/// <summary>
		/// Handlers for Filled StatusCodes.
		/// </summary>
		static Dictionary<StatusCode, WorkingOrderRowHandler> filledChanges = new Dictionary<StatusCode, WorkingOrderRowHandler>()
		{
			{StatusCode.New, DestinationOrderService.OnCanceledAction},
			{StatusCode.PartiallyFilled, DestinationOrderService.OnPartialAction},
			{StatusCode.Canceled, DestinationOrderService.OnCanceledAction},
			{StatusCode.Deleted, DestinationOrderService.OnCanceledAction},
			{StatusCode.Error, DestinationOrderService.OnSetErrorAction}
		};

		/// <summary>
		/// Handlers for New StatusCodes.
		/// </summary>
		static Dictionary<StatusCode, WorkingOrderRowHandler> newChanges = new Dictionary<StatusCode, WorkingOrderRowHandler>()
		{
			{StatusCode.PartiallyFilled, DestinationOrderService.OnPartialAction},
			{StatusCode.Filled, DestinationOrderService.OnFilledAction},
			{StatusCode.Canceled, DestinationOrderService.OnDoNothing},
			{StatusCode.Deleted, DestinationOrderService.OnDoNothing},
			{StatusCode.Error, DestinationOrderService.OnSetErrorAction}
		};

		/// <summary>
		/// Handlers for PartiallyFilled StatusCodes.
		/// </summary>
		static Dictionary<StatusCode, WorkingOrderRowHandler> partiallyFilledChanges = new Dictionary<StatusCode, WorkingOrderRowHandler>()
		{
			{StatusCode.New, DestinationOrderService.OnCanceledAction},
			{StatusCode.Filled, DestinationOrderService.OnFilledAction},
			{StatusCode.Canceled, DestinationOrderService.OnCanceledAction},
			{StatusCode.Deleted, DestinationOrderService.OnCanceledAction},
			{StatusCode.Error, DestinationOrderService.OnSetErrorAction}
		};

		/// <summary>
		/// Used to select a handler based on the change in state from one StatusCode to another.
		/// </summary>
		static Dictionary<StatusCode, Dictionary<StatusCode, WorkingOrderRowHandler>> statusChangeMap =
			new Dictionary<StatusCode, Dictionary<StatusCode, WorkingOrderRowHandler>>()
			{
				{StatusCode.Error, DestinationOrderService.errorChanges},
				{StatusCode.Filled, DestinationOrderService.filledChanges},
				{StatusCode.New, DestinationOrderService.newChanges},
				{StatusCode.PartiallyFilled, DestinationOrderService.partiallyFilledChanges}
			};

		/// <summary>
		/// Creates one or more destination orders.
		/// </summary>
		/// <param name="destinationOrders">An array of structures describing the destination orders to be created.</param>
		internal static void CreateDestinationOrders(DestinationOrderInfo[] destinationOrders)
		{

			// Business logic: provide the current time and the user identifier for the new destination order.
			DateTime dateTime = DateTime.UtcNow;
			Guid userId = UserService.GetUserId();

			foreach (DestinationOrderInfo destinationOrderInfo in destinationOrders)
			{

				Guid destinationOrderId = Guid.NewGuid();

				DataModel.TenantDataModel.CreateDestinationOrder(
					0.0M,
					null,
					dateTime,
					userId,
					destinationOrderInfo.DestinationId,
					destinationOrderId,
					null,
					false,
					false,
					null,
					dateTime,
					userId,
					destinationOrderInfo.OrderedQuantity,
					destinationOrderInfo.OrderTypeCode,
					destinationOrderInfo.SecurityId,
					dateTime,
					destinationOrderInfo.SettlementId,
					destinationOrderInfo.SideCode,
					StateCode.Initial,
					StatusCode.New,
					null,
					destinationOrderInfo.TimeInForceCode,
					dateTime,
					null,
					destinationOrderInfo.WorkingOrderId);
			}


		}

		/// <summary>
		/// Destroys one or more destination orders.
		/// </summary>
		/// <param name="destinationOrders">An array of structures referencing the destination orders to destroy.</param>
		internal static void DestroyDestinationOrders(DestinationOrderReference[] destinationOrders)
		{

			// Destroy each of the destination orders described in the array.
			foreach (DestinationOrderReference destinationOrderReference in destinationOrders)
				DataModel.TenantDataModel.DestroyDestinationOrder(new Object[] { destinationOrderReference.DestinationId }, destinationOrderReference.RowVersion);

		}
    
		/// <summary>
		/// Truncates the DestinationOrder table.
		/// </summary>
		internal static void Truncate()
		{

			// We need a transaction in order to scan the DestinationOrder table.
			DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

			// We can't modify a list while iterating through it, so we need to collect all the DestinationOrder records in this list.  Then when we start to
			// destroy them, we won't disturb the iterator.
			List<DestroyInfo> destroyInfoList = new List<DestroyInfo>();

			try
			{

				// A reader lock is required to protect the table iterators while we collect all the DestinationOrders.
				DataModel.DataLock.EnterReadLock();

				// This will collect all the DestinationOrders.
				foreach (DataModel.DestinationOrderRow destinationOrderRow in DataModel.DestinationOrder)
				{
					destinationOrderRow.AcquireReaderLock(dataModelTransaction);
					dataModelTransaction.AddLock(destinationOrderRow);
					destroyInfoList.Add(new DestroyInfo(new Object[] { destinationOrderRow.DestinationOrderId }, destinationOrderRow.RowVersion));
				}

			}
			finally
			{

				// We don't need to protect the DestinationOrder table iterators any longer.
				DataModel.DataLock.ExitReadLock();

			}

			// At this point we've collected all the DestinationOrders in the table.  This will call the internal method to destroy them.
			foreach (DestroyInfo destroyInfo in destroyInfoList)
				DataModel.TenantDataModel.DestroyDestinationOrder(destroyInfo.Key, destroyInfo.RowVersion);

		}

		/// <summary>
		/// Handles a change to a cancelled state.
		/// </summary>        
		/// <param name="workingOrderRow">The parent working order.</param>
		static void OnCanceledAction(DataModel.WorkingOrderRow workingOrderRow)
		{

			// A transaction is needed to handle the change.
			DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

			// This will lock the WorkingOrderRow while we examine it.
			workingOrderRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
			dataModelTransaction.AddLock(workingOrderRow);
			if (workingOrderRow.RowState == DataRowState.Detached)
				return;

			// When a destination order is canceled we will return the order to its previous state (as determined by aggregating the executed and destination
			// quantities).  The only exception to this is when the working order is in the error state.
			if (workingOrderRow.StatusCode != StatusCode.Error)
			{
				Decimal quantityExecuted = WorkingOrderService.GetExecutionQuantity(dataModelTransaction, workingOrderRow);
				Decimal quantityOrdered = WorkingOrderService.GetSourceOrderQuantity(dataModelTransaction, workingOrderRow);
				if (quantityExecuted == 0.0M && workingOrderRow.StatusCode != StatusCode.New)
					WorkingOrderService.UpdateWorkingOrderStatus(workingOrderRow, StatusCode.New);
				if (0.0M < quantityExecuted && quantityExecuted < quantityOrdered && workingOrderRow.StatusCode != StatusCode.PartiallyFilled)
					WorkingOrderService.UpdateWorkingOrderStatus(workingOrderRow, StatusCode.PartiallyFilled);

			}

		}

		/// <summary>
		/// Handles a change to a error state.
		/// </summary>        
		/// <param name="workingOrderRow">The parent working order.</param>
		static void OnClearErrorAction(DataModel.WorkingOrderRow workingOrderRow)
		{

			// A transaction is needed to handle the change.
			DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

			// This will lock the WorkingOrderRow while we examine it.
			workingOrderRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
			dataModelTransaction.AddLock(workingOrderRow);
			if (workingOrderRow.RowState == DataRowState.Detached)
				return;

			// The error status is cleared only when none of the sibling destination orders has an error.
			Boolean isErrorStatus = false;
			foreach (DataModel.DestinationOrderRow siblingOrderRow in workingOrderRow.GetDestinationOrderRows())
			{
				siblingOrderRow.AcquireReaderLock(dataModelTransaction);
				if (siblingOrderRow.StatusCode == StatusCode.Error)
				{
					isErrorStatus = true;
					break;
				}
			}

			// If none of the siblings has an error, the we're going to set the working order's status to what it was before the error occurred.
			if (!isErrorStatus)
			{
				Decimal quantityExecuted = WorkingOrderService.GetExecutionQuantity(dataModelTransaction, workingOrderRow);
				Decimal quantityOrdered = WorkingOrderService.GetSourceOrderQuantity(dataModelTransaction, workingOrderRow);
				if (quantityExecuted == 0.0M && workingOrderRow.StatusCode != StatusCode.New)
					WorkingOrderService.UpdateWorkingOrderStatus(workingOrderRow, StatusCode.New);
				if (0.0M < quantityExecuted && quantityExecuted < quantityOrdered && workingOrderRow.StatusCode != StatusCode.PartiallyFilled)
					WorkingOrderService.UpdateWorkingOrderStatus(workingOrderRow, StatusCode.PartiallyFilled);
				if (quantityExecuted == quantityOrdered && workingOrderRow.StatusCode != StatusCode.Filled)
					WorkingOrderService.UpdateWorkingOrderStatus(workingOrderRow, StatusCode.Filled);
			}

		}

		/// <summary>
		/// Handler for validating Destination Order records.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		static void OnDestinationOrderAdd(DataModel.DestinationOrderRow destinationOrderRow)
		{

			// A transaction is needed to handle the change.
			DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

			DataModel.WorkingOrderRow workingOrderRow = destinationOrderRow.WorkingOrderRow;
			workingOrderRow.AcquireReaderLock(dataModelTransaction);

			Decimal sourceOrderQuantity = WorkingOrderService.GetSourceOrderQuantity(dataModelTransaction, workingOrderRow);
			Decimal destinationOrderQuantity = WorkingOrderService.GetDestinationOrderQuantity(dataModelTransaction, workingOrderRow);
			if (sourceOrderQuantity < destinationOrderQuantity)
				throw new FaultException<DestinationQuantityFault>(
					new DestinationQuantityFault(workingOrderRow.WorkingOrderId, sourceOrderQuantity, destinationOrderQuantity));

		}

		/// <summary>
		/// Handler for validating Destination Order records.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		static void OnDestinationOrderChange(DataModel.DestinationOrderRow destinationOrderRow)
		{

			// A transaction is needed to handle the change.
			DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

			// If the quantity has changed then we need to make sure that the quantity sent to a destination (broker, exchange, etc.) is not less than the amount 
			// ordered.  That is, we can't accept a change that leaves us overcommited with a destination.
			Decimal originalQuantity = (Decimal)destinationOrderRow[DataModel.DestinationOrder.OrderedQuantityColumn, DataRowVersion.Original];
			Decimal currentQuantity = (Decimal)destinationOrderRow[DataModel.DestinationOrder.OrderedQuantityColumn, DataRowVersion.Current];
			if (originalQuantity < currentQuantity)
			{

				// We need to aggregate at the working order, so we need to lock the working order while we do our sums.
				DataModel.WorkingOrderRow workingOrderRow = destinationOrderRow.WorkingOrderRow;
				workingOrderRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
				dataModelTransaction.AddLock(workingOrderRow);

				// This will aggregate the source and destination orders and throw an exception if this transaction would leave us overcommitted with the 
				// destination.
				Decimal sourceOrderQuantity = WorkingOrderService.GetSourceOrderQuantity(dataModelTransaction, workingOrderRow);
				Decimal destinationOrderQuantity = WorkingOrderService.GetDestinationOrderQuantity(dataModelTransaction, workingOrderRow);
				if (sourceOrderQuantity < destinationOrderQuantity)
					throw new FaultException<DestinationQuantityFault>(
						new DestinationQuantityFault(workingOrderRow.WorkingOrderId, sourceOrderQuantity, destinationOrderQuantity));

			}

			// If the StatusCode of the destination order has changed then find the right handler for the state change and go execute the business logic for this
			// change.  The 'statusChangeMap' is a two dimensional Dictionary (hash table) using the previous and current states to find the right handler.
			StatusCode previousStatusCode = (StatusCode)destinationOrderRow[DataModel.DestinationOrder.StatusCodeColumn, DataRowVersion.Original];
			StatusCode currentStatusCode = destinationOrderRow.StatusCode;
			if (previousStatusCode != currentStatusCode)
				statusChangeMap[previousStatusCode][currentStatusCode](destinationOrderRow.WorkingOrderRow);

		}

		/// <summary>
		/// Handler for validating Destination Order records.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		static void OnDestinationOrderDelete(DataModel.DestinationOrderRow destinationOrderRow)
		{

			// A transaction is needed to handle the change.
			DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

			// If the StatusCode of the destination order has changed then find the right handler for the state change and go execute the business logic for this 
			// change.  The 'statusChangeMap' is a two dimensional Dictionary (hash table) using the previous and current states to find the right handler.  Note 
			// that on a deletion, we don't have easy access to the original working order, so we need to extract it from the deleted record.
			StatusCode previousStatusCode = (StatusCode)destinationOrderRow[DataModel.DestinationOrder.StatusCodeColumn, DataRowVersion.Original];
			StatusCode currentStatusCode = StatusCode.Deleted;
			Guid workingOrderId = (Guid)destinationOrderRow[DataModel.DestinationOrder.WorkingOrderIdColumn, DataRowVersion.Original];
			DataModel.WorkingOrderRow workingOrderRow = DataModel.WorkingOrder.WorkingOrderKey.Find(new Object[] { workingOrderId });
			if (workingOrderRow != null)
				statusChangeMap[previousStatusCode][currentStatusCode](workingOrderRow);

		}

		/// <summary>
		/// Validates a change to a Source Order.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event data.</param>
		internal static void OnDestinationOrderRowValidate(object sender, DataModel.DestinationOrderRowChangeEventArgs e)
		{

			// Use the map to call the handler for the action.  Note that I would rather do nothing than incur conditional logic.
			DestinationOrderService.destinationOrderHandlerMap[e.Action](e.Row);

		}

		/// <summary>
		/// Do no action for the working order.
		/// </summary>
		/// <param name="workingOrderRow">The working order.</param>
		static void OnDoNothing(DataModel.WorkingOrderRow workingOrderRow) { }

		/// <summary>
		/// Do nothing.
		/// </summary>
		/// <param name="workingOrderRow">The source order row that was changed.</param>
		static void OnDoNothing(DataModel.DestinationOrderRow destinationOrderRow) { }

		/// <summary>
		/// Handles a change to a filled state.
		/// </summary>        
		/// <param name="workingOrderRow">The parent working order.</param>
		static void OnFilledAction(DataModel.WorkingOrderRow workingOrderRow)
		{

			// A transaction is needed to handle the change.
			DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

			// This will lock the WorkingOrderRow while we examine it.
			workingOrderRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
			dataModelTransaction.AddLock(workingOrderRow);
			if (workingOrderRow.RowState == DataRowState.Detached)
				return;

			// This will mark the working order as filled when the quantity executed is the same as the quantity ordered.  The only exception to this is when ng
			// the workiorder is in an error state.
			if (workingOrderRow.StatusCode != StatusCode.Error)
			{
				Decimal quantityOrdered = WorkingOrderService.GetSourceOrderQuantity(dataModelTransaction, workingOrderRow);
				Decimal quantityExecuted = WorkingOrderService.GetExecutionQuantity(dataModelTransaction, workingOrderRow);
				if (quantityOrdered == quantityExecuted)
					WorkingOrderService.UpdateWorkingOrderStatus(workingOrderRow, StatusCode.Filled);
			}

		}

		/// <summary>
		/// Handles a change to a partially executed state.
		/// </summary>        
		/// <param name="workingOrderRow">The parent working order.</param>
		static void OnPartialAction(DataModel.WorkingOrderRow workingOrderRow)
		{

			// A transaction is needed to handle the change.
			DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

			// This will lock the WorkingOrderRow while we examine it.
			workingOrderRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
			dataModelTransaction.AddLock(workingOrderRow);
			if (workingOrderRow.RowState == DataRowState.Detached)
				return;

			// The working order is considered partially filled if any of its destination orders are partially filled.  The only exception to this is when the
			// working order is in an error state.
			if (workingOrderRow.StatusCode != StatusCode.Error && workingOrderRow.StatusCode != StatusCode.PartiallyFilled)
				WorkingOrderService.UpdateWorkingOrderStatus(workingOrderRow, StatusCode.PartiallyFilled);

		}

		/// <summary>
		/// Handles a change to a error state.
		/// </summary>        
		/// <param name="workingOrderRow">The parent working order.</param>
		static void OnSetErrorAction(DataModel.WorkingOrderRow workingOrderRow)
		{

			// A transaction is needed to handle the change.
			DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

			// This will lock the WorkingOrderRow while we examine it.
			workingOrderRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
			dataModelTransaction.AddLock(workingOrderRow);
			if (workingOrderRow.RowState == DataRowState.Detached)
				return;

			// This logic is simple enough, set to an error state if we're not already there.
			if (workingOrderRow.StatusCode != StatusCode.Error)
				WorkingOrderService.UpdateWorkingOrderStatus(workingOrderRow, StatusCode.Error);

		}

		/// <summary>
		/// Updates the status of the Destination Order.
		/// </summary>
		/// <param name="destinationOrderRow">The Working Order to be modified.</param>
		/// <param name="statusCode">The new status of the Working Order.</param>
		internal static void UpdateDestinationOrderStatus(DataModel.DestinationOrderRow destinationOrderRow, StatusCode statusCode)
		{

			// This will update only the status of the given destination order.
			DataModel.TenantDataModel.UpdateDestinationOrder(
				null,
				null,
				null,
				null,
				null,
				null,
				new Object[] { destinationOrderRow.DestinationOrderId },
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				destinationOrderRow.RowVersion,
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
				null);

		}

	}

}
