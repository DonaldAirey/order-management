﻿namespace Teraque.DataModelGenerator
{

	using System;
	using System.CodeDom;
	using System.ServiceModel;

	/// <summary>
	/// Creates a 'Record Not Found' Fault for the primary key of a table.
	/// </summary>
	class CodeThrowRecordNotFoundExceptionStatement : CodeThrowExceptionStatement
	{

		/// <summary>
		/// Creates a 'Record Not Found' Exception for the primary key of a table.
		/// </summary>
		/// <param name="tableSchema">The table where the error occured.</param>
		public CodeThrowRecordNotFoundExceptionStatement(TableSchema tableSchema, CodeExpression keyExpression)
		{

			//                throw new global::System.ServiceModel.FaultException<RecordNotFoundFault>(new global::Teraque.RecordNotFoundFault("Attempt to access a Employee record ({0}) that doesn\'t exist", employeeIdKeyText));
			this.ToThrow = new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(FaultException<RecordNotFoundFault>)),
				new CodeObjectCreateExpression(new CodeGlobalTypeReference(typeof(RecordNotFoundFault)), new CodePrimitiveExpression(tableSchema.Name), keyExpression));

		}

	}
}
