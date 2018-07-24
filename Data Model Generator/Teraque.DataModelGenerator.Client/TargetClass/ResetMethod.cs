namespace Teraque.DataModelGenerator.TargetClass
{

    using System;
    using System.CodeDom;
	using System.Threading;

    /// <summary>
	/// Creates a method to dispose of the resources used by the data model.
	/// </summary>
	class ResetMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method to dispose of the resources used by the data model.
		/// </summary>
		/// <param name="schema">The data model schema.</param>
		public ResetMethod(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// Resets the DataModel.
			//		/// </summary>
			//		public static void Reset()
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Resets the {0}.", dataModelSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.AddRange(new CodeCustomAttributesForMethods());
			this.Name = "Reset";
			this.Attributes = MemberAttributes.Public | MemberAttributes.Static;

			//			try
			//          {
			CodeTryCatchFinallyStatement tryLockUpdateEvent = new CodeTryCatchFinallyStatement();

			//              Monitor.Enter(DataModel.syncRoot);
			tryLockUpdateEvent.TryStatements.Add(new CodeMethodInvokeExpression(new CodeGlobalTypeReferenceExpression(typeof(Monitor)), "Enter", new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "syncRoot")));

			//				DataModel.dataSetId = Guid.Empty;
			tryLockUpdateEvent.TryStatements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "dataSetId"),
					new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(Guid)), "Empty")));

			//				Monitor.Wait(DataModel.syncRoot);
			//          }
			//          finally
			//          {
			//              Monitor.Exit(DataModel.syncRoot);
			//          }
			tryLockUpdateEvent.FinallyStatements.Add(new CodeMethodInvokeExpression(new CodeGlobalTypeReferenceExpression(typeof(Monitor)), "Exit", new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "syncRoot")));
			this.Statements.Add(tryLockUpdateEvent);

			//		}

		}

	}
}
