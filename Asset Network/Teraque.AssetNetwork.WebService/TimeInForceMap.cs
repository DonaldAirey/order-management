namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Transactions;
	using Teraque;
	using Teraque.AssetNetwork;

	/// <summary>
	/// Converts the TimeInForceMap constants to internal identifiers.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	class TimeInForceMap
	{

		// Static private fields
		private static Dictionary<Guid, TimeInForceCode> idDictionary;
		private static Dictionary<TimeInForceCode, Guid> enumDictionary;

		/// <summary>
		/// Creates a mapping between internal constants and database identifiers.
		/// </summary>
		static TimeInForceMap()
		{

			// Initialize the object
			TimeInForceMap.idDictionary = new Dictionary<Guid, TimeInForceCode>();
			TimeInForceMap.enumDictionary = new Dictionary<TimeInForceCode, Guid>();

			// A transaction is required to lock the records while the table is read.
			try
			{

				// Lock the whole data model before reading the table.
				DataModel.DataLock.EnterReadLock();

				// This will read each of the values into a hash table.  This hash table can be used in the code without having to lock the tables because each
				// of these values is constant and doesn't change after the system has been started.
				foreach (DataModel.TimeInForceRow timeInForceRow in DataModel.TimeInForce)
				{
					TimeInForceMap.idDictionary.Add((Guid)timeInForceRow[DataModel.TimeInForce.TimeInForceIdColumn], (TimeInForceCode)timeInForceRow[DataModel.TimeInForce.TimeInForceCodeColumn]);
					TimeInForceMap.enumDictionary.Add((TimeInForceCode)timeInForceRow[DataModel.TimeInForce.TimeInForceCodeColumn], (Guid)timeInForceRow[DataModel.TimeInForce.TimeInForceIdColumn]);
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
		public static Guid FromCode(TimeInForceCode key)
		{
			return TimeInForceMap.enumDictionary[key];
		}

		/// <summary>
		/// Gets the strongly typed enumerated value from the internal database identifier.
		/// </summary>
		/// <param name="key">The internal database identifier.</param>
		/// <returns>The strongly typed enumerated value.</returns>
		public static TimeInForceCode FromId(Guid key)
		{
			return TimeInForceMap.idDictionary[key];
		}

	}

}
