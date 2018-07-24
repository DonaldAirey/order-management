namespace Teraque
{

    using System.Collections;

	/// <summary>
	/// Type Converter for FIX OrdType Field
	/// </summary>
	public class OrdTypeConverter
	{

		// Private Members
		private static Hashtable fromTable;
		private static Hashtable toTable;
		private static object[,] pairs =
		{
			{OrderTypeCode.Market, "1"},
			{OrderTypeCode.Limit, "2"},
			{OrderTypeCode.Stop, "3"},
			{OrderTypeCode.StopLimit, "4"},
			{OrderTypeCode.MarketOnClose, "5"},
			{OrderTypeCode.WithOrWithout, "6"},
			{OrderTypeCode.LimitOrBetter, "7"},
			{OrderTypeCode.LimitWithOrWithout, "8"},
			{OrderTypeCode.OnBasis, "9"},
			{OrderTypeCode.OnClose, "A"},
			{OrderTypeCode.LimitOnClose, "B"},
			{OrderTypeCode.PreviouslyQuoted, "D"},
			{OrderTypeCode.PreviouslyIndicated, "E"},
			{OrderTypeCode.ForexLimit, "F"},
			{OrderTypeCode.ForexSwap, "G"},
			{OrderTypeCode.ForexPreviouslyIndicated, "H"},
			{OrderTypeCode.Funarii, "I"},
			{OrderTypeCode.MarketIfTouched, "J"},
			{OrderTypeCode.MarketWithLeftoverAsLimit, "K"},
			{OrderTypeCode.PreviousFundValuationPoint, "L"},
			{OrderTypeCode.NextFundValuationPoint, "M"},
			{OrderTypeCode.Pegged, "P"},
		};

		/// <summary>
		/// Initializes the shared members of a OrdTypeConverter.
		/// </summary>
		static OrdTypeConverter()
		{

			// Initialize the mapping of strings to OrdType.
			OrdTypeConverter.fromTable = new Hashtable();
			for (int element = 0; element < pairs.GetLength(0); element++)
				OrdTypeConverter.fromTable.Add(pairs[element, 1], pairs[element, 0]);

			// Initialize the mapping of OrdType to strings.
			OrdTypeConverter.toTable = new Hashtable();
			for (int element = 0; element < pairs.GetLength(0); element++)
				OrdTypeConverter.toTable.Add(pairs[element, 0], pairs[element, 1]);

		}

		/// <summary>
		/// Converts a string to a OrdType.
		/// </summary>
		/// <param name="value">The FIX string representation of a OrdType.</param>
		/// <returns>A OrdType value.</returns>
		public static OrderTypeCode ConvertFrom(string value) {return (OrderTypeCode)OrdTypeConverter.fromTable[value];}

		/// <summary>
		/// Converts a OrdType to a string.
		/// </summary>
		/// <returns>A OrdType value.</returns>
		/// <param name="value">The FIX string representation of a OrdType.</param>
		public static string ConvertTo(OrderTypeCode messageType) {return (string)OrdTypeConverter.toTable[messageType];}

	}

}
