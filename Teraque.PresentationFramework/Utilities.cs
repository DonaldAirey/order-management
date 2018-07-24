namespace Teraque.Windows
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
		/// Provides a diagnostic readout of the current FocusScope hierarchy.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public static void DumpFocusScope()
		{

			// This is helpful in tracking the mouse capture.
			IInputElement keyboardFocused = Keyboard.FocusedElement;
			IInputElement mouseCaptured = Mouse.Captured;
			Console.WriteLine(
				Properties.Resources.FocusScopeDump,
				keyboardFocused == null ? (Object)"<null>" : keyboardFocused,
				mouseCaptured == null ? (Object)"<null>" : mouseCaptured);

			// Dump the FocusScope hierarchy starting at the top.
			Utilities.DumpFocusScope(Application.Current.MainWindow, 0);

		}

		/// <summary>
		/// Recursively generates a diagnostic readout of the current FocusScope hierarchy.
		/// </summary>
		/// <param name="root">The current element in the visual tree hierarchy.</param>
		/// <param name="level">The current level used for indentation.</param>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		static void DumpFocusScope(DependencyObject root, Int32 level)
		{

			// Display the Scope and the currently focused element within that scope.
			if (FocusManager.GetIsFocusScope(root))
			{
				IInputElement focusedElement = FocusManager.GetFocusedElement(root);
				Console.WriteLine(Properties.Resources.FocusScopeScope, new String('\t', level), root, focusedElement == null ? (Object)"<null>" : focusedElement);
				level++;
			}

			// Display the currently focused item in its proper place in the hierarchy.
			if (Keyboard.FocusedElement == root)
				Console.WriteLine(Properties.Resources.FocusScopeCurrentFocus, new String('\t', level), root);

			// This will use the visual tree to recurse into the hierarchy looking for more UIElements that can be FocusScopes.
			for (Int32 childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(root); childIndex++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(root, childIndex);
				if (child is UIElement)
					DumpFocusScope(child, level);
			}

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
