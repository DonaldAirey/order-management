namespace Teraque.DataModelGenerator.TenantTargetClass
{

    using System;
    using System.CodeDom;

    /// <summary>
	/// Creates a field that defines how old an transaction item can be before it is consolodated.
	/// </summary>
	class TransactionLogItemAgeField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that defines how old an transaction item can be before it is consolodated.
		/// </summary>
		public TransactionLogItemAgeField()
		{

			//        private static global::System.Threading.Thread garbageCollector;
			this.Attributes = MemberAttributes.Private;
			this.Type = new CodeGlobalTypeReference(typeof(TimeSpan));
			this.Name = "transactionLogItemAge";

		}

	}

}
