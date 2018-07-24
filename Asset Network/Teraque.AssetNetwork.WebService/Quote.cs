namespace Teraque.AssetNetwork
{

	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// A price quote.
	/// </summary>
	[DataContract]
	public class Quote
	{

		[DataMember]
		public Decimal AskPrice;

		[DataMember]
		public Decimal AskSize;

		[DataMember]
		public Decimal BidPrice;

		[DataMember]
		public Decimal BidSize;

		[DataMember]
		public Decimal LastPrice;

		[DataMember]
		public Decimal LastSize;

		[DataMember]
		public String Symbol;

	}

}
