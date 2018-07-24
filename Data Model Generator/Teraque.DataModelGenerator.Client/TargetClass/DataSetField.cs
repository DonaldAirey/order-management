namespace Teraque.DataModelGenerator.TargetClass
{

    using System.CodeDom;
	using System.Data;

	/// <summary>
	/// Creates a private field to hold the generic DataSet.
	/// </summary>
	public class DataSetField : CodeMemberField
	{

		/// <summary>
		/// Creates a private field to hold the generic DataSet.
		/// </summary>
		public DataSetField()
		{

			//        // The generic data behind the strongly typed data model.
			//        private static global::System.Data.DataSet dataSet;
			this.Attributes = MemberAttributes.Private | MemberAttributes.Static;
			this.Type = new CodeGlobalTypeReference(typeof(DataSet));
			this.Name = "dataSet";

		}

	}

}
