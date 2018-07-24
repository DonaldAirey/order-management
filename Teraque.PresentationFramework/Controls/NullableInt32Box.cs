namespace Teraque.Windows.Controls
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// Represents a control that can be used to display or edit formatted System.Nullable&lt;Int32&gt; values.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class NullableInt32Box : TextBox
	{

		/// <summary>
		/// Identifies the Format dependency property.
		/// </summary>
		public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(
			"Format",
			typeof(String),
			typeof(NullableInt32Box),
			new FrameworkPropertyMetadata("G", new PropertyChangedCallback(NullableInt32Box.OnFormatPropertyChanged)));

		/// <summary>
		/// Identifies the Value dependency property.
		/// </summary>
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
			"Value",
			typeof(Nullable<Int32>),
			typeof(NullableInt32Box),
			new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(NullableInt32Box.OnValuePropertyChanged)));

		/// <summary>
		/// Initialies a new instance of the NullableInt32Box class.
		/// </summary>
		public NullableInt32Box()
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
			this.Value = String.IsNullOrEmpty(this.Text) ? new Nullable<Int32>() : new Nullable<Int32>(Convert.ToInt32(this.Text, CultureInfo.CurrentCulture));

		}

		/// <summary>
		/// Handles a change to the Format property.
		/// </summary>
		/// <param name="dependencyObject">The Object that originated the event.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event arguments.</param>
		static void OnFormatPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will convert the value into a text string that can be dispalyed in the base TextBox.
			NullableInt32Box int32Box = dependencyObject as NullableInt32Box;
			String format = dependencyPropertyChangedEventArgs.NewValue as String;
			int32Box.Text = int32Box.Value.HasValue ? int32Box.Value.Value.ToString(int32Box.Format, CultureInfo.CurrentCulture) : String.Empty;

		}

		/// <summary>
		/// Handles a change to the Value property.
		/// </summary>
		/// <param name="dependencyObject">The Object that originated the event.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event arguments.</param>
		static void OnValuePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will convert the value into a text string that can be dispalyed in the base TextBox.
			NullableInt32Box int32Box = dependencyObject as NullableInt32Box;
			Nullable<Int32> value = (Nullable<Int32>)dependencyPropertyChangedEventArgs.NewValue;
			int32Box.Text = value.HasValue ? value.Value.ToString(int32Box.Format, CultureInfo.CurrentCulture) : String.Empty;

		}

		/// <summary>
		/// Gets or sets the format field used to display the Nullable&lt;Int32&gt;.
		/// </summary>
		public String Format
		{
			get
			{
				return this.GetValue(NullableInt32Box.FormatProperty) as String;
			}
			set
			{
				this.SetValue(NullableInt32Box.FormatProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of the NullableInt32Box.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
		public Nullable<Int32> Value
		{
			get
			{
				return (Nullable<Int32>)this.GetValue(NullableInt32Box.ValueProperty);
			}
			set
			{
				this.SetValue(NullableInt32Box.ValueProperty, value);
			}
		}

	}

}
