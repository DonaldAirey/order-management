namespace Teraque.DataModelGenerator.TenantTargetClass
{

	using System;
	using System.CodeDom;
    using System.Collections.Generic;

	/// <summary>
	/// Creates an expression to find a record based on the primary index of a table.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class CodeFindByIndexExpression : CodeMethodInvokeExpression
	{

		/// <summary>
		/// Creates an expression to find a record based on the primary index of a table.
		/// </summary>
		/// <param name="tableSchema"></param>
		public CodeFindByIndexExpression(TableSchema tableSchema, CodeExpression keyExpression, CodeExpression targetDataSet)
		{

			//            Teraque.UnitTest.Server.DataModel.Employee.EmployeeKey.Find(new object[] {
			//                        employeeId});
			foreach (KeyValuePair<string, ConstraintSchema> constraintPair in tableSchema.Constraints)
				if (constraintPair.Value is UniqueConstraintSchema)
				{
					UniqueConstraintSchema uniqueConstraintSchema = constraintPair.Value as UniqueConstraintSchema;
					if (uniqueConstraintSchema.IsPrimaryKey)
					{
						this.Method = new CodeMethodReferenceExpression(
							new CodePropertyReferenceExpression(
								new CodeFieldReferenceExpression(targetDataSet, String.Format("table{0}", tableSchema.Name)), uniqueConstraintSchema.Name), "Find");
						this.Parameters.Add(keyExpression);
					}
				}

		}

	}

}
