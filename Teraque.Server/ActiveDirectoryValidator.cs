namespace Teraque
{

	using System;
	using System.DirectoryServices.AccountManagement;
	using System.DirectoryServices.Protocols;
	using System.IdentityModel.Selectors;
	using System.ServiceModel;
	using Teraque;

	/// <summary>
	/// Validates a user using the active directory.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public sealed class ActiveDirectoryValidator : UserNamePasswordValidator, IDisposable
	{

		/// <summary>
		/// Encapsulates the server or domain against which all active directory operations are performed, the container that is used as the base of those
		/// operations, and the credentials used to perform the operations.
		/// </summary>
		private PrincipalContext principalContext = null;

		/// <summary>
		/// Releases allocated resources.
		/// </summary>
		public void Dispose()
		{
			this.principalContext.Dispose();
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Validates the specified tenant, username and password.
		/// </summary>
		/// <param name="userName">The combined tenant and user name to validate</param>
		/// <param name="password">The password to validate.</param>
		public override void Validate(String userName, String password)
		{

			// For reasons I don't completely understand, the active directory service times out during inactivity.  I researched it as best I could on the web and 
			// there were no answers though there were plenty of other users with the same issue.  For now, the answer is to catch the error and retry as it seems
			// to work the second time.  This problem should be taken up with Microsoft sometime in the near future for a resolution.
			while (true)
			{
				try
				{
                    if (this.principalContext == null)
                    {
                        // This is our connection to the active directory.
                        this.principalContext = new PrincipalContext(
                            ContextType.ApplicationDirectory,
                            Properties.Settings.Default.ActiveDirectoryRoot,
                            Properties.Settings.Default.OrganizationDistinctName,
                            ContextOptions.SimpleBind);
                    }

                    // Throw a fault if the user can't be authenticated using the fast binding.  Note that we are taking the raw user name fed to us by the client.  We
                    // assume the security token on the client has formatted the name as a distinguished name.  This is about as stream-lined a validation as you'll get
                    // using Active Directory.
                    if (this.principalContext.ValidateCredentials(userName, password, ContextOptions.SimpleBind))
						break;
					throw new FaultException(Properties.Resources.InvalidUserNamePasswordError);
				}
				catch (LdapException ldapException)
				{
                    this.principalContext = null;
					Log.Error("{0}", ldapException.Message);
				}
				catch (DirectoryOperationException directoryOperationException)
				{
                    this.principalContext = null;
                    Log.Error("{0}", directoryOperationException.Message);
				}
			}

		}

	}

}
