﻿namespace Teraque.DataModelGenerator.ChangeEventArgsClass
{

    using System.CodeDom;
	using System.Data;

	/// <summary>
	/// Creates a field for the strongly typed row.
	/// </summary>
	class DataActionField : CodeMemberField
	{

		/// <summary>
		/// Creates a field for the strongly typed row.
		/// </summary>
		public DataActionField(TableSchema tableSchema)
		{

			//            /// <summary>
			//            /// The action that caused the change to the row.
			//            /// </summary>
			//            private global::System.Data.DataRowAction dataRowAction;
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("The action that caused the change to the row.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Attributes = MemberAttributes.Private;
			this.Type = new CodeGlobalTypeReference(typeof(DataRowAction));
			this.Name = "dataRowAction";

		}

	}
}
