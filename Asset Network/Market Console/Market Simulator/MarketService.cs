namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.ServiceModel;
	using System.Text;

	/// <summary>
	/// Market Simulation.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class MarketService : IMarketService
	{

        /// <summary>
        /// Clears the orders from the simulator.
        /// </summary>
        /// <param name="message">A message containing an order.</param>
        public void ClearOrders()
        {

            // This will clear out the simulator of any orders.
            lock (MarketData.SyncRoot)
            {
                MarketData.Order.Clear();
                MarketData.Order.AcceptChanges();
            }

        }

        /// <summary>
        /// Execute an order.
        /// </summary>
        /// <param name="message">A message containing an order.</param>
        public void ExecuteOrder(Message[] messages)
        {

            // We really can only add an order to the simulated broker's book from here.  It will be executed sometime later by the broker simulator.
            foreach (Message message in messages)
                BrokerSimulator.AddOrder(message);

        }

        /// <summary>
		/// Get simulation parameters.
		/// </summary>
		/// <returns>SimulatorParameters</returns>		
		public SimulatorParameters GetSimulatorParameters()
		{

			// Provide the client with the simulator settings.
			return MarketSimulator.SimulatorParameters;

		}

		/// <summary>
		/// Sets the simulator parameters.
		/// </summary>
		/// <param name="simulatorParameters">The simulator parameters.</param>
		public void SetSimulatorParameters(SimulatorParameters simulatorParameters)
		{

			// This will reconcile the simulator parameters to the specified settings.
			MarketSimulator.SimulatorParameters = simulatorParameters;

		}

	}

}
