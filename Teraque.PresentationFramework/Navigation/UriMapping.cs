namespace Teraque.Windows.Navigation
{

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Text.RegularExpressions;
	using System.Windows;
	using System.Windows.Data;
	using System.Windows.Markup;
	using Teraque.Properties;

	/// <summary>
	/// Converts a uniform resource identifier (URI) into a new URI based on the rules of a matching object specified in a collection of mapping objects.
	/// </summary>
	public sealed class UriMapping
	{

		/// <summary>
		/// Regex for capturing replacement parameters.
		/// </summary>
		private static readonly Regex conversionRegex = new Regex("{(?<parameter>.*)}", RegexOptions.ExplicitCapture);

		/// <summary>
		/// Indicates whether or not this object needs to be initialized.
		/// </summary>
		private Boolean initialized;

		/// <summary>
		/// The URI to which the friendly URI is mapped (including replacement parameters).
		/// </summary>
		private Uri mappedUri;

		/// <summary>
		/// A list of identifiers that used in the parameter replacement in the mapped URI.
		/// </summary>
		private List<String> mappedUriIdentifiers;

		/// <summary>
		/// The friendly URI.
		/// </summary>
		private Uri uri;

		/// <summary>
		/// A list of identifiers used in parameter replacement from the template URI.
		/// </summary>
		private List<String> uriIdentifiers;

		/// <summary>
		/// A regular expression used for parsing elements out of the template URI and replacing them in the mapping URI.
		/// </summary>
		private Regex uriRegex;

		/// <summary>
		/// Initializes a new instance of the UriMapping class.
		/// </summary>
		public UriMapping()
		{

			// Initialize the object.
			this.uriIdentifiers = new List<String>();
			this.mappedUriIdentifiers = new List<String>();

		}

		/// <summary>
		/// Gets or sets the uniform resource identifier (URI) that is navigated to instead of the originally requested URI.
		/// </summary>
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public String MappedUri
		{
			get
			{
				return this.mappedUri.OriginalString;
			}
			set
			{
				this.mappedUri = new Uri(value, UriKind.RelativeOrAbsolute);
				this.initialized = false;
			}
		}

		/// <summary>
		/// Gets or sets the pattern to match when determining if the requested uniform resource identifier (URI) is converted to a mapped URI.
		/// </summary>
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public String Uri
		{
			get
			{
				return this.uri.OriginalString;
			}
			set
			{
				this.uri = new Uri(value, UriKind.Relative);
				this.initialized = false;
			}
		}

		/// <summary>
		/// Check that the URI meets the conditions required for mapping.
		/// </summary>
		private void Initialize()
		{

			// Check that a mapping URI has been provided for this mapping.
			if (this.mappedUri == null)
				throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.PropertyCannotBeNull, "MappedUri"));

			// Check that a URI has been provided for this mapping.
			if (this.uri == null)
				throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.PropertyCannotBeNull, "Uri"));

			// The object can be uninitialized by seeting either of it's public properties.  This will re-initialize it just in time for the mapping.
			if (!this.initialized)
			{
				String regexBuilder = this.uri.OriginalString.Replace("\\", "\\\\");
				this.uriRegex = new Regex("^" + conversionRegex.Replace(regexBuilder, "(?<$1>.*)") + "$");
				this.GetIdentifiersForUri();
				this.GetIdentifiersForMappedUri();
				this.initialized = true;
			}

		}

		/// <summary>
		/// Parses the mapped URI for parameter replacement identifiers.
		/// </summary>
		private void GetIdentifiersForMappedUri()
		{

			// This will parse the mapping URI for replacement parameters.
			this.mappedUriIdentifiers.Clear();
			MatchCollection matches = conversionRegex.Matches(this.mappedUri.OriginalString);
			foreach (Match match in matches)
			{
				String item = match.Groups["parameter"].Value;
				if (!this.mappedUriIdentifiers.Contains(item))
					this.mappedUriIdentifiers.Add(item);
			}

		}

		/// <summary>
		/// Gets the identifiers for the template URI.
		/// </summary>
		private void GetIdentifiersForUri()
		{

			// This will collect the unique names of all the replacement parameters in the template URI.
			this.uriIdentifiers.Clear();
			MatchCollection matches = conversionRegex.Matches(this.uri.OriginalString);
			foreach (Match match in matches)
			{
				String item = match.Groups["parameter"].Value;
				if (this.uriIdentifiers.Contains(item))
					throw new InvalidOperationException(ExceptionMessage.Format(ExceptionMessages.UriMappingUriTemplateCannotRepeatIdentifier, "Uri"));
				this.uriIdentifiers.Add(item);
			}

		}

		/// <summary>
		/// Maps an abrieviated URI and expands the replacable elements to form a fully specified URI.
		/// </summary>
		/// <param name="newUri">A URI to match up against the template.  The URI can also contain elements that are copyied to the fully specified URI.</param>
		/// <returns>null if the give URI does not match the template, a fully specified URI including the replaced elements if there is a match.</returns>
		public Uri MapUri(Uri newUri)
		{

            // Validate parameters
            if (newUri == null)
                throw new ArgumentNullException("newUri");

            // The object needs to be initialized after being instantiated or when either of the URLs has changed.
			this.Initialize();

			// The most basic operation of this class is to match a friendly name to a fully formed URL.  If the template is not part of the given URL, then the
			// given URI can't be mapped to the URL in this object.
			Match match = this.uriRegex.Match(newUri.OriginalString);
			if (!match.Success)
				return null;

			// This is the local path for the mapped URI.  Once the parameters have been replaced it is returned as the mapping of the given URL.
			String uriBase = this.mappedUri.OriginalString;

			// This will replace all the parameters from the template with the actual values found in the input URI.
			foreach (String mappedIdentifier in this.mappedUriIdentifiers)
				uriBase = uriBase.Replace(String.Format(CultureInfo.CurrentCulture, "{{{0}}}", mappedIdentifier), match.Groups[mappedIdentifier].Value);

			// This is the mapped URI with the replaceable elements from the input URI.
			return new Uri(uriBase, UriKind.RelativeOrAbsolute);

		}

	}

}