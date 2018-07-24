namespace Teraque
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Net;
	using System.Security;
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using System.ServiceModel.Dispatcher;
	using System.Text;
	using System.Threading;
	using System.Windows;
	using System.Windows.Threading;

	/// <summary>
	/// Allows a user to enter a domain, user name and password before a channel is initialized.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	sealed class UserNameChannelInitializer : InteractiveChannelInitializer
	{

		/// <summary>
		/// The credentials associated with all instances of this channel.
		/// </summary>
		static DistinguishedNameCredential distinguishedNameCredentials = UserNameChannelInitializer.GetDistinguishedNameCredentials();

		/// <summary>
		/// Initialize the UserNameChannelInitializer class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static UserNameChannelInitializer()
		{

			// We never override the designer's decision to propt the user.  However, if we've decided to not prompt the user for credentials, we'll then test to see if that's
			// possible.  It's only possible to ignore prompting when there is a valid set of credentials.
			if (!UserNameChannelInitializer.IsPrompted)
				UserNameChannelInitializer.IsPrompted =
					(String.IsNullOrEmpty(distinguishedNameCredentials.DistinguishedName) || String.IsNullOrEmpty(distinguishedNameCredentials.Password)) ||
					!distinguishedNameCredentials.RememberCredentials;

		}
	
		/// <summary>
		/// Gets the DistinguishedNameCredentials from the persistent storage.
		/// </summary>
		/// <returns>The most recently stored DistinguishedNameCredential from the users persistent store.</returns>
		static DistinguishedNameCredential GetDistinguishedNameCredentials()
		{

			// Construct the credentials from the users settings.
			return new DistinguishedNameCredential()
			{
				DistinguishedName = Properties.Settings.Default.UserDistinguishedName,
				RememberCredentials = Properties.Settings.Default.RememberCredentials,
				SecurePassword = Cryptography.DecryptString(Properties.Settings.Default.UserPassword)
			};

		}

		/// <summary>
		/// Initializes a new instance of a UserNameChannelInitializer class.
		/// </summary>
		/// <param name="promptedClientCredentials">The credentials that are to be initialized.</param>
		public UserNameChannelInitializer(PromptedClientCredentials promptedClientCredentials) : base(promptedClientCredentials)
        {

            // If credentials were supplied by the channel, then they will override the static credentials stored in the settings file.
            if (!String.IsNullOrEmpty(promptedClientCredentials.UserName.UserName))
            {

                // This will create a set of prompted credentials from the channel's credentials.
                this.Credentials = new DistinguishedNameCredential()
                {
                    DistinguishedName = promptedClientCredentials.UserName.UserName,
                    SecurePassword = Cryptography.ToSecureString(promptedClientCredentials.UserName.Password),
                    RememberCredentials = true,
                };

                // Explicitly providing credentials will defeat the prompting by design.
                UserNameChannelInitializer.IsPrompted = false;

            }
        
        }

		/// <summary>
		/// Gets or sets the credentials for this channel.
		/// </summary>
		protected override object Credentials
		{

			get
			{
				return UserNameChannelInitializer.distinguishedNameCredentials;
			}

			set
			{

				// Setting this value also saves the credentials the the persistent store (or clears the persistent memory, depending on the user's preference).  
				// Note that the password is encrypted before being saved.
				UserNameChannelInitializer.distinguishedNameCredentials = value as DistinguishedNameCredential;
				Properties.Settings.Default.RememberCredentials = UserNameChannelInitializer.distinguishedNameCredentials.RememberCredentials;
				if (UserNameChannelInitializer.distinguishedNameCredentials.RememberCredentials)
				{
					Properties.Settings.Default.UserDistinguishedName = UserNameChannelInitializer.distinguishedNameCredentials.DistinguishedName;
					Properties.Settings.Default.UserPassword = Cryptography.EncryptString(UserNameChannelInitializer.distinguishedNameCredentials.SecurePassword);
				}
				else
				{
					Properties.Settings.Default.UserDistinguishedName = String.Empty;
					Properties.Settings.Default.Domain = String.Empty;
					Properties.Settings.Default.UserPassword = String.Empty;
				}

			}
		}

		/// <summary>
		/// Gets a callback method that will prompt the user for their credentials.
		/// </summary>
		protected override PromptCallback Prompt
		{
			get
			{
				return new PromptCallback(UserNameChannelInitializer.PromptForUserName);
			}
		}

		/// <summary>
		/// Prompts the user for a certificate to authenticate that user on the channel.
		/// </summary>
		/// <param name="credentialAsyncResult">The CredentialAsyncResult that is called to complete the initialization.</param>
		static void PromptForUserName(CredentialAsyncResult credentialAsyncResult)
		{

			// This dialog will prompt the user with a list of certificate that match the
			WindowDistinguishedName windowBasic = new WindowDistinguishedName();
			windowBasic.DistinguishedNameCredential = credentialAsyncResult.Credentials as DistinguishedNameCredential;
			windowBasic.ServerName = credentialAsyncResult.IClientChannel.RemoteAddress.Uri.Host;

			// This will display the dialog and wait for the user to either accept the credentials or cancel out of the login.
			if (windowBasic.ShowDialog() == true)
			{
				credentialAsyncResult.IsCanceled = false;
				credentialAsyncResult.Credentials = windowBasic.DistinguishedNameCredential;
			}
			else
			{
				credentialAsyncResult.IsCanceled = true;
			}

			// If a valid callback was provided in the IAsyncResult structure then send a signal that we're through.
			if (credentialAsyncResult.AsyncCallback != null)
				credentialAsyncResult.AsyncCallback(credentialAsyncResult);

		}

	}

}
