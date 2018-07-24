namespace Teraque.Windows.Controls
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// Represents a Windows menu control with an overflow panel that enables you to hierarchically organize elements associated with commands and event handlers.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ExplorerBar : GadgetBar
	{

		/// <summary>
		/// Initializes the ExplorerBar class.
		/// </summary>
		static ExplorerBar()
		{

			// This allows ExplorerBar instances to find their implicit styles in the themes.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ExplorerBar), new FrameworkPropertyMetadata(typeof(ExplorerBar)));

			// The ExplorerBar is not typically used as the main menu of an application.
			Menu.IsMainMenuProperty.OverrideMetadata(typeof(ExplorerBar), new FrameworkPropertyMetadata(false));

		}

	}

}
