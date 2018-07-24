namespace Teraque.DataModelGenerator.TargetClientClass
{

	using System;
	using System.CodeDom;
    using System.Collections.Generic;
    using Teraque;

    /// <summary>
	/// Stores the CodeDOM of a method to insert a record using transacted logic.
	/// </summary>
	/// <copyright>Copyright � 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class StoreMethod : CodeMemberMethod
	{

		/// <summary>
		/// Stores the CodeDOM for a method to insert a record into a table using transacted logic.
		/// </summary>
		/// <param name="tableSchema">A description of the table.</param>
		public StoreMethod(TableSchema tableSchema)
		{

			// This shreds the list of parameters up into a metadata stucture that is helpful in extracting ordinary parameters 
			// from those that need to be found in other tables using external identifiers.
			CreateExParameterMatrix createParameterMatrix = new CreateExParameterMatrix(tableSchema);

			//	[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			//	public void StoreObjectEx(string configurationId, object description, string externalId, string name, global::System.Guid objectId, object typeCode)
			//	{
			this.CustomAttributes.AddRange(new CodeCustomAttributesForMethods());
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.Name = String.Format("Store{0}", tableSchema.Name);
			foreach (KeyValuePair<string, ExternalParameterItem> parameterPair in createParameterMatrix.ExternalParameterItems)
				this.Parameters.Add(parameterPair.Value.CodeParameterDeclarationExpression);

			//		base.Channel.StoreObjectEx(configurationId, description, externalId, name, objectId, typeCode);
			List<CodeExpression> arguments = new List<CodeExpression>();
			foreach (KeyValuePair<string, ExternalParameterItem> parameterPair in createParameterMatrix.ExternalParameterItems)
				arguments.Add(new CodeArgumentReferenceExpression(parameterPair.Value.Name));
			this.Statements.Add(new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(new CodeBaseReferenceExpression(), "Channel"), this.Name,
				arguments.ToArray()));

			//	}

		}

	}

}