namespace Teraque.Windows.Documents
{

	using System;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Documents;
	using System.Windows.Media;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Adornes the targeted user interface element with a watermark.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class WatermarkAdorner : Adorner
	{

		/// <summary>
		/// Identifies the Angle dependency property.
		/// </summary>
		public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
			"Angle",
			typeof(Double),
			typeof(WatermarkAdorner),
			new FrameworkPropertyMetadata(-15.0));
		
		/// <summary>
		/// The black 'Evaluation' text.
		/// </summary>
		FormattedText blackText;

		/// <summary>
		/// Identifies the Depth dependency property.
		/// </summary>
		public static readonly DependencyProperty DepthProperty = DependencyProperty.Register(
			"Depth",
			typeof(Double),
			typeof(WatermarkAdorner),
			new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, WatermarkAdorner.OnWatermarkPropertyChanged));

		/// <summary>
		/// The gray 'Evaluation' text.
		/// </summary>
		FormattedText grayText;

		/// <summary>
		/// Identifies the FontFamily dependency property.
		/// </summary>
		public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(
			typeof(WatermarkAdorner),
			new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, FrameworkPropertyMetadataOptions.Inherits, WatermarkAdorner.OnWatermarkPropertyChanged));

		/// <summary>
		/// Identifies the FontSize dependency property.
		/// </summary>
		public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(
			typeof(WatermarkAdorner),
			new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.Inherits, WatermarkAdorner.OnWatermarkPropertyChanged));

		/// <summary>
		/// Identifies the FontStretch dependency property.
		/// </summary>
		public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(
			typeof(WatermarkAdorner),
			new FrameworkPropertyMetadata(
				TextElement.FontStretchProperty.DefaultMetadata.DefaultValue,
				FrameworkPropertyMetadataOptions.Inherits,
				WatermarkAdorner.OnWatermarkPropertyChanged));

		/// <summary>
		/// Identifies the FontStyle dependency property.
		/// </summary>
		public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(
			typeof(WatermarkAdorner),
			new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle, FrameworkPropertyMetadataOptions.Inherits, WatermarkAdorner.OnWatermarkPropertyChanged));

		/// <summary>
		/// Identifies the FontWeight dependency property.
		/// </summary>
		public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(
			typeof(WatermarkAdorner),
			new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight, FrameworkPropertyMetadataOptions.Inherits, WatermarkAdorner.OnWatermarkPropertyChanged));

		/// <summary>
		/// Identifies the Text dependency property.
		/// </summary>
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text",
			typeof(String),
			typeof(WatermarkAdorner),
			new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, WatermarkAdorner.OnWatermarkPropertyChanged));

		/// <summary>
		/// The white 'Evaluation' text.
		/// </summary>
		FormattedText whiteText;

		/// <summary>
		/// Initializes the EvaluationAdorner class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static WatermarkAdorner()
		{

			// We want this adorner to act as a watermark.  It shouldn't effect any mouse operations.
			WatermarkAdorner.IsHitTestVisibleProperty.OverrideMetadata(typeof(WatermarkAdorner), new FrameworkPropertyMetadata(false));

			// The standard watermark is nearly invisible.
			WatermarkAdorner.OpacityProperty.OverrideMetadata(typeof(WatermarkAdorner), new FrameworkPropertyMetadata(0.10));

		}

		/// <summary>
		/// Initializes a new instance of the EvlauationAdorner class.
		/// </summary>
		/// <param name="adornedElement">The element to bind the adorner to.</param>
		public WatermarkAdorner(UIElement adornedElement) : base(adornedElement)
		{

			// This will prepare the text for the OnRender method override.
			this.PrepareText();

		}

		/// <summary>
		/// Gets or sets the angle of the watermark.
		/// </summary>
		public Double Angle
		{
			get
			{
				return (Double)base.GetValue(WatermarkAdorner.AngleProperty);
			}
			set
			{
				base.SetValue(WatermarkAdorner.AngleProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the depth of the shadow effect.
		/// </summary>
		public Double Depth
		{
			get
			{
				return (Double)base.GetValue(WatermarkAdorner.DepthProperty);
			}
			set
			{
				base.SetValue(WatermarkAdorner.DepthProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the font family of the control.
		/// </summary>
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)this.GetValue(WatermarkAdorner.FontFamilyProperty);
			}
			set
			{
				this.SetValue(WatermarkAdorner.FontFamilyProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the font size.
		/// </summary>
		public Double FontSize
		{
			get
			{
				return (Double)base.GetValue(WatermarkAdorner.FontSizeProperty);
			}
			set
			{
				base.SetValue(WatermarkAdorner.FontSizeProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the degree to which a font is condensed or expanded on the screen.
		/// </summary>
		public FontStretch FontStretch
		{
			get
			{
				return (FontStretch)base.GetValue(WatermarkAdorner.FontStretchProperty);
			}
			set
			{
				base.SetValue(WatermarkAdorner.FontStretchProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the font style.
		/// </summary>
		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(WatermarkAdorner.FontStyleProperty);
			}
			set
			{
				base.SetValue(WatermarkAdorner.FontStyleProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the weight or thickness of the specified font.
		/// </summary>
		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(WatermarkAdorner.FontWeightProperty);
			}
			set
			{
				base.SetValue(WatermarkAdorner.FontWeightProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the text contents of the watermark adorner.
		/// </summary>
		public String Text
		{
			get
			{
				return (String)base.GetValue(WatermarkAdorner.TextProperty);
			}
			set
			{
				base.SetValue(WatermarkAdorner.TextProperty, value);
			}
		}

		/// <summary>
		/// Participates in rendering operations that are directed by the layout system.
		/// </summary>
		/// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
		protected override void OnRender(DrawingContext drawingContext)
		{

			// Validate the parameters.
			if (drawingContext == null)
				throw new ArgumentNullException("drawingContext");

			// The general idea of this adorner is to display the 'Evaluation' text in the center of the screen and make it transparent enough so that it doesn't
			// get in the way but opaque enough so that the user knows its there.  The size of the adorned item is needed to center the text in the available space.
			Size size = this.AdornedElement.RenderSize;
			Double x = (size.Width - whiteText.Width) / 2.0;
			Double y = (size.Height - whiteText.Height) / 2.0;

			// Here's where the embossed effect is rendered on the drawing surface at a slight angle.
			drawingContext.PushTransform(new RotateTransform(this.Angle, size.Width / 2.0, size.Height / 2.0));
			drawingContext.DrawText(this.blackText, new Point(x + this.Depth, y + this.Depth));
			drawingContext.DrawText(this.grayText, new Point(x - this.Depth, y - this.Depth));
			drawingContext.DrawText(this.whiteText, new Point(x, y));

		}

		/// <summary>
		/// Handles a change to any of the properties that are used to construct the watermark.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this property.
		/// </param>
		static void OnWatermarkPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// The text that is displayed during the rendering is prepared ahead of time so it doesn't slow down the render operation.
			WatermarkAdorner watermarkAdorner = dependencyObject as WatermarkAdorner;
			watermarkAdorner.PrepareText();

		}

		/// <summary>
		/// Prepares the text that is displayed in the watermark.
		/// </summary>
		void PrepareText()
		{

			// An embossed effect is created by writing the same text in different colors to simulate a shadow and a highlight.  These parameters are the same for
			// the three different FormattedText objects that are drawn on the adorner surface to create the effect.
			CultureInfo cultureInfo = CultureInfo.CurrentCulture;
			FlowDirection flowDirection = FlowDirection.LeftToRight;
			Typeface typeface = new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch);

			// The 'Evaluation' text is embossed on the surface.  This is done basically by drawing the same thing in a highlight, normal and shaded colors and just
			// slightly offsetting them to produce an apparent edge to the text.
			this.whiteText = new FormattedText(this.Text, cultureInfo, flowDirection, typeface, this.FontSize, Brushes.White);
			this.grayText = new FormattedText(this.Text, cultureInfo, flowDirection, typeface, this.FontSize, Brushes.LightGray);
			this.blackText = new FormattedText(this.Text, cultureInfo, flowDirection, typeface, this.FontSize, Brushes.Black);

		}

	}

}
