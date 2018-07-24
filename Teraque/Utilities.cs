namespace Teraque
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.IO;
	using System.Windows;
	using System.Windows.Input;
	using System.Windows.Media;

	/// <summary>
	/// Various utilities that don't fall in an existing class.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public static class Utilities
	{

		/// <summary>
		/// The size of a buffer used to read a stream.
		/// </summary>
		const Int32 bufferSize = 16384;

		/// <summary>
		/// Outputs the contents of a byte array.
		/// </summary>
		/// <param name="name">The name of the byte array.</param>
		/// <param name="array">The byte array.</param>
		public static void EmitByteArray(String name, Byte[] array)
		{

			// Validate the arguments before using them.
			if (array == null)
				throw new ArgumentNullException("array");

			// This will format the byte array according to a C# declaration and emit as many elements as will fit on a line.
			Int32 width = 0;
			String declaration = String.Format(CultureInfo.InvariantCulture, Properties.Resources.EmitByteArrayDeclaration, name);
			width += declaration.Length + 8;
			Console.Write(Properties.Resources.EmitByteArrayHangingMargin, declaration);
			for (Int32 index = 0; index < array.Length - 1; index++)
			{
				String formattedByte = String.Format(CultureInfo.InvariantCulture, Properties.Resources.EmitByteArrayByteFormat, array[index]);
				width += formattedByte.Length;
				Console.Write(formattedByte);
				if (width + 1 + String.Format(CultureInfo.InvariantCulture, Properties.Resources.EmitByteArrayByteFormat, array[index + 1]).Length >= 165)
				{
					Console.WriteLine();
					Console.Write(Properties.Resources.EmitByteArrayMargin);
					width = 23;
				}
				else
				{
					Console.Write(Properties.Resources.EmitByteArrayByteGap);
					width++;
				}
			}
			Console.WriteLine(Properties.Resources.EmitByteArrayTermination, array[array.Length - 1]);

		}

		/// <summary>
		/// Read the entire contents of a stream into a byte array.
		/// </summary>
		/// <param name="stream">The stream to read.</param>
		/// <returns>A byte array containing the entire contents of the stream.</returns>
		public static Byte[] ReadStream(Stream stream)
		{

			// Validate the argument.
			if (stream == null)
				throw new ArgumentNullException("stream");

			// This will read the given stream into a memory stream and then return the results as a byte array.
			Byte[] buffer = new Byte[Utilities.bufferSize];
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32 read;
				while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
					memoryStream.Write(buffer, 0, read);
				return memoryStream.ToArray();
			}

		}


	}
}
