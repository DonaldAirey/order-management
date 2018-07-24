namespace Teraque.AssetNetwork
{

	using Teraque;
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.IdentityModel.Policy;
	using System.IdentityModel.Claims;
	using System.Security.Principal;
	using System.ServiceModel;
	using System.Threading;
	using System.Transactions;

	/// <summary>
	/// Manages the business logic for the server data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal class BusinessRules
	{

		/// <summary>
		/// Starts the business rules for a tenant.
		/// </summary>
		internal static void Start()
		{

			// Install the validators that will provide the business logic for the trading tables.
			DataModel.DestinationOrder.DestinationOrderRowValidate += DestinationOrderService.OnDestinationOrderRowValidate;
			DataModel.Execution.ExecutionRowValidate += ExecutionService.OnExecutionRowValidate;
			DataModel.SourceOrder.SourceOrderRowValidate += SourceOrderService.OnSourceOrderRowValidate;
			DataModel.WorkingOrder.WorkingOrderRowValidate += WorkingOrderService.OnWorkingOrderRowValidate;

		}

		/// <summary>
		/// Stops the business rules for a tenant.
		/// </summary>
		internal static void Stop()
		{

			// Remove the validators from the data model.
			DataModel.DestinationOrder.DestinationOrderRowValidate -= DestinationOrderService.OnDestinationOrderRowValidate;
			DataModel.Execution.ExecutionRowValidate -= ExecutionService.OnExecutionRowValidate;
			DataModel.SourceOrder.SourceOrderRowValidate -= SourceOrderService.OnSourceOrderRowValidate;
			DataModel.WorkingOrder.WorkingOrderRowValidate -= WorkingOrderService.OnWorkingOrderRowValidate;

		}

	}

}
