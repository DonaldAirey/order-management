namespace Teraque.AssetNetwork
{

	using System;
	using System.Windows.Input;

	/// <summary>
	/// Market Console Commands.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class Commands
	{

		/// <summary>
		/// Apply
		/// </summary>
		public static RoutedCommand Apply = new RoutedCommand("Apply", typeof(Commands));

		/// <summary>
		/// Clear the orders and executions.
		/// </summary>
		public static RoutedCommand ClearOrders = new RoutedCommand("ClearOrders", typeof(Commands));

		/// <summary>
		/// Refreshes the display
		/// </summary>
		public static RoutedCommand Refresh = new RoutedCommand("Refresh", typeof(Commands));

		/// <summary>
		/// Restarts a tenant.
		/// </summary>
		public static RoutedCommand Restart = new RoutedCommand("Restart", typeof(Commands));

		/// <summary>
		/// Set the simulator parameters.
		/// </summary>
		public static RoutedCommand SetSimulator = new RoutedCommand("SetSimulator", typeof(Commands));

		/// <summary>
		/// Starts a tenant.
		/// </summary>
		public static RoutedCommand Start = new RoutedCommand("Start", typeof(Commands));

		/// <summary>
		/// Stops a tenant.
		/// </summary>
		public static RoutedCommand Stop = new RoutedCommand("Stop", typeof(Commands));

	}

}
