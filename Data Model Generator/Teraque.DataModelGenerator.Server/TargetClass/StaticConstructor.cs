namespace Teraque.DataModelGenerator.TargetClass
{

    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
	using System.Xml.Linq;

	/// <summary>
	/// Creates a static constructor for the data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class StaticConstructor : CodeTypeConstructor
	{

		/// <summary>
		/// Create a static constructor for the data model.
		/// </summary>
		/// <param name="dataModelSchema">A description of the data model.</param>
		public StaticConstructor(DataModelSchema dataModelSchema)
		{

			/// <summary>
			//        static DataModel() {
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Static Constructor for the {0}.", dataModelSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));

			//			System.Xml.Linq.XDocument xDocument = System.Xml.Linq.XDocument.Load(System.Environment.ExpandEnvironmentVariables(Teraque.Properties.Settings.Default.OrganizationConfigurationFile));
			this.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference(typeof(XDocument)),
					"xDocument",
					new CodeMethodInvokeExpression(
						new CodeTypeReferenceExpression(typeof(XDocument)),
						"Load",
						new CodeMethodInvokeExpression(
							new CodeTypeReferenceExpression(typeof(Environment)),
							"ExpandEnvironmentVariables",
							new CodePropertyReferenceExpression(
								new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(Teraque.Properties.Settings)), "Default"), "OrganizationConfigurationFile")))));

			//			for (System.Collections.Generic.IEnumerator<System.Xml.Linq.XElement> iEnumerator = xDocument.Root.Elements("Organization").GetEnumerator(); iEnumerator.MoveNext(); )
			//			{
			CodeIterationStatement organizationLoop = new CodeIterationStatement();
			organizationLoop.InitStatement = new CodeVariableDeclarationStatement(
				new CodeTypeReference(typeof(IEnumerator<XElement>)),
				"iEnumerator",
				new CodeMethodInvokeExpression(
					new CodeMethodInvokeExpression(
						new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("xDocument"), "Root"),
						"Elements",
						new CodePrimitiveExpression("Organization")),
					"GetEnumerator"));
			organizationLoop.TestExpression = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("iEnumerator"), "MoveNext");
			organizationLoop.IncrementStatement = new CodeSnippetStatement();

			//				System.Xml.Linq.XElement xElement = iEnumerator.Current;
			//				System.String organizationName = xElement.Attribute("Name").Value;
			//				System.String connectionString = xElement.Attribute("ConnectionString").Value;
			//				DataModel.organizationMap.Add(organizationName, new OrganizationTarget(connectionString));
			organizationLoop.Statements.Add(
				new CodeVariableDeclarationStatement(new CodeTypeReference(typeof(XElement)),
					"xElement",
					new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("iEnumerator"), "Current")));
			organizationLoop.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference(typeof(String)), "organizationName",
					new CodePropertyReferenceExpression(
						new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("xElement"), "Attribute", new CodePrimitiveExpression("Name")), "Value")));
			organizationLoop.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference(typeof(String)), "connectionString",
					new CodePropertyReferenceExpression(
						new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("xElement"), "Attribute", new CodePrimitiveExpression("ConnectionString")), "Value")));
			organizationLoop.Statements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "organizationMap"),
					"Add",
					new CodeVariableReferenceExpression("organizationName"),
					new CodeObjectCreateExpression(String.Format("Organization{0}", dataModelSchema.Name), new CodeVariableReferenceExpression("connectionString"))));

			//			}
			this.Statements.Add(organizationLoop);

			//		}

		}

	}

}
