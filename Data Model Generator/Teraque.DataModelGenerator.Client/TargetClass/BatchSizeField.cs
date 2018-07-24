namespace Teraque.DataModelGenerator.TargetClass
{

	using System;
	using System.CodeDom;
	using Teraque;

	/// <summary>
	/// Create a field for the data model.
	/// </summary>
	class BatchSizeField : CodeMemberField
	{

		/// <summary>
		/// Create a field for the data model.
		/// </summary>
		public BatchSizeField()
		{

			//			const int batchSize = 1024;
			this.Attributes = MemberAttributes.Private | MemberAttributes.Const;
			this.Type = new CodeGlobalTypeReference(typeof(Int32));
			this.Name = "batchSize";
			this.InitExpression = new CodePrimitiveExpression(1024);

		}

	}

}
