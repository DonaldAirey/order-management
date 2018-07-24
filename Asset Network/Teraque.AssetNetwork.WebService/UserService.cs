namespace Teraque.AssetNetwork
{

	using System;
	using System.Data;
	using System.ServiceModel;
	using System.Threading;

	/// <summary>
	/// Web Services for users.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal static class UserService
	{

		/// <summary>
		/// Gets the user's unique identifier.
		/// </summary>
		/// <returns>The user's unique identifier.</returns>		
		public static Guid GetUserId()
		{

			// Get the current transaction and the user's identity principal.
			DataModelTransaction dataModelTransaction = DataModel.CurrentTransaction;
			OrganizationPrincipal organizationPrincipal = Thread.CurrentPrincipal as OrganizationPrincipal;

			// Try to find the user in the user tables.
			DataModel.UserRow userRow = DataModel.User.UserKeyDistinguishedName.Find(organizationPrincipal.DistinguishedName);
			if (userRow == null)
			{
				Log.Error(String.Format("Invalid Login request for {0}", organizationPrincipal.DistinguishedName));
				throw new FaultException<RecordNotFoundFault>(new RecordNotFoundFault("User", new Object[] { organizationPrincipal.DistinguishedName }));
			}

			// If a record is found in the User table that matches the thread's identity, then lock it for the duration of the transaction and insure that it wasn't
			// deleted between the time it was found and the time it was locked.
			userRow.AcquireReaderLock(dataModelTransaction);
			dataModelTransaction.AddLock(userRow);
			if (userRow.RowState == DataRowState.Detached)
				throw new FaultException<RecordNotFoundFault>(new RecordNotFoundFault("User", new Object[] { organizationPrincipal.DistinguishedName }));

			// This is the user's internal identity to the data model.
			return userRow.UserId;

		}

	}

}
