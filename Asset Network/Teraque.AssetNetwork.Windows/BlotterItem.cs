namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.ComponentModel;
	using Teraque.Windows;

	/// <summary>
	/// A Blotter Record.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class BlotterItem : INotifyPropertyChanged
	{

		/// <summary>
		/// The unique identifier of the blotter.
		/// </summary>
		Guid blotterIdField;

		/// <summary>
		/// The blotter description.
		/// </summary>
		String descriptionField;

		/// <summary>
		/// The blotter name.
		/// </summary>
		String nameField;

		/// <summary>
		/// Notifies listeners that a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Initialize a new instance of a BlotterItem class with the information from the data model.
		/// </summary>
		/// <param name="blotterRow">The source of the information for the BlotterRow.</param>
		internal BlotterItem(DataModel.BlotterRow blotterRow)
		{

			// This will populate the new item with information from the data model.
			this.Copy(blotterRow);

		}

		/// <summary>
		/// Copies the values from the data model into the BlotterItem instance.
		/// </summary>
		/// <param name="blotterRow">The source of the data.</param>
		internal void Copy(DataModel.BlotterRow blotterRow)
		{

			// Copy the members from the data model.
			this.Description = blotterRow.EntityRow.IsDescriptionNull() ? String.Empty : blotterRow.EntityRow.Description;
			this.Name = blotterRow.EntityRow.Name;
			this.BlotterId = blotterRow.BlotterId;

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
		/// Gets or sets the unique Blotter identifier.
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
		/// Description
		/// </summary>
		public String Description
		{
			get
			{
				return this.descriptionField;
			}
			set
			{
				if (this.descriptionField != value)
				{
					this.descriptionField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Description"));
				}
			}
		}

		/// <summary>
		/// Name
		/// </summary>
		public String Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				if (this.nameField != value)
				{
					this.nameField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Name"));
				}
			}
		}

	}

}
