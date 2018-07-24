namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using System.Windows;
	using System.Windows.Media;
	using System.Windows.Threading;
	using Teraque;
	using Teraque.Windows;
	using Teraque.AssetNetwork.Properties;
	using Teraque.AssetNetwork.WebService;

	/// <summary>
	/// A hierarchical view of the data displayed in an Explorer application.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class AssetNetworkCollection : AssetNetworkItem, IDisposable
	{

		/// <summary>
		/// Maps an entity id to an AssetNetwork item in the hierarchical collection.
		/// </summary>
		Dictionary<Guid, AssetNetworkItem> entityItemMap = new Dictionary<Guid, AssetNetworkItem>();

		/// <summary>
		/// The thread used to get the user identification from the web service.
		/// </summary>
		Thread getUserIdThread;

		/// <summary>
		/// Initialize a new instance of the AssetNetworkCollection class.
		/// </summary>
		public AssetNetworkCollection()
			: base()
		{

			// Install the event handlers for keeping the collection synchronized to the data model.
			DataModel.User.UserRowChanged += this.OnChangeUserRow;
			DataModel.Entity.EntityRowChanged += this.OnChangeEntityRow;
			DataModel.EntityTree.EntityTreeRowChanged += this.OnChangeEntityTreeRow;
			DataModel.PropertyStore.PropertyStoreRowChanged += this.OnPropertyStoreRowChanged;

			// A user identifier is needed to give a context to this collection.  The user identifier determines the collection of items in this list.  A thread is
			// spawned in the background to collect the identifier which is then passed back to the foreground so a collection associated with this user can be
			// built.
			this.getUserIdThread = new Thread(this.CollectorThread);
			this.getUserIdThread.Name = "GetUserIdThread";
			this.getUserIdThread.Start();

		}

		/// <summary>
		/// Finalize this instance of the AssetNetworkCollection class.
		/// </summary>
		~AssetNetworkCollection()
		{

			// Call the virtual method to dispose of the resources.  This (recommended) design pattern gives any derived classes a chance to clean up unmanaged 
			// resources even though this base class has none.
			this.Dispose(false);

		}

		/// <summary>
		/// Add a descendant to the list of items managed by this collection.
		/// </summary>
		/// <param name="assetNetworkItem"></param>
		internal void AddDescendant(AssetNetworkItem assetNetworkItem)
		{

			// This list is used to watch for changes to the collection.
			this.entityItemMap.Add(assetNetworkItem.EntityId, assetNetworkItem);

		}

		/// <summary>
		/// Asks the web service for the user's unique identification.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		void CollectorThread()
		{

			// This will hold the user's unique identifier retrieved from the web service.
			Guid userId;

			while (true)
			{
				try
				{

					// Create a web service to ask for the user's identifier.
					using (WebServiceClient webServiceClient = new WebServiceClient(Settings.Default.WebServiceEndpoint))
					{
						userId = webServiceClient.GetUserId();
						break;
					}
				}
				catch { }
			}

			// Once we have a user id, we'll kick off the foreground process to create the hierarchy of items using that id.
			Application.Current.Dispatcher.BeginInvoke(new Action<Guid>(this.CreateCollection), userId);

		}
		
		/// <summary>
		/// Initializes the collection with the user id collected from the web service.
		/// </summary>
		/// <param name="userId">The identifier of the current user.</param>
		void CreateCollection(Guid userId)
		{

			// The 'UpdateCollection' will be called several times during initialization since it is tied directly to events in the data model.  However, until it
			// has a context, it won't be able to do anything.  This will force the root of the list to have a context from which we can evaluate the data model
			// changes and integrate them into the collection when those changes are germain (i.e. related to this user's hierarchy).
			this.EntityId = userId;

			// The rest of the initialization looks just like an update.
			this.UpdateCollection();

		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{

			// Call the virtual method to allow derived classes to clean up resources.
			this.Dispose(true);

			// Since we took care of cleaning up the resources, there is no need to call the finalizer.
			GC.SuppressFinalize(this);

		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing">true to indicate that the object is being disposed, false to indicate that the object is being finalized.</param>
		protected virtual void Dispose(Boolean disposing)
		{

			// Kill the thread that gets the user id if it's still alive at this point.
			if (this.getUserIdThread.IsAlive)
				this.getUserIdThread.Abort();

			// This will remove this object from the data model events.
			if (disposing)
			{
				DataModel.User.UserRowChanged -= this.OnChangeUserRow;
				DataModel.Entity.EntityRowChanged -= this.OnChangeEntityRow;
				DataModel.EntityTree.EntityTreeRowChanged -= this.OnChangeEntityTreeRow;
				DataModel.PropertyStore.PropertyStoreRowChanged -= this.OnPropertyStoreRowChanged;
			}

		}

		/// <summary>
		/// Handles a change to the DataModel.Entity table.
		/// </summary>
		/// <param name="sender">Object that generated the event.</param>
		/// <param name="entityRowChangeEvent">The event arguments.</param>
		void OnChangeEntityRow(Object sender, DataModel.EntityRowChangeEventArgs entityRowChangeEvent)
		{

			// Update the entire collection when the relationship between entities has changed.
			this.UpdateCollection();

		}

		/// <summary>
		/// Handles a change to the DataModel.EntityTree table.
		/// </summary>
		/// <param name="sender">Object that generated the event.</param>
		/// <param name="entityTreeRowChangeEvent">The event arguments.</param>
		void OnChangeEntityTreeRow(Object sender, DataModel.EntityTreeRowChangeEventArgs entityTreeRowChangeEvent)
		{

			// Update the entire collection when the relationship between entities has changed.
			this.UpdateCollection();

		}

		/// <summary>
		/// Handles a change to the DataModel.Property table.
		/// </summary>
		/// <param name="sender">User that generated the event.</param>
		/// <param name="userRowChangeEvent">The event arguments.</param>
		void OnPropertyStoreRowChanged(object sender, DataModel.PropertyStoreRowChangeEventArgs e)
		{

			// Update the entire collection when an item in this collection has it's property changed.
			if (this.entityItemMap.ContainsKey(e.Row.EntityId))
				this.UpdateCollection();

		}

		/// <summary>
		/// Handles a change to the DataModel.User table.
		/// </summary>
		/// <param name="sender">User that generated the event.</param>
		/// <param name="userRowChangeEvent">The event arguments.</param>
		void OnChangeUserRow(Object sender, DataModel.UserRowChangeEventArgs userRowChangeEvent)
		{

			// Update the entire collection when the user data changes.
			this.UpdateCollection();

		}

		/// <summary>
		/// Remove a descendant from the list of items managed by this collection.
		/// </summary>
		/// <param name="assetNetworkItem"></param>
		internal void RemoveDescendant(AssetNetworkItem assetNetworkItem)
		{

			// This list is used to watch for changes to the collection.
			this.entityItemMap.Remove(assetNetworkItem.EntityId);

		}

		/// <summary>
		/// Recursively updates the collection from the data model.
		/// </summary>
		void UpdateCollection()
		{

			// This will construct the hierarchy of objects for which this user has access.
			DataModel.UserRow userRow = DataModel.User.UserKey.Find(this.EntityId);
			if (userRow != null)
				this.Copy(this, userRow.EntityRow);

		}

	}

}
