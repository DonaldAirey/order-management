namespace Teraque.Windows
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.IO;
	using System.Windows;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Markup;

	/// <summary>
	/// An implmentation of the IExplorerItem as a node.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[ContentProperty("Children")]
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public class ExplorerItem : ObservableCollection<IExplorerItem>, IExplorerItem
	{

		/// <summary>
		/// The date the item was created.
		/// </summary>
		DateTime dateCreatedField;

		/// <summary>
		/// The date the item was last modified.
		/// </summary>
		DateTime dateModifiedField;

		/// <summary>
		/// The extra large image source for this item.
		/// </summary>
		ImageSource extraLargeImageSourceField;

		/// <summary>
		/// Indicates whether the node has expanded items or not.  Part of the Model View.
		/// </summary>
		Boolean isExpandedField;

		/// <summary>
		/// Indicates whether the node is selected or not.  Part of the Model View.
		/// </summary>
		Boolean isSelectedField;

		/// <summary>
		/// The large image source for this item.
		/// </summary>
		ImageSource largeImageSourceField;

		/// <summary>
		/// The leaf elements of this child.
		/// </summary>
		ObservableCollection<IExplorerItem> leavesField = new ObservableCollection<IExplorerItem>();

		/// <summary>
		/// The medium image source for this item.
		/// </summary>
		ImageSource mediumImageSourceField;

		/// <summary>
		/// The display name of the item.
		/// </summary>
		String nameField;

		/// <summary>
		/// A dictionary of metadata items associated with this object.
		/// </summary>
		Dictionary<Guid, Object> propertyStore = new Dictionary<Guid, Object>();

		/// <summary>
		/// The estimated size of the item.
		/// </summary>
		Nullable<Int64> sizeField;

		/// <summary>
		/// The small image source for this item.
		/// </summary>
		ImageSource smallImageSourceField;

		/// <summary>
		/// The description of the object's type.
		/// </summary>
		String typeDescriptionField;

		/// <summary>
		/// The address of the viewer used to display this item.
		/// </summary>
		Uri viewerField;

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public new event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Creates a new instance of the ExplorerItem class.
		/// </summary>
		public ExplorerItem()
		{

			// The leaves of this node need to reference the parent so we'll tag them with the parent node as they are added to the collection.
			this.leavesField.CollectionChanged += this.OnLeavesCollectionChanged;

		}

		/// <summary>
		/// Handles a change to the leaves collection.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event data.</param>
		void OnLeavesCollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
		{

			// This will set the parent of the item as it is added to the collection.  The parent item is critical for navigating upwards in the hiearchy.
			if (e.Action == NotifyCollectionChangedAction.Add)
				foreach (ExplorerItem explorerItem in e.NewItems)
					if (explorerItem != null)
						explorerItem.Parent = this;

		}

		/// <summary>
		/// Raises the PropertyChanged event with the provided arguments.
		/// </summary>
		/// <param name="propertyChangedEventArgs">Arguments of the event being raised.</param>
		protected new virtual void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
		{

			// Notify any listeners that the proeprty has changed.
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, propertyChangedEventArgs);

		}

		/// <summary>
		/// The children of this node.
		/// </summary>
		public virtual ObservableCollection<IExplorerItem> Children
		{
			get
			{
				return this;
			}
		}

		/// <summary>
		/// Gets the data associated with this object.
		/// </summary>
		public virtual Byte[] Data
		{
			get
			{
				return null;
			}
			set { }
		}

		/// <summary>
		/// The date and time the object was created.
		/// </summary>
		public virtual DateTime DateCreated
		{
			get
			{
				return this.dateCreatedField;
			}
			set
			{
				if (this.dateCreatedField != value)
				{
					this.dateCreatedField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("DateCreated"));
				}
			}
		}

		/// <summary>
		/// The date and time the object was last modified.
		/// </summary>
		public virtual DateTime DateModified
		{
			get
			{
				return this.dateModifiedField;
			}
			set
			{
				if (this.dateModifiedField != value)
				{
					this.dateModifiedField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("DateModified"));
				}
			}
		}

		/// <summary>
		/// The extra large image source.
		/// </summary>
		public virtual ImageSource ExtraLargeImageSource
		{
			get
			{
				return this.extraLargeImageSourceField;
			}
			set
			{
				if (this.extraLargeImageSourceField != value)
				{
					this.extraLargeImageSourceField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("ExtraLargeImageSource"));
				}
			}
		}

		/// <summary>
		/// Gets whether the item can hold other items.
		/// </summary>
		public virtual Boolean IsContainer
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Indicates whether or not this node is expanded in a hierarchy.
		/// </summary>
		public virtual Boolean IsExpandable
		{
			get
			{
				return this.Children.Count != 0;
			}
		}

		/// <summary>
		/// Indicates whether or not this node is expanded in a hierarchy.
		/// </summary>
		public virtual Boolean IsExpanded
		{

			get
			{
				return this.isExpandedField;
			}

			set
			{

				// Notify subscribers when the property has changed.
				if (value != this.isExpandedField)
				{
					this.isExpandedField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("IsExpanded"));
				}

				// Expand all the way up to the root.
				if (this.isExpandedField && this.Parent != null)
					this.Parent.IsExpanded = true;

			}
		}

		/// <summary>
		/// Indicates whether or not this is the top of a hierarchy tree.
		/// </summary>
		public virtual Boolean IsRoot
		{
			get
			{
				return this.Parent == null;
			}
		}

		/// <summary>
		/// Indicates whether or not this node is selected.
		/// </summary>
		public virtual Boolean IsSelected
		{
			get
			{
				return this.isSelectedField;
			}
			set
			{
				if (value != this.isSelectedField)
				{
					this.isSelectedField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("IsSelected"));
				}
			}
		}

		/// <summary>
		/// The extra large image source.
		/// </summary>
		public virtual ImageSource LargeImageSource
		{
			get
			{
				return this.largeImageSourceField;
			}
			set
			{
				if (this.largeImageSourceField != value)
				{
					this.largeImageSourceField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("LargeImageSource"));
				}
			}
		}

		/// <summary>
		/// The Leaves of the node.
		/// </summary>
		public virtual ObservableCollection<IExplorerItem> Leaves
		{
			get
			{
				return this.leavesField;
			}
		}

		/// <summary>
		/// The medium image source.
		/// </summary>
		public virtual ImageSource MediumImageSource
		{
			get
			{
				return this.mediumImageSourceField;
			}
			set
			{
				if (this.mediumImageSourceField != value)
				{
					this.mediumImageSourceField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("MediumImageSource"));
				}
			}
		}

		/// <summary>
		/// The name of this object.
		/// </summary>
		public virtual String Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				if (this.nameField != value)
				{
					this.nameField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Name"));
				}
			}
		}

		/// <summary>
		/// The parent of this object.
		/// </summary>
		public virtual IExplorerItem Parent { get; set; }

		/// <summary>
		/// The metadata property store for this object.
		/// </summary>
		public Dictionary<Guid, Object> PropertyStore
		{
			get
			{
				return this.propertyStore;
			}
		}

		/// <summary>
		/// The extra large image source.
		/// </summary>
		public virtual Nullable<Int64> Size
		{
			get
			{
				return this.sizeField;
			}
			set
			{
				if (this.sizeField != value)
				{
					this.sizeField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Size"));
				}
			}
		}

		/// <summary>
		/// The Small Image Source.
		/// </summary>
		public virtual ImageSource SmallImageSource
		{
			get
			{
				return this.smallImageSourceField;
			}
			set
			{
				if (this.smallImageSourceField != value)
				{
					this.smallImageSourceField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("SmallImageSource"));
				}
			}
		}

		/// <summary>
		/// The application level type of this object.
		/// </summary>
		public virtual String TypeDescription
		{
			get
			{
				return this.typeDescriptionField;
			}
			set
			{
				if (this.typeDescriptionField != value)
				{
					this.typeDescriptionField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("TypeDescription"));
				}
			}
		}

		/// <summary>
		/// The viewer used to display this object.
		/// </summary>
		public virtual Uri Viewer
		{
			get
			{
				return this.viewerField;
			}
			set
			{
				if (this.viewerField != value)
				{
					this.viewerField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Viewer"));
				}
			}
		}

		/// <summary>
		/// Raises the CollectionChanged event with the provided arguments.
		/// </summary>
		/// <param name="e">Arguments of the event being raised.</param>
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{

			// Validate the parameter.
			if (e == null)
				throw new ArgumentNullException("e");

			// This will set the parent of the item as it is added to the collection.  The parent item is critical for navigating upwards in the hiearchy.
			if (e.Action == NotifyCollectionChangedAction.Add)
				foreach (ExplorerItem explorerItem in e.NewItems)
					if (explorerItem != null)
						explorerItem.Parent = this;

			// Allow the base collection to handle the rest of the change.
			base.OnCollectionChanged(e);

		}

		/// <summary>
		/// Returns a String that represents the current Object.
		/// </summary>
		/// <returns>A String that represents the current Object.</returns>
		public override String ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "ExplorerItem {0}: Count: {1}", this.Name, this.Count);
		}

	}

}
