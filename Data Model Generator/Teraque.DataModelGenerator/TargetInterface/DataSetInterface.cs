namespace Teraque.DataModelGenerator.TargetInterface
{

	using System;
	using System.CodeDom;
    using System.Collections.Generic;
    using System.Reflection;
	using System.ServiceModel;

    /// <summary>
	/// Generates the CodeDOM for a strongly typed DataSet from a schema description.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class TargetInterface : CodeTypeDeclaration
	{

		/// <summary>
		/// Creates a strongly typed DataSet from a schema description.
		/// </summary>
		/// <param name="dataSetNamespace">The CodeDOM namespace Teraque.DataModelGenerator.TargetInterface contains this strongly typed DataSet.</param>
		public TargetInterface(DataModelSchema dataModelSchema)
		{

			//	/// <summary>
			//	/// Abstract interface to a thread-safe, multi-tiered DataModel.
			//	/// </summary>
			//	[System.ServiceModel.ServiceContractAttribute()]
			//	public interface IDataModel
			//	{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Abstract interface to a thread-safe, multi-tiered {0}.", dataModelSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(ServiceContractAttribute))));
			this.Name = String.Format("I{0}", dataModelSchema.Name);
			this.TypeAttributes = TypeAttributes.Public | TypeAttributes.Interface;
			this.IsPartial = true;

			// Methods
			List<CodeMemberMethod> methodList = new List<CodeMemberMethod>();
			methodList.Add(new ReadMethod(dataModelSchema));
			foreach (KeyValuePair<String, TableSchema> tablePair in dataModelSchema.Tables)
			{
				methodList.Add(new CreateMethod(tablePair.Value));
				methodList.Add(new DestroyMethod(tablePair.Value));
				methodList.Add(new StoreMethod(tablePair.Value));
				methodList.Add(new UpdateMethod(tablePair.Value));
			}
			methodList.Sort(delegate(CodeMemberMethod firstMethod, CodeMemberMethod secondMethod) { return firstMethod.Name.CompareTo(secondMethod.Name); });
			foreach (CodeMemberMethod codeMemberMethod in methodList)
				this.Members.Add(codeMemberMethod);

		}

	}

}
