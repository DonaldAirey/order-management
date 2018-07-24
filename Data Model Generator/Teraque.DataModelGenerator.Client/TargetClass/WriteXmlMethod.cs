namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;

	/// <summary>
	/// Creates a method to read an XML file.
	/// </summary>
	class WriteXmlMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method to read an XML file.
		/// </summary>
		/// <param name="schema">The data model schema.</param>
		public WriteXmlMethod(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Writes an XML file into the data model.
			//		/// </summary>
			//		/// <param name="fileName">The name of the file to read.</param>
			//		public static void WriteXml(string fileName)
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Writes an XML file to a file.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Comments.Add(new CodeCommentStatement("<param name=\"fileName\">The name of the file to write.</param>", true));
			this.Name = "WriteXml";
			this.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeGlobalTypeReference(typeof(String)), "fileName"));

			//			this.WriteXml(fileName);
			this.Statements.Add(
				new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "dataSet"), "WriteXml", new CodeArgumentReferenceExpression("fileName")));

			//		}

		}

	}
}
