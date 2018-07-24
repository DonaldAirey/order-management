namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;

	/// <summary>
	/// Create a field for the data model.
	/// </summary>
	class RowDataIndexField : CodeMemberField
	{

		/// <summary>
		/// Create a field for the data model.
		/// </summary>
		public RowDataIndexField()
		{

			this.Comments.Add(new CodeCommentStatement("The index into the array of reconcled data where the data is found."));
			this.Attributes = MemberAttributes.Private | MemberAttributes.Const;
			this.Type = new CodeGlobalTypeReference(typeof(Int32));
			this.Name = "rowDataIndex";
			this.InitExpression = new CodePrimitiveExpression(1);

		}

	}

}
