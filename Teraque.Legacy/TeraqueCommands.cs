namespace Teraque
{

	using System;
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Input;

	public class TeraqueCommands
	{

		/// <summary>
		/// Applies the current action but doesn't dismiss the prompt.
		/// </summary>
		public static readonly RoutedCommand Apply;

		/// <summary>
		/// Cancels the current action and dismisses the prompt.
		/// </summary>
		public static readonly RoutedCommand Cancel;
		
		/// <summary>
		/// Freezes the headers at their current position.
		/// </summary>
		public static readonly RoutedCommand FreezeHeaders;

		/// <summary>
		/// Freezes the panes of the report in their current position.
		/// </summary>
		public static readonly RoutedCommand FreezePanes;

		/// <summary>
		/// Mark the element as having been read.
		/// </summary>
        public static readonly RoutedCommand MarkAsRead;

		/// <summary>
		/// Mark the element has not having been read.
		/// </summary>
        public static readonly RoutedCommand MarkAsUnread;

		/// <summary>
		/// What?
		/// </summary>
        public static readonly RoutedCommand MaximizeRestore;

		/// <summary>
		/// Changes the authentication used for the current user.
		/// </summary>
        public static readonly RoutedCommand Login;

		/// <summary>
		/// Accepts the current action and dismisses the prompt.
		/// </summary>
		public static readonly RoutedCommand OK;

		/// <summary>
		/// Create a preview of the report before it is printed.
		/// </summary>
		public static readonly RoutedCommand PrintPreview;

		/// <summary>
		/// Reset the application parameters to their factory defaults.
		/// </summary>
		public static readonly RoutedCommand ResetSettings;

		/// <summary>
		/// Resync Data 
		/// </summary>
		public static readonly RoutedCommand ResyncData;

		/// <summary>
		/// Select one or more columns in the report.
		/// </summary>
        public static readonly RoutedCommand SelectColumns;

		/// <summary>
		/// Set the animation to the fastest setting.
		/// </summary>
        public static readonly RoutedCommand SetAnimationFast;

		/// <summary>
		/// Set the animation to the medium setting.
		/// </summary>
		public static readonly RoutedCommand SetAnimationMedium;

		/// <summary>
		/// Turn off the animation effects.
		/// </summary>
		public static readonly RoutedCommand SetAnimationOff;

		/// <summary>
		/// Set the animation effects to the slowest settings.
		/// </summary>
		public static readonly RoutedCommand SetAnimationSlow;

		/// <summary>
		/// Set the Frame.
		/// </summary>
		public static readonly RoutedCommand SetFrame;

		/// <summary>
		/// Enables the navigation pane.
		/// </summary>
		public static readonly RoutedCommand SetIsNavigationPaneVisible;

		/// <summary>
		/// Sorts the report using a selected column.
		/// </summary>
		public static readonly RoutedCommand SortReport;

		/// <summary>
		/// Shows a window in the report.
		/// </summary>
		public static readonly RoutedCommand ReportShowWindow;

		/// <summary>
		/// Create the static resources used by this test module.
		/// </summary>
		static TeraqueCommands()
		{

			// Routed Commands
			TeraqueCommands.Apply = new RoutedCommand("Apply", typeof(TeraqueCommands));
			TeraqueCommands.Cancel = new RoutedCommand("Cancel", typeof(TeraqueCommands));			
			TeraqueCommands.FreezeHeaders = new RoutedCommand("FreezeHeaders", typeof(TeraqueCommands));
			TeraqueCommands.FreezePanes = new RoutedCommand("FreezePanes", typeof(TeraqueCommands));
			TeraqueCommands.Login = new RoutedCommand("Login", typeof(TeraqueCommands));
            TeraqueCommands.MarkAsRead = new RoutedCommand("MarkAsRead", typeof(TeraqueCommands));
            TeraqueCommands.MarkAsUnread = new RoutedCommand("MarkAsUnread", typeof(TeraqueCommands));
            TeraqueCommands.MaximizeRestore = new RoutedCommand("MaximizeRestore", typeof(TeraqueCommands));
			TeraqueCommands.OK = new RoutedCommand("OK", typeof(TeraqueCommands));
			TeraqueCommands.PrintPreview = new RoutedCommand("PrintPreview", typeof(TeraqueCommands));
			TeraqueCommands.ResetSettings = new RoutedCommand("ResetSettings", typeof(TeraqueCommands));
			TeraqueCommands.ResyncData = new RoutedCommand("ResyncData", typeof(TeraqueCommands));			
            TeraqueCommands.SelectColumns = new RoutedCommand("SelectColumns", typeof(TeraqueCommands));
            TeraqueCommands.SetAnimationFast = new RoutedCommand("SetAnimationFast", typeof(TeraqueCommands));
			TeraqueCommands.SetAnimationMedium = new RoutedCommand("SetAnimationMedium", typeof(TeraqueCommands));
			TeraqueCommands.SetAnimationOff = new RoutedCommand("SetAnimationOff", typeof(TeraqueCommands));
			TeraqueCommands.SetAnimationSlow = new RoutedCommand("SetAnimationSlow", typeof(TeraqueCommands));
			TeraqueCommands.SetFrame = new RoutedCommand("SetFrame", typeof(TeraqueCommands));
			TeraqueCommands.SetIsNavigationPaneVisible = new RoutedCommand("SetIsNavigationPaneVisible", typeof(TeraqueCommands));
			TeraqueCommands.SortReport = new RoutedCommand("SortReport", typeof(TeraqueCommands));
			TeraqueCommands.ReportShowWindow = new RoutedCommand("ReportShowWindow", typeof(TeraqueCommands));

            // Global Key Gestures
            TeraqueCommands.MarkAsRead.InputGestures.Add(new KeyGesture(Key.Q, ModifierKeys.Control));
            TeraqueCommands.MarkAsUnread.InputGestures.Add(new KeyGesture(Key.U, ModifierKeys.Control));
            TeraqueCommands.MaximizeRestore.InputGestures.Add(new KeyGesture(Key.F11));

		}

	}

}
