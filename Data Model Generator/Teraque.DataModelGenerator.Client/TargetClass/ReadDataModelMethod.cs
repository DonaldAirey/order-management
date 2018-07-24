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
	/// Creates a method to handle moving the deleted records from the active data model to the deleted data model.
	/// </summary>
	class ReadDataModelMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates a method to handle moving the deleted records from the active data model to the deleted data model.
		/// </summary>
		/// <param name="schema">The data model schema.</param>
		public ReadDataModelMethod(DataModelSchema dataModelSchema)
		{

			// These variable are used to create a connection to the server.
			String clientTypeName = String.Format("{0}Client", dataModelSchema.Name);
			String endpointName = String.Format("{0}Endpoint", dataModelSchema.Name);
			String clientVariableName = CommonConversion.ToCamelCase(clientTypeName);

			//		/// <summary>
			//		/// This thread will periodically reconcile the client data model with the server's.
			//		/// </summary>
			//		private static void ReadDataModel()
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("This thread will periodically reconcile the client data model with the server's.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.CustomAttributes.AddRange(new CodeCustomAttributesForMethods());
			this.Attributes = MemberAttributes.Private | MemberAttributes.Static;
			this.Name = "ReadDataModel";

			//			DataModelClient dataModelClient = new DataModelClient(Teraque.AssetNetwork.Properties.Settings.Default.DataModelEndpoint);
			this.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference(clientTypeName),
					clientVariableName,
					new CodeObjectCreateExpression(
						new CodeTypeReference(clientTypeName),
						new CodePropertyReferenceExpression(
							new CodePropertyReferenceExpression(
								new CodePropertyReferenceExpression(
									new CodeTypeReferenceExpression(String.Format("{0}.{1}", dataModelSchema.TargetNamespace, "Properties")),
									"Settings"),
								"Default"),
							String.Format("{0}Endpoint", dataModelSchema.Name)))));

			//			for (
			//			; (DataModel.IsReading == true);
			//			)
			//			{
			CodeIterationStatement whileReconciling = new CodeIterationStatement(
				new CodeSnippetStatement(),
				new CodeBinaryOperatorExpression(
					new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "IsReading"),
					CodeBinaryOperatorType.IdentityEquality,
					new CodePrimitiveExpression(true)),
				new CodeSnippetStatement());

			//				try
			//				{
			CodeTryCatchFinallyStatement tryReading = new CodeTryCatchFinallyStatement();

			//					object[] dataHeader = dataModelClient.Read(DataModel.dataSetId, DataModel.sequence);
			tryReading.TryStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Object[])),
					"dataHeader",
					new CodeMethodInvokeExpression(
						new CodeVariableReferenceExpression(clientVariableName),
						"Read",
						new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "dataSetId"),
						new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "sequence"))));

			//					global::System.Guid dataSetId = ((global::System.Guid)(dataHeader[0]));
			tryReading.TryStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Guid)),
					"dataSetId",
					new CodeCastExpression(
						new CodeGlobalTypeReference(typeof(Guid)),
						new CodeIndexerExpression(new CodeVariableReferenceExpression("dataHeader"), new CodePrimitiveExpression(0)))));
	
			//					DataModel.sequence = ((long)(dataHeader[1]));
			tryReading.TryStatements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "sequence"),
					new CodeCastExpression(
						new CodeGlobalTypeReference(typeof(Int64)),
						new CodeIndexerExpression(new CodeVariableReferenceExpression("dataHeader"), new CodePrimitiveExpression(1)))));

			//					object[] transactionLog = ((object[])(dataHeader[2]));
			tryReading.TryStatements.Add(
				new CodeVariableDeclarationStatement(
					new CodeGlobalTypeReference(typeof(Object[])),
					"transactionLog",
					new CodeCastExpression(
						new CodeGlobalTypeReference(typeof(Object[])),
						new CodeIndexerExpression(new CodeVariableReferenceExpression("dataHeader"), new CodePrimitiveExpression(2)))));
	
			//					if ((dataSetId != DataModel.dataSetId))
			//					{
			CodeConditionStatement ifInvalidDataSet = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("dataSetId"),
					CodeBinaryOperatorType.IdentityInequality,
					new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "dataSetId")));
			
			//						DataModel.dataSetId = dataSetId;
			ifInvalidDataSet.TrueStatements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "dataSetId"),
					new CodeVariableReferenceExpression("dataSetId")));

			//						DataModel.dataSet.EnforceConstraints = false;
			ifInvalidDataSet.TrueStatements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(
						new CodeFieldReferenceExpression(
							new CodeTypeReferenceExpression(dataModelSchema.Name), "dataSet"),
						"EnforceConstraints"),
					new CodePrimitiveExpression(false)));

			//						DataModel.dataSet.Clear();
			//							DataModel.dataSet.Clear();
			ifInvalidDataSet.TrueStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "dataSet"),
					"Clear"));

			//						DataModel.dataSet.EnforceConstraints = true;
			ifInvalidDataSet.TrueStatements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(
						new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "dataSet"),
						"EnforceConstraints"),
					new CodePrimitiveExpression(true)));

			//					}
			tryReading.TryStatements.Add(ifInvalidDataSet);

			//					if ((transactionLog.Length != 0))
			//					{
			CodeConditionStatement ifResults = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("transactionLog"), "Length"),
					CodeBinaryOperatorType.IdentityInequality,
					new CodePrimitiveExpression(0)));

			//						System.Windows.Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle, new System.Action<object[]>(DataModel.StartMerge), transactionLog);
			//					}
			ifResults.TrueStatements.Add(
				new CodeMethodInvokeExpression(
					new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(Application)), "Current"), "Dispatcher"),
					"BeginInvoke",
					new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(DispatcherPriority)), "SystemIdle"),
					new CodeObjectCreateExpression(new CodeTypeReference(typeof(Action<Object[]>)), new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "StartMerge")),
					new CodeVariableReferenceExpression("transactionLog")));
			tryReading.TryStatements.Add(ifResults);

			//				catch (global::System.ServiceModel.FaultException<Teraque.TenantNotLoadedFault> tenantNotFoundFaultException)
			//				{
			//					if (DataModel.TenantNotLoaded != null)
			//						DataModel.TenantNotLoaded(typeof(DataModel), tenantNotFoundFaultException.Detail.TenantName);
			//				}
			CodeCatchClause tenantNotLoadedFaultCatch = new CodeCatchClause(
				"tenantNotFoundFaultException",
				new CodeGlobalTypeReference(typeof(FaultException<TenantNotLoadedFault>)));
			tenantNotLoadedFaultCatch.Statements.Add(
				new CodeMethodInvokeExpression(
					new CodeGlobalTypeReferenceExpression(typeof(Log)),
					"Error",
					new CodePrimitiveExpression("Tenant {0} not loaded."),
					new CodePropertyReferenceExpression(
						new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("tenantNotFoundFaultException"), "Detail"), "TenantName")));
			tenantNotLoadedFaultCatch.Statements.Add(
				new CodeConditionStatement(
					new CodeBinaryOperatorExpression(
						new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "TenantNotLoaded"),
						CodeBinaryOperatorType.IdentityInequality,
						new CodePrimitiveExpression(null)),
					new CodeStatement[] {
						new CodeExpressionStatement(
							new CodeMethodInvokeExpression(
								new CodeTypeReferenceExpression(dataModelSchema.Name),
								"TenantNotLoaded",
								new CodeTypeOfExpression(new CodeTypeReference(dataModelSchema.Name)),
								new CodePropertyReferenceExpression(
									new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("tenantNotFoundFaultException"), "Detail"), "TenantName")))}));

			//				}
			tryReading.CatchClauses.Add(tenantNotLoadedFaultCatch);

			//				catch (global::System.Exception exception)
			//				{
			CodeCatchClause generalCatch = new CodeCatchClause("exception", new CodeGlobalTypeReference(typeof(Exception)));

			//					global::Teraque.Log.Error("{0}, {1}", exception.Message, exception.StackTrace);
			//				}
			generalCatch.Statements.Add(
				new CodeMethodInvokeExpression(
					new CodeGlobalTypeReferenceExpression(typeof(Log)),
					"Error",
					new CodePrimitiveExpression("{0}, {1}"),
					new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("exception"), "Message"),
					new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression("exception"), "StackTrace")));
			tryReading.CatchClauses.Add(generalCatch);
			whileReconciling.Statements.Add(tryReading);

			//				}
			//				finally
			//				{
			//					if ((dataModelClient.State != System.ServiceModel.CommunicationState.Opened))
			//					{
			CodeConditionStatement ifChannelNotOpen = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("dataModelClient"), "State"),
					CodeBinaryOperatorType.IdentityInequality,
					new CodePropertyReferenceExpression(new CodeGlobalTypeReferenceExpression(typeof(CommunicationState)), "Opened")));
			
			//						global::System.Threading.Thread.Sleep(1000);
			ifChannelNotOpen.TrueStatements.Add(new CodeMethodInvokeExpression(
					new CodeGlobalTypeReferenceExpression(typeof(Thread)),
					"Sleep",
					new CodePrimitiveExpression(1000)));

			//						dataModelClient = new DataModelClient(Teraque.AssetNetwork.Properties.Settings.Default.DataModelEndpoint);
			//					}
			//				}
			ifChannelNotOpen.TrueStatements.Add(
				new CodeAssignStatement(
					new CodeVariableReferenceExpression(clientVariableName),
					new CodeObjectCreateExpression(
						new CodeTypeReference(clientTypeName),
						new CodePropertyReferenceExpression(
							new CodePropertyReferenceExpression(
								new CodePropertyReferenceExpression(
									new CodeTypeReferenceExpression(String.Format("{0}.{1}", dataModelSchema.TargetNamespace, "Properties")),
									"Settings"),
								"Default"),
							String.Format("{0}Endpoint", dataModelSchema.Name)))));
			tryReading.FinallyStatements.Add(ifChannelNotOpen);
			
			//				global::System.Threading.Thread.Sleep(DataModel.refreshInterval);
			//				global::System.Threading.Thread.Sleep(DataModel.refreshInterval);
			whileReconciling.Statements.Add(
				new CodeMethodInvokeExpression(
					new CodeGlobalTypeReferenceExpression(typeof(Thread)),
					"Sleep",
					new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "refreshInterval")));

			//			}
			this.Statements.Add(whileReconciling);

			//		}

		}

	}

}
