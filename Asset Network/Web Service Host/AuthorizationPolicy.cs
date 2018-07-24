namespace Teraque.AssetNetwork
{

    using System;
	using System.DirectoryServices.AccountManagement;
    using System.Collections.Generic;
    using System.IdentityModel.Claims;
    using System.Security.Principal;
	using System.IdentityModel.Policy;
	using Teraque;

	/// <summary>
	/// Application specific set of rules for authorizing a user, given a set of claims.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class AuthorizationPolicy : ClaimsAuthorizationPolicy
    {

		/// <summary>
		/// Encapsulates the server or domain against which all active directory operations are performed, the container that is used as the base of those
		/// operations, and the credentials used to perform the operations.
		/// </summary>
		PrincipalContext principalContext;

		/// <summary>
		/// A collection of roles and the claims possessed by someone with that role.
		/// </summary>
		static Dictionary<String, Claim[]> roleMap = new Dictionary<string, Claim[]>
		{
			{"Administrators", new Claim[] {
				new Claim(Teraque.AssetNetwork.ClaimTypes.Configure, Resources.Application, Rights.PossessProperty),
				new Claim(Teraque.ClaimTypes.Read, Resources.Application, Rights.PossessProperty)}},
			{"TenantAdministrators", new Claim[] {
                new Claim(Teraque.AssetNetwork.ClaimTypes.Quote, Resources.Application, Rights.PossessProperty),
                new Claim(Teraque.AssetNetwork.ClaimTypes.Report, Resources.Application, Rights.PossessProperty),
                new Claim(Teraque.AssetNetwork.ClaimTypes.Service, Resources.Application, Rights.PossessProperty),
                new Claim(Teraque.AssetNetwork.ClaimTypes.Truncate, Resources.Application, Rights.PossessProperty),
                new Claim(Teraque.ClaimTypes.Create, Resources.Application, Rights.PossessProperty),
				new Claim(Teraque.ClaimTypes.Destroy, Resources.Application, Rights.PossessProperty),
				new Claim(Teraque.ClaimTypes.Update, Resources.Application, Rights.PossessProperty),
				new Claim(Teraque.ClaimTypes.Read, Resources.Application, Rights.PossessProperty),
                new Claim(Teraque.AssetNetwork.ClaimTypes.ManageBlotter, Resources.Application, Rights.PossessProperty)}},
			{"Traders", new Claim[] {
				new Claim(Teraque.ClaimTypes.Read, Resources.Application, Rights.PossessProperty),
				new Claim(Teraque.AssetNetwork.ClaimTypes.ManageBlotter, Resources.Application, Rights.PossessProperty)}},
			{"Managers", new Claim[] {
				new Claim(Teraque.ClaimTypes.Read, Resources.Application, Rights.PossessProperty),
				new Claim(Teraque.AssetNetwork.ClaimTypes.ManageBlotter, Resources.Application, Rights.PossessProperty)}},
			{"Analysts", new Claim[] {
				new Claim(Teraque.ClaimTypes.Read, Resources.Application, Rights.PossessProperty)}}
		};

		/// <summary>
		/// Initializes a new instance of the AuthorizationPolicy class.
		/// </summary>
		public AuthorizationPolicy()
		{

			// This is our connection to the active directory which will provide the roles for our users.
			this.principalContext = new PrincipalContext(
				ContextType.ApplicationDirectory,
				Teraque.Properties.Settings.Default.ActiveDirectoryRoot,
				Teraque.Properties.Settings.Default.OrganizationDistinctName);

		}

		/// <summary>
		/// Gets a collection of roles to which the user has been assigned.
		/// </summary>
		/// <param name="iIdentity">The identity of the user.</param>
		/// <returns>An array of roles assigned to the user.</returns>
		protected override String[] GetRoles(String userDistinguishedName)
		{

			// This will use the active directory to determine what roles have been assigned to the current user.  It's important to remember that a given user may
			// participate in more than one rule and thus have a combination of claims.
			List<String> roles = new List<String>();
			try
			{
				UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(this.principalContext, IdentityType.DistinguishedName, userDistinguishedName);
				if (userPrincipal != null)
					foreach (Principal principal in userPrincipal.GetAuthorizationGroups())
						roles.Add(principal.Name);
			}
			catch (Exception exception)
			{

				// Log the error and rethrow it.
				Log.Error("{0}, {1}", exception.Message, exception.StackTrace);
				throw exception;

			}

			// This is the set of roles for which the current user is assigned.
			return roles.ToArray();

		}

		/// <summary>
		/// Construct a set of claims for a user based on their role.
		/// </summary>
		/// <param name="iIdentity">The identity of the current operation context.</param>
		/// <returns>A set of claims associated with the user's role.</returns>
		protected override ClaimSet MapClaims(GenericPrincipal genericPrincipal)
		{

			// This will create a distinct set of claims based on all the roles to which the current user has been assigned.  Keep in mind that a given principal
			// (user) can be assigned to one or more roles, so the final set must contain the distinct union of all the claims.
			HashSet<Claim> hashSetClaims = new HashSet<Claim>();
			foreach (KeyValuePair<String, Claim[]> keyValuePair in roleMap)
				if (genericPrincipal.IsInRole(keyValuePair.Key))
					foreach (Claim claim in keyValuePair.Value)
						hashSetClaims.Add(claim);

			// This will create a ClaimSet based on the union of all the claims mapped to this user's roles.
			Claim[] claims = new Claim[hashSetClaims.Count];
			hashSetClaims.CopyTo(claims);
			return new DefaultClaimSet(DefaultClaimSet.System, claims);

		}

    }

}
