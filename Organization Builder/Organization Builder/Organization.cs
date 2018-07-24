namespace Teraque
{

	using System;
	using System.Collections.Generic;
	using System.Xml.Linq;
	using Teraque;

	/// <summary>
	/// Used to build a script that will realize an organization from the description.
	/// </summary>
	class Organization
	{

		/// <summary>
		/// Create an organization from an XML description.
		/// </summary>
		/// <param name="organizationDescription">An XML description of the organization.</param>
		/// <returns>A XML script that can be run against the data model to produce the organization.</returns>
		public static XDocument Create(XDocument organizationDescription)
		{

			// This is the document where we'll build the script.
			XDocument organizationScript = new XDocument();

			// These values defined at the root are used several levels deep in the construction of the scripts so we make them available in the class.
			String organizationName = organizationDescription.Root.Attribute("name").Value;
			String configurationId = organizationDescription.Root.Attribute("configurationId").Value;
			
			// The root node of the script.
			XElement script = new XElement("script");
			script.Add(new XAttribute("name", organizationName));
			organizationScript.Add(script);

			// Create the channel description.
			script.Add(Organization.CreateClientConnection());

			// This will recurse into the structure creating the hierarchy.
			Organization.CreateOrganizationHierarchy(script, organizationDescription.Root, configurationId, null);

			// This script can be run against the data model to realize the organization described in the original XML.
			return organizationScript;

		}

		/// <summary>
		/// Create a blotter element in the object hierarchy.
		/// </summary>
		/// <returns>A method to create a blotter element.</returns>
		static XElement CreateBlotter(String configurationId, String entityKey, String partyTypeKey)
		{

			//    <method name="StoreBlotter">
			//      <parameter name="configurationId" value="Default" />
			//      <parameter name="entityKey" value="PILGRIM INVESTMENTS" />
			//      <parameter name="partyTypeKey" value="INSTITUTION" />
			//    </method>
			return new XElement("method",
				new XAttribute("name", "StoreBlotter"),
				new XElement("parameter", new XAttribute("name", "configurationId"), new XAttribute("value", configurationId)),
				new XElement("parameter", new XAttribute("name", "entityKey"), new XAttribute("value", entityKey)),
				new XElement("parameter", new XAttribute("name", "partyTypeKey"), new XAttribute("value", partyTypeKey)));

		}

		/// <summary>
		/// Creates a connection for the script.
		/// </summary>
		/// <returns>An element that defines a client channel connection.</returns>
		static XElement CreateClientConnection()
		{

			//   <client name="DataModelClient" type="DataModelClient,Teraque.AssetNetwork.ClientDataModel" endpoint="TcpDataModelEndpoint" />
			XElement clientElement = new XElement("client");
			clientElement.Add(new XAttribute("type", "DataModelClient,Teraque.AssetNetwork.ClientDataModel"));
			return clientElement;

		}

		/// <summary>
		/// Creates a script to crate a desk from a description.
		/// </summary>
		/// <param name="unitDescription">A description of the desk.</param>
		/// <returns>A script that will create the desk.</returns>
		static XElement CreateBlotter(XElement unitDescription, String configurationId, String externalId, String parentKey)
		{

			// This makes sure that the organizational unit is either built correctly or rolled back completely.
			XElement transactionElement = new XElement("transaction");

			// Extract the properties of the organizational unit from the element description.
			String imageKey = unitDescription.Attribute("imageKey").Value;
			String name = unitDescription.Attribute("name").Value;
			String typeKey = unitDescription.Attribute("typeKey").Value;
			String partyTypeKey = unitDescription.Attribute("partyTypeKey").Value;

			// Create the hierarchy of a desk.
			transactionElement.Add(Organization.CreateEntity(configurationId, externalId, imageKey, name, typeKey));
			transactionElement.Add(Organization.CreateBlotter(configurationId, externalId, partyTypeKey));
			transactionElement.Add(Organization.CreateRelation(configurationId, externalId, parentKey));

			// Associate the blotter with an equity blotter viewer.
			transactionElement.Add(
				CreateProperty(configurationId, externalId, "VIEWER", "pack://application:,,,/Teraque.AssetNetwork.Blotter;component/EquityBlotterPage.xaml"));

			// This will recurse into the hiearchy and produce the next level of the organization.
			foreach (XElement childElement in unitDescription.Elements())
				CreateOrganizationHierarchy(transactionElement, childElement, configurationId, externalId);

			// This script will execute as a unit or be rolled back as a unit.
			return transactionElement;

		}

		/// <summary>
		/// Create a script that defines the basic entity of an organization.
		/// </summary>
		/// <param name="organizationRoot">A description of the organization.</param>
		/// <returns>An script element that will create the entity and the associated data structures.</returns>
		static XElement CreateEntity(String configurationId, String entityId, String imageKey, String name, String typeKey)
		{

			//    <method name="StoreEntity">
			//      <parameter name="configurationId" value="Default" />
			//      <parameter name="createdTime" value="6/18/2009 4:41:46 PM" />
			//      <parameter name="externalId0" value="PILGRIM INVESTMENTS" />
			//      <parameter name="hidden" value="false" />
			//      <parameter name="imageKey" value="INSTITUTION" />
			//      <parameter name="modifiedTime" value="6/18/2009 4:41:46 PM" />
			//      <parameter name="name" value="Pilgrim Investments" />
			//      <parameter name="readOnly" value="false" />
			//      <parameter name="typeKey" value="INSTITUTION" />
			//    </method>
			return new XElement("method",
				new XAttribute("name", "StoreEntity"),
				new XElement("parameter", new XAttribute("name", "configurationId"), new XAttribute("value", configurationId)),
				new XElement("parameter", new XAttribute("name", "createdTime"), new XAttribute("value", DateTime.Now.ToString("G"))),
				new XElement("parameter", new XAttribute("name", "externalId0"), new XAttribute("value", entityId)),
				new XElement("parameter", new XAttribute("name", "hidden"), new XAttribute("value", "false")),
				new XElement("parameter", new XAttribute("name", "imageKey"), new XAttribute("value", imageKey)),
				new XElement("parameter", new XAttribute("name", "modifiedTime"), new XAttribute("value", DateTime.Now.ToString("G"))),
				new XElement("parameter", new XAttribute("name", "name"), new XAttribute("value", name)),
				new XElement("parameter", new XAttribute("name", "readOnly"), new XAttribute("value", "false")),
				new XElement("parameter", new XAttribute("name", "typeKey"), new XAttribute("value", typeKey)));

		}

		/// <summary>
		/// Creates a script that builds the organization and all the supporting data structures.
		/// </summary>
		/// <param name="descriptionElement">The description of the organization.</param>
		/// <returns>A script that can be run through the script loader and will build the organization.</returns>
		static XElement CreateHedgeFund(XElement descriptionElement, String configurationId, String parentKey)
		{

			// Extract the attributes of this organization.
			if (descriptionElement.Attribute("configurationId") != null)
				configurationId = descriptionElement.Attribute("configurationId").Value;
			String externalId = descriptionElement.Attribute("externalId").Value;
			String imageKey = descriptionElement.Attribute("imageKey").Value;
			String name = descriptionElement.Attribute("name").Value;
			String partyTypeKey = "HEDGE FUND";
			String typeKey = "HEDGE FUND";

			// An institution is created as a complete transaction.
			XElement transactionElement = new XElement("transaction");

			// Create the object hierarchy for an Organization.
			transactionElement.Add(Organization.CreateEntity(configurationId, externalId, imageKey, name, typeKey));
			transactionElement.Add(Organization.CreateBlotter(configurationId, externalId, partyTypeKey));
			transactionElement.Add(Organization.CreateSource(configurationId, externalId));

			//    <method name="StoreInstitution">
			//      <parameter name="configurationId" value="Default" />
			//      <parameter name="sourceKey" value="PILGRIM INVESTMENTS" />
			//    </method>
			transactionElement.Add(
				new XElement("method",
					new XAttribute("name", "StoreInstitution"),
					new XElement("parameter", new XAttribute("name", "configurationId"), new XAttribute("value", configurationId)),
					new XElement("parameter", new XAttribute("name", "sourceKey"), new XAttribute("value", externalId))));

			// All institutions act like navigators.
			transactionElement.Add(
				CreateProperty(configurationId, externalId, "VIEWER", "pack://application:,,,/Teraque.AssetNetwork.Navigator;component/DirectoryPage.xaml"));

			// This will recurse into the hiearchy and produce the next level of the organization.
			foreach (XElement childElement in descriptionElement.Elements())
				CreateOrganizationHierarchy(transactionElement, childElement, configurationId, externalId);

			// This is a complete script to build the institution described.
			return transactionElement;

		}

		/// <summary>
		/// Creates a script that builds the organization and all the supporting data structures.
		/// </summary>
		/// <param name="descriptionElement">The description of the organization.</param>
		/// <returns>A script that can be run through the script loader and will build the organization.</returns>
		static XElement CreateInstitution(XElement descriptionElement, String configurationId, String parentKey)
		{

			// Extract the attributes of this organization.
			if (descriptionElement.Attribute("configurationId") != null)
				configurationId = descriptionElement.Attribute("configurationId").Value;
			String externalId = descriptionElement.Attribute("externalId").Value;
			String imageKey = descriptionElement.Attribute("imageKey").Value;
			String name = descriptionElement.Attribute("name").Value;
			String partyTypeKey = "INSTITUTION";
			String typeKey = "INSTITUTION";

			// An institution is created as a complete transaction.
			XElement transactionElement = new XElement("transaction");

			// Create the object hierarchy for an Organization.
			transactionElement.Add(Organization.CreateEntity(configurationId, externalId, imageKey, name, typeKey));
			transactionElement.Add(Organization.CreateBlotter(configurationId, externalId, partyTypeKey));
			transactionElement.Add(Organization.CreateSource(configurationId, externalId));

			//    <method name="StoreInstitution">
			//      <parameter name="configurationId" value="Default" />
			//      <parameter name="sourceKey" value="PILGRIM INVESTMENTS" />
			//    </method>
			transactionElement.Add(
				new XElement("method",
					new XAttribute("name", "StoreInstitution"),
					new XElement("parameter", new XAttribute("name", "configurationId"), new XAttribute("value", configurationId)),
					new XElement("parameter", new XAttribute("name", "sourceKey"), new XAttribute("value", externalId))));

			// All institutions act like navigators.
			transactionElement.Add(
				CreateProperty(configurationId, externalId, "VIEWER", "pack://application:,,,/Teraque.AssetNetwork.Navigator;component/DirectoryPage.xaml"));

			// This will recurse into the hiearchy and produce the next level of the organization.
			foreach (XElement childElement in descriptionElement.Elements())
				CreateOrganizationHierarchy(transactionElement, childElement, configurationId, externalId);

			// This is a complete script to build the institution described.
			return transactionElement;

		}

		/// <summary>
		/// <summary>
		/// Creates a script to create a parent-child relationship from just a reference to an entity.
		/// </summary>
		/// <param name="unitDescription">A description of the desk.</param>
		/// <returns>A script that will create the desk.</returns>
		static XElement CreateEntityReference(XElement unitDescription, String configurationId, String parentKey)
		{

			// Extract the properties from the element.
			String childKey = unitDescription.Attribute("entityId").Value;

			// Create a relationship between two entities.
			return Organization.CreateRelation(configurationId, childKey, parentKey);

		}

		/// <summary>
		/// Creates a script to create a Trader.
		/// </summary>
		/// <param name="unitDescription">A description of the desk.</param>
		/// <returns>A script that will create the desk.</returns>
		static XElement CreateManager(XElement unitDescription, String configurationId, String externalId)
		{

			// This makes sure that the organizational unit is either built correctly or rolled back completely.
			XElement transactionElement = new XElement("transaction");

			// Extract the properties of the organizational unit from the element description.
			String imageKey = unitDescription.Attribute("imageKey").Value;
			String name = unitDescription.Attribute("name").Value;
			String distinguishedName = unitDescription.Attribute("distinguishedName").Value;

			// Create the hierarchy of a desk.
			transactionElement.Add(Organization.CreateEntity(configurationId, externalId, imageKey, name, "USER"));
			transactionElement.Add(Organization.CreateUser(configurationId, externalId, distinguishedName));

			// This will recurse into the hiearchy and produce the next level of the organization.
			foreach (XElement childElement in unitDescription.Elements())
				CreateOrganizationHierarchy(transactionElement, childElement, configurationId, externalId);

			// This script will execute as a unit or be rolled back as a unit.
			return transactionElement;

		}

		/// <summary>
		/// Creates scripts for the organizational hierarchy, recursing into the structure to create sub-groups.
		/// </summary>
		/// <param name="descriptionElement">The scription of the organizational unit in the current level of the hierarchy.</param>
		/// <returns>A script to create the organization described by the 'unitDescription' node.</returns>
		static void CreateOrganizationHierarchy(XElement script, XElement descriptionElement, String configurationId, String parentKey)
		{

			// The child key is used to build the relationship between the parent and child in the hierarchy.
			XAttribute childKeyAttribute = descriptionElement.Attribute("externalId");
			String childKey = childKeyAttribute == null ? String.Empty : childKeyAttribute.Value;

			// Different organizations can be created based on the node type.
			switch (descriptionElement.Name.LocalName)
			{

			case "User":

				script.Add(Organization.CreateUser(descriptionElement, configurationId, childKey));
				break;

			case "Blotter":

				script.Add(Organization.CreateBlotter(descriptionElement, configurationId, childKey, parentKey));
				break;

			case "EntityReference":

				script.Add(Organization.CreateEntityReference(descriptionElement, configurationId, parentKey));
				break;

			case "HedgeFund":

				script.Add(Organization.CreateHedgeFund(descriptionElement, configurationId, parentKey));
				break;

			case "Institution":

				script.Add(Organization.CreateInstitution(descriptionElement, configurationId, parentKey));
				break;

			case "Manager":

				script.Add(Organization.CreateManager(descriptionElement, configurationId, childKey));
				break;

			case "Properties":

				foreach (XElement propertyElement in descriptionElement.Elements("Property"))
				{
					String key = propertyElement.Attribute("key").Value;
					String value = propertyElement.Attribute("value").Value;
					script.Add(Organization.CreateProperty(configurationId, parentKey, key, value));
				}

				break;

			case "SystemFolder":

				script.Add(Organization.CreateSystemFolder(descriptionElement, configurationId, childKey, parentKey));
				break;

			case "Trader":

				script.Add(Organization.CreateTrader(descriptionElement, configurationId, childKey));
				break;

			}

		}

		/// <summary>
		/// Create a script that loads the properties of an organization.
		/// </summary>
		/// <param name="propertyDescription">The description of the property.</param>
		/// <returns>A script that builds the property settings.</returns>
		static XElement CreateProperty(String configurationId, String entityId, String key, String value)
		{

			//		<method name="StorePropertyStore">
			//			<parameter name="configurationId" value="Default" />
			//			<parameter name="entityKey" value="PILGRIM INVESTMENTS" />
			//			<parameter name="externalId0" value="{08817A37-4C93-4a75-B7C9-5DFD229ACB54}" />
			//			<parameter name="propertyKey" value="VIEWER" />
			//			<parameter name="value" value="pack://application:,,,/Teraque.AssetNetwork.Navigator;component/DirectoryPage.xaml" />
			//		</method>
			return new XElement("method",
				new XAttribute("name", "StorePropertyStore"),
				new XElement("parameter", new XAttribute("name", "configurationId"), new XAttribute("value", configurationId)),
				new XElement("parameter", new XAttribute("name", "entityKey"), new XAttribute("value", entityId)),
				new XElement("parameter", new XAttribute("name", "externalId0"), new XAttribute("value", Guid.NewGuid().ToString("B"))),
				new XElement("parameter", new XAttribute("name", "propertyKey"), new XAttribute("value", key)),
				new XElement("parameter", new XAttribute("name", "value"), new XAttribute("value", value)));

		}

		/// <summary>
		/// Create a relationship between two entities.
		/// </summary>
		/// <param name="configurationId">The configuration of indices to use.</param>
		/// <param name="childId">The child entity id.</param>
		/// <param name="parentId">The parent entity id.</param>
		/// <returns>A script to build a relationship between two entities.</returns>
		static XElement CreateRelation(String configurationId, String childId, String parentId)
		{

			//  <method name="StoreEntityTree">
			//    <parameter name="configurationId" value="Default" />
			//    <parameter name="entityKeyByChildId" value="PILGRIM STRATEGY DESK BLOTTER" />
			//    <parameter name="entityKeyByParentId" value="PILGRIM INVESTMENTS" />
			//    <parameter name="externalId0" value="{C2AEEA9C-2E13-427e-8D9E-9E82865A654F}" />
			//  </method>
			return new XElement("method",
				new XAttribute("name", "StoreEntityTree"),
				new XElement("parameter", new XAttribute("name", "configurationId"), new XAttribute("value", configurationId)),
				new XElement("parameter", new XAttribute("name", "entityKeyByChildId"), new XAttribute("value", childId)),
				new XElement("parameter", new XAttribute("name", "entityKeyByParentId"), new XAttribute("value", parentId)),
				new XElement("parameter", new XAttribute("name", "externalId0"), new XAttribute("value", Guid.NewGuid().ToString("B"))));

		}

		/// <summary>
		/// Create a source element in the object hierarchy.
		/// </summary>
		/// <returns>A method to create a source element.</returns>
		static XElement CreateSource(String configurationId, String externalId)
		{

			//    <method name="StoreSource">
			//      <parameter name="blotterKey" value="PILGRIM INVESTMENTS" />
			//      <parameter name="configurationId" value="Default" />
			//    </method>
			return new XElement("method",
				new XAttribute("name", "StoreSource"),
				new XElement("parameter", new XAttribute("name", "blotterKey"), new XAttribute("value", externalId)),
				new XElement("parameter", new XAttribute("name", "configurationId"), new XAttribute("value", configurationId)));

		}

		/// <summary>
		/// Creates a script to create a System folder.
		/// </summary>
		/// <param name="unitDescription">A description of the system folder.</param>
		/// <returns>A script that will create the folder.</returns>
		static XElement CreateSystemFolder(XElement unitDescription, String configurationId, String externalId, String parentKey)
		{

			// This makes sure that the organizational unit is either built correctly or rolled back completely.
			XElement transactionElement = new XElement("transaction");

			// Extract the properties of the organizational unit from the element description.
			String imageKey = unitDescription.Attribute("imageKey").Value;
			String name = unitDescription.Attribute("name").Value;

			// Create the hierarchy for a system folder.
			transactionElement.Add(Organization.CreateEntity(configurationId, externalId, imageKey, name, "SYSTEM FOLDER"));

			//    <method name="StoreFolder" client="DataModelClient">
			//      <parameter name="configurationId" value="Default" />
			//      <parameter name="entityKey" value="SURESH KAPUR FOLDER" />
			//    </method>
			transactionElement.Add(new XElement("method",
				new XAttribute("name", "StoreFolder"),
				new XElement("parameter", new XAttribute("name", "configurationId"), new XAttribute("value", configurationId)),
				new XElement("parameter", new XAttribute("name", "entityKey"), new XAttribute("value", externalId))));

			//    <method name="StoreSystemFolder" client="DataModelClient">
			//      <parameter name="configurationId" value="Default" />
			//      <parameter name="folderKey" value="SURESH KAPUR FOLDER" />
			//    </method>
			transactionElement.Add(new XElement("method",
				new XAttribute("name", "StoreSystemFolder"),
				new XElement("parameter", new XAttribute("name", "configurationId"), new XAttribute("value", configurationId)),
				new XElement("parameter", new XAttribute("name", "folderKey"), new XAttribute("value", externalId))));

			// Attach the folder to it's parent when a parent is provided.
			if (!String.IsNullOrEmpty(parentKey))
				transactionElement.Add(Organization.CreateRelation(configurationId, externalId, parentKey));

			// All system folders act like navigators.
			transactionElement.Add(
				CreateProperty(configurationId, externalId, "VIEWER", "pack://application:,,,/Teraque.AssetNetwork.Navigator;component/DirectoryPage.xaml"));
			
			// This will recurse into the hiearchy and produce the next level of the organization.
			foreach (XElement childElement in unitDescription.Elements())
				CreateOrganizationHierarchy(transactionElement, childElement, configurationId, externalId);

			// This script will execute as a unit or be rolled back as a unit.
			return transactionElement;

		}

		/// <summary>
		/// Creates a script to create a Trader.
		/// </summary>
		/// <param name="unitDescription">A description of the desk.</param>
		/// <returns>A script that will create the desk.</returns>
		static XElement CreateTrader(XElement unitDescription, String configurationId, String externalId)
		{

			// This makes sure that the organizational unit is either built correctly or rolled back completely.
			XElement transactionElement = new XElement("transaction");

			// Extract the properties of the organizational unit from the element description.
			String imageKey = unitDescription.Attribute("imageKey").Value;
			String name = unitDescription.Attribute("name").Value;
			String distinguishedName = unitDescription.Attribute("distinguishedName").Value;
			String blotterDefaultId = unitDescription.Attribute("blotterDefaultId").Value;
			String isAgencyMatch = unitDescription.Attribute("isAgencyMatch").Value;
			String isBrokerMatch = unitDescription.Attribute("isBrokerMatch").Value;
			String isHedgeMatch = unitDescription.Attribute("isHedgeMatch").Value;
			String isInstitutionMatch = unitDescription.Attribute("isInstitutionMatch").Value;

			// Create the hierarchy of a desk.
			transactionElement.Add(Organization.CreateEntity(configurationId, externalId, imageKey, name, "TRADER"));
			transactionElement.Add(Organization.CreateUser(configurationId, externalId, distinguishedName));

			//    <method name="StoreTrader" client="DataModelClient">
			//      <parameter name="blotterIdDefault" value="HILDEGARD KOHLER BLOTTER" />
			//      <parameter name="configurationId" value="Default" />
			//      <parameter name="isAgencyMatch" value="false" />
			//      <parameter name="isBrokerMatch" value="true" />
			//      <parameter name="isCommissionChangeAllowed" value="false" />
			//      <parameter name="isEditExecutionsAllowed" value="true" />
			//      <parameter name="isHedgeMatch" value="false" />
			//      <parameter name="isHeld" value="true" />
			//      <parameter name="isInstitutionMatch" value="true" />
			//      <parameter name="lotSizeDefault" value="100" />
			//      <parameter name="marketSleep" value="10" />
			//      <parameter name="reviewWindow" value="0" />
			//      <parameter name="submissionTypeCode" value="AlwaysMatch" />
			//      <parameter name="userKey" value="HILDEGARD KOHLER" />
			//    </method>
			transactionElement.Add(new XElement("method",
				new XAttribute("name", "StoreTrader"),
				new XElement("parameter", new XAttribute("name", "blotterDefaultId"), new XAttribute("value", blotterDefaultId)),
				new XElement("parameter", new XAttribute("name", "configurationId"), new XAttribute("value", configurationId)),
				new XElement("parameter", new XAttribute("name", "isAgencyMatch"), new XAttribute("value", isAgencyMatch)),
				new XElement("parameter", new XAttribute("name", "isBrokerMatch"), new XAttribute("value", isBrokerMatch)),
				new XElement("parameter", new XAttribute("name", "isCommissionChangeAllowed"), new XAttribute("value", "false")),
				new XElement("parameter", new XAttribute("name", "isEditExecutionsAllowed"), new XAttribute("value", "true")),
				new XElement("parameter", new XAttribute("name", "isHedgeMatch"), new XAttribute("value", isHedgeMatch)),
				new XElement("parameter", new XAttribute("name", "isHeld"), new XAttribute("value", "true")),
				new XElement("parameter", new XAttribute("name", "isInstitutionMatch"), new XAttribute("value", isInstitutionMatch)),
				new XElement("parameter", new XAttribute("name", "lotSizeDefault"), new XAttribute("value", "100")),
				new XElement("parameter", new XAttribute("name", "marketSleep"), new XAttribute("value", "10")),
				new XElement("parameter", new XAttribute("name", "reviewWindow"), new XAttribute("value", "0")),
				new XElement("parameter", new XAttribute("name", "submissionTypeCode"), new XAttribute("value", "AlwaysMatch")),
				new XElement("parameter", new XAttribute("name", "userKey"), new XAttribute("value", externalId))));

			// This will recurse into the hiearchy and produce the next level of the organization.
			foreach (XElement childElement in unitDescription.Elements())
				CreateOrganizationHierarchy(transactionElement, childElement, configurationId, externalId);

			// This script will execute as a unit or be rolled back as a unit.
			return transactionElement;

		}

		/// <summary>
		/// Create a user's group
		/// </summary>
		/// <returns>A method to create a source element.</returns>
		static XElement CreateUser(String configurationId, String entityKey, String distinguishedName)
		{

			//    <method name="StoreUser" client="DataModelClient">
			//      <parameter name="configurationId" value="Default" />
			//      <parameter name="entityKey" value="ALICE WONG" />
			//      <parameter name="distinguishedName" value="cn=Alice Wong,ou=Aspen Group,o=Teraque,dc=teraque,dc=com" />
			//    </method>
			return new XElement("method",
				new XAttribute("name", "StoreUser"),
				new XElement("parameter", new XAttribute("name", "configurationId"), new XAttribute("value", configurationId)),
				new XElement("parameter", new XAttribute("name", "entityKey"), new XAttribute("value", entityKey)),
				new XElement("parameter", new XAttribute("name", "distinguishedName"), new XAttribute("value", distinguishedName)));

		}

		/// <summary>
		/// Creates a script to crate a desk from a description.
		/// </summary>
		/// <param name="unitDescription">A description of the desk.</param>
		/// <returns>A script that will create the desk.</returns>
		static XElement CreateUser(XElement unitDescription, String configurationId, String externalId)
		{

			// This makes sure that the organizational unit is either built correctly or rolled back completely.
			XElement transactionElement = new XElement("transaction");

			// Extract the properties of the organizational unit from the element description.
			String imageKey = unitDescription.Attribute("imageKey").Value;
			String name = unitDescription.Attribute("name").Value;
			String distinguishedName = unitDescription.Attribute("distinguishedName").Value;

			// Create the hierarchy of a desk.
			transactionElement.Add(Organization.CreateEntity(configurationId, externalId, imageKey, name, "USER"));
			transactionElement.Add(Organization.CreateUser(configurationId, externalId, distinguishedName));

			// This will recurse into the hiearchy and produce the next level of the organization.
			foreach (XElement childElement in unitDescription.Elements())
				Organization.CreateOrganizationHierarchy(transactionElement, childElement, configurationId, externalId);

			// This script will execute as a unit or be rolled back as a unit.
			return transactionElement;

		}

		/// <summary>
		/// Create a user's group
		/// </summary>
		/// <returns>A method to create a source element.</returns>
		static XElement CreateUserGroup(String configurationId, String groupName, String groupTypeKey, String description, String externalId, String imageKey)
		{

			XElement transaction = new XElement("transaction");

			//    <method name="StoreEntity">
			//      <parameter name="configurationId" value="Default" />
			//      <parameter name="createdTime" value="6/18/2009 4:41:46 PM" />
			//      <parameter name="description" value="Administrators have complete and unrestricted access to the system" />
			//      <parameter name="externalId0" value="PILGRIM INVESTMENTS ADMINISTRATORS" />
			//      <parameter name="hidden" value="false" />
			//      <parameter name="modifiedTime" value="6/18/2009 4:41:46 PM" />
			//      <parameter name="imageKey" value="GROUP" />
			//      <parameter name="name" value="Administrators" />
			//      <parameter name="readOnly" value="false" />
			//      <parameter name="typeKey" value="GROUP" />
			//    </method>
			transaction.Add(Organization.CreateEntity(configurationId, externalId, imageKey, groupName, "GROUP"));

			//    <method name="StoreGroup">
			//      <parameter name="configurationId" value="Default" />
			//      <parameter name="groupTypeKey" value="SITE ADMIN" />
			//      <parameter name="entityKey" value="PILGRIM INVESTMENTS ADMINISTRATORS" />
			//    </method>
			transaction.Add(new XElement("method",
				new XAttribute("name", "StoreGroup"),
				new XElement("parameter", new XAttribute("name", "configurationId"), new XAttribute("value", configurationId)),
				new XElement("parameter", new XAttribute("name", "groupTypeKey"), new XAttribute("value", groupTypeKey)),
				new XElement("parameter", new XAttribute("name", "entityKey"), new XAttribute("value", externalId))));

			// This defines a complete user group.
			return transaction;

		}

	}

}
