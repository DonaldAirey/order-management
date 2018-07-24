namespace Teraque.Windows
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.IO;
	using System.Windows.Resources;
	using System.Windows;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;

	/// <summary>
	/// Various utilities that help with processing images.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public static class ImageHelper
	{

		/// <summary>
		/// The size, in pixels, of a 'Small' image.
		/// </summary>
		const Int32 smallImageSize = 16;

		/// <summary>
		/// The size, in pixels, of a 'Medium' image.
		/// </summary>
		const Int32 mediumImageSize = 32;

		/// <summary>
		/// The size, in pixels, of a 'Large' image.
		/// </summary>
		const Int32 largeImageSize = 48;

		/// <summary>
		/// The size, in pixels, of an 'ExtraLarge' image.
		/// </summary>
		const Int32 extraLargeImageSize = 256;

		/// <summary>
		/// Disassembles an icon and selects the most appropriate image available for the discrete ImageSizes supported by the application.
		/// </summary>
		/// <param name="iconSource">The binary source of the icon.</param>
		/// <returns>A dictionary of suitable images indexed by the ImageSize.</returns>
		public static Dictionary<ImageSize, ImageSource> DecodeIcon(Byte[] iconSource)
		{

			// The return from this operation is a dictionary indexed by the ImageSize.  The most appropriate image available is selected for the different image
			// sizes available.  That is, for a small image, it will select the ImageSource that has the given pixel size and the greatest number of colors from
			// the images stored in the icon.
			Dictionary<ImageSize, ImageSource> images = new Dictionary<ImageSize, ImageSource>();

			// This will extract the various BitmapFrames from the icon and select the best one for the different image sizes.
			using (MemoryStream memoryStream = new MemoryStream(iconSource))
            {

				// An icon contains several images in various sizes and colors.  This will sort the images by the pixel size and select the image with the greatest
				// color depth.  Note for some reason known only to the engineers at Microsoft, the original color depth of the image can only be extracted from
				// the Thumbnail.  They tried to explain why this is so but the reason was illogical and I couldn't repeat it.
				IconBitmapDecoder iconBitmapDecoder = new IconBitmapDecoder(memoryStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                SortedDictionary<Int32, BitmapFrame> availableFrames = new SortedDictionary<Int32, BitmapFrame>();
                foreach (BitmapFrame bitmapFrame in iconBitmapDecoder.Frames)
                {
                    BitmapFrame currentFrame;
                    if (availableFrames.TryGetValue(bitmapFrame.PixelHeight, out currentFrame))
                        if (currentFrame.Thumbnail.Format.BitsPerPixel >= bitmapFrame.Thumbnail.Format.BitsPerPixel)
                            continue;
                    availableFrames[bitmapFrame.PixelHeight] = bitmapFrame;
                }

				// This will sort the frames into easy-to-use buckets.
                foreach (KeyValuePair<Int32, BitmapFrame> keyValuePair in availableFrames)
                {
                    if (keyValuePair.Key <= ImageHelper.smallImageSize)
                        images[ImageSize.Small] = keyValuePair.Value;
                    if (keyValuePair.Key <= ImageHelper.mediumImageSize)
						images[ImageSize.Medium] = keyValuePair.Value;
                    if (keyValuePair.Key <= ImageHelper.largeImageSize)
						images[ImageSize.Large] = keyValuePair.Value;
                    if (keyValuePair.Key <= ImageHelper.extraLargeImageSize)
						images[ImageSize.ExtraLarge] = keyValuePair.Value;
                }

            }

			// Freeze the objects for a little performance boost.
			images[ImageSize.Small].Freeze();
			images[ImageSize.Medium].Freeze();
			images[ImageSize.Large].Freeze();
			images[ImageSize.ExtraLarge].Freeze();

			// This is a dictionary of the best images available in the icon indexed by the image size.
			return images;

		}

		/// <summary>
		/// Disassembles an icon specified by the Uri and selects the most appropriate image available for the discrete ImageSizes supported by the application.
		/// </summary>
		/// <param name="uri">The PackUri address of the Icon resource.</param>
		/// <returns>A dictionary of suitable images indexed by the ImageSize.</returns>
		public static Dictionary<ImageSize, ImageSource> DecodeIcon(Uri uri)
		{

			// This will read the contents of the given stream into a buffer and then decode the buffer.
			StreamResourceInfo streamResourceInfo = Application.GetResourceStream(uri);
			return DecodeIcon(Utilities.ReadStream(streamResourceInfo.Stream));

		}

	}

}
