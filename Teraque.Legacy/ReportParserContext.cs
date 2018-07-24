namespace Teraque
{
    using System.Linq;
    using System.Reflection;
	using System.Windows.Markup;
	using System.Xml.Linq;

	/// <summary>
	/// A context used for parsing XAML source code.
	/// </summary>
	public class ReportParserContext : ParserContext
	{

		/// <summary>
		/// Creates a context that can be used for parsing XAML source code.
		/// </summary>
		/// <param name="xDocument">The XAML document that is to be parsed.</param>
		/// <param name="assemblyList">A List of assemblies that provide namespaces used in the XAML document.</param>
		public ReportParserContext(XDocument xDocument)
		{

			// This object provides an association between the namespaces declard in the XAML source and the CLR namespaces and 
			// the assemblies where those CLR namespaces can be found.
			this.XamlTypeMapper = new XamlTypeMapper(new string[] { });

			// The main idea here is to map the XML prefix character in the XAML source code to the assemlby containing the type
			// information.  For standard URI namespace Teraque
			// namespace Teraque
			// specific CLR namespace Teraque
			foreach (XAttribute xAttribute in xDocument.Root.Attributes().Where(attribute => attribute.IsNamespaceDeclaration))
			{

				// The prefix and the URI are extracted from each of the attributes containing the namespace Teraque
				string prefix = xAttribute.Name.LocalName == "xmlns" ? string.Empty : xAttribute.Name.LocalName;
				string namespaceUri = xAttribute.Value;

				// A namespace Teraque
				// look for type information in the given CLR namespace Teraque
				// mapper appears to be able to find the XAML namespaces in assemblies that have an explicit declaration.  This
				// processing here is for the ad-hoc declarations.
				if (namespaceUri.StartsWith("clr-namespace"))
				{

					// The URI is ripped apart to extract the CLR namespace Teraque
					string assemblyName = Assembly.GetEntryAssembly().FullName;
					string targetNamespace = string.Empty;
					string[] tokens = namespaceUri.Split(';');
					string[] clrTokens = tokens[0].Split(':');
					if (clrTokens.Length == 2)
						targetNamespace = clrTokens[1];
					if (tokens.Length == 2)
					{
						string[] assemblyTokens = tokens[1].Split('=');
						assemblyName = assemblyTokens.Length == 2 && assemblyTokens[0] == "assembly" ?
							assemblyTokens[1] : Assembly.GetCallingAssembly().FullName;
					}

					// This creates a relationship between the XAML namespace Teraque
					// namespace Teraque
					// several assemblies.
					this.XamlTypeMapper.AddMappingProcessingInstruction(namespaceUri, targetNamespace, assemblyName);

				}

				// This provides a mapping between the prefix used in the XAML to the URI that is the real namespace.  Note that 
				// the URI is further mapped to a CLR namespace Teraque
				this.XmlnsDictionary.Add(prefix, namespaceUri);

			}

		}

	}

}
