namespace Teraque.DataModelGenerator.FieldCollectorClass
{

    using System;
    using System.CodeDom;
    using System.Collections.Generic;

	/// <summary>
	/// Creates a field that holds a node that aggregates fields from various transaction log records into a single record.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class FieldTableField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds a node that aggregates fields from various transaction log records into a single record.
		/// </summary>
		public FieldTableField()
		{

			//		internal global::System.Collections.Generic.Dictionary<int, object> fieldTable;
			this.Attributes = MemberAttributes.Assembly;
			this.Type = new CodeGlobalTypeReference(typeof(Dictionary<Int32, Object>));
			this.Name = "fieldTable";

		}

	}

}
