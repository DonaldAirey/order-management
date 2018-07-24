namespace Teraque
{

	using System;
	using System.Collections;
    using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
    using System.Threading;

	/// <summary>
	/// A thread-safe queue of objects of TType that will cause the consumer to block until an item is available.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
	[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
	public class WaitQueue<TType>
	{

		/// <summary>
		/// The queue of object of TType.
		/// </summary>
		Queue<TType> queue;

		/// <summary>
		/// Wakes up the consumer when an object of TType is available.
		/// </summary>
		ManualResetEvent queueEvent;

		/// <summary>
		/// Initializes a new instance of the WaitQueue class.
		/// </summary>
		public WaitQueue()
		{

			// This queue is used to hold the elements.
			this.queue = new Queue<TType>();

			// This event is used to signal a waiting thread that new data is avaiable in the ticker.
			this.queueEvent = new ManualResetEvent(false);

		}

		/// <summary>
		/// Place an object in the queue.
		/// </summary>
		/// <param name="queueObject">The object to be placed in the queue.</param>
		public void Enqueue(TType queueObject)
		{

			try
			{

				// Make sure we release the lock when we exit.
				Monitor.Enter(this.queue);

				// Place the object in the queue.
				this.queue.Enqueue(queueObject);

				// Signal anyone waiting on a tick that one is ready in the queue.
				if (this.queue.Count == 1)
					this.queueEvent.Set();

			}
			finally
			{
				Monitor.Exit(this.queue);
			}

		}

		/// <summary>
		/// Gets an indication whether the queue is empty or not.
		/// </summary>
		public Boolean IsEmpty
		{
			get
			{
				Boolean isEmpty = true;
				try
				{
					Monitor.Enter(this.queue);
					isEmpty = this.queue.Count == 0;
				}
				finally
				{
					Monitor.Exit(this.queue);
				}
				return isEmpty;
			}
		}

		/// <summary>
		/// Gets the number of items in the queue.
		/// </summary>
		public Int32 Count
		{
			get
			{
				Int32 count;
				try
				{
					Monitor.Enter(this.queue);
					count = this.queue.Count;
				}
				finally
				{
					Monitor.Exit(this.queue);
				}
				return count;
			}
		}

		/// <summary>
		/// Returns the element at the beginning of the Teraque.WaitQueue&lt;T&gt; without removing it.
		/// </summary>
		/// <returns>The next price record on the queue.</returns>
		public TType Peek()
		{

			try
			{

				// Insure thread safety.
				Monitor.Enter(this.queue);

				// If there is nothing in the queue, wait until something is put in the other end.
				if (this.queue.Count == 0)
				{
					Monitor.Exit(this.queue);
					this.queueEvent.WaitOne();
					Monitor.Enter(this.queue);
				}

				// Remove the first item placed in the queue.
				return this.queue.Peek();

			}
			finally
			{

				// The queue doesn't need to be blocked any longer.
				Monitor.Exit(this.queue);

			}

		}

		/// <summary>
		/// Remove an item from the queue.
		/// </summary>
		/// <returns>The next object of TType in the queue.</returns>
		public TType Dequeue()
		{
			Boolean timedOut;
			return this.Dequeue(-1, out timedOut);
		}


		/// <summary>
		/// Remove an item of TType from the queue.
		/// </summary>
		/// <param name="timeout">The time to wait for an object.</param>
		/// <param name="isTimedOut">Indicates that the thread timed out waiting for an object.</param>
		/// <returns>The most first item placed in the queue.</returns>
		[SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
		public TType Dequeue(Int32 timeout, out Boolean isTimedOut)
		{

			isTimedOut = false;
			try
			{

				// Insure thread safety.
				Monitor.Enter(this.queue);

				// Wait for an item of TType to be placed in the queue.
				if (this.queue.Count == 0)
				{

					Monitor.Exit(this.queue);
					try
					{
						if (this.queueEvent.WaitOne(timeout) == false)
						{
							isTimedOut = true;
							return default(TType);
						}
					}
					finally
					{
						Monitor.Enter(this.queue);
					}
				}

				// Remove the first item placed in the queue.
				TType queueObject = this.queue.Dequeue();

				// If there is nothing left in the queue, then clear the event.  This will block any calls to 'Dequeue' until there is something to extract from the
				// queue.
				if (this.queue.Count == 0)
					this.queueEvent.Reset();

				// This is the first item placed in the queue.
				return queueObject;

			}
			finally
			{

				// The queue can be accessed by other threads now.
				Monitor.Exit(this.queue);

			}

		}

	}
	
}
