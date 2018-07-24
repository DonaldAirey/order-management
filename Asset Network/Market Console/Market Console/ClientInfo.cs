namespace Teraque.AssetNetwork
{

	using System;
	using System.ComponentModel;

	/// <summary>
	/// Information about each endpoint that subscribes to data from the simulatorl
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal class ClientInfo : INotifyPropertyChanged
	{

		/// <summary>
		/// The name of the endpoint configuration.
		/// </summary>
		String endpointField;

		/// <summary>
		/// The user's password.
		/// </summary>
		String passwordField;

		/// <summary>
		/// The user name.
		/// </summary>
		String userNameField;

		/// <summary>
		/// Invoked when the property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Sends out a notification that a property has changed.
		/// </summary>
		/// <param name="propertyChangedEventArgs">The PropertyChanged event data.</param>
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
		{

			// Notify anyone listening that the property has changed.
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, propertyChangedEventArgs);

		}

		/// <summary>
		/// The name of the endpoint configuration.
		/// </summary>
		public String EndpointConfigurationName
		{
			get
			{
				return this.endpointField;
			}
			set
			{
				if (this.endpointField != value)
				{
					this.endpointField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Endpoint"));
				}
			}
		}

		/// <summary>
		/// The user's password.
		/// </summary>
		public String Password
		{
			get
			{
				return this.passwordField;
			}
			set
			{
				if (this.passwordField != value)
				{
					this.passwordField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Password"));
				}
			}
		}

		/// <summary>
		/// The user name.
		/// </summary>
		public String UserName
		{
			get
			{
				return this.userNameField;
			}
			set
			{
				if (this.userNameField != value)
				{
					this.userNameField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("UserName"));
				}
			}
		}

	}

}
