namespace Teraque.Windows
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Media;
	using System.Windows.Media.Effects;

	/// <summary>
	/// Produces an embossed effect of light and dark shading on the target image.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Windows.Media.Effects.ShaderEffect")]
	public class EmbossedEffect : ShaderEffect
	{

		/// <summary>
		/// Identifies the Amount dependency property.
		/// </summary>
		public static readonly DependencyProperty AmountProperty = DependencyProperty.Register(
			"Amount",
			typeof(Double),
			typeof(EmbossedEffect),
			new UIPropertyMetadata(0.5, PixelShaderConstantCallback(0)));

		/// <summary>
		/// Identifies the Input dependency property.
		/// </summary>
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(EmbossedEffect), 0);

		/// <summary>
		/// Identifies the Width dependency property.
		/// </summary>
		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
			"Width",
			typeof(Double),
			typeof(EmbossedEffect),
			new UIPropertyMetadata(0.003, PixelShaderConstantCallback(1)));

		/// <summary>
		/// Initializes a new instance of Emboss class.
		/// </summary>
		[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Windows.Media.Effects.ShaderEffect")]
		[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Windows.Media.Effects.PixelShader")]
		public EmbossedEffect()
		{

			// Each instance will create a new pixel shader from the resources.
			PixelShader pixelShader = new PixelShader();
			pixelShader.UriSource = new Uri("/Teraque.PresentationFramework;component/Resources/Embossed.ps", UriKind.Relative);
			this.PixelShader = pixelShader;

			// This associates the dependency properties with the GPU registers.
			this.UpdateShaderValue(InputProperty);
			this.UpdateShaderValue(AmountProperty);
			this.UpdateShaderValue(WidthProperty);

		}

		/// <summary>
		/// Gets or sets the brush used as the input to the effect.
		/// </summary>
		public Brush Input
		{
			get
			{
				return this.GetValue(EmbossedEffect.InputProperty) as Brush;
			}
			set
			{
				this.SetValue(EmbossedEffect.InputProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the amount (intensity) of embossing.
		/// </summary>
		public Double Amount
		{
			get
			{
				return (Double)(this.GetValue(AmountProperty));
			}
			set
			{
				this.SetValue(AmountProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the width between the light and dark shades of the embossed image.
		/// </summary>
		public Double Width
		{
			get
			{
				return (Double)(this.GetValue(WidthProperty));
			}
			set
			{
				this.SetValue(WidthProperty, value);
			}
		}
	}

}
