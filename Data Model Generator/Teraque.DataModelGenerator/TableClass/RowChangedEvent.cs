namespace Teraque.DataModelGenerator.TableClass
{

	using System;
	using System.CodeDom;

	/// <summary>
	/// Creates an event that notifies listeners to a successful row change.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class TableRowChangedEvent : CodeMemberEvent
	{

		/// <summary>
		/// Creates an event that notifies listeners to a successful row change.
		/// </summary>
		/// <param name="tableSchema">The description of a table.</param>
		public TableRowChangedEvent(TableSchema tableSchema)
		{

			//            /// <summary>
			//            /// Occurs after a Department row has been changed successfully.
			//            /// </summary>
			//            public event Teraque.UnitTest.Server.DataModel.DepartmentRowChangeEventHandler DepartmentRowChanged;
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Occurs after a {0} row has been changed successfully.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Attributes = MemberAttributes.Public;
			this.Type = new CodeTypeReference(String.Format("{0}RowChangeEventHandler", tableSchema.Name));
			this.Name = String.Format("{0}RowChanged", tableSchema.Name);

		}

	}

}
