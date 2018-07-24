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
	public class License : ExplorerItem
	{

		/// <summary>
		/// Initializes a new instance of a CategoryNode object.
		/// </summary>
		/// <param name="licenseRow"></param>
		public License(IExplorerItem parent, DataSet.LicenseRow licenseRow)
		{

			// Validate the parameters.
			if (licenseRow == null)
				throw new ArgumentNullException("licenseRow");

			// Extract the values from the record.
			base.Name = licenseRow.SerialNumber.ToString();
			base.Parent = parent;

			// This will locat and disassemble the folder icon and give us the basic image sizes supported by the application framework.
			Uri uri = new Uri("/Teraque.DataModel;component/Resources/License.ico", UriKind.Relative);
			Dictionary<ImageSize, ImageSource> images = ImageHelper.DecodeIcon(uri);
			base.SmallImageSource = images[ImageSize.Small];
			base.MediumImageSource = images[ImageSize.Medium];
			base.LargeImageSource = images[ImageSize.Large];
			base.ExtraLargeImageSource = images[ImageSize.ExtraLarge];

		}

	}

}
