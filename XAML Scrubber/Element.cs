namespace Teraque.Tools
{

	using System;

	/// <summary>
	/// An Element in the output of an XML document.
	/// </summary>
	internal class Element
	{

		/// <summary>
		/// The number of attributes associated with this element.
		/// </summary>
		private Int32 attributeCount;

		/// <summary>
		/// The number of child elements associated with this element.
		/// </summary>
		private Int32 childCount;

		/// <summary>
		/// The local name of the element.
		/// </summary>
		private String localName;

		/// <summary>
		/// The URI of the namespace to which the local name belongs.
		/// </summary>
		private String namespaceUri;

		/// <summary>
		/// The prefix that has been assigned to the namespace.
		/// </summary>
		private String prefix;

		/// <summary>
		/// Creates a new instance of the Element class.
		/// </summary>
		/// <param name="localName">The local name of the element.</param>
		/// <param name="namespaceUri">The URI of the namespace to which the local name belongs.</param>
		/// <param name="prefix">The namespace prefix of the element.</param>
		public Element(String localName, String namespaceUri, String prefix)
		{

			// Initialize the object
			this.attributeCount = 0;
			this.childCount = 0;
			this.localName = localName;
			this.namespaceUri = namespaceUri;
			this.prefix = prefix;

		}

		/// <summary>
		/// The number of attributes associated with this element.
		/// </summary>
		public Int32 AttributeCount
		{
			get { return this.attributeCount; }
			set { this.attributeCount = value; }
		}

		/// <summary>
		/// The number of child elements associated with this element.
		/// </summary>
		public Int32 ChildCount
		{
			get { return this.childCount; }
			set { this.childCount = value; }
		}

		/// <summary>
		/// The fully qualified name of the element.
		/// </summary>
		public String QualifiedName
		{
			get
			{
				return this.prefix == String.Empty ? this.localName : String.Format("{0}:{1}", this.prefix, this.localName);
			}
		}

	}

}
