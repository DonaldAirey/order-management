namespace Teraque.AssetNetwork
{

	using System;

	/// <summary>
	/// Teraque specific claims types for authenticating users.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public static class ClaimTypes
	{

		/// <summary>
		/// Permission to configure service.
		/// </summary>
		public const String Configure = "http://schemas.teraque.com/identity/claims/configure";

		/// <summary>
		/// Permission to manage services.
		/// </summary>
		public const String Service = "http://schemas.teraque.com/identity/claims/service";

		/// <summary>
		/// Permission to manage quotes.
		/// </summary>
		public const String Quote = "http://schemas.teraque.com/identity/claims/quote";

		/// <summary>
		/// Permission to report and update executions.
		/// </summary>
		public const String Report = "http://schemas.teraque.com/identity/claims/report";

		/// <summary>
		/// Permission to truncate a table.
		/// </summary>
		public const String Truncate = "http://schemas.teraque.com/identity/claims/truncate";

		/// <summary>
		/// Permission to manage a blotter.
		/// </summary>
		public const String ManageBlotter = "http://schemas.teraque.com/identity/claims/truncate";

	}

}
