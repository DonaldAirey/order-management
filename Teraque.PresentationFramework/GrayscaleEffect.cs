namespace Teraque.Windows
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Media;
	using System.Windows.Media.Effects;

	/// <summary>
	/// Converts a color image into a monochromatic image using luminance conversion.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Windows.Media.Effects.ShaderEffect")]
	public class GrayscaleEffect : ShaderEffect
	{

		/// <summary>
		/// Identifies the Saturation dependency property.
		/// </summary>
		public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
			"Saturation",
			typeof(Single),
			typeof(GrayscaleEffect),
			new UIPropertyMetadata(0.0f, PixelShaderConstantCallback(0)));

		/// <summary>
		/// Identifies the Input dependency property.
		/// </summary>
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(GrayscaleEffect), 0);

		/// <summary>
		/// Initializes a new instance of GrayscaleEffect class.
		/// </summary>
		[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Windows.Media.Effects.ShaderEffect")]
		[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Windows.Media.Effects.PixelShader")]
		public GrayscaleEffect()
		{

			// Each instance will create a new pixel shader from the resources.
			PixelShader pixelShader = new PixelShader();
			pixelShader.UriSource = new Uri("/Teraque.PresentationFramework;Component/Resources/GrayscaleEffect.ps", UriKind.Relative);
			this.PixelShader = pixelShader;

			// This associates the dependency properties with the GPU registers.
			UpdateShaderValue(GrayscaleEffect.InputProperty);
			UpdateShaderValue(GrayscaleEffect.SaturationProperty);

		}

		/// <summary>
		/// Gets or sets the saturation factor for the texture.
		/// </summary>
		public float Saturation
		{
			get
			{
				return ((float)this.GetValue(GrayscaleEffect.SaturationProperty));
			}
			set
			{
				this.SetValue(GrayscaleEffect.SaturationProperty, value);
			}
		}

	}

}
