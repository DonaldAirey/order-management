namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
    using System.CodeDom;

    /// <summary>
	/// Creates a property for getting or setting the time a record lock is held.
	/// </summary>
	class LockTimeoutProperty : CodeMemberProperty
	{

		/// <summary>
		/// Creates a property for getting or setting the time a record lock is held.
		/// </summary>
		/// <param name="schema">The description of the data model.</param>
		public LockTimeoutProperty(DataModelSchema dataModelSchema)
		{

			//        /// <summary>
			//        /// Gets or sets the timeout value for locking operations.
			//        /// </summary>
			//        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//        [global::System.ComponentModel.BrowsableAttribute(false)]
			//        public static global::System.TimeSpan LockTimeout {
			//            get {
			//                return Teraque.UnitTest.Server.DataModel.lockTimeout;
			//            }
			//            set {
			//                Teraque.UnitTest.Server.DataModel.lockTimeout = value;
			//            }
			//        }
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Gets or sets the timeout value for locking operations.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.AddRange(new CodeCustomAttributesForProperties());
			this.Attributes = MemberAttributes.Public | MemberAttributes.Static;
			this.Type = new CodeGlobalTypeReference(typeof(TimeSpan));
			this.Name = "LockTimeout";
			this.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(String.Format("Tenant{0}", dataModelSchema.Name)), "lockTimeout")));
			this.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(String.Format("Tenant{0}", dataModelSchema.Name)), "lockTimeout"), new CodeVariableReferenceExpression("value")));
		
		}

	}
}
