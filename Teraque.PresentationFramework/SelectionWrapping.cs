namespace Teraque.Windows
{

	using System;

	/// <summary>
	/// Specifies whether the selection navigation wraps when it reaches the edge of its container.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum SelectionWrapping
	{

		/// <summary>
		/// Do not wrap the navigation back to the start/end of the container when a boundary is reached.
		/// </summary>
		NoWrap,

		/// <summary>
		/// Wrap the navigation back to the start/end of the container when a boundary is reached.
		/// </summary>
		Wrap

	}

}
