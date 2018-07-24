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

		}

		/// <summary>
		/// Copies the input stream to the output stream.
		/// </summary>
		/// <param name="inputStream"></param>
		/// <param name="outputStream"></param>
		static void Copy(Stream inputStream, Stream outputStream)
		{

			// Move chunks of memory from the input to the output stream until they've all been moved.
			Int32 bufferLength = 0x1000;
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
			openFileDialog.Filter = "Asset Network Files|*.xlsx;*.pdf";
			if (openFileDialog.ShowDialog() == true)
			{

				// If the user selects a bitmap then open it up, convert it into a string and copy it to the clipboard.
				using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
				{
					MemoryStream memoryStream = new MemoryStream();
					MainWindow.Copy(fileStream, memoryStream);
					this.TextBox.Text = Convert.ToBase64String(memoryStream.GetBuffer(), 0, Convert.ToInt32(memoryStream.Length));
					this.TextBox.Focus();
					this.TextBox.SelectAll();
					this.TextBox.Copy();
				}

			}

		}

	}

}
