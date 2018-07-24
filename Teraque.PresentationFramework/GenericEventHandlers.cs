namespace Teraque.Windows
{

	using System;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// A generic event handler.
	/// </summary>
	/// <param name="sender">The object that originated the event.</param>
	/// <param name="genericEventArgs">The generic event arguments.</param>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances")]
	public delegate void GenericEventHandler(Object sender, GenericEventArgs genericEventArgs);

}
