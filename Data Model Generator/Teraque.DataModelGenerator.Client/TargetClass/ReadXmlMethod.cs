namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;

	/// <summary>
	/// Creates a method to read an XML file.
	/// </summary>
	class ReadXmlMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method to read an XML file.
		/// </summary>
		/// <param name="schema">The data model schema.</param>
		public ReadXmlMethod(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Reads an XML file into the data model.
			//		/// </summary>
			//		/// <param name="fileName">The name of the file to read.</param>
			//		public static void ReadXml(string fileName)
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Reads an XML file into the data model.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Comments.Add(new CodeCommentStatement("<param name=\"fileName\">The name of the file to read.</param>", true));
			this.Name = "ReadXml";
			this.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeGlobalTypeReference(typeof(String)), "fileName"));

			//			this.ReadXml(fileName);
			this.Statements.Add(
				new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "dataSet"), "ReadXml", new CodeArgumentReferenceExpression("fileName")));

			//		}

		}

	}
}
