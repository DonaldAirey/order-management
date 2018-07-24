namespace Teraque.DataModelGenerator.FieldCollectorClass
{

    using System.CodeDom;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
	/// Creates a CodeDOM description of a class used to collect fields during the transaction log compression.
	/// </summary>
	/// <copyright>Copyright � 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class FieldCollectorClass : CodeTypeDeclaration
	{

		/// <summary>
		/// Creates a CodeDOM description of a class used to collect fields during the transaction log compression.
		/// </summary>
		/// <param name="tableSchema">The schema that describes the table.</param>
		public FieldCollectorClass(DataModelSchema dataModelSchema)
		{

			//	/// <summary>
			//	/// Collects fields during the transaction log compression.
			//	/// </summary>
			//	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Teraque.DataModelGenerator.Server", "1.3.1.0")]
			//	[global::System.ComponentModel.DesignerCategoryAttribute("code")]
			//	public class FieldCollector
			//	{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Collects fields during the transaction log compression.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.Add(new CodeGeneratedCodeAttribute(typeof(DataModelGenerator)));
			this.CustomAttributes.Add(
				new CodeAttributeDeclaration(
					new CodeGlobalTypeReference(typeof(DesignerCategoryAttribute)),
					new CodeAttributeArgument(new CodePrimitiveExpression("code"))));
			this.Name = "FieldCollector";
			this.TypeAttributes = TypeAttributes.Public | TypeAttributes.Class;

			// Private Instance Fields
			List<CodeMemberField> fieldList = new List<CodeMemberField>();
			fieldList.Add(new LinkedListNodeField());
			fieldList.Add(new FieldTableField());
			fieldList.Sort(delegate(CodeMemberField firstField, CodeMemberField secondField) { return firstField.Name.CompareTo(secondField.Name); });
			foreach (CodeMemberField codeMemberField in fieldList)
				this.Members.Add(codeMemberField);

			// Constructors
			this.Members.Add(new Constructor(dataModelSchema));

			//        }

		}

	}

}
