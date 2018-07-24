namespace Teraque
{

	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.ServiceModel;
	using System.Threading;

	/// <summary>
	/// Base class for all rows in the generated data model.
	/// </summary>
	/// <remarks>This class was originally generated so as to make the generated data model more self-contained.  But as the pressures of debugging increased, it 
	/// was removed to a base class.  I believe the ideal is still valid, so one day in the future we should examine the possibility of re-integrating this into the
	/// code generation process and relieve the generated code of the dependancy on the base libraries.</remarks>
	public abstract class DataRowBase : DataRow, IRow
	{

		/// <summary>
		/// A set of all the readers (transactions) waiting for a read lock.
		/// </summary>
		HashSet<Guid> readers = new HashSet<Guid>();

		/// <summary>
		/// The number of readers waiting for a read lock.
		/// </summary>
		Int32 readerWaiters;

		/// <summary>
		/// Used to synchronize access to the reader lock.
		/// </summary>
		Object readRoot = new Object();

		/// <summary>
		/// Used to synchronize access to the housekeeping fields.
		/// </summary>
		Object syncRoot = new Object();

		/// <summary>
		/// The owner of the write lock.
		/// </summary>
		Guid writer = Guid.Empty;

		/// <summary>
		/// Used to synchronize access to the writer lock.
		/// </summary>
		Object writerRoot = new Object();

		/// <summary>
		/// The number of writers waiting for a write lock.
		/// </summary>
		Int64 writerWaiters;

		/// <summary>
		/// Initialize a new instance of the DataRowBase class.
		/// </summary>
		/// <param name="dataRowBuilder">The source row for building a DataRowBase object.</param>
		protected DataRowBase(DataRowBuilder dataRowBuilder) : base(dataRowBuilder) { }

		/// <summary>
		/// returns LockTimeout for dataSet
		/// </summary>
		public abstract TimeSpan LockTimeout { get; }

		/// <summary>
		/// Acquires a reader lock for this record and add the row to the given transaction.
		/// </summary>
		/// <param name="dataModelTransaction">The transaction context for this operation.</param>
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
		public virtual void AcquireReaderLock(IDataModelTransaction dataModelTransaction)
		{

			// Use the internal method to acquire the read lock.
			this.AcquireReaderLock(dataModelTransaction.TransactionId, this.LockTimeout);

			// This can now be added to the list of rows locked by the transaction.
			dataModelTransaction.AddLock(this);

			// This will insure that we haven't attempted to lock a deleted row.  This is a helper function to make the server-side transactions easier to write
			// and, thus, there's the potential to make mistakes that the generated data model would not make.
			if (this.RowState == DataRowState.Detached)
				throw new LockException(String.Format(CultureInfo.CurrentCulture, Properties.Resources.DeletedAfterLockedError, this.GetType()));

		}

		/// <summary>
		/// Acquires a reader lock.
		/// </summary>
		/// <param name="transactionId">A token used to identify the holder of the lock.</param>
		/// <param name="timeSpan">The time that the thread will wait for the lock.</param>
		[DebuggerNonUserCodeAttribute()]
		public virtual void AcquireReaderLock(Guid transactionId, TimeSpan timeSpan)
		{
			try
			{

				// Wait for the housekeeping fields to become available.
				Monitor.Enter(this.syncRoot);

				// At this point we can join the number of readers waiting to read.
				this.readerWaiters++;

				// It is possible to time-out while waiting for a reader lock.  If this happens, we don't want to acquire the lock.
				Boolean isReaderLockAcquired = true;

				// If we don't own the write lock, then we need to join a collection of transactions waiting for the read lock to become available.
				if (this.writer != transactionId)
				{

					try
					{
						try
						{

							// This will wait until another thread pulses the readers and allows them to wake up.  Note that while this transaction is waiting to be
							// woken up, we are not going to stop writers from trying to acquire a lock.
							Monitor.Enter(this.readRoot);
							Monitor.Exit(this.syncRoot);
							while (this.writer != Guid.Empty)
								isReaderLockAcquired = Monitor.Wait(this.readRoot, timeSpan);

						}
						finally
						{

							// At this point we either acquired the lock, or timed out.
							Monitor.Exit(this.readRoot);

						}
					}
					finally
					{

						// At this point we're going acquire access to the housekeeping fields again so we can conclude the lock operation.
						Monitor.Enter(this.syncRoot);

					}

					// If the transaction didn't time-out while waiting for the reader lock, then we're free to acquire it now (placing the transaction identifier 
					// in the reader set is all that's required to obtain the lock).
					if (isReaderLockAcquired)
						this.readers.Add(transactionId);

				}

			}
			finally
			{

				// When we've obtained the reader lock, we can drop the number of transactions waiting for the lock to balance the books.
				this.readerWaiters--;

				// We no longer need to keep the housekeeping fields locked.
				Monitor.Exit(this.syncRoot);

			}

		}

		/// <summary>
		/// Acquires a writer lock for this record.
		/// </summary>
		/// <param name="dataModelTransaction">The transaction context for this operation.</param>
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
		public virtual void AcquireWriterLock(IDataModelTransaction dataModelTransaction)
		{

			// This is a helper method and really just combines three frequently used operations to make the server-side code cleaner to read.
			this.AcquireWriterLock(dataModelTransaction.TransactionId, this.LockTimeout);
			dataModelTransaction.AddLock(this);

			// This will insure that we haven't attempted to lock a deleted row.  This is a helper function to make the server-side transactions easier to write
			// and, thus, there's the potential to make mistakes that the generated data model would not make.
			if (this.RowState == DataRowState.Detached)
				throw new LockException(String.Format(CultureInfo.CurrentCulture, Properties.Resources.DeletedAfterLockedError, this.GetType()));

		}

		/// <summary>
		/// Acquires a writer lock.
		/// </summary>
		/// <param name="transactionId">A token used to identify the holder of the lock.</param>
		/// <param name="timeSpan">The time that the thread will wait for the lock.</param>
		public virtual void AcquireWriterLock(Guid transactionId, TimeSpan timeSpan)
		{

			try
			{

				// This prevents other threads from examining and changing the housekeeping fields while this transaction uses them.
				Monitor.Enter(this.syncRoot);

				// This keeps track of the number of transactions waiting for a write lock.
				this.writerWaiters++;

				// If this transaction doesn't already own the write lock, then we will either aquire it or wait until we can acquire it.
				if (this.writer != transactionId)
				{

					// If we are already own a reader lock, then it will be promoted to a writer lock.
					this.readers.Remove(transactionId);

					// This indicates whether we timed out of successfully acquire the writer lock.
					Boolean isWriterLockAcquired = true;

					// While either another transaction has the write lock or there are readers, we are going to wait here until someone pulses us.
					while (this.writer != Guid.Empty || this.readers.Count != 0)
					{
						try
						{
							try
							{

								// We will wait here until we are pulsed by another transaction releasing either all the reader locks or the writer lock.  Note that
								// we don't prevent readers from queuing up while we wait to be woken up.
								Monitor.Enter(this.writerRoot);
								Monitor.Exit(this.syncRoot);
								isWriterLockAcquired = Monitor.Wait(this.writerRoot, timeSpan);

							}
							finally
							{

								// At this point we either acquired the lock, or timed out.  In either event, we don't need to hold the writers any longer.
								Monitor.Exit(this.writerRoot);

							}
						}
						finally
						{

							// At this point we're going acquire access to the housekeeping fields again so we can conclude the lock operation.
							Monitor.Enter(this.syncRoot);

						}

					}

					// In the event that we timed out while waiting for the writer to become available, we won't acquire the writer lock.
					if (isWriterLockAcquired)
						this.writer = transactionId;

				}

			}
			finally
			{

				// Decrement the number of transactions waiting on the writer and free up the housekeeping fields for other threads.
				this.writerWaiters--;
				Monitor.Exit(this.syncRoot);

			}

		}

		/// <summary>
		/// Gets a value indicating whether the owner of a token holds a reader lock.
		/// </summary>
		/// <param name="transactionId">The transaction unique identifier.</param>
		/// <returns>true if the current token owner holds a reader lock.</returns>
		public Boolean IsLockHeld(Guid transactionId)
		{
			try
			{
				Monitor.Enter(this.syncRoot);
				return (this.writer == transactionId || this.readers.Contains(transactionId));
			}
			finally
			{
				Monitor.Exit(this.syncRoot);
			}
		}


		/// <summary>
		/// Gets a value indicating whether the owner of a token holds a reader lock.
		/// </summary>
		/// <returns>true if the current token owner holds a reader lock.</returns>
		public Boolean IsReaderLockHeld(Guid transactionId)
		{
			try
			{
				Monitor.Enter(this.syncRoot);
				return this.readers.Contains(transactionId);
			}
			finally
			{
				Monitor.Exit(this.syncRoot);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the owner of a token holds a writer lock.
		/// </summary>
		/// <returns>true if the current token owner holds a writer lock.</returns>
		public Boolean IsWriterLockHeld(Guid transactionId)
		{
			try
			{
				Monitor.Enter(this.syncRoot);
				return this.writer == transactionId;
			}
			finally
			{
				Monitor.Exit(this.syncRoot);
			}
		}

		/// <summary>
		/// Releases every lock held by this record.
		/// </summary>
		/// <param name="transactionId">A token used to identify the holder of the lock.</param>
		public virtual void ReleaseLock(Guid transactionId)
		{

			try
			{

				// Lock the housekeeping fields while we remove every lock.
				Monitor.Enter(this.syncRoot);

				// This will remove the reader lock and then pulse the next writer if there are no more readers waiting.
				if (this.readers.Remove(transactionId) && this.readers.Count == 0)
					if (this.writerWaiters != 0)
					{
						try
						{
							Monitor.Enter(this.writerRoot);
							Monitor.Pulse(this.writerRoot);
						}
						finally
						{
							Monitor.Exit(this.writerRoot);
						}
					}

				// If a writer lock is held by this transaction we will clear it and first wake up all the readers.  If there are no readers, then we'll wake a 
				// writer.
				if (this.writer == transactionId)
				{

					// This will clear the writer lock.
					this.writer = Guid.Empty;

					// If there are readers queued up at this point, then wake them all.
					if (this.readerWaiters > 0)
					{
						try
						{
							Monitor.Enter(this.readRoot);
							Monitor.PulseAll(this.readRoot);
						}
						finally
						{
							Monitor.Exit(this.readRoot);
						}
					}
					else
					{

						// If there are writers queued up, then pulse them.  The first one to wake up will get the writer lock.
						if (this.writerWaiters > 0)
						{
							try
							{
								Monitor.Enter(this.writerRoot);
								Monitor.Pulse(this.writerRoot);
							}
							finally
							{
								Monitor.Exit(this.writerRoot);
							}
						}

					}

				}

			}
			finally
			{

				// This releases the housekeeping fields for the next thread.
				Monitor.Exit(this.syncRoot);

			}

		}

		/// <summary>
		/// Releases the reader lock on this record.
		/// </summary>
		/// <param name="transactionId">A token used to hold locks.</param>
		public virtual void ReleaseReaderLock(Guid transactionId)
		{

			try
			{

				// Lock the housekeeping fields while we remove every lock.
				Monitor.Enter(this.syncRoot);

				// If the transaction can't be removed from the set of reader locks then we have a synchronization exception to report.
				if (!this.readers.Remove(transactionId))
					throw new FaultException<SynchronizationLockFault>(new SynchronizationLockFault(this.Table.TableName));

				// This will pulse a writer when all the readers have released their locks.  The first one to wake up will obtain the writer lock if there are
				// multiple writers waiting.
				if (this.readers.Count == 0 && this.writerWaiters != 0)
				{
					try
					{
						Monitor.Enter(this.writerRoot);
						Monitor.Pulse(this.writerRoot);
					}
					finally
					{
						Monitor.Exit(this.writerRoot);
					}
				}

			}
			finally
			{

				// We don't need to lock the housekeeping fields any longer.
				Monitor.Exit(this.syncRoot);

			}

		}

		/// <summary>
		/// Releases the writer lock on this record.
		/// </summary>
		/// <param name="transactionId">A token used to hold locks.</param>
		public virtual void ReleaseWriterLock(Guid transactionId)
		{

			try
			{

				// Lock the housekeeping fields while we remove every lock.
				Monitor.Enter(this.syncRoot);

				// If we don't own the writer lock, then we have a syncrhonization fault to report.
				if (this.writer != transactionId)
					throw new FaultException<SynchronizationLockFault>(new SynchronizationLockFault(this.Table.TableName));

				// Clear the writer lock.
				this.writer = Guid.Empty;

				// This will wake all the readers if we have readers queued up.
				if (this.readerWaiters > 0)
				{
					try
					{
						Monitor.Enter(this.readRoot);
						Monitor.PulseAll(this.readRoot);
					}
					finally
					{
						Monitor.Exit(this.readRoot);
					}
				}
				else
				{

					// If there are no readers waiting, then try to wake a writer.  The first one to wake up will get the lock.
					if (this.writerWaiters > 0)
					{
						try
						{
							Monitor.Enter(this.writerRoot);
							Monitor.Pulse(this.writerRoot);
						}
						finally
						{
							Monitor.Exit(this.writerRoot);
						}
					}

				}

			}
			finally
			{

				// We don't need to lock the housekeeping fields any longer.
				Monitor.Exit(this.syncRoot);

			}

		}

	}

}
