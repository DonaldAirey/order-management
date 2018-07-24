namespace Teraque.Windows
{

	using System;
	using System.Windows;
	using System.Windows.Navigation;
	using Teraque.Windows.Navigation;

	/// <summary>
	/// The saves state of an explorer page.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[Serializable]
	class ExplorerContentState : CustomContentState
	{

		/// <summary>
		/// The character used to separate elements of the path.
		/// </summary>
		const Char separatorCharacter = '/';

		/// <summary>
		/// The name for the content that is stored in navigation history.
		/// </summary>
		String journalEntryName;

		/// <summary>
		/// Initializes a new instance of the ExplorerContentState class.
		/// </summary>
		/// <param name="originalUri">The original URI that identified this content (before mapping).</param>
		public ExplorerContentState(Uri originalUri)
		{

			// Initialize the object.
			this.OriginalUri = originalUri;

			// Only the leaf part of the entire path will be displayed in the journal.
			String[] pathParts = this.OriginalUri.OriginalString.Split(separatorCharacter);
			this.journalEntryName = pathParts[pathParts.Length - 1];

		}

		/// <summary>
		/// The URI address of the page.
		/// </summary>
		public Uri OriginalUri { set; get; }

		/// <summary>
		/// Initializes a new instance of the CustomContentState class.
		/// </summary>
		/// <param name="navigationService">The NavigationService owned by the navigator responsible for the content to which this CustomContentState is being
		/// applied.</param>
		/// <param name="mode">A NavigationMode that specifies how the content to which the CustomContentState is being applied was navigated to.</param>
		public override void Replay(NavigationService navigationService, NavigationMode mode)
		{

			// Validate the argument.
			if (navigationService == null)
                throw new ArgumentNullException("navigationService");

			// Pages are not stored in memory by default.  Only a journal entry containing the path and other items used to reconstitute the page are stored.  When
			// we 'Replay' a page we need to retrieve the original data context and (re)attach it to the page.
			DependencyObject content = navigationService.Content as DependencyObject;
			ExplorerWindow.SetOriginalUri(content, this.OriginalUri);

		}

		/// <summary>
		/// The name for the content that is stored in navigation history.
		/// </summary>
		public override String JournalEntryName
		{
			get
			{
				return this.journalEntryName;
			}
		}

	}

}
