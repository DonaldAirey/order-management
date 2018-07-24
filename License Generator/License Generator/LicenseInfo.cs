namespace Teraque.LicenseGenerator
{

	using System;

	/// <summary>
	/// Information about a license.
	/// </summary>
	class LicenseInfo
	{

		public DateTime DateCreated { get; set; }
	
		/// <summary>
		/// The unique product id.
		/// </summary>
		public Guid ProductId {get; set;}
		
		/// <summary>
		/// The unique customer id.
		/// </summary>
		public Guid CustomerId {get; set;}

		/// <summary>
		/// The license type.
		/// </summary>
		public Int16 LicenseType { get; set; }

	}

}
