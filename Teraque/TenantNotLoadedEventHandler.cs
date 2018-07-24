namespace Teraque
{

	using System;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Invoked when the tenant is not loaded.
	/// </summary>
	/// <param name="sender">The object that originated the event.</param>
	/// <param name="tenantName">The name of the tenant that isn't loaded.</param>
	/// <copyright>Copyright © 2010 - 2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public delegate void TenantNotLoadedEventHandler(Object sender, String tenantName);

}
