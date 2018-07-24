namespace Teraque.Tools
{

	using System;

	/// <summary>
	/// An Attribute in the output of an XML document.
	/// </summary>
	internal struct Attribute
	{

		/// <summary>
		/// The local name of the attribute.
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
		/// Initializes a new instance of Teraque.Tools.Attributes class.
		/// </summary>
		/// <param name="localName">The local name of the attribute.</param>
		/// <param name="namespaceUri">The full URI specification of the namespace.</param>
		/// <param name="prefix">The prefix to which the namespace has been assigned.</param>
		public Attribute(String localName, String namespaceUri, String prefix)
		{

			// Initialize the object
			this.localName = localName;
			this.namespaceUri = namespaceUri;
			this.prefix = prefix;

		}

		/// <summary>
		/// The fully quanified name of the attribute.
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
