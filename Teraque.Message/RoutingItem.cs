namespace Teraque
{

	using System;

	/// <summary>
	/// Defines a routing instruction.
	/// </summary>
	[Serializable]
	public class RoutingItem
	{

		/// <summary>
		/// The Routing Id.
		/// </summary>
		public String RoutingId { get; set; }

		/// <summary>
		/// The RoutingType.
		/// </summary>
		public RoutingTypeCode RoutingType { get; set; }

	}

}
