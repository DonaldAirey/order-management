namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;

	class RefreshIntervalField : CodeMemberField
	{

		/// <summary>
		/// Create a member to wait for a thread to expire.
		/// </summary>
		public RefreshIntervalField()
		{

			this.Comments.Add(new CodeCommentStatement("The time that the background thread waits before asking for a reconcilliation from the server."));
			this.Attributes = MemberAttributes.Private | MemberAttributes.Const;
			this.Type = new CodeGlobalTypeReference(typeof(Int32));
			this.Name = "refreshInterval";
			this.InitExpression = new CodePrimitiveExpression(250);

		}

	}

}
