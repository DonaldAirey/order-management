namespace Teraque.AssetNetwork
{

	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Controls the operation of the simulator.
	/// </summary>
	[DataContract]
	public class SimulatorParameters
	{

		/// <summary>
		/// The frequency of the equity price updates.
		/// </summary>
		Double equityFrequencyField;

		/// <summary>
		/// The frequency of the executions in the simulator.
		/// </summary>
		Double executionFrequencyField;

		/// <summary>
		/// Gets or sets whether the order execution simulator is running.
		/// </summary>
		Boolean isExchangeSimulatorRunningField;

		/// <summary>
		/// Gets or sets whether the price simulator is running.
		/// </summary>
		Boolean isPriceSimulatorRunningField;

		/// <summary>
		/// Gets or sets the frequency of the equity price updates.
		/// </summary>
		[DataMember]
		public Double EquityFrequency
		{
			get
			{
				return this.equityFrequencyField;
			}
			set
			{
				this.equityFrequencyField = value;
			}
		}

		/// <summary>
		/// Gets or sets the frequency of the executions in the simulator.
		/// </summary>
		[DataMember]
		public Double ExecutionFrequency
		{
			get
			{
				return this.executionFrequencyField;
			}
			set
			{
				this.executionFrequencyField = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the order execution simulator is running.
		/// </summary>
		[DataMember]
		public Boolean IsExchangeSimulatorRunning
		{
			get
			{
				return this.isExchangeSimulatorRunningField;
			}
			set
			{
				this.isExchangeSimulatorRunningField = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the price simulator is running.
		/// </summary>
		[DataMember]
		public Boolean IsPriceSimulatorRunning
		{
			get
			{
				return this.isPriceSimulatorRunningField;
			}
			set
			{
				this.isPriceSimulatorRunningField = value;
			}
		}

	}

}
