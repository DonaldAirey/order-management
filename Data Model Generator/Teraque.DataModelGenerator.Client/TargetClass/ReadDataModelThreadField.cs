namespace Teraque.DataModelGenerator.TargetClass
{

    using System.CodeDom;
	using System.Threading;

	class ReadDataModelThreadField : CodeMemberField
	{

		/// <summary>
		/// A private field.
		/// </summary>
		public ReadDataModelThreadField()
		{

			this.Comments.Add(new CodeCommentStatement("Background thread for reconciling with the server data model."));
			this.Attributes = MemberAttributes.Private | MemberAttributes.Static;
			this.Type = new CodeGlobalTypeReference(typeof(Thread));
			this.Name = "reconcilerThread";

		}

	}

}
