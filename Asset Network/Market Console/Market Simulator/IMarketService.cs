namespace Teraque.AssetNetwork
{

	using System;
	using System.ServiceModel;
	using Teraque;

	/// <summary>
	/// Interface for the market simulation.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[ServiceContract]
	public interface IMarketService
	{

        /// <summary>
        /// Get the simulation parameters.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        void ClearOrders();

        /// <summary>
        /// Executes a batch of orders on the simulated exchange.
        /// </summary>
        /// <param name="message"></param>
        [OperationContract]
        [ServiceKnownType(typeof(OrderTypeCode))]
        [ServiceKnownType(typeof(OrderStatusCode))]
        [ServiceKnownType(typeof(SideCode))]
        [ServiceKnownType(typeof(StatusCode))]
        [ServiceKnownType(typeof(TimeInForceCode))]
        void ExecuteOrder(Message[] messages);

        /// <summary>
		/// Get the simulation parameters.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		SimulatorParameters GetSimulatorParameters();

		/// <summary>
		/// Set the simulation parameters.
		/// </summary>
		/// <param name="simulationParameters"></param>
		[OperationContract]
		void SetSimulatorParameters(SimulatorParameters simulationParameters);

	}

}
