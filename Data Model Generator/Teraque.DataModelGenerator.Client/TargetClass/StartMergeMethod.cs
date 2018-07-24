namespace Teraque.DataModelGenerator.TargetClass
{

    using System;
    using System.CodeDom;
    using System.Collections;
    using System.Data;
    using System.Threading;
	using System.ServiceModel;
	using System.Windows;
	using System.Windows.Threading;
    using Teraque;

	/// <summary>
	/// Creates a method that starts the process of merging data collected from the server into the client's foreground data model.
	/// </summary>
	class StartMergeMethod : CodeMemberMethod
	{

		/// <summary>
		/// Initializes a new instance of the StartMergeMethod class.
		/// </summary>
		/// <param name="schema">The data model schema.</param>
		public StartMergeMethod(DataModelSchema dataModelSchema)
		{

			//		/// <summary>
			//		/// This thread will periodically reconcile the client data model with the server's.
			//		/// </summary>
			//		static void StartMerge(object[] transactionLog)
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(
				"Starts the process of merging the data collected in the background from the server into the client's data model.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.AddRange(new CodeCustomAttributesForMethods());
			this.Attributes = MemberAttributes.Private | MemberAttributes.Static;
			this.Name = "StartMerge";
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(Object[])), "transactionLog"));

			//			DataModel.mergeStateQueue.Enqueue(new MergeState(transactionLog));
			this.Statements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "mergeStateQueue"),
					"Enqueue",
					new CodeObjectCreateExpression("MergeState", new CodeArgumentReferenceExpression("transactionLog"))));

			//			if (DataModel.mergeStateQueue.Count == 1)
			//				global::System.Windows.Application.Current.Dispatcher.BeginInvoke(global::System.Windows.Threading.DispatcherPriority.SystemIdle, new System.Action(DataModel.MergeDataModel));
			this.Statements.Add(
				new CodeConditionStatement(
					new CodeBinaryOperatorExpression(
						new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "mergeStateQueue"), "Count"),
						CodeBinaryOperatorType.IdentityEquality,
						new CodePrimitiveExpression(1)),
						new CodeStatement[]
						{
							new CodeExpressionStatement(
								new CodeMethodInvokeExpression(
									new CodePropertyReferenceExpression(
										new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(Application)), "Current"),
										"Dispatcher"),
									"BeginInvoke",
									new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(DispatcherPriority)), "SystemIdle"),
									new CodeObjectCreateExpression(
										typeof(Action),
										new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "MergeDataModel"))))}));

			//		}

		}

	}

}
