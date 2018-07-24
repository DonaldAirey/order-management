namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.ComponentModel;

	/// <summary>
	/// Represents an item in the SettlementUnitList.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class SettlementUnitItem : INotifyPropertyChanged
	{

		/// <summary>
		/// The name.
		/// </summary>
		String name;

		/// <summary>
		/// The Settlement Unit Code.
		/// </summary>
		SettlementUnitCode settlementUnitCode;

		/// <summary>
		/// Notifies listeners that a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Initialize a new instance of a SettlementUnitItem class with the information from the data model.
		/// </summary>
		/// <param name="settlementUnitRow">The source of the information for the SettlementUnitRow.</param>
		internal SettlementUnitItem(DataModel.SettlementUnitRow settlementUnitRow)
		{

			// This will populate the new item with information from the data model.
			this.Copy(settlementUnitRow);

		}

		/// <summary>
		/// Copies the values from the data model into the SettlementUnitItem instance.
		/// </summary>
		/// <param name="settlementUnitRow">The source of the data.</param>
		internal void Copy(DataModel.SettlementUnitRow settlementUnitRow)
		{

			// Copy the members from the data model.  This will also notify anyone listening for the change.
			this.Name = settlementUnitRow.Name;
			this.SettlementUnitCode = settlementUnitRow.SettlementUnitCode;

		}

		/// <summary>
		/// The name.
		/// </summary>
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if ((this.name != value))
				{
					this.name = value;
					if ((this.PropertyChanged != null))
						this.PropertyChanged(this, new PropertyChangedEventArgs("Name"));
				}
			}
		}

		/// <summary>
		/// The SettlementUnit value.
		/// </summary>
		public SettlementUnitCode SettlementUnitCode
		{
			get
			{
				return this.settlementUnitCode;
			}
			set
			{
				if ((this.settlementUnitCode != value))
				{
					this.settlementUnitCode = value;
					if ((this.PropertyChanged != null))
						this.PropertyChanged(this, new PropertyChangedEventArgs("SettlementUnitCode"));
				}
			}
		}

	}

}
