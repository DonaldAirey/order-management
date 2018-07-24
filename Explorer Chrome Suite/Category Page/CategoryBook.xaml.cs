namespace Teraque.CategoryPage
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using Teraque.Windows.Controls;

	/// <summary>
	/// A page with a common frame used for a directory-like view of the content.
	/// </summary>
	/// <copyright>Copyright © 2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	[SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
	public partial class CategoryBook : ExplorerBook
	{

		/// <summary>
		/// The shared frame for all instances of this class.
		/// </summary>
		static CategoryFrame categoryFrame = new CategoryFrame();

		/// <summary>
		/// Initializes a new instance of the CategoryBook class.
		/// </summary>
		public CategoryBook()
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
				return CategoryBook.categoryFrame;
			}
		}

	}

}
