namespace Teraque.Windows.Input
{

	using System;
	using System.Windows.Input;

	/// <summary>
	/// Commands that are common to all Teraque applications.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public sealed class Commands
	{

		/// <summary>
		/// Initializes a new instance of the Commands class. 
		/// </summary>
		Commands() { }

		/// <summary>
		/// Present the application's "About" window.
		/// </summary>
		public static readonly RoutedCommand About = new RoutedCommand("About", typeof(Commands));

		/// <summary>
		/// Change column.
		/// </summary>
		public static readonly RoutedCommand ChangeColumns = new RoutedCommand("ChangeColumns", typeof(Commands));

		/// <summary>
		/// Accept the values in a dialog.
		/// </summary>
		public static readonly RoutedCommand DialogAccept = new RoutedCommand("DialogAccept", typeof(Commands));

		/// <summary>
		/// Apply the values of a dialog.
		/// </summary>
		public static readonly RoutedCommand DialogApply = new RoutedCommand("DialogApply", typeof(Commands));

		/// <summary>
		/// Cancels a dialog.
		/// </summary>
		public static readonly RoutedCommand DialogCancel = new RoutedCommand("DialogCancel", typeof(Commands));

		/// <summary>
		/// Filter
		/// </summary>
		public static readonly RoutedCommand Filter = new RoutedCommand("Filter", typeof(Commands));

		/// <summary>
		/// Auto-size All Columns command.
		/// </summary>
		public static readonly RoutedCommand FitAllColumns = new RoutedCommand("FitAllColumns", typeof(Commands));

		/// <summary>
		/// Auto-size Column command.
		/// </summary>
		public static readonly RoutedCommand FitColumn = new RoutedCommand("FitColumn", typeof(Commands));

		/// <summary>
		/// Choose folder and search options.
		/// </summary>
		public static readonly RoutedCommand FolderAndSearchOptions = new RoutedCommand("FolderAndSearchOptions", typeof(Commands));

		/// <summary>
		/// LogOff.
		/// </summary>
		public static readonly RoutedCommand LogOff = new RoutedCommand("LogOff", typeof(Commands));

		/// <summary>
		/// Maximize a window.
		/// </summary>
		public static readonly RoutedCommand MaximizeWindow = new RoutedCommand("MaximizeWindowCommand", typeof(Commands));

		/// <summary>
		/// More of whatever it is you got.
		/// </summary>
		public static readonly RoutedCommand More = new RoutedCommand("More", typeof(Commands));

		/// <summary>
		/// Minimize a window.
		/// </summary>
		public static readonly RoutedCommand MinimizeWindow = new RoutedCommand("MinimizeWindowCommand", typeof(Commands));

		/// <summary>
		/// The Open Item command.
		/// </summary>
		public static readonly RoutedCommand OpenItem = new RoutedCommand("OpenItem", typeof(Commands));

		/// <summary>
		/// Recycle (Refresh).
		/// </summary>
		public static readonly RoutedCommand Recycle = new RoutedCommand("Recycle", typeof(Commands));

		/// <summary>
		/// Remove the properties from the selected items.
		/// </summary>
		public static readonly RoutedCommand RemoveProperties = new RoutedCommand("RemoveProperties", typeof(Commands));

		/// <summary>
		/// Rename the selected item.
		/// </summary>
		public static readonly RoutedCommand Rename = new RoutedCommand("Rename", typeof(Commands));

		/// <summary>
		/// Reset the user preferences.
		/// </summary>
		public static readonly RoutedCommand ResetSettings = new RoutedCommand("ResetSettings", typeof(Commands));

		/// <summary>
		/// Starts or stops a Search operation.
		/// </summary>
		public static readonly RoutedCommand Search = new RoutedCommand("Search", typeof(Commands));

		/// <summary>
		/// Sorts the current view.
		/// </summary>
		public static readonly RoutedCommand Sort = new RoutedCommand("Sort", typeof(Commands));

		/// <summary>
		/// The View Content command.
		/// </summary>
		public static readonly RoutedCommand ViewContent = new RoutedCommand("ViewContent", typeof(Commands));

		/// <summary>
		/// Sets the visible state of the detail pane in an application.
		/// </summary>
		public static readonly RoutedCommand ViewDetailPane = new RoutedCommand("ViewDetailPane", typeof(Commands));

		/// <summary>
		/// The View Details command.
		/// </summary>
		public static readonly RoutedCommand ViewDetails = new RoutedCommand("ViewDetails", typeof(Commands));

		/// <summary>
		/// The View Extra Large Icons command.
		/// </summary>
		public static readonly RoutedCommand ViewExtraLargeIcons = new RoutedCommand("ViewExtraLargeIcons", typeof(Commands));

		/// <summary>
		/// The View Large Icons command.
		/// </summary>
		public static readonly RoutedCommand ViewLargeIcons = new RoutedCommand("ViewLargeIcons", typeof(Commands));

		/// <summary>
		/// Sets the visible state of the library pane in an application.
		/// </summary>
		public static readonly RoutedCommand ViewLibraryPane = new RoutedCommand("ViewLibraryPane", typeof(Commands));

		/// <summary>
		/// The View Medium Icons command.
		/// </summary>
		public static readonly RoutedCommand ViewMediumIcons = new RoutedCommand("ViewMediumIcons", typeof(Commands));
		/// <summary>
		/// Sets the visible state of the menu bar in an application.
		/// </summary>
		public static readonly RoutedCommand ViewMenuPane = new RoutedCommand("ViewMenuPane", typeof(Commands));

		/// <summary>
		/// Sets the visible state of the navigation pane in an application.
		/// </summary>
		public static readonly RoutedCommand ViewNavigationPane = new RoutedCommand("ViewNavigationPane", typeof(Commands));

		/// <summary>
		/// Sets the visible state of the preview pane in an application.
		/// </summary>
		public static readonly RoutedCommand ViewPreviewPane = new RoutedCommand("ViewPreviewPane", typeof(Commands));

		/// <summary>
		/// The View Simple List command.
		/// </summary>
		public static readonly RoutedCommand ViewSimpleList = new RoutedCommand("ViewSimpleList", typeof(Commands));

		/// <summary>
		/// The View Small Icons command.
		/// </summary>
		public static readonly RoutedCommand ViewSmallIcons = new RoutedCommand("ViewSmallIcons", typeof(Commands));

		/// <summary>
		/// Sets the visible state of the status pane in an application.
		/// </summary>
		public static readonly RoutedCommand ViewStatusPane = new RoutedCommand("ViewStatusPane", typeof(Commands));

		/// <summary>
		/// The View Tiles command.
		/// </summary>
		public static readonly RoutedCommand ViewTiles = new RoutedCommand("ViewTiles", typeof(Commands));

	}

}
