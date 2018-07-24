namespace Teraque.DataModelGenerator.TableClass
{

	using System;
	using System.CodeDom;

	/// <summary>
	/// Creates an event that notifies listeners to a successful row change.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class TableRowDeletingEvent : CodeMemberEvent
	{

		/// <summary>
		/// Creates an event that notifies listeners to a successful row change.
		/// </summary>
		/// <param name="tableSchema">The description of a table.</param>
		public TableRowDeletingEvent(TableSchema tableSchema)
		{

			//            /// <summary>
			//            /// Occurs after a Department row in the table has been deleted.
			//            /// </summary>
			//            public event Teraque.UnitTest.Server.DataModel.DepartmentRowChangeEventHandler DepartmentRowDeleting;
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Occurs before a {0} row in the table is about to be deleted.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Attributes = MemberAttributes.Public;
			this.Name = String.Format("{0}RowDeleting", tableSchema.Name);
			this.Type = new CodeTypeReference(String.Format("{0}RowChangeEventHandler", tableSchema.Name));

		}

	}

}
