namespace Teraque.DataModelGenerator.TargetClass.MergeState
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
	public class MergeStateClass : CodeTypeDeclaration
	{

		/// <summary>
		/// A class to manage the state of merging results from the server.
		/// </summary>
		/// <param name="dataModelSchema">The schema used to generate the class.</param>
		public MergeStateClass(DataModelSchema dataModelSchema)
		{

			//		class MergeState
			//		{
			this.Name = "MergeState";
			this.TypeAttributes = TypeAttributes.Class;

			// Fields
			List<CodeMemberField> privateInstanceFieldList = new List<CodeMemberField>();
			privateInstanceFieldList.Add(new IsAnythingMergedField());
			privateInstanceFieldList.Add(new RowIndexField());
			privateInstanceFieldList.Add(new TransactionLogField());
			privateInstanceFieldList.Add(new UnhandledRowsField());
			privateInstanceFieldList.Sort(delegate(CodeMemberField firstField, CodeMemberField secondField) { return firstField.Name.CompareTo(secondField.Name); });
			foreach (CodeMemberField codeMemberField in privateInstanceFieldList)
				this.Members.Add(codeMemberField);

			// Constructors
			this.Members.Add(new ObjectArrayConstructor(dataModelSchema));


		}

	}

}