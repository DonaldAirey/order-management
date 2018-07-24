namespace Teraque.DataModelGenerator
{

	using System;
	using System.CodeDom;

	/// <summary>
	/// Gets or creates a transaction.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class CodeCreateTransactionStatement : CodeVariableDeclarationStatement
	{

		/// <summary>
		/// Initializes a new instance of the CodeCreateTransactionStatement class.
		/// </summary>
		/// <param name="dataModelSchema">The data model schema.</param>
		/// <param name="transactionExpression">The name of the local variable to which the transaction is assigned.</param>
		public CodeCreateTransactionStatement(DataModelSchema dataModelSchema, CodeVariableReferenceExpression transactionExpression)
		{

			//                    DataModelTransaction o1881 = DataModel.CurrentTransaction;
			this.Type = new CodeTypeReference(String.Format("{0}Transaction", dataModelSchema.Name));
			this.Name = transactionExpression.VariableName;
			this.InitExpression = new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(dataModelSchema.Name), "CurrentTransaction");

		}

	}

}
