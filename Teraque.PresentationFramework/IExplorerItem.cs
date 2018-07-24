namespace Teraque.Windows
{

	using System;
	using System.ComponentModel;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Windows;
	using System.Windows.Media;

	/// <summary>
	/// Represents a set of properties and methods common to all navigatable objects in the Teraque.ExplorerWindow.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public interface IExplorerItem : INotifyPropertyChanged
	{

		/// <summary>
		/// The large icon of the item.
		/// </summary>
		ObservableCollection<IExplorerItem> Children { get; }

		/// <summary>
		/// The item's data.
		/// </summary>
		Byte[] Data { get; set; }

		/// <summary>
		/// The date created.
		/// </summary>
		DateTime DateCreated { get; }

		/// <summary>
		/// The date modified.
		/// </summary>
		DateTime DateModified { get; }

		/// <summary>
		/// The extra large icon of the item.
		/// </summary>
		ImageSource ExtraLargeImageSource { get; }

		/// <summary>
		/// Gets whether the item can hold other items.
		/// </summary>
		Boolean IsContainer { get; }

		/// <summary>
		/// Gets whether the item can be expanded to show the children.
		/// </summary>
		Boolean IsExpandable { get; }

		/// <summary>
		/// Gets whether the item's children are displayed.
		/// </summary>
		Boolean IsExpanded { get; set; }

		/// <summary>
		/// Gets or sets whether the item is the root of the hierarchy.
		/// </summary>
		Boolean IsRoot { get; }

		/// <summary>
		/// Gets or sets whether the item is selected.
		/// </summary>
		Boolean IsSelected { get; set; }

		/// <summary>
		/// The large icon of the item.
		/// </summary>
		ObservableCollection<IExplorerItem> Leaves { get; }

		/// <summary>
		/// The large icon of the item.
		/// </summary>
		ImageSource LargeImageSource { get; }

		/// <summary>
		/// The medium icon of the item.
		/// </summary>
		ImageSource MediumImageSource { get; }

		/// <summary>
		/// All extra data specific to the item.
		/// </summary>
		Dictionary<Guid, Object> PropertyStore { get; }

		/// <summary>
		/// The name of the item.
		/// </summary>
		String Name { get; }

		/// <summary>
		/// The parent of the item.
		/// </summary>
		IExplorerItem Parent { get; set; }

		/// <summary>
		/// The size of the item.
		/// </summary>
		Nullable<Int64> Size { get; }

		/// <summary>
		/// The small icon of the item.
		/// </summary>
		ImageSource SmallImageSource { get; }

		/// <summary>
		/// Describes the type of object.
		/// </summary>
		String TypeDescription { get; }

		/// <summary>
		/// The Uri of a page used to display the item.
		/// </summary>
		Uri Viewer { get; }

	}

}
