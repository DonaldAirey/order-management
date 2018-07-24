namespace Teraque.AssetNetwork
{

	using System;

	class Order
	{

		public Decimal LimitPrice {get; set;}

		public OrderTypeCode OrderTypeCode {get; set;}

		public Decimal Quantity {get; set;}

		public String Organization {get; set;}

		public String Symbol {get; set;}

		public SideCode SideCode {get; set;}

		public TimeInForceCode TimeInForceCode {get; set;}

	}

}
