namespace Teraque
{

    using System.Collections;

    /// <summary>
	/// Type Converter for FIX IoiQltyInd Field
	/// </summary>
	public class IoiQltyIndConverter
	{

		// Private Members
		private static Hashtable fromTable;
		private static Hashtable toTable;
		private static object[,] pairs =
		{
			{IoiQualityIndicatorCode.Low, "L"},
			{IoiQualityIndicatorCode.Medium, "M"},
			{IoiQualityIndicatorCode.High, "H"}
		};

		/// <summary>
		/// Initializes the shared members of a IoiQltyIndConverter.
		/// </summary>
		static IoiQltyIndConverter()
		{

			// Initialize the mapping of strings to IoiQltyInd.
			IoiQltyIndConverter.fromTable = new Hashtable();
			for (int element = 0; element < pairs.GetLength(0); element++)
				IoiQltyIndConverter.fromTable.Add(pairs[element, 1], pairs[element, 0]);

			// Initialize the mapping of IoiQltyInd to strings.
			IoiQltyIndConverter.toTable = new Hashtable();
			for (int element = 0; element < pairs.GetLength(0); element++)
				IoiQltyIndConverter.toTable.Add(pairs[element, 0], pairs[element, 1]);

		}

		/// <summary>
		/// Converts a string to a IoiQltyInd.
		/// </summary>
		/// <param name="value">The FIX string representation of a IoiQltyInd.</param>
		/// <returns>A IoiQltyInd value.</returns>
		public static IoiQualityIndicatorCode ConvertFrom(string value) {return (IoiQualityIndicatorCode)IoiQltyIndConverter.fromTable[value];}

		/// <summary>
		/// Converts a IoiQltyInd to a string.
		/// </summary>
		/// <returns>A IoiQltyInd value.</returns>
		/// <param name="value">The FIX string representation of a IoiQltyInd.</param>
		public static string ConvertTo(IoiQualityIndicatorCode value) {return (string)IoiQltyIndConverter.toTable[value];}

	}

}
