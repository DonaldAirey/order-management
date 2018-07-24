namespace Teraque.DataModelGenerator.TransactionClass
{

	using System.CodeDom;
	using System.Threading;

	/// <summary>
	/// Creates a field that holds the locks used in a transaction.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class CountdownEventField : CodeMemberField
	{

		/// <summary>
		/// Creates a field that holds the locks used in a transaction.
		/// </summary>
		public CountdownEventField()
		{

			//		private global::System.Threading.CountdownEvent countdownEvent = new global::System.Threading.CountdownEvent(1);
			this.Attributes = MemberAttributes.Private;
			this.Type = new CodeTypeReference(typeof(CountdownEvent));
			this.Name = "countdownEvent";
			this.InitExpression = new CodeObjectCreateExpression(new CodeTypeReference(typeof(CountdownEvent)), new CodePrimitiveExpression(1));

		}

	}

}
