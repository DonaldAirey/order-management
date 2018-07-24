namespace Teraque.DataModelGenerator.TargetClass.MergeState
{

	using System;
	using System.CodeDom;
    using Teraque;

	/// <summary>
	/// Create a field for the data model.
	/// </summary>
	class TransactionLogField : CodeMemberField
	{

		/// <summary>
		/// Create a field for the data model.
		/// </summary>
		public TransactionLogField()
		{

			//			internal object[] transactionLog;
			this.Attributes = MemberAttributes.Assembly;
			this.Type = new CodeGlobalTypeReference(typeof(Object[]));
			this.Name = "transactionLog";

		}

	}

}
