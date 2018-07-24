namespace Teraque
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using System.ServiceModel.Dispatcher;
	using System.Threading;
	using System.Windows;
	using System.Windows.Threading;

	/// <summary>
	/// Allows a user to enter a domain, user name and password before a channel is initialized.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public abstract class InteractiveChannelInitializer : IInteractiveChannelInitializer
	{

		/// <summary>
		/// Used to call the prompt from a DispatcherObject.
		/// </summary>
		/// <param name="credentialAsyncResult">The CredentialAsyncResult that is called to complete the initialization.</param>
		protected delegate void PromptCallback(CredentialAsyncResult credentialAsyncResult);

		/// <summary>
		/// Indicates that the user should be prompted for credentials.
		/// </summary>
		static Boolean isPrompted = true;

		/// <summary>
		/// A prompt used for selecting a certificate for the channel.
		/// </summary>
		PromptedClientCredentials promptedClientCredentials;

		/// <summary>
		/// Used to coordinate access between threads.
		/// </summary>
		static Object syncRoot = new Object();

		/// <summary>
		/// Coordinates an attempt to log in by one or more threads.
		/// </summary>
		static ManualResetEvent validCredentialsEvent = new ManualResetEvent(false);

		/// <summary>
		/// Initializes a new instance of a ChannelInitializer class.
		/// </summary>
		/// <param name="promptedClientCredentials">The credentials that are to be initialized.</param>
		protected InteractiveChannelInitializer(PromptedClientCredentials promptedClientCredentials)
		{

			// Initialize the object.
			this.promptedClientCredentials = promptedClientCredentials;

		}

		/// <summary>
		/// Gets or sets the credentials for this channel.
		/// </summary>
		protected abstract Object Credentials { get; set; }

		/// <summary>
		/// Gets or sets an indication of whether the user should be prompted for credentials.
		/// </summary>
		public static Boolean IsPrompted
		{
			get
			{
				lock (InteractiveChannelInitializer.syncRoot)
					return InteractiveChannelInitializer.isPrompted;
			}
			set
			{
				lock (InteractiveChannelInitializer.syncRoot)
					InteractiveChannelInitializer.isPrompted = value;
			}
		}

		/// <summary>
		/// Gets a prompt for the types of credentials used on this channel.
		/// </summary>
		protected abstract PromptCallback Prompt { get; }

		/// <summary>
		/// Notifies one or more waiting threads that valid credentials are available on this channel.
		/// </summary>
		public static ManualResetEvent ValidCredentials
		{
			get { return InteractiveChannelInitializer.validCredentialsEvent; }
		}

		/// <summary>
		/// Starts the user interface that prompts a user for a certificate.
		/// </summary>
		/// <param name="channel">The client channel.</param>
		/// <param name="callback">The callback object.</param>
		/// <param name="state">Any state data.</param>
		/// <returns>The System.IAsyncResult to use to call back when processing has completed.</returns>
		public IAsyncResult BeginDisplayInitializationUI(IClientChannel channel, AsyncCallback callback, Object state)
		{

			// Any worker thread can request a channel, so the user prompt needs to be invoked on a Single Threaded Apartment (STA) thread.  This will create that
			// thread and communicate to it the current state of the channel.
			CredentialAsyncResult credentialAsyncResult = new CredentialAsyncResult()
			{
				AsyncCallback = callback,
				AsyncState = state,
				Credentials = this.Credentials,
				IClientChannel = channel
			};

			// Only single threaded apartments are allowed to call the user interface to prompt the user.  Background threads must wait until an apartment that can
			// prompt the user has validated the credentials.
			if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
			{

				// Only prompt the user once for their credentials.  After a set of credentials are chosen, they'll be used by all subsequent calls to use this
				// channel until the 'IsPrompted' flag is cleared.
				if (InteractiveChannelInitializer.IsPrompted)
				{

					// This will prompt the user for their credentials.  It effectively creates a modal dialog box and prevents the other threads from executing,
					// which effectively obviates the whole IAsyncResult strategy here, but this was the only way to come up with a consistent way to call the
					// channels from the console or a Windows environment.  Note that the frame will only return control to this thread when we set the 'Continue' 
					// property to 'false'.
					DispatcherFrame dispatcherFrame = new DispatcherFrame();
					dispatcherFrame.Dispatcher.BeginInvoke(DispatcherPriority.Normal, this.Prompt, credentialAsyncResult);
					dispatcherFrame.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => dispatcherFrame.Continue = false));
					Dispatcher.PushFrame(dispatcherFrame);

				}
				else
				{

					// If we have already prompted the user then return the credentials immediately.
					credentialAsyncResult.Credentials = this.Credentials;
					credentialAsyncResult.SetCompletedSynchronously();

				}

			}
			else
			{

				// The non-UI threads must wait here until a UI thread has validated the credentials.
				InteractiveChannelInitializer.ValidCredentials.WaitOne();

				// Setting the credentials in this way will make the channel act synchronously (that is, without prompting the user).  This is important after the
				// first time the channel has been initialized as we don't want to keep bother the user for credentials.
				credentialAsyncResult.Credentials = this.Credentials;
				credentialAsyncResult.SetCompletedSynchronously();

			}

			// This will be used by the caller to manage the asynchronous task that prompts the user and then invoke the EndDisplayInitializationUI with the results
			// of the dialog.
			return credentialAsyncResult;

		}

		/// <summary>
		/// Completes the user interface that prompts a user for a certificate.
		/// </summary>
		/// <param name="result">Represents the status of an asynchrous operation.</param>
		public void EndDisplayInitializationUI(IAsyncResult result)
		{

			// This will extract the channel from the generic event argument.
			CredentialAsyncResult certificateAsyncResult = result as CredentialAsyncResult;

			// The user has the option to cancel the login operation.  If they cancel, the current operation will be aborted and further attempts to initialize a
			// channel will be suspended.  If not, an attempt is made to use the credentials to authorize communication with the web service.
			if (certificateAsyncResult.IsCanceled)
			{

				// This will generate an exception that the UI threads must handle. The non-UI threads should never have to deal with what is essentially a user
				// generated exception.
				certificateAsyncResult.IClientChannel.Abort();

			}
			else
			{

				// Copy the information out of the dialog box after it has been dismissed.  This information is shared between all channels using DomainUserName
				// authentication.
				this.Credentials = certificateAsyncResult.Credentials;

			}

			// There is no good way to pass the credentials back to the security token manager.  The MSDN help provides an example that uses the properties of the
			// channel to store the credentials, but that property doesn't exist on all communication stacks (in particular, the net.tcp stack).  So we'll use the
			// PromptedClientCredentials class which is acting as an intermediary between the channel and the security token manager anyway.
			this.promptedClientCredentials.Credentials = certificateAsyncResult.Credentials;

		}

	}

}
