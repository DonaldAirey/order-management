namespace Teraque.Windows.Controls
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media.Imaging;
	using Teraque.Windows.Input;

	/// <summary>
	/// An item describing a filter.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class FilterItem : CheckBox
	{

		/// <summary>
		/// Identifies the GroupName dependency property.
		/// </summary>
		public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(String), typeof(FilterItem));

		/// <summary>
		/// Identifies the Icon dependency property.
		/// </summary>
		public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(Object), typeof(FilterItem));

		/// <summary>
		/// Initializes the FilterItem class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static FilterItem()
		{

			// This allows the templates in resource files to be properly associated with this new class.  Without this override, the type of the base class would
			// be used as the key in any lookup involving resources dictionaries.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterItem), new FrameworkPropertyMetadata(typeof(FilterItem)));

			// This will provide a default command to be invoked when the check box is clicked.
			CheckBox.CommandProperty.OverrideMetadata(typeof(FilterItem), new FrameworkPropertyMetadata(Commands.Filter));

		}

		/// <summary>
		/// Initialize a new instance of the FilterItem class.
		/// </summary>
		public FilterItem()
		{

			// This wil provide a default icon for the filters.  The icon can be overridden by explicilty providing an Icon property when declaring the filter.
			this.Icon = new BitmapImage(new Uri("/Teraque.PresentationFramework;component/Resources/Filter Pages.png", UriKind.Relative));

		}

		/// <summary>
		/// Gets or sets the name of a group which an be used to combine filters for a column (or other logical grouping).
		/// </summary>
		public String GroupName
		{
			get
			{
				return base.GetValue(FilterItem.GroupNameProperty) as String;
			}
			set
			{
				base.SetValue(FilterItem.GroupNameProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the image to be displayed for the FilterItem.
		/// </summary>
		public Object Icon
		{
			get
			{
				return base.GetValue(FilterItem.IconProperty);
			}
			set
			{
				base.SetValue(FilterItem.IconProperty, value);
			}
		}

	}

}
