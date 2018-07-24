namespace Teraque.Windows.Data
{

	using System;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Data;
	using System.Windows.Markup;

	/// <summary>
	/// Converts a value into the URI into a String.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[ValueConversion(typeof(Uri), typeof(String))]
	public class UriConverter : IValueConverter
	{

		/// <summary>
		/// Converts a value.
		/// </summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns> converted value. If the method returns nullNothingnullptra null reference (Nothing in Visual Basic), the valid null value is used.</returns>
		public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
		{

			// If not value is given to convert, then return the value.  This prevents nulls from generating exceptions.
			if (value == null)
				return value;

			// Convert the date to the specified format (or use the generic one if no format was specified).
			Uri uri = value as Uri;
			return uri.OriginalString.Replace('/', '\\');

		}

		/// <summary>
		/// Converts a value.
		/// </summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns nullNothingnullptra null reference (Nothing in Visual Basic), the valid null value is used.</returns>
		public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
		{

			// Convert the string back into the native datatype.
			String stringValue = value as String;
			return new Uri(stringValue.Replace('\\', '/'), UriKind.RelativeOrAbsolute);

		}

	}

}
