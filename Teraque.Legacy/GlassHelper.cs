namespace Teraque
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;

	/// <summary>
	/// Support functions for the glass frame in Windows Vista.
	/// </summary>
	public static class GlassHelper
	{

		/// <summary>
		/// Gets an indication of whether the DWM is running or not.
		/// </summary>
		/// <param name="pfEnabled"></param>
		[DllImport("dwmapi.dll")]
		private static extern void DwmIsCompositionEnabled(ref bool pfEnabled);

		/// <summary>
		/// Extends the frame drawing into the client area.
		/// </summary>
		/// <param name="hwnd">A handle to the frame window.</param>
		/// <param name="margins">The new margins of the frame.</param>
		/// <returns>0 if the method was successful, otherwise an error code.</returns>
		[DllImport("dwmapi.dll")]
		private static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Margins margins);

		/// <summary>
		/// The margins of the frame window.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		private struct Margins
		{
			public int Left;
			public int Right;
			public int Top;
			public int Bottom;
		}

		/// <summary>
		/// Gets an indication of whether or not the DWM is enabled for this machine.
		/// </summary>
		public static bool IsCompositionEnabled
		{

			get
			{

				try
				{

					// Don't even bother calling the DLL if the OS isn't Vista.
					if (Environment.OSVersion.Version.Major < 6)
						return false;

					// This will ask DWM if it can draw a glass frame around the client window.
					bool compositionEnabled = false;
					DwmIsCompositionEnabled(ref compositionEnabled);
					return compositionEnabled;

				}
				catch (DllNotFoundException dllNotFoundException)
				{

					// Make sure any errors trying to load the DWM DLL are logged.
					Log.Error("{0}, {1}", dllNotFoundException.Message, dllNotFoundException.StackTrace);

				}

				// Any error loading the library will disable any features that go along with DWM, such as the glass frame around a
				// window.
				return false;

			}

		}

		/// <summary>
		/// Extends the drawing of the frame around a client window into the client area.
		/// </summary>
		/// <param name="window">The window that requires a different size frame.</param>
		/// <param name="thickness">The new margins around the window where the frame will be drawn.</param>
		public static void ExtendGlassIntoClientArea(Window window, Thickness thickness)
		{

			try
			{

				// The window requires a transparent background for the client area in order to draw the glass frame.  I can't 
				// find any documentation to say why this should work, just an example on the MSDN site. The documentation does
				// claim that the background must be opaque, but the fact that this works seems to contradict the documentation.
				IntPtr mainWindowPtr = new WindowInteropHelper(window).Handle;
				HwndSource mainWindowSrc = HwndSource.FromHwnd(mainWindowPtr);
				mainWindowSrc.CompositionTarget.BackgroundColor = Colors.Transparent;

				// This will set the margin of the frame window to the new thickness settings.
				Margins margins = new Margins();
				margins.Left = Convert.ToInt32(thickness.Left);
				margins.Right = Convert.ToInt32(thickness.Right);
				margins.Top = Convert.ToInt32(thickness.Top);
				margins.Bottom = Convert.ToInt32(thickness.Bottom);
				DwmExtendFrameIntoClientArea(mainWindowSrc.Handle, ref margins);

			}
			catch (DllNotFoundException dllNotFoundException)
			{

				// Make sure any errors trying to load the DWM DLL are logged.
				Log.Error("{0}, {1}", dllNotFoundException.Message, dllNotFoundException.StackTrace);

			}

		}

	}

}
