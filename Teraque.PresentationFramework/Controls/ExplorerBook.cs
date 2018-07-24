namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Reflection;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Controls.Primitives;
	using System.Windows.Input;
	using Teraque.Properties;

	/// <summary>
	/// A page for hosting static (shared) content.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public abstract class ExplorerBook : Page
	{

		/// <summary>
		/// This frame is shared by all instances of this page.
		/// </summary>
		static ExplorerFrame explorerFrame = new ExplorerFrame();

		/// <summary>
		/// The last page to be instantiated.  Used to release the logical relation to the shared frame.
		/// </summary>
		static ExplorerBook lastPage;

		/// <summary>
		/// Initialize the ExplorerBook class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static ExplorerBook()
		{

			// This is a complex control and will manage it's own focus scope.
			FocusManager.IsFocusScopeProperty.OverrideMetadata(typeof(ExplorerBook), new FrameworkPropertyMetadata(true));

		}

		/// <summary>
		/// Initializes a new instance of the ExplorerBook class.
		/// </summary>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		protected ExplorerBook()
		{

			// A shared frame can have only one logical parent at a time.  Since the pages are not destroyed immediately and do not release their content
			// automatically, the previous instance of the ExplorerBook must release the shared content before it can be given to another instance of an
			// ExplorerBook.
			if (ExplorerBook.lastPage != null)
				ExplorerBook.lastPage.Content = null;
			ExplorerBook.lastPage = this;

			// The content of this page is a shared frame which remains more or less static between instances of different pages.  That is, the user can navigate
			// between different pages that show up in the ContentPresenter portion of this frame, but the frame elements remain the same, such as the navigation
			// window and the toolbar.
			this.Content = this.FrameCore;

			// RoutedCommands present a special challenge in an interface based on URI navigation.  Because the visual tree is not fixed when you can navigate to
			// any kind of a page, there is no way to send commands beyond the content window of the outer frame.  For example, instead of an ExplorerBook in the  
			// content window, there could be an HTTP page from a web server.  The main frame then has a limitation that it can't select as a CommandTarget anything
			// that is inside of a page because it has no knowlege of the visual tree inside a page.  However, when an ExplorerBook is the content the class can
			// receive the routed events used for the commanding structure and pass them on to the content of the page because it understands the internal visual
			// tree structure.  These class handlers will subscribe to the routed command events and pass them on when we determine that they are intended for the
			// layers inside.
			this.AddHandler(CommandManager.PreviewCanExecuteEvent, new CanExecuteRoutedEventHandler(this.OnPreviewCanExecute));
			this.AddHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(this.OnPreviewExecuted));

		}

		/// <summary>
		/// Gets the shared frame window to be used with this class of pages.
		/// </summary>
		/// <returns>The shared frame used by all instances of this class.</returns>
		protected virtual ExplorerFrame FrameCore
		{
			get
			{
				return ExplorerBook.explorerFrame;
			}
		}

		/// <summary>
		/// Invoked when a GotKeyboardFocus attached event reaches this element in its route.
		/// </summary>
		/// <param name="e">The KeyboardFocusChangedEventArgs that contains the event data.</param>
		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// The focus is given to the content on initialization.  This allows the parent of this page to set the focus on the page without having to know about
			// the inner structure and who should get the focus once the page has been selected.
			if (e.NewFocus == this)
			{
				DependencyObject focusScope = FocusManager.GetFocusScope(this);
				IInputElement focusedElement = FocusManager.GetFocusedElement(focusScope);
				if (focusedElement == null)
				{
					IInputElement contentElement = this.Content as IInputElement;
					if (contentElement != null)
						FocusManager.SetFocusedElement(focusScope, contentElement);
				}
			}

		}

		/// <summary>
		/// Occurs when a PreviewCanExecute event is raised.
		/// </summary>
		/// <param name="sender">The command target that is invoking the handler.</param>
		/// <param name="e">The event data.</param>
		void OnPreviewCanExecute(Object sender, CanExecuteRoutedEventArgs e)
		{

			// This will act as a relay to pass the 'CanExecute' event on to the content of this control and return the result back to the original source.  In 
			// this way an outer frame window can target the content of a page for routed commands.
			if (e.OriginalSource == this)
			{
				RoutedCommand routedCommand = e.Command as RoutedCommand;
				e.CanExecute = routedCommand.CanExecute(null, this.Content as IInputElement);
				e.Handled = true;
			}

		}

		/// <summary>
		/// Occurs when a PreviewExecuted event is raised.
		/// </summary>
		/// <param name="sender">The command target that is invoking the handler.</param>
		/// <param name="e">The event data.</param>
		void OnPreviewExecuted(Object sender, ExecutedRoutedEventArgs e)
		{

			// This will act as a relay to pass the 'Executed' event on to the content of this control.  In this way an outer frame window can target the content of
			// a page for routed commands.
			if (e.OriginalSource == this)
			{
				RoutedCommand routedCommand = e.Command as RoutedCommand;
				routedCommand.Execute(null, this.Content as IInputElement);
				e.Handled = true;
			}

		}

	}

}
