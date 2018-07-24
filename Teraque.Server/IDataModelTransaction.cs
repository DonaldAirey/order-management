namespace Teraque
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Data.SqlClient;
	using System.Transactions;

	/// <summary>
	/// A common interface for accessing the transactions created by the data model generator.
	/// </summary>
	public interface IDataModelTransaction
	{

		/// <summary>
		/// Add a lock to the row.
		/// </summary>
		/// <param name="iRow"></param>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i")]
		void AddLock(IRow iRow);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="iRow"></param>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i")]
		void AddRecord(IRow iRow);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="enlistment"></param>
		void Commit(Enlistment enlistment);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="enlistment"></param>
		void InDoubt(Enlistment enlistment);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="preparingEnlistment"></param>
		void Prepare(PreparingEnlistment preparingEnlistment);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="enlistment"></param>
		void Rollback(Enlistment enlistment);

		/// <summary>
		/// 
		/// </summary>
		SqlConnection SqlConnection { get; }

		/// <summary>
		/// 
		/// </summary>
		Guid TransactionId { get; }

	}

}
