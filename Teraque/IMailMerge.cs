namespace Teraque
{

	using System;
	using System.Collections.Generic;
	using System.IO;

	/// <summary>
	/// Creates a merged Word document from a template and a dictionary of merge fields.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	public interface IMailMerge
	{

		/// <summary>
		/// Creates a document using the parameters provided.
		/// </summary>
		/// <param name="sourceDocument">The template for the document.</param>
		/// <param name="dictionary">The mail merge parameters.</param>
		/// <returns>The template document with the parameters substituted for the fields.</returns>
		MemoryStream CreateDocument(Byte[] sourceDocument, Dictionary<String, Object> dictionary);

	}

}
