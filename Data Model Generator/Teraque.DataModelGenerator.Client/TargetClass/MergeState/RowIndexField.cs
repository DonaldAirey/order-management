namespace Teraque.DataModelGenerator.TargetClass.MergeState
{

	using System;
	using System.CodeDom;
    using Teraque;

	/// <summary>
	/// Create a field for the data model.
	/// </summary>
	class RowIndexField : CodeMemberField
	{

		/// <summary>
		/// Create a field for the data model.
		/// </summary>
		public RowIndexField()
		{

			//			internal object[] rowIndex;
			this.Attributes = MemberAttributes.Assembly;
			this.Type = new CodeGlobalTypeReference(typeof(Int32));
			this.Name = "rowIndex";

		}

	}

}
