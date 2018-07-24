namespace Teraque.DataModelGenerator
{

	using System;
    using System.CodeDom;
	using System.ServiceModel;

	/// <summary>
	/// Creates a general purpose 'Argment' fault.
	/// </summary>
	class CodeThrowArgumentExceptionStatement : CodeThrowExceptionStatement
	{

		/// <summary>
		/// Creates a general purpose 'Argment' fault.
		/// </summary>
		/// <param name="tableSchema">The table where the error occured.</param>
		public CodeThrowArgumentExceptionStatement(CodeExpression codeExpression)
		{

			//                throw new global::System.ServiceModel.FaultException<ArgumentFault>(new global::Teraque.ArgumentFault("Attempt to access a Employee record ({0}) that doesn\'t exist", employeeIdKeyText));
			this.ToThrow = new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(FaultException<ArgumentFault>)),
				new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(ArgumentFault)), codeExpression));

		}

	}

}
