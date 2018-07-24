namespace Teraque.Windows.Controls
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;

	/// <summary>
	/// Provides a lightweight control for displaying small amounts of flow content.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class DoubleBlock : TextBlock
	{

		/// <summary>
		/// Identifies the Format dependency property.
		/// </summary>
		public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(
			"Format",
			typeof(String),
			typeof(DoubleBlock),
			new FrameworkPropertyMetadata("G", new PropertyChangedCallback(DoubleBlock.OnFormatPropertyChanged)));

		/// <summary>
		/// Identifies the Value dependency property.
		/// </summary>
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
			"Value",
			typeof(Double),
			typeof(DoubleBlock),
			new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DoubleBlock.OnValuePropertyChanged)));

		/// <summary>
		/// Initialies a new instance of the DoubleBlock class.
		/// </summary>
		public DoubleBlock()
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
			DoubleBlock int32Block = dependencyObject as DoubleBlock;
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

			// Convert the text to a Double.
			this.Value = String.IsNullOrEmpty(this.Text) ? 0 : Convert.ToDouble(this.Text, CultureInfo.CurrentCulture);

		}

		/// <summary>
		/// Handles a change to the Value property.
		/// </summary>
		/// <param name="dependencyObject">The Object that originated the event.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event arguments.</param>
		static void OnValuePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will convert the value into a text string that can be dispalyed in the base TextBlock.
			DoubleBlock int32Block = dependencyObject as DoubleBlock;
			Double value = (Double)dependencyPropertyChangedEventArgs.NewValue;
			int32Block.Text = value.ToString(int32Block.Format, CultureInfo.CurrentCulture);

		}

		/// <summary>
		/// Gets or sets the format field used to display the Double.
		/// </summary>
		public String Format
		{
			get
			{
				return this.GetValue(DoubleBlock.FormatProperty) as String;
			}
			set
			{
				this.SetValue(DoubleBlock.FormatProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of the DoubleBlock.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
		public Double Value
		{
			get
			{
				return (Double)this.GetValue(DoubleBlock.ValueProperty);
			}
			set
			{
				this.SetValue(DoubleBlock.ValueProperty, value);
			}
		}

	}

}
