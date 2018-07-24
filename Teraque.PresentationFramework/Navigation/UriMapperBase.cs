namespace Teraque.Windows.Navigation
{

	using System;

	/// <summary>
	/// Represents the base class for classes that convert a requested uniform resource identifier (URI) into a new URI based on mapping rules.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public abstract class UriMapperBase
	{

		/// <summary>
		/// Initializes a new instance of the UriMapperBase class.
		/// </summary>
		protected UriMapperBase()
		{
		}

		/// <summary>
		/// When overridden in a derived class, converts a requested uniform resource identifier (URI) to a new URI.
		/// </summary>
		/// <param name="uri">The original URI value to be mapped to a new URI.</param>
		/// <returns>A URI to use for the request instead of the value in the uri parameter.</returns>
		public abstract Uri MapUri(Uri uri);

	}

}
