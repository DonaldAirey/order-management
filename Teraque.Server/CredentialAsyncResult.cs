namespace Teraque
{

	using System;
	using System.Security.Cryptography;
	using System.Security.Cryptography.X509Certificates;
	using System.ServiceModel;
	using System.ServiceModel.Dispatcher;
	using System.Threading;
	using System.Windows;

	/// <summary>
	/// Used to coordinate the dialog boxes that prompt users for their credentials for authentication.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class CredentialAsyncResult : IAsyncResult
	{

		/// <summary>
		/// Indicates that the operation completed synchronously.
		/// </summary>
		Boolean completedSynchronously;
	
		/// <summary>
		/// Gets the callback used to complete the operation.
		/// </summary>
		public AsyncCallback AsyncCallback { get; set; }

		/// <summary>
		/// Gets a user-defined Object that qualifies or contains information about an asynchronous operation.
		/// </summary>
		public Object AsyncState { get; set; }

		/// <summary>
		/// Gets a WaitHandle that is used to wait for an asynchronous operation to complete.
		/// </summary>
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Gets a value that indicates whether the asynchronous operation completed synchronously.
		/// </summary>
		public Boolean CompletedSynchronously
		{
			get
			{
				return this.completedSynchronously;
			}
		}

		/// <summary>
		/// Gets or sets the credentials.
		/// </summary>
		public Object Credentials { get; set; }

		/// <summary>
		/// Defines the behavior of outbound request and request/reply channels used by client applications.
		/// </summary>
		public IClientChannel IClientChannel { get; set; }

		/// <summary>
		/// Indicates that the user cancelled the initialization of the channel.
		/// </summary>
		public Boolean IsCanceled { get; set; }

		/// <summary>
		/// Gets a value that indicates whether the asynchronous operation has completed.
		/// </summary>
		public Boolean IsCompleted
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Sets the results to indicate the operation completed synchronously.
		/// </summary>
		public void SetCompletedSynchronously()
		{

			// This tells the channel that it doesn't need to prompt the user for credentials.
			this.completedSynchronously = true;

		}

	}

}
