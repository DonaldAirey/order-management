namespace Teraque.DataModelGenerator.TargetClass
{

    using System;
    using System.CodeDom;
    using Teraque;

	/// <summary>
	/// Create a field for the data model.
	/// </summary>
	class SequenceField : CodeMemberField
	{

		/// <summary>
		/// Create a field for the data model.
		/// </summary>
		public SequenceField()
		{

			this.Attributes = MemberAttributes.Private | MemberAttributes.Static;
			this.Type = new CodeGlobalTypeReference(typeof(Int64));
			this.Name = "sequence";

		}

	}

}
