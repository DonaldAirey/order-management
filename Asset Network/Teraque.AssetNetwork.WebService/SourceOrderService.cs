namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.ServiceModel;

	/// <summary>
	/// Business rules for Source Orders.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal class SourceOrderService
	{

		/// <summary>
		/// Handles a change to a Source Order.
		/// </summary>
		/// <param name="sourceOrderRow">The source order that has changed.</param>
		delegate void SourceOrderRowHandler(DataModel.SourceOrderRow sourceOrderRow);

		/// <summary>
		/// Maps an action to a handler for that action.
		/// </summary>
		static Dictionary<DataRowAction, SourceOrderRowHandler> sourceOrderHandlerMap = new Dictionary<DataRowAction, SourceOrderRowHandler>()
		{
			{DataRowAction.Add, SourceOrderService.OnDoNothing},
			{DataRowAction.Change, SourceOrderService.OnSourceOrderChange},
			{DataRowAction.Commit, SourceOrderService.OnDoNothing},
			{DataRowAction.Delete, SourceOrderService.OnSourceOrderDelete},
			{DataRowAction.Rollback, SourceOrderService.OnDoNothing}
		};

		/// <summary>
		/// Do nothing.
		/// </summary>
		/// <param name="sourceOrderRow">The source order row that was changed.</param>
		static void OnDoNothing(DataModel.SourceOrderRow sourceOrderRow) { }

		/// <summary>
		/// Validates the source order row when it changes.
		/// </summary>
		/// <param name="sourceOrderRow">The source order row that was changed.</param>
		static void OnSourceOrderChange(DataModel.SourceOrderRow sourceOrderRow)
		{

			// We can't allow the quantity of source order shares to drop below the quantity of destination order shares.  We only need to check for this condition 
			// when the quantity of the source order record has changed.
			Decimal originalQuantity = (Decimal)sourceOrderRow[DataModel.SourceOrder.OrderedQuantityColumn, DataRowVersion.Original];
			Decimal currentQuantity = (Decimal)sourceOrderRow[DataModel.SourceOrder.OrderedQuantityColumn, DataRowVersion.Current];
			if (originalQuantity < currentQuantity)
			{

				// We'll need to lock several records in order to check source order and destination order totals.
				DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

				// Get the working order associated with this source order and lock it.
				DataModel.WorkingOrderRow workingOrderRow = sourceOrderRow.WorkingOrderRow;
				workingOrderRow.AcquireReaderLock(dataModelTransaction);

				// Now aggregate the source order quantities and the destination order quantities.  Throw an exception if the source orders are less than the
				// destination orders (we can't have less quantity ordered than we've placed with brokers and exchanges).
				Decimal sourceOrderQuantity = WorkingOrderService.GetSourceOrderQuantity(dataModelTransaction, workingOrderRow);
				Decimal destinationOrderQuantity = WorkingOrderService.GetDestinationOrderQuantity(dataModelTransaction, workingOrderRow);
				if (sourceOrderQuantity < destinationOrderQuantity)
					throw new FaultException<DestinationQuantityFault>(
						new DestinationQuantityFault(workingOrderRow.WorkingOrderId, sourceOrderQuantity, destinationOrderQuantity));

			}

		}

		/// <summary>
		/// Validates the source order row when it is deleted.
		/// </summary>
		/// <param name="sourceOrderRow">The source order row that was deleted.</param>
		static void OnSourceOrderDelete(DataModel.SourceOrderRow sourceOrderRow)
		{

			// We'll need to add several rows to the transaction as we validate the source order deletion.
			DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

			// There is no implicit locking mechanism for deleted rows, so we need to lock the record manually as we access the parent working order of the deleted 
			// source order.
			DataModel.WorkingOrderRow workingOrderRow = null;
			try
			{
				DataModel.DataLock.EnterReadLock();
				workingOrderRow = ((DataModel.WorkingOrderRow)(sourceOrderRow.GetParentRow(DataModel.SourceOrder.WorkingOrderSourceOrderRelation, DataRowVersion.Original)));
			}
			finally
			{
				DataModel.DataLock.ExitReadLock();
			}
			workingOrderRow.AcquireReaderLock(dataModelTransaction);

			// This will insure that the quantity ordered doesn't fall below the quantity placed with brokers and exchanges.
			Decimal sourceOrderQuantity = WorkingOrderService.GetSourceOrderQuantity(dataModelTransaction, workingOrderRow);
			Decimal destinationOrderQuantity = WorkingOrderService.GetDestinationOrderQuantity(dataModelTransaction, workingOrderRow);
			if (sourceOrderQuantity < destinationOrderQuantity)
				throw new FaultException<DestinationQuantityFault>(
					new DestinationQuantityFault(workingOrderRow.WorkingOrderId, sourceOrderQuantity, destinationOrderQuantity));

		}

		/// <summary>
		/// Validates a change to a Source Order.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event data.</param>
		internal static void OnSourceOrderRowValidate(object sender, DataModel.SourceOrderRowChangeEventArgs e)
		{

			// Use the map to call the handler for the action.  Note that I would rather do nothing than incur conditional logic.
			SourceOrderService.sourceOrderHandlerMap[e.Action](e.Row);

		}

	}

}
