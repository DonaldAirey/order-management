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
	/// The basic unit of data for the categories.
	/// </summary>
    public class Product : ExplorerItem
	{

		/// <summary>
		/// Initializes a new instance of a CategoryNode object.
		/// </summary>
		/// <param name="productRow"></param>
		public Product(IExplorerItem parent, DataSet.ProductRow productRow)
		{

            // Validate the parameters.
            if (productRow == null)
                throw new ArgumentNullException("productRow");

			// Extract the values from the record.
			base.Name = productRow.Name;
			base.Parent = parent;

			// This will locat and disassemble the folder icon and give us the basic image sizes supported by the application framework.
			Uri uri = new Uri("/Teraque.DataModel;component/Resources/Product.ico", UriKind.Relative);
			Dictionary<ImageSize, ImageSource> images = ImageHelper.DecodeIcon(uri);
			base.SmallImageSource = images[ImageSize.Small];
			base.MediumImageSource = images[ImageSize.Medium];
			base.LargeImageSource = images[ImageSize.Large];
			base.ExtraLargeImageSource = images[ImageSize.ExtraLarge];

			this.Viewer = new Uri(String.Format(@"pack://application:,,,/Teraque.LicenseBook;component/LicenseDirectory.xaml?path=\Root\Product\{0}", productRow.Name));

			foreach (DataSet.LicenseRow licenseRow in productRow.GetLicenseRows())
				this.Add(new License(this, licenseRow));
		
		}

		public override bool IsExpandable
		{
			get
			{
				return false;
			}
		}

	}

}
