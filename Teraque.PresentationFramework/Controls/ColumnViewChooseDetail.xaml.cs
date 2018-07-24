namespace Teraque.Windows.Controls
{

	using System;
	using System.Windows;
	using System.Windows.Controls;
	using Teraque.Windows.Controls;

	/// <summary>
	/// Interaction logic for ColumnViewChooseDetail.xaml
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class ColumnViewChooseDetail : Window
	{

		/// <summary>
		/// Initializes a new instance of the ColumnViewChooseDetail class.
		/// </summary>
		public ColumnViewChooseDetail()
		{

			// The IDE managed resources are initialized here.
			InitializeComponent();

			// These events control the appearance of the dialog.
			this.ListBox.SelectionChanged += this.OnListBoxSelectionChanged;
			this.WidthTextBox.TextChanged += this.OnWidthTextBoxTextChanged;

			// This will initialize the dialog once it's visible.
			this.Loaded += this.OnLoaded;

		}

		/// <summary>
		/// Handles the Cancel button click.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		private void OnCancelButtonClick(Object sender, RoutedEventArgs e)
		{

			// The dialog was rejected.
			this.DialogResult = true;

		}

		/// <summary>
		/// Handles a change to selection in the list box.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnListBoxSelectionChanged(Object sender, SelectionChangedEventArgs e)
		{

			// This will reconcile the 'Width' dialog box to the ColumnDescription item chosen in the list box.
			ListBox listBox = sender as ListBox;
			ColumnDescription columnDefinition = listBox.SelectedItem as ColumnDescription;
			this.WidthTextBox.Text = columnDefinition.Width.ToString();

		}

		/// <summary>
		/// Occurs when the element is laid out, rendered, and ready for interaction.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnLoaded(Object sender, RoutedEventArgs e)
		{

			// This will select the first item when the dialog opens.  This will kick the dialog into action by seeding it with an item.
			this.ListBox.SelectedIndex = 0;

		}

		/// <summary>
		/// Handles the OK button click.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		private void OnOkButtonClick(Object sender, RoutedEventArgs e)
		{

			// The dialog was accepted.
			this.DialogResult = true;

		}

		/// <summary>
		/// Handles a change to the width of a column.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		void OnWidthTextBoxTextChanged(Object sender, TextChangedEventArgs e)
		{
		}

	}

}
