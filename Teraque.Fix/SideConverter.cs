namespace Teraque
{

    using System.Collections;

	/// <summary>
	/// Type Converter for FIX Side Field
	/// </summary>
	public class SideConverter
	{

		// Private Members
		private static Hashtable fromTable;
		private static Hashtable toTable;
		private static object[,] pairs =
		{
			{SideCode.Buy, "1"},
			{SideCode.Sell, "2"},
			{SideCode.BuyMinus, "3"},
			{SideCode.SellPlus, "4"},
			{SideCode.SellShort, "5"},
			{SideCode.SellShortExempt, "6"},
			{SideCode.Undisclosed, "7"},
			{SideCode.Cross, "8"},
			{SideCode.CrossShort, "9"},
			{SideCode.CrossShortExempt, "A"},
			{SideCode.AsDefined, "B"},
			{SideCode.Opposite, "C"},
			{SideCode.Subscribe, "D"},
			{SideCode.Redeem, "E"},
			{SideCode.Lend, "F"},
			{SideCode.Borrow, "G"}
		};

		/// <summary>
		/// Initializes the shared members of a SideConverter.
		/// </summary>
		static SideConverter()
		{

			// Initialize the mapping of strings to Side.
			SideConverter.fromTable = new Hashtable();
			for (int element = 0; element < pairs.GetLength(0); element++)
				SideConverter.fromTable.Add(pairs[element, 1], pairs[element, 0]);

			// Initialize the mapping of Side to strings.
			SideConverter.toTable = new Hashtable();
			for (int element = 0; element < pairs.GetLength(0); element++)
				SideConverter.toTable.Add(pairs[element, 0], pairs[element, 1]);

		}

		/// <summary>
		/// Converts a string to a Side.
		/// </summary>
		/// <param name="value">The FIX string representation of a Side.</param>
		/// <returns>A Side value.</returns>
		public static SideCode ConvertFrom(string value) {return (SideCode)SideConverter.fromTable[value];}

		/// <summary>
		/// Converts a Side to a string.
		/// </summary>
		/// <returns>A Side value.</returns>
		/// <param name="value">The FIX string representation of a Side.</param>
		public static string ConvertTo(SideCode messageType) {return (string)SideConverter.toTable[messageType];}

	}

}
