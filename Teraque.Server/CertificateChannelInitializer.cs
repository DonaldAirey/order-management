namespace Teraque
{

    using System;
	using System.Diagnostics.CodeAnalysis;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.ServiceModel;
	using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;

	/// <summary>
	/// Allows a user to select a certificate before a channel is initialized.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	sealed class CertificateChannelInitializer : InteractiveChannelInitializer
	{

		/// <summary>
		/// The name of the store where the current user's credentials are stored.
		/// </summary>
		const String myStoreName = "My";

		/// <summary>
		/// An OID that identifies clients that can be used for client authentication.
		/// </summary>
		static Oid oidClientAuthentication = new Oid("1.3.6.1.5.5.7.3.2", "Client Authentication");

		/// <summary>
		/// The X509 certificate used to authenticate this channel.
		/// </summary>
		static X509Certificate2 x509Certificate2 = CertificateChannelInitializer.GetPersistentCertificate();

		/// <summary>
		/// Initializes a new instance of a CertificateChannelInitializer class.
		/// </summary>
		/// <param name="promptedClientCredentials">The credentials that are to be initialized.</param>
		public CertificateChannelInitializer(PromptedClientCredentials promptedClientCredentials) : base(promptedClientCredentials) { }

		/// <summary>
		/// Gets the saved certificate.
		/// </summary>
		/// <returns>The certificate that was saved to the user properties store after the last use of this channel.</returns>
		static X509Certificate2 GetPersistentCertificate()
		{

			// This is capture the last known certificate used by this channel.
			X509Certificate2 persistentCertificate = null;

			// The distinguished subject name is used to store the unique identification of the last selected certificate.  If a valid certificate can be found that
			// matches the distinguished name, then there's no need to prompt the user again (unless the prompt is forced).
			String thumbprint = Properties.Settings.Default.Thumbprint;
			if (!String.IsNullOrEmpty(thumbprint))
			{
				X509Store store = new X509Store(CertificateChannelInitializer.myStoreName, StoreLocation.CurrentUser);
				store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
				foreach (X509Certificate2 x509Certificate2 in store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, true))
					persistentCertificate = x509Certificate2;
				store.Close();
			}

			// Return the last known certificate used by this channel.
			return persistentCertificate;

		}

		/// <summary>
		/// Initialize the CertificateChannelInitializer class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static CertificateChannelInitializer()
		{

			// There is no need to prompt the user if a certificate has been found.  The channel initializer will assume that the last known credentials are the
			// ones to be used by this channel.
			if (CertificateChannelInitializer.x509Certificate2 != null)
				CertificateChannelInitializer.IsPrompted = false;

		}

		/// <summary>
		/// Prompts the user for a certificate to authenticate that user on the channel.
		/// </summary>
		/// <param name="state">A generic parameter used to initialize the thread.</param>
		static void PromptForCertificate(CredentialAsyncResult credentialAsyncResult)
		{

			// This will select a list of valid certificates from the store that can be used for client authentication.
			X509Store store = new X509Store(CertificateChannelInitializer.myStoreName, StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
			X509Certificate2Collection clientCertificates = store.Certificates.Find(
				X509FindType.FindByApplicationPolicy,
				CertificateChannelInitializer.oidClientAuthentication.Value,
				true);

			// This dialog will prompt the user with a list of certificate that match the
			WindowCertificate windowCertificate = new WindowCertificate();
			foreach (X509Certificate2 x509Certificate2 in clientCertificates)
				windowCertificate.X509Certificate2s.Add(x509Certificate2);
			windowCertificate.X509Certificate2 = credentialAsyncResult.Credentials as X509Certificate2;
			if (windowCertificate.ShowDialog() == true)
			{
				credentialAsyncResult.IsCanceled = false;
				credentialAsyncResult.Credentials = windowCertificate.X509Certificate2;
			}
			else
			{
				credentialAsyncResult.IsCanceled = true;
			}

			// If a valid callback was provided in the IAsyncResult structure then send a signal that we're through.
			if (credentialAsyncResult.AsyncCallback != null)
				credentialAsyncResult.AsyncCallback(credentialAsyncResult);

		}

		/// <summary>
		/// Gets or sets the credentials for this channel.
		/// </summary>
		protected override Object Credentials
		{

			get
			{
				return CertificateChannelInitializer.x509Certificate2;
			}

			set
			{

				// Setting the credentials will also save the credentials to a persistent store.
				CertificateChannelInitializer.x509Certificate2 = value as X509Certificate2;
				Properties.Settings.Default.Thumbprint = CertificateChannelInitializer.x509Certificate2.Thumbprint;

			}

		}

		/// <summary>
		/// Gets a callback method that will prompt the user for their credentials.
		/// </summary>
		protected override PromptCallback Prompt
		{
			get
			{
				return new PromptCallback(CertificateChannelInitializer.PromptForCertificate);
			}
		}

	}

}
