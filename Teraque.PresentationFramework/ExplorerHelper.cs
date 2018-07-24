namespace Teraque.Windows
{

	using System;
	using System.Collections;
	using System.Text;
    using Teraque.Properties;

	/// <summary>
	/// A set of helper methods for the Explorer Hierarchy.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public static class ExplorerHelper
	{

		/// <summary>
		/// The charcter used to separate elements of the path.
		/// </summary>
		const Char separatorCharacter = '/';

		/// <summary>
		/// Find the item in an Explorer Hierarchy having the given path.
		/// </summary>
		/// <param name="collection">The hierarchy to be searched.</param>
		/// <param name="source">The URI to find.</param>
		/// <returns>The IExplorerItem in the given hierarchy that matches the given URI.</returns>
        public static IExplorerItem FindExplorerItem(IExplorerItem iExplorerItem, Uri source)
		{

			// Validate the parameters.
			if (source == null)
				throw new ArgumentNullException("source");

			// This will split the URI into its components and look for them in the hierarchy.
			String[] sourceElements = source.OriginalString.Split(ExplorerHelper.separatorCharacter);
			return sourceElements.Length == 1 ? null : FindExplorerItem(iExplorerItem, sourceElements, 1);

		}

		/// <summary>
		/// Find the item in an Explorer Hierarchy having the given path.
		/// </summary>
		/// <param name="collection">The hierarchy to be searched.</param>
		/// <param name="sourceElements">An array of path elements.</param>
		/// <param name="level">The current depth of the search.</param>
		/// <returns>The IExplorerItem in the given hierarchy that matches the given URI.</returns>
		static IExplorerItem FindExplorerItem(IExplorerItem parentItem, String[] sourceElements, Int32 level)
		{

			// This will dig into the hierarchy level by level checking the current name of the item against the path element at the same level.  When all the path
			// elements have been examined and matched, then we have a match for the given source.
			foreach (IExplorerItem childItem in parentItem.Children)
				if (childItem != null && childItem.Name == sourceElements[level])
					return level == sourceElements.Length - 1 ? childItem : FindExplorerItem(childItem, sourceElements, level + 1);

			// Leaves are not the same as children when it comes to the hierarchy.  Children will appear in the navigator and breadcrumb bar, leaves do not.  
			// However, they are legitimate targets for a search.  Note that there is no need to recurse into a leaf item.
			foreach (IExplorerItem leafItem in parentItem.Leaves)
				if (leafItem.Name == sourceElements[level])
					return leafItem;

			// At this point all the given source URI was not found in the hierarchy.
			return null;

		}

		/// <summary>
		/// Generates a fully qualified path name from the given IExplorerItem.
		/// </summary>
		/// <param name="explorerItem">An item in the hierarchy.</param>
		/// <returns>The fully qualified path name to the given item.</returns>
		public static Uri GenerateSource(IExplorerItem explorerItem)
		{

			// Travel up the hierarchy building a source URI that describes the item as we go.
			StringBuilder stringBuilder = new StringBuilder();
			while (explorerItem.Parent != null)
			{
				stringBuilder.Insert(0, ExplorerHelper.separatorCharacter + explorerItem.Name);
				explorerItem = explorerItem.Parent;
			}

			// This string represents the absolute path to the given item.
			return new Uri(stringBuilder.ToString(), UriKind.RelativeOrAbsolute);

		}

	}

}
