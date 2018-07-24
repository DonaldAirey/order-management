namespace Teraque.Windows
{

    using System.Windows;

    /// <summary>
	/// Common Undo/Redo class for controls.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public abstract class UndoBase : IUndo
	{

		/// <summary>
		/// The undo manager.
		/// </summary>
		UndoManager undoManager;

		/// <summary>
		/// Initializes a new instance of the UndoManager class.
		/// </summary>
		/// <param name="undoManager"></param>
		protected UndoBase(UndoManager undoManager)
		{

			// Initialize the object.
			this.undoManager = undoManager;

		}

		/// <summary>
		/// Gets the UndoManager.
		/// </summary>
		public UndoManager UndoManager
		{
			get
			{
				return this.undoManager;
			}
		}

		/// <summary>
		/// Registers an object for Undo/Redo.
		/// </summary>
		/// <param name="dependencyObject">The object to register.</param>
		public abstract void Register(DependencyObject dependencyObject);

		/// <summary>
		/// Unregisters an object for Undo/Redo.
		/// </summary>
		/// <param name="dependencyObject">The object to unregister.</param>
		public abstract void Unregister(DependencyObject dependencyObject);

	}

}
