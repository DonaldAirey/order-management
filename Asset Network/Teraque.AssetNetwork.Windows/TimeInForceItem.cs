namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using Teraque.Windows;

	/// <summary>
	/// A TimeInForce record.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class TimeInForceItem : INotifyPropertyChanged
	{

		/// <summary>
		/// The description.
		/// </summary>
		String description;

		/// <summary>
		/// The mnemonic.
		/// </summary>
		String mnemonic;

		/// <summary>
		/// The sort order.
		/// </summary>
		Int32 sortOrder;

		/// <summary>
		/// The time in force.
		/// </summary>
		TimeInForceCode timeInForceCode;

		/// <summary>
		/// Notifies listeners that a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Initialize a new instance of a TimeInForceItem class with the information from the data model.
		/// </summary>
		/// <param name="timeInForceRow">The source of the information for the TimeInForceRow.</param>
		internal TimeInForceItem(DataModel.TimeInForceRow timeInForceRow)
		{

			// This will populate the new item with information from the data model.
			this.Copy(timeInForceRow);

		}

		/// <summary>
		/// Copies the values from the data model into the TimeInForceItem instance.
		/// </summary>
		/// <param name="timeInForceRow">The source of the data.</param>
		internal void Copy(DataModel.TimeInForceRow timeInForceRow)
		{

			// Copy the members from the data model.
			this.Description = timeInForceRow.Description;
			this.Mnemonic = timeInForceRow.Mnemonic;
			this.SortOrder = timeInForceRow.SortOrder;
			this.TimeInForceCode = timeInForceRow.TimeInForceCode;

		}

		/// <summary>
		/// Occurs when a property has changed.
		/// </summary>
		/// <param name="propertyChangedEventArgs">The event data.</param>
		void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
		{

			// This will notify anyone listening that the property has changed.
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, propertyChangedEventArgs);

		}

		/// <summary>
		/// TimeInForceCode
		/// </summary>
		public TimeInForceCode TimeInForceCode
		{
			get
			{
				return this.timeInForceCode;
			}
			set
			{
				if ((this.timeInForceCode != value))
				{
					this.timeInForceCode = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("TimeInForceCode"));
				}
			}
		}

		/// <summary>
		/// Mnemonic
		/// </summary>
		public string Mnemonic
		{
			get
			{
				return this.mnemonic;
			}
			set
			{
				if ((this.mnemonic != value))
				{
					this.mnemonic = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Mnemonic"));
				}
			}
		}

		/// <summary>
		/// Description
		/// </summary>
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				if ((this.description != value))
				{
					this.description = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Description"));
				}
			}
		}

		/// <summary>
		/// SortOrder
		/// </summary>
		public Int32 SortOrder
		{
			get
			{
				return this.sortOrder;
			}
			set
			{
				if ((this.sortOrder != value))
				{
					this.sortOrder = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("SortOrder"));
				}
			}
		}

	}

}
