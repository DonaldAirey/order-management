namespace Teraque
{

	using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
	using System.Reflection;
	using System.Windows;
	using System.Windows.Resources;

	/// <summary>
	/// A shared data model.
	/// </summary>
	/// <copyright>Copyright © 2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	public static class DataModel
	{

		/// <summary>
		/// This data set contains the strongly typed data used for the sample database.
		/// </summary>
		static DataSet dataSet = new DataSet();

		/// <summary>
		/// Initializes the DataModel class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static DataModel()
		{

			// Load the sample data used to qualify the Explorer Window shell.
			AssemblyName assemblyName = new AssemblyName(Assembly.GetAssembly(typeof(DataModel)).FullName);
			String packUri = String.Format(CultureInfo.InvariantCulture, "pack://application:,,,/{0};component/Sample Data.xml", assemblyName.Name);
			DataModel.dataSet.ReadXml(Application.GetResourceStream(new Uri(packUri)).Stream);

		}

		/// <summary>
		/// Gets access to the data in the Teraque.DataModel.
		/// </summary>
		public static DataSet DataSet
		{
			get { return DataModel.dataSet; }
		}

	}

}
