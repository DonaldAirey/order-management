namespace Teraque
{

	using System;

	/// <summary>
	/// The global unique identifier for various properties in the data model.
	/// </summary>
	public static class PropertyId
	{

		/// <summary>
		/// The object data.
		/// </summary>
		public readonly static Guid Data = new Guid("{4A87EEC0-4FE7-42D4-8E71-D4E9FD14E3AF}");

		/// <summary>
		/// The date the record was created.
		/// </summary>
		public readonly static Guid DateCreated = new Guid("{95D033C1-29B0-4C6B-AD3A-9ECC85F305AD}");

		/// <summary>
		/// The date the record was deleted.
		/// </summary>
		public readonly static Guid DateModified = new Guid("{AD5B422F-2BDB-4F12-9D48-419E27F4A047}");

		/// <summary>
		/// The size of the record.
		/// </summary>
		public readonly static Guid Size = new Guid("{6BB2E00E-76B4-4F15-9B93-530D6FAD0DBF}");

		/// <summary>
		/// The viewer used to display the record.
		/// </summary>
		public readonly static Guid Viewer = new Guid("{8AE00186-7AD0-4621-9ED6-29B4D5C20880}");

	}
}
