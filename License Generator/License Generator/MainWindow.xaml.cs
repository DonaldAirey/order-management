namespace Teraque.LicenseGenerator
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Windows.Data;
	using System.IO;
	using System.Linq;
	using System.Windows.Input;
	using System.Windows;
	using System.Windows.Controls;
	using System.Security.Cryptography;
	using Teraque;
	using Teraque.Windows;
	using Teraque.Windows.Navigation;
	using Teraque.LicenseGenerator.LicensePage;

	/// <summary>
	/// Creates and maintains a database of license keys.
	/// </summary>
	/// <copyright>Copyright © 2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class MainWindow : ExplorerWindow
	{

		static Guid ExplorerChromeSuite = new Guid("{9FD4FF74-CCAC-463E-922E-79613F6A7CD9}");

		static Guid DataTierGenerator = new Guid("{AA65499B-EFEA-4989-800E-E0C29B5A279B}");

		static Guid DataPresentationSuite = new Guid("{B610D1FB-CA7D-45C1-99ED-C655E4E2BC3A}");

		static Guid TeraqueCustomerId = new Guid("{51023150-61E9-4D46-AC49-053BD420D8C0}");

		/// <summary>
		/// Main Window of the License Generator application.
		/// </summary>
		public MainWindow()
		{

			// The IDE generated controls are initialized here.
			this.InitializeComponent();

			// The immutable command bindings are found here.
			this.CommandBindings.Add(new CommandBinding(LicenseCommand.GenerateLicense, this.OnGenerateLicense));

			this.GenerateSeedData();

			ObservableCollection<RootCollection> root = new ObservableCollection<RootCollection>();
			root.Add(new RootCollection());
			this.DataContext = root;

			Binding itemsSourceBinding = new Binding();
			itemsSourceBinding.Source = this.DataContext;
			BindingOperations.SetBinding(this, MainWindow.ItemsSourceProperty, itemsSourceBinding);

			// This will have the effect of setting the current path to the root of the given hierarchy.
			if (this.Items.Count != 0)
			{
				IExplorerItem iExplorerItem = this.Items[0] as IExplorerItem;
				if (iExplorerItem != null)
					this.Source = ExplorerHelper.GenerateSource(iExplorerItem);
			}

		}

		void DisplayCommandLineArguments()
		{

			Window window = new Window();
			TextBox textBox = new TextBox();
			textBox.TextWrapping = TextWrapping.Wrap;
			textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
			textBox.Text = Environment.CommandLine;
			window.Content = textBox;
			window.ShowDialog();

		}
	
		void GenerateKey()
		{

			RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider();
			String privateKey = rsaCryptoServiceProvider.ToXmlString(true);
			String publicKey = rsaCryptoServiceProvider.ToXmlString(false);
			RSAParameters rsaParameters = rsaCryptoServiceProvider.ExportParameters(true);
			Teraque.Utilities.EmitByteArray("privateKeyD", rsaParameters.D);
			Teraque.Utilities.EmitByteArray("privateKeyDp", rsaParameters.DP);
			Teraque.Utilities.EmitByteArray("privateKeyDq", rsaParameters.DQ);
			Teraque.Utilities.EmitByteArray("publicKeyExponent", rsaParameters.Exponent);
			Teraque.Utilities.EmitByteArray("privateKeyInverseQ", rsaParameters.InverseQ);
			Teraque.Utilities.EmitByteArray("publicKeyModulus", rsaParameters.Modulus);
			Teraque.Utilities.EmitByteArray("privateKeyP", rsaParameters.P);
			Teraque.Utilities.EmitByteArray("privateKeyQ", rsaParameters.Q);

		}

		void GenerateSeedData()
		{

			// Predefined Products
			if (DataModel.Product.FindByProductId(MainWindow.ExplorerChromeSuite) == null)
				DataModel.Product.AddProductRow(DateTime.Now, DateTime.Now, "Suite of Libraries and Components for Building Explorer Chrome Applications", "Explorer Chrome Suite", MainWindow.ExplorerChromeSuite);
			if (DataModel.Product.FindByProductId(MainWindow.DataTierGenerator) == null)
				DataModel.Product.AddProductRow(DateTime.Now, DateTime.Now, "A distributed, industrial strength, multi-user database from a schema.", "Data Tier Generator", MainWindow.DataTierGenerator);
			if (DataModel.Product.FindByProductId(MainWindow.DataPresentationSuite) == null)
				DataModel.Product.AddProductRow(DateTime.Now, DateTime.Now, "An MVVM design for presentation of large, complex data sets.", "Data Presentation Suite", MainWindow.DataPresentationSuite);
			DataModel.ProductTableAdapter.Update(DataModel.Product);

			// Predefined Customers
			if (DataModel.Customer.FindByCustomerId(MainWindow.TeraqueCustomerId) == null)
				DataModel.Customer.AddCustomerRow(
					"185 Wilder Road",
					"Bolton",
					"US",
					MainWindow.TeraqueCustomerId,
					DateTime.Now,
					DateTime.Now,
					"Donald.Roy.Airey@teraque.com",
					"Donald",
					"Airey",
					"(617) 963-8996",
					"01740",
					"MA");
			DataModel.CustomerTableAdapter.Update(DataModel.Customer);

		}

		void OnGenerateLicense(Object sender, RoutedEventArgs e)
		{

			Byte[] licenseTypeArray = new Byte[2];
			licenseTypeArray[0] = LicenseType.Evaluation3Month;
			licenseTypeArray[1] = LicenseType.Evaluation3Month;
			Int16 licenseType = BitConverter.ToInt16(licenseTypeArray, 0);

			DateTime dateCreated = DateTime.Now;

			LicenseInfo licenseInfo = new LicenseInfo()
			{
				DateCreated = dateCreated,
				CustomerId = MainWindow.TeraqueCustomerId,
				LicenseType = licenseType,
				ProductId = MainWindow.ExplorerChromeSuite
			};

			LicenseManager.GenerateLicense(licenseInfo, "../../license.lic");

		}

	}

}
