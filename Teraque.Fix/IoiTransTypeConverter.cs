namespace Teraque
{

    using System.Collections;

    /// <summary>
	/// Type Converter for FIX IoiTransType Field
	/// </summary>
	public class IoiTransTypeConverter
	{

		// Private Members
		private static Hashtable fromTable;
		private static Hashtable toTable;
		private static object[,] pairs =
		{
			{IoiTransactionTypeCode.New, "N"},
			{IoiTransactionTypeCode.Cancel, "C"},
			{IoiTransactionTypeCode.Replace, "R"}
		};

		/// <summary>
		/// Initializes the shared members of a IoiTransTypeConverter.
		/// </summary>
		static IoiTransTypeConverter()
		{

			// Initialize the mapping of strings to IoiTransType.
			IoiTransTypeConverter.fromTable = new Hashtable();
			for (int element = 0; element < pairs.GetLength(0); element++)
				IoiTransTypeConverter.fromTable.Add(pairs[element, 1], pairs[element, 0]);

			// Initialize the mapping of IoiTransType to strings.
			IoiTransTypeConverter.toTable = new Hashtable();
			for (int element = 0; element < pairs.GetLength(0); element++)
				IoiTransTypeConverter.toTable.Add(pairs[element, 0], pairs[element, 1]);

		}

		/// <summary>
		/// Converts a string to a IoiTransType.
		/// </summary>
		/// <param name="value">The FIX string representation of a IoiTransType.</param>
		/// <returns>A IoiTransType value.</returns>
		public static IoiTransactionTypeCode ConvertFrom(string value) {return (IoiTransactionTypeCode)IoiTransTypeConverter.fromTable[value];}

		/// <summary>
		/// Converts a IoiTransType to a string.
		/// </summary>
		/// <returns>A IoiTransType value.</returns>
		/// <param name="value">The FIX string representation of a IoiTransType.</param>
		public static string ConvertTo(IoiTransactionTypeCode value) {return (string)IoiTransTypeConverter.toTable[value];}

	}

}
