namespace Teraque
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Media;
	using Teraque.Windows;

	/// <summary>
	/// The basic unit of data for the categories.
	/// </summary>
    public class CategoryCollection : ExplorerItem
	{

		/// <summary>
		/// Identifies the object.
		/// </summary>
		Guid rootIdField;

		/// <summary>
		/// The internal type of this node.
		/// </summary>
		Guid typeIdField;

		/// <summary>
		/// Initializes a new instance of a CategoryCollection class.
		/// </summary>
		public CategoryCollection()
		{

			// Extract the values from the record.
			this.rootIdField = Guid.NewGuid();
			this.Name = String.Empty;
			this.Parent = null;
			this.typeIdField = TypeId.FileFolder;
			this.TypeDescription = String.Empty;

		}

		/// <summary>
		/// Initializes a new instance of a CategoryCollection class.
		/// </summary>
		/// <param name="parent">The parent of the category node.</param>
		/// <param name="rootRow">The data model record that provides a context for this Model View.</param>
		public CategoryCollection(CategoryCollection parent, DataSet.RootRow rootRow)
		{

            // Validate the parameters.
            if (rootRow == null)
                throw new ArgumentNullException("rootRow");

			// Extract the values from the record.
			this.Name = rootRow.Name;
			this.Parent = parent;
			this.rootIdField = rootRow.RootId;
			this.typeIdField = rootRow.TypeId;
			this.TypeDescription = rootRow.TypeRow.Name;

			// This will disassemble the icon and give us the basic image sizes supported by the application framework.
			Dictionary<ImageSize, ImageSource> images = ImageHelper.DecodeIcon(Convert.FromBase64String(rootRow.Image));
			this.SmallImageSource = images[ImageSize.Small];
			this.MediumImageSource = images[ImageSize.Medium];
			this.LargeImageSource = images[ImageSize.Large];
			this.ExtraLargeImageSource = images[ImageSize.ExtraLarge];

			// These are the metadata properties of the item.
			foreach (DataSet.PropertyStoreRow propertyStoreRow in rootRow.GetPropertyStoreRows())
				this.PropertyStore.Add(propertyStoreRow.PropertyId, propertyStoreRow.Value);

			// This will dig into the relations and recursively construct a hierarchy of items.
			this.ExpandTree(rootRow);

        }

		/// <summary>
		/// Gets or sets the data for this object.
		/// </summary>
		public override Byte[] Data
		{
			get
			{
				Object dataString;
				if (this.PropertyStore.TryGetValue(PropertyId.Data, out dataString))
					return Convert.FromBase64String(dataString as String);
				return null;
			}
			set
			{
				this.PropertyStore[PropertyId.Data] = Convert.ToBase64String(value);	
			}
		}
		
		/// <summary>
		/// Gets the date and time the object was created.
		/// </summary>
		public override DateTime DateCreated
		{
			get
			{
				return (DateTime)this.PropertyStore[PropertyId.DateCreated];
			}
			set
			{
				this.PropertyStore[PropertyId.DateCreated] = value;
			}
		}

		/// <summary>
		/// Gets the date and time the object was modified.
		/// </summary>
		public override DateTime DateModified
		{
			get
			{
				return (DateTime)this.PropertyStore[PropertyId.DateModified];
			}
			set
			{
				this.PropertyStore[PropertyId.DateModified] = value;
			}
		}

		/// <summary>
		/// Gets whether the item can hold other items.
		/// </summary>
		public override Boolean IsContainer
		{
			get
			{
				return this.typeIdField == TypeId.FileFolder;
			}
		}

		/// <summary>
		/// Gets the unique identifier of this row.
		/// </summary>
		public Guid RootId
		{
			get
			{
				return this.rootIdField;
			}
		}

		/// <summary>
		/// Gets the size of this object on disk.
		/// </summary>
		public override Nullable<Int64> Size
		{
			get
			{
				Byte[] data = this.Data;
				if (data != null)
					return data.Length;
				return null;
			}
		}

		/// <summary>
		/// Gets the viewer used to display this object.
		/// </summary>
		public override Uri Viewer
		{
			get
			{
				Object uri;
				this.PropertyStore.TryGetValue(PropertyId.Viewer, out uri);
				return uri as Uri;
			}
			set
			{
				this.PropertyStore[PropertyId.Viewer] = value;
			}
		}

		/// <summary>
		/// Expands the tree using the given data model record.
		/// </summary>
		/// <param name="currentRootRow">The data model used to provide a context for the model view.</param>
		void ExpandTree(DataSet.RootRow currentRootRow)
		{
			foreach (DataSet.TreeRow treeRow in currentRootRow.GetTreeRowsByFK_Root_Tree_RootIdParentId())
			{
				DataSet.RootRow rootRow = treeRow.RootRowByFK_Root_Tree_RootIdChildId;

				if (rootRow.TypeId == TypeId.FileFolder)
					this.Children.Add(new CategoryCollection(this, rootRow));
				else
					this.Leaves.Add(new CategoryCollection(this, rootRow));

			}
		}

	}

}
