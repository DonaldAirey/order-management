namespace Teraque
{

	using System;
	using System.Globalization;
    using System.Security.Cryptography.X509Certificates;
    using System.Windows.Data;

	/// <summary>
	/// Converts the expiration date on an X509 Certificate to a System.String.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[ValueConversion(typeof(X509Certificate2), typeof(String))]
	public class ExpirationDateConverter : IValueConverter
	{

		/// <summary>
		/// Convert the expiration date on the certificate to a string.
		/// </summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns Nothing, the valid null value is used.</returns>
		public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
		{

			// This will extract the human readable name from the X500 method of encoding tags.
			X509Certificate2 x509Certificate2 = (X509Certificate2)value;
			return x509Certificate2 == null ? String.Empty : x509Certificate2.NotAfter.ToShortDateString();

		}

		/// <summary>
		/// Converts a value back to the expiration date on the certificate string.
		/// </summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns Nothing, the valid null value is used.</returns>
		public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			return null;
		}

	}

}
