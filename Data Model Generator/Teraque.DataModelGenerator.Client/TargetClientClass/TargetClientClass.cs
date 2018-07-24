namespace Teraque.DataModelGenerator.TargetClientClass
{

	using System;
	using System.CodeDom;
    using System.Collections.Generic;
    using System.Reflection;
    using Teraque;

    /// <summary>
	/// Generates the CodeDOM for a strongly typed DataSet from a schema description.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class TargetClientClass : CodeTypeDeclaration
	{

		/// <summary>
		/// Creates a strongly typed DataSet from a schema description.
		/// </summary>
		/// <param name="dataSetNamespace">The CodeDOM namespace Teraque.DataModelGenerator.TargetClientClass contains this strongly typed DataSet.</param>
		public TargetClientClass(DataModelSchema dataModelSchema)
		{

			//	[System.Diagnostics.DebuggerStepThroughAttribute()]
			//	public partial class DataModelClient : System.ServiceModel.ClientBase<IDataModel>, IDataModel
			//	{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Client interface to a shared {0}.", dataModelSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.IsPartial = true;
			this.TypeAttributes = TypeAttributes.Public | TypeAttributes.Class;
			this.Name = String.Format("{0}Client", dataModelSchema.Name);
			this.BaseTypes.Add(new CodeTypeReference(String.Format("global::System.ServiceModel.ClientBase<I{0}>", dataModelSchema.Name)));
			this.BaseTypes.Add(new CodeTypeReference(String.Format("I{0}", dataModelSchema.Name)));

			// Constructors
			this.Members.Add(new VoidConstructor(dataModelSchema));
			this.Members.Add(new StringConstructor(dataModelSchema));
	
			// Methods
			List<CodeMemberMethod> methodList = new List<CodeMemberMethod>();
			methodList.Add(new ReadMethod(dataModelSchema));
			foreach (KeyValuePair<string, TableSchema> tablePair in dataModelSchema.Tables)
			{
				methodList.Add(new CreateMethod(tablePair.Value));
				methodList.Add(new UpdateMethod(tablePair.Value));
				methodList.Add(new DestroyMethod(tablePair.Value));
				methodList.Add(new StoreMethod(tablePair.Value));
			}
			methodList.Sort(delegate(CodeMemberMethod firstMethod, CodeMemberMethod secondMethod) { return firstMethod.Name.CompareTo(secondMethod.Name); });
			foreach (CodeMemberMethod codeMemberMethod in methodList)
				this.Members.Add(codeMemberMethod);

		}

	}

}
