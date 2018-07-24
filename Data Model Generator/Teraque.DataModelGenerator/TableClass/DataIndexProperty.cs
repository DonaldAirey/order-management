namespace Teraque.DataModelGenerator.TableClass
{

	using System;
	using System.CodeDom;

	/// <summary>
	/// Creates a property to get a index.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class DataIndexProperty : CodeMemberProperty
	{

		/// <summary>
		/// Creates a property to get a index.
		/// </summary>
		/// <param name="uniqueConstraintSchema">The description of a unique constraint.</param>
		public DataIndexProperty(UniqueConstraintSchema uniqueConstraintSchema)
		{

			//            /// <summary>
			//            /// Gets the DepartmentKey index on the Department table.
			//            /// </summary>
			//            public UnitTest.Server.DataModel.DepartmentKeyIndex DepartmentKey {
			//                get {
			//                    return this.indexDepartmentKey;
			//                }
			//            }
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Gets the {0} index on the {1} table.", uniqueConstraintSchema.Name, uniqueConstraintSchema.Table.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.Type = new CodeTypeReference(String.Format("{0}Index", uniqueConstraintSchema.Name));
			this.Name = uniqueConstraintSchema.Name;


			this.GetStatements.Add(new CodeMethodReturnStatement(new CodeCastExpression(this.Type, new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), String.Format("index{0}", uniqueConstraintSchema.Name)))));

		}

	}

}
