namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Transactions;
	using Teraque;
	using Teraque.AssetNetwork;

	/// <summary>
	/// Converts the SideMap constants to internal identifiers.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	class SideMap
	{

		// Static fields
		static Dictionary<Guid, SideCode> idDictionary;
		static Dictionary<SideCode, Guid> enumDictionary;

		/// <summary>
		/// Creates a mapping between internal constants and database identifiers.
		/// </summary>
		static SideMap()
		{

			// Initialize the object
			SideMap.idDictionary = new Dictionary<Guid, SideCode>();
			SideMap.enumDictionary = new Dictionary<SideCode, Guid>();

			// A transaction is required to lock the records while the table is read.
			try
			{

				// Lock the whole data model before reading the table.
				DataModel.DataLock.EnterReadLock();

				// This will read each of the values into a hash table.  This hash table can be used in the code without having to lock the tables because each
				// of these values is constant and doesn't change after the system has been started.
				foreach (DataModel.SideRow sideRow in DataModel.Side)
				{
					SideMap.idDictionary.Add((Guid)sideRow[DataModel.Side.SideIdColumn], (SideCode)sideRow[DataModel.Side.SideCodeColumn]);
					SideMap.enumDictionary.Add((SideCode)sideRow[DataModel.Side.SideCodeColumn], (Guid)sideRow[DataModel.Side.SideIdColumn]);
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
		public static Guid FromCode(SideCode key)
		{
			return SideMap.enumDictionary[key];
		}

		/// <summary>
		/// Gets the strongly typed enumerated value from the internal database identifier.
		/// </summary>
		/// <param name="key">The internal database identifier.</param>
		/// <returns>The strongly typed enumerated value.</returns>
		public static SideCode FromId(Guid key)
		{
			return SideMap.idDictionary[key];
		}

	}

}
