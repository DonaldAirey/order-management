namespace Teraque
{

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
	using Teraque.Windows;
	using Teraque.Windows.Controls;

	/// <summary>
	/// Displays multiple views of the content in a directory-like environment.
	/// </summary>
	/// <copyright>Copyright © 2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class CategoryPage : ViewPage
	{

		/// <summary>
		/// Specifies the properties used for building an object dropped onto this surface.
		/// </summary>
		class ItemProperty
		{

			/// <summary>
			/// The URI of an icon used to display this object.
			/// </summary>
			public String IconUri;

			/// <summary>
			/// The object type for this object.
			/// </summary>
			public Guid typeId;

			/// <summary>
			/// The URI of the viewer used to present the data in this object.
			/// </summary>
			public String ViewerUri;

			/// <summary>
			/// Initializes a new instance of the ExternalObject class.
			/// </summary>
			/// <param name="iconUri">The URI of an icon used to display this object.</param>
			/// <param name="typeId">The object type.</param>
			/// <param name="viewerUri">The URI of the viewer used to present the data in this object.</param>
			public ItemProperty(String iconUri, Guid typeId, String viewerUri)
			{

				// Initialize the object
				this.typeId = typeId;
				this.IconUri = iconUri;
				this.ViewerUri = viewerUri;
			}

		};

		/// <summary>
		/// Displays an image of the object being dragged.
		/// </summary>
		Window dragWindow;

		/// <summary>
		/// Contains the properties associated with different file types used for managing the drag and drop operations.
		/// </summary>
		static Dictionary<String, ItemProperty> itemProperties = new Dictionary<String, ItemProperty>()
		{
		};

		/// <summary>
		/// Initializes a new instance of CategoryPage class.
		/// </summary>
		public CategoryPage()
		{

			// The IDE managed resources are initialized here.
			this.InitializeComponent();

		}

		/// <summary>
		/// Copies the input stream to the output stream.
		/// </summary>
		/// <param name="inputStream"></param>
		/// <param name="outputStream"></param>
		static void Copy(Stream inputStream, Stream outputStream)
		{

			// Move chunks of memory from the input to the output stream until they've all been moved.
			Int32 bufferLength = 0x1000;
			Byte[] buffer = new Byte[bufferLength];
			Int32 bytesRead = inputStream.Read(buffer, 0, bufferLength);
			while (bytesRead > 0)
			{
				outputStream.Write(buffer, 0, bytesRead);
				bytesRead = inputStream.Read(buffer, 0, bufferLength);
			}

		}

		/// <summary>
		/// Invoked when an unhandled DragDrop.DragEnter attached event reaches an element in its route that is derived from this class.
		/// </summary>
		/// <param name="dragEventArgs">The DragEventArgs that contains the event data.</param>
		protected override void OnDragEnter(DragEventArgs dragEventArgs)
		{

			// This will find an icon resource name for the kind of object that is being dragged.  The general idea is that we want to construct an image of the
			// object being dragged as a visual cue.
			String iconUri = String.Empty;
			String[] paths = dragEventArgs.Data.GetData("FileDrop") as String[];
			if (paths != null)
				foreach (String path in paths)
				{
					ItemProperty dropObjectAttribute;
					CategoryPage.itemProperties.TryGetValue(Path.GetExtension(path).ToLower(), out dropObjectAttribute);
					iconUri = dropObjectAttribute.IconUri;
				}

			// This is the image that will be placed in the DragWindow as it moves around with the cursor.
			Image image = new Image();

			// If we found a icon in the resources for the kind of object being dragged, then we'll load it into memory and take the extra large version of the icon
			// (there are usually several image sizes in a single icon).
			if (!String.IsNullOrEmpty(iconUri))
			{
				Stream sourceStream = Application.GetResourceStream(new Uri(iconUri, UriKind.Relative)).Stream;
				MemoryStream iconStream = new MemoryStream();
				CategoryPage.Copy(sourceStream, iconStream);
				Dictionary<ImageSize, ImageSource> images = ImageHelper.DecodeIcon(iconStream.ToArray());
				image.Source = images[ImageSize.ExtraLarge];
			}

			// This will create the actual window that hold the image if the item being dragged.
			this.dragWindow = new DragWindow();
			this.dragWindow.Content = image;
			this.dragWindow.Show();

		}

		/// <summary>
		/// Invoked when an unhandled DragDrop.DragLeave attached event reaches an element in its route that is derived from this class.
		/// </summary>
		/// <param name="dragEventArgs">The DragEventArgs that contains the event data.</param>
		protected override void OnDragLeave(DragEventArgs dragEventArgs)
		{

			// Remove the window with the image of the item being dragged.
			this.dragWindow.Close();
			this.dragWindow = null;

		}

		/// <summary>
		/// Invoked when an unhandled DragDrop.DragEnter attached event reaches an element in its route that is derived from this class.
		/// </summary>
		/// <param name="dragEventArgs">The DragEventArgs that contains the event data.</param>
		protected override void OnDragOver(DragEventArgs dragEventArgs)
		{

			// This will determine if the type of object being dragged can be dropped on this surface.  If we have a handler for the object type, then it can be
			// dropped.
			Boolean canDrop = false;
			String[] paths = dragEventArgs.Data.GetData("FileDrop") as String[];
			if (paths != null)
			{
				canDrop = true;
				foreach (String path in paths)
					if (!CategoryPage.itemProperties.ContainsKey(Path.GetExtension(path).ToLower()))
						canDrop = false;
			}

			// At the point we get this message, we've already created a DragWindow holding an image of the item being dragged.  This will move the window so that
			// it always appears to be underneath the cursor.
			Point mousePosition = dragEventArgs.GetPosition(this);
			Point dragDropPosition = this.PointToScreen(mousePosition);
			this.dragWindow.Left = dragDropPosition.X - this.dragWindow.Width / 2.0;
			this.dragWindow.Top = dragDropPosition.Y - this.dragWindow.Height + 10.0;

			// The cursor effect is determined by whether or not the object can be dropped on this surface.
			dragEventArgs.Effects = canDrop ? DragDropEffects.Copy : DragDropEffects.None;
			dragEventArgs.Handled = true;

		}

		/// <summary>
		/// Invoked when an unhandled DragDrop.DragEnter attached event reaches an element in its route that is derived from this class.
		/// </summary>
		/// <param name="dragEventArgs">The DragEventArgs that contains the event data.</param>
		protected override void OnDrop(DragEventArgs dragEventArgs)
		{

			// Close (and dereference) the window used to display the object being dragged.
			this.dragWindow.Close();
			this.dragWindow = null;

			// Attempt to find the element currently selected in the page.  This item will be the parent of any object dropped onto the surface.
			CategoryCollection parentItem = ExplorerHelper.FindExplorerItem(this.DataContext as CategoryCollection, this.Source) as CategoryCollection;
			if (parentItem != null)
			{

				// We can import files dropped onto the surface.  If the format specifies one or more files dragged from the Windows Explorer, then set call a 
				// handler to import each filed based on the file type (that is, the extension).
				String[] paths = dragEventArgs.Data.GetData("FileDrop") as String[];
				if (paths != null)
					foreach (String path in paths)
					{
						ItemProperty externalObject;
						if (CategoryPage.itemProperties.TryGetValue(Path.GetExtension(path).ToLower(), out externalObject))
							CategoryPage.CreateItem(parentItem, path, externalObject);
					}

			}

		}

		/// <summary>
		/// Creates a data model entry for the given path element.
		/// </summary>
		/// <param name="categoryCollection"></param>
		/// <param name="path">The full file name of the object to be loaded.</param>
		/// <param name="externalObject">The properties used to build the object.</param>
		static void CreateItem(CategoryCollection categoryCollection, String path, ItemProperty externalObject)
		{

			// Create a new record for this object.
			DataSet.RootRow rootRow = DataModel.DataSet.Root.NewRootRow();
			rootRow.RootId = Guid.NewGuid();
			rootRow.Name = Path.GetFileName(path);
			rootRow.TypeId = externalObject.typeId;

			// Create an image for this item.
			Stream sourceStream = Application.GetResourceStream(new Uri(externalObject.IconUri, UriKind.Relative)).Stream;
			MemoryStream iconStream = new MemoryStream();
			CategoryPage.Copy(sourceStream, iconStream);
			rootRow.Image = Convert.ToBase64String(iconStream.ToArray());

			// Add this new record to the data model.
			DataModel.DataSet.Root.AddRootRow(rootRow);

			// If a viewer was provided, then make a property for it.
			if (String.IsNullOrEmpty(externalObject.ViewerUri))
			{
				DataSet.PropertyStoreRow viewerPropertyRow = DataModel.DataSet.PropertyStore.NewPropertyStoreRow();
				viewerPropertyRow.PropertyId = PropertyId.Viewer;
				viewerPropertyRow.PropertyStoreId = Guid.NewGuid();
				viewerPropertyRow.RootId = rootRow.RootId;
				viewerPropertyRow.Value = externalObject.ViewerUri;
				DataModel.DataSet.PropertyStore.AddPropertyStoreRow(viewerPropertyRow);
			}

			// Create the metadata for the creation date.
			DataSet.PropertyStoreRow dateCreatedPropertyRow = DataModel.DataSet.PropertyStore.NewPropertyStoreRow();
			dateCreatedPropertyRow.PropertyId = PropertyId.DateCreated;
			dateCreatedPropertyRow.PropertyStoreId = Guid.NewGuid();
			dateCreatedPropertyRow.RootId = rootRow.RootId;
			dateCreatedPropertyRow.Value = DateTime.Now;
			DataModel.DataSet.PropertyStore.AddPropertyStoreRow(dateCreatedPropertyRow);

			// Create the metadata for the creation time.
			DataSet.PropertyStoreRow dateModifiedPropertyRow = DataModel.DataSet.PropertyStore.NewPropertyStoreRow();
			dateModifiedPropertyRow.PropertyId = PropertyId.DateModified;
			dateModifiedPropertyRow.PropertyStoreId = Guid.NewGuid();
			dateModifiedPropertyRow.RootId = rootRow.RootId;
			dateModifiedPropertyRow.Value = DateTime.Now;
			DataModel.DataSet.PropertyStore.AddPropertyStoreRow(dateModifiedPropertyRow);

			// This will load a binary version of the file into the metadata.
			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				MemoryStream memoryStream = new MemoryStream();
				CategoryPage.Copy(fileStream, memoryStream);
				DataSet.PropertyStoreRow dataPropertyRow = DataModel.DataSet.PropertyStore.NewPropertyStoreRow();
				dataPropertyRow.PropertyId = PropertyId.Data;
				dataPropertyRow.PropertyStoreId = Guid.NewGuid();
				dataPropertyRow.RootId = rootRow.RootId;
				dataPropertyRow.Value = Convert.ToBase64String(memoryStream.GetBuffer(), 0, Convert.ToInt32(memoryStream.Length));
				DataModel.DataSet.PropertyStore.AddPropertyStoreRow(dataPropertyRow);
			}

			// This will create an association between the parent item and the object just created.
			DataSet.TreeRow treeRow = DataModel.DataSet.Tree.NewTreeRow();
			treeRow.ParentId = categoryCollection.RootId;
			treeRow.ChildId = rootRow.RootId;
			treeRow.TreeId = Guid.NewGuid();
			DataModel.DataSet.Tree.AddTreeRow(treeRow);

			// This will insert the new object in the View Model at the given location in the hierarchy.
			categoryCollection.Leaves.Add(new CategoryCollection(categoryCollection, rootRow));

		}

	}

}
