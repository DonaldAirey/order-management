namespace Teraque.AssetNetwork
{

	using System;
	using System.ComponentModel;
	using System.Globalization;

	/// <summary>
	/// Simple type converter for the Status enum.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal class StatusConverter : TypeConverter
	{

		/// <summary>
		/// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
		/// <param name="sourceType">A Type that represents the type you want to convert from.</param>
		/// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{

			// Only strings are supported as a source.
			return sourceType == typeof(String);

		}

		/// <summary>
		/// Returns whether this converter can convert the object to the specified type, using the specified context.
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
		/// <param name="destinationType">A Type that represents the type you want to convert to.</param>
		/// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{

			// Only strings are supported as a destination.
			return destinationType == typeof(String);

		}

		/// <summary>
		/// Converts the given object to the type of this converter, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
		/// <param name="culture">The CultureInfo to use as the current culture.</param>
		/// <param name="value">The Object to convert.</param>
		/// <returns>An Object that represents the converted value.</returns>
		public override Object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, Object value)
		{

			// Try to convert.  If it can't, then don't make a fuss, just return nothing.
			Object destinationValue = null;
			try
			{
				destinationValue = Enum.Parse(typeof(Status), value as String);
			}
			catch { }
			return destinationValue;

		}

		/// <summary>
		/// Converts the given value object to the specified type, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
		/// <param name="culture">A CultureInfo. If nullis passed, the current culture is assumed.</param>
		/// <param name="value">The Object to convert.</param>
		/// <param name="destinationType">The Type to convert the value parameter to.</param>
		/// <returns>An Object that represents the converted value.</returns>
		public override Object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, Object value, Type destinationType)
		{

			// Validate the arguments.
			if (destinationType == null)
				throw new ArgumentNullException("destinationType");

			// Enums have only one way to convert to a string.
			return value.ToString();

		}

	}

}
