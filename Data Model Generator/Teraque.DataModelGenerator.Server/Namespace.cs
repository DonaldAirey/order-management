namespace Teraque.DataModelGenerator
{

    using System.CodeDom;
    using System.Collections.Generic;

	/// <summary>
	/// The root namespace Teraque.DataModelGenerator the DataSet output file.
	/// </summary>
	public class Namespace : CodeNamespace
	{

		/// <summary>
		/// Creates a CodeDOM namespace Teraque.DataModelGenerator contains the strongly typed DataSet.
		/// </summary>
		/// <param name="schema">The schema description of the strongly typed DataSet.</param>
		public Namespace(DataModelSchema dataModelSchema)
		{

			//namespace Teraque.DataModelGenerator
			//{
			this.Name = dataModelSchema.TargetNamespace;

			// The interface provides the contracts for this service.
			this.Types.Add(new TargetInterface.TargetInterface(dataModelSchema));

			// Types
			List<CodeTypeDeclaration> typeList = new List<CodeTypeDeclaration>();
			typeList.Add(new TenantTargetClass.TenantTargetClass(dataModelSchema));
			typeList.Add(new FieldCollectorClass.FieldCollectorClass(dataModelSchema));
			typeList.Add(new TargetClass.TargetClass(dataModelSchema));
			typeList.Add(new TransactionClass.TransactionClass(dataModelSchema));
			typeList.Add(new TransactionLogItemClass.TransactionLogItemClass(dataModelSchema));
			typeList.Sort(
				delegate(CodeTypeDeclaration firstDeclaration, CodeTypeDeclaration secondDeclaration)
				{ return firstDeclaration.Name.CompareTo(secondDeclaration.Name); });
			foreach (CodeTypeDeclaration codeTypeDeclaration in typeList)
				this.Types.Add(codeTypeDeclaration);

			//}

		}

	}

}
