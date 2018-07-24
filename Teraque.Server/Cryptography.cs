namespace Teraque
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Runtime.InteropServices;
	using System.Security;
	using System.Security.Cryptography;
	using System.Text;

	/// <summary>
	/// Contains various methods for cryptography.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal static class Cryptography
	{

		/// <summary>
		/// Used to encode the string.
		/// </summary>
		static String entropy = "SQAgAGEAbQAgAHQAaABlACAAcwBsAGkAbQBlACAAbwBvAHoAaQBuAGcAIABvAHUAdAAgAGYAcgBvAG0AIAB5AG8AdQByACAAVABWACAAcwBlAHQALgA=";

		/// <summary>
		/// Decrypts an encrypted string.
		/// </summary>
		/// <param name="encryptedData">An encrypted string.</param>
		/// <returns>A SecureString copy of the encrypted data.</returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		internal static SecureString DecryptString(String encryptedData)
		{
			try
			{

				// The encryption logic has trouble with empty strings.  Since saving the credentials is not always enabled, this could potentially be an issue
				// each time the user logged in.  This will make accomodations for the empty password to prevent unnecessary exceptions.
				if (String.IsNullOrEmpty(encryptedData))
					return new SecureString();

				// This will decrypt the string and create a SecureString from the resulting text array.  It's possible that if the 'entropy' string changes from 
				// the time the user saved their password until the load it again, that this method will take an exception.  That is why we watch for the exception.
				Byte[] decryptedData = ProtectedData.Unprotect(
					Convert.FromBase64String(encryptedData),
					Convert.FromBase64String(entropy),
					DataProtectionScope.CurrentUser);
				return ToSecureString(Encoding.Unicode.GetString(decryptedData));

			}
			catch
			{
				
				// Any problems with the encoded text or the 'entropy' value will result in an empty SecureString.
				return new SecureString();

			}
		}

		/// <summary>
		/// Encryptes a SecureString.
		/// </summary>
		/// <param name="secureString">A SecureString.</param>
		/// <returns>The encrypted form of the SecureString.</returns>
		internal static String EncryptString(SecureString secureString)
		{
			// This will encrypt the contents of the SecureString as a byte array and return it as a Base64 encoded string.
			Byte[] encryptedData = ProtectedData.Protect(
				Encoding.Unicode.GetBytes(Cryptography.ToInsecureString(secureString)),
				Convert.FromBase64String(entropy),
				DataProtectionScope.CurrentUser);
			return Convert.ToBase64String(encryptedData);
		}

		/// <summary>
		/// Convert the System.String into a System.Security.SecureString.
		/// </summary>
		/// <param name="input">A String.</param>
		/// <returns>A SecureString version of the given string.</returns>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        internal static SecureString ToSecureString(String input)
		{
			SecureString secure = new SecureString();
			foreach (Char character in input)
				secure.AppendChar(character);
			secure.MakeReadOnly();
			return secure;
		}

		/// <summary>
		/// Convert the System.Security.SecureString to a System.String.
		/// </summary>
		/// <param name="input">A SecureString.</param>
		/// <returns>A String version of the SecureString.</returns>
		internal static String ToInsecureString(SecureString input)
		{
			// Use the interop services to convert the secure string to a normal string.  This violates the whole idea of a SecureString, but the end result is more
			// secure because of it.
			String returnValue = String.Empty;
			IntPtr intPtr = Marshal.SecureStringToBSTR(input);
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
