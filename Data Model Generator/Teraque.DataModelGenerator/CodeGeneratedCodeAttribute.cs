namespace Teraque.DataModelGenerator
{

	using System;
    using System.CodeDom;
	using System.CodeDom.Compiler;

    /// <summary>
	/// Generates a 'GeneratedCodeAttribute' describing the code generator used to create the code.
	/// </summary>
	public class CodeGeneratedCodeAttribute : CodeAttributeDeclaration
	{

		/// <summary>
		/// Generates a 'GeneratedCodeAttribute' describing the code generator used to create the code.
		/// </summary>
		public CodeGeneratedCodeAttribute(Type type)
			: base(new CodeGlobalTypeReference(typeof(GeneratedCodeAttribute)))
		{

			//        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Teraque.DataModelGenerator.Server", "1.3.1.0")]
			this.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(type.FullName)));
			this.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(type.Assembly.GetName().Version.ToString())));

		}

	}
}
