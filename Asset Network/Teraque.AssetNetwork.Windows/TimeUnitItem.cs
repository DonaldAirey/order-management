namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.ComponentModel;

	/// <summary>
	/// Represents an item in the TimeUnitList.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class TimeUnitItem : INotifyPropertyChanged
	{

		/// <summary>
		/// The name.
		/// </summary>
		String name;

		/// <summary>
		/// The TimeUnit code.
		/// </summary>
		TimeUnitCode timeUnitCode;

		/// <summary>
		/// Notifies listeners that a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Initialize a new instance of a TimeUnitItem class with the information from the data model.
		/// </summary>
		/// <param name="timeUnitRow">The source of the information for the TimeUnitRow.</param>
		internal TimeUnitItem(DataModel.TimeUnitRow timeUnitRow)
		{

			// This will populate the new item with information from the data model.
			this.Copy(timeUnitRow);

		}

		/// <summary>
		/// Copies the values from the data model into the TimeUnitItem instance.
		/// </summary>
		/// <param name="timeUnitRow">The source of the data.</param>
		internal void Copy(DataModel.TimeUnitRow timeUnitRow)
		{

			// Copy the members from the data model.
			this.Name = timeUnitRow.Name;
			this.TimeUnitCode = timeUnitRow.TimeUnitCode;

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
		/// The TimeUnit value.
		/// </summary>
		public TimeUnitCode TimeUnitCode
		{
			get
			{
				return this.timeUnitCode;
			}
			set
			{
				if ((this.timeUnitCode != value))
				{
					this.timeUnitCode = value;
					if ((this.PropertyChanged != null))
						this.PropertyChanged(this, new PropertyChangedEventArgs("TimeUnitCode"));
				}
			}
		}

	}

}
