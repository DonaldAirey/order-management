namespace Teraque.Windows
{

	using System;
	using System.Windows.Controls;

	/// <summary>
	/// Used to pass a value and an UndoAction through a command parameter.
	/// </summary>
	public class UndoObject
	{

		/// <summary>
		/// The old value.
		/// </summary>
		Object oldValueField;

		/// <summary>
		/// The new value.
		/// </summary>
		Object newValueField;

		/// <summary>
		/// The undo action.
		/// </summary>
		UndoAction undoActionField;

		/// <summary>
		/// Creates an object that describes both the value and the undo action to a command.
		/// </summary>
		/// <param name="value">The value of the object.</param>
		/// <param name="undoAction">The Undo action.</param>
		public UndoObject(object oldValue, object newValue, UndoAction undoAction)
		{

			// Initialize the object
			this.oldValueField = oldValue;
			this.newValueField = newValue;
			this.undoActionField = undoAction;

		}

		/// <summary>
		/// Gets the old value.
		/// </summary>
		public Object OldValue
		{
			get
			{
				return this.oldValueField;
			}
		}

		/// <summary>
		/// Gets the new value.
		/// </summary>
		public Object NewValue
		{
			get
			{
				return this.newValueField;
			}
		}

		/// <summary>
		/// Gets the undo action.
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
