namespace Teraque.Windows
{

	using System;
	using System.Collections.Generic;
	using System.Windows;
    using System.Windows.Media;
	using System.Windows.Media.Imaging;

	/// <summary>
	/// A string that can provide a source for a bitmap in XAML markup.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class IconSource
	{

		/// <summary>
		/// A library of image sources.
		/// </summary>
		Dictionary<ImageSize, ImageSource> images;

		/// <summary>
		/// Initialize a new instance of the IconSource class.
		/// </summary>
		/// <param name="source">A string encoded version of the icon.</param>
		public IconSource(String source)
		{

			// This will initialize the object with a library of images built from the decoded icon image.
			this.images = ImageHelper.DecodeIcon(Convert.FromBase64String(source));

		}

		/// <summary>
		/// Gets the extra large image source.
		/// </summary>
		public ImageSource ExtraLargeImage
		{
			get
			{
				return this.images[ImageSize.ExtraLarge];
			}
		}

		/// <summary>
		/// Gets the large image source.
		/// </summary>
		public ImageSource LargeImage
		{
			get
			{
				return this.images[ImageSize.Large];
			}
		}

		/// <summary>
		/// Gets the medium image source.
		/// </summary>
		public ImageSource MediumImage
		{
			get
			{
				return this.images[ImageSize.Medium];
			}
		}

		/// <summary>
		/// Gets the small image source.
		/// </summary>
		public ImageSource SmallImage
		{
			get
			{
				return this.images[ImageSize.Small];
			}
		}

	}

}
