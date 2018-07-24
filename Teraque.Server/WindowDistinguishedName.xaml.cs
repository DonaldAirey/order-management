namespace Teraque
{

	using System;
	using System.Collections.ObjectModel;
	using System.Net;
	using System.Security;
	using System.Security.Cryptography.X509Certificates;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Controls.Primitives;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Threading;

	/// <summary>
	/// Prompts the user for a domain, user name and password.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class WindowDistinguishedName : Window
	{

		/// <summary>
		/// The message displayed in the banner area of the dialog.
		/// </summary>
		public String BannerMessage { get; private set; }

		/// <summary>
		/// Identifies the CapsLockVisibilityProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty CapsLockVisibilityProperty = DependencyProperty.Register(
			"CapsLockVisibility",
			typeof(Visibility),
			typeof(WindowDistinguishedName),
			new FrameworkPropertyMetadata(Visibility.Collapsed));

		/// <summary>
		/// The credentials that are displayed in the dialog.
		/// </summary>
		DomainCredential domainCredentials = new DomainCredential();

		/// <summary>
		/// The name of the remote server.
		/// </summary>
		String serverName;

		/// <summary>
		/// Initializes a new instance of a WindowCertificate class.
		/// </summary>
		public WindowDistinguishedName()
		{

			// This will initialize the IDE maintained resources.
			this.InitializeComponent();

			// This provides a data context for all the MVVM operations in the dialog.
			this.DataContext = this;

			// This will provide an observable set of items for the control with a single element.  While it seems like overkill for a single element, the 
			// construction of the dialog box in this way leaves open the possibility of remembering and providing one or more set of credentials in the future.
			this.listBoxCredentials.Items.Add(domainCredentials);
			this.listBoxCredentials.SelectedItem = domainCredentials;
			this.listBoxCredentials.ItemContainerGenerator.StatusChanged += OnItemContainerGeneratorStatusChanged;

			// The dialog is not always activated when run from a console application.
			this.Loaded += OnLoaded;

			// A warning is generated in the dialog box when the caps lock is on.  This sets the initial state.  Hereafter, the key down event will check the status
			// and update the warning when the caps lock is toggled on.
			this.CapsLockVisibility = Keyboard.IsKeyToggled(Key.CapsLock) ? Visibility.Visible : Visibility.Collapsed;

		}

		/// <summary> 
		/// Gets or sets the visibility of the caps lock on warning.
		/// </summary>
		public Visibility CapsLockVisibility
		{
			get
			{
				return (Visibility)this.GetValue(WindowDistinguishedName.CapsLockVisibilityProperty);
			}
			set
			{
				this.SetValue(WindowDistinguishedName.CapsLockVisibilityProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the currently selected X509 certificate in the dialog.
		/// </summary>
		public DistinguishedNameCredential DistinguishedNameCredential
		{

			get
			{

				// This will convert the DistinguishedNameCredential with it's distinguished user name into a standard user login model view that is suitable for
				// presenting in a dialog box.  The tenant and user names are entered like a standard windows' domain user name (e.g. <Domain>\<User Name>').
				DistinguishedNameCredential distinguishedNameCredential = new DistinguishedNameCredential();
				String[] parts = this.domainCredentials.DomainUserName.Split('\\');
				distinguishedNameCredential.DistinguishedName = parts.Length == 1 ?
					String.Format("CN={0},{1}", parts[0], Properties.Settings.Default.OrganizationDistinctName) :
					String.Format("CN={0},OU={1},{2}", parts[1], parts[0], Properties.Settings.Default.OrganizationDistinctName);
				distinguishedNameCredential.SecurePassword = this.domainCredentials.SecurePassword;
				distinguishedNameCredential.RememberCredentials = this.domainCredentials.RememberCredentials;
				return distinguishedNameCredential;

			}

			set
			{

				// Validate the value before using it.
				if (value == null)
					throw new ArgumentNullException("value");

				// This will pull apart the stardard windows dialog box and construct a distinguished name from the single user field.  The distinguished name is
				// what our security tokens will recognize and as our server validator uses Active Directory, this puts most of the work of formatting a user name
				// on the client, where it belongs.
				String tenantalUnit = String.Empty;
				String commonName = String.Empty;
				foreach (String part in value.DistinguishedName.Split(','))
				{
					String[] smallerParts = part.Trim().Split('=');
					if (String.Compare(smallerParts[0], "cn", true) == 0)
						commonName = smallerParts[1];
					if (String.Compare(smallerParts[0], "ou", true) == 0)
						tenantalUnit = smallerParts[1];
				}
				this.domainCredentials.DomainUserName = tenantalUnit == String.Empty ? commonName : String.Format("{0}\\{1}", tenantalUnit, commonName);
				this.domainCredentials.SecurePassword = value.SecurePassword;
				this.domainCredentials.RememberCredentials = value.RememberCredentials;

			}

		}

		/// <summary>
		/// Gets or sets the name of the remote server.
		/// </summary>
		public String ServerName
		{
			get
			{
				return this.serverName;
			}
			set
			{
				this.serverName = value;
				this.BannerMessage = String.Format(Properties.Resources.DomainUserNamePasswordBannerMessage, this.serverName);
			}
		}

		/// <summary>
		/// Finds a visual child of the given type.
		/// </summary>
		/// <typeparam name="Type">The type of the visual child to find.</typeparam>
		/// <param name="dependencyObject">The current visual child to search.</param>
		/// <returns>The visual child that has the given type of null if none is present in the visual tree.</returns>
		Type FindVisualChild<Type>(DependencyObject dependencyObject)
			where Type : DependencyObject
		{

			// Recursively search the visual tree for a child of the given type.
			for (Int32 index = 0; index < VisualTreeHelper.GetChildrenCount(dependencyObject); index++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, index);
				if (child != null && child is Type)
					return (Type)child;
				else
				{
					Type childOfChild = FindVisualChild<Type>(child);
					if (childOfChild != null)
						return childOfChild;
				}
			}

			// At this point the entire visual tree was searched and no child of the given type was found.
			return null;

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
		/// Handles the Cancel button Click event.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="e">The routed event arguments.</param>
		void OnButtonCancelClick(Object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
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

							// The main idea here is to set the focus into the 'Password' control when a user name and domain are already present.  This lets the
							// user skip typing in their name or tabbing out of it when the name is persistent.  Note that the focus needs to be done at a later time.  There
							// is some strange connection between the ItemsGenerator and the control that will emit an exception (sometimes) when we try to set the 
							// focus during the status change.
							ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(listBoxItem);
							DataTemplate dataTemplate = contentPresenter.ContentTemplate;
							TextBox domainUserNameTextBox = dataTemplate.FindName("DomainUserNameTextBox", contentPresenter) as TextBox;
							PasswordBox passwordBox = dataTemplate.FindName("PasswordBox", contentPresenter) as PasswordBox;
							Dispatcher.BeginInvoke(
								DispatcherPriority.Background,
								new Action(() =>
								{
									if (domainUserNameTextBox.Text.Length == 0)
										domainUserNameTextBox.Focus();
									else
										passwordBox.Focus();
								}));

							// At this point we no longer need to set the focus.  The items are all generated and the regular focus scope will take care of the
							// keyboard navigation from now on.
							this.listBoxCredentials.ItemContainerGenerator.StatusChanged -= OnItemContainerGeneratorStatusChanged;

						}
					}
				}

		}

		/// <summary>
		/// Occurs when this FrameworkElement is initialized.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="eventArgs">An EventArgs that contains no event data.</param>
		void OnPasswordBoxInitialized(Object sender, EventArgs eventArgs)
		{

			// Since we don't have access to the password (as it is secured inside the SecureString object), we will fill in the password box with a dummy set of
			// characters that is the same length as the password.  Since the box encodes the characters anyway, it doesn't 'really matter what we initialize the
			// box with so long as it has the proper number of characters.
			PasswordBox passwordBox = sender as PasswordBox;
			DomainCredential domainCredentials = passwordBox.DataContext as DomainCredential;
			passwordBox.Password = new String('*', domainCredentials.SecurePassword.Length);

			// We assume that all passwords are going to be overwritten since we can't really edit them like a visible string.
			passwordBox.SelectAll();

		}

		/// <summary>
		/// Occurs when the value of the Password property changes.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="routedEventArgs">An EventArgs that contains no event data.</param>
		private void OnPasswordBoxPasswordChanged(object sender, RoutedEventArgs routedEventArgs)
		{

			// When the password box has been loaded and is taking changes from the user, we will copy the changes out of the password box and into the data context
			// that was associated with the item.  This isn't, strictly speaking, MVVM, but its as close as we're going to get without a dependency property on the
			// 'SecurePassword' property.  This is understandable for security reasons.
			if (this.IsLoaded)
			{
				PasswordBox passwordBox = sender as PasswordBox;
				DomainCredential domainCredentials = passwordBox.DataContext as DomainCredential;
				domainCredentials.SecurePassword = passwordBox.SecurePassword;
			}

		}

		/// <summary>
		/// Invoked when an unhandled Keyboard.PreviewKeyDown attached event reaches an element in its route that is derived from this class. Implement this method 
		/// to add class handling for this event.
		/// </summary>
		/// <param name="keyEventArgs">The KeyEventArgs that contains the event data.</param>
		protected override void OnPreviewKeyDown(KeyEventArgs keyEventArgs)
		{

			// This will update the status of the warning about the caps lock.  This is important because there is no feedback when entering a case-sensitive
			// password.
			this.CapsLockVisibility = Keyboard.IsKeyToggled(Key.CapsLock) ? Visibility.Visible : Visibility.Collapsed;

		}

	}

}
