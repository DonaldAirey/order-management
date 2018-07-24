namespace Teraque
{

    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Claims;
	using System.IdentityModel.Policy;
    using System.Security;
    using System.Security.Permissions;
	using System.Security.Principal;
	using System.ServiceModel;
	using System.Threading;

    /// <summary>
	/// Manages access to a resource based on a set of claims.
	/// </summary>
	public class ClaimsPermission : IPermission
    {

		/// <summary>
		/// Indicates that the current principal should be authenticated before permission can be granted.
		/// </summary>
		Boolean isAuthenticated;

		/// <summary>
		/// Indicates that no claims are needed for permission to be granted.
		/// </summary>
		Boolean isUnrestricted;

		/// <summary>
		/// The claims required by this permission.
		/// </summary>
		ClaimSet requiredClaims;

		/// <summary>
		/// Initializes a new instance of the ClaimsPermission class.
		/// </summary>
		public ClaimsPermission()
		{

			// Initialize the object
			this.isUnrestricted = true;
			this.isAuthenticated = false;
			this.requiredClaims = new DefaultClaimSet();

		}

		/// <summary>
		/// Create an object to manage the resources based on a set of claims.
		/// </summary>
		/// <param name="isAuthenticated">Indicates whether the Principal needs to be authenticated.</param>
		/// <param name="requiredClaims">A Set of Claims the Principal must have to use a resource.</param>
		public ClaimsPermission(Boolean isAuthenticated, ClaimSet requiredClaims)
		{

			// Initialize the object
			this.isUnrestricted = false;
			this.isAuthenticated = isAuthenticated;
			this.requiredClaims = requiredClaims;

		}

		/// <summary>
		/// Makes a copy of this ClaimsPermission.
		/// </summary>
		/// <returns>A copy of the ClaimsPermission object.</returns>
		public IPermission Copy()
        {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Throws a System.Security.SecurityException at run time if the security requirement is not met.
		/// </summary>
		public void Demand()
        {

			// It's always possible for a sloppy (or malicious) programmer to call a method that demands claims.  If that method has not set up a security context 
			// properly, then we won't be able to satisify the request for claims.
			ServiceSecurityContext serviceSecurityContext = ServiceSecurityContext.Current;
			if (serviceSecurityContext == null)
				throw new SecurityException(Properties.Resources.InvalidSecurityContextError);

			// If the premissions require the user to be authenticated, then throw a security exception.
			if (this.isAuthenticated && !ServiceSecurityContext.Current.PrimaryIdentity.IsAuthenticated)
				throw new SecurityException(Properties.Resources.PrincipalNotAuthenticatedError);

			// The authorization context has the claims that were established for this thread.
			AuthorizationContext authorizationContext = serviceSecurityContext.AuthorizationContext;

			// If claims have been asserted for this method, then we're going to check whether the available claims meet the criteria of the claims requested by the
			// called method.
			if (!this.isUnrestricted)
			{

				// There are several sets of claims that the current security context possesses.  We are going to check each one to see if the issuer is the same 
				// and, if it is, we'll then check to see if all of the claims required by the method match any of the claims offered by the security context.  If 
				// the security context possesses all the required claims, then the method can be called.
				Boolean areClaimsSatisfied = false;
				foreach (ClaimSet claimSet in authorizationContext.ClaimSets)
					if (this.requiredClaims.Issuer == claimSet.Issuer)
					{
						Boolean isMatchingSet = true;
						foreach (Claim claim in this.requiredClaims)
							if (!claimSet.ContainsClaim(claim))
							{
								isMatchingSet = false;
								break;
							}
						if (isMatchingSet)
						{
							areClaimsSatisfied = true;
							break;
						}
					}

				// An exception is thrown if we can't find the required claims in the current security context.
				if (!areClaimsSatisfied)
					throw new FaultException<InsufficientClaimsFault>(new InsufficientClaimsFault(Properties.Resources.AccessDeniedInsufficientClaimsError));

			}
	
        }

		/// <summary>
		/// Converts XML into a permission.
		/// </summary>
		/// <param name="e">An XML description of the permission.</param>
		public void FromXml(SecurityElement e)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates and returns a permission that is the intersection of the current permission and the specified permission.
		/// </summary>
		/// <param name="target">A permission to intersect with the current permission. It must be of the same type as the current
		/// permission.</param>
		/// <returns>A new permission that represents the intersection of the current permission and the specified permission. This
		/// new permission is null if the intersection is empty.</returns>
		public IPermission Intersect(IPermission target)
        {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Determines whether the current permission is a subset of the specified permission.
		/// </summary>
		/// <param name="target">A permission that is to be tested for the subset relationship. This permission must be of the same
		/// type as the current permission.</param>
		/// <returns>true if the current permission is a subset of the specified permission; otherwise, false.</returns>
		public Boolean IsSubsetOf(IPermission target)
        {
			return false;
        }

		/// <summary>
		/// Convert the permissions to XML.
		/// </summary>
		/// <returns>A Security Element</returns>
		public SecurityElement ToXml()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a permission that is the union of the current permission and the specified permission.
		/// </summary>
		/// <param name="target">A permission to combine with the current permission. It must be of the same type as the
		/// current permission.</param>
		/// <returns>A new permission that represents the union of the current permission and the specified permission.</returns>
		public IPermission Union(IPermission target)
        {
			throw new NotImplementedException();
		}

    }

}
