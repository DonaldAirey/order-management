namespace Teraque.DataModelGenerator.TargetClientClass
{

	using System;
	using System.CodeDom;
    using System.Collections.Generic;
    using Teraque;

    /// <summary>
	/// Creates the CodeDOM of a method to insert a record using transacted logic.
	/// </summary>
	/// <copyright>Copyright � 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class UpdateMethod : CodeMemberMethod
	{

		/// <summary>
		/// Creates the CodeDOM for a method to insert a record into a table using transacted logic.
		/// </summary>
		/// <param name="tableSchema">A description of the table.</param>
		public UpdateMethod(TableSchema tableSchema)
		{

			// This shreds the list of parameters up into a metadata stucture that is helpful in extracting ordinary parameters 
			// from those that need to be found in other tables using external identifiers.
			UpdateExParameterMatrix updateParameterMatrix = new UpdateExParameterMatrix(tableSchema);

			//	[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	public void UpdateObjectEx(string configurationId, object description, object externalId, object name, global::System.Guid objectId, object typeCode)
			//	{
			this.CustomAttributes.AddRange(new CodeCustomAttributesForMethods());
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.Name = String.Format("Update{0}", tableSchema.Name);
			foreach (KeyValuePair<string, ExternalParameterItem> parameterPair in updateParameterMatrix.ExternalParameterItems)
				this.Parameters.Add(parameterPair.Value.CodeParameterDeclarationExpression);

			//		base.Channel.UpdateObjectEx(configurationId, description, externalId, name, objectId, typeCode);
			List<CodeExpression> arguments = new List<CodeExpression>();
			foreach (KeyValuePair<string, ExternalParameterItem> parameterPair in updateParameterMatrix.ExternalParameterItems)
				arguments.Add(new CodeArgumentReferenceExpression(parameterPair.Value.Name));
			this.Statements.Add(new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(new CodeBaseReferenceExpression(), "Channel"), this.Name,
				arguments.ToArray()));

			//	}

		}

	}

}
