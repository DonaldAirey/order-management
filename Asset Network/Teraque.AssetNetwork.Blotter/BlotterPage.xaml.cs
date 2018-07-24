namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Data;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Windows;
	using System.Windows.Input;
	using Teraque.AssetNetwork.WebService;
	using Teraque.AssetNetwork.Windows;
	using Teraque.Windows.Controls;
	using Teraque.Windows.Controls.Primitives;

	/// <summary>
	/// Represents a control that displays a generic trade blotter.
	/// </summary>
	/// <copyright>Copyright © 2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class BlotterPage : BlotterPageBase
	{

		/// <summary>
		/// Initializes a new instance of BlotterPage class.
		/// </summary>
		public BlotterPage()
		{

			// The IDE managed resources are initialized here.
			this.InitializeComponent();

		}

	}

}
