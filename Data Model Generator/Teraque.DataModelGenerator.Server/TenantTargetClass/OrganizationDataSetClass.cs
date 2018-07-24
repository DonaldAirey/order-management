namespace Teraque.DataModelGenerator.TenantTargetClass
{

	using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Reflection;

    /// <summary>
	/// Creates a CodeDOM description of a strongly typed Table.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class TenantTargetClass : CodeTypeDeclaration
	{

		/// <summary>
		/// Create a CodeDOM description of a strongly typed Table.
		/// </summary>
		/// <param name="tableSchema">The schema that describes the table.</param>
		public TenantTargetClass(DataModelSchema dataModelSchema)
		{

			//	/// <summary>
			//	/// A thread-safe DataSet able to handle transactions.
			//	/// </summary>
			//	public class DataModelDataSet : global::System.Data.DataSet
			//	{
			//
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("A thread-safe DataSet able to handle transactions.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.Add(new CodeGeneratedCodeAttribute(typeof(DataModelGenerator)));
			this.CustomAttributes.Add(
				new CodeAttributeDeclaration(
					new CodeGlobalTypeReference(typeof(DesignerCategoryAttribute)),
					new CodeAttributeArgument(new CodePrimitiveExpression("code"))));
			this.Name = String.Format("Tenant{0}", dataModelSchema.Name);
			this.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
			this.IsPartial = false;
			this.BaseTypes.Add(new CodeGlobalTypeReference(typeof(DataSet)));

			// Private Instance Fields
			List<CodeMemberField> privateInstanceFieldList = new List<CodeMemberField>();
			privateInstanceFieldList.Add(new ConnectionStringField());
			privateInstanceFieldList.Add(new CompactTimeField());
			privateInstanceFieldList.Add(new CompressorThreadField());
			privateInstanceFieldList.Add(new DataLockField());
			privateInstanceFieldList.Add(new LogCompressionInvervalField());
			privateInstanceFieldList.Add(new IdentifierField());
			privateInstanceFieldList.Add(new LockTimeoutField());
			privateInstanceFieldList.Add(new MasterRowVersionField());
			foreach (KeyValuePair<string, RelationSchema> relationPair in dataModelSchema.Relations)
				privateInstanceFieldList.Add(new RelationField(relationPair.Value));
			privateInstanceFieldList.Add(new SequenceField());
			privateInstanceFieldList.Add(new SyncRootField());
			foreach (KeyValuePair<string, TableSchema> keyValuePair in dataModelSchema.Tables)
				privateInstanceFieldList.Add(new TableField(keyValuePair.Value));
			privateInstanceFieldList.Add(new ThreadWaitTimeField());
			privateInstanceFieldList.Add(new TransactionLogField());
			privateInstanceFieldList.Add(new TransactionLogBatchSizeField());
			privateInstanceFieldList.Add(new TransactionLogItemAgeField());
			privateInstanceFieldList.Add(new TransactionLogLockField());
			privateInstanceFieldList.Add(new TransactionTableField(dataModelSchema));
			privateInstanceFieldList.Sort((first, second) => { return first.Name.CompareTo(second.Name); });
			foreach (CodeMemberField codeMemberField in privateInstanceFieldList)
				this.Members.Add(codeMemberField);

			// Constructors
			this.Members.Add(new VoidConstructor(dataModelSchema));

			// Properties
			List<CodeMemberProperty> propertyList = new List<CodeMemberProperty>();
			propertyList.Add(new CurrentTransactionProperty(dataModelSchema));
			propertyList.Sort((first, second) => { return first.Name.CompareTo(second.Name); });
			foreach (CodeMemberProperty codeMemberProperty in propertyList)
				this.Members.Add(codeMemberProperty);

			// Methods
			List<CodeMemberMethod> methodList = new List<CodeMemberMethod>();
			methodList.Add(new AddTransactionMethod(dataModelSchema));
			methodList.Add(new DisposeMethod(dataModelSchema));
			methodList.Add(new CompressLogMethod(dataModelSchema));
			methodList.Add(new IncrementRowVersionMethod(dataModelSchema));
			methodList.Add(new LoadDataMethod(dataModelSchema));
			methodList.Add(new OnTransactionCompletedMethod(dataModelSchema));
			methodList.Add(new ReadMethod(dataModelSchema));
			methodList.Add(new SequenceRecordMethod(dataModelSchema));
			foreach (KeyValuePair<String, TableSchema> tablePair in dataModelSchema.Tables)
			{
				methodList.Add(new CreateMethod(tablePair.Value));
				methodList.Add(new DestroyMethod(tablePair.Value));
				methodList.Add(new UpdateMethod(tablePair.Value));
			}
			methodList.Sort(delegate(CodeMemberMethod firstMethod, CodeMemberMethod secondMethod) { return firstMethod.Name.CompareTo(secondMethod.Name); });
			foreach (CodeMemberMethod codeMemberMethod in methodList)
				this.Members.Add(codeMemberMethod);

			//        }

			this.IsPartial = true;
		}

	}

}
