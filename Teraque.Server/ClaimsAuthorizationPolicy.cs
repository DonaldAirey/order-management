namespace Teraque
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.IdentityModel.Claims;
	using System.IdentityModel.Policy;
	using System.Security.Principal;
	using System.Text;

	/// <summary>
	/// Defines a set of rules for authorizing a user, given a set of claims.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public abstract class ClaimsAuthorizationPolicy : IAuthorizationPolicy
	{

		/// <summary>
		/// A method used to manufacture distinguished names from the ones provided by the validator.
		/// </summary>
		/// <param name="userName">The name of the user provided by the authenticator.</param>
		/// <returns>A distinguished name.</returns>
		delegate String DistinguishedNameMakerDelegate(String userName);

		/// <summary>
		/// A dictionary of methods that will make a distinguished name from a generic one provided by the validator.
		/// </summary>
		static Dictionary<String, DistinguishedNameMakerDelegate> distinguishedNameMakerMap = new Dictionary<string, DistinguishedNameMakerDelegate>()
		{
			{"ActiveDirectoryValidator", ClaimsAuthorizationPolicy.ActiveDirectoryNameMaker},
			{"X509", ClaimsAuthorizationPolicy.CertificateNameMaker}
		};

		/// <summary>
		/// The unique identifier of this authorization policy.
		/// </summary>
		Guid guid = Guid.NewGuid();

		/// <summary>
		/// Common name for an unknown user.
		/// </summary>
		const String unknownUser = "cn=Unknown,o=Teraque,dc=teraque,dc=com";

		/// <summary>
		/// Gets a string that identifies this authorization component.
		/// </summary>
		public String Id
		{
			get { return this.guid.ToString(); }
		}

		/// <summary>
		/// Gets a claim set that represents the issuer of the authorization policy.
		/// </summary>
		public ClaimSet Issuer
		{
			get { return DefaultClaimSet.System; }
		}

		/// <summary>
		/// Evaluate the set of incoming claims.
		/// </summary>
		/// <param name="evaluationContext">The results of the authorization policies that have been evaluated.</param>
		/// <param name="state">The current state of the authorization evaluation.</param>
		/// <returns>true indicates evaluation of the authorization can continue, false will reject the authorization.</returns>
		public Boolean Evaluate(EvaluationContext evaluationContext, ref Object state)
		{

			// Validate the parameter before using it.
			if (evaluationContext == null)
				throw new ArgumentNullException("evaluationContext");

			// This will create an identity based on the validated user and map the claims against that identity using the role manager.  This is some pretty heavy 
			// stuff but the upshot is that Active Directory (or some similar provider) will find out what groups this user belongs to and then assign a set of
			// claims based on those roles.
			Object property;
			if (evaluationContext.Properties.TryGetValue("Identities", out property))
			{
				List<IIdentity> identities = property as List<IIdentity>;
				foreach (IIdentity iIdentity in identities)
				{
					String distinguishedName = ClaimsAuthorizationPolicy.distinguishedNameMakerMap[iIdentity.AuthenticationType](iIdentity.Name);
					OrganizationPrincipal organizationPrincipal = new OrganizationPrincipal(iIdentity, distinguishedName, this.GetRoles(distinguishedName));
					evaluationContext.Properties["Principal"] = organizationPrincipal;
					evaluationContext.AddClaimSet(this, this.MapClaims(organizationPrincipal));
				}
			}
			else
			{

				// If no identities are provided by the transport then we'll provide a generic, unknown user with no claims.  This user will be able to access the
				// 'Unrestricted' serivce contracts but, most importantly, this user will be able to run the WSDL code to generate an interface.
				evaluationContext.Properties["Principal"] = new OrganizationPrincipal(
					new GenericIdentity("unknown"),
					ClaimsAuthorizationPolicy.unknownUser,
					new String[0]);

			}

			// There is no need to call the evaluation again, everything here is computed in a single pass.
			return true;

		}

		/// <summary>
		/// Makes a distinguished name from the generic name coming from the active directory.
		/// </summary>
		/// <param name="userName">The name of the user provided by the authenticator.</param>
		/// <returns>The distinguished name of the user.</returns>
		static String ActiveDirectoryNameMaker(String userName)
		{

			// The active directory validator already requires the names to be distinguished.
			return userName;

		}

		/// <summary>
		/// Make a distinguished name from the user name provided by the X509 Certificate validator.
		/// </summary>
		/// <param name="userName">The name of the user provided by the authenticator.</param>
		/// <returns>The distinguished name of the user.</returns>
		static String CertificateNameMaker(String userName)
		{

			// The user name from the certificate validator is of the form "<distinguishedName>; <thumbprint>".  The thumbprint is not needed by the active 
			// directory but the distinguished name is.  We need to extract distinguished name from the certificate's version of the distinguished name but remove 
			// the spaces.
			StringBuilder distinguisedName = new StringBuilder();
			String certificateDistinguisedName = userName.Split(';')[0];
			String[] distinguishedNameParts = certificateDistinguisedName.Split(',');
			for (Int32 index = 0; index < distinguishedNameParts.Length; index++)
			{
				String[] parts = distinguishedNameParts[index].Split('=');
				distinguisedName.Append(parts[0].Trim() + '=' + parts[1].Trim());
				if (index < distinguishedNameParts.Length - 1)
					distinguisedName.Append(',');
			}
			return distinguisedName.ToString();

		}

		/// <summary>
		/// Gets the roles for the given identity.
		/// </summary>
		/// <param name="distinguishedName">The identity of the current user.</param>
		/// <returns>A set of roles that have been assigned to the identity.</returns>
		protected abstract String[] GetRoles(String distinguishedName);

		/// <summary>
		/// Construct a ClaimSet based on the given roles.
		/// </summary>
		/// <param name="genericPrincipal">The GenericPrincipal which has the roles for which a set of claims are mapped.</param>
		/// <returns>A ClaimSet associated with the roles possessed by the given GenericPrincipal.</returns>
		protected abstract ClaimSet MapClaims(GenericPrincipal genericPrincipal);

	}

}
