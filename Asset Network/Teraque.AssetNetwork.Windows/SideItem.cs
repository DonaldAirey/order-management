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
	/// A Side record.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class SideItem : INotifyPropertyChanged
	{

		/// <summary>
		/// The description.
		/// </summary>
		String descriptionField;

		/// <summary>
		/// The extra large image source for this item.
		/// </summary>
		ImageSource extraLargeImageSourceField;

		/// <summary>
		/// The large image source for this item.
		/// </summary>
		ImageSource largeImageSourceField;

		/// <summary>
		/// The medium image source for this item.
		/// </summary>
		ImageSource mediumImageSourceField;

		/// <summary>
		/// The mnemonic.
		/// </summary>
		String mnemonicField;

		/// <summary>
		/// The small image source for this item.
		/// </summary>
		ImageSource smallImageSourceField;

		/// <summary>
		/// The sort order.
		/// </summary>
		Int32 sortOrderField;

		/// <summary>
		/// The side code.
		/// </summary>
		SideCode sideCode;

		/// <summary>
		/// Notifies listeners that a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Initialize a new instance of a SideItem class with the information from the data model.
		/// </summary>
		/// <param name="sideRow">The source of the information for the SideRow.</param>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		internal SideItem(DataModel.SideRow sideRow)
		{

			// This will populate the new item with information from the data model.
			this.Copy(sideRow);

		}

		/// <summary>
		/// Copies the values from the data model into the SideItem instance.
		/// </summary>
		/// <param name="sideRow">The source of the data.</param>
		internal void Copy(DataModel.SideRow sideRow)
		{

			// Copy the members from the data model.
			this.Description = sideRow.Description;
			this.Mnemonic = sideRow.Mnemonic;
			this.SideCode = sideRow.SideCode;
			this.SortOrder = sideRow.SortOrder;

			// This will disassemble the icon and give us the basic image sizes supported by the application framework.
			Dictionary<ImageSize, ImageSource> images = ImageHelper.DecodeIcon(Convert.FromBase64String(sideRow.Image));
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
		/// The extra large image source.
		/// </summary>
		public virtual ImageSource ExtraLargeImageSource
		{
			get
			{
				return this.extraLargeImageSourceField;
			}
			set
			{
				if (this.extraLargeImageSourceField != value)
				{
					this.extraLargeImageSourceField = value;
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
				return this.largeImageSourceField;
			}

			set
			{
				if (this.largeImageSourceField != value)
				{
					this.largeImageSourceField = value;
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
				return this.mediumImageSourceField;
			}
			set
			{
				if (this.mediumImageSourceField != value)
				{
					this.mediumImageSourceField = value;
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
				return this.mnemonicField;
			}
			set
			{
				if (this.mnemonicField != value)
				{
					this.mnemonicField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Mnemonic"));
				}
			}
		}

		/// <summary>
		/// SideCode
		/// </summary>
		public SideCode SideCode
		{
			get
			{
				return this.sideCode;
			}
			set
			{
				if (this.sideCode != value)
				{
					this.sideCode = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("SideCode"));
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
				return this.smallImageSourceField;
			}
			set
			{
				if (this.smallImageSourceField != value)
				{
					this.OnPropertyChanged(new PropertyChangedEventArgs("SmallImageSource"));
					this.smallImageSourceField = value;
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
				return this.sortOrderField;
			}
			set
			{
				if (this.sortOrderField != value)
				{
					this.sortOrderField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("SortOrder"));
				}
			}
		}

	}

}
