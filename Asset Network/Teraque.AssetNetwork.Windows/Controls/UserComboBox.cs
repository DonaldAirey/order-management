namespace Teraque.AssetNetwork.Windows.Controls
{

	using System;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;

	/// <summary>
	/// A ComboBox used to select a User.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class UserComboBox : ComboBox
	{

		/// <summary>
		/// An ordered view of the users.
		/// </summary>
		static ListCollectionView listCollectionView;

		/// <summary>
		/// The collection of users in this control.
		/// </summary>
		static UserCollection userCollection = new UserCollection();

		/// <summary>
		/// Identifies the UserId dependency property.
		/// </summary>
		public static readonly DependencyProperty UserIdProperty = DependencyProperty.Register(
			"UserId",
			typeof(Guid),
			typeof(UserComboBox),
			new FrameworkPropertyMetadata(Guid.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(UserComboBox.OnUserIdPropertyChanged)));

		/// <summary>
		/// Identifies the UserItems dependency property key.
		/// </summary>
		static readonly DependencyPropertyKey userItemsPropertyKey = DependencyProperty.RegisterReadOnly(
				"UserItems",
				typeof(ListCollectionView),
				typeof(UserComboBox),
				new FrameworkPropertyMetadata());

		/// <summary>
		/// Identifies the UserItems dependency property.
		/// </summary>
		public readonly static DependencyProperty UserItemsProperty;

		/// <summary>
		/// Initialize the UserBox class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static UserComboBox()
		{

			// Initialize the read-only properties using their keys.
			UserComboBox.UserItemsProperty = UserComboBox.userItemsPropertyKey.DependencyProperty;

			// This creates an association with an implicit default style in the themes.
			UserComboBox.DefaultStyleKeyProperty.OverrideMetadata(typeof(UserComboBox), new FrameworkPropertyMetadata(typeof(UserComboBox)));

			// This will order the items in the drop down list alphabetically.
			UserComboBox.listCollectionView = CollectionViewSource.GetDefaultView(UserComboBox.userCollection) as ListCollectionView;
			UserComboBox.listCollectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

		}

		/// <summary>
		/// Initializes a new instance of the UserBox class.
		/// </summary>
		public UserComboBox()
		{

			// Provide the combobox with a set of static user items.  The collection of user items can be changed by the database and those changes will be
			// reflected in the list, but there is only one list needed for all the controls that consume this combobox, so we use a static list, which is not to
			// say the items in the list are static.
			this.SetValue(UserComboBox.userItemsPropertyKey, UserComboBox.listCollectionView);

			// Normally this would be a Metadata override, but the logic (which I considered flawed) for this property watches for changes to the default value to 
			// set the path to the property.  Therefore, overriding a default metadata will not actually set the property because the property never changes.  The 
			// same is true of the SelectedValuePath property.
			this.SetValue(UserComboBox.DisplayMemberPathProperty, "Name");
			this.SetValue(UserComboBox.SelectedValuePathProperty, "UserId");

			// This sets up a default binding for the ItemsSource property to the collection of users.  It can be overridden in the XAML, but practically there is
			// no reason to ever override the default collection.
			Binding itemsSourceBinding = new Binding("UserItems");
 			itemsSourceBinding.RelativeSource = new RelativeSource(RelativeSourceMode.Self);
			BindingOperations.SetBinding(this, UserComboBox.ItemsSourceProperty, itemsSourceBinding);

			// This allows us to listen to changes to the underlying list and select a default item when data finally appears.
			UserComboBox.userCollection.CollectionChanged += this.OnUserCollectionChanged;

		}

		/// <summary>
		/// Responds to a ComboBox selection change by raising a SelectionChanged event. 
		/// </summary>
		/// <param name="e">Provides data for SelectionChangedEventArgs.</param>
		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{

			// Allow the base class to handle the change.
			base.OnSelectionChanged(e);

			// This reconciles the UserId property to the SelectedValue property.
			this.UserId = (Guid)this.SelectedValue;

		}

		/// <summary>
		/// Handles a change to the collection of users.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event data.</param>
		void OnUserCollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
		{

			// This will select the first item in the list when there are no selections and the list of users has been udpated.
			if (e.Action == NotifyCollectionChangedAction.Add)
				if (this.SelectedIndex == -1)
					this.SelectedIndex = 0;

		}

		/// <summary>
		/// Handles a change to the UserId property.
		/// </summary>
		/// <param name="dependencyObject">The Object that originated the event.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event arguments.</param>
		static void OnUserIdPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will select the item with the given user id (if it exists in the collection of items).
			UserComboBox userComboBox = dependencyObject as UserComboBox;
			Guid userId = (Guid)dependencyPropertyChangedEventArgs.NewValue;
			userComboBox.SelectedValue = userId;

		}

		/// <summary>
		/// Gets or sets the currently selected user.
		/// </summary>
		public Guid UserId
		{
			get
			{
				return (Guid)this.GetValue(UserComboBox.UserIdProperty);
			}
			set
			{
				this.SetValue(UserComboBox.UserIdProperty, value);
			}
		}

		/// <summary> 
		/// Gets the read-only collection of user items.
		/// </summary>
		public ListCollectionView UserItems
		{
			get
			{
				return this.GetValue(UserComboBox.UserItemsProperty) as ListCollectionView;
			}
		}

	}

}
