namespace Teraque.DataModelGenerator.TargetClass
{

    using System.CodeDom;
	using System.ComponentModel;
	using System.Data;

    /// <summary>
	/// Creates a property that gets the collection of relationship between tables in the data model.
	/// </summary>
	public class RelationsProperty : CodeMemberProperty
	{

		/// <summary>
		/// Creates a property that gets the collection of relationship between tables in the data model.
		/// </summary>
		/// <param name="dataModelSchema">The data model schema.</param>
		public RelationsProperty(DataModelSchema dataModelSchema)
		{

			//        /// <summary>
			//        /// Gets the collection of relations that link tables and allow navigation between parent tables and child tables.
			//        /// </summary>
			//        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//        [global::System.ComponentModel.BrowsableAttribute(false)]
			//        public static global::System.Data.DataRelationCollection Relations {
			//            get {
			//                return Teraque.UnitTest.Server.DataModel.dataSet.Relations;
			//            }
			//        }
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Gets the collection of relations that link tables and allow navigation between parent tables and child tables.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
            // HACK - Put this line back in for official releases
            //this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(DebuggerNonUserCodeAttribute))));
			this.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeGlobalTypeReference(typeof(BrowsableAttribute)), new CodeAttributeArgument(new CodePrimitiveExpression(false))));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			this.Type = new CodeGlobalTypeReference(typeof(DataRelationCollection));
			this.Name = "Relations";
			this.GetStatements.Add(new CodeMethodReturnStatement(new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "dataSet"), "Relations")));
		
		}

	}
}
