namespace Teraque
{

	using System;
	using System.IdentityModel.Claims;
	using System.Security;
	using System.Security.Permissions;

	/// <summary>
	/// Specifies the code access for a Claims-Based security model.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class ClaimsPermissionAttribute : CodeAccessSecurityAttribute
    {

		/// <summary>
		/// Gets or sets the claim required.
		/// </summary>
		public String ClaimType { get; set; }

		/// <summary>
		/// Gets or sets whether authentication is required to grant permission.
		/// </summary>
		public Boolean IsAuthenticated { get; set; }

		/// <summary>
		/// Gets or sets the type of resource for which the claim is asserted.
		/// </summary>
		public String Resource { get; set; }

		/// <summary>
		/// Create a specification for the access to code for a Claims-Based security model.
		/// </summary>
		/// <param name="action"></param>
		public ClaimsPermissionAttribute(SecurityAction action)
            : base(action)
        {

			// Initialize the object
            this.IsAuthenticated = true;

        }

		/// <summary>
		/// Create the permissions needed to access code.
		/// </summary>
		/// <returns>The permission required of a thread before it can execute code.</returns>
		public override IPermission CreatePermission()
        {

			// This constructs an explicit permission that is needed by the thread's security context in order for execution to continue. If the thread doesn't
			// posses this set of claims then an exception will be thrown.  Also, if the permission is explicitly set to be unrestricted, then no claims or
			// authentication is required.
			return this.Unrestricted ?
				new ClaimsPermission() :
				new ClaimsPermission(
					this.IsAuthenticated,
					new DefaultClaimSet(DefaultClaimSet.System, new Claim(this.ClaimType, this.Resource, Rights.PossessProperty)));

        }
        
    }

}
