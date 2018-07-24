namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Data;
	using System.IO;
	using System.Text;
	using System.Threading;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
	using Teraque.AssetNetwork.Properties;
	using Teraque.AssetNetwork.WebService;
	using Teraque.Windows;
	using Teraque.Windows.Controls;
	using Teraque.Windows.Controls.Primitives;

	/// <summary>
	/// Represents a control that displays a directory of items.
	/// </summary>
	/// <copyright>Copyright © 2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class DirectoryPage : ViewPage, INotifyPropertyChanged
	{

		/// <summary>
		/// Specifies the properties used for building an object dropped onto this surface.
		/// </summary>
		class DocumentProperty
		{

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
			public DocumentProperty(Guid typeId, String viewerUri)
			{

				// Initialize the object
				this.typeId = typeId;
				this.ViewerUri = viewerUri;

			}

		};

		/// <summary>
		/// The time the object was created.
		/// </summary>
		DateTime createdTimeField;

		/// <summary>
		/// Contains the properties associated with different file types used for managing the drag and drop operations.
		/// </summary>
		static Dictionary<String, DocumentProperty> documentProperties = new Dictionary<String, DocumentProperty>()
		{
			{".xlsx", new DocumentProperty(TypeId.ExcelWorksheet, null)},
			{".pdf", new DocumentProperty(TypeId.PdfDocument, "pack://application:,,,/Teraque.PdfViewer;component/PdfPage.xaml")}
		};

		/// <summary>
		/// Displays an image of the object being dragged.
		/// </summary>
		Window dragWindow;

		/// <summary>
		/// The entity currently displayed in this directory.
		/// </summary>
		Guid entityIdField;

		/// <summary>
		/// The collection of metadata items that are displayed in the DetailBar.
		/// </summary>
		ObservableCollection<FrameworkElement> metadata = new ObservableCollection<FrameworkElement>();

		/// <summary>
		/// The time the object was last modified.
		/// </summary>
		DateTime modifiedTimeField;

		/// <summary>
		/// Notifies listeners that a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Initializes a new instance of NavigatorDirectory class.
		/// </summary>
		public DirectoryPage()
		{

			// The IDE managed resources are initialized here.
			this.InitializeComponent();

			// The metadata key/value pairs are initialized here.
			this.InitializeMetadata();

			// Load the metadata when the Page is loaded (or reloaded).
			this.Loaded += this.OnLoaded;
			this.Unloaded += this.OnUnloaded;

		}

		/// <summary>
		/// Gets or sets the time the blotter was last created.
		/// </summary>
		public DateTime CreatedTime
		{
			get
			{
				return this.createdTimeField;
			}
			set
			{
				if (this.createdTimeField != value)
				{
					this.createdTimeField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("CreatedTime"));
				}
			}
		}

		/// <summary>
		/// Gets the unique identifier of this entity.
		/// </summary>
		public Guid EntityId
		{
			get
			{
				return this.entityIdField;
			}
		}

		/// <summary>
		/// Gets or sets the time the blotter was last modified.
		/// </summary>
		public DateTime ModifiedTime
		{
			get
			{
				return this.modifiedTimeField;
			}
			set
			{
				if (this.modifiedTimeField != value)
				{
					this.modifiedTimeField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("ModifiedTime"));
				}
			}
		}

		/// <summary>
		/// Collect the metadata from the data model.
		/// </summary>
		/// <param name="state">The generic thread start parameter.</param>
		void CalculateMetadata()
		{

			// Copy the values from the data model into the metadata.
			DataModel.EntityRow entityRow = DataModel.Entity.EntityKey.Find(this.EntityId);
			this.CreatedTime = entityRow.CreatedTime;
			this.ModifiedTime = entityRow.ModifiedTime;

		}

		/// <summary>
		/// Copies the input stream to the output stream.
		/// </summary>
		/// <param name="inputStream">The source input stream.</param>
		/// <param name="outputStream">The target output stream.</param>
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
		/// Initializes the metadata key/value pairs.
		/// </summary>
		void InitializeMetadata()
		{

			// This will create elements for all the metadata key/value pairs that is displayed in the DetailsBar.
			this.metadata.Add(DetailBar.CreateMetadataElement("Date modified:", this, DetailBar.CreateDateTimeMetadataValue("ModifiedTime")));

		}

		/// <summary>
		/// Invoked when an unhandled DragDrop.DragEnter attached event reaches an element in its route that is derived from this class.
		/// </summary>
		/// <param name="dragEventArgs">The DragEventArgs that contains the event data.</param>
		protected override void OnDragEnter(DragEventArgs dragEventArgs)
		{

			// This will find an icon resource name for the kind of object that is being dragged.  The general idea is that we want to construct an image of the
			// object being dragged as a visual cue.
			String iconString = String.Empty;
			String[] paths = dragEventArgs.Data.GetData("FileDrop") as String[];
			if (paths != null)
				foreach (String path in paths)
				{
					DocumentProperty dropObjectAttribute;
					DirectoryPage.documentProperties.TryGetValue(Path.GetExtension(path).ToLower(), out dropObjectAttribute);
					DataModel.ImageRow imageRow = DataModel.Type.TypeKey.Find(dropObjectAttribute.typeId).ImageRow;
					if (imageRow != null)
						iconString = imageRow.Image;
				}

			// This is the image that will be placed in the DragWindow as it moves around with the cursor.
			Image image = new Image();

			// If we found a icon in the resources for the kind of object being dragged, then we'll load it into memory and take the extra large version of the icon
			// (there are usually several image sizes in a single icon).
			if (!String.IsNullOrEmpty(iconString))
			{
				MemoryStream iconStream = new MemoryStream(Convert.FromBase64String(iconString));
				Dictionary<ImageSize, ImageSource> images = ImageHelper.DecodeIcon(iconStream.ToArray());
				image.Source = images[ImageSize.ExtraLarge];
			}

			// This will calculate the position of the cursor with respect to the screen.  The Drag and Drop icon is a floating window.  When we set the coordinates
			// of the Drag-and-Drop icon, it will need to be in screen units also.
			Point cursorPosition = this.PointToScreen(dragEventArgs.GetPosition(this));

			// This will create the actual window that hold the image if the item being dragged.
			this.dragWindow = new DragWindow();
			this.dragWindow.Content = image;
			this.SetDragWindowPosition(cursorPosition);
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
					if (!DirectoryPage.documentProperties.ContainsKey(Path.GetExtension(path).ToLower()))
						canDrop = false;
			}

			// At the point we get this message, we've already created a DragWindow holding an image of the item being dragged.  This will move the window so that
			// it always appears to be underneath the cursor.
			this.SetDragWindowPosition(this.PointToScreen(dragEventArgs.GetPosition(this)));

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

		}

		/// <summary>
		/// Handles a change to a Entity row.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="entityRowChangeEventArgs">The event arguments.</param>
		void OnEntityRowChanged(Object sender, DataModel.EntityRowChangeEventArgs entityRowChangeEventArgs)
		{

			// We're only interested in changes that affect this Entity.
			if (entityRowChangeEventArgs.Action == DataRowAction.Change)
			{
				DataModel.EntityRow entityRow = entityRowChangeEventArgs.Row;
				if (entityRow.EntityId == this.EntityId)
				{
					this.CreatedTime = entityRow.CreatedTime;
					this.ModifiedTime = entityRow.ModifiedTime;
				}
			}

		}

		/// <summary>
		/// Occurs when the element is laid out, rendered, and ready for interaction.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="routedEventArgs">The event data.</param>
		void OnLoaded(Object sender, RoutedEventArgs routedEventArgs)
		{

			// This provides a data context for the blotter.
			AssetNetworkItem assetNetworkItem = this.DataContext as AssetNetworkItem;
			this.entityIdField = assetNetworkItem.EntityId;

			// Link the page into the data model.
			DataModel.Entity.EntityRowChanged += this.OnEntityRowChanged;

			// This will calculate the initial values for the displayed metadata.
			this.CalculateMetadata();

			// This will send a message up to the frame that there is a collection of metadata available.  This will be bound to the DetailBar by the frame and the
			// items will appear hosted in the Detail bar.  These items in the DetailBar are not copies but the elements created above.  Any change to the items
			// above will be reflected immediately in the DetailBar.
			this.RaiseEvent(new ItemsSourceEventArgs(ExplorerFrame.DetailBarChangedEvent, this.metadata));

		}

		/// <summary>
		/// Occurs when a property has changed.
		/// </summary>
		/// <param name="propertyChangedEventArgs">The event data.</param>
		public void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
		{

			// This will notify anyone listening that the property has changed.
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, propertyChangedEventArgs);

		}

		/// <summary>
		/// Occurs when the element is removed from within an element tree of loaded elements.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnUnloaded(Object sender, RoutedEventArgs e)
		{

			// Unhook this page from the data model when we're unloaded.
			DataModel.Entity.EntityRowChanged -= this.OnEntityRowChanged;

		}

		/// <summary>
		/// Sets the position of the drag window based on the location of the cursor.
		/// </summary>
		void SetDragWindowPosition(Point cursorPosition)
		{

			// This will position the drag image so the lower middle part of the image is under the cursor hot spot.  This is reverse engineered from Windows
			// Explorer.
			this.dragWindow.Left = cursorPosition.X - this.dragWindow.Width / 2.0;
			this.dragWindow.Top = cursorPosition.Y - this.dragWindow.Height + 10.0;

		}

	}

}
