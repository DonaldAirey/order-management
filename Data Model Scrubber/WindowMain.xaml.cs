namespace Teraque.Tools
{

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.IO;
	using System.Text;
	using System.Xml.Linq;
	using System.Windows;
	using System.Xml;
	using Teraque.Sandbox;

	/// <summary>
	/// Interaction logic for WindowMain.xaml
	/// </summary>
	public partial class WindowMain : Window
	{

		static XName nameName = XName.Get("Name", "http://tempuri.org/DataSet.xsd");

		static XName propertyName = XName.Get("Property", "http://tempuri.org/DataSet.xsd");

		static XName propertyIdName = XName.Get("PropertyId", "http://tempuri.org/DataSet.xsd");

		static XName propertyStoreName = XName.Get("PropertyStore", "http://tempuri.org/DataSet.xsd");

		static XName propertyStoreIdName = XName.Get("PropertyStoreId", "http://tempuri.org/DataSet.xsd");

		static XName rootName = XName.Get("Root", "http://tempuri.org/DataSet.xsd");

		static XName rootIdName = XName.Get("RootId", "http://tempuri.org/DataSet.xsd");

		static XName sizeName = XName.Get("Size", "http://tempuri.org/DataSet.xsd");

		static XName typeIdName = XName.Get("TypeId", "http://tempuri.org/DataSet.xsd");

		static XName typeName = XName.Get("type", "http://www.w3.org/2001/XMLSchema-instance");

		static XName valueName = XName.Get("Value", "http://tempuri.org/DataSet.xsd");

		static XName viewerName = XName.Get("Viewer", "http://tempuri.org/DataSet.xsd");

		XElement lastRootElement = null;

		List<XObject> propertyList;

		/// <summary>
		/// Initializes a new instance of Teraque.Tools.WindowMain class.
		/// </summary>
		public WindowMain()
		{

			// The IDE managed resources are initialized here.
			InitializeComponent();

			this.propertyList = new List<XObject>();

			// When a source file is passed to the program from the command line then there is no need to open up the user interface.  This will process the file
			// directly without prompting the user.
			String[] arguments = Environment.GetCommandLineArgs();
			if (arguments.Length > 1)
			{
				ScrubDocument(arguments[1]);
				Application.Current.Shutdown();
			}

		}

		/// <summary>
		/// Handles the clicking of the 'Cancel' button.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnCancelClick(object sender, RoutedEventArgs e)
		{

			// Finish without doing anything.
			this.Close();

		}

		/// <summary>
		/// Handles the clicking of the 'OK' button.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnOKClick(object sender, RoutedEventArgs e)
		{

			// This will scrub the source XML file provided by the user.
			ScrubDocument(this.FileNameTextBox.Text);

			// We're done.
			this.Close();

		}

		Boolean IsType(XElement parentElement, Guid typeId)
		{

			// Search for the node type.
			foreach (XElement xElement in parentElement.Descendants(WindowMain.typeIdName))
				return new Guid(xElement.Value) == typeId;

			return false;

		}

		DateTime GenerateDateTime()
		{

			return DateTime.Now;

		}

		/// <summary>
		/// Sorts the attributes of an element and all it's children in alphabetical order.
		/// </summary>
		/// <param name="sourceElement">The original XElement.</param>
		/// <param name="targetElement">The target XElement where the newly sorted node is placed.</param>
		void ProcessElements(XElement sourceElement, XContainer targetContainer)
		{

			// This will filter out all the existing properties.
			if (sourceElement.Name == propertyStoreName)
				return;

			// Add it back to the target document.
			XElement targetElement = new XElement(sourceElement);
			targetContainer.Add(targetElement);

			// Add processing to elements here.
			if (targetElement.Name == rootName)
			{

				Guid rootId = new Guid(targetElement.Element(rootIdName).Value);
				String elementName = targetElement.Element(nameName).Value;
				XElement viewerElement = targetElement.Element(viewerName);
				viewerElement.Remove();

				this.propertyList.Add(new XComment(String.Format(" {0}: Creation Date ", elementName)));

				XElement createdTimePropertyElement = new XElement(propertyStoreName);
				createdTimePropertyElement.Add(new XElement(propertyIdName, PropertyId.DateCreated.ToString("B").ToUpper()));
				createdTimePropertyElement.Add(new XElement(propertyStoreIdName, Guid.NewGuid().ToString("B").ToUpper()));
				createdTimePropertyElement.Add(new XElement(rootIdName, rootId.ToString("B").ToUpper()));
				createdTimePropertyElement.Add(
					new XElement(valueName,
						new XAttribute(typeName, "xs:dateTime"),
						new XAttribute(XNamespace.Xmlns + "xs", "http://www.w3.org/2001/XMLSchema"),
						new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
						GenerateDateTime().ToString("s")));
				this.propertyList.Add(createdTimePropertyElement);

				this.propertyList.Add(new XComment(String.Format(" {0}: Modification Date ", elementName)));

				XElement modifiedTimePropertyElement = new XElement(propertyStoreName);
				modifiedTimePropertyElement.Add(new XElement(propertyIdName, PropertyId.DateModified.ToString("B").ToUpper()));
				modifiedTimePropertyElement.Add(new XElement(propertyStoreIdName, Guid.NewGuid().ToString("B").ToUpper()));
				modifiedTimePropertyElement.Add(new XElement(rootIdName, rootId.ToString("B").ToUpper()));
				modifiedTimePropertyElement.Add(
					new XElement(valueName,
						new XAttribute(typeName, "xs:dateTime"),
						new XAttribute(XNamespace.Xmlns + "xs", "http://www.w3.org/2001/XMLSchema"),
						new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
						GenerateDateTime().ToString("s")));
				this.propertyList.Add(modifiedTimePropertyElement);

				this.propertyList.Add(new XComment(String.Format(" {0}: Viewer ", elementName)));

				XElement viewerPropertyElement = new XElement(propertyStoreName);
				viewerPropertyElement.Add(new XElement(propertyIdName, PropertyId.Viewer.ToString("B").ToUpper()));
				viewerPropertyElement.Add(new XElement(propertyStoreIdName, Guid.NewGuid().ToString("B").ToUpper()));
				viewerPropertyElement.Add(new XElement(rootIdName, rootId.ToString("B").ToUpper()));
				viewerPropertyElement.Add(
					new XElement(valueName,
						new XAttribute(typeName, "xs:anyURI"),
						new XAttribute(XNamespace.Xmlns + "xs", "http://www.w3.org/2001/XMLSchema"),
						new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
						viewerElement.Value));
				this.propertyList.Add(viewerPropertyElement);
				
				if (this.IsType(targetElement, TypeId.WebPage))
				{

					XElement sizeElement = targetElement.Element(sizeName);
					sizeElement.Remove();

					this.propertyList.Add(new XComment(String.Format(" {0}: Size ", elementName)));

					XElement sizePropertyElement = new XElement(propertyStoreName);
					sizePropertyElement.Add(new XElement(propertyIdName, PropertyId.Size.ToString("B").ToUpper()));
					sizePropertyElement.Add(new XElement(propertyStoreIdName, Guid.NewGuid().ToString("B").ToUpper()));
					sizePropertyElement.Add(new XElement(rootIdName, rootId.ToString("B").ToUpper()));
					sizePropertyElement.Add(new XElement(valueName, 128));
					this.propertyList.Add(sizePropertyElement);

				}

				this.lastRootElement = targetElement;

			}

		}

		void ProcessComments(XComment xComment, XContainer targetContainer)
		{

			if (xComment.Value.Contains("Properties:"))
				return;

			targetContainer.Add(new XComment(xComment));

		}

		/// <summary>
		/// Orders and beautifies an XML document.
		/// </summary>
		/// <param name="path">The XML document to order and beautify.</param>
		void ScrubDocument(String path)
		{

			try
			{

				// Open the source XML document from the path found in the dialog box.
				XDocument sourceDocument = XDocument.Load(path);

				// Create a new target document.  The output path will be constructed from the input path.
				XDocument targetDocument = new XDocument();
				XElement targetRoot = new XElement(sourceDocument.Root);
				targetRoot.RemoveNodes();
				targetDocument.Add(targetRoot);

				// This will order all the attributes in the document in alphabetical order.  Note that the elements can't be similarly ordered because this could 
				// produce forward reference errors.
				foreach (XObject xObject in sourceDocument.Root.Nodes())
				{

					// Comments are passed without alterations.
					XComment xComment = xObject as XComment;
					if (xComment != null)
						this.ProcessComments(xComment, targetRoot);

					// Elements are processed according to our needs.
					XElement xElement = xObject as XElement;
					if (xElement != null)
						this.ProcessElements(xElement, targetRoot);

				}

				this.propertyList.Reverse();
				foreach (XObject xObject in this.propertyList)
					lastRootElement.AddAfterSelf(xObject);

				// Write the output to a temporary file.
				String temporaryPath = Path.GetTempFileName();

				// Save the original document in a safe place.
				String backupPath = Path.Combine(
					Path.GetDirectoryName(path),
					String.Format("{0}.{1}{2}", Path.GetFileNameWithoutExtension(path), "backup", Path.GetExtension(path)));

				// Beautify and save the target document when it has been ordered.  The output file takes it's name from the input file.  If the input file is 
				// 'window1.xml', then the output file will be named 'window1.formatted.xml'.
				XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
				xmlWriterSettings.Indent = true;
				XmlWriter xmlWriter = XmlWriter.Create(temporaryPath, xmlWriterSettings);
				targetDocument.WriteTo(xmlWriter);
				xmlWriter.Close();

				//// Delete any existing backup file and move the original source to the new backup location.
				//if (File.Exists(backupPath))
				//    File.Delete(backupPath);
				//File.Move(path, backupPath);

				// Now move the temporary output to the original file location to make it look like the changes were made to the original file.
				String testPath = "../../output.xml";
				if (File.Exists(testPath))
					File.Delete(testPath);
				File.Move(temporaryPath, testPath);

			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, Properties.Resources.DataModelScrubberHeader);
			}

		}

	}

}
