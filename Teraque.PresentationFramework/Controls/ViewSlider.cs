namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;

	/// <summary>
	/// A slider used to select different views and magnifications.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class ViewSlider : Slider
	{

		/// <summary>
		/// Used to inhibit the snapping of the slider to the nearest value when using the keyboard.
		/// </summary>
		Boolean inhibitSnap;

		/// <summary>
		/// Identifies the Maximum dependency property.
		/// </summary>
		public static new readonly DependencyProperty MaximumProperty;

		/// <summary>
		/// Identifies the Maximum dependency property key.
		/// </summary>
		static DependencyPropertyKey maximumPropertyKey = DependencyProperty.RegisterReadOnly(
			"Maximum",
			typeof(Int32),
			typeof(ViewSlider),
			new FrameworkPropertyMetadata());

		/// <summary>
		/// Identifies the Minimum dependency property.
		/// </summary>
		public static new readonly DependencyProperty MinimumProperty;

		/// <summary>
		/// Identifies the Minimum dependency property key.
		/// </summary>
		static DependencyPropertyKey minimumPropertyKey = DependencyProperty.RegisterReadOnly(
			"Minimum",
			typeof(Int32),
			typeof(ViewSlider),
			new FrameworkPropertyMetadata());

		/// <summary>
		/// The small tick marks on the slider.
		/// </summary>
		static Double[] smallTicks = new Double[] {
			0.000, 0.123, 0.250, 0.382, 0.513, 0.518, 0.526, 0.539, 0.544, 0.557,
			0.561, 0.570, 0.583, 0.588, 0.592, 0.601, 0.605, 0.614, 0.618, 0.623,
			0.632, 0.636, 0.645, 0.649, 0.658, 0.662, 0.667, 0.675, 0.680, 0.689,
			0.693, 0.702, 0.706, 0.711, 0.719, 0.724, 0.732, 0.737, 0.746, 0.750,
			0.754, 0.763, 0.768, 0.776, 0.781, 0.789, 0.794, 0.798, 0.807, 0.811,
			0.820, 0.825, 0.833, 0.838, 0.842, 0.851, 0.855, 0.864, 0.868, 0.873,
			0.882, 0.886, 0.895, 0.899, 0.908, 0.912, 0.917, 0.925, 0.930, 0.939,
			0.943, 0.952, 0.956, 0.961, 0.969, 0.974, 0.982, 0.987, 0.996, 1.000};

		/// <summary>
		/// The large tick marks on the slider.
		/// </summary>
		static Double[] largeTicks = new Double[] {
			0.000, 0.123, 0.250, 0.382, 0.513, Double.NaN, Double.NaN, Double.NaN,
			Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN,
			Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN,
			Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN,
			Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, 0.702,
			Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN,
			Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN,
			Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN,
			Double.NaN, 0.825, Double.NaN, Double.NaN, Double.NaN, Double.NaN,
			Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN,
			Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN,
			Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN,
			Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, 1.000};

		/// <summary>
		/// A range of values used to snap the slider to the closest large value.
		/// </summary>
		static Range[] snappingRange = new Range[] {
			new Range(0.513, 0.544, 0.513),
			new Range(0.662, 0.737, 0.702),
			new Range(0.789, 0.868, 0.825),
			new Range(0.969, 1.000, 1.000)};

		/// <summary>
		/// Identifies the Value dependency property.
		/// </summary>
		public new static DependencyProperty ValueProperty = DependencyProperty.Register(
			"Value",
			typeof(Int32),
			typeof(ViewSlider),
			new FrameworkPropertyMetadata(0, ViewSlider.OnValuePropertyChanged));

		/// <summary>
		/// Maps the slider values into values that indicate index of the tick.  It is used to sync up the slider to the Value of the control.
		/// </summary>
		static Dictionary<Double, Int32> valueMap = new Dictionary<Double, Int32>() {
			{0.000, 0}, {0.123, 1}, {0.250, 2}, {0.382, 3}, {0.513, 4}, {0.518, 5}, {0.526, 6},
			{0.539, 7}, {0.544, 8}, {0.557, 9}, {0.561, 10}, {0.570, 11}, {0.583, 12}, {0.588, 13},
			{0.592, 14}, {0.601, 15}, {0.605, 16}, {0.614, 17}, {0.618, 18}, {0.623, 19}, {0.632, 20},
			{0.636, 21}, {0.645, 22}, {0.649, 23}, {0.658, 24}, {0.662, 25}, {0.667, 26}, {0.675, 27},
			{0.680, 28}, {0.689, 29}, {0.693, 30}, {0.702, 31}, {0.706, 32}, {0.711, 33}, {0.719, 34},
			{0.724, 35}, {0.732, 36}, {0.737, 37}, {0.746, 38}, {0.750, 39}, {0.754, 40}, {0.763, 41},
			{0.768, 42}, {0.776, 43}, {0.781, 44}, {0.789, 45}, {0.794, 46}, {0.798, 47}, {0.807, 48},
			{0.811, 49}, {0.820, 50}, {0.825, 51}, {0.833, 52}, {0.838, 53}, {0.842, 54}, {0.851, 55},
			{0.855, 56}, {0.864, 57}, {0.868, 58}, {0.873, 59}, {0.882, 60}, {0.886, 61}, {0.895, 62},
			{0.899, 63}, {0.908, 64}, {0.912, 65}, {0.917, 66}, {0.925, 67}, {0.930, 68}, {0.939, 69},
			{0.943, 70}, {0.952, 71}, {0.956, 72}, {0.961, 73}, {0.969, 74}, {0.974, 75}, {0.982, 76},
			{0.987, 77}, {0.996, 78}, {1.000, 79}};

		/// <summary>
		/// Used to store the ranges of slider values used to snap the slider to the closest large value.
		/// </summary>
		struct Range
		{

			/// <summary>
			/// The minimum value of the range.
			/// </summary>
			public Double Minimum;

			/// <summary>
			/// The maximum value of the range.
			/// </summary>
			public Double Maximum;

			/// <summary>
			/// The value associated with the snapping range.
			/// </summary>
			public Double Value;

			/// <summary>
			/// Initializes a new instance of the Range structure.
			/// </summary>
			/// <param name="minimum">The minimum value of a range.</param>
			/// <param name="maximum">The maximum value of a range.</param>
			/// <param name="value">The large tick value to which the range snaps.</param>
			public Range(Double minimum, Double maximum, Double value)
			{

				// Initialize the object.
				this.Minimum = minimum;
				this.Maximum = maximum;
				this.Value = value;

			}

		}

		/// <summary>
		/// Initialize the ViewSlider class.
		/// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static ViewSlider()
		{

			// The initialization of the keys must happen before the initialization of the properties.  To guarantee this order, the properties are initialized from the
            // keys here in spite of the FXCop suggestion that they all take place when the fields are defined.
			ViewSlider.MaximumProperty = ViewSlider.maximumPropertyKey.DependencyProperty;
			ViewSlider.MinimumProperty = ViewSlider.minimumPropertyKey.DependencyProperty;

            // This will force the value to snap to the closest value when using the mouse to move the thumb.
            Slider.ValueProperty.OverrideMetadata(typeof(ViewSlider), new FrameworkPropertyMetadata(null, ViewSlider.CoerceValuePropertyCallback));

        }

		/// <summary>
		/// Initializes a new instance of the ViewSlider class.
		/// </summary>
		public ViewSlider()
		{

			// When the ViewButton is opened, the slider should get the focus automatically.
			this.Focusable = true;

			// This control has a feature, reverse engineered from the original Microsoft Explorer control, that moves the value to the closest Icon viewing mode 
			// when the mouse is used to move the thumb.  But when the keyboard is used, this snapping is inhibited.  This flag is used to signal the event handler
			// that a keyboard operation is in progress and the snapping rules should be ignored.
			this.inhibitSnap = false;

			// This will force the slider to only stop at the given tick mark.
			this.IsSnapToTickEnabled = true;

			// The actual slider minimums and maximums are fixed to handle only normalized values.
			base.Maximum = 1.000;
			base.Minimum = 0.000;

			// These are the values that are seen externally as the minimum and maximum properties.
			this.SetValue(ViewSlider.minimumPropertyKey, 0);
			this.SetValue(ViewSlider.maximumPropertyKey, ViewSlider.smallTicks.Length - 1);

			// This kind of control is only found vertically in a ViewButton.
			this.Orientation = Orientation.Vertical;

			// This will fix the ticks to the values that were reverse engineered from the Microsoft Explorer control.  The thumb will snap to only these values.
			this.Ticks = new DoubleCollection(ViewSlider.smallTicks);

		}

		/// <summary>
		/// Gets the highest possible Value of the ViewSlider element.
		/// </summary>
		public new Double Maximum
		{
			get
			{
				return (Double)this.GetValue(ViewSlider.MaximumProperty);
			}
		}

		/// <summary>
		/// Gets or sets the Minimum possible Value of the ViewSlider element. 
		/// </summary>
		public new Double Minimum
		{
			get
			{
				return (Double)this.GetValue(ViewSlider.MinimumProperty);
			}
		}

		/// <summary>
		/// Gets or sets the Minimum possible Value of the ViewSlider element. 
		/// </summary>
		public new Int32 Value
		{
			get
			{
				return (Int32)this.GetValue(ViewSlider.ValueProperty);
			}
			set
			{
				this.SetValue(ViewSlider.ValueProperty, value);
			}
		}

		/// <summary>
		/// Called whenever a dependency property value is being re-evaluated, or coercion is specifically requested.
		/// </summary>
		/// <param name="dependencyObject">The object that the property exists on. When the callback is invoked, the property system will pass this value.</param>
		/// <param name="baseValue">The new value of the property, prior to any coercion attempt.</param>
		/// <returns>The coerced value (with appropriate type).</returns>
		static Object CoerceValuePropertyCallback(DependencyObject dependencyObject, Object baseValue)
		{

			// Extract the slider and the new value from the generic parameters.
			ViewSlider viewSlider = dependencyObject as ViewSlider;
			Double newValue = (Double)baseValue;

			// When the mouse is used to move the thumb it will snap into place when it is close to an integral view.  This provides the snapping logic that moves 
			// the mouse to an integral position on the slider when it gets close to that value.
			if (!viewSlider.inhibitSnap)
				foreach (Range range in ViewSlider.snappingRange)
					if (range.Minimum <= newValue && newValue < range.Maximum)
						baseValue = range.Value;

			// When the mouse is not close to a 'Large Tick' position on the slider the value is returned unaltered, but when it is close it will snap into place by
			// coercing the value.
			return baseValue;

		}

		/// <summary>
		/// Responds to the DecreaseLarge command.
		/// </summary>
		protected override void OnDecreaseLarge()
		{

			try
			{

				// Prevent the slider from snapping to a large value when if it is close.
				this.inhibitSnap = true;

				// The large tick values are kept in an array with a 1-to-1 correlation to the small tick values.  The items that are to be skipped over when
				// decreasing by a large amount are stored as a Double.NaN.  This method preserves a common index that can be used for both large and small
				// movements.
				Int32 currentValue = this.Value;
				while (currentValue != 0 && Double.IsNaN(ViewSlider.largeTicks[--currentValue])) { }
				this.Value = currentValue;

			}
			finally
			{

				// This allows mouse operations to snap to the nearest value.
				this.inhibitSnap = false;

			}

		}

		/// <summary>
		/// Responds to the DecreaseSmall command.
		/// </summary>
		protected override void OnDecreaseSmall()
		{

			try
			{

				// Prevent the slider from snapping to a large value when if it is close.
				this.inhibitSnap = true;

				// Move down to the next tick.
				if (this.Value != 0)
					this.Value--;

			}
			finally
			{

				// This allows mouse operations to snap to the nearest value.
				this.inhibitSnap = false;

			}

		}

		/// <summary>
		/// Responds to the IncreaseLarge command.
		/// </summary>
		protected override void OnIncreaseLarge()
		{

			try
			{

				// Prevent the slider from snapping to a large value when if it is close.
				this.inhibitSnap = true;

				// The large tick values are kept in an array with a 1-to-1 correlation to the small tick values.  The items that are to be skipped over when
				// increasing by a large amount are stored as a Double.NaN.  This method preserves a common index that can be used for both large and small
				// movements.
				Int32 currentValue = this.Value;
				while (currentValue != ViewSlider.largeTicks.Length - 1 && Double.IsNaN(ViewSlider.largeTicks[++currentValue])) { }
				this.Value = currentValue;

			}
			finally
			{

				// This allows mouse operations to snap to the nearest value.
				this.inhibitSnap = false;

			}

		}

		/// <summary>
		/// Responds to the IncreaseSmall command.
		/// </summary>
		protected override void OnIncreaseSmall()
		{

			try
			{

				// Prevent the slider from snapping to a large value when if it is close.
				this.inhibitSnap = true;

				// Move up to the next tick.
				if (this.Value != this.Ticks.Count - 1)
					this.Value++;

			}
			finally
			{

				// This allows mouse operations to snap to the nearest value.
				this.inhibitSnap = false;

			}

		}

		/// <summary>
		/// Updates the current position of the Slider when the Value property changes.
		/// </summary>
		/// <param name="oldValue">The old Value of the Slider.</param>
		/// <param name="newValue">The new Value of the Slider.</param>
		protected override void OnValueChanged(Double oldValue, Double newValue)
		{

			// This will sync the integer value that is consumed external with the double value that is used by the slider.  The external value is an integral value
			// indicating the position on the slider, the internal slider value is the position of the thumb on the slider.  Note that anytime you deal with Doubles
			// there's going to be sloppy stuff that makes it difficult to use as an index.  Ideally the slider would return an integer but we gotta work with what
			// we got.
			this.Value = ViewSlider.valueMap[Math.Round(newValue, 3)];

			// Allow the base class to handle the remainder of the event.
			base.OnValueChanged(oldValue, newValue);

		}

		/// <summary>
		/// Invoked when the effective property value of the ViewValue property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnValuePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Adjust the slider position to reflect the current integral Value using the mapping for the ticks on the slider.
			ViewSlider viewSlider = dependencyObject as ViewSlider;
			if (viewSlider.Value != Int32.MinValue)
				((Slider)viewSlider).Value = ViewSlider.smallTicks[viewSlider.Value];

		}

	}

}
