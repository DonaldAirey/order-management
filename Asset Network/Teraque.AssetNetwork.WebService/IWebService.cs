namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Runtime.Serialization;
	using System.ServiceModel;

	/// <summary>
	/// Web Service Contract
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	[ServiceContract]
	public interface IWebService
	{

		/// <summary>
		/// Clears all the destination orders and executions.
		/// </summary>
		[OperationContract]
		void ClearOrders();

		/// <summary>
		/// Creates one or more destination orders.
		/// </summary>
		/// <param name="destinationOrders">An array of structures describing the destination orders to be created.</param>
		[OperationContract]
		void CreateDestinationOrders(DestinationOrderInfo[] destinationOrders);

		/// <summary>
		/// Destroys one or more destination orders.
		/// </summary>
		/// <param name="destinationOrders">An array of structures referencing the destination orders to destroy.</param>
		[OperationContract]
		void DestroyDestinationOrders(DestinationOrderReference[] destinationOrders);

		/// <summary>
		/// Gets the tenants currently loaded on this service.
		/// </summary>
		/// <returns>An array of tenant names currently loaded on this service.</returns>
		[OperationContract]
		String[] GetTenants();

		/// <summary>
		/// Get user identifier of the current user.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		[FaultContract(typeof(RecordNotFoundFault))]
		Guid GetUserId();

		/// <summary>
		/// Loads an tenant into the service.
		/// </summary>
		/// <param name="tenantName">The name of the tenant.</param>
		/// <param name="connectionString">A connection string used to connect the tenant to a database.</param>
		[OperationContract]
		void LoadTenant(String tenantName, String connectionString);

		/// <summary>
		/// Reports an execution.
		/// </summary>
		/// <param name="message">The execution information.</param>
		[OperationContract]
        [ServiceKnownType(typeof(Message))]
        void ReportExecution(Message[] messages);

		/// <summary>
		/// Starts the web service support.
		/// </summary>
		[OperationContract]
		void Start();

		/// <summary>
		/// Stops the web service support.
		/// </summary>
		[OperationContract]
		void Stop();

		/// <summary>
		/// Unloads an tenant from the service.
		/// </summary>
		/// <param name="tenantName">The name of the tenant.</param>
		[OperationContract]
		void UnloadTenant(String tenantName);

		/// <summary>
		/// Updates prices using a Canadian ticker.
		/// </summary>
		/// <param name="quotes">A collection of price quotes.</param>
		[OperationContract]
		void UpdateCanadaTickerPrice(Quote[] quotes);

		/// <summary>
		/// Updates prices using a CUSIP.
		/// </summary>
		/// <param name="quotes">A collection of price quotes.</param>
		[OperationContract]
		void UpdateCusipPrice(Quote[] quotes);

		/// <summary>
		/// Updates currency prices.
		/// </summary>
		/// <param name="quotes">A collection of price quotes.</param>
		[OperationContract]
		void UpdateCurrencyPrice(Quote[] quotes);

		/// <summary>
		/// Updates prices using an aggregator
		/// </summary>
		/// <param name="quotes">A dictionary of prices from various feeds.</param>
		[OperationContract]
		void UpdatePrice(Dictionary<String, Quote[]> quotes);

		/// <summary>
		/// Updates prices using a United Kingdom ticker.
		/// </summary>
		/// <param name="quotes">A collection of price quotes.</param>
		[OperationContract]
		void UpdateUnitedKingdomTickerPrice(Quote[] quotes);

		/// <summary>
		/// Updates prices using a United States ticker.
		/// </summary>
		/// <param name="quotes">A collection of price quotes.</param>
		[OperationContract]
		void UpdateUnitedStatesTickerPrice(Quote[] quotes);

	}

}
