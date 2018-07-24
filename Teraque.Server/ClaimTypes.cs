namespace Teraque
{

	using System;

	/// <summary>
	/// Teraque specific claims types for authenticating users.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public static class ClaimTypes
	{

		/// <summary>
		/// Create permission required.
		/// </summary>
		public const String Create = "http://schemas.teraque.com/identity/claims/create";

		/// <summary>
		/// Destroy permission required.
		/// </summary>
		public const String Destroy = "http://schemas.teraque.com/identity/claims/destroy";

		/// <summary>
		/// Read permission required.
		/// </summary>
		public const String Read = "http://schemas.teraque.com/identity/claims/read";

		/// <summary>
		/// Update permission required.
		/// </summary>
		public const String Update = "http://schemas.teraque.com/identity/claims/update";

	}

}
