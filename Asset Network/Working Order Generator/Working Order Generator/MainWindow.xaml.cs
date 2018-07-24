namespace Teraque.AssetNetwork
{

	using Microsoft.Win32;
	using System;
	using System.IO;
	using System.Threading;
	using System.Windows;
	using System.Windows.Controls;
	using Teraque.AssetNetwork.WebService;
	using Teraque.AssetNetwork.Properties;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		GeneratorInfo generatorInfo = new GeneratorInfo();

		public MainWindow()
		{

			this.generatorInfo.FileName = WorkingOrderGenerator.Properties.Settings.Default.OutputDirectory;
			this.DataContext = this.generatorInfo;

			InitializeComponent();

			Thread thread = new Thread(this.LoginThread);
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();

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
						InteractiveChannelInitializer.ValidCredentials.Set();

						// This will force the loop to exit.
						counter = 0;

					}
					catch { }

				}

			} while (counter-- != 0);

		}

		void OnBrowseClick(Object sender, RoutedEventArgs e)
		{

			// Configure the 'Open File' dialog box to look for the available XML files.
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.DefaultExt = ".xml";
			saveFileDialog.Filter = "XML Documents (.xml)|*.xml";

			// Show open file dialog box
			Nullable<bool> result = saveFileDialog.ShowDialog();
			if (result == true)
				this.generatorInfo.FileName = saveFileDialog.FileName;

		}

		void OnExitButtonClick(Object sender, RoutedEventArgs e)
		{

			this.Close();

		}

		void OnGenerateButtonClick(Object sender, RoutedEventArgs e)
		{

			// Save the directory used for the output for the next invocation.
			WorkingOrderGenerator.Properties.Settings.Default.OutputDirectory = Path.GetDirectoryName(this.generatorInfo.FileName);

			Generator generator = new Generator(this.generatorInfo);
			generator.Create();

		}

	}

}
