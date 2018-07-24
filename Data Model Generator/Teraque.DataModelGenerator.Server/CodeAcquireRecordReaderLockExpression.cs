namespace Teraque.DataModelGenerator
{

	using System;
	using System.CodeDom;

	/// <summary>
	/// Creates a statement method invocation that adds a record to an ADO transaction.
	/// </summary>
	class CodeAcquireRecordReaderLockExpression : CodeMethodInvokeExpression
	{

		/// <summary>
		/// Creates a statement method invocation that adds a record to an ADO transaction.
		/// </summary>
		/// <param name="transactionExpression">The MiddleTierContext used for the transaction.</param>
		/// <param name="columnSchema">The record that is held for the duration of the transaction.</param>
		public CodeAcquireRecordReaderLockExpression(CodeExpression transactionExpression, CodeExpression rowExpression, DataModelSchema dataModelSchema)
		{

			//            departmentRow.ObjectRow.AcquireReaderLock(middleTierTransaction.AdoResourceManager.Guid, Teraque.UnitTest.Server.DataModel.lockTimeout);
			this.Method = new CodeMethodReferenceExpression(rowExpression, "AcquireReaderLock");
			this.Parameters.Add(new CodePropertyReferenceExpression(transactionExpression, "TransactionId"));
			this.Parameters.Add(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(String.Format("Tenant{0}", dataModelSchema.Name)), "lockTimeout"));

		}

	}

}
