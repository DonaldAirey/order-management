namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Input;
	using Teraque;

	/// <summary>
	/// Provides information about the application.
	/// </summary>
	public partial class WindowAbout : Window
	{

		/// <summary>
		/// Identifies the CreationTime dependency property.
		/// </summary>
		public static readonly DependencyProperty CreationTimeProperty = DependencyProperty.Register(
			"CreationTime",
			typeof(DateTime),
			typeof(WindowAbout),
			new FrameworkPropertyMetadata(DateTime.MinValue, WindowAbout.OnCreationTimePropertyChanged));

		/// <summary>
		/// Identifies the DaysLeft dependency property.
		/// </summary>
		public static readonly DependencyProperty DaysLeftProperty;

		/// <summary>
		/// Identifies the DaysLeft dependency property.key.
		/// </summary>
		static DependencyPropertyKey daysLeftPropertyKey = DependencyProperty.RegisterReadOnly(
			"DaysLeft",
			typeof(Double),
			typeof(WindowAbout),
			new FrameworkPropertyMetadata());

		/// <summary>
		/// Identifies the IsEvaluationLicense dependency property.
		/// </summary>
		public static readonly DependencyProperty IsEvaluationLicenseProperty;

		/// <summary>
		/// Identifies the IsEvaluationLicense dependency property.key.
		/// </summary>
		static DependencyPropertyKey isEvaluationLicensePropertyKey = DependencyProperty.RegisterReadOnly(
			"IsEvaluationLicense",
			typeof(Boolean),
			typeof(WindowAbout),
			new FrameworkPropertyMetadata());

		/// <summary>
		/// Identifies the IsFullLicense dependency property.
		/// </summary>
		public static readonly DependencyProperty IsFullLicenseProperty;

		/// <summary>
		/// Identifies the IsFullLicense dependency property.key.
		/// </summary>
		static DependencyPropertyKey isFullLicensePropertyKey = DependencyProperty.RegisterReadOnly(
			"IsFullLicense",
			typeof(Boolean),
			typeof(WindowAbout),
			new FrameworkPropertyMetadata());

		/// <summary>
		/// Identifies the IsPerpetualLicense dependency property.
		/// </summary>
		public static readonly DependencyProperty IsPerpetualLicenseProperty;

		/// <summary>
		/// Identifies the IsPerpetualLicense dependency property.key.
		/// </summary>
		static DependencyPropertyKey isPerpetualLicensePropertyKey = DependencyProperty.RegisterReadOnly(
			"IsPerpetualLicense",
			typeof(Boolean),
			typeof(WindowAbout),
			new FrameworkPropertyMetadata());

		/// <summary>
		/// This maps determines the amount of time allowed for a time dependent licenses.
		/// </summary>
		static Dictionary<Int16, Double> licensePeriod = new Dictionary<Int16, Double>()
		{
			{Teraque.LicenseType.Evaluation1Month, 30.0},
			{Teraque.LicenseType.Evaluation2Month, 60.0},
			{Teraque.LicenseType.Evaluation3Month, 90.0},
			{Teraque.LicenseType.Evaluation4Month, 120.0},
			{Teraque.LicenseType.Evaluation5Month, 150.0},
			{Teraque.LicenseType.Evaluation6Month, 180.0},
			{Teraque.LicenseType.Full1Year, 365.0},
			{Teraque.LicenseType.Full2Year, 730.0},
			{Teraque.LicenseType.Full3Year, 1095.0},
			{Teraque.LicenseType.Full4Year, 1460.0},
			{Teraque.LicenseType.Full5Year, 1825.0}
		};

		/// <summary>
		/// Identifies the LicenseType dependency property.
		/// </summary>
		public static readonly DependencyProperty LicenseTypeProperty = DependencyProperty.Register(
			"LicenseType",
			typeof(Int16),
			typeof(WindowAbout),
			new FrameworkPropertyMetadata((Int16)0, WindowAbout.OnLicenseTypePropertyChanged));

		/// <summary>
		/// Email Support.
		/// </summary>
		public static readonly RoutedCommand MailtoSupport = new RoutedCommand("MailtoSupport", typeof(WindowAbout));

		/// <summary>
		/// Navigate to the support home page.
		/// </summary>
		public static readonly RoutedCommand NavigateToSupport = new RoutedCommand("NavigateToSupport", typeof(WindowAbout));

		/// <summary>
		/// Purchases a license.
		/// </summary>
		public static readonly RoutedCommand PurchaseLicense = new RoutedCommand("PurchaseLicense", typeof(WindowAbout));
		
		/// <summary>
		/// Identifies the Version dependency property.
		/// </summary>
		public static readonly DependencyProperty VersionProperty = DependencyProperty.Register(
			"Version",
			typeof(Version),
			typeof(WindowAbout));

		/// <summary>
		/// Initializes the WindowAbout class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static WindowAbout()
		{

			// These properties must be initialized here to avoid forward reference problems that can occur if the fields are moved around.
			WindowAbout.DaysLeftProperty = WindowAbout.daysLeftPropertyKey.DependencyProperty;
			WindowAbout.IsEvaluationLicenseProperty = WindowAbout.isEvaluationLicensePropertyKey.DependencyProperty;
			WindowAbout.IsFullLicenseProperty = WindowAbout.isFullLicensePropertyKey.DependencyProperty;
			WindowAbout.IsPerpetualLicenseProperty = WindowAbout.isPerpetualLicensePropertyKey.DependencyProperty;

		}

		/// <summary>
		/// Initializes a new instance of the WindowAbout class.
		/// </summary>
		public WindowAbout()
		{

			// The IDE maintained components are initialized here.
			InitializeComponent();

		}

		/// <summary>
		/// Gets or sets whether the application is using an evaluation license.
		/// </summary>
		public DateTime CreationTime
		{
			get
			{
				return (DateTime)this.GetValue(WindowAbout.CreationTimeProperty);
			}
			set
			{
				this.SetValue(WindowAbout.CreationTimeProperty, value);
			}
		}

		/// <summary>
		/// Gets the number of days left in the license.
		/// </summary>
		public Double DaysLeft
		{
			get
			{
				return (Double)this.GetValue(WindowAbout.DaysLeftProperty);
			}
		}

		/// <summary>
		/// Gets an indication of whether the application is using an evaluation license or not.
		/// </summary>
		public Boolean IsEvaluationLicense
		{
			get
			{
				return (Boolean)this.GetValue(WindowAbout.IsEvaluationLicenseProperty);
			}
		}

		/// <summary>
		/// Gets an indication of whether the application is using a full license or not.
		/// </summary>
		public Boolean IsFullLicense
		{
			get
			{
				return (Boolean)this.GetValue(WindowAbout.IsFullLicenseProperty);
			}
		}

		/// <summary>
		/// Gets an indication of whether the application is using a perpetual license or not.
		/// </summary>
		public Boolean IsPerpetualLicense
		{
			get
			{
				return (Boolean)this.GetValue(WindowAbout.IsPerpetualLicenseProperty);
			}
		}

		/// <summary>
		/// Gets or sets whether the application is using an evaluation license.
		/// </summary>
		public Int16 LicenseType
		{
			get
			{
				return (Int16)this.GetValue(WindowAbout.LicenseTypeProperty);
			}
			set
			{
				this.SetValue(WindowAbout.LicenseTypeProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets whether the application is using a full license.
		/// </summary>
		public Version Version
		{
			get
			{
				return (Version)this.GetValue(WindowAbout.VersionProperty);
			}
			set
			{
				this.SetValue(WindowAbout.VersionProperty, value);
			}
		}

		/// <summary>
		/// Accepts the results of the dialog.
		/// </summary>
		/// <param name="sender">The object that issued the routed event.</param>
		/// <param name="e">The routed event arguments.</param>
		void OnDialogAccept(Object sender, ExecutedRoutedEventArgs e)
		{

			// This indicates the user accepted the dialog and dismisses the window.
			this.DialogResult = true;
			this.Close();

		}

		/// <summary>
		/// Close the dialog.
		/// </summary>
		/// <param name="sender">The object that issued the routed event.</param>
		/// <param name="e">The routed event arguments.</param>
		void OnMailtoSupport(Object sender, ExecutedRoutedEventArgs e)
		{

			// This will start an email message to support at Teraque.
			Process.Start(Explorer.Properties.Resources.AboutMailUrl);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender">The object that issued the routed event.</param>
		/// <param name="e">The routed event arguments.</param>
		void OnNavigateToSupport(Object sender, ExecutedRoutedEventArgs e)
		{

			// This will launch the browser and navigate to Teraque support.  In the near future we should be able to do this withing the example application to
			// demonstrate the power of a hybrid application frame.
			Process.Start(Explorer.Properties.Resources.AboutSupportUrl);

		}

		/// <summary>
		/// Sends the user to the Teraque web site where they can purchase a valid license.
		/// </summary>
		/// <param name="sender">The object that issued the routed event.</param>
		/// <param name="e">The routed event arguments.</param>
		void OnPurchaseLicense(Object sender, ExecutedRoutedEventArgs e)
		{

			// This will launch the browser and navigate to the Teraque web page where the user can purchase a license.
			Process.Start(Explorer.Properties.Resources.AboutPurchaseLicenseUrl);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dependencyObject"></param>
		/// <param name="dependencyPropertyChangedEventArgs"></param>
		static void OnCreationTimePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			WindowAbout windowAbout = dependencyObject as WindowAbout;
			DateTime creationTime = (DateTime)dependencyPropertyChangedEventArgs.NewValue;
			windowAbout.EvaluateLicense(creationTime, windowAbout.LicenseType);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dependencyObject"></param>
		/// <param name="dependencyPropertyChangedEventArgs"></param>
		static void OnLicenseTypePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			WindowAbout windowAbout = dependencyObject as WindowAbout;
			Int16 licenseType = (Int16)dependencyPropertyChangedEventArgs.NewValue;
			windowAbout.EvaluateLicense(windowAbout.CreationTime, licenseType);

		}

		/// <summary>
		/// Evaluates the license properties based on the given license type and license creation time.
		/// </summary>
		/// <param name="creationTime">The creation time of the license.</param>
		/// <param name="licenseType">The license type.</param>
		void EvaluateLicense(DateTime creationTime, Int16 licenseType)
		{

			// The license type is a free format value that can be re-defined by a subclass.  The default format contains two bytes, the first is the runtime
			// license type, the second byte is the design time license type.  For this 'About' box we will display the status of the design time license as this is
			// most important to our target audience.
			Byte[] licenseTypes = BitConverter.GetBytes(licenseType);
			Byte designTimeLicenseType = licenseTypes[1];
			switch (designTimeLicenseType)
			{

			case Teraque.LicenseType.Evaluation1Month:
			case Teraque.LicenseType.Evaluation2Month:
			case Teraque.LicenseType.Evaluation3Month:
			case Teraque.LicenseType.Evaluation4Month:
			case Teraque.LicenseType.Evaluation5Month:
			case Teraque.LicenseType.Evaluation6Month:

				// Configure the MVVM properties to display the information for an Evaluation License.
				this.SetValue(WindowAbout.isEvaluationLicensePropertyKey, true);
				this.SetValue(WindowAbout.isFullLicensePropertyKey, false);
				this.SetValue(WindowAbout.isPerpetualLicensePropertyKey, false);
				this.SetValue(WindowAbout.daysLeftPropertyKey, WindowAbout.licensePeriod[designTimeLicenseType] - DateTime.Now.Subtract(creationTime).Days);
				break;

			case Teraque.LicenseType.Full1Year:
			case Teraque.LicenseType.Full2Year:
			case Teraque.LicenseType.Full3Year:
			case Teraque.LicenseType.Full4Year:
			case Teraque.LicenseType.Full5Year:

				// Configure the MVVM properties to display the information for a Full License.
				this.SetValue(WindowAbout.isEvaluationLicensePropertyKey, false);
				this.SetValue(WindowAbout.isFullLicensePropertyKey, true);
				this.SetValue(WindowAbout.isPerpetualLicensePropertyKey, false);
				this.SetValue(WindowAbout.daysLeftPropertyKey, WindowAbout.licensePeriod[designTimeLicenseType] - DateTime.Now.Subtract(creationTime).Days);
				break;

			case Teraque.LicenseType.Perpetual:

				// Configure the MVVM properties to display the information for a Perpetual License.
				this.SetValue(WindowAbout.isEvaluationLicensePropertyKey, false);
				this.SetValue(WindowAbout.isFullLicensePropertyKey, false);
				this.SetValue(WindowAbout.isPerpetualLicensePropertyKey, true);
				break;

			}

		}

	}

}
