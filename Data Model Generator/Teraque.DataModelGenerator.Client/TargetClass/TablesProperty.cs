namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;
    using System.Data;
    using Teraque;

    /// <summary>
	/// Creates a property to return a collection of the tables in the data model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class TablesProperty : CodeMemberProperty
	{

		/// <summary>
		/// Creates a property to return a collection of the tables in the data model.
		/// </summary>
		/// <param name="dataModelSchema">The data model schema.</param>
		public TablesProperty(DataModelSchema dataModelSchema)
		{

			//        /// <summary>
			//        /// Gets the collection of tables contained in the DataModel.
			//        /// </summary>
			//        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//        [global::System.ComponentModel.BrowsableAttribute(false)]
			//        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
			//        public static global::System.Data.DataTableCollection Tables
			//		  {
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Gets the collection of tables contained in the {0}.", dataModelSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.AddRange(new CodeCustomAttributesForProperties());
			this.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			this.Type = new CodeGlobalTypeReference(typeof(DataTableCollection));
			this.Name = "Tables";

			//            get {
			//                return Teraque.UnitTest.Server.DataModel.dataSet.Tables;
			//            }
			this.GetStatements.Add(
				new CodeMethodReturnStatement(
					new CodePropertyReferenceExpression(
						new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "dataSet"),
						"Tables")));

			//        }

		}

	}
}
