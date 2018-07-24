namespace Teraque
{

	using System;

	/// <summary>
	/// The various states of a record during a transaction.
	/// </summary>
	public static class RecordState
	{

		/// <summary>
		/// The record has been added.
		/// </summary>
		public const Int32 Added = 0;

		/// <summary>
		/// The record has been deleted.
		/// </summary>
		public const Int32 Deleted = 1;

		/// <summary>
		/// The record has been modified.
		/// </summary>
		public const Int32 Modified = 2;

	}

}
