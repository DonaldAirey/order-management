namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Windows.Media;
	using Teraque.Windows;

	/// <summary>
	/// A hierarchical view of the data displayed in an Explorer application.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public class AssetNetworkItem : ExplorerItem
	{

		/// <summary>
		/// The data associated with this item.
		/// </summary>
		Byte[] dataField;

		/// <summary>
		/// The unique identifier for this item.
		/// </summary>
		Guid entityIdField;

		/// <summary>
		/// The unique identifier for this item's type.
		/// </summary>
		Guid typeIdField;

		/// <summary>
		/// Initialize a new instance of the AssetNetworkItem class.
		/// </summary>
		internal AssetNetworkItem() { }

		/// <summary>
		/// Initialize a new instance of the AssetNetworkItem class.
		/// </summary>
		/// <param name="owner">The owner collection.</param>
		/// <param name="entityRow">The record in the data model that contains the information for this item.</param>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		internal AssetNetworkItem(AssetNetworkCollection owner, DataModel.EntityRow entityRow)
		{

			// The new instance is populated with data from the data model.
			this.Copy(owner, entityRow);

		}

		/// <summary>
		/// The data of this item.
		/// </summary>
		public override Byte[] Data
		{
			get
			{
				return this.dataField;
			}
			set
			{
				if (this.dataField != value)
				{
					this.dataField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Data"));
				}
			}
		}

		/// <summary>
		/// Gets the unique identifier for this item.
		/// </summary>
		public Guid EntityId
		{
			get
			{
				return this.entityIdField;
			}
			set
			{
				if (this.entityIdField != value)
				{
					this.entityIdField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("EntityId"));
				}
			}
		}

		/// <summary>
		/// The application level type of this object.
		/// </summary>
		public virtual Guid TypeId
		{
			get
			{
				return this.typeIdField;
			}
			set
			{
				if (this.typeIdField != value)
				{
					this.typeIdField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("TypeId"));
				}
			}
		}

		/// <summary>
		/// Copies the information from the data model into the item.
		/// </summary>
		/// <param name="owner">The owner collection.</param>
		/// <param name="entityRow">The record in the data model that contains the information for this item.</param>
		protected void Copy(AssetNetworkCollection owner, DataModel.EntityRow entityRow)
		{

			// Validate the parameters
			if (owner == null)
				throw new ArgumentNullException("owner");
			if (entityRow == null)
				throw new ArgumentNullException("entityRow");

			// Copy the basic information out of the data model.  These statements will also cause the NotifyPropertyChange event to fire.
			this.DateCreated = entityRow.CreatedTime;
			this.DateModified = entityRow.ModifiedTime;
			this.EntityId = entityRow.EntityId;
			this.Name = entityRow.Name;
			this.TypeDescription = entityRow.TypeRow.Description;
			this.TypeId = entityRow.TypeId;

			// Create properties from the metadata associated with this entity.
			foreach (DataModel.PropertyStoreRow propertyStoreRow in entityRow.GetPropertyStoreRows())
			{

				// Copy the viewer property.
				if (propertyStoreRow.PropertyId == PropertyId.Viewer)
					this.Viewer = new Uri(Encoding.Unicode.GetString(propertyStoreRow.Value as Byte[]), UriKind.RelativeOrAbsolute);

				// Copy the data property.
				if (propertyStoreRow.PropertyId == PropertyId.Data)
					this.Data = propertyStoreRow.Value as Byte[];

			}

			// This will disassemble the icon and give us the basic image sizes supported by the application framework.
			Dictionary<ImageSize, ImageSource> images = ImageHelper.DecodeIcon(Convert.FromBase64String(entityRow.ImageRow.Image));
			this.SmallImageSource = images[ImageSize.Small];
			this.MediumImageSource = images[ImageSize.Medium];
			this.LargeImageSource = images[ImageSize.Large];
			this.ExtraLargeImageSource = images[ImageSize.ExtraLarge];

			// The next part of the copy operation involves recursing into the children and copying, moving or removing the items to reflect the current hierarchy. 
			// This query is the meat of recursing into the hierarchy.  This will create a sorted array of all the child items of this node.
			var children = from entityTreeItem in entityRow.GetEntityTreeRowsByFK_Entity_EntityTree_ParentId()
						   where entityTreeItem.EntityRowByFK_Entity_EntityTree_ChildId.IsContainer == true
						   orderby entityTreeItem.EntityRowByFK_Entity_EntityTree_ChildId.Name
						   select entityTreeItem.EntityRowByFK_Entity_EntityTree_ChildId;
			DataModel.EntityRow[] childArray = children.ToArray<DataModel.EntityRow>();

			// This list is part of a MVVM.  Since this collection is designed to be bound to the user interface, it is important not to wipe it clean and rebuild 
			// it each time something changes.  The main idea of this algorithm is to find out what children are new, what children need to be updated and what
			// children need to be deleted without disturbing the other children.
			Int32 sourceIndex = 0;
			Int32 targetIndex = 0;
			while (targetIndex < this.Children.Count && sourceIndex < childArray.Length)
			{

				// The names of the node (within the scope of their parent node) is unique, just like a file system.  If the list of new children doesn't match up
				// with the list provided by the data model, then we will insert, update or delete the list in order to reconcile the differences.
				AssetNetworkItem targetItem = this.Children[targetIndex] as AssetNetworkItem;
				DataModel.EntityRow childRow = childArray[sourceIndex];

				// Items no longer in the data model are deleted from the list.  Items that are already in the list have their contents copied (including children)
				// and new items are created (including their children) and added to the list.
				switch (String.Compare(targetItem.Name, childRow.Name, true, CultureInfo.CurrentCulture))
				{

				case -1:

					// Remove children no longer in the data model.
					AssetNetworkItem removedItem = this.Children[targetIndex] as AssetNetworkItem;
					owner.RemoveDescendant(removedItem);
					this.Children.Remove(removedItem);
					break;

				case 0:

					// Copy items that are already in the list.
					targetItem.Copy(owner, childRow);
					sourceIndex++;
					targetIndex++;
					break;

				case 1:

					// Add new items (and their children) when they aren't in the list.
					AssetNetworkItem addedItem = new AssetNetworkItem(owner, childRow);
					owner.AddDescendant(addedItem);
					this.Children.Insert(targetIndex, addedItem);
					targetIndex++;
					sourceIndex++;
					break;

				}

			}

			// This covers the case where there were several children added to the list after the last item in the existing list.  In this situation there is
			// nothing left to reconcile, just a bunch of new items to be concatenated to the current list of children.
			while (sourceIndex < childArray.Length)
			{
				AssetNetworkItem assetNetworkItem = new AssetNetworkItem(owner, childArray[sourceIndex]);
				owner.AddDescendant(assetNetworkItem);
				this.Children.Add(assetNetworkItem);
				sourceIndex++;
			}

			// The next part of the copy operation involves recursing into the children and copying, moving or removing the items to reflect the current hierarchy. 
			// This query is the meat of recursing into the hierarchy.  This will create a sorted array of all the child items of this node.
			var leaves = from entityTreeItem in entityRow.GetEntityTreeRowsByFK_Entity_EntityTree_ParentId()
						 where entityTreeItem.EntityRowByFK_Entity_EntityTree_ChildId.IsContainer == false
						 orderby entityTreeItem.EntityRowByFK_Entity_EntityTree_ChildId.Name
						 select entityTreeItem.EntityRowByFK_Entity_EntityTree_ChildId;
			DataModel.EntityRow[] leavesArray = leaves.ToArray<DataModel.EntityRow>();

			// This list is part of a MVVM.  Since this collection is designed to be bound to the user interface, it is important not to wipe it clean and rebuild 
			// it each time something changes.  The main idea of this algorithm is to find out what leaves are new, what leaves need to be updated and what
			// leaves need to be deleted without disturbing the other leaves.
			Int32 sourceLeavesIndex = 0;
			Int32 targetLeavesIndex = 0;
			while (targetLeavesIndex < this.Leaves.Count && sourceLeavesIndex < leavesArray.Length)
			{

				// The names of the node (within the scope of their parent node) is unique, just like a file system.  If the list of new leaves doesn't match up
				// with the list provided by the data model, then we will insert, update or delete the list in order to reconcile the differences.
				AssetNetworkItem targetItem = this.Leaves[targetLeavesIndex] as AssetNetworkItem;
				DataModel.EntityRow leafRow = leavesArray[sourceLeavesIndex];

				// Items no longer in the data model are deleted from the list.  Items that are already in the list have their contents copied (including leaves)
				// and new items are created (including their leaves) and added to the list.
				switch (String.Compare(targetItem.Name, leafRow.Name, true, CultureInfo.CurrentCulture))
				{

				case -1:

					// Remove leaves no longer in the data model.
					AssetNetworkItem removedItem = this.Leaves[targetLeavesIndex] as AssetNetworkItem;
					owner.RemoveDescendant(removedItem);
					this.Leaves.Remove(removedItem);
					break;

				case 0:

					// Copy items that are already in the list.
					targetItem.Copy(owner, leafRow);
					sourceLeavesIndex++;
					targetLeavesIndex++;
					break;

				case 1:

					// Add new items (and their leaves) when they aren't in the list.
					AssetNetworkItem addedItem = new AssetNetworkItem(owner, leafRow);
					owner.AddDescendant(addedItem);
					this.Leaves.Insert(targetLeavesIndex, addedItem);
					targetLeavesIndex++;
					sourceLeavesIndex++;
					break;

				}

			}

			// This covers the case where there were several leaves added to the list after the last item in the existing list.  In this situation there is
			// nothing left to reconcile, just a bunch of new items to be concatenated to the current list of leaves.
			while (sourceLeavesIndex < leavesArray.Length)
			{
				AssetNetworkItem assetNetworkItem = new AssetNetworkItem(owner, leavesArray[sourceLeavesIndex]);
				owner.AddDescendant(assetNetworkItem);
				this.Leaves.Add(assetNetworkItem);
				sourceLeavesIndex++;
			}

		}

		/// <summary>
		/// Returns a String that represents the current Object.
		/// </summary>
		/// <returns>A String that represents the current Object.</returns>
		public override String ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "AssetNetworkItem {0}: Count: {1}", this.Name, this.Children.Count);
		}

	}

}
