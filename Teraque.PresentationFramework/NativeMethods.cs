namespace Teraque
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Text;
	using System.Runtime.InteropServices;

	/// <summary>
	/// A collection of methods that call unmanaged (native) code.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal sealed class NativeMethods
	{

		/// <summary>
		/// Sent to all top-level windows when Desktop Window Manager (DWM) composition has been enabled or disabled.
		/// </summary>
		internal const Int32 ActivateApplication = 0x001c;

		/// <summary>
		/// Sent to all top-level windows when Desktop Window Manager (DWM) composition has been enabled or disabled.
		/// </summary>
		internal const Int32 DwmCompositionChanged = 0x031e;

		/// <summary>
		/// Sent to a window when the size or position of the window is about to change. An application can use this message to override the window's default
		/// maximized size and position, or its default minimum or maximum tracking size.
		/// </summary>
		internal const Int32 GetMinMaxInfo = 0x0024;

		/// <summary>
		/// Get Window Long Index - Window Id.
		/// </summary>
		internal const Int32 GetWindowLongId = -12;

		/// <summary>
		/// Get Window Long Index - Window Style.
		/// </summary>
		internal const Int32 GetWindowLongStyle = -16;

		/// <summary>
		/// Get Window Long Index - Window Extended Style.
		/// </summary>
		internal const Int32 GetWindowLongExtendedStyle = -20;

		/// <summary>
		/// The NonClientCalculateSize message is sent when the size and position of a window's client area must be calculated. By processing this message, an
		/// application can control the content of the window's client area when the size or position of the window changes.
		/// </summary>
		internal const Int32 NonClientCalculateSize = 0x0083;

		/// <summary>
		/// The NonClientActivate message is sent to a window when its nonclient area needs to be changed to indicate an active or inactive state.
		/// </summary>
		internal const Int32 NonClientActivate = 0x0086;

		/// <summary>
		/// Sets the metrics associated with icons.
		/// </summary>
		internal const Int32 SetIconMetrics = 0x002E;

		/// <summary>
		/// Sets the metrics associated with the nonclient area of nonminimized windows.
		/// </summary>
		internal const Int32 SetNonClientMetrics = 0x002A;

		/// <summary>
		/// A message that is sent to all top-level windows when the SystemParametersInfo function changes a system-wide setting or when policy settings have
		/// changed.
		/// </summary>
		internal const Int32 SettingChange = 0x001a;

		/// <summary>
		/// SetWindowLong - Layered Style.
		/// </summary>
		internal const Int32 WindowStyleExtendedLayered = 0x00080000;

		/// <summary>
		/// SetWindowLong - Transparent Style.
		/// </summary>
		internal const UInt32 WindowStyleExtendedTransparent = 0x0020;

		/// <summary>
		/// An application-defined callback function used with the EnumThreadWindows function.
		/// </summary>
		/// <param name="hwnd">A handle to a window associated with the thread specified in the EnumThreadWindows function.</param>
		/// <param name="lParam">The application-defined value given in the EnumThreadWindows function.</param>
		/// <returns>To continue enumeration, the callback function must return TRUE; to stop enumeration, it must return FALSE.</returns>
		internal delegate Boolean EnumThreadWndProc(IntPtr hwnd, IntPtr lParam);

		/// <summary>
		/// Delegate for calling native Windows Message Handlers.
		/// </summary>
		internal delegate IntPtr MessageHandler(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref Boolean handled);

		/// <summary>
		/// Defines the prototype for the callback function used by RemoveWindowSubclass and SetWindowSubclass.
		/// </summary>
		/// <param name="hWnd">The handle to the subclassed window.</param>
		/// <param name="msg">The message being passed.</param>
		/// <param name="wParam">Additional message information. The contents of this parameter depend on the value of uMsg.</param>
		/// <param name="lParam">Additional message information. The contents of this parameter depend on the value of uMsg. </param>
		/// <param name="uIdSubclass">The subclass ID.</param>
		/// <param name="refData">The reference data provided to the SetWindowSubclass function. This can be used to associate the subclass instance with a "this"
		/// pointer.</param>
		/// <returns>The return value is the result of the message processing and depends on the message sent.</returns>
		internal delegate IntPtr SubclassProc(IntPtr hWnd, Int32 msg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, IntPtr refData);

		/// <summary>
		/// SetWindowPos options
		/// </summary>
		[Flags]
		internal enum SetWindowPositionFlags : uint
		{
			ASYNCWINDOWPOS = 0x4000,
			DEFERERASE = 0x2000,
			DRAWFRAME = 0x0020,
			FRAMECHANGED = 0x0020,
			HIDEWINDOW = 0x0080,
			NOACTIVATE = 0x0010,
			NOCOPYBITS = 0x0100,
			NOMOVE = 0x0002,
			NOOWNERZORDER = 0x0200,
			NOREDRAW = 0x0008,
			NOREPOSITION = 0x0200,
			NOSENDCHANGING = 0x0400,
			NOSIZE = 0x0001,
			NOZORDER = 0x0004,
			SHOWWINDOW = 0x0040,
		}

		/// <summary>
		/// 
		/// </summary>
		[Flags]
		internal enum DwmBlurBehindFlags : uint
		{

			/// <summary>
			/// Indicates a value for fEnable has been specified.
			/// </summary>
			DWM_BB_ENABLE = 0x00000001,

			/// <summary>
			/// Indicates a value for hRgnBlur has been specified.
			/// </summary>
			DWM_BB_BLURREGION = 0x00000002,

			/// <summary>
			/// Indicates a value for fTransitionOnMaximized has been specified.
			/// </summary>
			DWM_BB_TRANSITIONONMAXIMIZED = 0x00000004

		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct DWM_BLURBEHIND
		{
			public DwmBlurBehindFlags dwFlags;
			public bool fEnable;
			public IntPtr hRgnBlur;
			public bool fTransitionOnMaximized;
		}

		/// <summary>
		/// Used to set the margins of a screen element during a layout operation.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		internal struct Margins
		{

			/// <summary>
			/// Left Margin
			/// </summary>
			public Int32 Left;

			/// <summary>
			/// Right Margin
			/// </summary>
			public Int32 Right;

			/// <summary>
			/// Top Margin
			/// </summary>
			public Int32 Top;

			/// <summary>
			/// Bottom Margin
			/// </summary>
			public Int32 Bottom;

		}

		/// <summary>
		/// 
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		internal struct Point
		{
			Int32 x;
			Int32 y;
		};

		/// <summary>
		/// 
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		internal struct MinMaxInfo
		{
			Point ptReserved;
			Point ptMaxSize;
			Point ptMaxPosition;
			Point ptMinTrackSize;
			Point ptMaxTrackSize;
		};

		/// <summary>
		/// Initializes a new instance of the NativeMethods class.
		/// </summary>
		NativeMethods()
		{
		}

		/// <summary>
		/// Calls the next handler in a window's subclass chain. The last handler in the subclass chain calls the original window procedure for the window.
		/// </summary>
		/// <param name="hwnd">A handle to the window being subclassed.</param>
		/// <param name="msg">A value of type unsigned int that specifies a window message.</param>
		/// <param name="wParam">Specifies additional message information. The contents of this parameter depend on the value of the window message.</param>
		/// <param name="lParam">Specifies additional message information. The contents of this parameter depend on the value of the window message.</param>
		/// <returns>The returned value is specific to the message sent. This value should be ignored.</returns>
		[DllImport("ComCtl32.dll")]
		internal static extern IntPtr DefSubclassProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="msg"></param>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll")]
		internal static extern IntPtr DefWindowProcW(IntPtr hWnd, Int32 msg, IntPtr wParam, IntPtr lParam);

		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("dwmapi.dll", PreserveSig = false)]
		internal static extern Int32 DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);

		/// <summary>
		/// Extends the window frame behind the client area.
		/// </summary>
		/// <param name="hwnd">The handle to the window for which the frame is extended into the client area.</param>
		/// <param name="margins">The pointer to a MARGINS Structure structure that describes the margins to use when extending the frame into the client
		/// area.</param>
		/// <returns>Returns S_OK if successful, or an error value otherwise.</returns>
		[DllImport("dwmapi.dll")]
		internal static extern Int32 DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Margins margins);

		/// <summary>
		/// Obtains a value that indicates whether Desktop Window Manager (DWM) composition is enabled.
		/// </summary>
		/// <param name="isEnabled">The pointer that receives the value indicating whether DWM composition is enabled. TRUE if DWM composition is enabled;
		/// otherwise, FALSE.</param>
		/// <returns>Returns S_OK if successful, or an error value otherwise.</returns>
		[DllImport("dwmapi.dll")]
		internal static extern Int32 DwmIsCompositionEnabled([MarshalAs(UnmanagedType.Bool)] ref Boolean isEnabled);

		/// <summary>
		/// Enumerates all nonchild windows associated with a thread by passing the handle to each window, in turn, to an application-defined callback function.
		/// </summary>
		/// <param name="dwThreadId">The identifier of the thread whose windows are to be enumerated.</param>
		/// <param name="lpfn">A pointer to an application-defined callback function. For more information, see EnumThreadWndProc.</param>
		/// <param name="lParam">An application-defined value to be passed to the callback function. </param>
		/// <returns>
		/// If the callback function returns TRUE for all windows in the thread specified by dwThreadId, the return value is TRUE. If the callback function returns
		/// FALSE on any enumerated window, or if there are no windows found in the thread specified by dwThreadId, the return value is FALSE.
		/// </returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern Boolean EnumThreadWindows(Int32 dwThreadId, EnumThreadWndProc lpfn, IntPtr lParam);

		/// <summary>
		/// Retrieves the thread identifier of the calling thread.
		/// </summary>
		/// <returns>The return value is the thread identifier of the calling thread.</returns>
		[DllImport("kernel32.dll")]
		internal static extern Int32 GetCurrentThreadId();

		[DllImport("user32.dll")]
		internal static extern UInt32 GetWindowLong(IntPtr hWnd, Int32 nIndex);

		/// <summary>
		/// Copies the text of the specified window's title bar (if it has one) into a buffer. If the specified window is a control, the text of the control is copied.
		/// </summary>
		/// <param name="hWnd">A handle to the window or control containing the text.</param>
		/// <param name="pString">The buffer that will receive the text. If the string is as long or longer than the buffer, the string is truncated and terminated
		/// with a null character.</param>
		/// <param name="nMaxCount">The maximum number of characters to copy to the buffer, including the null character. If the text exceeds this limit, it is
		/// truncated.</param>
		/// <returns>
		/// If the function succeeds, the return value is the length, in characters, of the copied string, not including the terminating null character. If the
		/// window has no title bar or text, if the title bar is empty, or if the window or control handle is invalid, the return value is zero. To get extended
		/// error information, call GetLastError. 
		/// </returns>
		[DllImport("User32.dll", CharSet=CharSet.Unicode)]
		internal static extern Int32 GetWindowText(IntPtr hWnd, StringBuilder pString, Int32 nMaxCount);

		/// <summary>
		/// Sends the specified message to a window or windows.
		/// </summary>
		/// <param name="hwnd">A handle to the window whose window procedure will receive the message.</param>
		/// <param name="msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="lParam">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport("user32.dll")]
		internal static extern IntPtr SendMessage(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
		/// </summary>
		/// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs..</param>
		/// <param name="nIndex">
		/// The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of
		/// an integer. To set any other value, specify one of the following values: GetWindowLongExtendedStyle, GWL_HINSTANCE, GetWindowLongId, GetWindowLongStyle, GWL_USERDATA, GWL_WNDPROC
		/// </param>
		/// <param name="dwNewLong">The replacement value.</param>
		/// <returns>
		/// If the function succeeds, the return value is the previous value of the specified 32-bit integer. If the function fails, the return value is zero. To get
		/// extended error information, call GetLastError.
		/// </returns>
		[DllImport("user32.dll")]
		internal static extern int SetWindowLong(IntPtr hWnd, Int32 nIndex, UInt32 dwNewLong);

		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll", PreserveSig = false)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern Boolean SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPositionFlags uFlags);

		/// <summary>
		/// Installs or updates a window subclass callback.
		/// </summary>
		/// <param name="hwnd">The handle of the window being subclassed.</param>
		/// <param name="pfnSubclass">A pointer to a window procedure. This pointer and the subclass ID uniquely identify this subclass callback.</param>
		/// <param name="uIdSubclass">The subclass ID. This ID together with the subclass procedure uniquely identify a subclass.</param>
		/// <param name="dwRefData">DWORD_PTR to reference data. The meaning of this value is determined by the calling application.</param>
		/// <returns></returns>
		[DllImport("ComCtl32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern Boolean SetWindowSubclass(IntPtr hwnd, SubclassProc pfnSubclass, IntPtr uIdSubclass, IntPtr dwRefData);

	}

}
