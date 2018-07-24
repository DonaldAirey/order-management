namespace Teraque.DataModelGenerator.TargetInterface
{

	using System;
	using System.CodeDom;
    using System.ServiceModel;

    /// <summary>
	/// Creates a method to handle moving the deleted records from the active data model to the deleted data model.
	/// </summary>
	class ReadMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method to handle moving the deleted records from the active data model to the deleted data model.
		/// </summary>
		/// <param name="schema">A description of the data model.</param>
		public ReadMethod(DataModelSchema dataModelSchema)
		{

			//	[global::System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataModel/Read", ReplyAction="http://tempuri.org/IDataModel/ReadResponse")]
			//	[global::System.ServiceModel.TransactionFlowAttribute(global::System.ServiceModel.TransactionFlowOption.NotAllowed)]
			//	[global::System.ServiceModel.ServiceKnownTypeAttribute(typeof(DBNull))]
			//	[global::System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
			//	object[] Read(global::System.Guid dataSetId, long sequence);
			this.CustomAttributes.Add(
				new CodeAttributeDeclaration(
					new CodeGlobalTypeReference(typeof(OperationContractAttribute)),
					new CodeAttributeArgument("Action", new CodePrimitiveExpression(String.Format("http://tempuri.org/I{0}/Read", dataModelSchema.Name))),
					new CodeAttributeArgument(
						"ReplyAction",
						new CodePrimitiveExpression(String.Format("http://tempuri.org/I{0}/ReadResponse", dataModelSchema.Name)))));
			this.CustomAttributes.Add(
				new CodeAttributeDeclaration(
					new CodeGlobalTypeReference(typeof(TransactionFlowAttribute)),
					new CodeAttributeArgument(
						new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(TransactionFlowOption)), "NotAllowed"))));
			this.CustomAttributes.Add(
				new CodeAttributeDeclaration(
					new CodeGlobalTypeReference(typeof(ServiceKnownTypeAttribute)), new CodeAttributeArgument(new CodeTypeOfExpression(typeof(DBNull)))));
			this.CustomAttributes.Add(
				new CodeAttributeDeclaration(
					new CodeGlobalTypeReference(typeof(ServiceKnownTypeAttribute)), new CodeAttributeArgument(new CodeTypeOfExpression(typeof(Guid)))));
			this.CustomAttributes.Add(
				new CodeAttributeDeclaration(
					new CodeGlobalTypeReference(typeof(ServiceKnownTypeAttribute)), new CodeAttributeArgument(new CodeTypeOfExpression(typeof(Object[])))));
			this.CustomAttributes.Add(
				new CodeAttributeDeclaration(
					new CodeGlobalTypeReference(typeof(FaultContractAttribute)),
					new CodeAttributeArgument(new CodeTypeOfExpression(new CodeGlobalTypeReference(typeof(TenantNotLoadedFault))))));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Abstract;
			this.ReturnType = new CodeGlobalTypeReference(typeof(Object[]));
			this.Name = "Read";
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeGlobalTypeReference(typeof(Guid)), "dataSetId"));
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeGlobalTypeReference(typeof(Int64)), "sequence"));

		}

	}

}
