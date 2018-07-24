namespace Teraque.Windows.Controls.Primitives
{

	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Controls.Primitives;
    using System.Diagnostics.CodeAnalysis;
	using System.Windows.Media;

	/// <summary>
	/// 
	/// </summary>
	public enum Highlight
	{

		/// <summary>
		/// 
		/// </summary>
		Elliptical,

		/// <summary>
		/// 
		/// </summary>
		Diffuse

	}

	/// <summary>
	/// 
	/// </summary>
	public class FrameButton : ToggleButton
	{

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty CornerRadiusProperty =
			Border.CornerRadiusProperty.AddOwner(typeof(FrameButton));

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty OuterBorderBrushProperty =
			DependencyProperty.Register("OuterBorderBrush", typeof(Brush), typeof(FrameButton));

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty OuterBorderThicknessProperty =
			DependencyProperty.Register("OuterBorderThickness", typeof(Thickness), typeof(FrameButton));

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty InnerBorderBrushProperty =
		   DependencyProperty.Register("InnerBorderBrush", typeof(Brush), typeof(FrameButton));

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty InnerBorderThicknessProperty =
			DependencyProperty.Register("InnerBorderThickness", typeof(Thickness), typeof(FrameButton));

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty GlowColorProperty =
			DependencyProperty.Register("GlowColor", typeof(SolidColorBrush), typeof(FrameButton));

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty HighlightAppearanceProperty =
			DependencyProperty.Register("HighlightAppearance", typeof(ControlTemplate), typeof(FrameButton));

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty HighlightMarginProperty =
			DependencyProperty.Register("HighlightMargin", typeof(Thickness), typeof(FrameButton));

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty HighlightBrightnessProperty =
			DependencyProperty.Register("HighlightBrightness", typeof(byte), typeof(FrameButton));

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty HighlightStyleProperty =
			DependencyProperty.Register("HighlightStyle", typeof(Highlight), typeof(FrameButton),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnHighlightStyleChanged)));

		/// <summary>
		/// 
		/// </summary>
		public Brush GlowColor
		{
			get { return (SolidColorBrush)GetValue(GlowColorProperty); }
			set { SetValue(GlowColorProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public CornerRadius CornerRadius
		{
			get { return (CornerRadius)GetValue(CornerRadiusProperty); }
			set { SetValue(CornerRadiusProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public Brush OuterBorderBrush
		{
			get { return (Brush)GetValue(OuterBorderBrushProperty); }
			set { SetValue(OuterBorderBrushProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public Thickness OuterBorderThickness
		{
			get { return (Thickness)GetValue(OuterBorderThicknessProperty); }
			set { SetValue(OuterBorderThicknessProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public Brush InnerBorderBrush
		{
			get { return (Brush)GetValue(InnerBorderBrushProperty); }
			set { SetValue(InnerBorderBrushProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public Thickness InnerBorderThickness
		{
			get { return (Thickness)GetValue(InnerBorderThicknessProperty); }
			set { SetValue(InnerBorderThicknessProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public ControlTemplate HighlightAppearance
		{
			get { return (ControlTemplate)GetValue(HighlightAppearanceProperty); }
			set { SetValue(HighlightAppearanceProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public Thickness HighlightMargin
		{
			get { return (Thickness)GetValue(HighlightMarginProperty); }
			set { SetValue(HighlightMarginProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public byte HighlightBrightness
		{
			get { return (byte)GetValue(HighlightBrightnessProperty); }
			set { SetValue(HighlightBrightnessProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public Highlight HighlightStyle
		{
			get { return (Highlight)GetValue(HighlightStyleProperty); }
			set { SetValue(HighlightStyleProperty, value); }
		}

		/// <summary>
		/// Initializes the FrameButton class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static FrameButton()
		{

			// This describes a new style for the class that will be found in the theme files.
			DefaultStyleKeyProperty.OverrideMetadata(typeof(FrameButton), new FrameworkPropertyMetadata(typeof(FrameButton)));

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		static void OnHighlightStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameButton btn = (FrameButton)d;

			Highlight highlight = (Highlight)e.NewValue;

			// Assign style associated with user-selected enum value
			btn.Style = (Style)btn.TryFindResource(new ComponentResourceKey(btn.GetType(), highlight.ToString()));
		}

	}

}
