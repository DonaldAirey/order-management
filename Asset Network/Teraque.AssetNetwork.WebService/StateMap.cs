namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Transactions;
	using Teraque;
	using Teraque.AssetNetwork;

	/// <summary>
	/// Converts the StateMap constants to internal identifiers.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	class StateMap
	{

		// Static private fields
		private static Dictionary<Guid, StateCode> idDictionary;
		private static Dictionary<StateCode, Guid> enumDictionary;

		/// <summary>
		/// Creates a mapping between internal constants and database identifiers.
		/// </summary>
		static StateMap()
		{

			// Initialize the object
			StateMap.idDictionary = new Dictionary<Guid, StateCode>();
			StateMap.enumDictionary = new Dictionary<StateCode, Guid>();

			// A transaction is required to lock the records while the table is read.
			try
			{

				// Lock the whole data model before reading the table.
				DataModel.DataLock.EnterReadLock();

				// This will read each of the values into a hash table.  This hash table can be used in the code without having to lock the tables because each
				// of these values is constant and doesn't change after the system has been started.
				foreach (DataModel.StateRow stateRow in DataModel.State)
				{
					StateMap.idDictionary.Add((Guid)stateRow[DataModel.State.StateIdColumn], (StateCode)stateRow[DataModel.State.StateCodeColumn]);
					StateMap.enumDictionary.Add((StateCode)stateRow[DataModel.State.StateCodeColumn], (Guid)stateRow[DataModel.State.StateIdColumn]);
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
		public static Guid FromCode(StateCode key)
		{
			return StateMap.enumDictionary[key];
		}

		/// <summary>
		/// Gets the strongly typed enumerated value from the internal database identifier.
		/// </summary>
		/// <param name="key">The internal database identifier.</param>
		/// <returns>The strongly typed enumerated value.</returns>
		public static StateCode FromId(Guid key)
		{
			return StateMap.idDictionary[key];
		}

	}

}
