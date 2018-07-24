namespace Teraque.DataModelGenerator.TargetInterface
{

	using System;
	using System.CodeDom;
	using System.Collections.Generic;
	using System.ServiceModel;

    /// <summary>
	/// Creates a method that loads records into the database from an external source.
	/// </summary>
	class UpdateMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method that loads records into the database from an external source.
		/// </summary>
		/// <param name="tableSchema">The schema used to describe the table.</param>
		public UpdateMethod(TableSchema tableSchema)
		{

			// This shreds the list of parameters up into a metadata stucture that is helpful in extracting ordinary parameters 
			// from those that need to be found in other tables using external identifiers.
			UpdateExParameterMatrix updateParameterMatrix = new UpdateExParameterMatrix(tableSchema);

			// This collects a distinct list of data types that are added to the contract.
			SortedList<string, Type> knownTypes = new SortedList<string, Type>();
			foreach (KeyValuePair<string, ExternalParameterItem> externalParameterPair in updateParameterMatrix.ExternalParameterItems)
			{
				if (externalParameterPair.Value.DeclaredDataType == typeof(Object))
					if (!knownTypes.ContainsKey(typeof(DBNull).Name))
						knownTypes.Add(typeof(DBNull).Name, typeof(DBNull));
				if (externalParameterPair.Value.ActualDataType != externalParameterPair.Value.DeclaredDataType)
					if (!externalParameterPair.Value.ActualDataType.IsPrimitive)
						if (!knownTypes.ContainsKey(externalParameterPair.Value.ActualDataType.Name))
							knownTypes.Add(externalParameterPair.Value.ActualDataType.Name, externalParameterPair.Value.ActualDataType);
			}

			//        public void CreateEmployee(int age, string configurationId, string departmentId, string employeeId, string marriageCode) {
			//			{
			string actionUri = String.Format("http://tempuri.org/I{0}/Update{1}", tableSchema.DataModel.Name, tableSchema.Name);
			string actionReplyUri = String.Format("http://tempuri.org/I{0}/Update{1}Response", tableSchema.DataModel.Name, tableSchema.Name);
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(OperationContractAttribute)), new CodeAttributeArgument("Action", new CodePrimitiveExpression(actionUri)), new CodeAttributeArgument("ReplyAction", new CodePrimitiveExpression(actionReplyUri))));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(TransactionFlowAttribute)), new CodeAttributeArgument(new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(TransactionFlowOption)), "Allowed"))));
			foreach (KeyValuePair<string, Type> knownTypePair in knownTypes)
						this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(ServiceKnownTypeAttribute)), new CodeAttributeArgument(new CodeTypeOfExpression(new CodeGlobalTypeReference(knownTypePair.Value)))));
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
			this.Name = String.Format("Update{0}", tableSchema.Name);
			foreach (KeyValuePair<string, ExternalParameterItem> parameterPair in updateParameterMatrix.ExternalParameterItems)
				this.Parameters.Add(parameterPair.Value.CodeParameterDeclarationExpression);

		}

	}

}
