namespace Teraque.AssetNetwork
{

	using Teraque;
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.ServiceModel;

	/// <summary>
	/// Summary description for Execution.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal class ExecutionService
	{

		// Private Static Fields
		static DataModel dataModel;

		/// <summary>
		/// Initialize the static elements of the DestinationOrder.
		/// </summary>
		static ExecutionService()
		{

			// An instance of the data model is required to make changes to it.
			ExecutionService.dataModel = new DataModel();

		}

		/// <summary>
		/// Truncates the Execution table.
		/// </summary>
		internal static void Truncate()
		{

			// We need a transaction in order to scan the Execution table.
			DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;

			// We can't modify a list while iterating through it, so we need to collect all the Execution records in this list.  Then when we start to
			// destroy them, we won't disturb the iterator.
			List<DestroyInfo> destroyInfoList = new List<DestroyInfo>();

			try
			{

				// A reader lock is required to protect the table iterators while we collect all the Executions.
				DataModel.DataLock.EnterReadLock();

				// This will collect all the Executions.
				foreach (DataModel.ExecutionRow executionRow in DataModel.Execution)
				{
					executionRow.AcquireReaderLock(dataModelTransaction);
					dataModelTransaction.AddLock(executionRow);
					destroyInfoList.Add(new DestroyInfo(new Object[] { executionRow.ExecutionId }, executionRow.RowVersion));
				}

			}
			finally
			{

				// We don't need to protect the Execution table iterators any longer.
				DataModel.DataLock.ExitReadLock();

			}

			// At this point we've collected all the Executions in the table.  This will call the internal method to destroy them.
			foreach (DestroyInfo destroyInfo in destroyInfoList)
				DataModel.TenantDataModel.DestroyExecution(destroyInfo.Key, destroyInfo.RowVersion);

		}

		/// <summary>
		/// Handler for validating Execution records.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		internal static void OnExecutionRowValidate(object sender, DataModel.ExecutionRowChangeEventArgs e)
		{

			DataModel.DestinationOrderRow destinationOrderRow;
			Decimal executedQuantity;
			DataModelTransaction dataModelTransaction;
			Decimal orderedQuantity;

			// The Business Rules will be enforced on this Execution.  Note that it is locked at the point this handler is called.
			DataModel.ExecutionRow executionRow = e.Row;

			// The action on the row determines which rule to evaluate.
			switch (e.Action)
			{

				case DataRowAction.Add:

					// This rule will update the status of the owning Destination Order based on the quantity executed.
					dataModelTransaction = DataModel.CurrentTransaction;

					// The aggregate of the quantities executed will determine whether the Destination Order requires a status change.
					destinationOrderRow = executionRow.DestinationOrderRow;
					destinationOrderRow.AcquireReaderLock(dataModelTransaction);
					if (destinationOrderRow.RowState == DataRowState.Detached)
						return;

					// This calculates the quantity outstanding on an order and the quantity executed on the order.
					orderedQuantity = destinationOrderRow.OrderedQuantity - destinationOrderRow.CanceledQuantity;
					executedQuantity = 0.0M;
					foreach (DataModel.ExecutionRow siblingRow in destinationOrderRow.GetExecutionRows())
					{
						siblingRow.AcquireReaderLock(dataModelTransaction);
						executedQuantity += siblingRow.ExecutionQuantity;
					}

					// This rule will set the status on the owning Destination Order based on the amont outstanding and the amount ordered.  First, if the quantity
					// executed is greater than the quantity ordered, the Order goes into an error state.  We can't reject executions as they come from an external
					// source, but the order can be flag and put into an error state.
					if (executedQuantity > orderedQuantity)
					{
						if (destinationOrderRow.StatusCode != StatusCode.Error)
							DestinationOrderService.UpdateDestinationOrderStatus(destinationOrderRow, StatusCode.Error);
					}
					else
					{

						// When the total quantity executed is reduced to zero then the order goes back into the initial state.
						if (executedQuantity == 0.0m)
						{
							if (destinationOrderRow.StatusCode != StatusCode.New)
								DestinationOrderService.UpdateDestinationOrderStatus(destinationOrderRow, StatusCode.New);
						}
						else
						{

							// While the executed quantity is still less than the outstanding quantity the order is consdered to be partially filled.  Otherwise,
							// the order is completely filled.
							if (executedQuantity < orderedQuantity)
							{
								if (destinationOrderRow.StatusCode != StatusCode.PartiallyFilled)
									DestinationOrderService.UpdateDestinationOrderStatus(destinationOrderRow, StatusCode.PartiallyFilled);
							}
							else
							{
								if (destinationOrderRow.StatusCode != StatusCode.Filled)
									DestinationOrderService.UpdateDestinationOrderStatus(destinationOrderRow, StatusCode.Filled);
							}

						}

					}

					break;

				case DataRowAction.Delete:

					// A middle tier context is required to lock the rows so the quantities can be aggregated.
					dataModelTransaction = DataModel.CurrentTransaction;

					// The aggregate of the quantities executed will determine whether the Destination Order requires a status change.
					destinationOrderRow = (DataModel.DestinationOrderRow)executionRow.GetParentRow(
						DataModel.Execution.DestinationOrderExecutionRelation,
						DataRowVersion.Original);
					destinationOrderRow.AcquireReaderLock(dataModelTransaction);
					if (destinationOrderRow.RowState == DataRowState.Detached)
						return;

					// This calculates the quantity outstanding on an order and the quantity executed on the order.
					orderedQuantity = destinationOrderRow.OrderedQuantity - destinationOrderRow.CanceledQuantity;
					executedQuantity = 0.0M;
					foreach (DataModel.ExecutionRow siblingRow in destinationOrderRow.GetExecutionRows())
					{
						siblingRow.AcquireReaderLock(dataModelTransaction);
						executedQuantity += siblingRow.ExecutionQuantity;
					}

					// When the total quantity executed is reduced to zero then the order goes back into the initial state.  Note that it is impossible for the
					// order to transition into a error state by deleting an execution.
					if (executedQuantity == 0.0m)
					{
						if (destinationOrderRow.StatusCode != StatusCode.New)
							DestinationOrderService.UpdateDestinationOrderStatus(destinationOrderRow, StatusCode.New);
					}
					else
					{

						// While the executed quantity is still less than the outstanding quantity the order is consdered to be partially filled.  Note that it is
						// impossible for the order to transition to a filled state by deleting an execution.
						if (executedQuantity < orderedQuantity)
						{
							if (destinationOrderRow.StatusCode != StatusCode.PartiallyFilled)
								DestinationOrderService.UpdateDestinationOrderStatus(destinationOrderRow, StatusCode.PartiallyFilled);
						}

					}

					break;

			}

		}

		/// <summary>
		/// Reports an execution.
		/// </summary>
		/// <param name="message">The tag based message containing the execution report.</param>
		internal static void ReportExecution(Message[] messages)
		{

			Guid userId = UserService.GetUserId();

			foreach (Message message in messages)
			{

				try
				{
					TenantDataModel tenantDataModel = DataModel.TenantDataModel;
					tenantDataModel.CreateExecution(
						null,
						null,
						null,
						null,
						DateTime.Now,
						userId,
						new Guid(message.ClOrdID),
						StateCode.Acknowledged,
						Guid.NewGuid(),
						message.Price,
						message.CumQty,
						null,
						null,
						DateTime.Now,
						userId,
						null,
						null,
						null,
						null,
						StateCode.Acknowledged,
						null,
						null,
						null,
						null);

				}
				catch { }

			}

		}

	}

}
