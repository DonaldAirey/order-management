namespace Teraque.DataModelGenerator.TransactionClass
{

    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Transactions;

	/// <summary>
	/// Creates a CodeDOM description of a strongly typed Table.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class TransactionClass : CodeTypeDeclaration
	{

		/// <summary>
		/// Create a CodeDOM description of a strongly typed Table.
		/// </summary>
		/// <param name="tableSchema">The schema that describes the table.</param>
		public TransactionClass(DataModelSchema dataModelSchema)
		{

			//	/// <summary>
			//	/// A transaction for the DataModel.
			//	/// </summary>
			//	public class DataModelTransaction : global::System.Transactions.IEnlistmentNotification
			//	{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(
				new CodeCommentStatement(
					String.Format("A transaction to add or reject a group of changes to the {0} as a single unit of work.", dataModelSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.Add(new CodeGeneratedCodeAttribute(typeof(DataModelGenerator)));
			this.CustomAttributes.Add(
				new CodeAttributeDeclaration(
					new CodeGlobalTypeReference(typeof(DesignerCategoryAttribute)),
					new CodeAttributeArgument(new CodePrimitiveExpression("code"))));
			this.Name = String.Format("{0}Transaction", dataModelSchema.Name);
			this.TypeAttributes = TypeAttributes.Public | TypeAttributes.Class;
			this.BaseTypes.Add(new CodeGlobalTypeReference(typeof(IEnlistmentNotification)));
			this.BaseTypes.Add(new CodeGlobalTypeReference(typeof(IDataModelTransaction)));

			// Private Instance Fields
			List<CodeMemberField> fieldList = new List<CodeMemberField>();
			fieldList.Add(new CountdownEventField());
			fieldList.Add(new LockListField());
			fieldList.Add(new TenantTargetField(dataModelSchema));
			fieldList.Add(new RecordListField());
			fieldList.Add(new SqlConnectionField());
			fieldList.Add(new TransactionIdField());
			fieldList.Sort(delegate(CodeMemberField firstField, CodeMemberField secondField) { return firstField.Name.CompareTo(secondField.Name); });
			foreach (CodeMemberField codeMemberField in fieldList)
				this.Members.Add(codeMemberField);

			// Constructors
			this.Members.Add(new VoidConstructor(dataModelSchema));

			// Properties
			List<CodeMemberProperty> propertyList = new List<CodeMemberProperty>();
			propertyList.Add(new TenantTargetProperty(dataModelSchema));
			propertyList.Add(new SqlConnectionProperty());
			propertyList.Add(new TransactionIdProperty());
			propertyList.Sort(delegate(CodeMemberProperty firstProperty, CodeMemberProperty secondProperty) { return firstProperty.Name.CompareTo(secondProperty.Name); });
			foreach (CodeMemberProperty codeMemberProperty in propertyList)
				this.Members.Add(codeMemberProperty);

			// Methods
			List<CodeMemberMethod> methodList = new List<CodeMemberMethod>();
			methodList.Add(new AddLockMethod(dataModelSchema));
			methodList.Add(new AddRecordMethod(dataModelSchema));
			methodList.Add(new CommitMethod(dataModelSchema));
			methodList.Add(new ExecuteMethod(dataModelSchema));
			methodList.Add(new HandleQueryCallbackMethod(dataModelSchema));
			methodList.Add(new InDoubtMethod(dataModelSchema));
			methodList.Add(new PrepareMethod(dataModelSchema));
			methodList.Add(new RollbackMethod(dataModelSchema));
			methodList.Sort(delegate(CodeMemberMethod firstMethod, CodeMemberMethod secondMethod) { return firstMethod.Name.CompareTo(secondMethod.Name); });
			foreach (CodeMemberMethod codeMemberMethod in methodList)
				this.Members.Add(codeMemberMethod);

			//        }

		}

	}

}
