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
	using System.Windows.Resources;
	using Teraque;
	using Teraque.Windows;

	/// <summary>
	/// The basic unit of data for the categories.
	/// </summary>
    public class RootCollection : ExplorerItem
	{

		/// <summary>
		/// Initializes a new instance of a CategoryNode object.
		/// </summary>
		/// <param name="parent">The parent of the category node.</param>
		/// <param name="rootRow">The data model record that provides a context for this Model View.</param>
		public RootCollection()
		{

			// Extract the values from the record.
			base.Name = "Root";

			// This will disassemble the icon and give us the basic image sizes supported by the application framework.
			Dictionary<ImageSize, ImageSource> images = ImageHelper.DecodeIcon(new Uri("/Teraque.DataModel;component/Resources/Root.ico", UriKind.Relative));
			base.SmallImageSource = images[ImageSize.Small];
			base.MediumImageSource = images[ImageSize.Medium];
			base.LargeImageSource = images[ImageSize.Large];
			base.ExtraLargeImageSource = images[ImageSize.ExtraLarge];

			base.Viewer = new Uri(@"pack://application:,,,/Teraque.LicenseBook;component/LicenseDirectory.xaml?path=\Root");

			FolderCollection productFolder = new FolderCollection(this, "Product");
			productFolder.Viewer = new Uri(@"pack://application:,,,/Teraque.LicenseBook;component/LicenseDirectory.xaml?path=\Root\Product");
			foreach (DataSet.ProductRow productRow in DataModel.Product)
				productFolder.Add(new Product(productFolder, productRow));
			this.Add(productFolder);

			FolderCollection customerFolder = new FolderCollection(this, "Customer");
			customerFolder.Viewer = new Uri(@"pack://application:,,,/Teraque.LicenseBook;component/LicenseDirectory.xaml?path=\Root\Customer");
			foreach (DataSet.CustomerRow customerRow in DataModel.Customer)
				customerFolder.Add(new Customer(customerFolder, customerRow));
			this.Add(customerFolder);

        }

	}

}
