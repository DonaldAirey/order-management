namespace Teraque
{

	using System;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.IO;
	using System.IO.IsolatedStorage;
	using System.Security;
	using System.Xml.Serialization;
	using Teraque.Properties;

	/// <summary>
	/// A collection of the most recently used items.
	/// </summary>
	/// <typeparam name="T">The type of item held in the collection.</typeparam>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class MruCollection<T> : ObservableCollection<T>
	{

		/// <summary>
		/// The default maximum number of items for the list.
		/// </summary>
		const Int32 defaultLimit = 10;

		/// <summary>
		/// This prefix is used to make the persistent storage file name more unique.
		/// </summary>
		const String filePrefix = "MruCollection";

		/// <summary>
		/// This prefix is used to make the persistent storage file name more unique.
		/// </summary>
		const String fileSuffix = ".xml";

		/// <summary>
		/// Used to prevent recursive updates to the persistent file when initializing.
		/// </summary>
		Boolean isInitialized;

		/// <summary>
		/// The maximum number of items in the list.
		/// </summary>
		Int32 limit = MruCollection<T>.defaultLimit;

		/// <summary>
		/// The unique name of the list.
		/// </summary>
		String name;

		/// <summary>
		/// Initializes a new instance of the MruCollection class.
		/// </summary>
		public MruCollection()
		{

			// Initialize the object.
			this.name = String.Empty;
			this.limit = MruCollection<T>.defaultLimit;

			// This collection is persistent and is initialize from local storage.
			this.LoadCollection();

		}

		/// <summary>
		/// Initializes a new instance of the MruCollection class.
		/// </summary>
		/// <param name="name">The unique name of the list.</param>
		public MruCollection(String name)
		{

			// Validate the arguments.
			if (name == null)
				throw new ArgumentNullException("name");

			// Initialize the object.
			this.name = name;
			this.limit = MruCollection<T>.defaultLimit;

			// This collection is persistent and is initialize from local storage.
			this.LoadCollection();

		}

		/// <summary>
		/// Initializes a new instance of the MruCollection class.
		/// </summary>
		/// <param name="name">The unique name of the list.</param>
		/// <param name="limit">The maximum number of items in the list.</param>
		public MruCollection(String name, Int32 limit)
		{

			// Validate the arguments.
			if (name == null)
				throw new ArgumentNullException("name");
			if (limit <= 0)
				throw new ArgumentOutOfRangeException("limit");

			// Initialize the object.
			this.name = name;
			this.limit = limit;

			// This collection is persistent and is initialize from local storage.
			this.LoadCollection();

		}

		/// <summary>
		/// Gets the name of the list.
		/// </summary>
		/// <value>The name of the list.</value>
		public String Name
		{

			get
			{
				return this.name;
			}

			set
			{

				// Validate the value.
				if (value == null)
					throw new ArgumentNullException("value");

				// Force a property change which will save the current collection to disk.
				if (this.name != value)
				{
					this.Name = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Name"));
				}

			}

		}

		/// <summary>
		/// Gets the maximum number of items in the MRU list.
		/// </summary>
		/// <value>The maximum number of items in the list.</value>
		public Int32 Limit
		{

			get
			{
				return this.limit;
			}

			set
			{

				// Validate the value.
				if (limit <= 0)
					throw new ArgumentOutOfRangeException("value");

				// Force a property change which will save the current collection to disk.
				if (this.limit != value)
				{
					this.limit = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Limit"));
				}
			}

		}

		/// <summary>
		/// Adds the given item to the MRU list.
		/// </summary>
		/// <param name="item">The item to be added to the list.</param>
		public void Push(T item)
		{

			// Remove the item from it's current position if it already exists in the list and place it at the start.
			if (this.Contains(item))
				this.Remove(item);
			this.Insert(0, item);

		}

		/// <summary>
		/// Load the collection from the isolated store.
		/// </summary>
		void LoadCollection()
		{

			// This will initialize the list from a persistent file.  As seeding the list will cause an update event to fire -- and this update event handler would 
			// write the file -- it's important to block re-entrancy while the list is initialized.  A field -- isInitialized -- will prevent the trigger from 
			// writing to the persistent storage until the field is set.  Also, since there are a lot of I/O operations, it's a good idea to catch all the 
			// exceptions that might occur with the isolated storage and serialization.
			try
			{

				// This will load the contents of the collection from a file in the persistent storage (if it exists).
				using (IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForAssembly())
				{
					String fileName = MruCollection<T>.filePrefix + Name + MruCollection<T>.fileSuffix;
					if (isolatedStorageFile.GetFileNames(fileName).Length != 0)
						using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(fileName, FileMode.Open, FileAccess.Read, isolatedStorageFile))
						{
							XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<T>));
							ObservableCollection<T> clone = xmlSerializer.Deserialize(stream) as ObservableCollection<T>;
							for (Int32 index = 0; index < clone.Count; index++)
								this.Insert(index, clone[index]);
						}
				}

			}
			catch (SecurityException) { }
			catch (InvalidOperationException) { }
			catch (IsolatedStorageException) { }

			// At this point the list is initialized and the event triggers are used to save any changes to disk.
			this.isInitialized = true;

		}

		/// <summary>
		/// Raises the PropertyChanged event with the provided arguments.
		/// </summary>
		/// <param name="e">Arguments of the event being raised.</param>
		protected override void OnPropertyChanged(PropertyChangedEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// Changing the limit or the name of the collection will change how the file is stored on disk.
			if (e.PropertyName == "Limit" || e.PropertyName == "Name")
				this.SaveCollection();

		}

		/// <summary>
		/// Raises the CollectionChanged event with the provided arguments.
		/// </summary>
		/// <param name="e">Arguments of the event being raised.</param>
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{

			// This will remove the items that don't fit into the MRU list.
			for (Int32 index = this.Limit; index < this.Count; index++)
				this.RemoveAt(index);

			// Each time the collection is changed (except when loading) it will be written to the isolated storage for persistence.
			this.SaveCollection();

			// Allow the base class to handle the rest of the event.
			base.OnCollectionChanged(e);

		}

		/// <summary>
		/// Saves the collection to the isolated storage.
		/// </summary>
		void SaveCollection()
		{

			// Saving a list to local storage can generate I/O errors.  They are intensionally trapped here and not passed on to the higher levels.  If a list can't
			// be saved to local storage then there is nothing a user can do about it anyway.  The feedback will be that the preferences are not saved.
			if (this.isInitialized)
				try
				{

					// This will store the list to the local storage associated with this assembly.
					using (IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForAssembly())
					{

						// Prepare the local file stream for writing using the unique list name.  The file is truncated if it already exists, created if it doesn't.
						String fileName = MruCollection<T>.filePrefix + Name + MruCollection<T>.fileSuffix;
						IsolatedStorageFileStream isolatedStorageFileStream = new IsolatedStorageFileStream(
							fileName,
							isolatedStorageFile.GetFileNames(fileName).Length == 0 ? FileMode.Create : FileMode.Truncate,
							FileAccess.Write,
							isolatedStorageFile);

						// This will save the collection to the local storage.
						using (isolatedStorageFileStream)
						{
							XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<T>));
							xmlSerializer.Serialize(isolatedStorageFileStream, this);
						}

					}

				}
				catch (SecurityException) { }
				catch (InvalidOperationException) { }
				catch (IsolatedStorageException) { }

		}

	}

}