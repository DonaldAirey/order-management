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
	public class Customer : ExplorerItem
	{

		/// <summary>
		/// Initializes a new instance of a CategoryNode object.
		/// </summary>
		/// <param name="customerRow"></param>
		public Customer(IExplorerItem parent, DataSet.CustomerRow customerRow)
		{

			// Validate the parameters.
			if (customerRow == null)
				throw new ArgumentNullException("customerRow");

			// Extract the values from the record.
			String name = String.Format("{0}, {1}", customerRow.LastName, customerRow.FirstName);
			base.Name = name;
			base.Parent = parent;

			// This will locat and disassemble the folder icon and give us the basic image sizes supported by the application framework.
			Uri uri = new Uri("/Teraque.DataModel;component/Resources/Customer.ico", UriKind.Relative);
			Dictionary<ImageSize, ImageSource> images = ImageHelper.DecodeIcon(uri);
			base.SmallImageSource = images[ImageSize.Small];
			base.MediumImageSource = images[ImageSize.Medium];
			base.LargeImageSource = images[ImageSize.Large];
			base.ExtraLargeImageSource = images[ImageSize.ExtraLarge];

			this.Viewer = new Uri(String.Format(@"pack://application:,,,/Teraque.LicenseBook;component/LicenseDirectory.xaml?path=\Root\Product\{0}", name));

			foreach (DataSet.LicenseRow licenseRow in customerRow.GetLicenseRows())
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
