namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;
	using System.Security;

	class RowVersionColumnField : CodeMemberField
	{

		/// <summary>
		/// Create a member to wait for a thread to expire.
		/// </summary>
		public RowVersionColumnField()
		{

			//		// The time to wait for a thread to respond before aborting it.
			//		private static int threadWaitTime = 1000;
			this.Comments.Add(new CodeCommentStatement("The offset to the row version in the deleted data model records."));
			this.Attributes = MemberAttributes.Private | MemberAttributes.Const;
			this.Type = new CodeGlobalTypeReference(typeof(Int32));
			this.Name = "rowVersionColumn";
			this.InitExpression = new CodePrimitiveExpression(0);

		}

	}

}
