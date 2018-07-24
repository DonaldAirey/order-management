namespace Teraque.AssetNetwork
{

	using System;
	using System.IO;
	using System.Reflection;
	using System.Xml;
	using System.Xml.Linq;

	internal class MarketData
	{

		static MarketDataModel marketDataModel = new MarketDataModel();

		static Object syncRootField = new Object();

		/// <summary>
		/// Initializes the static elements of the market simulator.
		/// </summary>
		static MarketData()
		{

			// This will seed the simulator with prices stored in the assembly's resources.
			MarketData.LoadPrices("Teraque.AssetNetwork.United States Ticker Prices.xml", "US TICKER", "EQUITY");

		}

		/// <summary>
		/// Loads the prices from the assembly resources.
		/// </summary>
		/// <param name="resourceName">The name of the assembly resource.</param>
		/// <param name="feedName">The simulated ticker feed name.</param>
		/// <param name="securityType">The security type.</param>
		static void LoadPrices(String resourceName, String feedName, String securityType)
		{

			// The price simulator needs realistic prices.  An XML file has been embedded with this assembly that provides the seed values for the prices.
			Random random = new Random(DateTime.Now.TimeOfDay.Milliseconds);
			Stream priceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
			XmlReader xmlReader = new XmlTextReader(priceStream);
			XDocument xDocument = XDocument.Load(xmlReader);
			foreach (XElement xElement in xDocument.Root.Elements())
			{
				String symbol = xElement.Attribute("Symbol").Value;
				Decimal lastPrice = Convert.ToDecimal(xElement.Attribute("Price").Value);
				Decimal lastSize = Convert.ToDecimal(random.Next(1, 10));
				Decimal askPrice = lastPrice + Convert.ToDecimal(Math.Round(random.NextDouble(), 2));
				Decimal askSize = Convert.ToDecimal(random.Next(1, 10));
				Decimal bidPrice = lastPrice - Convert.ToDecimal(Math.Round(random.NextDouble(), 2));
				Decimal bidSize = Convert.ToDecimal(random.Next(1, 10));
				MarketData.marketDataModel.Price.AddPriceRow(askPrice, askSize, bidPrice, bidSize, feedName, lastPrice, lastSize, securityType, symbol);
			}
			MarketData.marketDataModel.AcceptChanges();

		}

		internal static MarketDataModel.ExecutionDataTable Execution
		{
			get
			{
				return MarketData.marketDataModel.Execution;
			}
		}

		internal static MarketDataModel.OrderDataTable Order
		{
			get
			{
				return MarketData.marketDataModel.Order;
			}
		}

		internal static MarketDataModel.PriceDataTable Price
		{
			get
			{
				return MarketData.marketDataModel.Price;
			}
		}

		internal static Object SyncRoot
		{
			get
			{
				return MarketData.syncRootField;
			}
		}

	}

}
