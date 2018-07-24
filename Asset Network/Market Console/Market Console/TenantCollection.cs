namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Xml.Linq;
	using Teraque.AssetNetwork.WebService;
	using Teraque.AssetNetwork.MarketConsole.Properties;

	/// <summary>
	/// Represents the administrator and the tenants on the server.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal class TenantCollection : ObservableCollection<TenantInfo>
	{

		/// <summary>
		/// The connection details for the administrator.
		/// </summary>
		ClientInfo administratorClientInfo = new ClientInfo();

		/// <summary>
		/// Initializes a new instance of the TenantCollection class.
		/// </summary>
		public TenantCollection()
		{

			// This document contains the configuration of the server.
			XDocument xDocument = XDocument.Load(Environment.ExpandEnvironmentVariables(Settings.Default.TenantConfigurationFile));

			// We keep the administrator password at the root.  The administrator has priveleges to start and stop tenants.
			this.administratorClientInfo.EndpointConfigurationName = xDocument.Root.Attribute("Endpoint").Value;
			this.administratorClientInfo.UserName = xDocument.Root.Attribute("UserName").Value;
			this.administratorClientInfo.Password = xDocument.Root.Attribute("Password").Value;

			// This will load in the properties of each of the tenants.  These properties are used to connect to the tenant and manage it.
			foreach (XElement xElement in xDocument.Root.Elements("Tenant"))
				this.Add(
					new TenantInfo()
					{
						ConnectionString = xElement.Attribute("ConnectionString").Value,
						EndpointConfigurationName = xElement.Attribute("Endpoint").Value,
						Name = xElement.Attribute("Name").Value,
						Password = xElement.Attribute("Password").Value,
						StartupType = (StartupType)Enum.Parse(typeof(StartupType), xElement.Attribute("StartupType").Value),
						UserName = xElement.Attribute("UserName").Value
					});

		}

		/// <summary>
		/// Gets the administrator's web connection info.
		/// </summary>
		internal ClientInfo AdministratorClientInfo
		{
			get
			{
				return this.administratorClientInfo;
			}
		}

	}

}
