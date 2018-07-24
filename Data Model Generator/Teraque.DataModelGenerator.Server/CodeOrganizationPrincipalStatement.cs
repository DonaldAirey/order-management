namespace Teraque.DataModelGenerator
{

	using System;
	using System.CodeDom;
	using System.Threading;

	/// <summary>
	/// Creates a variable that holds the OrganizationPrincipal of the current user.
	/// </summary>
	class CodeOrganizationPrincipalExpression : CodeVariableDeclarationStatement
	{

		/// <summary>
		/// A statement creating an tenant principal.
		/// </summary>
		/// <param name="organizationPrincipal">The organizationPrincipal variable.</param>
		public CodeOrganizationPrincipalExpression(CodeVariableReferenceExpression organizationPrincipal)
		{

			//			global::Teraque.OrganizationPrincipal l1651 = ((Teraque.OrganizationPrincipal)(global::System.Threading.Thread.CurrentPrincipal));
			this.Type = new CodeGlobalTypeReference(typeof(OrganizationPrincipal));
			this.Name = organizationPrincipal.VariableName;
			this.InitExpression = new CodeCastExpression(typeof(OrganizationPrincipal), new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(Thread)), "CurrentPrincipal"));

		}

	}

}
