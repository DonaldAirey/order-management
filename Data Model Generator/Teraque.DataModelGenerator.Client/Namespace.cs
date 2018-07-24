namespace Teraque.DataModelGenerator
{

    using System.CodeDom;
    using System.Collections.Generic;
    using Teraque;

	/// <summary>
	/// The root namespace Teraque.ClientGenerator the DataSet output file.
	/// </summary>
	public class Namespace : CodeNamespace
	{

		/// <summary>
		/// Creates a CodeDOM namespace Teraque.ClientGenerator contains the strongly typed DataSet.
		/// </summary>
		/// <param name="schema">The schema description of the strongly typed DataSet.</param>
		public Namespace(DataModelSchema dataModelSchema)
		{

			//namespace Teraque.ClientGenerator
			//{
			this.Name = dataModelSchema.TargetNamespace;

			// The target class.
			this.Types.Add(new Teraque.DataModelGenerator.TargetClass.TargetClass(dataModelSchema));

			//}

		}

	}

}
