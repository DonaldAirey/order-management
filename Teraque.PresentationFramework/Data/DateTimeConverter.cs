namespace Teraque.Windows.Data
{

	using System;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Data;
	using System.Windows.Markup;

	/// <summary>
	/// Converts a value into the standard format for use in a grid
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[ValueConversion(typeof(DateTime), typeof(String))]
	public class DateTimeConverter : IValueConverter
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

			// Convert the date to the specified format (or use the generic one if no format was specified).
			DateTime date = (DateTime)value;
			String format = parameter as String;
            return format == null ? date.ToString(CultureInfo.CurrentCulture) : date.ToString(format, CultureInfo.CurrentCulture);

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

			// Parse the String back into the native datatype.
			String strValue = value as String;
			DateTime resultDateTime;
			if (DateTime.TryParse(strValue, out resultDateTime))
				return resultDateTime;
			return DependencyProperty.UnsetValue;

		}

	}

}
