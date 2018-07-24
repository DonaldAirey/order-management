namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.ServiceModel;
	using System.Transactions;
	using Teraque;
	using Teraque.AssetNetwork;

	/// <summary>
	/// Business rules for Status.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal class Status
	{

		/// <summary>
		/// Gets the StatusCode from the StatusId.
		/// </summary>
		internal static StatusCode FromId(DataModelTransaction dataModelTransaction, Guid statusId)
		{

			DataModel.StatusRow statusRow = DataModel.Status.StatusKey.Find(statusId);
			statusRow.AcquireReaderLock(dataModelTransaction);
			if (statusRow.RowState != System.Data.DataRowState.Detached)
				throw new FaultException<RecordNotFoundFault>(new RecordNotFoundFault("Status", new Object[] { statusId }));
			return statusRow.StatusCode;

		}

		/// <summary>
		/// Gets the StatusId from the StatusCode.
		/// </summary>
		internal static Guid FromCode(DataModelTransaction dataModelTransaction, StatusCode statusCode)
		{

			DataModel.StatusRow statusRow = DataModel.Status.StatusKeyStatusCode.Find(statusCode);
			statusRow.AcquireReaderLock(dataModelTransaction);
			if (statusRow.RowState != System.Data.DataRowState.Detached)
				throw new FaultException<RecordNotFoundFault>(new RecordNotFoundFault("Status", new Object[] { statusCode }));
			return statusRow.StatusId;

		}

	}

}
