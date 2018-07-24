namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Windows;
	using Teraque;

	/// <summary>
	/// The Model View for generic Working Orders.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class WorkingOrder :  INotifyPropertyChanged, IEditableObject
	{

		/// <summary>
		/// Indicates whether this working order is active or not.
		/// </summary>
		Boolean isActiveField;

		/// <summary>
		/// Ask Price.
		/// </summary>
		PriceQuote askPriceField = new PriceQuote();

		/// <summary>
		/// Avaiable Quantity.
		/// </summary>
		Decimal availableQuantityField;

		/// <summary>
		/// Bid Price.
		/// </summary>
		PriceQuote bidPriceField = new PriceQuote();

		/// <summary>
		/// The name of the blotter.
		/// </summary>
		String blotterNameField;

		/// <summary>
		/// Created By User.
		/// </summary>
		String createdByField;

		/// <summary>
		/// Created time.
		/// </summary>
		DateTime createdTimeField;

		/// <summary>
		/// Destination Order Quantity.
		/// </summary>
		Decimal destinationOrderQuantityField;

		/// <summary>
		/// Execution Order Quantity.
		/// </summary>
		Decimal executionQuantityField;

		/// <summary>
		/// Indicates whether this item is on an odd or even boundary.
		/// </summary>
		Boolean isEvenField;

		/// <summary>
		/// Last Price.
		/// </summary>
		PriceQuote lastPriceField = new PriceQuote();

		/// <summary>
		/// Leaves quantity.
		/// </summary>
		Decimal leavesQuantityField;

		/// <summary>
		/// Market value.
		/// </summary>
		Decimal marketValueField;

		/// <summary>
		/// Modified by user.
		/// </summary>
		String modifiedByField;

		/// <summary>
		/// Modified time.
		/// </summary>
		DateTime modifiedTimeField;

		/// <summary>
		/// RowVersion.
		/// </summary>
		Int64 rowVersionField;

		/// <summary>
		/// The name of the security.
		/// </summary>
		String securityNameField;

		/// <summary>
		/// The settlement date.
		/// </summary>
		DateTime settlementDateField;

		/// <summary>
		/// The side of the order.
		/// </summary>
		SideCode sideCodeField;

		/// <summary>
		/// The side of the order.
		/// </summary>
		String sideMnemonicField;

		/// <summary>
		/// Source order quantity.
		/// </summary>
		Decimal sourceOrderQuantityField;

		/// <summary>
		/// Status code.
		/// </summary>
		StatusCode statusCodeField;

		/// <summary>
		/// The common symbol of the security.
		/// </summary>
		String symbolField;

		/// <summary>
		/// The trade date.
		/// </summary>
		DateTime tradeDateField;

		/// <summary>
		/// The unique working order identifier.
		/// </summary>
		Guid workingOrderIdField;

		/// <summary>
		/// Notifies listeners that a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Initializes a new instance of the WorkingOrder class from a record in the data model.
		/// </summary>
		/// <param name="workingOrderRow">The working order record in the data model.</param>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public WorkingOrder(DataModel.WorkingOrderRow workingOrderRow)
		{

			// The new instance is initialized with a copy of the data from the data model.
			this.Copy(workingOrderRow);

		}

		/// <summary>
		/// Gets or sets the ask price.
		/// </summary>
		public PriceQuote AskPrice
		{
			get
			{
				return this.askPriceField;
			}
			set
			{
				{
					if (this.askPriceField != value)
					{
						this.askPriceField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("AskPrice"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the available quantity.
		/// </summary>
		public Decimal AvailableQuantity
		{
			get
			{
				return this.availableQuantityField;
			}
			set
			{
				{
					if (this.availableQuantityField != value)
					{
						this.availableQuantityField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("AvailableQuantity"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the bid price.
		/// </summary>
		public PriceQuote BidPrice
		{
			get
			{
				return this.bidPriceField;
			}
			set
			{
				{
					if (this.bidPriceField != value)
					{
						this.bidPriceField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("BidPrice"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the name of the blotter.
		/// </summary>
		public String BlotterName
		{
			get
			{
				return this.blotterNameField;
			}
			set
			{
				if (this.blotterNameField != value)
				{
					this.blotterNameField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("BlotterName"));
				}

			}
		}

		/// <summary>
		/// Gets or sets the created by user.
		/// </summary>
		public String CreatedBy
		{
			get
			{
				return this.createdByField;
			}
			set
			{
				{
					if (this.createdByField != value)
					{
						this.createdByField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("CreatedBy"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the created time.
		/// </summary>
		public DateTime CreatedTime
		{
			get
			{
				return this.createdTimeField;
			}
			set
			{
				{
					if (this.createdTimeField != value)
					{
						this.createdTimeField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("CreatedTime"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the destination order quantity.
		/// </summary>
		public Decimal DestinationOrderQuantity
		{
			get
			{
				return this.destinationOrderQuantityField;
			}
			set
			{
				{
					if (this.destinationOrderQuantityField != value)
					{
						this.destinationOrderQuantityField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("DestinationOrderQuantity"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the execution quantity.
		/// </summary>
		public Decimal ExecutionQuantity
		{
			get
			{
				return this.executionQuantityField;
			}
			set
			{
				{
					if (this.executionQuantityField != value)
					{
						this.executionQuantityField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("ExecutionQuantity"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets an indication of whether the working order is active or not.
		/// </summary>
		public Boolean IsActive
		{
			get
			{
				return this.isActiveField;
			}
			set
			{
				if (this.isActiveField != value)
				{
					this.isActiveField = value;
					this.AskPrice.IsActive = value;
					this.BidPrice.IsActive = value;
					this.LastPrice.IsActive = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("IsActive"));
				}
			}
		}

		/// <summary>
		/// Gets or sets an indication of whether the row is on an even or odd boundary in the view.
		/// </summary>
		public Boolean IsEven
		{
			get
			{
				return this.isEvenField;
			}
			set
			{
				if (this.isEvenField != value)
				{
					this.isEvenField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("IsEven"));
				}
			}
		}

		/// <summary>
		/// Gets or sets the last price.
		/// </summary>
		public PriceQuote LastPrice
		{
			get
			{
				return this.lastPriceField;
			}
			set
			{
				{
					if (this.lastPriceField != value)
					{
						this.lastPriceField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("LastPrice"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the leaves quantity.
		/// </summary>
		public Decimal LeavesQuantity
		{
			get
			{
				return this.leavesQuantityField;
			}
			set
			{
				{
					if (this.leavesQuantityField != value)
					{
						this.leavesQuantityField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("LeavesQuantity"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the market value.
		/// </summary>
		public Decimal MarketValue
		{
			get
			{
				return this.marketValueField;
			}
			set
			{
				{
					if (this.marketValueField != value)
					{
						this.marketValueField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("MarketValue"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the modified by user.
		/// </summary>
		public String ModifiedBy
		{
			get
			{
				return this.modifiedByField;
			}
			set
			{
				{
					if (this.modifiedByField != value)
					{
						this.modifiedByField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("ModifiedBy"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the modified time.
		/// </summary>
		public DateTime ModifiedTime
		{
			get
			{
				return this.modifiedTimeField;
			}
			set
			{
				{
					if (this.modifiedTimeField != value)
					{
						this.modifiedTimeField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("ModifiedTime"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the modified time.
		/// </summary>
		public Int64 RowVersion
		{
			get
			{
				return this.rowVersionField;
			}
			set
			{
				{
					if (this.rowVersionField != value)
					{
						this.rowVersionField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("RowVersion"));
					}
				}
			}
		}

		/// <summary>
		/// The name of the security.
		/// </summary>
		public String SecurityName
		{
			get
			{
				return this.securityNameField;
			}
			set
			{
				if (this.securityNameField != value)
				{
					this.securityNameField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("SecurityName"));
				}

			}
		}

		/// <summary>
		/// The name of the security.
		/// </summary>
		public DateTime SettlementDate
		{
			get
			{
				return this.settlementDateField;
			}
			set
			{
				if (this.settlementDateField != value)
				{
					this.settlementDateField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("SettlementDate"));
				}
			}
		}

		/// <summary>
		/// The name of the side code.
		/// </summary>
		public SideCode SideCode
		{
			get
			{
				return this.sideCodeField;
			}
			set
			{
				if (this.sideCodeField != value)
				{
					this.sideCodeField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("SideCode"));
				}
			}
		}

		/// <summary>
		/// Gets or sets the side mnemonic.
		/// </summary>
		public String SideMnemonic
		{
			get
			{
				return this.sideMnemonicField;
			}
			set
			{
				{
					if (this.sideMnemonicField != value)
					{
						this.sideMnemonicField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("SideMnemonic"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the source order quantity.
		/// </summary>
		public Decimal SourceOrderQuantity
		{
			get
			{
				return this.sourceOrderQuantityField;
			}
			set
			{
				{
					if (this.sourceOrderQuantityField != value)
					{
						this.sourceOrderQuantityField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("SourceOrderQuantity"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the status code.
		/// </summary>
		public StatusCode StatusCode
		{
			get
			{
				return this.statusCodeField;
			}
			set
			{
				{
					if (this.statusCodeField != value)
					{
						this.statusCodeField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("StatusCode"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the symbol.
		/// </summary>
		public String Symbol
		{
			get
			{
				return this.symbolField;
			}
			set
			{
				if (this.symbolField != value)
				{
					this.symbolField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Symbol"));
				}
			}
		}

		/// <summary>
		/// Gets or sets the trade date.
		/// </summary>
		public DateTime TradeDate
		{
			get
			{
				return this.tradeDateField;
			}
			set
			{
				if (this.tradeDateField != value)
				{
					this.tradeDateField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("TradeDate"));
				}
			}
		}

		/// <summary>
		/// Gets or sets the unique working order identifier.
		/// </summary>
		public Guid WorkingOrderId
		{
			get
			{
				return this.workingOrderIdField;
			}
			set
			{
				if (this.workingOrderIdField != value)
				{
					this.workingOrderIdField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("WorkingOrderId"));
				}
			}
		}

		/// <summary>
		/// Begins an edit on an object.
		/// </summary>
		public void BeginEdit() { }

		/// <summary>
		/// Copy the values from the data model.
		/// </summary>
		/// <param name="workingOrderRow">The data model row that is the source of the data.</param>
		public virtual void Copy(DataModel.WorkingOrderRow workingOrderRow)
		{

			// Validate the parameters.
			if (workingOrderRow == null)
				throw new ArgumentNullException("workingOrderRow");

			// Find the price associated with the security in this order and copy.
			DataModel.PriceRow priceRow = DataModel.Price.PriceKey.Find(
				workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SecurityId.SecurityId,
				workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SettlementId.SecurityId);
			if (priceRow != null)
			{
				this.AskPrice.Price = priceRow.AskPrice;
				this.BidPrice.Price = priceRow.BidPrice;
				this.LastPrice.Price = priceRow.LastPrice;
			}

			// Any order that is not filled is considered active.
			this.IsActive = workingOrderRow.StatusRow.StatusCode != StatusCode.Filled;

			// Copy the scalar values directly from the data model.
			this.BlotterName = workingOrderRow.BlotterRow.EntityRow.Name;
			this.CreatedBy = workingOrderRow.UserRowByFK_User_WorkingOrder_CreatedUserId.EntityRow.Name;
			this.CreatedTime = workingOrderRow.CreatedTime;
			this.ModifiedBy = workingOrderRow.UserRowByFK_User_WorkingOrder_ModifiedUserId.EntityRow.Name;
			this.ModifiedTime = workingOrderRow.ModifiedTime;
			this.RowVersion = workingOrderRow.RowVersion;
			this.SecurityName = workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SecurityId.EntityRow.Name;
			this.SideCode = workingOrderRow.SideRow.SideCode;
			this.SideMnemonic = workingOrderRow.SideRow.Mnemonic;
			this.SettlementDate = workingOrderRow.SettlementDate;
			this.StatusCode = workingOrderRow.StatusRow.StatusCode;
			this.Symbol = workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SecurityId.Symbol;
			this.TradeDate = workingOrderRow.TradeDate;
			this.WorkingOrderId = workingOrderRow.WorkingOrderId;

			// These factors are needed to compute the proper quantities and prices.
			Decimal quantityFactor = workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SecurityId.QuantityFactor;
			Decimal priceFactor = workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SecurityId.PriceFactor;

			// Aggregate the Destiantion Order and Execution Quantities.
			Decimal destinationOrderQuantity = 0.0m;
			Decimal executionQuantity = 0.0m;
			foreach (DataModel.DestinationOrderRow destinationOrderRow in workingOrderRow.GetDestinationOrderRows())
			{
				destinationOrderQuantity += destinationOrderRow.OrderedQuantity;
				foreach (DataModel.ExecutionRow executionRow in destinationOrderRow.GetExecutionRows())
					executionQuantity += executionRow.ExecutionQuantity;
			}
			this.DestinationOrderQuantity = destinationOrderQuantity;
			this.ExecutionQuantity = executionQuantity;

			// Aggregate the Source Order Quantity.
			Decimal sourceOrderQuantity = 0.0m;
			foreach (DataModel.SourceOrderRow sourceOrderRow in workingOrderRow.GetSourceOrderRows())
				sourceOrderQuantity += sourceOrderRow.OrderedQuantity;
			this.SourceOrderQuantity = sourceOrderQuantity;

			// These derived values must be calcualted after the value columns.
			this.AvailableQuantity = this.SourceOrderQuantity - this.DestinationOrderQuantity;
			this.MarketValue = sourceOrderQuantity * quantityFactor * this.LastPrice.Price * priceFactor;
			this.LeavesQuantity = this.DestinationOrderQuantity - this.ExecutionQuantity;

		}

		/// <summary>
		/// Discards changes since the last BeginEdit call.
		/// </summary>
		public void CancelEdit() { }

		/// <summary>
		/// Pushes changes since the last BeginEdit or IBindingList.AddNew call into the underlying object.
		/// </summary>
		public void EndEdit() { }

		/// <summary>
		/// Occurs when a property has changed.
		/// </summary>
		/// <param name="propertyChangedEventArgs">The event data.</param>
		public void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
		{

			// This will notify anyone listening that the property has changed.
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, propertyChangedEventArgs);

		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "{{{0} {1} of {2}}}", this.SideCode, this.SourceOrderQuantity, this.Symbol);
		}

	}

}
