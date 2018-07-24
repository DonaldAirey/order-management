namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Security.Permissions;
	using System.ServiceModel;
    
	/// <summary>
	/// Web Services for the Asset Network.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class WebService : IWebService
	{

		/// <summary>
		/// Clears all the destination orders and executions.
		/// </summary>
		[OperationBehavior(TransactionScopeRequired = true)]
		[ClaimsPermission(SecurityAction.Demand, ClaimType = Teraque.AssetNetwork.ClaimTypes.Truncate, Resource = Teraque.Resources.Application)]
		public void ClearOrders()
		{

			// Clear all the executions and then all the destination orders.
			ExecutionService.Truncate();
			DestinationOrderService.Truncate();

		}

		/// <summary>
		/// Creates one or more destination orders.
		/// </summary>
		/// <param name="destinationOrders">An array of structures describing the destination orders to be created.</param>
		[ClaimsPermission(SecurityAction.Demand, ClaimType = Teraque.ClaimTypes.Create, Resource = Teraque.Resources.Application)]
		[OperationBehavior(TransactionScopeRequired = true)]
		public void CreateDestinationOrders(DestinationOrderInfo[] destinationOrders)
		{

			// Create the orders.
			DestinationOrderService.CreateDestinationOrders(destinationOrders);

		}

		/// <summary>
		/// Destroys one or more destination orders.
		/// </summary>
		/// <param name="destinationOrders">An array of structures referencing the destination orders to destroy.</param>
		[ClaimsPermission(SecurityAction.Demand, ClaimType = Teraque.ClaimTypes.Destroy, Resource = Teraque.Resources.Application)]
		[OperationBehavior(TransactionScopeRequired = true)]
		public void DestroyDestinationOrders(DestinationOrderReference[] destinationOrders)
		{

			// Destroy the orders.
			DestinationOrderService.DestroyDestinationOrders(destinationOrders);

		}

		/// <summary>
		/// Gets the tenants currently loaded on this service.
		/// </summary>
		/// <returns>An array of tenant names currently loaded on this service.</returns>
		[ClaimsPermission(SecurityAction.Demand, ClaimType = Teraque.AssetNetwork.ClaimTypes.Configure, Resource = Teraque.Resources.Application)]
		public String[] GetTenants()
		{

			// This is the list of all the tenants currently loaded in the data model.
			return DataModelService.GetTenants();

		}

		/// <summary>
		/// Gets the user's unique identifier.
		/// </summary>
		/// <returns>The user's unique identifier.</returns>		
		[OperationBehavior(TransactionScopeRequired = true)]
		public Guid GetUserId()
		{

			// Return the user's id.
			return UserService.GetUserId();

		}

		/// </summary>
		/// <param name="tenantName">The name of the tenant.</param>
		/// <param name="connectionString">A connection string used to connect the tenant to a database.</param>
		[ClaimsPermission(SecurityAction.Demand, ClaimType = Teraque.AssetNetwork.ClaimTypes.Configure, Resource = Teraque.Resources.Application)]
		public void LoadTenant(String tenantName, String connectionString)
		{

			// This will load the tenant into the server.
			DataModelService.LoadTenant(tenantName, connectionString);

		}

		/// <summary>
		/// Reports an execution.
		/// </summary>
		/// <param name="fixMessage"></param>
		[OperationBehavior(TransactionScopeRequired = true)]
		[ClaimsPermission(SecurityAction.Demand, ClaimType = Teraque.AssetNetwork.ClaimTypes.Report, Resource = Teraque.Resources.Application)]
		public void ReportExecution(Message[] messages)
		{

			// Report the execution.
			ExecutionService.ReportExecution(messages);

		}

		/// <summary>
		/// Starts the support services for a tenant.
		/// </summary>
		[ClaimsPermission(SecurityAction.Demand, ClaimType = Teraque.AssetNetwork.ClaimTypes.Service, Resource = Teraque.Resources.Application)]
		public void Start()
		{

			BusinessRules.Start();
			MarketEngine.Start();

		}

		/// <summary>
		/// Stops the support services for a tenant.
		/// </summary>
		[ClaimsPermission(SecurityAction.Demand, ClaimType = Teraque.AssetNetwork.ClaimTypes.Service, Resource = Teraque.Resources.Application)]
		public void Stop()
		{

			BusinessRules.Stop();
			MarketEngine.Stop();
			DataModel.TenantDataModel.Dispose();

		}

		/// <summary>
		/// Unloads an tenant from the service.
		/// </summary>
		/// <param name="tenantName">The name of the tenant.</param>
		[ClaimsPermission(SecurityAction.Demand, ClaimType = Teraque.AssetNetwork.ClaimTypes.Configure, Resource = Teraque.Resources.Application)]
		public void UnloadTenant(String tenantName)
		{

			// This will unload the organziation from the server.
			DataModelService.UnloadTenant(tenantName);

		}

		/// <summary>
		/// Updates prices using a Canada ticker.
		/// </summary>
		/// <param name="quotes">A collection of price quotes.</param>
		[OperationBehavior(TransactionScopeRequired = true)]
		[ClaimsPermission(SecurityAction.Demand, ClaimType = Teraque.AssetNetwork.ClaimTypes.Quote, Resource = Teraque.Resources.Application)]
		public void UpdateCanadaTickerPrice(Quote[] quotes)
		{

			// Update the price using the US ticker.
			PriceService.UpdateCanadaTickerPrice(quotes);

		}

		/// <summary>
		/// Updates prices using a Cusip ticker.
		/// </summary>
		/// <param name="quotes">A collection of price quotes.</param>
		[OperationBehavior(TransactionScopeRequired = true)]
		[ClaimsPermission(SecurityAction.Demand, ClaimType = Teraque.AssetNetwork.ClaimTypes.Quote, Resource = Teraque.Resources.Application)]
		public void UpdateCusipPrice(Quote[] quotes)
		{

			// Update the price using the US ticker.
			PriceService.UpdateCusipPrice(quotes);

		}

		/// <summary>
		/// Updates currency prices.
		/// </summary>
		/// <param name="quotes">A collection of price quotes.</param>
		[OperationBehavior(TransactionScopeRequired = true)]
		[ClaimsPermission(SecurityAction.Demand, ClaimType = Teraque.AssetNetwork.ClaimTypes.Quote, Resource = Teraque.Resources.Application)]
		public void UpdateCurrencyPrice(Quote[] quotes)
		{

			// Update the price using the US ticker.
			PriceService.UpdateCurrencyPrice(quotes);

		}

		/// <summary>
		/// Updates prices using an aggregator
		/// </summary>
		/// <param name="quotes">A dictionary of prices from various feeds.</param>
		[OperationBehavior(TransactionScopeRequired = true)]
		[ClaimsPermission(SecurityAction.Demand, ClaimType = Teraque.AssetNetwork.ClaimTypes.Quote, Resource = Teraque.Resources.Application)]
		public void UpdatePrice(Dictionary<String, Quote[]> quotes)
		{

			// Update the price using the US ticker.
			PriceService.UpdatePrice(quotes);

		}

		/// <summary>
		/// Updates prices using a United Kingdom ticker.
		/// </summary>
		/// <param name="quotes">A collection of price quotes.</param>
		[OperationBehavior(TransactionScopeRequired = true)]
		[ClaimsPermission(SecurityAction.Demand, ClaimType = Teraque.AssetNetwork.ClaimTypes.Quote, Resource = Teraque.Resources.Application)]
		public void UpdateUnitedKingdomTickerPrice(Quote[] quotes)
		{

			// Update the price using the US ticker.
			PriceService.UpdateUnitedKingdomTickerPrice(quotes);

		}

		/// <summary>
		/// Updates prices using a United States ticker.
		/// </summary>
		/// <param name="quotes">A collection of price quotes.</param>
		[OperationBehavior(TransactionScopeRequired = true)]
		[ClaimsPermission(SecurityAction.Demand, ClaimType = Teraque.AssetNetwork.ClaimTypes.Quote, Resource = Teraque.Resources.Application)]
		public void UpdateUnitedStatesTickerPrice(Quote[] quotes)
		{

			// Update the price using the US ticker.
			PriceService.UpdateUnitedStatesTickerPrice(quotes);

		}

	}

}
