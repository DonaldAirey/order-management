namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// Represents a control that displays a list of data items in a viewport.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ListViewport : ListView
	{

		/// <summary>
		/// This is used to recycle the containers that appear in the Viewport.
		/// </summary>
		Queue<ListViewItem> listViewItemQueue = new Queue<ListViewItem>();

		/// <summary>
		/// Initializes a new instance of the ListViewport class.
		/// </summary>
		static ListViewport()
		{

			// The default ItemsPanelTemplate for this control is the ScrollingStackPanel which is a stripped down version of the VirtualizingStackPanel that is 
			// designed specifically for scrolling a viewport.
			ItemsPanelTemplate defaultValue = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(ViewportPanel)));
			defaultValue.Seal();
			ItemsControl.ItemsPanelProperty.OverrideMetadata(typeof(ListViewport), new FrameworkPropertyMetadata(defaultValue));
 
		}

		/// <summary>
		/// Removes all templates, styles, and bindings for the object that is displayed as a ListViewItem.
		/// </summary>
		/// <param name="element">The ListViewItem container to clear.</param>
		/// <param name="item">The object that the ListViewItem contains.</param>
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{

			// When the container releases an item we'll reclaim it.  These controls take a lot of time to generate and there is just no sense in throwing them away
			// when simply binding them to a new DataContext will make the perfectly usable.
			this.listViewItemQueue.Enqueue(element as ListViewItem);

		}

		/// <summary>
		/// Creates and returns a new ListViewItem container.
		/// </summary>
		/// <returns>A new ListViewItem control.</returns>
		protected override DependencyObject GetContainerForItemOverride()
		{

			// This will recycle the containers.  If a container is available in the queue, then use it.  Otherwise ask the base class to create a new container.
			// When the ListViewItem is removed from the panel, we will collect it in a queue rather than let it be recovered by the garbage collection.  The
			// only real problem to solve then is how to be informed when the container is no longer needed.  For this we attach ourselves to an event that will
			// notify us that the parent has changed and recover the container when it no longer has a parent.
			return listViewItemQueue.Count == 0 ? new ListViewItem() : listViewItemQueue.Dequeue();

		}

	}

}
