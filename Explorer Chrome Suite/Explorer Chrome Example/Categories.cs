namespace Teraque.ExplorerChromeExample
{

	using System;
	using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// A collection of hierarchical categories.
	/// </summary>
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	class Categories : CategoryCollection
	{

		/// <summary>
		/// Initializes a new instance of the Categories class.
		/// </summary>
		public Categories()
		{

			// This will create a hierarchical data structure of categories.  The root elements are the ones without any parents.  The root elements will
			// recursively construct a list of descendants before they are added to the top level of the tree.
			foreach (DataSet.RootRow rootRow in DataModel.DataSet.Root)
				if (rootRow.GetTreeRowsByFK_Root_Tree_RootIdChildId().Length == 0)
					this.Add(new CategoryCollection(null, rootRow));
		}

	}

}
