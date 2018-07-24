namespace Teraque.DataModelGenerator.TenantTargetClass
{

    using System;
    using System.CodeDom;

    /// <summary>
	/// Creates a field that defines the number of transaction log items that will be examined before surrending the CPU.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class TransactionLogBatchSizeField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that defines the number of transaction log items that will be examined before surrending the CPU.
		/// </summary>
		public TransactionLogBatchSizeField()
		{

			//		internal static global::System.Threading.ReaderWriterLockSlim dataLock;
			this.Attributes = MemberAttributes.Private;
			this.Type = new CodeGlobalTypeReference(typeof(Int32));
			this.Name = "transactionLogBatchSize";

		}

	}

}
