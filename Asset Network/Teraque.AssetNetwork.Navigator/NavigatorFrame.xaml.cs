namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections;
	using System.Windows.Controls;
	using Teraque.Windows;
	using Teraque.Windows.Controls;

	/// <summary>
	/// Displays categories using multiple views.
	/// </summary>
	/// <copyright>Copyright © 2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class NavigatorFrame : ExplorerFrame
	{

		/// <summary>
		/// Initializes a new instance of the NavigatorViewer class.
		/// </summary>
		public NavigatorFrame()
		{

			// The IDE managed resources are initialized here.
			InitializeComponent();

		}

		/// <summary>
		/// Handles a change to the interpreted Source URI for this frame.
		/// </summary>
		/// <param name="oldUri">The old Source URI.</param>
		/// <param name="newUri">The new Source URI.</param>
		protected override void OnSourceChanged(Uri oldUri, Uri newUri)
		{

			// The main idea here is that we have a data model that streams in.  There is no 'start' or 'end' of load, so we need to deal with the model as it 
			// builds itself. Because the AssetNetworkCollection can build itself incrementally as data loads, we can get into a scenario where the outline of the 
			// list is built and all the resources are loaded, but we haven't yet loaded the 'Property Store' table, where the metadata containing the viewer for 
			// any given object is kept.  This logic here will hook itself into the current AssetNetworkItem selected and watch for an update to the viewer.  If 
			// and when a new viewer should arrive, we'll just poke it in.  This will unhook the previous AssetNetworkItem from watching for the change.
			if (oldUri != null)
			{
				AssetNetworkItem oldItem = ExplorerHelper.FindExplorerItem(this.DataContext as IExplorerItem, oldUri) as AssetNetworkItem;
				if (oldItem != null)
					oldItem.PropertyChanged -= this.OnItemPropertyChanged;
			}

			// This will allow us to poke a new viewer in should the properties for the current AssetNetworkItem selected be changed by an update from the data 
			// model.
			if (newUri != null)
			{
				AssetNetworkItem newItem = ExplorerHelper.FindExplorerItem(this.DataContext as IExplorerItem, newUri) as AssetNetworkItem;
				if (newItem != null)
					newItem.PropertyChanged += this.OnItemPropertyChanged;
			}

		}

		/// <summary>
		/// Represents the method that will handle the PropertyChanged event raised when a property is changed on a component.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A PropertyChangedEventArgs that contains the event data.</param>
		void OnItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{

			// In this scenario, we may have constructed an AssetNetworkCollection hierarchy for the application, but not have all the properties loaded, such as 
			// the viewer for a given object. When the viewer finally does arrive, or if the viewer is changed dynamically, this will force the new viewer to be the
			// actual URI source of the frame.  This shouldn't be confused with the intepreted source, which is really just a method of Source URI mapping.  If I
			// hadn't been lazy, I might have just tried to use the SourceUri Mapping data structures on this window to map an '/Administrator' into a
			// 'pack://application:,,,/Teraque.AssetNetwork.Navigator;component/DirectoryPage.xaml?Administrator' source.
			AssetNetworkItem assetNetworkItem = sender as AssetNetworkItem;
			if (e.PropertyName == "Viewer")
				this.ActualSource = assetNetworkItem.Viewer;

		}

	}

}
