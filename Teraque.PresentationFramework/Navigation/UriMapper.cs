namespace Teraque.Windows.Navigation
{

	using System;
	using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Markup;

	/// <summary>
	/// Converts a uniform resource identifier (URI) into a new URI based on the rules of a matching object specified in a collection of mapping objects.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[ContentProperty("UriMappings")]
	public sealed class UriMapper : UriMapperBase
	{

		/// <summary>
		/// A table mapping a user friendly URI into a new URI.
		/// </summary>
		private Collection<UriMapping> uriMappings;

		/// <summary>
		/// Initializes a new instance of the UriMapper class.
		/// </summary>
		public UriMapper()
		{

			// Initialize the object.
			this.uriMappings = new Collection<UriMapping>();

		}

		/// <summary>
		/// Gets a collection of objects that are used to convert a uniform resource identifier (URI) into a new URI.
		/// </summary>
		public Collection<UriMapping> UriMappings
		{
			get
			{
				return this.uriMappings;
			}
		}

		/// <summary>
		/// Converts a specified uniform resource identifier (URI) into a new URI based on the rules of a matching object in the UriMappings collection.
		/// </summary>
		/// <param name="uri">Original URI value to be converted to a new URI.</param>
		/// <returns>A URI to use for handling the request instead of the value of the uri parameter. If no object in the UriMappings collection matches uri, the
		/// original value for uri is returned.</returns>
		public override Uri MapUri(Uri uri)
		{

			// Take the first mapping that matches the given URI pattern and return the mapping (complete with parameter substitution).
			foreach (UriMapping mapping in this.uriMappings)
			{
				Uri mappedUri = mapping.MapUri(uri);
				if (mappedUri != null)
					return mappedUri;
			}

			// If no mapping exists, the original URI is used.
			return uri;

		}

	}

}