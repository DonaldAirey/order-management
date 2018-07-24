namespace Teraque.Windows.Controls.Primitives
{

	using System;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;

	/// <summary>
	/// The manager adds and removes listeners for weak events (or callbacks) for ObservableCollection&lt;ColumnViewColumn&gt;s.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal class CollectionChangedEventManager : WeakEventManager
	{

		/// <summary>
		/// The CollectionChangedEventManager created for this class.
		/// </summary>
		static CollectionChangedEventManager currentManager;

		/// <summary>
		/// Initialize the CollectionChangedEventManager class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static CollectionChangedEventManager()
		{

			// This will create a weak event manager for this class.
			Type weakEventManagerType = typeof(CollectionChangedEventManager);
			CollectionChangedEventManager currentManager = WeakEventManager.GetCurrentManager(weakEventManagerType) as CollectionChangedEventManager;
			CollectionChangedEventManager.currentManager = new CollectionChangedEventManager();
			WeakEventManager.SetCurrentManager(weakEventManagerType, CollectionChangedEventManager.currentManager);

		}

		/// <summary>
		/// Adds a weak listener to changes in the collection.  This weak relation will not prevent the listener from being garbage collected.
		/// </summary>
		/// <param name="columnViewColumnCollection">The ObservableCollection&lt;ColumnViewColumn&gt; that is the source of the events.</param>
		/// <param name="iWeakEventListener">A weak listener to those events.</param>
		public static void AddListener(ObservableCollection<ColumnViewColumn> columnViewColumnCollection, IWeakEventListener iWeakEventListener)
		{

			// Validate the parameters.
			if (columnViewColumnCollection == null)
				throw new ArgumentNullException("columnViewColumnCollection");
			if (iWeakEventListener == null)
				throw new ArgumentNullException("iWeakEventListener");

			// Add a weak event listener for the ObservableCollection<ColumnViewColumn>s.
			CurrentManager.ProtectedAddListener(columnViewColumnCollection, iWeakEventListener);

		}

		/// <summary>
		/// Handles a change to the collection.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="args">The event arguments.</param>
		private void OnCollectionChanged(Object sender, NotifyCollectionChangedEventArgs args)
		{

			// Deliver the message to the listener (if it hasn't been garbage collected).
			this.DeliverEvent(sender, args);

		}

		/// <summary>
		/// Removes a weak listener to changes in the collection.
		/// </summary>
		/// <param name="columnViewColumnCollection">The ObservableCollection<ColumnViewColumn> that is the source of the events.</param>
		/// <param name="iWeakEventListener">A weak listener to those events.</param>
		public static void RemoveListener(ObservableCollection<ColumnViewColumn> columnViewColumnCollection, IWeakEventListener iWeakEventListener)
		{

			// Validate the arguments.
			if (columnViewColumnCollection == null)
				throw new ArgumentNullException("columnViewColumnCollection");
			if (iWeakEventListener == null)
				throw new ArgumentNullException("iWeakEventListener");

			// Remove the weak listener.
			CurrentManager.ProtectedRemoveListener(columnViewColumnCollection, iWeakEventListener);

		}

		/// <summary>
		/// Starts listening for the event being managed. After the StartListening method is first called, the manager should be in the state of calling
		/// DeliverEvent or DeliverEventToList whenever the relevant event from the provided source is handled.
		/// </summary>
		/// <param name="source">The source to begin listening on.</param>
		protected override void StartListening(Object source)
		{

			// Hook the collection into the event manager.
			ObservableCollection<ColumnViewColumn> columns = (ObservableCollection<ColumnViewColumn>)source;
			columns.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);

		}

		/// <summary>
		/// Stops listening on the provided source for the event being managed.
		/// </summary>
		/// <param name="source">The source to stop listening on.</param>
		protected override void StopListening(Object source)
		{

			// This will decouple the ObservableCollection<ColumnViewColumn> from the event manager.
			ObservableCollection<ColumnViewColumn> columns = (ObservableCollection<ColumnViewColumn>)source;
			columns.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);

		}

		/// <summary>
		/// Gets the current Event Manager for this WeakEventManager type.
		/// </summary>
		private static CollectionChangedEventManager CurrentManager
		{
			get
			{
				return CollectionChangedEventManager.currentManager;
			}
		}

	}

}