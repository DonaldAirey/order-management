namespace Teraque.Windows.Controls
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// A handler for redoing/undoing a change to a DependencyProperty.
	/// </summary>
	/// <param name="sender">The object that orignated the event.</param>
	/// <param name="undoPropertyChangedEventArgs">The event data.</param>
	[SuppressMessage ("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances")]
	public delegate void UndoPropertyChangedEventHandler(Object sender, UndoPropertyChangedEventArgs undoPropertyChangedEventArgs);

	/// <summary>
	/// The event arguments for undoing/redoing a property change.
	/// </summary>
	public class UndoPropertyChangedEventArgs : RoutedEventArgs
	{

		/// <summary>
		/// The dependency property to Undo/Redo.
		/// </summary>
		DependencyProperty dependencyPropertyField;

		/// <summary>
		/// The new value of the property.
		/// </summary>
		Object newValueField;

		/// <summary>
		/// The old value of the property.
		/// </summary>
		Object oldValueField;

		/// <summary>
		/// The undo action of the property.
		/// </summary>
		UndoAction undoActionField;

		/// <summary>
		/// Initializes a new instance of the UndoPropertyChangedEventArgs class.
		/// </summary>
		/// <param name="routedEvent">The routed event that caused the property to change.</param>
		/// <param name="undoAction">The undo/redo action of the property.</param>
		/// <param name="dependencyProperty">The DependencyProperty that has changed.</param>
		/// <param name="oldValue">The old value of the property.</param>
		/// <param name="newValue">The new value of the property.</param>
		public UndoPropertyChangedEventArgs(
			RoutedEvent routedEvent,
			UndoAction undoAction,
			DependencyProperty dependencyProperty,
			Object oldValue,
			Object newValue)
			: base(routedEvent)
		{

			// Initialize the object
			this.undoActionField = undoAction;
			this.dependencyPropertyField = dependencyProperty;
			this.oldValueField = oldValue;
			this.newValueField = newValue;

		}

		/// <summary>
		/// Gets the DependencyProperty that is to be undone/redone.
		/// </summary>
		public DependencyProperty DependencyProperty
		{
			get
			{
				return this.dependencyPropertyField;
			}
		}

		/// <summary>
		/// Gets the new value of the property.
		/// </summary>
		public Object NewValue
		{
			get
			{
				return this.newValueField;
			}
		}

		/// <summary>
		/// Gets the old value of the property.
		/// </summary>
		public Object OldValue
		{
			get
			{
				return this.oldValueField;
			}
		}

		/// <summary>
		/// Gets the undo action of the property.
		/// </summary>
		public UndoAction UndoAction
		{
			get
			{
				return this.undoActionField;
			}
		}

	}

}
