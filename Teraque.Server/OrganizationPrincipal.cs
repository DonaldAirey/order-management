namespace Teraque
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Security.Principal;

	/// <summary>
	/// Represents a principal that provides information about the organization to which the principal belongs.
	/// </summary>
	public class OrganizationPrincipal : GenericPrincipal
	{

		/// <summary>
		/// Initializes a new instance of the GenericPrincipal class from a user identity, an organization and an array of role names to which the user represented
		/// by that identity belongs.
		/// </summary>
		/// <param name="iIdentity">A basic implementation of IIdentity that represents any user.</param>
		/// <param name="distinguishedName">The distinguished name of the principal.</param>
		/// <param name="roles">An array of role names to which the user represented by the identity parameter belongs.</param>
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
		[SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.String.ToLower")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i")]
		public OrganizationPrincipal(IIdentity iIdentity, String distinguishedName, String[] roles) : base(iIdentity, roles)
		{

			// Initialize the object.
			this.DistinguishedName = distinguishedName;
			this.Organization = String.Empty;
			this.UserName = String.Empty;

			// Now pull apart the distinguished name looking for the components to load into the principal.  The user name and organizational unit are important for
			// a multi-tiered security system.
			String[] distinguishedNameParts = distinguishedName.Split(',');
			foreach (String distinguishedNamePart in distinguishedNameParts)
			{
				String[] parts = distinguishedNamePart.Trim().Split('=');
				String component = parts[0].Trim().ToLower();
				if (component == "cn")
					this.UserName = parts[1];
				if (component == "ou")
					this.Organization = parts[1];
			}

		}

		/// <summary>
		/// The distinguished name of the principal.
		/// </summary>
		public String DistinguishedName { get; private set; }

		/// <summary>
		/// Gets the organization to which the principal belongs.
		/// </summary>
		public String Organization { get; private set; }

		/// <summary>
		/// The user name of the principal.
		/// </summary>
		public String UserName { get; private set; }

	}

}
