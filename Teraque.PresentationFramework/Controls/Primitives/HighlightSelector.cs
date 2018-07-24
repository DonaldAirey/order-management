namespace Teraque.Windows.Controls.Primitives
{

	using System;
	using System.Collections.Specialized;
	using System.Windows;
	using System.Windows.Controls.Primitives;
	using System.Windows.Input;

	/// <summary>
	/// Represents a control that allows a user to select a highlight item from among its child elements.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class HighlightSelector : Selector
	{

		/// <summary>
		/// Identifies the Clicked event.
		/// </summary>
		public static readonly RoutedEvent ClickedEvent = EventManager.RegisterRoutedEvent(
			"Clicked",
			RoutingStrategy.Bubble,
			typeof(RoutedEventHandler),
			typeof(HighlightSelector));

		/// <summary>
		/// A weak reference to the item which is highlighted.
		/// </summary>
		WeakReference highlightedReference;

		/// <summary>
		/// Identifies the SelectionWrapping dependency property key.
		/// </summary>
		public static readonly DependencyProperty SelectionWrappingProperty = DependencyProperty.Register(
			"SelectionWrapping",
			typeof(SelectionWrapping),
			typeof(HighlightSelector),
			new FrameworkPropertyMetadata(SelectionWrapping.NoWrap));

		/// <summary>
		/// Identifies the SelectionWrapping event.
		/// </summary>
		public static readonly RoutedEvent SelectionWrappedEvent = EventManager.RegisterRoutedEvent(
			"SelectionWrapping",
			RoutingStrategy.Bubble,
			typeof(RoutedEventHandler),
			typeof(HighlightSelector));

		/// <summary>
		/// Initializes a new instance of the HighlightSelector class.
		/// </summary>
		public HighlightSelector()
		{

			// The highlight needs to be cleared when the collection changes.
			this.ItemContainerGenerator.ItemsChanged += new ItemsChangedEventHandler(OnItemContainerGeneratorItemsChanged);

		}

		/// <summary>
		/// Occurs when the an item in the selector is clicked with the mouse.
		/// </summary>
		public event RoutedEventHandler Clicked
		{
			add
			{
				base.AddHandler(HighlightSelector.ClickedEvent, value);
			}
			remove
			{
				base.RemoveHandler(HighlightSelector.ClickedEvent, value);
			}
		}

		/// <summary>
		/// Occurs when the selection navigation has reached the boundary of the container.
		/// </summary>
		public event RoutedEventHandler SelectionWrapped
		{
			add
			{
				base.AddHandler(HighlightSelector.SelectionWrappedEvent, value);
			}
			remove
			{
				base.RemoveHandler(HighlightSelector.SelectionWrappedEvent, value);
			}
		}

		/// <summary>
		/// The container element that currently has the highlight.
		/// </summary>
		internal HighlightElement HighlightedElement
		{

			get
			{

				// A weak reference is used to allow the element's lifetime to be independent of the selector's lifetime.
				return this.highlightedReference == null ? null : highlightedReference.Target as HighlightElement;

			}

			set
			{

				// Pull the currently highlighted element from the weak reference and remove the highlight if it's still around.
				HighlightElement currentHighlightElement = this.highlightedReference == null || !this.highlightedReference.IsAlive ?
					null :
					this.highlightedReference.Target as HighlightElement;
				if (currentHighlightElement != null)
					currentHighlightElement.SetIsHighlighted(false);

				// Create a new weak reference to the highlighted element and higlight it.  The weak reference will allow it to be destroyed without having to 
				// dispose of this class.
				if (value != null)
				{
					this.highlightedReference = new WeakReference(value);
					value.SetIsHighlighted(true);
				}
				else
				{
					this.highlightedReference = null;
				}

			}

		}

		/// <summary>
		/// Gets or sets the highlighted item.
		/// </summary>
		public Object HighlightedItem
		{

			get
			{
				HighlightElement highlightElement = this.HighlightedElement;
				return highlightElement.Content;
			}

			set
			{
				HighlightElement highlightElement = this.ItemContainerGenerator.ContainerFromItem(value) as HighlightElement;
				if (highlightElement != null)
					this.HighlightedElement = highlightElement;
			}

		}

		/// <summary>
		/// Gets or sets the selected item.
		/// </summary>
		HighlightElement SelectedElement
		{

			get
			{
				return this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) as HighlightElement;
			}

			set
			{

				// Only one item at a time can be selected, so the previous item must be unselected first.
				HighlightElement currentElement = this.SelectedElement;
				if (currentElement != null)
					HighlightSelector.SetIsSelected(currentElement, false);
				HighlightSelector.SetIsSelected(value, true);

			}

		}

		/// <summary>
		/// Gets or sets how the selection navigation will wrap when it gets to the end of it's container.
		/// </summary>
		public SelectionWrapping SelectionWrapping
		{
			get
			{
				return (SelectionWrapping)this.GetValue(HighlightSelector.SelectionWrappingProperty);
			}
			set
			{
				this.SetValue(HighlightSelector.SelectionWrappingProperty, value);
			}
		}

		/// <summary>
		/// Creates or identifies the element that is used to display the given item.
		/// </summary>
		/// <returns>The element that is used to display the given item.</returns>
		protected override DependencyObject GetContainerForItemOverride()
		{

			// Only HighlightElements are displayed in the selector.
			return new HighlightElement();

		}

		/// <summary>
		/// Determines if the specified item is (or is eligible to be) its own container.
		/// </summary>
		/// <param name="item">The item to check.</param>
		/// <returns>true if the item is (or is eligible to be) its own container; otherwise, false.</returns>
		protected override Boolean IsItemItsOwnContainerOverride(Object item)
		{

			// Only HighlightElements can be considered containers for items in this selector.
			return item is HighlightElement;

		}

		/// <summary>
		/// Moves the highlight by a line in the direction specified.
		/// </summary>
		/// <param name="focusNavigationDirection">The direction in which to move the highlight.</param>
		internal void MoveByLine(FocusNavigationDirection focusNavigationDirection)
		{

			// The highlight selector never gets the focus.  Instead, it simulates the focus property with a 'highlighted' property that provides visual feedback to
			// the user of the item that would be selected if they hit the mouse button or enter key.  This allows the edit control to get all the keystrokes with
			// the exception of the few that generate commands for the highlight selector.  One of those commands is to move the selection and highlight by a single
			// line.  This will use the 'PredictFocus' to find the next element in the direction of navigation.  The next item will get the highlight instead of the
			// focus in this paradigm.
			UIElement navigationStart = this.HighlightedElement == null ? (UIElement)this : (UIElement)this.HighlightedElement;
			HighlightElement nextElement = navigationStart.PredictFocus(focusNavigationDirection) as HighlightElement;
			if (nextElement == null)
			{

				// Clearing the selection when wrapping is enabled will trigger that informs the listener that the selection has been cleared.  The listening
				// object can elect what to do.  For example, the listener could choose to move the navigation to the start of the window or clear out some other
				// control that is linked to this selection.  When wrapping is not enabled, the selection will reach the end of the container and just stay there
				// no matter how many times the navigation arrows are pressed.
				if (this.SelectionWrapping == SelectionWrapping.Wrap)
				{
					this.HighlightedElement = null;
					this.OnSelectionWrapped(new RoutedEventArgs(HighlightSelector.SelectionWrappedEvent, this));
				}

			}
			else
			{

				// This will bring the element into view, highlight it and select it.  The selection will trigger an event that the owner window can watch for to
				// take some action, like fill in another control with the selected item.  It is the selection and not the highlight that determine which object is
				// chosen by the user.  So even though visually an item has a highlight, when they choose an action it is the selected item that provides a value
				// from this control.
				nextElement.BringIntoView();
				this.HighlightedElement = nextElement;
				this.SelectedElement = nextElement;

			}

		}

		/// <summary>
		/// Called from the item to indicate that the mouse was released while over an item.
		/// </summary>
		/// <param name="highlightElement">The element that caused the event.</param>
		internal void NotifyHighlightItemSelect(HighlightElement highlightElement)
		{

			// Selecting an item with the mouse will close the highlight drop down and exit out of the editing mode.
			this.SelectedElement = highlightElement;

			// This will bubble the event up the element tree until someone cares enough to act on it.
			this.OnClicked(new RoutedEventArgs(HighlightSelector.ClickedEvent));

		}

		/// <summary>
		/// Handles the ItemsChanged event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="itemsChangedEventArgs">The event data.</param>
		void OnItemContainerGeneratorItemsChanged(Object sender, ItemsChangedEventArgs itemsChangedEventArgs)
		{

			// The highlight is used for relative operations on a static collection and will most likely reference an object that is no longer relevant after the
			// collection has changed.
			this.HighlightedElement = null;

		}

		/// <summary>
		/// Invoked when an unhandled Mouse.MouseMove attached event reaches an element in its route that is derived from this class.
		/// </summary>
		/// <param name="e">The MouseEventArgs that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// This will highlight (and bring comletely into view) the element underneath the mouse.  There is a verion of this in the WPF ComboBox that waits for a
			// MouseEnter event from the element and then tries to figure out if mouse movement caused the event.  This is cumbersome because a MouseEnter event can
			// be caused by the mouse being moved over the element, or the element being scrolled underneath the mouse.  An element being scrolled underneath a 
			// mouse that hasn't moved can cause a false highlighting of the element, so a whole 'nuther set of logic was required to see if the mouse had actually
			// moved. Since this event assumes that the mouse has moved, it is simpler just to find out what element is under the mouse and change the highlight if
			// it wasn't the last thing highlighted.
			DependencyObject dependencyObject = this.InputHitTest(e.GetPosition(this)) as DependencyObject;
			HighlightElement highlightElement = VisualTreeExtensions.FindAncestor<HighlightElement>(dependencyObject);
			if (highlightElement != this.HighlightedElement)
			{
				highlightElement.BringIntoView();
				this.HighlightedElement = highlightElement;
			}

			// Because an intermediate class in the inheritance might implement this method, we recommend that you call the base implementation in your
			// implementation.
			base.OnMouseMove(e);

		}

		/// <summary>
		/// Invoked when an unhandled HighlightSelector.Clicked event reaches this element.  Implement this method to add class handling for this event.
		/// </summary>
		/// <param name="routedEventArgs">The event data.</param>
		protected virtual void OnClicked(RoutedEventArgs routedEventArgs)
		{

			// Send the event up the element tree.
			base.RaiseEvent(routedEventArgs);

		}

		/// <summary>
		/// Invoked when an unhandled HighlightSelector.SelectionWrapped event reaches this element.  Implement this method to add class handling for this event.
		/// </summary>
		/// <param name="routedEventArgs">The event data.</param>
		protected virtual void OnSelectionWrapped(RoutedEventArgs routedEventArgs)
		{

			// Send the event up the element tree.
			base.RaiseEvent(routedEventArgs);

		}

	}

}
