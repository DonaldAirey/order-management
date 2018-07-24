namespace Teraque.Windows.Controls
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// Represents a control that can be used to display or edit formatted System.Int32 values.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class Int32Box : TextBox
	{

		/// <summary>
		/// Identifies the Format dependency property.
		/// </summary>
		public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(
			"Format",
			typeof(String),
			typeof(Int32Box),
			new FrameworkPropertyMetadata("G", new PropertyChangedCallback(Int32Box.OnFormatPropertyChanged)));

		/// <summary>
		/// Identifies the Value dependency property.
		/// </summary>
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
			"Value",
			typeof(Int32),
			typeof(Int32Box),
			new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Int32Box.OnValuePropertyChanged)));

		/// <summary>
		/// Initialies a new instance of the Int32Box class.
		/// </summary>
		public Int32Box()
		{

			// This will keep the Value property reconcilled to the text displayed.
			this.TextChanged += this.OnTextChanged;

		}

		/// <summary>
		/// Handles a change to the Text property.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="textChangedEventArgs">The event arguments.</param>
		void OnTextChanged(Object sender, TextChangedEventArgs textChangedEventArgs)
		{

			// Convert the text to a Int32.
			this.Value = String.IsNullOrEmpty(this.Text) ? 0 : Convert.ToInt32(this.Text, CultureInfo.CurrentCulture);

		}

		/// <summary>
		/// Handles a change to the Format property.
		/// </summary>
		/// <param name="dependencyObject">The Object that originated the event.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event arguments.</param>
		static void OnFormatPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will convert the value into a text string that can be dispalyed in the base TextBox.
			Int32Box int32Box = dependencyObject as Int32Box;
			String format = dependencyPropertyChangedEventArgs.NewValue as String;
			int32Box.Text = int32Box.Value.ToString(int32Box.Format, CultureInfo.CurrentCulture);

		}

		/// <summary>
		/// Handles a change to the Value property.
		/// </summary>
		/// <param name="dependencyObject">The Object that originated the event.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event arguments.</param>
		static void OnValuePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will convert the value into a text string that can be dispalyed in the base TextBox.
			Int32Box int32Box = dependencyObject as Int32Box;
			Int32 value = (Int32)dependencyPropertyChangedEventArgs.NewValue;
			int32Box.Text = value.ToString(int32Box.Format, CultureInfo.CurrentCulture);

		}

		/// <summary>
		/// Gets or sets the format field used to display the Int32.
		/// </summary>
		public String Format
		{
			get
			{
				return this.GetValue(Int32Box.FormatProperty) as String;
			}
			set
			{
				this.SetValue(Int32Box.FormatProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of the Int32Box.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
		public Int32 Value
		{
			get
			{
				return (Int32)this.GetValue(Int32Box.ValueProperty);
			}
			set
			{
				this.SetValue(Int32Box.ValueProperty, value);
			}
		}

	}

}
