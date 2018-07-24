namespace Teraque.AssetNetwork
{

	using System;
	using System.Runtime.Serialization;
	using System.ServiceModel;

	/// <summary>
	/// Used to specify executions that are to be destroyed.
	/// </summary>
	[DataContract]
	public class DestroyInfo
	{

		Object[] keyField;

		Int64 rowVersionField;

		public DestroyInfo(Object[] key, Int64 rowVersion)
		{

			// Initialize the object.
			this.keyField = key;
			this.rowVersionField = rowVersion;

		}

		[DataMember]
		public Object[] Key
		{
			get
			{
				return this.keyField;
			}
			set
			{
				this.keyField = value;
			}
		}

		[DataMember]
		public Int64 RowVersion
		{
			get
			{
				return this.rowVersionField;
			}
			set
			{
				this.rowVersionField = value;
			}
		}

	}

}
