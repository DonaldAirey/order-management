namespace Teraque.Windows.Controls
{

	using System;
	using System.Globalization;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;

	/// <summary>
	/// Provides a lightweight control for displaying small amounts of flow content.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class DateTimeBlock : TextBlock
	{

		/// <summary>
		/// Identifies the Format dependency property.
		/// </summary>
		public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(
			"Format",
			typeof(String),
			typeof(DateTimeBlock),
			new FrameworkPropertyMetadata("g", new PropertyChangedCallback(DateTimeBlock.OnFormatPropertyChanged)));

		/// <summary>
		/// Identifies the Value dependency property.
		/// </summary>
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
			"Value",
			typeof(DateTime),
			typeof(DateTimeBlock),
			new FrameworkPropertyMetadata(
				DateTime.MinValue,
				FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
				new PropertyChangedCallback(DateTimeBlock.OnValuePropertyChanged)));

		/// <summary>
		/// Initialies a new instance of the DateTimeBlock class.
		/// </summary>
		public DateTimeBlock()
		{

			// This will set the object to display it's initial value.
			this.Text = this.Value.ToString(this.Format, CultureInfo.CurrentCulture);

			// This will keep the Value property reconcilled to the text displayed.
			this.TextInput += this.OnTextInput;

		}

		/// <summary>
		/// Handles a change to the Format property.
		/// </summary>
		/// <param name="dependencyObject">The Object that originated the event.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event arguments.</param>
		static void OnFormatPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will convert the value into a text string that can be dispalyed in the base TextBlock.
			DateTimeBlock int32Block = dependencyObject as DateTimeBlock;
			String format = dependencyPropertyChangedEventArgs.NewValue as String;
			int32Block.Text = int32Block.Value.ToString(int32Block.Format, CultureInfo.CurrentCulture);

		}

		/// <summary>
		/// Occurs when this element gets text in a device-independent manner.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="textChangedEventArgs">The event data.</param>
		void OnTextInput(Object sender, TextCompositionEventArgs textCompositionEventArgs)
		{

			// Convert the text to a DateTime.
			this.Value = String.IsNullOrEmpty(this.Text) ? DateTime.MinValue : Convert.ToDateTime(this.Text, CultureInfo.CurrentCulture);

		}

		/// <summary>
		/// Handles a change to the Value property.
		/// </summary>
		/// <param name="dependencyObject">The Object that originated the event.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event arguments.</param>
		static void OnValuePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will convert the value into a text string that can be dispalyed in the base TextBlock.
			DateTimeBlock int32Block = dependencyObject as DateTimeBlock;
			DateTime value = (DateTime)dependencyPropertyChangedEventArgs.NewValue;
			int32Block.Text = value.ToString(int32Block.Format, CultureInfo.CurrentCulture);

		}

		/// <summary>
		/// Gets or sets the format field used to display the DateTime.
		/// </summary>
		public String Format
		{
			get
			{
				return this.GetValue(DateTimeBlock.FormatProperty) as String;
			}
			set
			{
				this.SetValue(DateTimeBlock.FormatProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of the DateTimeBlock.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
		public DateTime Value
		{
			get
			{
				return (DateTime)this.GetValue(DateTimeBlock.ValueProperty);
			}
			set
			{
				this.SetValue(DateTimeBlock.ValueProperty, value);
			}
		}

	}

}
