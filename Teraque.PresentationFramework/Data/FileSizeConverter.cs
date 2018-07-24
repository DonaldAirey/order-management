namespace Teraque.Windows.Data
{

	using System;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Data;
	using System.Windows.Markup;

	/// <summary>
	/// Converts a file size into a presentable text.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[ValueConversion(typeof(Nullable<Int64>), typeof(String))]
	public class FileSizeConverter : IValueConverter
	{

		/// <summary>
		/// Conversion factor for Gigabytes.
		/// </summary>
		static Double gigabytes = Math.Pow(2, 30);

		/// <summary>
		/// Conversion factor for Kilobytes.
		/// </summary>
		static Double kilobytes = Math.Pow(2, 10);

		/// <summary>
		/// Conversion factor for Megabytes.
		/// </summary>
		static Double megabytes = Math.Pow(2, 20);

		/// <summary>
		/// Conversion factor for Terabytes.
		/// </summary>
		static Double terabytes = Math.Pow(2, 40);

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

			// Convert the total bytes into an easier to read form by dividing by a power of 2.  1,024 for KB, 1,048,576 for MB, etc.
			Nullable<Int64> byteValue = (Nullable<Int64>)value;
			if (byteValue != null)
			{
				Double fileSize = System.Convert.ToDouble(byteValue, CultureInfo.CurrentCulture);
				if (fileSize >= FileSizeConverter.terabytes)
					return String.Format(CultureInfo.CurrentCulture, "{0} TB", fileSize / FileSizeConverter.terabytes);
				if (fileSize >= FileSizeConverter.gigabytes)
					return String.Format(CultureInfo.CurrentCulture, "{0} GB", fileSize / FileSizeConverter.gigabytes);
				if (byteValue >= FileSizeConverter.megabytes)
                    return String.Format(CultureInfo.CurrentCulture, "{0} MB", fileSize / FileSizeConverter.megabytes);
				if (byteValue >= FileSizeConverter.kilobytes)
                    return String.Format(CultureInfo.CurrentCulture, "{0} KB", fileSize / FileSizeConverter.kilobytes);
                return String.Format(CultureInfo.CurrentCulture, "{0} bytes", byteValue);
			}

			// Null values are not converted.
			return byteValue;

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

			// Conversion back is not supported at this time.
			return DependencyProperty.UnsetValue;

		}

	}

}
