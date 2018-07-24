namespace Teraque.DataModelGenerator
{

	using System;
	using System.CodeDom;
	using System.ServiceModel;

	/// <summary>
	/// Creates a general purpose 'Argment' fault.
	/// </summary>
	class CodeThrowTenantNotLoadedExceptionStatement : CodeThrowExceptionStatement
	{

		/// <summary>
		/// Creates a general purpose 'TenantNotLoaded' fault.
		/// </summary>
		/// <param name="tableSchema">The table where the error occured.</param>
		public CodeThrowTenantNotLoadedExceptionStatement(CodeExpression codeExpression)
		{

			//                throw new global::System.ServiceModel.FaultException<TenantNotLoadedFault>(new global::Teraque.TenantNotLoadedFault("TenantName"));
			this.ToThrow = new CodeObjectCreateExpression(
				new CodeGlobalTypeReference(typeof(FaultException<TenantNotLoadedFault>)),
				new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(TenantNotLoadedFault)), codeExpression));

		}

	}

}
