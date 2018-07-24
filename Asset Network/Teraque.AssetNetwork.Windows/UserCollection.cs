namespace Teraque.AssetNetwork.Windows
{

    using System;
    using System.Collections.ObjectModel;
    using System.Data;

	/// <summary>
	/// Represents a collection of UserItem elements.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class UserCollection : ObservableCollection<UserItem>, IDisposable
	{

		/// <summary>
		/// Creates a list of UserCollection that is bound to the data model.
		/// </summary>
		public UserCollection()
		{

			// The list is bound to the data model through these event handlers.
			DataModel.Entity.EntityRowChanged += this.OnEntityRowChanged;
			DataModel.User.UserRowChanged += this.OnUserRowChanged;
			DataModel.User.UserRowDeleted += this.OnUserRowDeleted;

			// Create a sorted list from the data model.
			foreach (DataModel.UserRow userRow in DataModel.User)
				this.Insert(~this.BinarySearch(userItem => userItem.UserId, userRow.UserId), new UserItem(userRow));

		}

		/// <summary>
		/// Finalize this instance of the UserList class.
		/// </summary>
		~UserCollection()
		{

			// Call the virtual method to dispose of the resources.  This (recommended) design pattern gives any derived classes a chance to clean up unmanaged 
			// resources even though this base class has none.
			this.Dispose(false);

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

			// This will remove this object from the data model events.
			if (disposing)
			{
				DataModel.Entity.EntityRowChanged -= this.OnEntityRowChanged;
				DataModel.User.UserRowChanged -= this.OnUserRowChanged;
				DataModel.User.UserRowDeleted -= this.OnUserRowDeleted;
			}

		}

		/// <summary>
		/// Handles a change to a Entity record in the data model.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="entityRowChangeEventArgs">The event arguments.</param>
		private void OnEntityRowChanged(Object sender, DataModel.EntityRowChangeEventArgs entityRowChangeEventArgs)
		{

			// Any changes in the Entity table will be copied to only the users in this list.
			if (entityRowChangeEventArgs.Action == DataRowAction.Change)
				foreach (DataModel.UserRow userRow in entityRowChangeEventArgs.Row.GetUserRows())
				{
					Int32 index = this.BinarySearch(userItem => userItem.UserId, userRow.UserId);
					if (index >= 0)
						this[index].Copy(userRow);
				}

		}

		/// <summary>
		/// Handles a change to a User record in the data model.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="userRowChangeEventArgs">The event arguments.</param>
		private void OnUserRowChanged(Object sender, DataModel.UserRowChangeEventArgs userRowChangeEventArgs)
		{

			// We're only interested in additions and changes in this handler.
			if (userRowChangeEventArgs.Action == DataRowAction.Add || userRowChangeEventArgs.Action == DataRowAction.Change)
			{

				// If the item doesn't exist, it is added.  If it exists, it's updated.
				Int32 index = this.BinarySearch(userItem => userItem.UserId, userRowChangeEventArgs.Row.UserId);
				if (index < 0)
					this.Insert(~index, new UserItem(userRowChangeEventArgs.Row));
				else
					this[index].Copy(userRowChangeEventArgs.Row);

			}

		}

		/// <summary>
		/// Handles a change to a User record in the data model.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="userRowChangeEventArgs">The event arguments.</param>
		private void OnUserRowDeleted(Object sender, DataModel.UserRowChangeEventArgs userRowChangeEventArgs)
		{

			// This will delete the item from the list when it is deleted from the data model.
			if (userRowChangeEventArgs.Action == DataRowAction.Delete)
			{
				Int32 index = this.BinarySearch(userItem => userItem.UserId, userRowChangeEventArgs.Row.UserId);
				if (index >= 0)
					this.RemoveAt(index);
			}

		}

	}

}
