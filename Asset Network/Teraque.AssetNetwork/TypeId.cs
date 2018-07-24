namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// Known Asset Network object types.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public static class TypeId
	{

		/// <summary>
		/// A single account.
		/// </summary>
		public static readonly Guid Account = new Guid("{8B51759B-A5BC-4BB4-BB6E-2859F3B010C0}");

		/// <summary>
		/// A group of accounts.
		/// </summary>
		public static readonly Guid AccountGroup = new Guid("{79091B2A-36DD-4C6A-9AAF-C88BD3262E6E}");

		/// <summary>
		/// Someone who analyzes data.
		/// </summary>
		public static readonly Guid Analyst = new Guid("{39F5B666-7EC6-4FA0-937C-1057CCC34BEC}");

		/// <summary>
		/// A collection of orders.
		/// </summary>
		public static readonly Guid Blotter = new Guid("{E8145636-FC40-4E63-90EF-31EC752001B0}");

		/// <summary>
		/// An agent for exchanging securities.
		/// </summary>
		public static readonly Guid Broker = new Guid("{E8145636-FC40-4E63-90EF-31EC752001B0}");

		/// <summary>
		/// Someone who enforces compliance with prospectuses and regulations.
		/// </summary>
		public static readonly Guid ComplianceOfficer = new Guid("{635FAB27-DBF3-437F-A39E-2B0B0666D6E5}");

		/// <summary>
		/// A collection of fixed income orders.
		/// </summary>
		public static readonly Guid DebtBlotter = new Guid("{D1286CB8-D5D3-4239-98DD-42E572873F28}");

		/// <summary>
		/// A contract between two parties that specifies conditions under which payments are to be made between the parties.
		/// </summary>
		public static readonly Guid Derivative = new Guid("{3A5FDCDD-71B5-43D5-8FCC-A67FCFD29E2C}");

		/// <summary>
		/// Represents ownership of a company.
		/// </summary>
		public static readonly Guid Equity = new Guid("{A4AE2EDD-F347-4E87-84BA-D8FD2E7F942D}");

		/// <summary>
		/// A collection of Equity orders.
		/// </summary>
		public static readonly Guid EquityBlotter = new Guid("{30C8C11A-679D-4F7E-8210-9539645C5062}");

		/// <summary>
		/// Microsoft Excel Worksheet.
		/// </summary>
		public static readonly Guid ExcelWorksheet = new Guid("{8898C4E3-784C-43FA-B28C-A64EAA4FE4A2}");

		/// <summary>
		/// File folder.
		/// </summary>
		public static readonly Guid FileFolder = new Guid("{D6ED0291-1D36-453E-BB6C-4E87B39BB783}");

		/// <summary>
		/// An investment vehicle employing short selling and leverage.
		/// </summary>
		public static readonly Guid HedgeFund = new Guid("{C142199E-857D-4B17-89E8-F3B10672564D}");

		/// <summary>
		/// An entity that provides financial services for its clients or members.
		/// </summary>
		public static readonly Guid MoneyManager = new Guid("{B090571B-A14D-4CC3-A94E-9BFFC436AB5D}");

		/// <summary>
		/// A fungible, negotiable financial instrument representing financial value.
		/// </summary>
		public static readonly Guid Security = new Guid("{A922878F-D0C1-4DCF-B532-B7FBDA2058AC}");

		/// <summary>
		/// Adobe Acrobat Document
		/// </summary>
		public static readonly Guid PdfDocument = new Guid("{5985F915-5494-4D60-8AF9-EF50C69AAC97}");

		/// <summary>
		/// A person who exchanges one security for another.
		/// </summary>
		public static readonly Guid Trader = new Guid("{A922878F-D0C1-4DCF-B532-B7FBDA2058AC}");

		/// <summary>
		/// A user.
		/// </summary>
		public static readonly Guid User = new Guid("{38502AB2-EFB0-4111-8842-7968B62EE02A}");

	}

}
