namespace Teraque
{

	using System;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Represents the exception thrown when a component cannot be granted a license due to the evaluation period expiring.
	/// </summary>
	[Serializable]
	[SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
	public class ExpiredLicenseException : InvalidLicenseException
	{

		/// <summary>
		/// Initializes a new instance of the LicenseException class for the type and the instance of the component that was denied a license.
		/// </summary>
		/// <param name="instance">The instance of the component that was not granted a license.</param>
		public ExpiredLicenseException(Object instance)
			: base(instance, ExceptionMessage.Format(Properties.ExceptionMessages.ExpiredLicense, ExpiredLicenseException.GetType(instance)))
		{
		}

	}

}
