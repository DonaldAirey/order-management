namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Controls.Primitives;
	using System.Windows.Markup;
	using Teraque.Properties;

	/// <summary>
	/// Defines an area where breadcrumb items are arranged horizontally with an overflow control for items that don't fit.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class BreadcrumbPanel : GadgetPanel
	{

		/// <summary>
		/// Measures the child elements of a Teraque.BreadcrumbPanel prior to arranging them during the ArrangeOverride pass.
		/// </summary>
		/// <param name="availableSize">A maximum Size to not exceed.</param>
		/// <returns>A Size that represents the element size you want.</returns>
		protected override Size MeasureOverride(Size availableSize)
		{

			// This panel can provide no useful measurement or layout if it isn't an items host.
			if (!this.IsItemsHost)
				return availableSize;

			// This is the collection that is associated with the user interface in this panel.  Items that can fit in the panel or have been explicitly set to be
			// visible will be added to this collection.
			this.UIElementCollection.Clear();

			// This is the only place where the decision about what items actually appear in the panel is made.  An event associated with this control is invoked
			// when visible items are added or removed.  Since the GadgetBar will go through several passes of adding and removing items before it determines what
			// fits and what doesn't, a simple ObservableList-type trigger wouldn't work because there'd be too many false signals.  To provide the required
			// functionality, a copy of the list is made here before the measurement and will be used later to determine if the final set of items has changed.
			List<Object> originalList = new List<Object>();
			foreach (Object item in this.GadgetBar.PanelItems)
				originalList.Add(item);

			// This observable list maintains the logical relationship with the parent menu.  When items are added to or removed from this list a trigger in the
			// GadgetBar will add them to or remove them from the logical children of that control.
			this.GadgetBar.PanelItems.Clear();

			// This observable list maintains the logical relationship with the overflow menu.  These relationships must be managed manually.  When an item is added
			// to the overflow panel it must be removed from the main panel.  It must also be added at the exact same time or orphaned MenuItems will generate
			// messages about data binding failures.
			this.GadgetBar.OverflowItems.Clear();

			// After each pass through the measuring the child controls the logical items will be shifted around.  Some of them will be moved to the overflow panel
			// and some will remain in the main panel.  This confuses the items container generator when it comes time to creating the containers.  The original 
			// logical relationship of the items is restored here in order to set things right for the ItemsContainerGenerator.
			ItemsControl itemsControl = this.GadgetBar as ItemsControl;
			foreach (Object item in itemsControl.Items)
				this.GadgetBar.PanelItems.Add(item);

			// The main panel must make a proper measurement of all the child controls to determine if there's enough space.  If there isn't, then the items are 
			// moved out of the main panel and into an overflow panel in a very well prescribed order.  The trouble with this is that items don't move out of their
			// containers very well.  They seem to be broken when another items container tries to use the same item.  In this case, the items container for the
			// items in the main panel are broken when the overflow panel creates menu item containers for them.  There seems to be no mechanism to repair a broken
			// container so they are regenerated here.  Also, recycling doesn't appear to work.  Once a container is broken, apparently it must be discarded so each
			// time through the measure override a new set of containers is generated.
			IItemContainerGenerator iItemContainerGenerator = this.ItemContainerGenerator as ItemContainerGenerator;
			iItemContainerGenerator.RemoveAll();

			// The order of the items as they come out of the generator is used to determine their order in the overflow panel.  This table is used to determine the
			// relative order of items as they are moved to the overflow panel.
			Dictionary<Object, Int32> itemTable = new Dictionary<object, int>();

			// When items are moved out of the main panel and into the overflow panel they keep the same relative order.  The algorithm to do this is a bit tricky
			// as the items are moved in several passes.  The first pass takes the items that are marked to always appear in the overflow panel.  The next pass
			// takes the items from the visible panel that are marked to be moved as needed.
			Int32 itemIndex = 0;
			Int32 overflowIndex = 0;
			Int32 panelIndex = 0;

			// This variable will capture the overflow menu item, if it exists as part of the members of the panel.  The overflow menu item has special properties 
			// in that an item that doesn't fit into the panel will be made children of this item.
			OverflowItem overflowItem = null;

			GadgetBar gadgetBar = VisualTreeExtensions.FindAncestor<GadgetBar>(this) as GadgetBar;
			System.ComponentModel.ICollectionView iCollectionView = System.Windows.Data.CollectionViewSource.GetDefaultView(gadgetBar.ItemsSource);
			iCollectionView.Refresh();

			// This will generate a collection of containers from the items hosted by the parent.  Note that these items are not associated with a user interface
			// yet.  This collection will be split into items that appear on the panel and those available through the overflow control.  The containers must be
			// created each time through the measure override because broken containers can't be repaired or recycled.  When another container has usurped the
			// contained item, the original container is broken.  This happens when an item is moved from the main panel to the overflow panel.
			using (iItemContainerGenerator.StartAt(new GeneratorPosition(-1, 0), GeneratorDirection.Forward))
			{

				// This will generate a new container for each item and determin whether it belongs in the main panel or the overflow panel.
				UIElement uiElement;
				while ((uiElement = iItemContainerGenerator.GenerateNext() as UIElement) != null)
				{

					// This allows the host to make modifications to the container before it is displayed.  For example, a GadgetBar would assign a style to the item
					// container based on the item type.
					iItemContainerGenerator.PrepareItemContainer(uiElement);

					// Overflow menu items have special meaning for the panel.  Any item that doesn't fit into the panel will be added to this item.
                    OverflowItem generatedOverflowItem = uiElement as OverflowItem;
					if (generatedOverflowItem != null)
					{
						overflowItem = generatedOverflowItem;
						continue;
					}

					// This constructs a mapping of the item to its ordinal and is used to determine the relative order of the overflow items as they are added to
					// the overflow panel in multiple passes.
					Object item = this.ItemContainerGenerator.ItemFromContainer(uiElement);
					itemTable.Add(item, itemIndex++);

					// The OverflowMode attached property can be use to force an item into the overflow panel.
					switch (GadgetBar.GetOverflowMode(uiElement))
					{
					case OverflowMode.Always:

						// These items always appear in the overflow panel in the order they were added to the Items property of the GadgetBar.
						this.GadgetBar.PanelItems.Remove(item);

						// Conversely the item removed from the main panel is moved into the overflow panel.  The same item can not be the logical child of two
						// windows at the same time.  The observable lists will take care of removing the child from one parent and giving it to the other.
						this.GadgetBar.OverflowItems.Insert(overflowIndex, uiElement);
						overflowIndex++;

						// Remove the generated item if it has no place in the main panel.
						Int32 containerIndex = this.ItemContainerGenerator.IndexFromContainer(uiElement);
						GeneratorPosition generatorPosition = iItemContainerGenerator.GeneratorPositionFromIndex(containerIndex);
						iItemContainerGenerator.Remove(generatorPosition, 1);

						break;

					default:

						// These items include the ones that will move to the overflow panel if needed and the ones that never move.  Note that the collection of
						// containers is not the same as the collection of items. The collection of items is a logical organization whereas the collection of
						// containers is visual.  This pass is organizing the visual elements. Also note that the overflow button is always part of the 
						// BreadcrumbBar and always appears as the first item.
						this.UIElementCollection.Insert(panelIndex, uiElement);
						panelIndex++;

						break;

					}
				}
			}

			// This will insert the overflow item when it has child items that can be displayed.  The overflow item always appears at the left edge of the
			// BreadcrumbBar when it's visible.  In the case where there is only one element at the root of the directory, the overflow item is hidden because 
			// there is nothing to navigate to in this situation.
			if (overflowItem.Items.Count != 0)
				this.UIElementCollection.Insert(0, overflowItem);

			// This keeps track of how much space in the panel is occupied by the items as they are laid out.
			Size allocatedSize = new Size();

			// This constraint is used to allow the controls to measure themselves out in the direction in which the panel is oriented.  That is, if it has a
			// horizontal orientation then an infinite amount of space is given during the measurement process in this direction.  It allows the controls the
			// calculate their theoretical size.  If the item doesn't fit, it will be removed from the panel and its desired size will be recalculated inside the
			// overflow panel.  If it does fit, then the desired size is the actual size it is given in the panel.
			Size infiniteSize = new Size(Double.PositiveInfinity, availableSize.Height);

			// This pass will measure everything that wants to appear in this panel.  An infinite amount of room is given in the direction in which this panel is
			// oriented so the measurement operation won't be constrained.  Another pass will actually determine if the items fit or not.
			foreach (UIElement uiElement in this.UIElementCollection)
			{
				uiElement.Measure(infiniteSize);
				allocatedSize = new Size(allocatedSize.Width + uiElement.DesiredSize.Width, Math.Max(allocatedSize.Height, uiElement.DesiredSize.Height));
			}

			// This will attempt to make sure that everything can fit into the alloted space.  If there isn't enough room and individual items are willing to be
			// placed into the overflow panel, then they are removed from this panel.  This concept is very important because moving logical children from one 
			// container to another breaks the container and it can't be repaired.  That is why the items must be regenerated each time the panel is measured.  The
			// items are moved in two passes: the items on the near side of the panel are removed before the items on the far side.
			Double availableLength = availableSize.Width;
			Double allocatedLength = allocatedSize.Width;
			if (allocatedLength > availableLength)
			{

				// The calculation of what items can appear in the overflow panel is accomplished in two passes.  The first pass will look at the near-aligned items
				// and move them into the overflow panel starting from the farthest item to the item to the nearest until the items fit in the available space.
				Int32 index = 0;
				while (allocatedLength > availableLength && index >= 0 && index < this.UIElementCollection.Count)
				{

					// This element will be examined to see if it can be removed from the panel when there are too many elements to fit.
					UIElement uiElement = this.UIElementCollection[index];

					// The overflow item is never considered for removal from the main panel.
					if (uiElement is OverflowItem)
					{
						index++;
						continue;
					}

					// This will move the next element in the panel into the overflow panel as needed.  Note that the overflow button is never moved into the
					// overflow panel.
					if (GadgetBar.GetOverflowMode(uiElement) == OverflowMode.AsNeeded)
					{

						// This element will no longer appear on the main panel.  This will remove both the visual and the logical relationship.  If the logical
						// relationship isn't broken then this item can't be added to the overflow panel as an item can have only one logical parent at a time.  If 
						// the proper logical relation isn't made then the containers and the contents won't pick up the proper styles.  This is particularly
						// important for menu items as top level items behave differently than sub-menu items.
						this.UIElementCollection.Remove(uiElement);
						Object item = this.ItemContainerGenerator.ItemFromContainer(uiElement);
						this.GadgetBar.PanelItems.Remove(item);

						// This will provide the housekeeping with the ItemsContainerGenerator by removing containers that aren't needed for this panel.
						Int32 containerIndex = this.ItemContainerGenerator.IndexFromContainer(uiElement);
						GeneratorPosition generatorPosition = iItemContainerGenerator.GeneratorPositionFromIndex(containerIndex);
						iItemContainerGenerator.Remove(generatorPosition, 1);

						// Insert the item into the overflow panel in the same order in which it appears in the panel.
						this.GadgetBar.OverflowItems.Insert(0, item);

						// These act as cursors when ordering the items in the overflow panel.
						overflowIndex++;
						panelIndex--;

						// Adjust the available space by the size of the item that was just removed to the overflow panel.
						allocatedLength -= uiElement.DesiredSize.Width;

					}
					else
					{

						// Consider the next container in the panel.
						index++;

					}

				}

			}

			// This will advise any listeners that the items in the panel have changed.  Since panels are not part of a standard template, this information is
			// normally inaccessible to a parent class except as events bubbled up through the visual tree hierarchy.
			Boolean isEqual = this.GadgetBar.PanelItems.Count == originalList.Count;
			for (Int32 index = 0; isEqual && index < this.GadgetBar.PanelItems.Count; index++)
				isEqual = Object.Equals(this.GadgetBar.PanelItems[index], originalList[index]);
			if (!isEqual)
				this.RaiseEvent(new RoutedEventArgs(GadgetPanel.ItemsChangedEvent, this));

			// This is how much room is needed for the panel.  Note that the maximum height (or width) of the tool panel is determined by all the items whether they
			// appear in the tool panel or the overflow panel.  This one-size-fits-all approach keeps the panel from jumping around as items are added from or
			// removed to the overflow panel.
			return new Size(allocatedLength, allocatedSize.Height);

		}

	}

}
