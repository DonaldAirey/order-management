namespace Teraque
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.IdentityModel.Selectors;
	using System.IdentityModel.Tokens;
	using System.Net;
	using System.Security.Cryptography.X509Certificates;
	using System.ServiceModel;
	using System.ServiceModel.Description;

	/// <summary>
	/// Manages security tokens for the client.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class CustomSecurityTokenManager : ClientCredentialsSecurityTokenManager
	{

		/// <summary>
		/// Initializes a new instance of the CustomSecurityTokenManager class.
		/// </summary>
		/// <param name="clientCredentials">The original ClientCredential manager.</param>
		public CustomSecurityTokenManager(ClientCredentials clientCredentials) : base(clientCredentials) { }

		/// <summary>
		/// Creates a security token provider.
		/// </summary>
		/// <param name="securityTokenRequirement">The System.IdentityModel.Selectors.SecurityTokenRequirement</param>
		/// <returns>A Security Token Provider.</returns>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		public override SecurityTokenProvider CreateSecurityTokenProvider(SecurityTokenRequirement securityTokenRequirement)
		{

			// Validate the argument before using it.
			if (securityTokenRequirement == null)
				throw new ArgumentNullException("securityTokenRequirement");

			// The actual credentials are held by the PromptedClientCredentials object.  There are examles out there where the properties of the channel are used to
			// pass the credentials back to the security token manager, but the TCP stack doesn't appear to have this collection, so we've chosen to use a property
			// of this class to communicate the credentials.
			if (this.ClientCredentials is PromptedClientCredentials)
			{

				// A set of credentials is maintained by the PromptedClientCredentials and populated by the channel initializers with input from the user.
				PromptedClientCredentials promptedClientCredentials = this.ClientCredentials as PromptedClientCredentials;

				// This will create the appropriate security token provider based on the key usage required.
				switch (securityTokenRequirement.KeyUsage)
				{

				case SecurityKeyUsage.Signature:

					// This will create a security token for distinguished user names and passwords.
					if (securityTokenRequirement.TokenType == SecurityTokenTypes.UserName)
					{
						DistinguishedNameCredential distinguishedNameCredential = promptedClientCredentials.Credentials as DistinguishedNameCredential;
						return new UserNameSecurityTokenProvider(distinguishedNameCredential.DistinguishedName, distinguishedNameCredential.Password);

					}

					// This will create a security token for certificates.
					if (securityTokenRequirement.TokenType == SecurityTokenTypes.X509Certificate)
					{
						X509Certificate2 x509Certificate2 = promptedClientCredentials.Credentials as X509Certificate2;
						return new X509SecurityTokenProvider(x509Certificate2);
					}

					break;

				}

			}

			// Any configuration not handled by the logic above is given a generic token handler.
			return base.CreateSecurityTokenProvider(securityTokenRequirement);

		}

		/// <summary>
		/// Creates a security token authenticator.
		/// </summary>
		/// <param name="tokenRequirement">tokenRequirement</param>
		/// <param name="outOfBandTokenResolver">outOfBandTokenResolver</param>
		/// <returns>A custom security token authenticator.</returns>
		public override SecurityTokenAuthenticator CreateSecurityTokenAuthenticator(
			SecurityTokenRequirement tokenRequirement,
			out SecurityTokenResolver outOfBandTokenResolver)
		{
			return base.CreateSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
		}

	}

}
