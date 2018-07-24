namespace Teraque.Windows.Controls
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
	using System.Windows.Media.Animation;
	using System.Windows.Markup;

	/// <summary>
	/// Provides a lightweight control for displaying small amounts of flow content in a user specified format.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[ContentProperty("Percent")]
	public class PercentControl : ContentControl
	{

		/// <summary>
		/// Identifies the Percent dependency property.
		/// </summary>
		public static readonly DependencyProperty PercentProperty = DependencyProperty.Register(
			"Percent",
			typeof(Decimal),
			typeof(PercentControl),
			new FrameworkPropertyMetadata(0.0m, new PropertyChangedCallback(PercentControl.OnPercentPropertyChanged)));

		/// <summary>
		/// Initialize the PercentControl class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static PercentControl()
		{

			// By default, the content of this control will be aligned to the right because it's a numeric value.
			PercentControl.HorizontalContentAlignmentProperty.OverrideMetadata(typeof(PercentControl), new FrameworkPropertyMetadata(HorizontalAlignment.Right));

			// This means that zeros will show up in the control when it's initialized rather than a blank control.
			PercentControl.ContentProperty.OverrideMetadata(typeof(PercentControl), new FrameworkPropertyMetadata(0.0m));

			// This provides a default format for percent display.
			PercentControl.ContentStringFormatProperty.OverrideMetadata(
				typeof(PercentControl),
				new FrameworkPropertyMetadata("0.00%;-0.00%", PercentControl.OnPercentPropertyChanged));

		}

		///	<summary>
		/// Gets or sets the price.
		/// </summary>
		public Decimal Percent
		{
			get
			{
				return (Decimal)this.GetValue(PercentControl.PercentProperty);
			}
			set
			{
				this.SetValue(PercentControl.PercentProperty, value);
			}
		}

		/// <summary>
		/// Handles a change to the Percent property.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that that tracks changes to the effective value of this property.</param>
		static void OnPercentPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will use the content control to display the formated percent.  The formatting will implicity convert a value to a percent when the '%' 
			// character is present in the format string.  Since there's no way around this, we'll embrace it and just perform the conversion when we have a format
			// string that doesn't include the percent character.
			PercentControl priceControl = dependencyObject as PercentControl;
			priceControl.Content = priceControl.ContentStringFormat.Contains("%") ? priceControl.Percent : priceControl.Percent * 100.0m;

		}

	}

}
