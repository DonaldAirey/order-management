namespace Teraque.DataModelGenerator.TableClass
{

	using System;
	using System.CodeDom;
	using System.Collections.Generic;
	using System.ComponentModel;

	/// <summary>
	/// Creates a propert that provides a table of indices.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class IndicesField : CodeMemberField
	{

		/// <summary>
		/// Creates a propert that gets the count of rows in the table.
		/// </summary>
		/// <param name="tableSchema">A description of the table.</param>
		public IndicesField()
		{

			this.Attributes = MemberAttributes.Assembly | MemberAttributes.Final;
			this.Type = new CodeGlobalTypeReference(typeof(Dictionary<String, DataIndex>));
			this.Name = "Indices";
			this.InitExpression = new CodeObjectCreateExpression(typeof(Dictionary<String, DataIndex>));

		}

	}

}
