namespace Teraque.DataModelGenerator
{

    using System.CodeDom;
	using System.ComponentModel;

	/// <summary>
	/// Represents a standard collection of custom attributes for properties.
	/// </summary>
	public class CodeCustomAttributesForProperties : CodeAttributeDeclarationCollection
	{

		/// <summary>
		/// Creates a standard collection of custom attributes for properties.
		/// </summary>
		public CodeCustomAttributesForProperties()
		{

			//        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//        [global::System.ComponentModel.BrowsableAttribute(false)]
            // HACK - Put this line back in for official releases
            //this.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(DebuggerNonUserCodeAttribute))));
			this.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(BrowsableAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression(false))));

		}

	}

}
