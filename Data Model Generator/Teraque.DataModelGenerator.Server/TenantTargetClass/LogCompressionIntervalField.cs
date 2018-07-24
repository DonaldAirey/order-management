namespace Teraque.DataModelGenerator.TenantTargetClass
{

    using System;
    using System.CodeDom;

    /// <summary>
	/// Creates a field that defines the interval between compressing the transaction log.
	/// </summary>
	class LogCompressionInvervalField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that defines the interval between compressing the transaction log.
		/// </summary>
		public LogCompressionInvervalField()
		{

			//        private static global::System.Threading.Thread garbageCollector;
			this.Attributes = MemberAttributes.Private;
			this.Type = new CodeGlobalTypeReference(typeof(TimeSpan));
			this.Name = "logCompressionInterval";

		}

	}

}
