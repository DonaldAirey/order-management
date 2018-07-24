namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
    using System.CodeDom;
    using System.Collections.Generic;
	using System.ComponentModel;
    using System.Reflection;

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
			this.CustomAttributes.Add(new CodeGeneratedCodeAttribute(typeof(DataModelGenerator)));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(DesignerCategoryAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression("code"))));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(ToolboxItemAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression(true))));
			this.Name = dataModelSchema.Name;
			this.TypeAttributes = TypeAttributes.Public | TypeAttributes.Class;
			this.BaseTypes.Add(new CodeTypeReference(String.Format("I{0}", dataModelSchema.Name)));
			this.IsPartial = true;

			//	Private Instance Fields
			List<CodeMemberField> privateInstanceFieldList = new List<CodeMemberField>();
			privateInstanceFieldList.Add(new TenantMapField(dataModelSchema));
			privateInstanceFieldList.Sort((first, second) => { return first.Name.CompareTo(second.Name); });
			foreach (CodeMemberField codeMemberField in privateInstanceFieldList)
				this.Members.Add(codeMemberField);

			// Properties
			List<CodeMemberProperty> propertyList = new List<CodeMemberProperty>();
			propertyList.Add(new LockTimeoutProperty(dataModelSchema));
			propertyList.Add(new CurrentTransactionProperty(dataModelSchema));
			propertyList.Add(new TenantTargetProperty(dataModelSchema));
			propertyList.Add(new DataLockProperty(dataModelSchema));
			propertyList.Add(new RelationsProperty(dataModelSchema));
			foreach (KeyValuePair<String, TableSchema> keyValuePair in dataModelSchema.Tables)
				propertyList.Add(new TableProperty(keyValuePair.Value));
			propertyList.Add(new TablesProperty(dataModelSchema));
			propertyList.Sort((first, second) => { return first.Name.CompareTo(second.Name); });
			foreach (CodeMemberProperty codeMemberProperty in propertyList)
				this.Members.Add(codeMemberProperty);

			// Methods
			List<CodeMemberMethod> methodList = new List<CodeMemberMethod>();
			methodList.Add(new GetTenantsMethod(dataModelSchema));
			methodList.Add(new LoadTenantMethod(dataModelSchema));
			methodList.Add(new UnloadTenantMethod(dataModelSchema));
			methodList.Add(new ReadMethod(dataModelSchema));
			methodList.Add(new ReadXmlMethod(dataModelSchema));
			foreach (KeyValuePair<String, TableSchema> tablePair in dataModelSchema.Tables)
			{
				methodList.Add(new CreateMethod(tablePair.Value));
				methodList.Add(new DestroyMethod(tablePair.Value));
				methodList.Add(new StoreMethod(tablePair.Value));
				methodList.Add(new UpdateMethod(tablePair.Value));
			}
			methodList.Sort((first, second) => { return first.Name.CompareTo(second.Name); });
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
				foreach (KeyValuePair<String, ConstraintSchema> constraintPair in keyValuePair.Value.Constraints)
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
