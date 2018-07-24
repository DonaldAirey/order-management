namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;
    using System.Collections.Generic;
	using System.ComponentModel;
    using System.Reflection;
    using Teraque.DataModelGenerator.TargetClass;
	using Teraque.DataModelGenerator.TargetClass.MergeState;

    /// <summary>
	/// Generates the CodeDOM for a strongly typed DataSet from a schema description.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class TargetClass : CodeTypeDeclaration
	{

		/// <summary>
		/// Creates a strongly typed DataSet from a schema description.
		/// </summary>
		/// <param name="dataSetNamespace">The CodeDOM namespace Teraque.DataModelGenerator.TargetClass
		public TargetClass(DataModelSchema dataModelSchema)
		{

			//    /// <summary>
			//    /// A thread-safe, multi-tiered DataModel.
			//    /// </summary>
			//    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Teraque.DataModelGenerator.Server", "1.3.1.0")]
			//    [global::System.ComponentModel.DesignerCategoryAttribute("code")]
			//    [global::System.ComponentModel.ToolboxItemAttribute(true)]
			//    public class DataModel : Teraque.UnitTest.Server.IDataModel {
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("A thread-safe, multi-tiered {0}.", dataModelSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.Add(new CodeGeneratedCodeAttribute(typeof(ClientGenerator)));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(DesignerCategoryAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression("code"))));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(ToolboxItemAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression(true))));
			this.Name = dataModelSchema.Name;
			this.TypeAttributes = TypeAttributes.Public | TypeAttributes.Class;
			this.IsPartial = true;

			// Private Constants
			List<CodeMemberField> constantList = new List<CodeMemberField>();
			constantList.Add(new DeletedTimeColumnField());
			constantList.Add(new PrimaryKeyOffsetField());
			constantList.Add(new RefreshIntervalField());
			constantList.Add(new RowDataIndexField());
			constantList.Add(new RowStateIndexField());
			constantList.Add(new RowVersionColumnField());
			constantList.Add(new TableRowsIndexField());
			constantList.Add(new TableTableNameIndexField());
			constantList.Add(new ThreadWaitTimeField());
			constantList.Sort(delegate(CodeMemberField firstField, CodeMemberField secondField) { return firstField.Name.CompareTo(secondField.Name); });
			foreach (CodeMemberField codeMemberField in constantList)
				this.Members.Add(codeMemberField);

			// Classes
			List<CodeTypeDeclaration> classItemList = new List<CodeTypeDeclaration>();
			classItemList.Add(new MergeStateClass(dataModelSchema));
			classItemList.Sort(delegate(CodeTypeDeclaration firstField, CodeTypeDeclaration secondField) { return firstField.Name.CompareTo(secondField.Name); });
			foreach (CodeTypeDeclaration classItem in classItemList)
				this.Members.Add(classItem);

			// Private Instance Fields
			List<CodeMemberField> fieldList = new List<CodeMemberField>();
			fieldList.Add(new BatchSizeField());
			fieldList.Add(new DataSetField());
			fieldList.Add(new DataSetIdField());
			fieldList.Add(new IsReadingField());
			fieldList.Add(new MergeStateQueueField());
			fieldList.Add(new ReadDataModelThreadField());
			fieldList.Add(new SequenceField());
			fieldList.Add(new SyncRootField());
			fieldList.Add(new TenantNotLoadedField());
			fieldList.Add(new UpdateBufferMutexField());
			fieldList.Add(new SyncUpdateField());
			foreach (KeyValuePair<String, TableSchema> keyValuePair in dataModelSchema.Tables)
				fieldList.Add(new TableField(keyValuePair.Value));
			foreach (KeyValuePair<String, RelationSchema> relationPair in dataModelSchema.Relations)
				fieldList.Add(new RelationField(relationPair.Value));
			fieldList.Sort(delegate(CodeMemberField firstField, CodeMemberField secondField) { return firstField.Name.CompareTo(secondField.Name); });
			foreach (CodeMemberField codeMemberField in fieldList)
				this.Members.Add(codeMemberField);

			// Constructors
			this.Members.Add(new StaticConstructor(dataModelSchema));

			// Properties
			List<CodeMemberProperty> propertyList = new List<CodeMemberProperty>();
			propertyList.Add(new IsReconcilingProperty(dataModelSchema));
			propertyList.Add(new SyncRootProperty(dataModelSchema));
			propertyList.Add(new RelationsProperty(dataModelSchema));
			propertyList.Add(new TablesProperty(dataModelSchema));
			foreach (KeyValuePair<String, TableSchema> keyValuePair in dataModelSchema.Tables)
				propertyList.Add(new TableProperty(keyValuePair.Value));
			propertyList.Sort(delegate(CodeMemberProperty firstProperty, CodeMemberProperty secondProperty) { return firstProperty.Name.CompareTo(secondProperty.Name); });
			foreach (CodeMemberProperty codeMemberProperty in propertyList)
				this.Members.Add(codeMemberProperty);

			// Methods
			List<CodeMemberMethod> methodList = new List<CodeMemberMethod>();
			methodList.Add(new MergeDataModelMethod(dataModelSchema));
			methodList.Add(new ReadDataModelMethod(dataModelSchema));
			methodList.Add(new StartMergeMethod(dataModelSchema));
			methodList.Add(new ReadXmlMethod(dataModelSchema));
			methodList.Add(new ResetMethod(dataModelSchema));
			methodList.Add(new WriteXmlMethod(dataModelSchema));
			methodList.Sort(delegate(CodeMemberMethod firstMethod, CodeMemberMethod secondMethod) { return firstMethod.Name.CompareTo(secondMethod.Name); });
			foreach (CodeMemberMethod codeMemberMethod in methodList)
				this.Members.Add(codeMemberMethod);

			// Delegates
			foreach (KeyValuePair<String, TableSchema> keyValuePair in dataModelSchema.Tables)
				this.Members.Add(new RowChangeDelegate(keyValuePair.Value));

			// Tables
			foreach (KeyValuePair<String, TableSchema> keyValuePair in dataModelSchema.Tables)
				this.Members.Add(new TableClass.TableClass(keyValuePair.Value));

			// Create a type for each of the rows.
			foreach (KeyValuePair<String, TableSchema> keyValuePair in dataModelSchema.Tables)
				this.Members.Add(new RowClass.RowClass(keyValuePair.Value));

			// Create a strongly typed version of the ChangeEventArgs class for each table.
			foreach (KeyValuePair<String, TableSchema> keyValuePair in dataModelSchema.Tables)
				this.Members.Add(new ChangeEventArgsClass.ChangeEventArgsClass(keyValuePair.Value));

			// Create the strongly typed indices.
			foreach (KeyValuePair<String, TableSchema> keyValuePair in dataModelSchema.Tables)
			{
				this.Members.Add(new Teraque.DataModelGenerator.IndexInterface.IndexInterface(keyValuePair.Value));
				foreach (KeyValuePair<string, ConstraintSchema> constraintPair in keyValuePair.Value.Constraints)
					if (constraintPair.Value is UniqueConstraintSchema)
					{
						UniqueConstraintSchema uniqueConstraintSchema = constraintPair.Value as UniqueConstraintSchema;
						if (uniqueConstraintSchema.IsPrimaryKey)
							this.Members.Add(new ClusteredIndexClass.ClusteredIndexClass(uniqueConstraintSchema));
						else
							this.Members.Add(new NonClusteredIndexClass.NonClusteredIndexClass(uniqueConstraintSchema));
					}
			}

		}

	}

}
