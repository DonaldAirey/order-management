namespace Teraque.DataModelGenerator
{

	using System;
	using System.CodeDom;
	using System.ServiceModel;

	/// <summary>
	/// Creates a general purpose 'Argment' fault.
	/// </summary>
	class CodeThrowFormatExceptionStatement : CodeThrowExceptionStatement
	{

		/// <summary>
		/// Creates a general purpose 'Argment' fault.
		/// </summary>
		/// <param name="tableSchema">The table where the error occured.</param>
		public CodeThrowFormatExceptionStatement(CodeExpression codeExpression)
		{

			//                throw new global::System.ServiceModel.FaultException<FormatFault>(new global::Teraque.FormatFault("Attempt to access a Employee record ({0}) that doesn\'t exist", employeeIdKeyText));
			this.ToThrow = new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(FaultException<FormatFault>)),
				new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(FormatFault)), codeExpression));

		}

	}

}
