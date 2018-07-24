namespace Teraque
{

	using System;
	using System.Collections.Generic;
	using System.Security;

	/// <summary>
	/// A Domain\User Name and Password combination.
	/// </summary>
	public class DomainCredential
	{

		/// <summary>
		/// The Domain\User Name part of the credentials.
		/// </summary>
		public String DomainUserName { get; set; }

		/// <summary>
		/// The password.
		/// </summary>
		public SecureString SecurePassword { get; set; }

		/// <summary>
		/// Remember the credentials.
		/// </summary>
		public Boolean RememberCredentials { get; set; }

		/// <summary>
		/// Initializes a new instance of the DomainCrential class.
		/// </summary>
		public DomainCredential()
		{

			// Initialize the object
			this.DomainUserName = String.Empty;

		}

		/// <summary>
		/// Determines whether the specified Object is equal to the current Object.
		/// </summary>
		/// <param name="obj">The object to compare with the current object.</param>
		/// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
		public override bool Equals(object obj)
		{

			// Only the domain and user name are significant for comparing two objects.
			DomainCredential domainCredential = obj as DomainCredential;
			if (domainCredential != null)
				return this.DomainUserName == domainCredential.DomainUserName;

			// There is no comparing this object to any other type.
			return false;

		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>A hash code for the current Object.</returns>
		public override int GetHashCode()
		{

			// The domain and user name are the only significant fields in a hash.
			return this.DomainUserName.GetHashCode();

		}

	}

}
