namespace Teraque
{

	using System;
	using System.Globalization;
	using System.Collections.ObjectModel;

    /// <summary>
	/// Common Conversions
	/// </summary>
	[CLSCompliant(false)]
	public static class CommonConversion
	{

		/// <summary>
		/// Converts a string to have a lower case starting character.
		/// </summary>
		/// <param name="text">The text to be converted.</param>
		/// <returns>The input string with a lower case starting letter.</returns>
		public static String ToCamelCase(String text)
		{

			// Validate the argument before using it.
			if (text == null)
				throw new ArgumentNullException("text");

			// Convert the variable to its camel case equivalent.
			return text[0].ToString().ToLower(CultureInfo.CurrentCulture) + text.Remove(0, 1);

		}

		/// <summary>
		/// Converts a relative date into an actual date.
		/// </summary>
		/// <param name="length">An amount of time.</param>
		/// <param name="timeUnit">The units in which the time is measured.</param>
		/// <returns>The current time plus the relative time.</returns>
		public static DateTime ToDateTime(Decimal length, TimeUnitCode timeUnit)
		{

			// Start with the current time.
			DateTime startDate = DateTime.Now;

			switch (timeUnit)
			{
			case TimeUnitCode.Days:

				// When the time is measured in days.
				startDate += TimeSpan.FromDays(Convert.ToInt32(length));
				break;

			case TimeUnitCode.Weeks:

				// When the time is measured in weeks.
				startDate += TimeSpan.FromDays(Convert.ToInt32(length) * 7);
				break;

			case TimeUnitCode.Months:

				// When the time is measured in months.
				startDate += TimeSpan.FromDays(Convert.ToInt32(length) * 30);
				break;

			}

			// This value represents and absolute date from today.
			return startDate;

		}

		/// <summary>
		/// Converts an array of objects into the equivalent text.
		/// </summary>
		/// <param name="array">An array of objects to be converted.</param>
		/// <returns>The text equivalent of the array.</returns>
		public static String FromArray(Object[] array)
		{

			// Validate the parameters before using.
			if (array == null)
				throw new ArgumentNullException("array");

			String keyText = String.Empty;
			for (int index = 0; index < array.Length; index++)
			{
				if (array[index] != null)
					keyText = array[index].ToString();
				else
					keyText = "{NULL}";
				if (index < array.Length - 1)
					keyText += ", ";
			}

			return keyText;

		}

		/// <summary>
		/// Converts an array of objects into the equivalent text.
		/// </summary>
		/// <param name="collection">An array of objects to be converted.</param>
		/// <returns>The text equivalent of the array.</returns>
		public static String FromArray(ReadOnlyCollection<Object> collection)
		{

			// Validate the parameters before using.
			if (collection == null)
				throw new ArgumentNullException("collection");

			String keyText = String.Empty;
			for (int index = 0; index < collection.Count; index++)
			{
				if (collection[index] != null)
					keyText = collection[index].ToString();
				else
					keyText = "{NULL}";
				if (index < collection.Count - 1)
					keyText += ", ";
			}
			return keyText;

		}

	}

}
