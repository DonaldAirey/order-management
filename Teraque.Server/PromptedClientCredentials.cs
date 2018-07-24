namespace Teraque
{

	using System;
	using System.IdentityModel.Selectors;
	using System.ServiceModel;
	using System.ServiceModel.Description;
	using System.ServiceModel.Dispatcher;
	using System.ServiceModel.Channels;

	/// <summary>
	/// Enables the user to configure client and service credentials as well as service credential authentication settings for use on the client side of
	/// communication.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class PromptedClientCredentials : ClientCredentials
	{

		/// <summary>
		/// The credentials.
		/// </summary>
		public Object Credentials { get; set; }

		/// <summary>
		/// Initialize a new instance of the PromptedClientCredentials class.
		/// </summary>
		public PromptedClientCredentials() : base() { }

		/// <summary>
		/// Initializes a new instance of the PromptedCredentials class.
		/// </summary>
		/// <param name="promptedClientCredentials">The original PromptedClientCredentials object.</param>
		PromptedClientCredentials(PromptedClientCredentials promptedClientCredentials) : base(promptedClientCredentials) { }

		/// <summary>
		/// Creates a manager for the security tokens used by this set of credentials.
		/// </summary>
		/// <returns>An object that manages security tokens for the client.</returns>
		public override SecurityTokenManager CreateSecurityTokenManager()
		{

			// Create an object that manages security tokens for this set of client credentials.
			return new CustomSecurityTokenManager(this);

		}

		/// <summary>
		/// Applies the specified client behavoir to the endpoint.
		/// </summary>
		/// <param name="serviceEndpoint">The endpoint to which the specified client behavior is applied.</param>
		/// <param name="behavior">The client behavior that is applied.</param>
		public override void ApplyClientBehavior(ServiceEndpoint serviceEndpoint, ClientRuntime behavior)
		{

			// Validate the parameter before using it.
			if (serviceEndpoint == null)
				throw new ArgumentNullException("serviceEndpoint");
			if (behavior == null)
				throw new ArgumentNullException("behavior");

			// This will create channel initializers for HTTP bindings.
			if (serviceEndpoint.Binding is BasicHttpBinding)
			{

				// Extract the HTTP bindings from the generic object.
				BasicHttpBinding basicHttpBinding = serviceEndpoint.Binding as BasicHttpBinding;

				// Create a specialized initializer that will prompt the user for the appropriate credentials.
				switch (basicHttpBinding.Security.Transport.ClientCredentialType)
				{

				case HttpClientCredentialType.Basic:

					// Prompts for user name, domain & password.
					behavior.InteractiveChannelInitializers.Add(new UserNameChannelInitializer(this));
					break;

				case HttpClientCredentialType.Certificate:

					// Selects a certificate from the local store.
					behavior.InteractiveChannelInitializers.Add(new CertificateChannelInitializer(this));
					break;

				}

			}

			// This will create channel initializes for TCP bindings.
			if (serviceEndpoint.Binding is NetTcpBinding)
			{

				// Extract the TCP bindings from the generic object.
				NetTcpBinding netTcpBinding = serviceEndpoint.Binding as NetTcpBinding;

				// Create a specialized initializer that will prompt the user for the appropriate credentials.
				switch (netTcpBinding.Security.Transport.ClientCredentialType)
				{

				case TcpClientCredentialType.Certificate:

					// Selects a certificate from the local store.
					behavior.InteractiveChannelInitializers.Add(new CertificateChannelInitializer(this));
					break;

				case TcpClientCredentialType.Windows:

					behavior.InteractiveChannelInitializers.Add(new UserNameChannelInitializer(this));
					break;

				}

			}

			// Allow the base class to apply the remaining behavoirs.
			base.ApplyClientBehavior(serviceEndpoint, behavior);

		}

		/// <summary>
		/// Creates a copy of the client credentials.
		/// </summary>
		/// <returns>A copy of the client credentials</returns>
		protected override ClientCredentials CloneCore()
		{
			return new PromptedClientCredentials(this);
		}

	}

}
