namespace Teraque.LicenseGenerator
{

	using System;
	using Teraque;
	using Teraque.Windows.Controls;

	/// <summary>
	/// A page with a common frame used for a directory-like view of the content.
	/// </summary>
	/// <copyright>Copyright © 2010 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class LicenseBook : ExplorerBook
	{

		/// <summary>
		/// The shared frame for all instances of this class.
		/// </summary>
		static LicenseViewer categoryViewer;

		/// <summary>
		/// Initialize the LicenseManagementPage class.
		/// </summary>
		static LicenseBook()
		{

			// This object is shared between all the instances of this page.  The general idea is that the actual content of the page can change, but the frame
			// members do not.  This is useful in something like a directory browser where you want the navigation tree and the tool bar to remain static, but want
			// the content to change as the user navigates through the hierarchy.
			LicenseBook.categoryViewer = new LicenseViewer();

		}

		/// <summary>
		/// Initializes a new instance of the LicenseManagementPage class.
		/// </summary>
		public LicenseBook()
		{

			// This is where the IDE managed resources are initialized.
			this.InitializeComponent();

		}

		/// <summary>
		/// Gets the shared frame window to be used with this class of pages.
		/// </summary>
		/// <returns>The frame used for all instances of this class.</returns>
		protected override ExplorerFrame FrameCore
		{
			get
			{
				return LicenseBook.categoryViewer;
			}
		}

	}

}
