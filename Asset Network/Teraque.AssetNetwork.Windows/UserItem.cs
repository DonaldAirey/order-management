namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.ComponentModel;
	using Teraque.Windows;

	/// <summary>
	/// A User Record.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class UserItem : INotifyPropertyChanged
	{

		/// <summary>
		/// The user description.
		/// </summary>
		String descriptionField;

		/// <summary>
		/// The user name.
		/// </summary>
		String nameField;

		/// <summary>
		/// The unique identifier of the user.
		/// </summary>
		Guid userIdField;

		/// <summary>
		/// Notifies listeners that a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Initialize a new instance of a UserItem class with the information from the data model.
		/// </summary>
		/// <param name="userRow">The source of the information for the UserRow.</param>
		internal UserItem(DataModel.UserRow userRow)
		{

			// This will populate the new item with information from the data model.
			this.Copy(userRow);

		}

		/// <summary>
		/// Copies the values from the data model into the UserItem instance.
		/// </summary>
		/// <param name="userRow">The source of the data.</param>
		internal void Copy(DataModel.UserRow userRow)
		{

			// Copy the members from the data model.
			this.Description = userRow.EntityRow.IsDescriptionNull() ? String.Empty : userRow.EntityRow.Description;
			this.Name = userRow.EntityRow.Name;
			this.UserId = userRow.UserId;

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

		/// <summary>
		/// Gets or sets the unique User identifier.
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
