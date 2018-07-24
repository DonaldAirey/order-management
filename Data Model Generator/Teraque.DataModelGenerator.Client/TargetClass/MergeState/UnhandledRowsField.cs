namespace Teraque.DataModelGenerator.TargetClass.MergeState
{

	using System;
	using System.CodeDom;
	using System.Collections;
    using Teraque;

	/// <summary>
	/// Create a field for the data model.
	/// </summary>
	class UnhandledRowsField : CodeMemberField
	{

		/// <summary>
		/// Create a field for the data model.
		/// </summary>
		public UnhandledRowsField()
		{

			//			public System.Collections.ArrayList UnhandledRows = new System.Collections.ArrayList();
			this.Attributes = MemberAttributes.Assembly;
			this.Type = new CodeGlobalTypeReference(typeof(ArrayList));
			this.Name = "unhandledRows";
			this.InitExpression = new CodeObjectCreateExpression(this.Type);

		}

	}

}
