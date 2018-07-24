namespace Teraque.Windows.Data
{

	using System;
	using System.Globalization;
	using System.Reflection;
	using System.Windows;
	using System.Windows.Data;
	using System.Windows.Markup;

	/// <summary>
	/// Converts a file size into a presentable text.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[ValueConversion(typeof(Version), typeof(String))]
	public class VersionConverter : IValueConverter
	{

		/// <summary>
		/// Converts a value.
		/// </summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>The converted value. If the method returns a null reference, the valid null value is used.</returns>
		public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
		{

			// The build and revision numbers are generally not important in a display of the version, however, and option is provided if the build numbers are
			// being maintained.
			Version version = (Version)value;
			if (version != null)
				return version.Build == 0 ?
					String.Format(CultureInfo.CurrentCulture, Properties.Resources.VersionFormat, version.Major, version.Minor) :
					String.Format(CultureInfo.CurrentCulture, Properties.Resources.BuildFormat, version.Major, version.Minor, version.Build);

			// Null values are not converted.
			return version;

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
