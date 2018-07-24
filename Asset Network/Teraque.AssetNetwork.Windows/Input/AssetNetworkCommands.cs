namespace Teraque.AssetNetwork.Windows.Input
{

	using System;
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Input;

	/// <summary>
	/// The commands used for the Teraque application.
	/// </summary>
	public sealed class AssetNetworkCommands
	{

		/// <summary>
		/// Initializes a new instance of the AssetNetworkCommands class.
		/// </summary>
		AssetNetworkCommands() { }

		/// <summary>
		/// Creates a slice from the selected orders and sends them to a destiantion.
		/// </summary>
		public static readonly RoutedCommand CreateSlice = new RoutedCommand("CreateSlice", typeof(AssetNetworkCommands));

		/// <summary>
		/// Opens the properties dialog for the selected item.
		/// </summary>
		public static readonly RoutedCommand Properties = new RoutedCommand("Properties", typeof(AssetNetworkCommands));

	}

}
