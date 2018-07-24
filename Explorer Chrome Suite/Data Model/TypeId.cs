namespace Teraque
{

	using System;
    using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// The identifier for the type of databae.
	/// </summary>
	public static class TypeId
	{

		/// <summary>
		/// A Directory-like folder.
		/// </summary>
		public readonly static Guid FileFolder = new Guid("{CF75D917-1284-4FAB-BFCF-685546F0481B}");

		/// <summary>
		/// A Spreadsheet.
		/// </summary>
		public readonly static Guid Spreadsheet = new Guid("{94A31E20-47D2-4F9E-8D39-20396E3058E1}");

		/// <summary>
		/// A Web Page object.
		/// </summary>
		public readonly static Guid Webpage = new Guid("{E6C6A93E-092F-4B56-B446-36DC1F93B3F3}");

	}

}
