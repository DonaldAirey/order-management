namespace Teraque.ExplorerChromeExample
{

	using System;
	using System.Windows;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Animation;
	using Teraque.Windows.Controls;
	using Teraque.Windows.Documents;
	using Teraque.Windows.Navigation;

	/// <summary>
	/// The main window of the sandbox environment for the ExplorerWindow.
	/// </summary>
	/// <copyright>Copyright © 2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class MainWindow : ExplorerWindow
	{

		/// <summary>
		/// The size of the font used in the embossed watermark.
		/// </summary>
		const Double fontSize = 220.0;

		/// <summary>
		/// Initialize a new instance of the MainWindow class.
		/// </summary>
		public MainWindow()
		{

			// The IDE maintained resources are initialized here.
			this.InitializeComponent();

		}

		/// <summary>
		/// Called when the template generation for the visual tree is created.
		/// </summary>
		public override void OnApplyTemplate()
		{

			// The basic idea here is to place a watermark over the entire client area when an developer evaluation license is present.  This is not meant as a
			// security measure as anyone can remove this code.  The watermark, or rather the absense of a watermark, is an indicator to the developer that they've
			// installed the perpetual license correctly.  The first step is to call the base class so we have a ContentPresenter part on which we can apply an
			// adornment (in the form of a transparent watermark).
			base.OnApplyTemplate();

			// If the developer license is for evaluation then we want to put up the watermark as a reminder that any code produced with this license will expire
			// sometime in the near future.
			Byte[] licenseTypes = BitConverter.GetBytes(this.License.LicenseType);
			switch (licenseTypes[1])
			{
			case LicenseType.EvaluationNag:
			case LicenseType.Evaluation1Month:
			case LicenseType.Evaluation2Month:
			case LicenseType.Evaluation3Month:
			case LicenseType.Evaluation4Month:
			case LicenseType.Evaluation5Month:
			case LicenseType.Evaluation6Month:

				// This will apply an adorner over the area where the content is presented (basically the entire client area) that indicates the developer is using 
				// an evaluation license.
				AdornerLayer myAdornerLayer = AdornerLayer.GetAdornerLayer(this.ContentPresenter);
				myAdornerLayer.Add(
					new WatermarkAdorner(this.ContentPresenter)
					{
						Text = Properties.Resources.Evaluation,
						FontSize = MainWindow.fontSize
					});
				break;

			}

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

		/// <summary>
		/// Displays information about the application and the license.
		/// </summary>
		/// <param name="sender">The object that sent the command.</param>
		/// <param name="e">The command arguments.</param>
		void OnAbout(object sender, ExecutedRoutedEventArgs e)
		{

			// This will display the About box with the license information in it.
			WindowAbout windowAbout = new WindowAbout()
			{
				LicenseType = this.License.LicenseType,
				CreationTime = this.License.CreationTime,
				Version = typeof(ExplorerWindow).Assembly.GetName().Version
			};
			windowAbout.ShowDialog();

		}

	}

}
