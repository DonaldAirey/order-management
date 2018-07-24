namespace Teraque.LicenseGenerator
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.IO;
	using System.Resources;
	using System.Windows;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using Teraque;
	using Teraque.LicenseGenerator.DataSetTableAdapters;

	public sealed class DataModel
	{

		/// <summary>
		/// DataAdapter for the Customer table.
		/// </summary>
		static CustomerTableAdapter customerTableAdapter;

		static DataSet dataSet;

		/// <summary>
		/// DataAdapter for the Product table.
		/// </summary>
		static LicenseTableAdapter licenseTableAdapter;

		/// <summary>
		/// DataAdapter for the Product table.
		/// </summary>
		static ProductTableAdapter productTableAdapter;

		static DataModel()
		{

			// Create the data model and the table adapters.
			DataModel.dataSet = new DataSet();
			DataModel.customerTableAdapter = new CustomerTableAdapter();
			DataModel.licenseTableAdapter = new LicenseTableAdapter();
			DataModel.productTableAdapter = new ProductTableAdapter();

			// Load in the data from the SQL Server.
			DataModel.productTableAdapter.Fill(DataModel.dataSet.Product);
			DataModel.customerTableAdapter.Fill(DataModel.dataSet.Customer);
			DataModel.licenseTableAdapter.Fill(DataModel.dataSet.License);

		}

		public static DataSet.CustomerDataTable Customer
		{
			get
			{
				return DataModel.dataSet.Customer;
			}
		}

		public static DataSet.ProductDataTable Product
		{
			get
			{
				return DataModel.dataSet.Product;
			}
		}

		public static DataSet.LicenseDataTable License
		{
			get
			{
				return DataModel.dataSet.License;
			}
		}

		public static DataSetTableAdapters.CustomerTableAdapter CustomerTableAdapter
		{
			get
			{
				return DataModel.customerTableAdapter;
			}
		}

		public static DataSetTableAdapters.LicenseTableAdapter LicenseTableAdapter
		{
			get
			{
				return DataModel.licenseTableAdapter;
			}
		}

		public static ProductTableAdapter ProductTableAdapter
		{
			get
			{
				return DataModel.productTableAdapter;
			}
		}

	}

}