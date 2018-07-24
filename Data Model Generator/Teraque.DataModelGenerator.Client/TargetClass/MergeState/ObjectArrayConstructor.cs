namespace Teraque.DataModelGenerator.TargetClass.MergeState
{

    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using Teraque;

	/// <summary>
	/// Represents a declaration of a property that gets the parent row.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class ObjectArrayConstructor : CodeConstructor
	{

		/// <summary>
		/// Generates a property to get a parent row.
		/// </summary>
		/// <param name="foreignKeyConstraintSchema">The foreign key that references the parent table.</param>
		public ObjectArrayConstructor(DataModelSchema dataModelSchema)
		{

			//            internal MergeState(Object[] transactionLog) {
			this.Attributes = MemberAttributes.Assembly;
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(Object[])), "transactionLog"));

			//			this.transactionLog = transationLog;
			this.Statements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLog"), new CodeArgumentReferenceExpression("transactionLog")));
			//			this.rowIndex = transationLog.Length - 1;
			this.Statements.Add(
				new CodeAssignStatement(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "rowIndex"),
					new CodeBinaryOperatorExpression(
						new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLog"), "Length"),
						CodeBinaryOperatorType.Subtract,
						new CodePrimitiveExpression(1))));

			//            }

		}

	}

}
