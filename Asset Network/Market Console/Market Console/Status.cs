namespace Teraque.AssetNetwork
{

	using System;
	using System.ComponentModel;
	using System.Globalization;

	/// <summary>
	/// Describes the state of the tenant.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[TypeConverter(typeof(StatusConverter))]
	internal enum Status { Running, Stopped };

}
