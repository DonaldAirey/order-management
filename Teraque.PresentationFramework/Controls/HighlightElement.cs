namespace Teraque.Windows.Controls
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using Teraque.Windows.Controls.Primitives;

	/// <summary>
	/// Represents a selectable item in a HighlightSelector.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class HighlightElement : ListBoxItem
	{

		/// <summary>
		/// Identifies the IsHighlighted dependency property.
		/// </summary>
		public static readonly DependencyProperty IsHighlightedProperty;

		/// <summary>
		/// Identifies the IsHighlighted dependency property key.
		/// </summary>
		static DependencyPropertyKey isHighlightedPropertyKey = DependencyProperty.RegisterReadOnly(
			"IsHighlighted",
			typeof(Boolean),
			typeof(HighlightElement),
			new FrameworkPropertyMetadata(false));

		/// <summary>
		/// Initializes the HighlightElement class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static HighlightElement()
		{

			// Initialize the read-only properties using their keys.
			HighlightElement.IsHighlightedProperty = HighlightElement.isHighlightedPropertyKey.DependencyProperty;

			// This is important for being able to find the implicit default style in themes.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HighlightElement), new FrameworkPropertyMetadata(typeof(HighlightElement)));

		}

		/// <summary>
		/// Gets a value that indicates whether the item is highlighted.
		/// </summary>
		public Boolean IsHighlighted
		{
			get
			{
				return (Boolean)base.GetValue(IsHighlightedProperty);
			}
		}

		/// <summary>
		/// The HighlightSelector that generated this container.
		/// </summary>
		HighlightSelector ParentHighlightSelector
		{
			get
			{
				return ItemsControl.ItemsControlFromItemContainer(this) as HighlightSelector;
			}
		}

		/// <summary>
		/// Called when the user presses the right mouse button over the ListBoxItem.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// This will prevent the item from generating a focus event.  In a ComboBox-like interface, the focus needs to stay in the
			// edit control so it can catch the user's keystrokes.  The drop down selection box only pretends to have the focus by managing a
			// 'Highlighted' property which takes the place of the 'Focused' property in ItemsControls that get the focus.
			e.Handled = true;

		}

		/// <summary>
		/// Invoked when an unhandled MouseLeftButtonUp routed event reaches an element in its route that is derived from this class.
		/// </summary>
		/// <param name="e">The MouseButtonEventArgs that contains the event data. The event data reports that the left mouse button was released.</param>
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// No more handling is required for this message.
			e.Handled = true;

			// Notify the container of the item that it has been selected.
			HighlightSelector highlightSelector = this.ParentHighlightSelector;
			if (highlightSelector != null)
				highlightSelector.NotifyHighlightItemSelect(this);

		}

		/// <summary>
		/// Sets the IsHighlighted property.
		/// </summary>
		/// <param name="isHighlighted">true to set the highlighting, false to clear it.</param>
		internal void SetIsHighlighted(Boolean isHighlighted)
		{

			// Set the highlighted property.
			this.SetValue(HighlightElement.isHighlightedPropertyKey, isHighlighted);

		}

	}

}