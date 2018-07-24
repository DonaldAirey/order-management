namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using Teraque.Windows;

	/// <summary>
	/// A Status record.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class StatusItem : INotifyPropertyChanged
	{

		/// <summary>
		/// The description.
		/// </summary>
		String description;

		/// <summary>
		/// The extra large image source for this item.
		/// </summary>
		ImageSource extraLargeImageSource;

		/// <summary>
		/// The large image source for this item.
		/// </summary>
		ImageSource largeImageSource;

		/// <summary>
		/// The medium image source for this item.
		/// </summary>
		ImageSource mediumImageSource;

		/// <summary>
		/// The mnemonic.
		/// </summary>
		String mnemonic;

		/// <summary>
		/// The small image source for this item.
		/// </summary>
		ImageSource smallImageSource;

		/// <summary>
		/// The sort order.
		/// </summary>
		Int32 sortOrder;

		/// <summary>
		/// The status code.
		/// </summary>
		StatusCode statusCode;

		/// <summary>
		/// Notifies listeners that a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Initialize a new instance of a StatusItem class with the information from the data model.
		/// </summary>
		/// <param name="statusRow">The source of the information for the StatusRow.</param>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		internal StatusItem(DataModel.StatusRow statusRow)
		{

			// This will populate the new item with information from the data model.
			this.Copy(statusRow);

		}

		/// <summary>
		/// Copies the values from the data model into the StatusItem instance.
		/// </summary>
		/// <param name="statusRow">The source of the data.</param>
		internal void Copy(DataModel.StatusRow statusRow)
		{

			// Copy the members from the data model.
			this.Description = statusRow.Description;
			this.Mnemonic = statusRow.Mnemonic;
			this.StatusCode = statusRow.StatusCode;
			this.SortOrder = statusRow.SortOrder;

			// This will disassemble the icon and give us the basic image sizes supported by the application framework.
			Dictionary<ImageSize, ImageSource> images = ImageHelper.DecodeIcon(Convert.FromBase64String(statusRow.Image));
			this.SmallImageSource = images[ImageSize.Small];
			this.MediumImageSource = images[ImageSize.Medium];
			this.LargeImageSource = images[ImageSize.Large];
			this.ExtraLargeImageSource = images[ImageSize.ExtraLarge];

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
				return this.description;
			}
			set
			{
				if (this.description != value)
				{
					this.description = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Description"));
				}
			}
		}

		/// <summary>
		/// The extra large image source.
		/// </summary>
		public virtual ImageSource ExtraLargeImageSource
		{
			get
			{
				return this.extraLargeImageSource;
			}
			set
			{
				if (this.extraLargeImageSource != value)
				{
					this.extraLargeImageSource = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("ExtraLargeImageSource"));
				}
			}
		}

		/// <summary>
		/// The extra large image source.
		/// </summary>
		public virtual ImageSource LargeImageSource
		{
			get
			{
				return this.largeImageSource;
			}

			set
			{
				if (this.largeImageSource != value)
				{
					this.largeImageSource = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("LargeImageSource"));
				}
			}
		}

		/// <summary>
		/// The medium image source.
		/// </summary>
		public virtual ImageSource MediumImageSource
		{
			get
			{
				return this.mediumImageSource;
			}
			set
			{
				if (this.mediumImageSource != value)
				{
					this.mediumImageSource = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("MediumImageSource"));
				}
			}
		}

		/// Mnemonic
		/// </summary>
		public String Mnemonic
		{
			get
			{
				return this.mnemonic;
			}
			set
			{
				if (this.mnemonic != value)
				{
					this.mnemonic = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Mnemonic"));
				}
			}
		}

		/// <summary>
		/// StatusCode
		/// </summary>
		public StatusCode StatusCode
		{
			get
			{
				return this.statusCode;
			}
			set
			{
				if (this.statusCode != value)
				{
					this.statusCode = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("StatusCode"));
				}
			}
		}

		/// <summary>
		/// <summary>
		/// The Small Image Source.
		/// </summary>
		public virtual ImageSource SmallImageSource
		{
			get
			{
				return this.smallImageSource;
			}
			set
			{
				if (this.smallImageSource != value)
				{
					this.OnPropertyChanged(new PropertyChangedEventArgs("SmallImageSource"));
					this.smallImageSource = value;
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
				if (this.sortOrder != value)
				{
					this.sortOrder = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("SortOrder"));
				}
			}
		}

	}

}
