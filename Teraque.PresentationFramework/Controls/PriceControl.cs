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
	[ContentProperty("Price")]
	public class PriceControl : ContentControl
	{

		/// <summary>
		/// Identifies the LastPrice dependency property.
		/// </summary>
		public static readonly DependencyProperty LastPriceProperty = DependencyProperty.Register(
			"LastPrice",
			typeof(Decimal),
			typeof(PriceControl),
			new FrameworkPropertyMetadata(0.0m, new PropertyChangedCallback(PriceControl.OnLastPricePropertyChanged)));

		/// <summary>
		/// Identifies the MaskBackground dependency property.
		/// </summary>
		public static readonly DependencyProperty MaskBackgroundProperty = DependencyProperty.Register(
			"MaskBackground",
			typeof(Brush),
			typeof(PriceControl));

		/// <summary>
		/// Identifies the MaskOpacity dependency property.
		/// </summary>
		public static readonly DependencyProperty MaskOpacityProperty = DependencyProperty.Register(
			"MaskOpacity",
			typeof(Double),
			typeof(PriceControl));

		/// <summary>
		/// Identifies the Price dependency property.
		/// </summary>
		public static readonly DependencyProperty PriceProperty = DependencyProperty.Register(
			"Price",
			typeof(Decimal),
			typeof(PriceControl),
			new FrameworkPropertyMetadata(0.0m, new PropertyChangedCallback(PriceControl.OnPricePropertyChanged)));

		/// <summary>
		/// Identifies the TickDownBackground dependency property.
		/// </summary>
		public static readonly DependencyProperty TickDownBackgroundProperty = DependencyProperty.Register(
			"TickDownBackground",
			typeof(Brush),
			typeof(PriceControl));

		/// <summary>
		/// Identifies the TickDuration dependency property.
		/// </summary>
		public static readonly DependencyProperty TickDurationProperty = DependencyProperty.Register(
			"TickDuration",
			typeof(Duration),
			typeof(PriceControl),
			new FrameworkPropertyMetadata(new Duration(TimeSpan.FromSeconds(1.0))));

		/// <summary>
		/// Identifies the TickOpacity dependency property.
		/// </summary>
		public static readonly DependencyProperty TickOpacityProperty = DependencyProperty.Register(
			"TickOpacity",
			typeof(Double),
			typeof(PriceControl),
			new FrameworkPropertyMetadata(1.0));

		/// <summary>
		/// Identifies the TickTime dependency property.
		/// </summary>
		public static readonly DependencyProperty TickTimeProperty = DependencyProperty.Register(
			"TickTime",
			typeof(DateTime),
			typeof(PriceControl),
			new FrameworkPropertyMetadata(DateTime.MinValue, new PropertyChangedCallback(PriceControl.OnTickTimePropertyChanged)));

		/// <summary>
		/// Identifies the TickUpBackground dependency property.
		/// </summary>
		public static readonly DependencyProperty TickUpBackgroundProperty = DependencyProperty.Register(
			"TickUpBackground",
			typeof(Brush),
			typeof(PriceControl));
	
		/// <summary>
		/// Initialize the PriceControl class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static PriceControl()
		{

			// By default, the content of this control will be aligned to the right because it's a numeric value.
			PriceControl.HorizontalContentAlignmentProperty.OverrideMetadata(typeof(PriceControl), new FrameworkPropertyMetadata(HorizontalAlignment.Right));

			// This means that zeros will show up in the control when it's initialized rather than a blank control.
			PriceControl.ContentProperty.OverrideMetadata(typeof(PriceControl), new FrameworkPropertyMetadata(0.0m));

			// This provides a default format for prices.
			PriceControl.ContentStringFormatProperty.OverrideMetadata(typeof(PriceControl), new FrameworkPropertyMetadata("#,##0.00;-#,##0.00"));

		}

		/// <summary>
		/// Initialize a new instance of the PriceControl class.
		/// </summary>
		public PriceControl()
		{

			// This handler will remove the animation before the control is recycled.
			this.DataContextChanged += this.OnDataContextChanged;

		}

		/// <summary>
		/// Gets or sets the last price.
		/// </summary>
		public Decimal LastPrice
		{
			get
			{
				return (Decimal)this.GetValue(PriceControl.LastPriceProperty);
			}
			set
			{
				this.SetValue(PriceControl.LastPriceProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the background brush to use when drawing the tick animation.
		/// </summary>
		public Brush MaskBackground
		{
			get
			{
				return this.GetValue(PriceControl.MaskBackgroundProperty) as Brush;
			}
			private set
			{
				this.SetValue(PriceControl.MaskBackgroundProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the current opacity of the background tick mask.
		/// </summary>
		public Double MaskOpacity
		{
			get
			{
				return (Double)this.GetValue(PriceControl.MaskOpacityProperty);
			}
			private set
			{
				this.SetValue(PriceControl.MaskOpacityProperty, value);
			}
		}

		///	<summary>
		/// Gets or sets the price.
		/// </summary>
		public Decimal Price
		{
			get
			{
				return (Decimal)this.GetValue(PriceControl.PriceProperty);
			}
			set
			{
				this.SetValue(PriceControl.PriceProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the background brush to use when the price moves down.
		/// </summary>
		public Brush TickDownBackground
		{
			get
			{
				return this.GetValue(PriceControl.TickDownBackgroundProperty) as Brush;
			}
			set
			{
				this.SetValue(PriceControl.TickDownBackgroundProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the amount of time that the tick animation is active.
		/// </summary>
		public Duration TickDuration
		{
			get
			{
				return (Duration)this.GetValue(PriceControl.TickDurationProperty);
			}
			set
			{
				this.SetValue(PriceControl.TickDurationProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets opacity of the background mask to use when a tick occurs.
		/// </summary>
		public Double TickOpacity
		{
			get
			{
				return (Double)this.GetValue(PriceControl.TickOpacityProperty);
			}
			set
			{
				this.SetValue(PriceControl.TickOpacityProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the time that the last tick occurred.
		/// </summary>
		public DateTime TickTime
		{
			get
			{
				return (DateTime)this.GetValue(PriceControl.TickTimeProperty);
			}
			set
			{
				this.SetValue(PriceControl.TickTimeProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the background brush to use when the price moves up.
		/// </summary>
		public Brush TickUpBackground
		{
			get
			{
				return this.GetValue(PriceControl.TickUpBackgroundProperty) as Brush;
			}
			set
			{
				this.SetValue(PriceControl.TickUpBackgroundProperty, value);
			}
		}
		
		/// <summary>
		/// Handles the completion of an animation clock.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="eventArgs">The event data.</param>
		void OnAnimationCompleted(Object sender, EventArgs eventArgs)
		{

			// When the animation is complete the clock is removed from the properties it animates
			AnimationClock animationClock = sender as AnimationClock;
			animationClock.Controller.Remove();

		}

		/// <summary>
		/// Handles the changing of the data context for this element. 
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event data.</param>
		void OnDataContextChanged(Object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Remove any prior animation that may have been associated with this control when it was recycled.
			this.BeginAnimation(PriceControl.MaskOpacityProperty, null);

		}

		/// <summary>
		/// Handles a change to the LastPrice property.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that that tracks changes to the effective value of this property.</param>
		static void OnLastPricePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This makes sure that the mask background color reflect the direction of the price change.
			PriceControl priceControl = dependencyObject as PriceControl;
			priceControl.MaskBackground = priceControl.LastPrice < priceControl.Price ? priceControl.TickUpBackground :
				priceControl.LastPrice > priceControl.Price ? priceControl.TickDownBackground :
				null;

		}

		/// <summary>
		/// Handles a change to the Price property.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that that tracks changes to the effective value of this property.</param>
		static void OnPricePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will make sure that the background mask reflects the proper price change 
			PriceControl priceControl = dependencyObject as PriceControl;
			priceControl.MaskBackground = priceControl.LastPrice < priceControl.Price ? priceControl.TickUpBackground :
				priceControl.LastPrice > priceControl.Price ? priceControl.TickDownBackground :
				null;
			priceControl.Content = priceControl.Price;

		}

		/// <summary>
		/// Handles a change to the TickTime property.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that that tracks changes to the effective value of this property.</param>
		static void OnTickTimePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// IMPORTANT CONCEPT: in any virtualized panel, we need to accept that any given control can be recycled and re-realized at any time, even during an
			// animation which could last several seconds. To account for this in our design, we don't store the state of any animation.  When a new data context is
			// provided for this control, we'll use the start time from the View Model and the current time and calculate how much time is left in the animation. In
			// this way, when a PriceControl is realized, it will pick up how far into the animation cycle we are and provide animation only for as long as the 
			// control is realized (or until the animation is complete).
			PriceControl priceControl = dependencyObject as PriceControl;
			Duration elapsedTime = new Duration(DateTime.Now.Subtract((DateTime)dependencyPropertyChangedEventArgs.NewValue));

			//  This will animate the background in the currently selected background color until the time since the last tick is equal to or greater than the 
			//  PriceControl.TickDuration property.
			if (elapsedTime < priceControl.TickDuration)
			{

				// This is the amount of time remaining in the animation.
				Duration remainingDuration = priceControl.TickDuration - elapsedTime;

				// The most important part of this animation, to make it appear as if we've been keeping track of all the price changes in the entire virtual 
				// space, is to calculate the current age of the tick and from that interpolate the current state of animation.
				Double proRata = remainingDuration.TimeSpan.TotalMilliseconds / priceControl.TickDuration.TimeSpan.TotalMilliseconds;
				Double from = priceControl.TickOpacity * proRata;

				// We will only animate this control as long as it is realized or until the interpolated animation has completed.  Note that we take the time to
				// clean up after the animation is completed as there are likely to be many items that are animated and the lifetime of any one PriceControl is
				// indefinite.  Note that this is a relatively slow animation and there will likely be many of them, so we've chosen a framerate that will not
				// overly tax the CPU.
				DoubleAnimation opacityAnimation = new DoubleAnimation();
				opacityAnimation.Completed += priceControl.OnAnimationCompleted;
				opacityAnimation.Duration = remainingDuration;
				opacityAnimation.FillBehavior = FillBehavior.Stop;
				opacityAnimation.From = from;
				Timeline.SetDesiredFrameRate(opacityAnimation, 4);
				priceControl.BeginAnimation(PriceControl.MaskOpacityProperty, opacityAnimation, HandoffBehavior.SnapshotAndReplace);

			}

		}

	}

}
