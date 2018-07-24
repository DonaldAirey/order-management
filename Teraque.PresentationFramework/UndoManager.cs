namespace Teraque.Windows
{

	using System;
	using System.Collections.Generic;
    using System.Windows;
	using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
	/// Manages the Undo/Redo operations for a single control, group of controls or an entire application.
	/// </summary>
	public class UndoManager
	{

		/// <summary>
		/// Identifies the UndoManager dependency property.
		/// </summary>
		public static readonly DependencyProperty UndoManagerProperty = DependencyProperty.RegisterAttached(
				"UndoManager",
				typeof(UndoManager),
				typeof(UndoManager),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnUndoManagerChanged)));

		/// <summary>
		/// Identifies the UndoScope dependency property.
		/// </summary>
		public static readonly DependencyProperty UndoScopeProperty = DependencyProperty.RegisterAttached(
				"UndoScope",
				typeof(UndoManager),
				typeof(UndoManager),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnUndoScopeChanged)));

		/// <summary>
		/// Tracks the focus.
		/// </summary>
		FocusUndo focusUndo;

		/// <summary>
		/// Maps the data type to an Undo/Redo operation.
		/// </summary>
		Dictionary<Type, IUndo> typeUndoMapField;

		/// <summary>
		/// The Undo stack.
		/// </summary>
		Stack<CommandArgumentPair> undoStackField;

		/// <summary>
		/// The Redo stack.
		/// </summary>
		Stack<CommandArgumentPair> redoStackField;

		/// <summary>
		/// Creates a manager for the undo/redo operations.
		/// </summary>
		public UndoManager()
		{

			// This stacks are used to store the undo and redo operations.
			this.undoStackField = new Stack<CommandArgumentPair>();
			this.redoStackField = new Stack<CommandArgumentPair>();

			// This Undo class handles the focus changes and is associated with the root element of the scope.  It watches for all focus changes and records
			// them on the undo stack.
			this.focusUndo = new FocusUndo(this);

			// This creates a mapping between the type of control and the logic that handles the undo and redo operations for that control.
			this.typeUndoMapField = new Dictionary<Type, IUndo>();
			this.typeUndoMapField.Add(typeof(TextBox), new TextBoxUndo(this));
			this.typeUndoMapField.Add(typeof(RichTextBox), new RichTextBoxUndo(this));
			this.typeUndoMapField.Add(typeof(ComboBox), new SelectorUndo(this));
			this.typeUndoMapField.Add(typeof(ListBox), new SelectorUndo(this));
			this.typeUndoMapField.Add(typeof(TabControl), new SelectorUndo(this));
			this.typeUndoMapField.Add(typeof(CheckBox), new ToggleButtonUndo(this));
			this.typeUndoMapField.Add(typeof(RadioButton), new ToggleButtonUndo(this));

		}

		/// <summary>
		/// Gets the undo type map.
		/// </summary>
		public Dictionary<Type, IUndo> TypeUndoMap
		{
			get
			{
				return this.typeUndoMapField;
			}
		}

		/// <summary>
		/// Gets the Undo Stack.
		/// </summary>
		public Stack<CommandArgumentPair> UndoStack
		{
			get
			{
				return this.undoStackField;
			}
		}

		/// <summary>
		/// Gets the Redo Stack.
		/// </summary>
		public Stack<CommandArgumentPair> RedoStack
		{
			get
			{
				return this.redoStackField;
			}
		}
	
		/// <summary>
		/// Set accessor for the UndoScope attached property.
		/// </summary>
		/// <param name="dependencyObject">The target object for this attached property.</param>
		/// <param name="undoManager">The value for the attached property.</param>
		public static void SetUndoScope(DependencyObject dependencyObject, UndoManager undoManager)
		{

			// Validate the parameters.
			if (dependencyObject == null)
				throw new ArgumentNullException("dependencyObject");
			if (undoManager == null)
				throw new ArgumentNullException("undoManager");

			// This method shouldn't do anything more than set the attached property as XAML bypasses this accessor.
			dependencyObject.SetValue(UndoScopeProperty, undoManager);

		}

		/// <summary>
		/// Get accessor for the UndoScope attached property.
		/// </summary>
		/// <param name="dependencyObject">The target object for this attached property.</param>
		/// <param name="undoManager">The value for the attached property.</param>
		public static UndoManager GetUndoScope(DependencyObject dependencyObject)
		{

			// Validate the parameters.
			if (dependencyObject == null)
				throw new ArgumentNullException("dependencyObject");

			// This method shouldn't do anything more than get the attached property as XAML bypasses this accessor.
			return dependencyObject.GetValue(UndoScopeProperty) as UndoManager;

		}

		/// <summary>
		/// Handles a change to the undo manager for a given dependency object.
		/// </summary>
		/// <param name="dependencyObject">The target object for this attached property.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event parameters.</param>
		static void OnUndoScopeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Validate the parameters.
			if (dependencyObject == null)
				throw new ArgumentNullException("dependencyObject");
			if (dependencyPropertyChangedEventArgs == null)
				throw new ArgumentNullException("dependencyPropertyChangedEventArgs");

			// Extract the specific arguments from the generic ones.
			UndoManager oldUndoManager = dependencyPropertyChangedEventArgs.OldValue as UndoManager;
			UndoManager newUndoManager = dependencyPropertyChangedEventArgs.NewValue as UndoManager;
			FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
			FrameworkContentElement frameworkContentElement = dependencyObject as FrameworkContentElement;

			// This is the manager that was previously associated with the root of the Undo Scope.
			if (oldUndoManager != null)
			{

				// This will remove a handler for the mouse focus events.
				oldUndoManager.focusUndo.Unregister(dependencyObject);

				// This will remove the UndoManager from all the child controls.
				dependencyObject.ClearValue(UndoManager.UndoManagerProperty);

				// This will remove the handlers for the undo/redo operations for the given undo manager.
				if (frameworkElement != null)
					frameworkElement.RemoveHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(oldUndoManager.OnCommand));

				// This will remove the handlers for the undo/redo operations for the given undo manager.
				if (frameworkContentElement != null)
					frameworkContentElement.RemoveHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(oldUndoManager.OnCommand));

			}

			// This will add the high level handlers for the root node in the UndoManager scope.
			if (newUndoManager != null)
			{

				// This will add a handler for the mouse focus events.
				newUndoManager.focusUndo.Register(dependencyObject);

				// This adds the Undo Manager to all the child windows in the scope of this user interface element which forms
				// the root of the scope.
				dependencyObject.SetValue(UndoManager.UndoManagerProperty, newUndoManager);

				// This will add the handlers for the undo/redo operations for the given undo manager.
				if (frameworkElement != null)
					frameworkElement.AddHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(newUndoManager.OnCommand), true);

				//// This will add the handlers for the undo/redo operations for the given undo manager.
				if (frameworkContentElement != null)
					frameworkContentElement.AddHandler(CommandManager.PreviewExecutedEvent,	new ExecutedRoutedEventHandler(newUndoManager.OnCommand), true);

			}

		}

		/// <summary>
		/// Handles a change to the undo manager for a given dependency object.
		/// </summary>
		/// <param name="dependencyObject">The target object for this attached property.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event parameters.</param>
		static void OnUndoManagerChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will remove the previous UndoManager from the object.  Note that an old UndoManager must be removed from the class before a new one can be
			// added, otherwise the events will still be connected to the old manager after the new one is set.
			if (dependencyPropertyChangedEventArgs.OldValue is UndoManager)
			{

				// This is the 'UnDo' manager for the old scope.
				UndoManager undoManager = dependencyPropertyChangedEventArgs.OldValue as UndoManager;

				// There is also a specific handler for the particulars of a given class (TextBox, CheckBox, etc.).  To save time, a mapping is used to find an
				// interface to the methods that handle the particulars of each class.  An entry must be made in this map for any new user interface elements
				// not covered by default by this class.
				IUndo iUndo;
				if (undoManager.typeUndoMapField.TryGetValue(dependencyObject.GetType(), out iUndo))
					iUndo.Unregister(dependencyObject);

			}

			// This will connect a User Interface Element to an UndoManager.  The UndoManager will be hooked into the elements events and when the control's
			// content is changed, entries will be made in a stack that keeps track of the operations.  This stack is integrated with all the other elements in
			// the scope of this UndoManager so that actions are 'Undone' in the order they were added to the UndoManager in this scope.
			if (dependencyPropertyChangedEventArgs.NewValue is UndoManager)
			{

				// This is the 'UnDo' manager for the new scope.
				UndoManager undoManager = dependencyPropertyChangedEventArgs.NewValue as UndoManager;

				// There is also a specific handler for the particulars of a given class (TextBox, CheckBox, etc.).  To save time, a mapping is used to find an
				// interface to the methods that handle the particulars of each class.  An entry must be made in this map for any new user interface elements
				// not covered by default by this class.
				IUndo iUndo;
				if (undoManager.typeUndoMapField.TryGetValue(dependencyObject.GetType(), out iUndo))
					iUndo.Register(dependencyObject);

			}

		}

		/// <summary>
		/// Clears the Undo scope of all actions.
		/// </summary>
		public void Clear()
		{

			// Clear the stacks.
			this.undoStackField.Clear();
			this.redoStackField.Clear();

		}

		/// <summary>
		/// Handles a command.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="executedRoutedEventArgs">The event arguments.</param>
		internal void OnCommand(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
		{

			// The undo command will pop a generic handler off the stack that will roll back the last operation.
			if (executedRoutedEventArgs.Command == ApplicationCommands.Undo)
			{

				// This indicates that the command was handled and no further action is required.
				executedRoutedEventArgs.Handled = true;

				// Keep popping operations off the stack until the stack is exhausted.  The 'undo' handler is specified by the specific control handlers when
				// operation was pushed onto the stack.  Each handler has the information needed to 'undo' the specific action that pushed this object onto the
				// stack.
				if (this.undoStackField.Count > 0)
				{
					CommandArgumentPair commandArgumentPair = this.undoStackField.Pop();
					commandArgumentPair.GenericEventHandler(this, commandArgumentPair.GenericEventArgs);
				}

			}

			// the redo command will pop a generic handler off the stack that will roll forward the last operation.
			if (executedRoutedEventArgs.Command == ApplicationCommands.Redo)
			{

				// This indicates that the command was handled and no further action is required.
				executedRoutedEventArgs.Handled = true;

				// Keep popping operation off the stack until the stack is exhausted.  The 'redo' handler is specified by the specific control handler when the
				// the operation was pushed onto the stack.
				if (this.redoStackField.Count > 0)
				{
					CommandArgumentPair commandArgumentPair = this.redoStackField.Pop();
					commandArgumentPair.GenericEventHandler(this, commandArgumentPair.GenericEventArgs);
				}

			}

		}

	}

}
