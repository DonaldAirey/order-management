namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;
    using Teraque;

	/// <summary>
	/// Create a field for the data model.
	/// </summary>
	class MergeStateQueueField : CodeMemberField
	{

		/// <summary>
		/// Create a field for the data model.
		/// </summary>
		public MergeStateQueueField()
		{

			//		static System.Collections.Generic.Queue<MergeState> mergeStateQueue = new System.Collections.Generic.Queue<MergeState>();
			this.Attributes = MemberAttributes.Private | MemberAttributes.Static;
			this.Type = new CodeTypeReference("System.Collections.Generic.Queue<MergeState>");
			this.Name = "mergeStateQueue";
			this.InitExpression = new CodeObjectCreateExpression(this.Type);

		}

	}

}
