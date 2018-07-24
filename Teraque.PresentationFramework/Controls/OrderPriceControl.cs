namespace Teraque.Windows.Controls
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
    using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// Represents a control with instructions for pricing an order.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class OrderPriceControl : ContentControl
	{

		/// <summary>
		/// Identifies the LimitPrice dependency property.
		/// </summary>
		public static readonly DependencyProperty LimitPriceProperty = DependencyProperty.Register(
			"LimitPrice",
			typeof(Decimal),
			typeof(OrderPriceControl),
			new FrameworkPropertyMetadata(OrderPriceControl.OnContentChanged));

		/// <summary>
		/// Identifies the Mnemonic dependency property.
		/// </summary>
		public static readonly DependencyProperty MnemonicProperty = DependencyProperty.Register(
			"Mnemonic",
			typeof(String),
			typeof(OrderPriceControl),
			new FrameworkPropertyMetadata(OrderPriceControl.OnContentChanged));

		/// <summary>
		/// Identifies the OrderTypeCode dependency property.
		/// </summary>
		public static readonly DependencyProperty OrderTypeCodeProperty = DependencyProperty.Register(
			"OrderTypeCode",
			typeof(OrderTypeCode),
			typeof(OrderPriceControl),
			new FrameworkPropertyMetadata(OrderTypeCode.Market, OnContentChanged));

		/// <summary>
		/// Identifies the StopPrice dependency property.
		/// </summary>
		public static readonly DependencyProperty StopPriceProperty = DependencyProperty.Register(
			"StopPrice",
			typeof(Decimal),
			typeof(OrderPriceControl),
			new FrameworkPropertyMetadata(OrderPriceControl.OnContentChanged));

		/// <summary>
		/// Identifies the TextAlignment dependency property.
		/// </summary>
		public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
			"TextAlignment",
			typeof(TextAlignment),
			typeof(OrderPriceControl),
			new FrameworkPropertyMetadata(TextAlignment.Left));

		/// <summary>
		/// Initializes the OrderTypeBlock class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static OrderPriceControl()
		{

			// By default, these content of these controls are aligned to the right because they are essentially numeric.
			Control.HorizontalContentAlignmentProperty.OverrideMetadata(typeof(OrderPriceControl), new FrameworkPropertyMetadata(HorizontalAlignment.Right));

		}

		/// <summary>
		/// Gets or sets the LimitPrice for this control.
		/// </summary>
		public Decimal LimitPrice
		{
			get
			{
				return (Decimal)this.GetValue(OrderPriceControl.LimitPriceProperty);
			}
			set
			{
				this.SetValue(OrderPriceControl.LimitPriceProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the Mnemonic for this control.
		/// </summary>
		public String Mnemonic
		{
			get
			{
				return (String)this.GetValue(OrderPriceControl.MnemonicProperty);
			}
			set
			{
				this.SetValue(OrderPriceControl.MnemonicProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the OrderTypeCode for this control.
		/// </summary>
		public OrderTypeCode OrderTypeCode
		{
			get
			{
				return (OrderTypeCode)this.GetValue(OrderPriceControl.OrderTypeCodeProperty);
			}
			set
			{
				this.SetValue(OrderPriceControl.OrderTypeCodeProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the StopPrice for this control.
		/// </summary>
		public Decimal StopPrice
		{
			get
			{
				return (Decimal)this.GetValue(OrderPriceControl.StopPriceProperty);
			}
			set
			{
				this.SetValue(OrderPriceControl.StopPriceProperty, value);
			}
		}

		/// <summary>
		/// Handles a change to the LimitPrice property
		/// </summary>
		/// <param name="dependencyObject">The Dependency Property that has changed.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event arguments describing the property change.</param>
		static void OnContentChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Reformat the content based on the order type code.
			OrderPriceControl orderPriceControl = dependencyObject as OrderPriceControl;
			switch (orderPriceControl.OrderTypeCode)
			{

			case OrderTypeCode.Limit:

				// A limit price is displayed as a simnple price.
				orderPriceControl.Content = String.Format(CultureInfo.CurrentCulture, "{0:#,##0.00}", orderPriceControl.LimitPrice);
				break;

			case OrderTypeCode.Market:
			case OrderTypeCode.MarketOnClose:
			case OrderTypeCode.OnClose:

				// These types just display the mnemonic.
				orderPriceControl.Content = orderPriceControl.Mnemonic;
				break;

			case OrderTypeCode.Stop:

				// The Stop Limit uses the mnemonic to distinguish it from the Limit price.
				orderPriceControl.Content = String.Format(CultureInfo.CurrentCulture, "{0:#,##0.00} {1}", orderPriceControl.StopPrice, orderPriceControl.Mnemonic);
				break;

			}

		}

	}

}
