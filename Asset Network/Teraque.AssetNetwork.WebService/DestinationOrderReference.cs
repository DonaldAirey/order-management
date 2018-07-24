namespace Teraque.AssetNetwork
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Runtime.Serialization;

	/// <summary>
	/// Destination order identification
	/// </summary>
	[DataContract]
	public class DestinationOrderReference
	{

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public Guid DestinationId;

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public Int64 RowVersion;

	}

}
