namespace Teraque.Tools
{

	using Microsoft.Win32;
	using System;
	using System.IO;
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;

	/// <summary>
	/// A utility used to read bitmaps from files and serialize them.
	/// </summary>
	public partial class MainWindow : Window
	{

		/// <summary>
		/// Initialize a new instance of the Teraque.Tools.MainWindow class.
		/// </summary>
		public MainWindow()
		{

			// The IDE maintained resources are initialized here.
			InitializeComponent();

			this.TextBox.TextChanged += this.OnTextBoxTextChanged;

		}

		void OnTextBoxTextChanged(Object sender, TextChangedEventArgs e)
		{

			Boolean isSuccessful = false;

			// This will extract the various BitmapFrames from the icon and select the best one for the different image sizes.
			try
			{
				using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(this.TextBox.Text)))
				{

					// An icon contains several images in various sizes and colors.  This will sort the images by the pixel size and select the image with the greatest
					// color depth.  Note for some reason known only to the engineers at Microsoft, the original color depth of the image can only be extracted from
					// the Thumbnail.  They tried to explain why this is so but the reason was illogical and I couldn't repeat it.
					IconBitmapDecoder iconBitmapDecoder = new IconBitmapDecoder(memoryStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
					SortedDictionary<Int32, BitmapFrame> availableFrames = new SortedDictionary<Int32, BitmapFrame>();
					Int32 maxResolution = Int32.MinValue;
					foreach (BitmapFrame bitmapFrame in iconBitmapDecoder.Frames)
					{
						BitmapFrame currentFrame;
						if (availableFrames.TryGetValue(bitmapFrame.PixelHeight, out currentFrame))
							if (currentFrame.Thumbnail.Format.BitsPerPixel >= bitmapFrame.Thumbnail.Format.BitsPerPixel)
								continue;
						availableFrames[bitmapFrame.PixelHeight] = bitmapFrame;
						maxResolution = Math.Max(maxResolution, bitmapFrame.PixelHeight);
					}

					// This will selecte the largest frame for the image.
					this.Image.Source = availableFrames[maxResolution];

					isSuccessful = true;

				}
			}
			catch (NotSupportedException) { }
			catch (FileFormatException) { }

			if (!isSuccessful)
			{

				try
				{

					using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(this.TextBox.Text)))
					{
						BitmapDecoder bitmapDecoder = BitmapDecoder.Create(memoryStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
						this.Image.Source = bitmapDecoder.Frames[0];
					}

				}
				catch (NotSupportedException) { }
				catch (FileFormatException) { }
				catch (Exception exception)
				{

					MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK);

				}
			}

		}

		/// <summary>
		/// Copies the input stream to the output stream.
		/// </summary>
		/// <param name="inputStream"></param>
		/// <param name="outputStream"></param>
		static void Copy(Stream inputStream, Stream outputStream)
		{

			// Move chunks of memory from the input to the output stream until they've all been moved.
			Int32 bufferLength = 0x500;
			Byte[] buffer = new Byte[bufferLength];
			Int32 bytesRead = inputStream.Read(buffer, 0, bufferLength);
			while (bytesRead > 0)
			{
				outputStream.Write(buffer, 0, bytesRead);
				bytesRead = inputStream.Read(buffer, 0, bufferLength);
			}

		}

		/// <summary>
		/// Handles a request to open up a file.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="executedRoutedEventArgs">The event data.</param>
		private void OnOpen(Object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
		{

			// Present the dialog that allows the user to select a bitmap.
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image Files (*.ICO, *.PNG)|*.ico;*.png";
			if (openFileDialog.ShowDialog() == true)
			{

				// If the user selects a bitmap then open it up, convert it into a string and copy it to the clipboard.
				using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
				{
					MemoryStream memoryStream = new MemoryStream();
					MainWindow.Copy(fileStream, memoryStream);
					this.TextBox.Text = Convert.ToBase64String(memoryStream.GetBuffer());
					this.TextBox.Focus();
					this.TextBox.SelectAll();
					this.TextBox.Copy();
				}

			}

		}

	}

}
