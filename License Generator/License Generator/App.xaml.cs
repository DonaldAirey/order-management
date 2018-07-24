namespace Teraque.LicenseGenerator
{

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;
	using Teraque;

	/// <summary>
	/// Creates and manages cryptographically signed licenses.
	/// </summary>
	/// <copyright>Copyright © 2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class App : Application
	{

		/// <summary>
		/// Command line parameter tokens.
		/// </summary>
		enum Parameter { Address, City, Command, Country, Email, FirstName, LastName, None, Output, Phone, Product, Province, PostalCode };

		/// <summary>
		/// A dictionary of the command line parameter/value pairs.
		/// </summary>
		Dictionary<Parameter, String> commandLine;

		/// <summary>
		/// This map is used to turn the command line parameters into tokens.
		/// </summary>
		static Dictionary<String, Parameter> parameterMap = new Dictionary<String, Parameter>()
		{
			{"address", Parameter.Address},
			{"city", Parameter.City},
			{"command", Parameter.Command},
			{"country", Parameter.Country},
			{"email", Parameter.Email},
			{"firstname", Parameter.FirstName},
			{"lastname", Parameter.LastName},
			{"output", Parameter.Output},
			{"phone", Parameter.Phone},
			{"product", Parameter.Product},
			{"province", Parameter.Province},
			{"postalcode", Parameter.PostalCode}
		};

		/// <summary>
		/// Creates a new instance of the App class.
		/// </summary>
		public App()
		{

			// This will break down the command line into a series of tokens and their values.
			this.commandLine = new Dictionary<Parameter, String>();
			Parameter parameter = Parameter.None;
			foreach (String commandLineArgument in Environment.GetCommandLineArgs())
				if (commandLineArgument[0] == '-')
					App.parameterMap.TryGetValue(commandLineArgument.ToLower().Substring(1), out parameter);
				else
					this.commandLine[parameter] = commandLineArgument;

			// This will execute any commands that were found on the command line.
			String commandText;
			if (this.commandLine.TryGetValue(Parameter.Command, out commandText))
			{

				try
				{

					// Execute the command and make sure that any exceptions are caught and the application can control how it is shutdown.
					switch (commandText)
					{

					case "GenerateLicense":

						// This will generate a new license.
//						this.GenerateLicense();
						break;

					}

					// Commands execute and then exit and don't bring up the user interface.
//					Application.Current.Shutdown(0);

				}
				catch (Exception exception)
				{

					// This will catch any errors executing the command and return a generic error code to the caller.
					Log.Error(exception.Message);
					Application.Current.Shutdown(1);

				}

			}

		}

		/// <summary>
		/// Generate a license from the command line arguments.
		/// </summary>
		void GenerateLicense()
		{

			// A license requires the product.  This will extract the product id from the command line and attempt to find it in the table of products.
			Guid productId = Guid.Parse(this.commandLine[Parameter.Product]);
			DataSet.ProductRow productRow = DataModel.Product.FindByProductId(productId);

			// The email address is used as a unique identifier for the customer.
			String email = this.commandLine[Parameter.Email];
			Guid customerId = Guid.Empty;
			var results = from row in DataModel.Customer.AsEnumerable() where row.Email == email select row;
			DataSet.CustomerRow customerRow = results.FirstOrDefault();

			// If the customer doesn't exist yet, then create one from the command line parameters.
			if (customerRow == null)
			{

				// This will generate a new record from the command line arguments which likely have been provided by a daemon process in Outlook.
				customerRow = DataModel.Customer.NewCustomerRow();
				customerRow.Address = this.commandLine[Parameter.Address];
				customerRow.City = this.commandLine[Parameter.City];
				customerRow.Country = this.commandLine[Parameter.Country];
				customerRow.CustomerId = Guid.NewGuid();
				customerRow.DateCreated = DateTime.Now;
				customerRow.DateModified = DateTime.Now;
				customerRow.Email = email;
				customerRow.FirstName = this.commandLine[Parameter.FirstName];
				customerRow.LastName = this.commandLine[Parameter.LastName];
				customerRow.Phone = this.commandLine[Parameter.Phone];
				customerRow.PostalCode = this.commandLine[Parameter.PostalCode];
				customerRow.Province = this.commandLine[Parameter.Province];

				// Add the customer and update the database.
				DataModel.Customer.AddCustomerRow(customerRow);
				DataModel.CustomerTableAdapter.Update(DataModel.Customer);

			}

			// Create a perpetual license for development and runtime.
			Byte[] licenseTypeArray = new Byte[2];
			licenseTypeArray[0] = LicenseType.Perpetual;
			licenseTypeArray[1] = LicenseType.Perpetual;
			Int16 licenseType = BitConverter.ToInt16(licenseTypeArray, 0);

			// The effective date of this license is right now.
			DateTime dateCreated = DateTime.Now;

			// This will generate the license from the command line arguments parsed above and deposit it in a file also parsed from the command line.
			LicenseInfo licenseInfo = new LicenseInfo()
			{
				DateCreated = dateCreated,
				CustomerId = customerRow.CustomerId,
				LicenseType = licenseType,
				ProductId = productRow.ProductId
			};
			LicenseManager.GenerateLicense(licenseInfo, this.commandLine[Parameter.Output]);

		}

	}

}
