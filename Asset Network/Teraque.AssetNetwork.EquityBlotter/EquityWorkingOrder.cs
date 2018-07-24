namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Windows;
	using Teraque;
	using Teraque.AssetNetwork;

	/// <summary>
	/// The Model View for generic Working Orders.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class EquityWorkingOrder : WorkingOrder
	{

		/// <summary>
		/// Crossing Code.
		/// </summary>
		Boolean isCrossingField;

		/// <summary>
		/// Is a Broker match.
		/// </summary>
		Boolean isBrokerMatchField;

		/// <summary>
		/// Is a Hedge Fund match.
		/// </summary>
		Boolean isHedgeMatchField;

		/// <summary>
		/// Is an Institution match.
		/// </summary>
		Boolean isInstutionMatchField;

		/// <summary>
		/// Limit Price.
		/// </summary>
		Decimal limitPriceField;

		/// <summary>
		/// Order type mnemonic.
		/// </summary>
		String orderTypeMnemonicField;

		/// <summary>
		/// Order type code.
		/// </summary>
		OrderTypeCode orderTypeCodeField;

		/// <summary>
		/// Stop price.
		/// </summary>
		Decimal stopPriceField;

		/// <summary>
		/// The time in force of the order.
		/// </summary>
		TimeInForceCode timeInForceCodeField;
	
		/// <summary>
		/// Initializes a new instance of the EquityWorkingOrder class from a record in the data model.
		/// </summary>
		/// <param name="workingOrderRow">The working order record in the data model.</param>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public EquityWorkingOrder(DataModel.WorkingOrderRow workingOrderRow)
			: base(workingOrderRow)
		{

			// The new instance is initialized with a copy of the data from the data model.
			this.Copy(workingOrderRow);

		}

		/// <summary>
		/// Gets or sets the crossing code.
		/// </summary>
		public Boolean IsCrossing
		{
			get
			{
				return this.isCrossingField;
			}
			set
			{
				{
					if (this.isCrossingField != value)
					{
						this.isCrossingField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("IsCrossing"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets whether or not this is a broker match.
		/// </summary>
		public Boolean IsBrokerMatch
		{
			get
			{
				return this.isBrokerMatchField;
			}
			set
			{
				{
					if (this.isBrokerMatchField != value)
					{
						this.isBrokerMatchField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("IsBrokerMatch"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets whether or not this is a hedge fund match.
		/// </summary>
		public Boolean IsHedgeMatch
		{
			get
			{
				return this.isHedgeMatchField;
			}
			set
			{
				{
					if (this.isHedgeMatchField != value)
					{
						this.isHedgeMatchField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("IsHedgeMatch"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets whether or not this is an insitution match.
		/// </summary>
		public Boolean IsInstitutionMatch
		{
			get
			{
				return this.isInstutionMatchField;
			}
			set
			{
				{
					if (this.isInstutionMatchField != value)
					{
						this.isInstutionMatchField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("IsInstitutionMatch"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the limit price.
		/// </summary>
		public Decimal LimitPrice
		{
			get
			{
				return this.limitPriceField;
			}
			set
			{
				{
					if (this.limitPriceField != value)
					{
						this.limitPriceField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("LimitPrice"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the order type code.
		/// </summary>
		public OrderTypeCode OrderTypeCode
		{
			get
			{
				return this.orderTypeCodeField;
			}
			set
			{
				{
					if (this.orderTypeCodeField != value)
					{
						this.orderTypeCodeField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("OrderTypeCode"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the order mnemonic.
		/// </summary>
		public String OrderTypeMnemonic
		{
			get
			{
				return this.orderTypeMnemonicField;
			}
			set
			{
				{
					if (this.orderTypeMnemonicField != value)
					{
						this.orderTypeMnemonicField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("OrderTypeMnemonic"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the stop price.
		/// </summary>
		public Decimal StopPrice
		{
			get
			{
				return this.stopPriceField;
			}
			set
			{
				{
					if (this.stopPriceField != value)
					{
						this.stopPriceField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("StopPrice"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the time in force.
		/// </summary>
		public TimeInForceCode TimeInForceCode
		{
			get
			{
				return this.timeInForceCodeField;
			}
			set
			{
				if (this.timeInForceCodeField != value)
				{
					this.timeInForceCodeField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("TimeInForceCode"));
				}
			}
		}

		/// <summary>
		/// Copy the values from the data model.
		/// </summary>
		/// <param name="workingOrderRow">The data model row that is the source of the data.</param>
		public override void Copy(DataModel.WorkingOrderRow workingOrderRow)
		{

			// Validate the parameters.
			if (workingOrderRow == null)
				throw new ArgumentNullException("workingOrderRow");

			// Allow the base class to copy the core of the working order.
			base.Copy(workingOrderRow);

			// Copy the scalar that are specific to the Equity instruments directly from the data model.
			this.IsCrossing = workingOrderRow.CrossingRow.CrossingCode == CrossingCode.AlwaysMatch;
			this.IsBrokerMatch = workingOrderRow.IsBrokerMatch;
			this.IsHedgeMatch = workingOrderRow.IsHedgeMatch;
			this.IsInstitutionMatch = workingOrderRow.IsInstitutionMatch;
			this.LimitPrice = workingOrderRow.IsLimitPriceNull() ? 0.0M : workingOrderRow.LimitPrice;
			this.OrderTypeMnemonic = workingOrderRow.OrderTypeRow.Mnemonic;
			this.OrderTypeCode = workingOrderRow.OrderTypeRow.OrderTypeCode;
			this.StopPrice = workingOrderRow.IsStopPriceNull() ? 0.0M : workingOrderRow.StopPrice;
			this.TimeInForceCode = workingOrderRow.TimeInForceRow.TimeInForceCode;

		}

	}

}
