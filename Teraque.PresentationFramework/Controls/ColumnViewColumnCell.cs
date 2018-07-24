namespace Teraque.Windows.Controls
{

	using System;
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// Provides a way to style simple content that appears in the ColumnView.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ColumnViewColumnCell : ContentControl
	{

		/// <summary>
		/// Initializes the ColumnViewColumnCell class.
		/// </summary>
		static ColumnViewColumnCell()
		{

			// This provides a default style based on the ColumnViewColumnCell class.  It allows the ResourceManager to find the default style for objects of this
			// type when they are instantiated.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ColumnViewColumnCell), new FrameworkPropertyMetadata(typeof(ColumnViewColumnCell)));

		}

	}

}