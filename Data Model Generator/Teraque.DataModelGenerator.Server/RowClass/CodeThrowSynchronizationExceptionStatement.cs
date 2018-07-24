namespace Teraque.DataModelGenerator.RowClass
{

	using System;
	using System.CodeDom;
	using System.ServiceModel;

	/// <summary>
	/// Creates a 'Record Not Locked' Exception for a table.
	/// </summary>
	class CodeThrowSynchronizationExceptionStatement : CodeThrowExceptionStatement
	{

		/// <summary>
		/// Creates a 'Record Not Locked' Exception for a table.
		/// </summary>
		/// <param name="tableSchema">A description of the table.</param>
		public CodeThrowSynchronizationExceptionStatement(TableSchema tableSchema)
		{

			//                        throw new global::System.ServiceModel.FaultException<SynchronizationLockFault>(new global::Teraque.SynchronizationLockFault("Attempt to access a Department record without a lock."));
			this.ToThrow = new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(FaultException<SynchronizationLockFault>)),
				new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(SynchronizationLockFault)), new CodePrimitiveExpression(tableSchema.Name)));

		}

	}
}
