namespace Teraque.AssetNetwork
{

	using Teraque;
	using System;
	using System.ComponentModel;
	using System.Configuration;
	using System.Collections;
	using System.Data;
	using System.ServiceModel;
	using System.Security.Principal;
	using System.Threading;
	using System.Transactions;
	using System.Collections.Generic;
	using System.IdentityModel.Claims;
	using System.IdentityModel.Policy;
	using System.IdentityModel.Selectors;
	using System.IdentityModel.Tokens;

	/// <summary>
	/// Manages the business logic for the server data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class OperationManager
	{

		/// <summary>
		/// The operating parameters.
		/// </summary>
		static OperationParameters operatingParameters;

		/// <summary>
		/// Used to coordinate threads.
		/// </summary>
		static Object syncRoot;

		/// <summary>
		/// Initialize a new instance of the OperationManager class.
		/// </summary>
		static OperationManager()
		{

			// This object is used by the System.Monitor methods that control multithreaded access to the data in this class.
			OperationManager.syncRoot = new Object();

			// These are the initial operating parameters for the business rules.
			OperationParameters operatingParameters = new OperationParameters();
			operatingParameters.AreBusinessRulesActive = true;
			operatingParameters.IsCrossingActive = true;
			operatingParameters.IsChatActive = true;
			OperationManager.OperatingParameters = operatingParameters;

		}

		/// <summary>
		/// Gets or sets the thread safe parameters that control the trading operations.
		/// </summary>
		public static OperationParameters OperatingParameters
		{

			get
			{

				// These parameters are shared by different threads and must be locked before reading.
				lock (OperationManager.syncRoot)
					return OperationManager.operatingParameters;

			}
			set
			{

				// These parameters are shared by different threads.
				lock (OperationManager.syncRoot)
				{

					// The saved parameters reflect the state of the simulation.
					OperationManager.operatingParameters = value;

					// Start or stop the business rules.
					BusinessRules.AreBusinessRulesActive = OperationManager.operatingParameters.AreBusinessRulesActive;

				}

			}

		}

	}

}
