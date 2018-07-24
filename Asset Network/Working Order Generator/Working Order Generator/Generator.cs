namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Input;
	using System.Windows.Threading;
	using System.Xml.Linq;
	using System.Threading;

	/// <summary>
	/// Creates equity orders for a given blotter.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal class Generator
	{

		/// <summary>
		/// The instrument types.
		/// </summary>
		enum InstrumentType { Equity, FixedIncome };

		/// <summary>
		/// A position.
		/// </summary>
		struct Position
		{

			/// <summary>
			/// The unique security identifier.
			/// </summary>
			public Guid securityId;

			/// <summary>
			/// The unique side code.
			/// </summary>
			public SideCode sideCode;

			/// <summary>
			/// Initialize a new instance of the Position class.
			/// </summary>
			/// <param name="securityId">The security id.</param>
			/// <param name="sideCode">The side code.</param>
			public Position(Guid securityId, SideCode sideCode)
			{

				// Initialize the object.
				this.securityId = securityId;
				this.sideCode = sideCode;

			}

			/// <summary>
			/// Indicates whether this instance and a specified object are equal.
			/// </summary>
			/// <param name="obj">Another object to compare to.</param>
			/// <returns>true if obj and this instance are the same type and represent the same value; otherwise, false.</returns>
			public override bool Equals(object obj)
			{

				// If the other object is a Position, then compare the fields.
				if (obj is Position)
				{
					Position position = (Position)obj;
					return position.securityId == this.securityId && position.sideCode == this.sideCode;
				}

				// All other comparisons are not equal.
				return false;

			}

			/// <summary>
			/// Returns the hash code for this instance.
			/// </summary>
			/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
			public override int GetHashCode()
			{
				return this.securityId.GetHashCode() + this.sideCode.GetHashCode();
			}

		}

		/// <summary>
		/// Instructions on how to generate the orders.
		/// </summary>
		GeneratorInfo generatorInfo;
	
		/// <summary>
		/// Used to map the instrument types to the generators that will create orders of that type.
		/// </summary>
		Dictionary<InstrumentType, Action> instrumentTypeHandler;

		/// <summary>
		/// Provides the randomizer used to generate random data.
		/// </summary>
		Random random = new Random();

		/// <summary>
		/// Used to prevent two of the same securities from ending up on the same blotter.
		/// </summary>
		HashSet<Position> positionSet = new HashSet<Position>();

		/// <summary>
		/// The document into which the orders are deposited.
		/// </summary>
		XDocument xDocument = new XDocument();

		/// <summary>
		/// Initializes a new instance of the Generator class.
		/// </summary>
		/// <param name="generatorInfo">Instructions on how to generate the working orders.</param>
		public Generator(GeneratorInfo generatorInfo)
		{

			// Initialize the object
			this.generatorInfo = generatorInfo;

			// This map is used to call a handler that will create working orders for the different instrument types.
			this.instrumentTypeHandler = new Dictionary<InstrumentType, Action>()
			{
				{InstrumentType.Equity, this.CreateEquity},
				{InstrumentType.FixedIncome, this.CreateDebt}
			};

		}

		/// <summary>
		/// Creates a script of equity orders.
		/// </summary>
		public void Create()
		{

			// This list is used to randomize the type of instrument generated based on the allowable instrument types in this blotter.
			List<InstrumentType> instrumentTypes = new List<InstrumentType>();
			if (this.generatorInfo.IsEquity)
				instrumentTypes.Add(InstrumentType.Equity);
			if (this.generatorInfo.IsFixedIncome)
				instrumentTypes.Add(InstrumentType.FixedIncome);

			// <script name="Emerging Markets Orders">
			DataModel.BlotterRow blotterRow = DataModel.Blotter.BlotterKey.Find(generatorInfo.BlotterId);
			XElement elementRoot = new XElement("script", new XAttribute("name", String.Format("{0} Orders", blotterRow.EntityRow.Name)));
			xDocument.Add(elementRoot);

			//  <client type="DataModelClient, Teraque.AssetNetwork.ClientDataModel" />
			elementRoot.Add(new XElement("client", new XAttribute("type", "DataModelClient, Teraque.AssetNetwork.ClientDataModel")));

			// This will generate the requested number of working orders for the selected instrument type(s).
			for (Int32 counter = 0; counter < generatorInfo.OrderCount; counter++)
				this.instrumentTypeHandler[instrumentTypes[this.random.Next(instrumentTypes.Count)]]();

			// At this point the script can be committed to the given file.
			xDocument.Save(generatorInfo.FileName);

		}

		/// <summary>
		/// Create an equity working order.
		/// </summary>
		void CreateEquity()
		{

			// All orders get a unique identifier.
			Guid workingOrderId = Guid.NewGuid();

			// These records provide the starting point for generating the orders.
			DataModel.BlotterRow blotterRow = DataModel.Blotter.BlotterKey.Find(generatorInfo.BlotterId);
			DataModel.UserRow userRow = DataModel.User.UserKey.Find(generatorInfo.UserId);
			DataModel.CountryRow unitedStatesRow = DataModel.Country.CountryKeyExternalId0.Find("US");
			DataModel.StatusRow statusRow = DataModel.Status.StatusKey.Find(StatusCode.New);

			// Generate the settlement currency
			DataModel.EntityRow usdEntityRow = DataModel.Entity.EntityKeyExternalId0.Find("USD");
			DataModel.SecurityRow settlementCurrencyRow = DataModel.Security.SecurityKey.Find(usdEntityRow.EntityId);

			// The orders are an even mix of Buys and Sells.  Most positions are long.
			Boolean isBuy = random.Next(2) == 0;
			Boolean isLong = random.Next(4) != 0;
			SideCode sideCode = isBuy && isLong ? SideCode.Buy : !isBuy && isLong ? SideCode.Sell : isBuy && !isLong ? SideCode.BuyCover : SideCode.SellShort;
			DataModel.SideRow sideRow = DataModel.Side.SideKey.Find(sideCode);

			// Find a random equity position that is unique to this blotter.
			DataModel.SecurityRow securityRow = null;
			while (securityRow == null)
			{

				// Select a random equity and price for the next order.
				DataModel.EquityRow equityRow = DataModel.Equity[random.Next(DataModel.Equity.Count - 1)];
				DataModel.PriceRow priceRow = DataModel.Price.PriceKey.Find(equityRow.EquityId, settlementCurrencyRow.SecurityId);

				// Only generate orders for securities that are unique to this blotter and for which we have prices.  It is important when the price simulator runs
				// that we don't have zeros showing up for prices as it requires more explaination that we want to provide.
				Position position = new Position(equityRow.EquityId, sideCode);
				if (!positionSet.Contains(position) && priceRow != null)
				{
					securityRow = equityRow.SecurityRowByFK_Security_Equity_EquityId;
					positionSet.Add(position);
				}

			}

			// These orders will not match by default.  We need to turn them on manually.
			DataModel.CrossingRow crossingRow = DataModel.Crossing.CrossingKey.Find(CrossingCode.NeverMatch);

			// Select a random Time In Force for this order.  Most orders are Day orders but occationally we'll generate a GTC just to keep it interesting.
			TimeInForceCode timeInForce = random.Next(4) == 0 ? TimeInForceCode.GoodTillCancel : TimeInForceCode.Day;
			DataModel.TimeInForceRow timeInForceRow = DataModel.TimeInForce.TimeInForceKey.Find(timeInForce);

			// Generate the trade and settlement dates based on the current time and make sure it doesn't fall on a weekend.
			DateTime tradeDate = DateTime.Now;
			DateTime settlementDate = DateTime.Now;
			TimeSpan oneDay = TimeSpan.FromDays(1.0);
			for (Int32 dayIndex = 0; dayIndex < 3; dayIndex++)
			{
				settlementDate += oneDay;
				if (settlementDate.DayOfWeek == DayOfWeek.Saturday)
					settlementDate += oneDay;
				if (settlementDate.DayOfWeek == DayOfWeek.Sunday)
					settlementDate += oneDay;
			}

			// This will generate random matching preferences for the orders for demonstrating the matching box capabilities.
			Boolean isBrokerMatch = random.Next(20) == 0;
			Boolean isHedgeMatch = random.Next(15) == 0;
			Boolean isInstitutionMatch = true;

			// This randomizes the random matching patterns.  Every now and then, don't match with anyone.
			if (random.Next(5) == 0)
			{
				isBrokerMatch = false;
				isHedgeMatch = false;
				isInstitutionMatch = false;
			}

			//  <transaction>
			XElement elementTransaction = new XElement("transaction");
			this.xDocument.Root.Add(elementTransaction);

			//    <method name="StoreWorkingOrder">
			XElement elementWorkingOrder = new XElement("method", new XAttribute("name", "StoreWorkingOrder"));
			elementTransaction.Add(elementWorkingOrder);

			//      <parameter name="blotterKey" value="EMERGING MARKETS BLOTTER" />
			elementWorkingOrder.Add(
				new XElement("parameter",
					new XAttribute("name", "blotterKey"),
					new XAttribute("value", blotterRow.EntityRow.ExternalId0)));

			//      <parameter name="configurationId" value="US TICKER" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "configurationId"), new XAttribute("value", "US TICKER")));

			//      <parameter name="createdTime" value="5/6/2012 5:27:56 PM" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "createdTime"), new XAttribute("value", DateTime.Now.ToString("G"))));

			//      <parameter name="crossingKey" value="NEVER MATCH" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "crossingKey"), new XAttribute("value", crossingRow.ExternalId0)));

			//      <parameter name="externalId0" value="{bab88942-5c4e-440a-a352-c8e9b00fec12}" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "externalId0"), new XAttribute("value", workingOrderId.ToString("B"))));

			//      <parameter name="isBrokerMatch" value="false" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "isBrokerMatch"), new XAttribute("value", isBrokerMatch)));

			//      <parameter name="isHedgeMatch" value="false" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "isHedgeMatch"), new XAttribute("value", isHedgeMatch)));

			//      <parameter name="isInstitutionMatch" value="true" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "isInstitutionMatch"), new XAttribute("value", isInstitutionMatch)));

			//      <parameter name="modifiedTime" value="5/6/2012 5:27:56 PM" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "modifiedTime"), new XAttribute("value", DateTime.Now.ToString("G"))));

			//      <parameter name="orderTypeKey" value="MKT" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "orderTypeKey"), new XAttribute("value", "MKT")));

			//      <parameter name="securityKeyBySecurityId" value="ODP" />
			elementWorkingOrder.Add(
				new XElement("parameter",
					new XAttribute("name", "securityKeyBySecurityId"),
					new XAttribute("value", securityRow.EntityRow.ExternalId3)));

			//      <parameter name="securityKeyBySettlementId" value="USD" />
			elementWorkingOrder.Add(
				new XElement("parameter",
					new XAttribute("name", "securityKeyBySettlementId"),
					new XAttribute("value", settlementCurrencyRow.EntityRow.ExternalId0)));

			//      <parameter name="settlementDate" value="5/9/2012 5:27:56 PM" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "settlementDate"), new XAttribute("value", settlementDate.ToString("G"))));

			//      <parameter name="sideKey" value="SELL" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "sideKey"), new XAttribute("value", sideRow.ExternalId0)));

			//      <parameter name="statusKey" value="NEW" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "statusKey"), new XAttribute("value", statusRow.ExternalId0)));

			//      <parameter name="timeInForceKey" value="DAY" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "timeInForceKey"), new XAttribute("value", timeInForceRow.ExternalId0)));

			//      <parameter name="tradeDate" value="5/6/2012 5:27:56 PM" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "tradeDate"), new XAttribute("value", tradeDate.ToString("G"))));

			//      <parameter name="userKeyByCreatedUserId" value="DEV KAPOOR" />
			elementWorkingOrder.Add(
				new XElement("parameter",
					new XAttribute("name", "userKeyByCreatedUserId"),
					new XAttribute("value", userRow.EntityRow.ExternalId0)));

			//      <parameter name="userKeyByModifiedUserId" value="DEV KAPOOR" />
			elementWorkingOrder.Add(
				new XElement("parameter",
					new XAttribute("name", "userKeyByModifiedUserId"),
					new XAttribute("value", userRow.EntityRow.ExternalId0)));

			// This will generate between one and six source orders for the working order.  Source orders are a blocking concept.  You can get several orders for
			// the same security on the same side for the same price.  When this happens, it is much more efficient to block them as a single order and execute them
			// as a single order and allocate them back to the original orders when the order is done.
			Int32 sourceOrderCount = random.Next(1, 6);
			for (Int32 sourceOrderIndex = 0; sourceOrderIndex < sourceOrderCount; sourceOrderIndex++)
			{

				// This creates a unique identifier for the source order.
				Guid sourceOrderId = Guid.NewGuid();

				// This generates a random quantity for the order between 100 and 10,000 shares.
				Decimal orderedQuantity = Convert.ToDecimal(random.Next(1, 100)) * 100.0M;

				//    <method name="StoreSourceOrder">
				XElement elementSourceOrder = new XElement("method", new XAttribute("name", "StoreSourceOrder"));
				elementTransaction.Add(elementSourceOrder);

				//      <parameter name="configurationId" value="US TICKER" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "configurationId"), new XAttribute("value", "US TICKER")));

				//      <parameter name="createdTime" value="2012-05-06T17:27:56.2658093-04:00" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "createdTime"), new XAttribute("value", DateTime.Now)));

				//      <parameter name="externalId0" value="{3c69e0a0-3316-4499-a7b1-6dda5a058837}" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "externalId0"), new XAttribute("value", sourceOrderId.ToString("B"))));

				//      <parameter name="modifiedTime" value="5/6/2012 5:27:56 PM" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "modifiedTime"), new XAttribute("value", DateTime.Now.ToString("G"))));

				//      <parameter name="orderedQuantity" value="4300.0" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "orderedQuantity"), new XAttribute("value", orderedQuantity)));

				//      <parameter name="orderTypeKey" value="MKT" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "orderTypeKey"), new XAttribute("value", "MKT")));

				//      <parameter name="securityKeyBySecurityId" value="ODP" />
				elementSourceOrder.Add(
					new XElement("parameter",
						new XAttribute("name", "securityKeyBySecurityId"),
						new XAttribute("value", securityRow.EntityRow.ExternalId3)));

				//      <parameter name="securityKeyBySettlementId" value="USD" />
				elementSourceOrder.Add(
					new XElement("parameter",
						new XAttribute("name", "securityKeyBySettlementId"),
						new XAttribute("value", settlementCurrencyRow.EntityRow.ExternalId0)));

				//      <parameter name="settlementDate" value="2012-05-09T17:27:56.2528086-04:00" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "settlementDate"), new XAttribute("value", settlementDate)));

				//      <parameter name="sideKey" value="SELL" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "sideKey"), new XAttribute("value", sideRow.ExternalId0)));

				//      <parameter name="statusKey" value="NEW" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "statusKey"), new XAttribute("value", statusRow.ExternalId0)));

				//      <parameter name="timeInForceKey" value="DAY" />
				elementSourceOrder.Add(
					new XElement("parameter",
						new XAttribute("name", "timeInForceKey"),
						new XAttribute("value", timeInForceRow.ExternalId0)));

				//      <parameter name="tradeDate" value="5/6/2012 5:27:56 PM" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "tradeDate"), new XAttribute("value", tradeDate.ToString("G"))));

				//      <parameter name="userKeyByCreatedUserId" value="DEV KAPOOR" />
				elementSourceOrder.Add(
					new XElement("parameter",
						new XAttribute("name", "userKeyByCreatedUserId"),
						new XAttribute("value", userRow.EntityRow.ExternalId0)));

				//      <parameter name="userKeyByModifiedUserId" value="DEV KAPOOR" />
				elementSourceOrder.Add(
					new XElement("parameter",
						new XAttribute("name", "userKeyByModifiedUserId"),
						new XAttribute("value", userRow.EntityRow.ExternalId0)));

				//      <parameter name="workingOrderKey" value="{bab88942-5c4e-440a-a352-c8e9b00fec12}" />
				elementSourceOrder.Add(
					new XElement("parameter",
						new XAttribute("name", "workingOrderKey"),
						new XAttribute("value", workingOrderId.ToString("B"))));

			}

		}

		/// <summary>
		/// Create an debt working order.
		/// </summary>
		void CreateDebt()
		{

			// All orders get a unique identifier.
			Guid workingOrderId = Guid.NewGuid();

			// These records provide the starting point for generating the orders.
			DataModel.BlotterRow blotterRow = DataModel.Blotter.BlotterKey.Find(generatorInfo.BlotterId);
			DataModel.UserRow userRow = DataModel.User.UserKey.Find(generatorInfo.UserId);
			DataModel.CountryRow unitedStatesRow = DataModel.Country.CountryKeyExternalId0.Find("US");
			DataModel.StatusRow statusRow = DataModel.Status.StatusKey.Find(StatusCode.New);

			// Generate the settlement currency
			DataModel.EntityRow usdEntityRow = DataModel.Entity.EntityKeyExternalId0.Find("USD");
			DataModel.SecurityRow settlementCurrencyRow = DataModel.Security.SecurityKey.Find(usdEntityRow.EntityId);

			// The orders are an even mix of Buys and Sells.  Most positions are long.
			Boolean isBuy = random.Next(2) == 0;
			Boolean isLong = random.Next(4) != 0;
			SideCode sideCode = isBuy && isLong ? SideCode.Buy : !isBuy && isLong ? SideCode.Sell : isBuy && !isLong ? SideCode.BuyCover : SideCode.SellShort;
			DataModel.SideRow sideRow = DataModel.Side.SideKey.Find(sideCode);

			// Find a random debt position that is unique to this blotter.
			DataModel.SecurityRow securityRow = null;
			while (securityRow == null)
			{

				// Select a random debt instrument for the next order.
				DataModel.DebtRow debtRow = DataModel.Debt[random.Next(DataModel.Debt.Count - 1)];

				// Only generate orders for positions that are unique to this blotter.
				Position position = new Position(debtRow.DebtId, sideCode);
				if (!positionSet.Contains(position))
				{
					securityRow = debtRow.SecurityRowByFK_Security_Debt_DebtId;
					positionSet.Add(position);
				}

			}

			// These orders will not match by default.  We need to turn them on manually.
			DataModel.CrossingRow crossingRow = DataModel.Crossing.CrossingKey.Find(CrossingCode.NeverMatch);

			// Select a random Time In Force for this order.  Most orders are Day orders but occationally we'll generate a GTC just to keep it interesting.
			TimeInForceCode timeInForce = random.Next(4) == 0 ? TimeInForceCode.GoodTillCancel : TimeInForceCode.Day;
			DataModel.TimeInForceRow timeInForceRow = DataModel.TimeInForce.TimeInForceKey.Find(timeInForce);

			// Generate the trade and settlement dates based on the current time and make sure it doesn't fall on a weekend.
			DateTime tradeDate = DateTime.Now;
			DateTime settlementDate = DateTime.Now;
			TimeSpan oneDay = TimeSpan.FromDays(1.0);
			for (Int32 dayIndex = 0; dayIndex < 3; dayIndex++)
			{
				settlementDate += oneDay;
				if (settlementDate.DayOfWeek == DayOfWeek.Saturday)
					settlementDate += oneDay;
				if (settlementDate.DayOfWeek == DayOfWeek.Sunday)
					settlementDate += oneDay;
			}

			// This will generate random matching preferences for the orders for demonstrating the matching box capabilities.
			Boolean isBrokerMatch = random.Next(20) == 0;
			Boolean isHedgeMatch = random.Next(15) == 0;
			Boolean isInstitutionMatch = true;

			// This randomizes the random matching patterns.  Every now and then, don't match with anyone.
			if (random.Next(5) == 0)
			{
				isBrokerMatch = false;
				isHedgeMatch = false;
				isInstitutionMatch = false;
			}

			//  <transaction>
			XElement elementTransaction = new XElement("transaction");
			this.xDocument.Root.Add(elementTransaction);

			//    <method name="StoreWorkingOrder">
			XElement elementWorkingOrder = new XElement("method", new XAttribute("name", "StoreWorkingOrder"));
			elementTransaction.Add(elementWorkingOrder);

			//      <parameter name="blotterKey" value="EMERGING MARKETS BLOTTER" />
			elementWorkingOrder.Add(
				new XElement("parameter",
					new XAttribute("name", "blotterKey"),
					new XAttribute("value", blotterRow.EntityRow.ExternalId0)));

			//      <parameter name="configurationId" value="US TICKER" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "configurationId"), new XAttribute("value", "CUSIP")));

			//      <parameter name="createdTime" value="5/6/2012 5:27:56 PM" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "createdTime"), new XAttribute("value", DateTime.Now.ToString("G"))));

			//      <parameter name="crossingKey" value="NEVER MATCH" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "crossingKey"), new XAttribute("value", crossingRow.ExternalId0)));

			//      <parameter name="externalId0" value="{bab88942-5c4e-440a-a352-c8e9b00fec12}" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "externalId0"), new XAttribute("value", workingOrderId.ToString("B"))));

			//      <parameter name="isBrokerMatch" value="false" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "isBrokerMatch"), new XAttribute("value", isBrokerMatch)));

			//      <parameter name="isHedgeMatch" value="false" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "isHedgeMatch"), new XAttribute("value", isHedgeMatch)));

			//      <parameter name="isInstitutionMatch" value="true" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "isInstitutionMatch"), new XAttribute("value", isInstitutionMatch)));

			//      <parameter name="modifiedTime" value="5/6/2012 5:27:56 PM" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "modifiedTime"), new XAttribute("value", DateTime.Now.ToString("G"))));

			//      <parameter name="orderTypeKey" value="MKT" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "orderTypeKey"), new XAttribute("value", "MKT")));

			//      <parameter name="securityKeyBySecurityId" value="ODP" />
			elementWorkingOrder.Add(
				new XElement("parameter",
					new XAttribute("name", "securityKeyBySecurityId"),
					new XAttribute("value", securityRow.EntityRow.ExternalId4)));

			//      <parameter name="securityKeyBySettlementId" value="USD" />
			elementWorkingOrder.Add(
				new XElement("parameter",
					new XAttribute("name", "securityKeyBySettlementId"),
					new XAttribute("value", settlementCurrencyRow.EntityRow.ExternalId0)));

			//      <parameter name="settlementDate" value="5/9/2012 5:27:56 PM" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "settlementDate"), new XAttribute("value", settlementDate.ToString("G"))));

			//      <parameter name="sideKey" value="SELL" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "sideKey"), new XAttribute("value", sideRow.ExternalId0)));

			//      <parameter name="statusKey" value="NEW" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "statusKey"), new XAttribute("value", statusRow.ExternalId0)));

			//      <parameter name="timeInForceKey" value="DAY" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "timeInForceKey"), new XAttribute("value", timeInForceRow.ExternalId0)));

			//      <parameter name="tradeDate" value="5/6/2012 5:27:56 PM" />
			elementWorkingOrder.Add(new XElement("parameter", new XAttribute("name", "tradeDate"), new XAttribute("value", tradeDate.ToString("G"))));

			//      <parameter name="userKeyByCreatedUserId" value="DEV KAPOOR" />
			elementWorkingOrder.Add(
				new XElement("parameter",
					new XAttribute("name", "userKeyByCreatedUserId"),
					new XAttribute("value", userRow.EntityRow.ExternalId0)));

			//      <parameter name="userKeyByModifiedUserId" value="DEV KAPOOR" />
			elementWorkingOrder.Add(
				new XElement("parameter",
					new XAttribute("name", "userKeyByModifiedUserId"),
					new XAttribute("value", userRow.EntityRow.ExternalId0)));

			// This will generate between one and three source orders for the working order.  Source orders are a blocking concept.  You can get several orders for
			// the same security on the same side for the same price.  When this happens, it is much more efficient to block them as a single order and execute them
			// as a single order and allocate them back to the original orders when the order is done.
			Int32 sourceOrderCount = random.Next(1, 3);
			for (Int32 sourceOrderIndex = 0; sourceOrderIndex < sourceOrderCount; sourceOrderIndex++)
			{

				// This creates a unique identifier for the source order.
				Guid sourceOrderId = Guid.NewGuid();

				// This generates a random quantity for the order between 100 and 10,000 shares.
				Decimal orderedQuantity = Convert.ToDecimal(random.Next(1, 100)) * 100.0M;

				//    <method name="StoreSourceOrder">
				XElement elementSourceOrder = new XElement("method", new XAttribute("name", "StoreSourceOrder"));
				elementTransaction.Add(elementSourceOrder);

				//      <parameter name="configurationId" value="US TICKER" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "configurationId"), new XAttribute("value", "CUSIP")));

				//      <parameter name="createdTime" value="2012-05-06T17:27:56.2658093-04:00" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "createdTime"), new XAttribute("value", DateTime.Now)));

				//      <parameter name="externalId0" value="{3c69e0a0-3316-4499-a7b1-6dda5a058837}" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "externalId0"), new XAttribute("value", sourceOrderId.ToString("B"))));

				//      <parameter name="modifiedTime" value="5/6/2012 5:27:56 PM" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "modifiedTime"), new XAttribute("value", DateTime.Now.ToString("G"))));

				//      <parameter name="orderedQuantity" value="4300.0" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "orderedQuantity"), new XAttribute("value", orderedQuantity)));

				//      <parameter name="orderTypeKey" value="MKT" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "orderTypeKey"), new XAttribute("value", "MKT")));

				//      <parameter name="securityKeyBySecurityId" value="ODP" />
				elementSourceOrder.Add(
					new XElement("parameter",
						new XAttribute("name", "securityKeyBySecurityId"),
						new XAttribute("value", securityRow.EntityRow.ExternalId4)));

				//      <parameter name="securityKeyBySettlementId" value="USD" />
				elementSourceOrder.Add(
					new XElement("parameter",
						new XAttribute("name", "securityKeyBySettlementId"),
						new XAttribute("value", settlementCurrencyRow.EntityRow.ExternalId0)));

				//      <parameter name="settlementDate" value="2012-05-09T17:27:56.2528086-04:00" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "settlementDate"), new XAttribute("value", settlementDate)));

				//      <parameter name="sideKey" value="SELL" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "sideKey"), new XAttribute("value", sideRow.ExternalId0)));

				//      <parameter name="statusKey" value="NEW" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "statusKey"), new XAttribute("value", statusRow.ExternalId0)));

				//      <parameter name="timeInForceKey" value="DAY" />
				elementSourceOrder.Add(
					new XElement("parameter",
						new XAttribute("name", "timeInForceKey"),
						new XAttribute("value", timeInForceRow.ExternalId0)));

				//      <parameter name="tradeDate" value="5/6/2012 5:27:56 PM" />
				elementSourceOrder.Add(new XElement("parameter", new XAttribute("name", "tradeDate"), new XAttribute("value", tradeDate.ToString("G"))));

				//      <parameter name="userKeyByCreatedUserId" value="DEV KAPOOR" />
				elementSourceOrder.Add(
					new XElement("parameter",
						new XAttribute("name", "userKeyByCreatedUserId"),
						new XAttribute("value", userRow.EntityRow.ExternalId0)));

				//      <parameter name="userKeyByModifiedUserId" value="DEV KAPOOR" />
				elementSourceOrder.Add(
					new XElement("parameter",
						new XAttribute("name", "userKeyByModifiedUserId"),
						new XAttribute("value", userRow.EntityRow.ExternalId0)));

				//      <parameter name="workingOrderKey" value="{bab88942-5c4e-440a-a352-c8e9b00fec12}" />
				elementSourceOrder.Add(
					new XElement("parameter",
						new XAttribute("name", "workingOrderKey"),
						new XAttribute("value", workingOrderId.ToString("B"))));

			}

		}

	}

}
