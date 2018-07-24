namespace Teraque.Tools
{

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Xml.Linq;
	using System.Windows;
	using System.Xml;

	/// <summary>
	/// Interaction logic for WindowMain.xaml
	/// </summary>
	public partial class WindowMain : Window
	{

		/// <summary>
		/// Initializes a new instance of Teraque.Tools.WindowMain class.
		/// </summary>
		public WindowMain()
		{

			// The IDE managed resources are initialized here.
			InitializeComponent();

			// When a source file is passed to the program from the command line then there is no need to open up the user interface.  This will process the file
			// directly without prompting the user.
			String[] arguments = Environment.GetCommandLineArgs();
			if (arguments.Length > 1)
			{
				this.ScrubDocument(arguments[1]);
				Application.Current.Shutdown();
			}

		}

		/// <summary>
		/// Handles the clicking of the 'Cancel' button.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		private void OnCancelClick(object sender, RoutedEventArgs e)
		{

			// Finish without doing anything.
			this.Close();

		}

		/// <summary>
		/// Handles the clicking of the 'OK' button.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		private void OnOKClick(object sender, RoutedEventArgs e)
		{

			// This will scrub the source XML file provided by the user.
			this.ScrubDocument(this.FileNameTextBox.Text);

		}

		/// <summary>
		/// Sorts the attributes of an element and all it's children in alphabetical order.
		/// </summary>
		/// <param name="sourceNode">The original XElement.</param>
		/// <param name="targetElement">The target XElement where the newly sorted node is placed.</param>
		private void RecurseIntoDocument(XNode sourceNode, XContainer targetContainer)
		{

			// Convert any XElements found in the tree.
			if (sourceNode is XElement)
			{

				// Convert the generic node to a specific type.
				XElement sourceElement = sourceNode as XElement;

				// Arrange all the attributes in alphabetical order.
				SortedDictionary<String, XAttribute> sortedDictionary = new SortedDictionary<string, XAttribute>();
				foreach (XAttribute xAttribute in sourceElement.Attributes())
					sortedDictionary.Add(xAttribute.Name.LocalName, xAttribute);

				// Create a new target element from the source and remove the existing attributes.  They'll be replaced with the ordered list.
				XElement xElement = new XElement(sourceElement);
				xElement.RemoveAll();

				// Replace the attributes in alphabetical order.
				foreach (KeyValuePair<String, XAttribute> keyValuePair in sortedDictionary)
					xElement.Add(keyValuePair.Value);

				// Add it back to the target document.
				targetContainer.Add(xElement);

				// Recurse into each of the child elements.
				foreach (XNode childNode in sourceElement.Nodes())
					RecurseIntoDocument(childNode, xElement);

			}
			else
			{

				// Pass all other nodes into the output without alterations.
				targetContainer.Add(sourceNode);

			}

		}

		/// <summary>
		/// Orders and beautifies an XML document.
		/// </summary>
		/// <param name="path">The XML document to order and beautify.</param>
		private void ScrubDocument(String path)
		{

			try
			{

				// Open the source XML document from the path found in the dialog box.
				XDocument sourceDocument = XDocument.Load(path);

				// Create a new target document.  The output path will be constructed from the input path.
				XDocument targetDocument = new XDocument();

				// This will order all the attributes in the document in alphabetical order.  Note that the elements can't be similarly ordered because this could 
				// produce forward reference errors.
				RecurseIntoDocument(sourceDocument.Root, targetDocument);

				// Write the output to a temporary file.
				String temporaryPath = Path.GetTempFileName();

				// Save the original document in a safe place.
				String backupPath = Path.Combine(
					Path.GetDirectoryName(path),
					String.Format("{0}.{1}{2}", Path.GetFileNameWithoutExtension(path), "backup", Path.GetExtension(path)));

				// Beautify and save the target document when it has been ordered.  The output file takes it's name from the input file.  If the input file is 
				// 'window1.xml', then the output file will be named 'window1.formatted.xml'.
				XmlFormattedTextWriter xmlFormattedTextWriter = new XmlFormattedTextWriter(temporaryPath);
				xmlFormattedTextWriter.TabSize = Path.GetExtension(path).ToLower() == ".xaml" ? 4 : 2;
				XmlWriter xmlWriter = XmlWriter.Create(xmlFormattedTextWriter);
				targetDocument.WriteTo(xmlWriter);
				xmlWriter.Close();

				// Delete any existing backup file and move the original source to the new backup location.
				if (File.Exists(backupPath))
					File.Delete(backupPath);
				File.Move(path, backupPath);

				// Now move the temporary output to the original file location to make it look like the changes were made to the original file.
				File.Move(temporaryPath, path);

			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, Properties.Resources.XamlScrubberHeader);
			}

		}

	}

}
