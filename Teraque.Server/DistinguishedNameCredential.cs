namespace Teraque
{

	using System;
	using System.Security;
	using System.Runtime.InteropServices;

	/// <summary>
	/// Provides credentials for password-based authentication schemes that require a distinguished user name.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class DistinguishedNameCredential
	{

		/// <summary>
		/// The distinguished name of the user.
		/// </summary>
		public String DistinguishedName { get; set; }

		/// <summary>
		/// Gets or sets an indicator of whether these credentials should be remembered in a persistent store.
		/// </summary>
		public Boolean RememberCredentials { get; set; }
	
		/// <summary>
		/// Gets or sets the password as a SecureString instance.
		/// </summary>
		public SecureString SecurePassword { get; set; }

		/// <summary>
		/// Gets the clear-text password for the user name associated with the credentials.
		/// </summary>
		public String Password
		{

			get
			{

				// Use the interop services to convert the secure string to a normal string.  This violates the whole idea of a SecureString, but we've been very 
				// careful about when we expose this value.
				String returnValue = String.Empty;
				IntPtr intPtr = Marshal.SecureStringToBSTR(this.SecurePassword);
				try
				{
					returnValue = Marshal.PtrToStringBSTR(intPtr);
				}
				finally
				{
					Marshal.ZeroFreeBSTR(intPtr);
				}
				return returnValue;

			}

		}

	}

}
