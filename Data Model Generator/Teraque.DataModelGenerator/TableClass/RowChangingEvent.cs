namespace Teraque.DataModelGenerator.TableClass
{

	using System;
	using System.CodeDom;

	/// <summary>
	/// Creates an event that notifies listeners to a successful row change.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class TableRowChangingEvent : CodeMemberEvent
	{

		/// <summary>
		/// Creates an event that notifies listeners to a successful row change.
		/// </summary>
		/// <param name="tableSchema">The description of a table.</param>
		public TableRowChangingEvent(TableSchema tableSchema)
		{

			//            /// <summary>
			//            /// Occurs when a Department row is changing.
			//            /// </summary>
			//            public event Teraque.UnitTest.Server.DataModel.DepartmentRowChangeEventHandler DepartmentRowChanging;
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Occurs when a {0} row is changing.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Attributes = MemberAttributes.Public;
			this.Name = String.Format("{0}RowChanging", tableSchema.Name);
			this.Type = new CodeTypeReference(String.Format("{0}RowChangeEventHandler", tableSchema.Name));

		}

	}

}
