namespace Teraque.DataModelGenerator.TenantTargetClass
{

	using System;
	using System.CodeDom;
    using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;
	using System.Threading;

	/// <summary>
	/// Creates the CodeDOM to release managed resources.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class DisposeMethod : CodeMemberMethod
	{

		/// <summary>
		/// Initializes a new instance of the DisposeMethod class.
		/// </summary>
		/// <param name="tableSchema">A description of the table.</param>
		public DisposeMethod(DataModelSchema dataModelSchema)
		{

			//      /// <summary>
			//      /// Releases the resources used by the MarshalByValueComponent.
			//      /// </summary>
			//      /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
			//		protected override void Dispose(bool disposing)
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement("Releases the resources used by the MarshalByValueComponent.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Comments.Add(new CodeCommentStatement("<param name=\"isDisposing\">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>", true));
			this.Attributes = MemberAttributes.Family | MemberAttributes.Override;
			this.Name = "Dispose";
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(Boolean)), "disposing"));

			//			if ((disposing))
			//			{
			CodeConditionStatement isDisposing = new CodeConditionStatement(new CodeArgumentReferenceExpression("disposing"));

			//				if ((this.compressorThread.IsAlive))
			//					this.compressorThread.Abort();
			isDisposing.TrueStatements.Add(
				new CodeConditionStatement(
					new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "compressorThread"), "IsAlive"),
					new CodeStatement[]
					{
						new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "compressorThread"), "Abort"))
					}));

			//			}
			this.Statements.Add(isDisposing);
	
			//			base.Dispose(disposing);
			this.Statements.Add(new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "Dispose", new CodeArgumentReferenceExpression("disposing")));
			
			//		}

		}

	}

}
