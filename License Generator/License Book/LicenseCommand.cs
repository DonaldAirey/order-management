namespace Teraque.LicenseGenerator
{

	using System;
	using System.Windows.Input;

	/// <summary>
	/// Commands that are common to all Teraque applications.
	/// </summary>
	public sealed class LicenseCommand
	{

		/// <summary>
		/// Initializes a new instance of the LicenseCommands class. 
		/// </summary>
		LicenseCommand() { }

		/// <summary>
		/// Generates a license key.
		/// </summary>
		public static readonly RoutedCommand GenerateLicense = new RoutedCommand("GenerateLicense", typeof(LicenseCommand));

	}

}
