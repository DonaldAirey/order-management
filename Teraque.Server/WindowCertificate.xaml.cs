namespace Teraque
{

	using System;
	using System.Collections.ObjectModel;
	using System.Security.Cryptography.X509Certificates;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Controls.Primitives;
	using System.Windows.Threading;

	/// <summary>
	/// Prompts the user for a certificate.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class WindowCertificate : Window
	{

		/// <summary>
		/// Initializes a new instance of a WindowCertificate class.
		/// </summary>
		public WindowCertificate()
		{

			// This will initialize the IDE maintained resources.
			this.InitializeComponent();

			// This provides a data context for all the MVVM operations in the dialog.
			this.DataContext = this;

			// This will provide an observable set of items for the list box.
			this.listBoxCredentials.ItemContainerGenerator.StatusChanged += OnItemContainerGeneratorStatusChanged;

			// The dialog is not always activated when run from a console application.
			this.Loaded += OnLoaded;

		}

		/// <summary>
		/// Gets the collection of X509 Certificates that are displayed in the dialog.
		/// </summary>
		public ItemCollection X509Certificate2s
		{
			get
			{
				return this.listBoxCredentials.Items;
			}
		}

		/// <summary>
		/// Gets or sets the currently selected X509 certificate in the dialog.
		/// </summary>
		public X509Certificate2 X509Certificate2
		{
			get
			{
				return this.listBoxCredentials.SelectedItem as X509Certificate2;
			}
			set
			{
				this.listBoxCredentials.SelectedItem = value;
				this.listBoxCredentials.ScrollIntoView(this.listBoxCredentials.SelectedItem);
			}
		}

		/// <summary>
		/// Handles the Cancel button Click event.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="e">The routed event arguments.</param>
		void OnButtonCancelClick(Object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}

		/// <summary>
		/// Handles the OK button Click event.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The routed event arguments.</param>
		void OnButtonOkClick(Object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		/// <summary>
		/// Handles the ViewCertificate button Click event.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The routed event arguments.</param>
		void OnButtonViewCertificateClick(Object sender, RoutedEventArgs e)
		{

			// Display the selected certificate.
			X509Certificate2 selectedCertificate = this.listBoxCredentials.SelectedItem as X509Certificate2;
			if (selectedCertificate != null)
				X509Certificate2UI.DisplayCertificate(selectedCertificate);

		}

		/// <summary>
		/// The StatusChanged event is raised by a ItemContainerGenerator to inform controls that its status has changed.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="eventArgs">An EventArgs that contains no event data.</param>
		void OnItemContainerGeneratorStatusChanged(Object sender, EventArgs eventArgs)
		{

			// The main idea here is to wait for the items container generator to generate the selected item, then we can set the focus on it.  This is a queer 
			// architecture because in the MVVM world, there is no property for setting the focus and, even if you could, the object (in a virtual world) doesn't
			// always exist at the point where you select it.
			if (this.listBoxCredentials.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
				if (this.listBoxCredentials.Items.Count != 0)
				{
					Int32 index = this.listBoxCredentials.SelectedIndex;
					if (index >= 0)
					{
						ListBoxItem listBoxItem = this.listBoxCredentials.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem;
						if (listBoxItem != null)
						{

							// This will unhook the event handler from the status updates and set the focus.  Notice that we need to defer the actual operation of
							// setting the focus as there is some strange connection between the ItemsControl and the keyboard focus that will throw an exception
							// (sometimes) when the focus is set during the items container generator status change.
							this.listBoxCredentials.ItemContainerGenerator.StatusChanged -= OnItemContainerGeneratorStatusChanged;
							Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => listBoxItem.Focus()));

						}
					}
				}

		}

		/// <summary>
		/// Occurs when the element is laid out, rendered, and ready for interaction.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="routedEventArgs">An EventArgs that contains no event data.</param>
		void OnLoaded(Object sender, RoutedEventArgs routedEventArgs)
		{

			// When called from a console application, this dialog isn't always activated automatically.
			this.Activate();

		}

	}

}
