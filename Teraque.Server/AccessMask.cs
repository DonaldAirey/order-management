namespace Teraque
{

	using System;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Describes the masks used to extract the permissions from the Access Control List.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
	public enum AccessMask : long
	{
		/// <summary>
		/// Mask for no Access
		/// </summary>
		None = 0x00000000L,
		/// <summary>
		/// Mask for Generic Read
		/// </summary>
		GenericRead = 0x80000000L,
		/// <summary>
		/// Mask for Generic Write
		/// </summary>
		GenericWrite = 0x40000000L,
		/// <summary>
		/// Mask for Generic Execute
		/// </summary>
		GenericExecute = 0x20000000L,
		/// <summary>
		/// Mask for All Access
		/// </summary>
		GenericAll = 0x10000000L,
		/// <summary>
		/// Mask for access the System Access Control List
		/// </summary>
		SystemAccessControlList = 0x01000000L,
		/// <summary>
		/// Mask for Standard Access Rights
		/// </summary>
		StandardAccessRights = 0x00ff0000L,
		/// <summary>
		/// Mask for Object Specific Rights
		/// </summary>
		ObjectSpecificAccessRights = 0x0000ffffL
	}

}
