namespace Teraque.DataModelGenerator.TenantTargetClass
{

	using System;
    using System.CodeDom;

	/// <summary>
	/// Creates a statement method invocation that adds a record to an ADO transaction.
	/// </summary>
	class CodeAcquireRecordWriterLockExpression : CodeMethodInvokeExpression
	{

		/// <summary>
		/// Creates a statement method invocation that adds a record to an ADO transaction.
		/// </summary>
		/// <param name="transactionExpression">The MiddleTierContext used for the transaction.</param>
		/// <param name="columnSchema">The record that is held for the duration of the transaction.</param>
		public CodeAcquireRecordWriterLockExpression(CodeVariableReferenceExpression transactionExpression, CodeVariableReferenceExpression rowExpression, TableSchema tableSchema)
		{

			//			configurationRow.AcquireWriterLock(middleTierTransaction.AdoResourceManager.Guid, Teraque.UnitTest.Server.DataModel.lockTimeout);
			this.Method = new CodeMethodReferenceExpression(rowExpression, "AcquireWriterLock");
			this.Parameters.Add(new CodePropertyReferenceExpression(transactionExpression, "TransactionId"));
			this.Parameters.Add(
				new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(String.Format("Tenant{0}", tableSchema.DataModelSchema.Name)), "lockTimeout"));

		}

	}

}
