namespace Teraque.DataModelGenerator.DataSetClientClass
{

    using System.CodeDom;
    using Teraque;
	using Teraque.DataModelGenerator;

    class VoidConstructor : CodeConstructor
	{

		public VoidConstructor(DataModelSchema dataModelSchema)
		{

			//		public DataModelClass()
			//		{
			this.Attributes = MemberAttributes.Public;

			//		}
			
		}

	}

}
