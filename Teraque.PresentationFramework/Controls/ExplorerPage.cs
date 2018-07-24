namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections.ObjectModel;
	using System.Windows;
	using System.Windows.Controls;
	using Teraque.Windows.Controls.Primitives;
	using System.Windows.Input;

	/// <summary>
	/// A Page that interacts with the ExplorerFrame host.
	/// </summary>
	public class ExplorerPage : Page
	{

		/// <summary>
		/// A collection of elements that will be bound to the GadgetBar when the page is loaded.
		/// </summary>
		ObservableCollection<FrameworkElement> elementCollection = new ObservableCollection<FrameworkElement>();

		/// <summary>
		/// Initializes a new instance of the ContentPage class.
		/// </summary>
		public ExplorerPage()
		{

			// Whenver this page is loaded it needs to interact with the frame.
			this.Loaded += new RoutedEventHandler(this.OnLoaded);

		}

		/// <summary>
		/// Gets the collection of Gadgets for this page.
		/// </summary>
		public ObservableCollection<FrameworkElement> GadgetBar
		{
			get
			{
				return this.elementCollection;
			}
		}

		/// <summary>
		/// Occurs when the element is laid out, rendered, and ready for interaction.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnLoaded(Object sender, RoutedEventArgs e)
		{

			// The GadgetBar is actually not part of the ExplorerPage even though its resources are declared here.  When the ExplorerPage is hooked into the
			// ExplorerFrame, the frame gets a message asking it to use the GadgetBar (which is really just an ObservableCollection of Gadgets) as the ItemsSource  
			// in the ExplorerBar control on the frame.  So The GadgetBar property in this control is really just a collection that is used by the main frame window
			// of the application.  As such, it has no way of routing commands back to the ExplorerPage where the Gadget was declared.  If we left the CommandTarget
			// alone, the Command Routing would attempt to find the element with the focus, but that is too random for most high level, application-type commands.
			// This will direct every Gadget with a command, that has a corresponding CommandBinding, to target this page when the command is invoked.
			foreach (Gadget gadget in this.GadgetBar)
				foreach (CommandBinding commandBinding in this.CommandBindings)
					if (gadget.Command == commandBinding.Command && gadget.CommandTarget == null)
						gadget.CommandTarget = this;

			// The data context for the page is the item selected in the frame.
			ExplorerFrame explorerFrame = VisualTreeExtensions.FindAncestor<ExplorerFrame>(this);
			if (explorerFrame != null)
				this.DataContext = explorerFrame.SelectedItem;

			// This will send a message up to the frame that there is a new set of gadgets available.  The frame will use this instance's collection directly so
			// that changing the local collection here will change the way the gadgets are displayed in the frame's toolbar.
			if (this.GadgetBar.Count != 0)
				this.RaiseEvent(new ItemsSourceEventArgs(ExplorerFrame.GadgetBarChangedEvent, this.GadgetBar));

		}

	}

}
