namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.ServiceModel;

	/// <summary>
	/// Web Services for prices.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal static class PriceService
	{

		/// <summary>
		/// Updates prices using a Canada ticker.
		/// </summary>
		/// <param name="quotes">A collection of price quotes.</param>
		internal static void UpdateCanadaTickerPrice(Quote[] quotes)
		{

			// Update the price table with Canada ticker symbols.
			DataModel.IEntityIndex symbolIndex = DataModel.Entity.EntityKeyExternalId1;
			DataModel.IEntityIndex currencyIndex = DataModel.Entity.EntityKeyExternalId0;
			foreach (Quote quote in quotes)
				PriceService.UpdatePriceBySymbolCurrency(symbolIndex, new Object[] { quote.Symbol }, currencyIndex, new Object[] { "CAD" }, quote);

		}

		/// <summary>
		/// Updates prices using a Cusip ticker.
		/// </summary>
		/// <param name="quotes">A collection of price quotes.</param>
		internal static void UpdateCusipPrice(Quote[] quotes)
		{

			// Update the price table with Cusip ticker symbols.
			DataModel.IEntityIndex symbolIndex = DataModel.Entity.EntityKeyExternalId4;
			DataModel.IEntityIndex currencyIndex = DataModel.Entity.EntityKeyExternalId0;
			foreach (Quote quote in quotes)
				PriceService.UpdatePriceBySymbolCurrency(symbolIndex, new Object[] { quote.Symbol }, currencyIndex, new Object[] { "USD" }, quote);

		}

		/// <summary>
		/// Updates currency prices.
		/// </summary>
		/// <param name="quotes">A collection of price quotes.</param>
		internal static void UpdateCurrencyPrice(Quote[] quotes)
		{

			// Update the price table with currency ticker symbols.
			DataModel.IEntityIndex symbolIndex = DataModel.Entity.EntityKeyExternalId0;
			DataModel.IEntityIndex currencyIndex = DataModel.Entity.EntityKeyExternalId0;
			foreach (Quote quote in quotes)
				PriceService.UpdatePriceBySymbolCurrency(symbolIndex, new Object[] { "USD" }, currencyIndex, new Object[] { quote.Symbol }, quote);

		}

		/// <summary>
		/// Updates prices using an aggregator
		/// </summary>
		/// <param name="quotes">A dictionary of prices from various feeds.</param>
		internal static void UpdatePrice(Dictionary<String, Quote[]> quotes)
		{

			// Update the prices of each of the feeds found in the aggregator.
			PriceService.UpdateCanadaTickerPrice(quotes["CA TICKER"]);
			PriceService.UpdateUnitedKingdomTickerPrice(quotes["UK TICKER"]);
			PriceService.UpdateUnitedStatesTickerPrice(quotes["US TICKER"]);

		}

		/// <summary>
		/// Updates prices using a United Kingdom ticker.
		/// </summary>
		/// <param name="quotes">A collection of price quotes.</param>
		internal static void UpdateUnitedKingdomTickerPrice(Quote[] quotes)
		{

			// Update the price table with United Kingdom ticker symbols.
			DataModel.IEntityIndex symbolIndex = DataModel.Entity.EntityKeyExternalId2;
			DataModel.IEntityIndex currencyIndex = DataModel.Entity.EntityKeyExternalId0;
			foreach (Quote quote in quotes)
				PriceService.UpdatePriceBySymbolCurrency(symbolIndex, new Object[] { quote.Symbol }, currencyIndex, new Object[] { "GBP" }, quote);

		}

		/// <summary>
		/// Updates prices using a United States ticker.
		/// </summary>
		/// <param name="quotes">A collection of price quotes.</param>
		internal static void UpdateUnitedStatesTickerPrice(Quote[] quotes)
		{

			// Update the price table with United States ticker symbols.
			DataModel.IEntityIndex symbolIndex = DataModel.Entity.EntityKeyExternalId3;
			DataModel.IEntityIndex currencyIndex = DataModel.Entity.EntityKeyExternalId0;
			foreach (Quote quote in quotes)
				PriceService.UpdatePriceBySymbolCurrency(symbolIndex, new Object[] { quote.Symbol }, currencyIndex, new Object[] { "USD" }, quote);

		}

		/// <summary>
		/// Updates prices using a United States ticker.
		/// </summary>
		/// <param name="symbolIndex">The index to use to find the symbol.</param>
		/// <param name="symbolKey">The symbol key.</param>
		/// <param name="currencyIndex">The index to use to find the currency.</param>
		/// <param name="currencyKey">The currency key.</param>
		/// <param name="quote">The quote used to update the price.</param>
		static void UpdatePriceBySymbolCurrency(
			DataModel.IEntityIndex symbolIndex,
			Object[] symbolKey,
			DataModel.IEntityIndex currencyIndex,
			Object[] currencyKey,
			Quote quote)
		{

			// The current transaction and the target data model is extracted from the thread.
			DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;
			TenantDataModel tenantDataSet = dataModelTransaction.TenantDataModel;

			// This will find the currency of the quote.
			DataModel.EntityRow currencyEntityRow = currencyIndex.Find(currencyKey);
			if (currencyEntityRow == null)
				throw new FaultException<RecordNotFoundFault>(new RecordNotFoundFault("Entity", currencyKey));
			currencyEntityRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
			dataModelTransaction.AddLock(currencyEntityRow);
			if (currencyEntityRow.RowState == System.Data.DataRowState.Detached)
				throw new FaultException<RecordNotFoundFault>(new RecordNotFoundFault("Entity", currencyKey));
			Guid currencyId = currencyEntityRow.EntityId;

			// This will find the security using the external identifier.
			DataModel.EntityRow securityEntityRow = symbolIndex.Find(symbolKey);
			if (securityEntityRow == null)
				throw new FaultException<RecordNotFoundFault>(new RecordNotFoundFault("Entity", symbolKey));
			securityEntityRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
			dataModelTransaction.AddLock(securityEntityRow);
			if (securityEntityRow.RowState == System.Data.DataRowState.Detached)
				throw new FaultException<RecordNotFoundFault>(new RecordNotFoundFault("Entity", symbolKey));
			Guid securityId = securityEntityRow.EntityId;

			// If the price record exists, then update it.  If it doesn't exist then create it.
			Object[] priceKey = new Object[] { securityId, currencyId };
			DataModel.PriceRow priceRow = DataModel.Price.PriceKey.Find(priceKey);
			if (priceRow == null)
				tenantDataSet.CreatePrice(
					quote.AskPrice,
					quote.AskSize,
					quote.BidPrice,
					quote.BidSize,
					null,
					currencyId,
					null,
					quote.LastPrice,
					quote.LastSize,
					null,
					null,
					null,
					securityId,
					null,
					null);
			else
			{
				priceRow.AcquireReaderLock(dataModelTransaction.TransactionId, DataModel.LockTimeout);
				dataModelTransaction.AddLock(priceRow);
				if (priceRow.RowState == System.Data.DataRowState.Detached)
					throw new FaultException<RecordNotFoundFault>(new RecordNotFoundFault("Price", priceKey));
				tenantDataSet.UpdatePrice(
					quote.AskPrice,
					quote.AskSize,
					quote.BidPrice,
					quote.BidSize,
					null,
					currencyId,
					null,
					quote.LastPrice,
					quote.LastSize,
					null,
					null, 
					null, 
					priceKey,
					priceRow.RowVersion,
					securityId,
					null,
					null);
			}

		}

	}

}
