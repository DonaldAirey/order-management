namespace Teraque.AssetNetwork
{

	using System;

	/// <summary>
	/// Information about each endpoint that subscribes to data from the simulatorl
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class ClientInfo
	{

		/// <summary>
		/// The name of the endpoint configuration.
		/// </summary>
		public String EndpointName { get; set; }

		/// <summary>
		/// The user name.
		/// </summary>
		public String UserName { get; set; }

		/// <summary>
		/// The user's password.
		/// </summary>
		public String Password { get; set; }

	}

}
