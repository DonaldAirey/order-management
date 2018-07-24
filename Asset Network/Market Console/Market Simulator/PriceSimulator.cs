namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.ServiceModel;
	using System.Threading;
	using Teraque.AssetNetwork.WebService;

	/// <summary>
	/// Simulates price changes.
	/// </summary>
	internal static class PriceSimulator
	{

		/// <summary>
		/// Different price quotes.
		/// </summary>
		enum QuoteType { Ask, Bid, Last };

		/// <summary>
		/// Gets or sets an indication of whether or not the prices have been initialized.
		/// </summary>
		static Boolean arePricesInitialized;

		/// <summary>
		/// Subscribers to currency prices.
		/// </summary>
		static List<ClientInfo> currencyClients = new List<ClientInfo>()
		{
			new ClientInfo() {EndpointName = "TcpWebServiceUserNameEndpoint", UserName="CN=Administrator,OU=Aspen Group,O=Dark Bond,DC=darkbond,DC=com", Password="Teraque"}
		};

		/// <summary>
		/// Subscribers to debt prices.
		/// </summary>
		static List<ClientInfo> debtClients = new List<ClientInfo>()
		{
			new ClientInfo() {EndpointName = "TcpWebServiceUserNameEndpoint", UserName="CN=Administrator,OU=Aspen Group,O=Dark Bond,DC=darkbond,DC=com", Password="Teraque"}
		};

		/// <summary>
		/// Subscribers to equity prices.
		/// </summary>
		static List<ClientInfo> equityClients = new List<ClientInfo>()
		{
			new ClientInfo() {EndpointName = "TcpWebServiceUserNameEndpoint", UserName="CN=Administrator,OU=Aspen Group,O=Dark Bond,DC=darkbond,DC=com", Password="Teraque"}
		};

		/// <summary>
		/// The frequency of equity price changes.
		/// </summary>
		static Double equityFrequencyField = 10;

		/// <summary>
		/// The time between price updates to the server.
		/// </summary>
		const Int32 interval = 250;

		/// <summary>
		/// The thread used to simulate equity prices.
		/// </summary>
		static Thread priceSimulationThread;

		/// <summary>
		/// Used for multithreaded access to operational parameters.
		/// </summary>
		static Object syncRoot = new Object();

		/// <summary>
		/// Initializes the server prices with the simulator prices for currency.
		/// </summary>
		static void InitializeCurrencyPrice()
		{

			// This creates a list of subscribers to the currency price updates.
			List<WebServiceClient> webServiceClients = new List<WebServiceClient>();
			foreach (ClientInfo clientInfo in PriceSimulator.currencyClients)
			{
				WebServiceClient webServiceClient = new WebServiceClient(clientInfo.EndpointName);
				webServiceClient.ClientCredentials.UserName.UserName = clientInfo.UserName;
				webServiceClient.ClientCredentials.UserName.Password = clientInfo.Password;
				webServiceClients.Add(webServiceClient);
			}

			// The quotes are collected here and sent to the server when we have them all.
			List<Quote> quotes = new List<Quote>();

			// The data model must be locked while we query it for currency prices.
			lock (MarketData.SyncRoot)
			{

				// This provides a view for equity prices
				DataView currencyView = new DataView(MarketData.Price);
				currencyView.RowFilter = "SecurityType='CURRENCY'";

				// This will segregate the quotes into the different feeds for all the equities in the simulator.
				foreach (DataRowView dataRowView in currencyView)
				{
					MarketDataModel.PriceRow priceRow = dataRowView.Row as MarketDataModel.PriceRow;
					quotes.Add(new Quote()
					{
						AskPrice = priceRow.AskPrice,
						AskSize = priceRow.AskSize,
						BidPrice = priceRow.BidPrice,
						BidSize = priceRow.BidSize,
						LastPrice = priceRow.LastPrice,
						LastSize = priceRow.LastSize,
						Symbol = priceRow.Symbol
					});
				}

			}

			// A real-world ticker feed will use a single scheme for identifying the securities.  Those schemes are not unique and will often overlap.  This
			// simulates a feed from multiple real-world vendors with different methods of identifying securities.  Each organization that has subscrived to
			// this simluator will get a copy of the batch.
			foreach (WebServiceClient webServiceClient in webServiceClients)
				webServiceClient.UpdateCurrencyPrice(quotes.ToArray());

		}

		/// <summary>
		/// Initializes the server prices with the simulator prices.
		/// </summary>
		static void InitializeDebtPrice()
		{

			// This creates a list of subscribers to the currency price updates.
			List<WebServiceClient> webServiceClients = new List<WebServiceClient>();
			foreach (ClientInfo clientInfo in PriceSimulator.debtClients)
			{
				WebServiceClient webServiceClient = new WebServiceClient(clientInfo.EndpointName);
				webServiceClient.ClientCredentials.UserName.UserName = clientInfo.UserName;
				webServiceClient.ClientCredentials.UserName.Password = clientInfo.Password;
				webServiceClients.Add(webServiceClient);
			}

			// The quotes are collected here and sent to the server when we have them all.
			List<Quote> quotes = new List<Quote>();

			// The market data must be locked while we query it for bond prices.
			lock (MarketData.SyncRoot)
			{

				// This provides a view for equity prices
				DataView debtView = new DataView(MarketData.Price);
				debtView.RowFilter = "SecurityType='DEBT'";

				// This will segregate the quotes into the different feeds for all the equities in the simulator.
				foreach (DataRowView dataRowView in debtView)
				{
					MarketDataModel.PriceRow priceRow = dataRowView.Row as MarketDataModel.PriceRow;
					quotes.Add(new Quote()
					{
						AskPrice = priceRow.AskPrice,
						AskSize = priceRow.AskSize,
						BidPrice = priceRow.BidPrice,
						BidSize = priceRow.BidSize,
						LastPrice = priceRow.LastPrice,
						LastSize = priceRow.LastSize,
						Symbol = priceRow.Symbol
					});
				}

			}

			// This will set the prices of the USD securities that use CUSIPs.
			foreach (WebServiceClient webServiceClient in webServiceClients)
				webServiceClient.UpdateCusipPrice(quotes.ToArray());

		}

		/// <summary>
		/// Initializes the server prices with the simulator prices.
		/// </summary>
		static void InitializeEquityPrice()
		{

			// This creates a list of subscribers to the currency price updates.
			List<WebServiceClient> webServiceClients = new List<WebServiceClient>();
			foreach (ClientInfo clientInfo in PriceSimulator.equityClients)
			{
				WebServiceClient webServiceClient = new WebServiceClient(clientInfo.EndpointName);
				webServiceClient.ClientCredentials.UserName.UserName = clientInfo.UserName;
				webServiceClient.ClientCredentials.UserName.Password = clientInfo.Password;
				webServiceClients.Add(webServiceClient);
			}

			// The price feeds from vendors will rely on their own unique identifiers.  This simulator tries to emulate this architecture by providing different web
			// services that recognize different unique identifiers.  This dictionary will segregate the prices by their identifiers.
			Dictionary<String, List<Quote>> quotes = new Dictionary<String, List<Quote>>()
			{
				{"CA TICKER", new List<Quote>()},
				{"UK TICKER", new List<Quote>()},
				{"US TICKER", new List<Quote>()}
			};

			// This will lock the market data while we query it for equity prices.
			lock (MarketData.SyncRoot)
			{

				// This provides a view for equity prices
				DataView equityView = new DataView(MarketData.Price);
				equityView.RowFilter = "SecurityType='EQUITY'";

				// This will segregate the quotes into the different feeds for all the equities in the simulator.
				foreach (DataRowView dataRowView in equityView)
				{
					MarketDataModel.PriceRow priceRow = dataRowView.Row as MarketDataModel.PriceRow;
					quotes[priceRow.Feed].Add(new Quote()
					{
						AskPrice = priceRow.AskPrice,
						AskSize = priceRow.AskSize,
						BidPrice = priceRow.BidPrice,
						BidSize = priceRow.BidSize,
						LastPrice = priceRow.LastPrice,
						LastSize = priceRow.LastSize,
						Symbol = priceRow.Symbol
					});
				}

			}

			// The web service has trouble with lists so we need to convert the collections to arrays before passing them.
			Dictionary<String, Quote[]> serviceQuotes = new Dictionary<String, Quote[]>()
			{
				{"CA TICKER", quotes["CA TICKER"].ToArray()},
				{"UK TICKER", quotes["UK TICKER"].ToArray()},
				{"US TICKER", quotes["US TICKER"].ToArray()}
			};

			// A real-world ticker feed will use a single scheme for identifying the securities.  Those schemes are not unique and will often overlap.  This
			// simulates a feed from multiple real-world vendors with different methods of identifying securities.  Each organization that has subscrived to
			// this simluator will get a copy of the batch.
			foreach (WebServiceClient webServiceClient in webServiceClients)
				webServiceClient.UpdatePrice(serviceQuotes);

		}

		/// <summary>
		/// Simulation of price changes in a stock market.
		/// </summary>
		static void SimulatePrice()
		{

			// This will seed the random number generator.
			Random random = new Random(DateTime.Now.Millisecond);

			// This creates a list of subscribers to the equity price updates.
			List<WebServiceClient> webServiceClients = new List<WebServiceClient>();
			foreach (ClientInfo clientInfo in PriceSimulator.equityClients)
			{
				WebServiceClient webServiceClient = new WebServiceClient(clientInfo.EndpointName);
				webServiceClient.ClientCredentials.UserName.UserName = clientInfo.UserName;
				webServiceClient.ClientCredentials.UserName.Password = clientInfo.Password;
				webServiceClients.Add(webServiceClient);
			}

			// The market data has different securities but we are only interested in simulating price changes for equities here.
			DataView equityView = null;
			lock (MarketData.SyncRoot)
			{
				equityView = new DataView(MarketData.Price);
				equityView.RowFilter = "SecurityType='EQUITY'";
			}

			// This thread will run until terminated externally.
			while (true)
			{

				// The price feeds from vendors will rely on their own unique identifiers.  This simulator tries to emulate this architecture by providing 
				// different web services that recognize different unique identifiers.  This dictionary will segregate the prices by their identifiers.
				Dictionary<String, List<Quote>> quotes = new Dictionary<String, List<Quote>>()
					{
						{"CA TICKER", new List<Quote>()},
						{"UK TICKER", new List<Quote>()},
						{"US TICKER", new List<Quote>()}
					};

				// In order to minimize the overhead for creating transaction to service the prices, the prices are batched up and processed in a single
				// transaction. The desired frequence determines the number of prices changed in a single batch since the frequence of the thread is fixed
				// through constants.  Note that the 'EquityFrequency' property is thread safe, so it can be changed from the outside.
				Int32 batchSize = Convert.ToInt32(PriceSimulator.EquityFrequency * Convert.ToDouble(PriceSimulator.interval) / 1000.0);

				// The prices are not updated continously because this would be a serious drain on the web services.  Instead, the prices are batched up and sent in
				// bulk.  This introduces a latency that is no greater than the PriceSimulator.interval value.
				for (Int32 index = 0; index < batchSize; index++)
				{


					// The market data must be locked while we query and change the prices.  However, a batch of quotes is built here which will be sent to the
					// subscribers after releasing the lock.
					lock (MarketData.SyncRoot)
					{

						// This will select a random price in the table and change the price by a random amount.
						MarketDataModel.PriceRow priceRow = equityView[random.Next(equityView.Count)].Row as MarketDataModel.PriceRow;

						// This will simulate a random selection of a price change is quoted. The 'quoteColumn' variable can be used to reference the selected quote
						// from any price row.  Below, a random row will be selected for the price change.
						Int32 quoteTypeIndex = random.Next(Enum.GetValues(typeof(QuoteType)).Length);
						switch ((QuoteType)Enum.GetValues(typeof(QuoteType)).GetValue(quoteTypeIndex))
						{

						case QuoteType.Ask:

							// Move the 'Ask' price.
							priceRow.AskPrice += Convert.ToDecimal(Math.Round(random.NextDouble() - 0.5, 2));
							priceRow.AskSize = Convert.ToDecimal(random.Next(1, 10));
							break;

						case QuoteType.Bid:

							// Move the 'Bid' price.
							priceRow.BidPrice += Convert.ToDecimal(Math.Round(random.NextDouble() - 0.5, 2));
							priceRow.BidSize = Convert.ToDecimal(random.Next(1, 10));
							break;

						case QuoteType.Last:

							// Move the 'Last' price.
							priceRow.LastPrice += Convert.ToDecimal(Math.Round(random.NextDouble() - 0.5, 2));
							priceRow.LastSize = Convert.ToDecimal(random.Next(1, 10));
							break;

						}

						// This will segregate the quotes into the different feeds.  This batch of price changes will be sent to each of the subscribing web clients
						// when we've accumulated the specified number of 'ticks'.
						quotes[priceRow.Feed].Add(new Quote()
						{
							AskPrice = priceRow.AskPrice,
							AskSize = priceRow.AskSize,
							BidPrice = priceRow.BidPrice,
							BidSize = priceRow.BidSize,
							LastPrice = priceRow.LastPrice,
							LastSize = priceRow.LastSize,
							Symbol = priceRow.Symbol
						});

					}

				}

				// The web service has trouble with lists so we need to convert the collections to arrays before passing them.  There are two different flavors of
				// updating the subscribers.  The first involves hitting the specific web services for the specific simulated feeds.  That is, we web service
				// contains methods for updating United States Ticker prices, Canadian Ticker Prices, United Kingdom Ticker prices.  However, each feed would then
				// require a different web service.  While this flavor is useful for shaking out the architecture of the server (and the simulator), it is less
				// efficient on bandwidth and CPU cycles as this method, which represents a consolidated feed that pull together quotes from different pricing
				// services and updates the server with one batch.
				Dictionary<String, Quote[]> serviceQuotes = new Dictionary<String, Quote[]>()
					{
						{"CA TICKER", quotes["CA TICKER"].ToArray()},
						{"UK TICKER", quotes["UK TICKER"].ToArray()},
						{"US TICKER", quotes["US TICKER"].ToArray()}
					};

				// A real-world ticker feed will use a single scheme for identifying the securities.  Those schemes are not unique and will often overlap.  This
				// simulates a feed from multiple real-world vendors with different methods of identifying securities that are consolidated into a single batch.  
				// Each organization that has subscrived to this simluator will get a copy of the batch.
				for (Int32 index = 0; index < webServiceClients.Count; index++)
				{

					// This is the next web service that we'll try to update with a batch of quotes.
					WebServiceClient webServiceClient = webServiceClients[index];

					while (true)
					{
						try
						{
							webServiceClient.UpdatePrice(serviceQuotes);
							break;
						}
						catch (Exception)
						{

							// If any exception causes the communication channel to close, then we'll try to reopen it and send the batch of quotes again.
							if (webServiceClient.State != CommunicationState.Opened)
								foreach (ClientInfo clientInfo in PriceSimulator.equityClients)
									if (webServiceClient.ClientCredentials.UserName.UserName == webServiceClient.ClientCredentials.UserName.UserName)
									{
										webServiceClients.Remove(webServiceClient);
										webServiceClient = new WebServiceClient(clientInfo.EndpointName);
										webServiceClient.ClientCredentials.UserName.UserName = clientInfo.UserName;
										webServiceClient.ClientCredentials.UserName.Password = clientInfo.Password;
										webServiceClients.Add(webServiceClient);
									}

						}
					}

				}

				// This allows other threads the chance to run after each batch of price changes has been delivered to the subscribers.
				Thread.Sleep(PriceSimulator.interval);

			}

		}

		/// <summary>
		/// Starts the simulation.
		/// </summary>
		internal static void Start()
		{

			// The price table on the server has a volatile price table.  Therefore, the prices will all be zero (and any market values based off those prices will
			// be zero) until prices are loaded.  This provides the initial prices for the volatile price table.
			if (!PriceSimulator.arePricesInitialized)
			{
				PriceSimulator.InitializeCurrencyPrice();
				PriceSimulator.InitializeEquityPrice();
				PriceSimulator.InitializeDebtPrice();
				PriceSimulator.arePricesInitialized = true;
			}

			// This will start the price simulator.
			PriceSimulator.priceSimulationThread = new Thread(new ThreadStart(PriceSimulator.SimulatePrice));
			PriceSimulator.priceSimulationThread.Name = "Price Simulator";
			PriceSimulator.priceSimulationThread.Start();

		}

		/// <summary>
		/// Stops the simulation.
		/// </summary>
		internal static void Stop()
		{

			// Terminate the price simulator thread.
			PriceSimulator.priceSimulationThread.Abort();

		}

		/// <summary>
		/// Gets or sets the frequency of equity price changes.
		/// </summary>
		internal static Double EquityFrequency
		{
			get
			{
				lock (PriceSimulator.syncRoot)
					return PriceSimulator.equityFrequencyField;
			}
			set
			{
				lock (PriceSimulator.syncRoot)
					PriceSimulator.equityFrequencyField = value;
			}
		}

	}

}
