namespace Teraque.LicenseGenerator
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.IO;
	using System.Resources;
	using System.Windows;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using Teraque;
	using Teraque.Windows;

	/// <summary>
	/// A generic collection of other nodes.
	/// </summary>
    public class FolderCollection : ExplorerItem
	{

		/// <summary>
		/// Initializes a new instance of a CategoryNode object.
		/// </summary>
		/// <param name="parent">The parent of the category node.</param>
		/// <param name="rootRow">The data model record that provides a context for this Model View.</param>
		public FolderCollection(RootCollection parent, String name)
		{

            // Validate the parameters.
            if (parent == null)
                throw new ArgumentNullException("parent");

			// Extract the values from the record.
			base.Name = name;
			base.Parent = parent;

			// This will locat and disassemble the folder icon and give us the basic image sizes supported by the application framework.
			Uri uri = new Uri("/Teraque.DataModel;component/Resources/Folder.ico", UriKind.Relative);
			Dictionary<ImageSize, ImageSource> images = ImageHelper.DecodeIcon(uri);
			base.SmallImageSource = images[ImageSize.Small];
			base.MediumImageSource = images[ImageSize.Medium];
			base.LargeImageSource = images[ImageSize.Large];
			base.ExtraLargeImageSource = images[ImageSize.ExtraLarge];

		}

	}

}
