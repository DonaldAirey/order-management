namespace Teraque.Windows.Controls
{

	using System;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows;

	/// <summary>
	/// A list box that is capable of providing several different views of the same data.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ListBoxView : ListView
	{

		/// <summary>
		/// Indicates that a deferred set focus scope operation should select the next element.
		/// </summary>
		Boolean focusNextElement;

		/// <summary>
		/// This acts as a command to select the first item processed through the ItemContainerGenerator.
		/// </summary>
		Boolean selectNextElement;

		/// <summary>
		/// Removes all templates, styles, and bindings for the object that is displayed as a ListViewItem.
		/// </summary>
		/// <param name="element">The ListViewItem container to clear.</param>
		/// <param name="item">The object that the ListViewItem contains.</param>
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{

			// This is the opposite action of adding an item to the container.  When an element is removed from the view, if it had the focus we want to move the
			// focus to the view.  A hidden element that has the keyboard focus is not a happy interface.
			DependencyObject focusScope = FocusManager.GetFocusScope(this);
			IInputElement focusedElement = FocusManager.GetFocusedElement(focusScope);
			if (element == focusedElement)
				FocusManager.SetFocusedElement(focusScope, null);

			// Allow the base class to finish cleaning up the element.
			base.ClearContainerForItemOverride(element, item);

		}

		/// <summary>
		/// Invoked when a GotKeyboardFocus attached event reaches this element in its route.
		/// </summary>
		/// <param name="e">The KeyboardFocusChangedEventArgs that contains the event data.</param>
		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{

			// Validate parameters
			if (e == null)
				throw new ArgumentNullException("e");

			// On initialization, this control will typically get the focus before it has any items to display.  This makes the job harder because typically we want
			// to give the focus to the first item in an ItemsControl when that ItemsControl is given the focus.  The solution is to 'defer' the focus change until
			// such time as the control actually has elements that can get the keyboard focus.
			if (e.NewFocus == this)
				this.focusNextElement = true;

		}

		/// <summary>
		/// Called when the source of an item in a selector changes.
		/// </summary>
		/// <param name="oldValue">Old value of the source.</param>
		/// <param name="newValue">New value of the source.</param>
		protected override void OnItemsSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue)
		{

			// This will act as a command to select the first item generated through the ItemContainerGenerator.  When a new list is presented to this view the
			// first item in the list will be selected automatically.  This insures that at least one item in the view is always selected on initialization.
			this.selectNextElement = true;

			// Allow the base class to handle the rest of the event.
			base.OnItemsSourceChanged(oldValue, newValue);

		}

		/// <summary>
		/// Sets the styles, templates, and bindings for a ListViewItem.
		/// </summary>
		/// <param name="element">An object that is a ListViewItem or that can be converted into one.</param>
		/// <param name="item">The object to use to create the ListViewItem.</param>
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{

			// Extract the specific arguments from the generic event arguments.
			ListBoxItem listBoxItem = element as ListBoxItem;

			// This will select the first item in a new items source.
			if (this.selectNextElement)
			{
				this.selectNextElement = false;
				listBoxItem.IsSelected = true;
			}

			// If the focus operation has been deferred, then now is the time to move the focus into the newly created item.  This is the first and only place where
			// we get a notification of a new child element so it's the first chance we have to give it the focus.
			if (this.IsFocused && this.focusNextElement)
			{
				this.focusNextElement = false;
				DependencyObject focusScope = FocusManager.GetFocusScope(this);
				FocusManager.SetFocusedElement(focusScope, listBoxItem);
			}

			// Allow the base class to finish preparing the item.
			base.PrepareContainerForItemOverride(element, item);

		}

	}

}
