namespace Teraque.Windows
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Media;
	using System.Windows.Media.Effects;

	/// <summary>
	/// Creates a halo of color around objects or areas of color.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Windows.Media.Effects.ShaderEffect")]
	public class OuterGlowEffect : ShaderEffect
	{

		/// <summary>
		/// Identifies the Input dependency property.
		/// </summary>
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(OuterGlowEffect), 0);

		/// <summary>
		/// Identifies the GlowColor dependency property.
		/// </summary>
		public static readonly DependencyProperty GlowColorProperty = DependencyProperty.Register(
			"GlowColor",
			typeof(Color),
			typeof(OuterGlowEffect),
			new UIPropertyMetadata(Colors.White, ShaderEffect.PixelShaderConstantCallback(1)));

		/// <summary>
		/// A specially constructed delegate used to associate a dependency property with a GPU register.
		/// </summary>
		static PropertyChangedCallback glowSizeCallback = ShaderEffect.PixelShaderConstantCallback(2);

		/// <summary>
		/// Identifies the GlowSize dependency property.
		/// </summary>
		public static readonly DependencyProperty GlowSizeProperty = DependencyProperty.Register(
			"GlowSize",
			typeof(Single),
			typeof(OuterGlowEffect),
			new UIPropertyMetadata(1.0f, OuterGlowEffect.OnGlowSizePropertyChanged));

		/// <summary>
		/// Identifies the Opacity dependency property.
		/// </summary>
		public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register(
			"Opacity",
			typeof(Single),
			typeof(OuterGlowEffect),
			new UIPropertyMetadata(1.0f, ShaderEffect.PixelShaderConstantCallback(3)));

		/// <summary>
		/// Initialize a new instance of the OuterGlowEffect class.
		/// </summary>
		[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Windows.Media.Effects.ShaderEffect")]
		[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Windows.Media.Effects.PixelShader")]
		public OuterGlowEffect()
		{

			// Each instance will create a new pixel shader from the resources.
			PixelShader pixelShader = new PixelShader();
			pixelShader.UriSource = new Uri("/Teraque.PresentationFramework;component/Resources/OuterGlowEffect.ps", UriKind.Relative);
			this.PixelShader = pixelShader;

			// This allows the effect a little extra space for drawing outside the bounds of the texture.
			this.PaddingBottom = this.GlowSize;
			this.PaddingLeft = this.GlowSize;
			this.PaddingRight = this.GlowSize;
			this.PaddingTop = this.GlowSize;

			// The partial derivatives are required to convert from a normalized coordinate system to pixels.
			this.DdxUvDdyUvRegisterIndex = 0;

			// This associates the dependency properties with the GPU registers.
			this.UpdateShaderValue(OuterGlowEffect.InputProperty);
			this.UpdateShaderValue(OuterGlowEffect.GlowColorProperty);
			this.UpdateShaderValue(OuterGlowEffect.GlowSizeProperty);
			this.UpdateShaderValue(OuterGlowEffect.OpacityProperty);

		}

		/// <summary>Gets of sets the color of the halo glow.</summary>
		public Color GlowColor
		{
			get
			{
				return ((Color)this.GetValue(OuterGlowEffect.GlowColorProperty));
			}
			set
			{
				this.SetValue(OuterGlowEffect.GlowColorProperty, value);
			}
		}

		/// <summary>Gets or sets the thickness of the halo glow.</summary>
		public Single GlowSize
		{
			get
			{
				return ((Single)this.GetValue(OuterGlowEffect.GlowSizeProperty));
			}
			set
			{
				this.SetValue(OuterGlowEffect.GlowSizeProperty, value);
			}
		}
		/// <summary>Gets or sets the degree of opacity of the halo glow.</summary>
		public Single Opacity
		{
			get
			{
				return ((Single)this.GetValue(OuterGlowEffect.OpacityProperty));
			}
			set
			{
				this.SetValue(OuterGlowEffect.OpacityProperty, value);
			}
		}

		/// <summary>
		/// Invoked when the size of the halo area around the effect has changed.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this property.</param>
		[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Windows.Media.Effects.ShaderEffect")]
		static void OnGlowSizePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will grow or shrink the area around the texture to accomidate the effect.
			OuterGlowEffect outerGlowEffect = dependencyObject as OuterGlowEffect;
			outerGlowEffect.PaddingBottom = outerGlowEffect.GlowSize;
			outerGlowEffect.PaddingLeft = outerGlowEffect.GlowSize;
			outerGlowEffect.PaddingRight = outerGlowEffect.GlowSize;
			outerGlowEffect.PaddingTop = outerGlowEffect.GlowSize;

			// This will call a specially constructed delegate that returns the register associated with the dependency property.
			OuterGlowEffect.glowSizeCallback(dependencyObject, dependencyPropertyChangedEventArgs);

		}

	}

}
