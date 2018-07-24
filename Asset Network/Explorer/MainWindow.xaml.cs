namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.IO;
	using System.IO.Pipes;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading;
	using System.Windows;
	using System.Windows.Input;
	using System.Windows.Media.Animation;
	using System.Windows.Navigation;
	using System.Windows.Threading;
	using Teraque;
	using Teraque.Windows;
	using Teraque.Windows.Controls;
	using Teraque.Windows.Navigation;
	using Teraque.AssetNetwork.Windows;
	using Teraque.AssetNetwork.WebService;
	using Teraque.AssetNetwork.Properties;
	using System.Linq.Dynamic;

	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using System.ServiceModel.Description;

	/// <summary>
	/// The main window of Asset Explorer application.
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	partial class MainWindow : ExplorerWindow
	{

		static Regex aggregateRegex = new Regex("select sum((?<selectClause>.*)) from (?<tableName>.*) where (?<whereClause>.*)");

		static Regex scalarRegex = new Regex("select (?<selectClause>.*) from (?<tableName>.*) where (?<whereClause>.*)");

		Thread pipeThread;

		/// <summary>
		/// The items used to navigate this window.
		/// </summary>
		AssetNetworkCollection assetNetworkCollection;

		/// <summary>
		/// Splash screen to keep the user entertained while we load the data model.
		/// </summary>
		SplashScreen splashScreen;

		/// <summary>
		/// Initialize a new instance of the MainWindow class.
		/// </summary>
		public MainWindow()
		{

			// The IDE maintained resources are initialized here.
			this.InitializeComponent();

			// The frame requires a hierarchical collection to navigate.  The 'AssetNetworkCollection' provides this context but requires a background thread in order to
			// establish the user's identity.  Running a web service is incompatible with the design surface because the design surface has no ability to read the
			// applicaton configuration file which is where the endpoints are kept.  This will prevent initialization of the collection when designing.
			if (!DesignerProperties.GetIsInDesignMode(this))
			{
				// This collection is connected to the data model and provides the data context for the entire application.  It contains the hierarchy used to navigate.
				this.assetNetworkCollection = new AssetNetworkCollection();
				this.DataContext = this.assetNetworkCollection;
			}

			// This will keep the breadcrumb bar synchronized with the list when the list changes.
			this.Items.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnItemsCollectionChanged);

			// This will distract the user while we load the data model.
			this.splashScreen = new SplashScreen();
			this.splashScreen.Show();

			// This thread will force the user to provide their credentials to the web service before any of the background threads will connect.  The
			// InteractiveChannelInitializer will only accept credentials from a single threaded (STA) thread and will not release the background threads to use 
			// the channel until the 'ValidCredentials' event is signaled.
			Thread loginThread = new Thread(this.LoginThread);
			loginThread.SetApartmentState(ApartmentState.STA);
			loginThread.Start();

			// This thread will provide the Named Pipe support for Excel Financial Function support.
			this.isPipeThreadRunning = true;
			this.pipeThread = new Thread(this.PipeThread);
			this.pipeThread.IsBackground = true;
			this.pipeThread.Start();

			// Once the data model is loaded we can make the main application visible.
			this.NavigationService.Navigated += new NavigatedEventHandler(this.OnNavigated);

		}

		Boolean isPipeThreadRunning = true;

		/// <summary>
		/// Forces the user to log into the web service.
		/// </summary>
		/// <param name="state">The thread start parameter.</param>
		void PipeThread(Object state)
		{

			try
			{

				NamedPipeServerStream namedPipeServerStream = new NamedPipeServerStream("Silopipe");

				Int32 threadId = Thread.CurrentThread.ManagedThreadId;

				// Wait for a client to connect
				namedPipeServerStream.WaitForConnection();

				while (this.isPipeThreadRunning)
				{

					Boolean isHandled = false;

					try
					{

						// Read the request from the client. Once the client has written to the pipe its security token will be available.
						StreamString streamString = new StreamString(namedPipeServerStream);

						String query = streamString.ReadString();
						if (query == String.Empty)
						{
							Thread.Sleep(100);
							continue;
						}

						DateTime startTime = DateTime.Now;


						// Make sure the account data is loaded for the given date and account combination.
						Match scalarMatch = MainWindow.scalarRegex.Match(query);
						if (scalarMatch.Success)
						{

							String tableName = scalarMatch.Groups["tableName"].Value;
							String whereClause = scalarMatch.Groups["whereClause"].Value;
							String selectClause = scalarMatch.Groups["selectClause"].Value;

							IEnumerable iEnumerable = DataModel.Tables[tableName] as IEnumerable;
							if (iEnumerable != null)
							{
								Decimal total = 0.0m;
								var result = iEnumerable.AsQueryable().Where(whereClause).GroupBy("TaxLotRow.PositionRow.AccountId", "it").Select(selectClause);
								foreach (Decimal sum in result)
									total += sum;
								streamString.WriteString(total.ToString());
							}

							isHandled = true;

						}

						Console.WriteLine("Time {0}: {1}", DateTime.Now.Subtract(startTime).TotalMilliseconds, query);

						if (!isHandled)
							streamString.WriteString("#Syntax Error");

					}
					catch (IOException e)
					{
						Console.WriteLine("ERROR: {0}", e.Message);
					}

				}

				namedPipeServerStream.Close();

			}
			catch { }

		}

		/// <summary>
		/// Forces the user to log into the web service.
		/// </summary>
		/// <param name="state">The thread start parameter.</param>
		void LoginThread(Object state)
		{

			// This will attempt to log in three times before giving up.
			Int32 counter = 3;
			do
			{

				// This will force the user to log in using a single threaded apartment (STA).
				using (WebServiceClient webServiceClient = new WebServiceClient(Settings.Default.WebServiceEndpoint))
				{

					// For some reason, the initial call will sometimes fail.  This will continue to try until we log in or die trying.
					try
					{

						// This is a dummy operation here but it will force the user to enter the credentials in the foreground.
						webServiceClient.GetUserId();

						// Allow the other background threads to use this channel as it is now initialized.
						InteractiveChannelInitializer.IsPrompted = false;
						InteractiveChannelInitializer.ValidCredentials.Set();

						// This will force the loop to exit.
						counter = 0;

					}
					catch { }

				}

			} while (counter-- != 0);

		}

		/// <summary>
		/// Displays information about the application and the license.
		/// </summary>
		/// <param name="sender">The Object that sent the command.</param>
		/// <param name="executedRoutedEventArgs">The command arguments.</param>
		void OnAbout(Object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
		{

			// This will display the About box with the license information in it.
			WindowAbout windowAbout = new WindowAbout()
			{
				Version = typeof(ExplorerWindow).Assembly.GetName().Version
			};
			windowAbout.ShowDialog();

		}

		/// <summary>
		/// Occurs when a request is made to cancel the current search.
		/// </summary>
		protected override void OnCancelSearch()
		{

			// This will tell the search box to clear itself.  It could be bound to a property but it is likely the connection in a real world example would be 
			// more complex.
			this.SearchBox.IsSearching = false;

		}

		/// <summary>
		/// Raises the Closed event.
		/// </summary>
		/// <param name="args">An EventArgs that contains the event data.</param>
		protected override void OnClosed(EventArgs args)
		{

			// This Object is connected to the data model and must be disposed explicilty.
			this.assetNetworkCollection.Dispose();

			// Allow the base class to handle the rest of the closing.
			base.OnClosed(args);

		}

		/// <summary>
		/// Handles the CollectionChanged event. 
		/// </summary>
		/// <param name="sender">The Object that raised the event.</param>
		/// <param name="e">Information about the event.</param>
		void OnItemsCollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
		{

			// If the collection used for navigation changes (i.e. if a user were to log in with another id), then we need to make sure that the Source is
			// synchronized to the new list.  This will basically select the first child of the hierarchy when the collection is changed.
			if (e.Action == NotifyCollectionChangedAction.Add && this.Items.Count != 0)
			{
				Uri newUri = ExplorerHelper.GenerateSource(this.Items[0] as ExplorerItem);
				if (this.Source != newUri)
					this.Source = newUri;
			}

		}

		/// <summary>
		/// Handle the Navigated event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="navigationEventArgs">The event data.</param>
		void OnNavigated(Object sender, NavigationEventArgs navigationEventArgs)
		{

			// When the first page has been navigated we will make the application visible.  If we make the application visible before this event we have to
			// watch the blank background for a while.  It is much snappier to have the splash screen give some visible indication that the application is loaded and then
			// just draw the screen after we've navigated to the first page.
			this.Visibility = Visibility.Visible;
			this.splashScreen.Close();
			this.splashScreen = null;

			// This event is only required when the application is initialized.
			this.NavigationService.Navigated -= new NavigatedEventHandler(OnNavigated);

		}

		/// <summary>
		/// Occurs when a request is made to search for some text in the hierarchy.
		/// </summary>
		/// <param name="text">The text to be found.</param>
		protected override void OnSearch(String text)
		{

			// An empty string is a signal to end the search because you can't search for nothing.  Otherwise we simulate a search that takes four seconds to
			// complete in order to demonstrate the look and feel of the progress indicator in the breadcrumb bar.
			if (String.IsNullOrEmpty(text))
			{
				this.IsSearching = false;
				this.BreadcrumbBar.BeginAnimation(BreadcrumbBar.ProgressValueProperty, null);
			}
			else
			{
				this.IsSearching = true;
				this.BreadcrumbBar.BeginAnimation(BreadcrumbBar.ProgressValueProperty, new DoubleAnimation(0.0, 1.0, TimeSpan.FromMilliseconds(4000)));
			}

		}

		// Defines the data protocol for reading and writing strings on our stream
		public class StreamString
		{
			private Stream ioStream;
			private UnicodeEncoding streamEncoding;

			public StreamString(Stream ioStream)
			{
				this.ioStream = ioStream;
				streamEncoding = new UnicodeEncoding();
			}

			public String ReadString()
			{
				Int32 len = 0;
				byte[] inBuffer = null;

				try
				{

					Int32 bytes = ioStream.ReadByte();
					if (bytes != -1)
					{
						len = bytes * 256;
						len += ioStream.ReadByte();
						inBuffer = new byte[len];
						ioStream.Read(inBuffer, 0, len);
					}

				}
				catch { }

				return inBuffer == null ? String.Empty : streamEncoding.GetString(inBuffer);

			}

			public Int32 WriteString(String outString)
			{
				byte[] outBuffer = streamEncoding.GetBytes(outString);
				Int32 len = outBuffer.Length;
				if (len > UInt16.MaxValue)
				{
					len = (Int32)UInt16.MaxValue;
				}
				try
				{
					ioStream.WriteByte((byte)(len / 256));
					ioStream.WriteByte((byte)(len & 255));
					ioStream.Write(outBuffer, 0, len);
					ioStream.Flush();
				}
				catch { }

				return outBuffer.Length + 2;
			}
		}

	}

}
