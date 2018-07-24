namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Transactions;
	using Teraque;

	/// <summary>
	/// Converts the OrderTypeMap constants to internal identifiers.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	class OrderTypeMap
	{

		// Static private fields
		private static Dictionary<Guid, OrderTypeCode> idDictionary;
		private static Dictionary<OrderTypeCode, Guid> enumDictionary;

		/// <summary>
		/// Creates a mapping between internal constants and database identifiers.
		/// </summary>
		static OrderTypeMap()
		{

			// Initialize the object
			OrderTypeMap.idDictionary = new Dictionary<Guid, OrderTypeCode>();
			OrderTypeMap.enumDictionary = new Dictionary<OrderTypeCode, Guid>();

			// A transaction is required to lock the records while the table is read.
			try
			{

				// Lock the whole data model before reading the table.
				DataModel.DataLock.EnterReadLock();

				// This will read each of the values into a hash table.  This hash table can be used in the code without having to lock the tables because each
				// of these values is constant and doesn't change after the system has been started.
				foreach (DataModel.OrderTypeRow orderTypeRow in DataModel.OrderType)
				{
					OrderTypeMap.idDictionary.Add((Guid)orderTypeRow[DataModel.OrderType.OrderTypeIdColumn], (OrderTypeCode)orderTypeRow[DataModel.OrderType.OrderTypeCodeColumn]);
					OrderTypeMap.enumDictionary.Add((OrderTypeCode)orderTypeRow[DataModel.OrderType.OrderTypeCodeColumn], (Guid)orderTypeRow[DataModel.OrderType.OrderTypeIdColumn]);
				}

			}
			finally
			{

				// The data model can be used by other threads now.
				DataModel.DataLock.ExitReadLock();

			}

		}

		/// <summary>
		/// Gets an internal database identifier based on the Enumerated value.
		/// </summary>
		/// <param name="key">The strongly typed enumerated value.</param>
		/// <returns>The equivalent record identifier from the database.</returns>
		public static Guid FromCode(OrderTypeCode key)
		{
			return OrderTypeMap.enumDictionary[key];
		}

		/// <summary>
		/// Gets the strongly typed enumerated value from the internal database identifier.
		/// </summary>
		/// <param name="key">The internal database identifier.</param>
		/// <returns>The strongly typed enumerated value.</returns>
		public static OrderTypeCode FromId(Guid key)
		{
			return OrderTypeMap.idDictionary[key];
		}

	}

}
