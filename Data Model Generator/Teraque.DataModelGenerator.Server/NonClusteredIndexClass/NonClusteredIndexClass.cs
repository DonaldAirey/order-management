namespace Teraque.DataModelGenerator.NonClusteredIndexClass
{

	using System;
	using System.CodeDom;
    using System.Reflection;
	using System.ComponentModel;

    /// <summary>
	/// Creates a CodeDOM description of a strongly typed index.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class NonClusteredIndexClass : CodeTypeDeclaration
	{

		/// <summary>
		/// Creates a CodeDOM description of a strongly typed index.
		/// </summary>
		/// <param name="constraintSchema">A description of a unique constraint.</param>
		public NonClusteredIndexClass(UniqueConstraintSchema uniqueConstraintSchema)
		{

			//    /// <summary>
			//    /// Represents a means of identifying a Gender row using a set of columns in which all values must be unique.
			//    /// </summary>
			//    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Teraque.DataModelGenerator.Server", "1.3.1.0")]
			//    [global::System.ComponentModel.DesignerCategoryAttribute("code")]
			//    [global::System.ComponentModel.ToolboxItemAttribute(true)]
			//    public class GenderKeyIndex : global::Teraque.NonClusteredIndex, IGenderIndex {
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Represents a means of identifying a {0} row using a set of columns in which all values must be unique.", uniqueConstraintSchema.Table.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.Add(new CodeGeneratedCodeAttribute(typeof(DataModelGenerator)));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(DesignerCategoryAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression("code"))));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(ToolboxItemAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression(true))));
			this.Name = String.Format("{0}Index", uniqueConstraintSchema.Name);
			this.TypeAttributes = TypeAttributes.Public | TypeAttributes.Class;
			this.BaseTypes.Add(new CodeGlobalTypeReference(typeof(NonClusteredIndex)));
			this.BaseTypes.Add(new CodeTypeReference(String.Format("I{0}Index", uniqueConstraintSchema.Table.Name)));

			// Constructors
			this.Members.Add(new Constructor(uniqueConstraintSchema));

			// Methods
			this.Members.Add(new FindByMethod(uniqueConstraintSchema));
			this.Members.Add(new FindByKeyMethod(uniqueConstraintSchema));

			//		}

		}

	}

}
