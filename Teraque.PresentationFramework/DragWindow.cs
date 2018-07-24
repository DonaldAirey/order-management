namespace Teraque.Windows
{

	using System;
	using System.Windows;
	using System.Windows.Interop;

	/// <summary>
	/// A Window used to hold an image of the object being dragged.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class DragWindow : Window
	{

		/// <summary>
		/// The size of this window is fixed (and reverse engineered from the Windows Explorer).
		/// </summary>
		const Double size = 104.0;

		/// <summary>
		/// Initialize the DragWindow class.
		/// </summary>
		static DragWindow()
		{

			// This creates an association with an implicit default style in the themes.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DragWindow), new FrameworkPropertyMetadata(typeof(DragWindow)));

		}

		/// <summary>
		/// Initialize a new instance of the DragWindow class.
		/// </summary>
		public DragWindow()
		{

			// Initialize the style of this window so it can hold a simple framework element without otherwise interacting with the environment.
			this.WindowStyle = WindowStyle.None;
			this.AllowsTransparency = true;
			this.AllowDrop = false;
			this.Background = null;
			this.IsEnabled = false;
			this.IsHitTestVisible = false;
			this.ShowInTaskbar = false;
			this.Topmost = true;

			// The size of this window is constant and reverse engineered from Windows Explorer.
			this.Width = DragWindow.size;
			this.Height = DragWindow.size;

			// In order to work as a propery drag-and-drop indicator, this window can't get involved with any of the hit testing or other windows messages that are
			// normally part of an active window.  Setting the extra style bits will make it appear to be simply an image rather than a functioning window.
			this.SourceInitialized += this.OnSourceInitialized;

		}

		/// <summary>
		/// Handles the initialization of the underlying window.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="eventArgs">The event data.</param>
		void OnSourceInitialized(Object sender, EventArgs eventArgs)
		{

			// A normal window will try to interact with the hit testing logic.  Since this window was designed to move with the cursor, this will cause all kinds
			// of havoc as the cursor moves because Windows will constantly think that the cursor has just moved over a new window, interrupt the normal
			// drag-and-drop operation, dispose of the window, create the window, ad nauseum.  This effectively makes the DragWindow dead to any sort of hit 
			// testing.
			PresentationSource windowSource = PresentationSource.FromVisual(this);
			IntPtr handle = ((HwndSource)windowSource).Handle;
			long styles = NativeMethods.GetWindowLong(handle, NativeMethods.GetWindowLongExtendedStyle);
			int hResult = NativeMethods.SetWindowLong(
				handle,
				NativeMethods.GetWindowLongExtendedStyle,
				(UInt32) styles | NativeMethods.WindowStyleExtendedLayered | NativeMethods.WindowStyleExtendedTransparent);
            if (hResult == 0)
                throw new InvalidOperationException("Call to Windows failed.");


		}

	}

}
