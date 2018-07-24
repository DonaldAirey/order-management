namespace Teraque.Windows
{

	using System;
	using System.IO;
	using System.Windows;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Xml;
	using System.Xml.Serialization;
	using System.Xml.Schema;

	/// <summary>
	/// An item displayed in the history drop down of the BreadcrumbBar control.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[Serializable]
	public class HistoryItem : IXmlSerializable
	{

		/// <summary>
		/// The source to the item.
		/// </summary>
		public Uri Source { get; set; }

		/// <summary>
		/// An image of the item.
		/// </summary>
		public ImageSource ImageSource { get; set; }

		/// <summary>
		/// Determines whether two Object instances are equal.
		/// </summary>
		/// <param name="obj">The Object to compare with the current Object.</param>
		/// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
		public override bool Equals(object obj)
		{

			// The Source member defines when two BreadcrumbHistoryItems are the same.
			HistoryItem breadcrumbHistoryItem = obj as HistoryItem;
			return breadcrumbHistoryItem == null ? base.Equals(obj) : breadcrumbHistoryItem.Source == this.Source;

		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>A hash code for the current Object.</returns>
		public override int GetHashCode()
		{
			return this.Source.GetHashCode();
		}

		/// <summary>
		/// This method is reserved and should not be used.
		/// </summary>
		/// <returns>null</returns>
		public XmlSchema GetSchema()
		{
			return null;
		}

		/// <summary>
		/// Generates an object from its XML representation.
		/// </summary>
		/// <param name="reader">The XmlReader stream from which the object is deserialized.</param>
		public void ReadXml(XmlReader reader)
		{

			// Validate the parameters.
			if (reader == null)
				throw new ArgumentNullException("reader");

			// The main idea here is to read the XML from the source into the fields of this object.  At this point the reader is positioned on the
			// root element ("<BreadcrumbHistoryItem>") and needs to be moved to the start of the first child element.
			reader.ReadStartElement();

			// The strings can be stored using native data types but the image source is a little more complicated.  They are stored as Base64 strings in the XML 
			// file and read as strings.  The Base64 string is then encoded as a BitmapSource and it's ready to use.
			this.Source = new Uri(reader.ReadElementString("Source"), UriKind.RelativeOrAbsolute);
			if (reader.Name == "BitmapImage")
			{
				String imageData = reader.ReadElementString("BitmapImage");
				using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(imageData)))
				{
					PngBitmapDecoder decoder = new PngBitmapDecoder(memoryStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
					this.ImageSource = decoder.Frames[0];
				}
			}

			// The reader is poisitioned at the end of the element at this point.  This will read past that token and leave the reader ready to read the next
			// BreadcrumbHistoryItem in the stream.
			reader.ReadEndElement();

		}

		/// <summary>
		/// Converts an object into its XML representation.
		/// </summary>
		/// <param name="writer">The XmlWriter stream to which the object is serialized.</param>
		public void WriteXml(XmlWriter writer)
		{

			// Validate the parameters.
			if (writer == null)
				throw new ArgumentNullException("writer");

			// The source is easy enough to write as a string.
			writer.WriteElementString("Source", this.Source.OriginalString);

			// The image requires a little more work as it needs to be decoded and turned into a Base64 string before it can be saved in a format that is
			// native to XML.
			BitmapSource bitmapSource = this.ImageSource as BitmapSource;
			if (bitmapSource != null)
			{
				PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
				pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
				using (MemoryStream memoryStream = new MemoryStream())
				{
					pngBitmapEncoder.Save(memoryStream);
					writer.WriteElementString("BitmapImage", Convert.ToBase64String(memoryStream.ToArray()));
				}
			}

		}

	}

}
