namespace Teraque.DataModelGenerator.TargetClass.MergeState
{

	using System;
	using System.CodeDom;
    using Teraque;

	/// <summary>
	/// Create a field for the data model.
	/// </summary>
	class IsAnythingMergedField : CodeMemberField
	{

		/// <summary>
		/// Create a field for the data model.
		/// </summary>
		public IsAnythingMergedField()
		{

			//			internal bool isAnythingMerged;
			this.Attributes = MemberAttributes.Assembly;
			this.Type = new CodeGlobalTypeReference(typeof(Boolean));
			this.Name = "isAnythingMerged";

		}

	}

}
