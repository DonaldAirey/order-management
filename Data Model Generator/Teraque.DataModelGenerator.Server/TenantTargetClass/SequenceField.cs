namespace Teraque.DataModelGenerator.TenantTargetClass
{

    using System;
    using System.CodeDom;

    /// <summary>
	/// Creates a field that holds a delegate to a method that filters rows from the client data model.
	/// </summary>
	/// <copyright>Copyright � 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class SequenceField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds the reader/writer lock for the current data model.
		/// </summary>
		public SequenceField()
		{

			//		internal static global::System.Threading.ReaderWriterLockSlim dataLock;
			this.Attributes = MemberAttributes.Private;
			this.Type = new CodeGlobalTypeReference(typeof(Int64));
			this.Name = "sequence";

		}

	}

}
