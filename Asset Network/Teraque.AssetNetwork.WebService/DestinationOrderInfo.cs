namespace Teraque.AssetNetwork
{

	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Information describing a destination order.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	[DataContract]
	public class DestinationOrderInfo
	{

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public Guid BlotterId;

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public Guid DestinationId;

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public Decimal OrderedQuantity;

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public OrderTypeCode OrderTypeCode;

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public Guid SecurityId;

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public Guid SettlementId;

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public SideCode SideCode;

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public TimeInForceCode TimeInForceCode;

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public Guid WorkingOrderId;

	}

}
