namespace Teraque.DataModelGenerator.TableClass
{

	using System;
	using System.CodeDom;
    using System.Collections.Generic;
	using System.ComponentModel;
    using System.Reflection;

	/// <summary>
	/// Creates a CodeDOM description of a strongly typed Table.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class TableClass : CodeTypeDeclaration
	{

		/// <summary>
		/// Create a CodeDOM description of a strongly typed Table.
		/// </summary>
		/// <param name="tableSchema">The schema that describes the table.</param>
		public TableClass(TableSchema tableSchema)
		{

			//        /// <summary>
			//        /// The Employee table.
			//        /// </summary>
			//        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Teraque.DataModelGenerator.Server", "1.3.1.0")]
			//        [global::System.ComponentModel.DesignerCategoryAttribute("code")]
			//        [global::System.ComponentModel.ToolboxItemAttribute(true)]
			//        public class EmployeeDataTable : global::System.Data.TypedTableBase<EmployeeRow>, global::ITable {
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("The {0} table.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.Add(new CodeGeneratedCodeAttribute(typeof(DataModelGenerator)));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(DesignerCategoryAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression("code"))));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(ToolboxItemAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression(true))));
			this.Name = String.Format("{0}DataTable", tableSchema.Name);
			this.TypeAttributes = TypeAttributes.Public | TypeAttributes.Class;
			this.BaseTypes.Add(String.Format("global::System.Data.TypedTableBase<{0}Row>", tableSchema.Name));
			this.BaseTypes.Add(typeof(Teraque.ITable));
			this.IsPartial = true;

			// Private Instance Fields
			List<CodeMemberField> privateInstanceFieldList = new List<CodeMemberField>();
			privateInstanceFieldList.Add(new OrdinalField(tableSchema));
			foreach (KeyValuePair<String, ColumnSchema> columnPair in tableSchema.Columns)
				privateInstanceFieldList.Add(new ColumnField(columnPair.Value));
			foreach (KeyValuePair<String, RelationSchema> relationPair in tableSchema.ParentRelations)
				privateInstanceFieldList.Add(new ParentRelationField(relationPair.Value));
			foreach (KeyValuePair<String, RelationSchema> relationPair in tableSchema.ChildRelations)
				privateInstanceFieldList.Add(new ChildRelationField(relationPair.Value));
			foreach (KeyValuePair<String, ConstraintSchema> constraintPair in tableSchema.Constraints)
				if (constraintPair.Value is UniqueConstraintSchema)
					privateInstanceFieldList.Add(new DataIndexField(constraintPair.Value as UniqueConstraintSchema));
			privateInstanceFieldList.Sort(delegate(CodeMemberField firstField, CodeMemberField secondField) { return firstField.Name.CompareTo(secondField.Name); });
			foreach (CodeMemberField codeMemberField in privateInstanceFieldList)
				this.Members.Add(codeMemberField);

			// Internal Instance Fields
			this.Members.Add(new IndicesField());

			// Constructors
			this.Members.Add(new VoidConstructor(tableSchema));

			// Properties
			List<CodeMemberProperty> propertyList = new List<CodeMemberProperty>();
			propertyList.Add(new CountProperty(tableSchema));
			propertyList.Add(new ItemInt32Property(tableSchema));
			propertyList.Add(new OrdinalProperty(tableSchema));
			foreach (KeyValuePair<String, ColumnSchema> columnPair in tableSchema.Columns)
				propertyList.Add(new ColumnProperty(tableSchema, columnPair.Value));
			foreach (KeyValuePair<String, RelationSchema> relationPair in tableSchema.ParentRelations)
				propertyList.Add(new ParentRelationProperty(relationPair.Value));
			foreach (KeyValuePair<String, RelationSchema> relationPair in tableSchema.ChildRelations)
				propertyList.Add(new ChildRelationProperty(relationPair.Value));
			foreach (KeyValuePair<String, ConstraintSchema> constraintPair in tableSchema.Constraints)
				if (constraintPair.Value is UniqueConstraintSchema)
					propertyList.Add(new DataIndexProperty(constraintPair.Value as UniqueConstraintSchema));
			propertyList.Sort(delegate(CodeMemberProperty firstProperty, CodeMemberProperty secondProperty) { return firstProperty.Name.CompareTo(secondProperty.Name); });
			foreach (CodeMemberProperty codeMemberProperty in propertyList)
				this.Members.Add(codeMemberProperty);

			// Events
			List<CodeMemberEvent> publicEventList = new List<CodeMemberEvent>();
			publicEventList.Add(new TableRowChangedEvent(tableSchema));
			publicEventList.Add(new TableRowChangingEvent(tableSchema));
			publicEventList.Add(new TableRowDeletedEvent(tableSchema));
			publicEventList.Add(new TableRowDeletingEvent(tableSchema));
			publicEventList.Add(new TableRowValidateEvent(tableSchema));
			publicEventList.Sort(delegate(CodeMemberEvent firstEvent, CodeMemberEvent secondEvent) { return firstEvent.Name.CompareTo(secondEvent.Name); });
			foreach (CodeMemberEvent codeMemberEvent in publicEventList)
				this.Members.Add(codeMemberEvent);

			// Methods
			List<CodeMemberMethod> methodList = new List<CodeMemberMethod>();
			methodList.Add(new InitializeRelationsMethod(tableSchema));
			methodList.Add(new GetRowTypeMethod(tableSchema));
			methodList.Add(new NewRowFromBuilderMethod(tableSchema));
			methodList.Add(new OnRowChangedMethod(tableSchema));
			methodList.Add(new OnRowChangingMethod(tableSchema));
			methodList.Add(new OnRowDeletedMethod(tableSchema));
			methodList.Add(new OnRowDeletingMethod(tableSchema));
			methodList.Add(new OnRowValidateMethod(tableSchema));
			methodList.Sort(delegate(CodeMemberMethod firstMethod, CodeMemberMethod secondMethod) { return firstMethod.Name.CompareTo(secondMethod.Name); });
			foreach (CodeMemberMethod codeMemberMethod in methodList)
				this.Members.Add(codeMemberMethod);

			//        }

		}

	}

}
