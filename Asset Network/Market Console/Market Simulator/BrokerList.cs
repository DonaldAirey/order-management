namespace Teraque.AssetNetwork
{

	using System;
	using System.IO;
	using System.Reflection;
	using System.Xml;
	using System.Xml.Linq;
	using System.Collections.Generic;

	/// <summary>
	/// A list of brokers to be simulated.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal class BrokerList : List<BrokerInfo>
	{

		/// <summary>
		/// The name of the resource where the brokers are found.
		/// </summary>
		const String brokerResourceName = "Teraque.AssetNetwork.Brokers.xml";

		/// <summary>
		/// Initializes a new instance of the BrokerList class.
		/// </summary>
		internal BrokerList()
		{

			// This will read the brokers from the resource into the list.
			Stream brokerStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(brokerResourceName);
			XmlReader xmlReader = new XmlTextReader(brokerStream);
			XDocument xDocument = XDocument.Load(xmlReader);
			foreach (XElement xElement in xDocument.Root.Elements())
				this.Add(new BrokerInfo() { Name = xElement.Attribute("Name").Value, Symbol = xElement.Attribute("Symbol").Value });

		}

	}

}
