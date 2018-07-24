namespace Teraque.Windows
{

	using System;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Xml.Linq;

	/// <summary>
	/// A Consumer.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class Consumer : INotifyPropertyChanged, IEditableObject
	{

		/// <summary>
		/// Address
		/// </summary>
		String addressField;

		/// <summary>
		/// City
		/// </summary>
		String cityField;

		/// <summary>
		/// Date of birth.
		/// </summary>
		DateTime dateOfBirthField;

		/// <summary>
		/// First name.
		/// </summary>
		String firstNameField;

		/// <summary>
		/// Last name.
		/// </summary>
		String lastNameField;

		/// <summary>
		/// Is even or odd.
		/// </summary>
		Boolean isEvenField;

		/// <summary>
		/// Phone number.
		/// </summary>
		String phoneField;

		/// <summary>
		/// Social Security Number.
		/// </summary>
		String socialSecurityNumberField;

		/// <summary>
		/// Salutation.
		/// </summary>
		String salutationField;

		/// <summary>
		/// State.
		/// </summary>
		String stateField;

		/// <summary>
		/// Zip code.
		/// </summary>
		String zipCodeField;

		/// <summary>
		/// Notifies when a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Initializes a new instance of the Consumer class.
		/// </summary>
		/// <param name="xElement">The XElement containing the consumer data.</param>
		public Consumer(XElement xElement)
		{

			// Initialize the object.
			this.Address = xElement.Attribute("Address").Value;
			this.City = xElement.Attribute("City").Value;
			this.DateOfBirth = xElement.Attribute("DateOfBirth").Value == String.Empty ? DateTime.MinValue : DateTime.Parse(xElement.Attribute("DateOfBirth").Value);
			this.FirstName = xElement.Attribute("FirstName").Value;
			this.LastName = xElement.Attribute("LastName").Value;
			this.Phone = xElement.Attribute("Phone").Value;
			this.Salutation = xElement.Attribute("Salutation").Value;
			this.SocialSecurityNumber = xElement.Attribute("SSN").Value;
			this.State = xElement.Attribute("State").Value;
			this.ZipCode = xElement.Attribute("ZipCode").Value;
			if (this.zipCodeField.Length > 5)
				this.zipCodeField = zipCodeField.Substring(0, 5);

		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return String.Format("{{{0}, {1} {2}}}", this.LastName, this.FirstName, this.SocialSecurityNumber);
		}

		/// <summary>
		/// Address
		/// </summary>
		public String Address
		{
			get
			{
				return this.addressField;
			}
			set
			{
				if (this.addressField != value)
				{
					this.addressField = value;
					if (this.PropertyChanged != null)
						this.PropertyChanged(this, new PropertyChangedEventArgs("Address"));
				}
			}
		}

		/// <summary>
		/// City
		/// </summary>
		public String City
		{
			get
			{
				return this.cityField;
			}
			set
			{
				if (this.cityField != value)
				{
					this.cityField = value;
					if (this.PropertyChanged != null)
						this.PropertyChanged(this, new PropertyChangedEventArgs("City"));
				}
			}
		}

		/// <summary>
		/// DateOfBirth
		/// </summary>
		public DateTime DateOfBirth
		{
			get
			{
				return this.dateOfBirthField;
			}
			set
			{
				if (this.dateOfBirthField != value)
				{
					this.dateOfBirthField = value;
					if (this.PropertyChanged != null)
						this.PropertyChanged(this, new PropertyChangedEventArgs("DateOfBirth"));
				}
			}
		}

		/// <summary>
		/// FirstName
		/// </summary>
		public String FirstName
		{
			get
			{
				return this.firstNameField;
			}
			set
			{
				if (this.firstNameField != value)
				{
					this.firstNameField = value;
					if (this.PropertyChanged != null)
						this.PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
				}
			}
		}

		/// <summary>
		/// LastName
		/// </summary>
		public String LastName
		{
			get
			{
				return this.lastNameField;
			}
			set
			{
				if (this.lastNameField != value)
				{
					this.lastNameField = value;
					if (this.PropertyChanged != null)
						this.PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
				}
			}
		}

		/// <summary>
		/// IsEven
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
					if (this.PropertyChanged != null)
						this.PropertyChanged(this, new PropertyChangedEventArgs("IsEven"));
				}
			}
		}

		/// <summary>
		/// Phone
		/// </summary>
		public String Phone
		{
			get
			{
				return this.phoneField;
			}
			set
			{
				if (this.phoneField != value)
				{
					this.phoneField = value;
					if (this.PropertyChanged != null)
						this.PropertyChanged(this, new PropertyChangedEventArgs("Phone"));
				}
			}
		}

		/// <summary>
		/// ZipCode
		/// </summary>
		public String ZipCode
		{
			get
			{
				return this.zipCodeField;
			}
			set
			{
				if (this.zipCodeField != value)
				{
					this.zipCodeField = value;
					if (this.PropertyChanged != null)
						this.PropertyChanged(this, new PropertyChangedEventArgs("ZipCode"));
				}
			}
		}

		/// <summary>
		/// SocialSecurityNumber
		/// </summary>
		public String SocialSecurityNumber
		{
			get
			{
				return this.socialSecurityNumberField;
			}
			set
			{
				if (this.socialSecurityNumberField != value)
				{
					this.socialSecurityNumberField = value;
					if (this.PropertyChanged != null)
						this.PropertyChanged(this, new PropertyChangedEventArgs("SocialSecurityNumber"));
				}
			}
		}

		/// <summary>
		/// Salutation
		/// </summary>
		public String Salutation
		{
			get
			{
				return this.salutationField;
			}
			set
			{
				if (this.salutationField != value)
				{
					this.salutationField = value;
					if (this.PropertyChanged != null)
						this.PropertyChanged(this, new PropertyChangedEventArgs("Salutation"));
				}
			}
		}

		/// <summary>
		/// State
		/// </summary>
		public String State
		{
			get
			{
				return this.stateField;
			}
			set
			{
				if (this.stateField != value)
				{
					this.stateField = value;
					if (this.PropertyChanged != null)
						this.PropertyChanged(this, new PropertyChangedEventArgs("State"));
				}
			}
		}

		public void BeginEdit()
		{
		}

		public void CancelEdit()
		{
		}

		public void EndEdit()
		{
		}

	}

}
