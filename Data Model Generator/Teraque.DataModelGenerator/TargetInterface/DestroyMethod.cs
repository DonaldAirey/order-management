namespace Teraque.DataModelGenerator.TargetInterface
{

	using System;
	using System.CodeDom;
	using System.Collections.Generic;
	using System.ServiceModel;

    /// <summary>
	/// Creates a method that loads records into the database from an external source.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class DestroyMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method that loads records into the database from an external source.
		/// </summary>
		/// <param name="tableSchema">The schema used to describe the table.</param>
		public DestroyMethod(TableSchema tableSchema)
		{

			// This shreds the list of parameters up into a metadata stucture that is helpful in extracting ordinary parameters 
			// from those that need to be found in other tables using external identifiers.
			DestroyExParameterMatrix destroyExParameterMatrix = new DestroyExParameterMatrix(tableSchema);

			//        public void CreateEmployee(int age, string configurationId, string departmentId, string employeeId, string marriageCode) {
			//			{
			string actionUri = String.Format("http://tempuri.org/I{0}/Destroy{1}", tableSchema.DataModel.Name, tableSchema.Name);
			string actionReplyUri = String.Format("http://tempuri.org/I{0}/Destroy{1}Response", tableSchema.DataModel.Name, tableSchema.Name);
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(OperationContractAttribute)), new CodeAttributeArgument("Action", new CodePrimitiveExpression(actionUri)), new CodeAttributeArgument("ReplyAction", new CodePrimitiveExpression(actionReplyUri))));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(TransactionFlowAttribute)), new CodeAttributeArgument(new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(TransactionFlowOption)), "Allowed"))));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(ServiceKnownTypeAttribute)), new CodeAttributeArgument(new CodeTypeOfExpression(new CodeGlobalTypeReference(typeof(DBNull))))));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(FaultContractAttribute)), new CodeAttributeArgument(new CodeTypeOfExpression(new CodeGlobalTypeReference(typeof(RecordNotFoundFault))))));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(FaultContractAttribute)), new CodeAttributeArgument(new CodeTypeOfExpression(new CodeGlobalTypeReference(typeof(IndexNotFoundFault))))));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(FaultContractAttribute)), new CodeAttributeArgument(new CodeTypeOfExpression(new CodeGlobalTypeReference(typeof(ArgumentFault))))));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(FaultContractAttribute)), new CodeAttributeArgument(new CodeTypeOfExpression(new CodeGlobalTypeReference(typeof(FormatFault))))));
			this.CustomAttributes.Add(
				new CodeAttributeDeclaration(
					new CodeGlobalTypeReference(typeof(FaultContractAttribute)),
					new CodeAttributeArgument(new CodeTypeOfExpression(new CodeGlobalTypeReference(typeof(TenantNotLoadedFault))))));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Abstract;
			this.Attributes = MemberAttributes.Public | MemberAttributes.Abstract;
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.Name = String.Format("Destroy{0}", tableSchema.Name);
			foreach (KeyValuePair<string, ExternalParameterItem> parameterPair in destroyExParameterMatrix.ExternalParameterItems)
				this.Parameters.Add(parameterPair.Value.CodeParameterDeclarationExpression);

		}

	}

}
