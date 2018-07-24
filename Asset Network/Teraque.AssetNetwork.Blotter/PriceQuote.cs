namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.ComponentModel;

	/// <summary>
	/// A price quote.
	/// </summary>
	public class PriceQuote : INotifyPropertyChanged
	{

		/// <summary>
		/// Indicates that the quote is part of an active record.
		/// </summary>
		Boolean isActiveField;

		/// <summary>
		/// Used to inhibit the first price change.
		/// </summary>
		Boolean isFirstTime = true;
	
		/// <summary>
		/// The previous price.
		/// </summary>
		Decimal lastPriceField;

		/// <summary>
		/// The current price.
		/// </summary>
		Decimal priceField;

		/// <summary>
		/// The time of the last price quote.
		/// </summary>
		DateTime priceTimeField;

		/// <summary>
		/// Notifies listeners that a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Gets or sets the available quantity.
		/// </summary>
		public Boolean IsActive
		{
			get
			{
				return this.isActiveField;
			}
			set
			{
				{
					if (this.isActiveField != value)
					{
						this.isActiveField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("IsActive"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the available quantity.
		/// </summary>
		public Decimal LastPrice
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
		/// Gets or sets the available quantity.
		/// </summary>
		public Decimal Price
		{
			get
			{
				return this.priceField;
			}
			set
			{
				{
					if (this.priceField != value)
					{

						if (this.isFirstTime)
							this.isFirstTime = false;
						else
							this.TickTime = DateTime.Now;
						this.LastPrice = this.Price;
						this.priceField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("Price"));
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the available quantity.
		/// </summary>
		public DateTime TickTime
		{
			get
			{
				return this.priceTimeField;
			}
			set
			{
				{
					if (this.priceTimeField != value)
					{
						this.priceTimeField = value;
						this.OnPropertyChanged(new PropertyChangedEventArgs("TickTime"));
					}
				}
			}
		}

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

	}

}
