namespace Teraque.Windows
{

	using System;
	using System.Windows.Controls;

    /// <summary>
	/// Handles the Undo/Redo logic for the TextBox classes of controls.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class RichTextBoxUndo : TextBoxBaseUndo
	{

		/// <summary>
		/// Creates an object that manages the Undo/Redo logic that is particular to this control.
		/// </summary>
		/// <param name="undoManager">An object that coordinates all the undo/redo actions in a scope.</param>
		public RichTextBoxUndo(UndoManager undoManager) : base(undoManager) { }

		/// <summary>
		/// Handles changes to the content of the RichTextBox family of controls.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event arguments.</param>
		public override void TextChangedHandler(Object sender, TextChangedEventArgs e)
		{

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (e == null)
				throw new ArgumentNullException("e");

			// The RichTextBox generates its own series of state changes for handling the Undo/Redo logic.  The main idea is to integrate the local operations into
			// the UndoScope so that they all appear to be one long sequence of actions that are captured in the Undo/Redo stacks.
			RichTextBox richTextBox = sender as RichTextBox;
			if (richTextBox != null)
			{

				// Only user initiated operations will modify the Undo/Redo stacks.
				switch (e.UndoAction)
				{
				case UndoAction.Redo:

					// Undoing a 'Redo' is handled by undoing the operation again.  This balances out the stack when doing 
					// multiple Undo/Redo commands.
					this.UndoManager.UndoStack.Push(new CommandArgumentPair(this.Undo, richTextBox));
					break;

				case UndoAction.Undo:

					// This allows the operation that was just 'undone' to be 'redone' by pushing it onto a stack.  The a
					// high-level 'Redo' command is handled by the root control in this scope, this command will be popped off 
					// the stack in the order it was 'Undone' and the original content will be restored.
					this.UndoManager.RedoStack.Push(new CommandArgumentPair(this.Redo, richTextBox));
					break;

				case UndoAction.Merge:

					// Merging is a way of combining serial changes to the text content.  Typing a sequence of two or more
					// characters doesn't need to push each character on the stack.  This combines similar sequences of actions
					// into a single action on the stack.
					CommandArgumentPair commandArgumentPair = this.UndoManager.UndoStack.Peek();
					RichTextBox currentRichTextBox = (RichTextBox)commandArgumentPair.GenericEventArgs.Arguments[0];
					if (currentRichTextBox != richTextBox)
						this.UndoManager.UndoStack.Push(new CommandArgumentPair(this.Undo, richTextBox));
					break;

				case UndoAction.Create:

					// This action creates an action on the stack.  Similar actions will be merged into this item on the stack 
					// and can be undone with a single call to 'Undo'.
					this.UndoManager.UndoStack.Push(new CommandArgumentPair(this.Undo, richTextBox));
					break;

				}

			}

		}

	}

}
