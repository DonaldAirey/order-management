namespace Teraque.AssetNetwork
{

	using System;
	using System.ComponentModel;

	/// <summary>
	/// Describes the orders to be generated.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class GeneratorInfo : INotifyPropertyChanged
	{

		/// <summary>
		/// The unique identifier of the blotter to receive the orders.
		/// </summary>
		Guid blotterIdField;

		/// <summary>
		/// The name of the script file to be generated.
		/// </summary>
		String fileNameField;

		/// <summary>
		/// Indicates whether or not Canadian Dollar orders are generated.
		/// </summary>
		Boolean isCanadianDollarField;

		/// <summary>
		/// Indicates whether or not Equity orders are generated.
		/// </summary>
		Boolean isEquityField;

		/// <summary>
		/// Indicates whether or not Fixed Income orders are generated.
		/// </summary>
		Boolean isFixedIncomeField;

		/// <summary>
		/// Indicates whether or not Great Britian Pound orders are generated.
		/// </summary>
		Boolean isGreatBritainPoundField;

		/// <summary>
		/// Indicates whether or not United States Dollar orders are generated.
		/// </summary>
		Boolean isUnitedStatesDollarField;

		/// <summary>
		/// The number of orders to be generated.
		/// </summary>
		Nullable<Int32> orderCountField = new Nullable<Int32>(0);

		/// <summary>
		/// The unique identifier of the user who created the orders.
		/// </summary>
		Guid userIdField;

		/// <summary>
		/// Notifies clients that a property value has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

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
		/// Gets or sets the unique identifier of the botter to recieve the orders.
		/// </summary>
		public Guid BlotterId
		{
			get
			{
				return this.blotterIdField;
			}
			set
			{
				if (this.blotterIdField != value)
				{
					this.blotterIdField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("BlotterId"));
				}
			}
		}

		/// <summary>
		/// Gets or sets the name of the script file where the orders will be written.
		/// </summary>
		public String FileName
		{
			get
			{
				return this.fileNameField;
			}
			set
			{
				if (this.fileNameField != value)
				{
					this.fileNameField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("FileName"));
				}
			}
		}

		/// <summary>
		/// Gets or sets an indication of whether or not Canadian Dollar orders are generated.
		/// </summary>
		public Boolean IsCanadianDollar
		{
			get
			{
				return this.isCanadianDollarField;
			}
			set
			{
				if (this.isCanadianDollarField != value)
				{
					this.isCanadianDollarField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("IsCanadianDollar"));
				}
			}
		}

		/// <summary>
		/// Gets or sets an indication of whether or not Equity orders are generated.
		/// </summary>
		public Boolean IsEquity
		{
			get
			{
				return this.isEquityField;
			}
			set
			{
				if (this.isEquityField != value)
				{
					this.isEquityField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("IsEquity"));
				}
			}
		}

		/// <summary>
		/// Gets or sets an indication of whether or not Fixed Income orders are generated.
		/// </summary>
		public Boolean IsFixedIncome
		{
			get
			{
				return this.isFixedIncomeField;
			}
			set
			{
				if (this.isFixedIncomeField != value)
				{
					this.isFixedIncomeField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("IsFixedIncome"));
				}
			}
		}

		/// <summary>
		/// Gets or sets an indication of whether or not Great Britain Pound orders are generated.
		/// </summary>
		public Boolean IsGreatBritainPound
		{
			get
			{
				return this.isGreatBritainPoundField;
			}
			set
			{
				if (this.isGreatBritainPoundField != value)
				{
					this.isGreatBritainPoundField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("IsGreatBritainPound"));
				}
			}
		}

		/// <summary>
		/// Gets or sets an indication of whether or not United States Dollar orders are generated.
		/// </summary>
		public Boolean IsUnitedStatesDollar
		{
			get
			{
				return this.isUnitedStatesDollarField;
			}
			set
			{
				if (this.isUnitedStatesDollarField != value)
				{
					this.isUnitedStatesDollarField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("IsUnitedStatesDollar"));
				}
			}
		}

		/// <summary>
		/// Gets or sets the number of orders to be generated.
		/// </summary>
		public Nullable<Int32> OrderCount
		{
			get
			{
				return this.orderCountField;
			}
			set
			{
				if (this.orderCountField != value)
				{
					this.orderCountField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("OrderCount"));
				}
			}
		}

		/// <summary>
		/// Gets or sets the unique identifier of the user who created the orders.
		/// </summary>
		public Guid UserId
		{
			get
			{
				return this.userIdField;
			}
			set
			{
				if (this.userIdField != value)
				{
					this.userIdField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("UserId"));
				}
			}
		}

	}

}
