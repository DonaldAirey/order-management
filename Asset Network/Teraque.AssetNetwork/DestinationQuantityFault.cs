namespace Teraque.AssetNetwork
{

	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Contains information about a failure to find a record.
	/// </summary>
	[DataContractAttribute]
	public class DestinationQuantityFault
	{

		/// <summary>
		/// The working order identifier.
		/// </summary>
		Guid workingOrderId;

		/// <summary>
		/// Quantity of the destination order.
		/// </summary>
		Decimal destinationOrderQuantity;

		/// <summary>
		/// Quantity of the source order.
		/// </summary>
		Decimal sourceOrderQuantity;

		/// <summary>
		/// Create information about a failure to find a record.
		/// </summary>
		/// <param name="workingOrderId">The working order identifier where the failure occurred.</param>
		/// <param name="sourceOrderQuantity">The source order that failed.</param>
		/// <param name="destinationOrderQuantity">The quantity that caused the failure.</param>
		public DestinationQuantityFault(Guid workingOrderId, Decimal sourceOrderQuantity, Decimal destinationOrderQuantity)
		{

			// Initialize the object.
			this.workingOrderId = workingOrderId;
			this.destinationOrderQuantity = destinationOrderQuantity;
			this.sourceOrderQuantity = sourceOrderQuantity;

		}

		/// <summary>
		/// Gets or sets the aggregate Destination Order quantity at the time of the fault.
		/// </summary>
		[DataMemberAttribute]
		public Decimal DestinationOrderQuantity
		{
			get { return this.destinationOrderQuantity; }
			set { this.destinationOrderQuantity = value; }
		}

		/// <summary>
		/// Gets or sets the aggregate Source Order quantity at the time of the fault.
		/// </summary>
		[DataMemberAttribute]
		public Decimal SourceOrderQuantity
		{
			get { return this.sourceOrderQuantity; }
			set { this.sourceOrderQuantity = value; }
		}

		/// <summary>
		/// Gets or sets the Working Order where the fault occurred.
		/// </summary>
		[DataMemberAttribute]
		public Guid WorkingOrderId
		{
			get { return this.workingOrderId; }
			set { this.workingOrderId = value; }
		}

	}

}
