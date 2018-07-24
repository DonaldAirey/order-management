namespace Teraque
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Data;
	using System.Diagnostics;
	using System.Threading;

	/// <summary>
	/// Abstract interface to common features of a row.
	/// </summary>
	public interface IRow
	{

		/// <summary>
		/// Gets the object at the given index.
		/// </summary>
		/// <param name="index">The column index into the row data.</param>
		/// <param name="dataRowVersion">The version of the data.</param>
		[SuppressMessage("Microsoft.Design", "CA1023:IndexersShouldNotBeMultidimensional")]
		Object this[Int32 index, DataRowVersion dataRowVersion]
		{
			get;
		}

		/// <summary>
		/// Gets the object at the given index.
		/// </summary>
		/// <param name="index">The column index into the row data.</param>
		[SuppressMessage("Microsoft.Design", "CA1043:UseIntegralOrStringArgumentForIndexers")]
		Object this[DataColumn index]
		{
			get;
		}

		/// <summary>
		/// Gets the object at the given index.
		/// </summary>
		/// <param name="index">The column index into the row data.</param>
		/// <param name="dataRowVersion">The version of the data.</param>
		[SuppressMessage("Microsoft.Design", "CA1023:IndexersShouldNotBeMultidimensional")]
		Object this[DataColumn index, DataRowVersion dataRowVersion]
		{
			get;
		}

		/// <summary>
		/// Gets the object at the given index.
		/// </summary>
		/// <param name="index">The column index into the row data.</param>
		Object this[Int32 index]
		{
			get;
		}

		/// <summary>
		/// Gets the table to which this row belongs.
		/// </summary>
		DataRowState RowState
		{
			get;
		}

		/// <summary>
		/// Gets the table to which this row belongs.
		/// </summary>
		DataTable Table
		{
			get;
		}

		/// <summary>
		/// Accepts any changes made to the row.
		/// </summary>
		void AcceptChanges();

		/// <summary>
		/// Acquires a reader lock for this record.
		/// </summary>
		/// <param name="transactionId">A token used to identify the holder of the lock.</param>
		/// <param name="timeSpan">The time that the thread will wait for the lock.</param>
		void AcquireReaderLock(Guid transactionId, TimeSpan timeSpan);

		/// <summary>
		/// Acquires a reader lock for this record.
		/// </summary>
		/// <param name="dataModelTransaction">The transaction context for this operation.</param>
		void AcquireReaderLock(IDataModelTransaction dataModelTransaction);

		/// <summary>
		/// Acquires a writer lock for this record.
		/// </summary>
		/// <param name="dataModelTransaction">The transaction context for this operation.</param>
		void AcquireWriterLock(IDataModelTransaction dataModelTransaction);

		/// <summary>
		/// Acquires a writer lock for this record.
		/// </summary>
		/// <param name="transactionId">A token used to identify the holder of the lock.</param>
		/// <param name="timeSpan">The time that the thread will wait for the lock.</param>
		void AcquireWriterLock(Guid transactionId, TimeSpan timeSpan);

		/// <summary>
		/// Gets the parent row using a relation.
		/// </summary>
		/// <param name="dataRelation">Represents a Parent/Child relationship between two DataTable objects.</param>
		/// <returns>The parent row of the this row.</returns>
		DataRow GetParentRow(DataRelation dataRelation);

		/// <summary>
		/// Gets a value indicating whether the owner of a token holds a reader lock.
		/// </summary>
		/// <returns>true if the current token owner holds a reader lock.</returns>
		Boolean IsLockHeld(Guid transactionId);

		/// <summary>
		/// Gets the timeout value for locking operations.
		/// </summary>
		TimeSpan LockTimeout { get; }

		/// <summary>
		/// Rejects any changes made to the row.
		/// </summary>
		void RejectChanges();

		/// <summary>
		/// Releases every lock held by this record.
		/// </summary>
		/// <param name="transactionId">A token used to identify the holder of the lock.</param>
		void ReleaseLock(Guid transactionId);

		/// <summary>
		/// Releases the reader lock on this record.
		/// </summary>
		/// <param name="transactionId">A token used to identify the holder of the lock.</param>
		void ReleaseReaderLock(Guid transactionId);

		/// <summary>
		/// Releases the writer lock on this record.
		/// </summary>
		/// <param name="transactionId">A token used to identify the holder of the lock.</param>
		void ReleaseWriterLock(Guid transactionId);

	}

}
