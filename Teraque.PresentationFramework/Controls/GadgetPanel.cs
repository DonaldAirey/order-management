namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Controls.Primitives;
	using System.Windows.Media;
	using System.Windows.Markup;
	using Teraque.Properties;

	/// <summary>
	/// Defines an area where items are arranged horizontally aligned to a near or far side with an overflow control for items that don't fit.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class GadgetPanel : Panel
	{

		/// <summary>
		/// The host of this panel that contains the items that are displayed.
		/// </summary>
		GadgetBar gadgetBar;

		/// <summary>
		/// The items generator for this panel.
		/// </summary>
		ItemContainerGenerator itemContainerGenerator;

		/// <summary>
		/// Identifies the ItemsChanged event.
		/// </summary>
		internal static readonly RoutedEvent ItemsChangedEvent = EventManager.RegisterRoutedEvent(
			"ItemsChanged",
			RoutingStrategy.Bubble,
			typeof(RoutedEventHandler),
			typeof(GadgetPanel));

		/// <summary>
		/// The collection of visible elements in this panel.
		/// </summary>
		UIElementCollection uiElementCollection;

		/// <summary>
		/// Initializes a new instance of the GadgetPanel class.
		/// </summary>
		public GadgetPanel()
		{

			// Initialize the object.
			this.gadgetBar = null;

			// This list contains the visual children in this panel.
			this.uiElementCollection = new UIElementCollection(this, null);

		}

        /// <summary>
        /// The owner of this panel.
        /// </summary>
        protected GadgetBar GadgetBar
        {
            get
            {
                return this.gadgetBar;
            }
        }

        /// <summary>
        /// Generates containers for the items in this control.
        /// </summary>
        protected ItemContainerGenerator ItemContainerGenerator
        {
            get
            {
                return this.itemContainerGenerator;
            }
        }

        /// <summary>
        /// The collection of visible elements in this panel.
        /// </summary>
        protected UIElementCollection UIElementCollection
        {
            get
            {
                return this.uiElementCollection;
            }
        }
        
        /// <summary>
		/// Gets the number of child Visual objects in this instance of Teraque.GadgetPanel.
		/// </summary>
		protected override int VisualChildrenCount
		{
			get
			{
				return this.uiElementCollection == null ? 0 : this.uiElementCollection.Count;
			}
		}

		/// <summary>
		/// Gets a UIElementCollection of child elements of this Panel.
		/// </summary>
		public new UIElementCollection Children
		{
			get
			{
				return this.uiElementCollection;
			}
		}

		/// <summary>
		/// Arranges and sizes GadgetBar items inside a GadgetBarPanel.
		/// </summary>
		/// <param name="finalSize">The size that the GadgetBarPanel assumes to position its children.</param>
		/// <returns>The size of the panel.</returns>
		protected override Size ArrangeOverride(Size finalSize)
		{

			// The margins keep track of how much space has been used by the items as they are placed in the panel.
			Double nearMargin = 0.0;

			// The elements aligned on the far side of the panel need a starting point.  Since they're 'far-justified', the starting point will need to be 
			// calculated backwards from the far margin.
			Double farMargin = finalSize.Width;
			foreach (UIElement uiElement in this.uiElementCollection)
				if (GadgetBar.GetToolDock(uiElement) == ToolDock.Far)
					farMargin -= uiElement.DesiredSize.Width;

			// The logic for this panel is pretty simple.  If the item docks to the near side, then dock it and update the near margin.  If it docks to the far
			// margin, then dock it and update the far margin.  The panel's orientation determins whether 'near' or 'far' is interpreted as left or right for
			// horizontal, or top and bottom for vertical.
			foreach (UIElement uiElement in this.uiElementCollection)
			{

				// The location and size of the current element is calculated here based on the orientation of the panel.
				Rect elementRect = new Rect();

				// This will place the tool bar item either to the near side of the panel or the far side based on the attached property.
				switch (GadgetBar.GetToolDock(uiElement))
				{

				case ToolDock.Near:

					// This docks the control to the near size of the panel based on the orientation.
					elementRect = new Rect(new Point(nearMargin, 0.0), new Size(uiElement.DesiredSize.Width, finalSize.Height));
					nearMargin += uiElement.DesiredSize.Width;
					break;

				case ToolDock.Far:

					// This docks the control to the far side of the panel based on the orientation.
					elementRect = new Rect(new Point(farMargin, 0.0), new Size(uiElement.DesiredSize.Width, finalSize.Height));
					farMargin += uiElement.DesiredSize.Width;
					break;

				}

				// Once the location and size have been calculated above, the item can be placed in the panel.
				uiElement.Arrange(elementRect);

			}

			// No adjustments are made to the final size during the arrangement of the items.
			return finalSize;

		}

		/// <summary>
		/// Gets a Visual child of this Panel at the specified index position.
		/// </summary>
		/// <param name="index">The index position of the Visual child.</param>
		/// <returns>A Visual child of the parent Panel element.</returns>
		protected override Visual GetVisualChild(int index)
		{
			if (index < 0 || index >= this.VisualChildrenCount)
				throw new ArgumentOutOfRangeException(ExceptionMessage.Format(ExceptionMessages.VisualArgumentOutOfRange, index));
			return this.uiElementCollection[index];
		}

		/// <summary>
		/// Measures the child elements of a Teraque.GadgetPanel prior to arranging them during the ArrangeOverride pass.
		/// </summary>
		/// <param name="availableSize">A maximum Size to not exceed.</param>
		/// <returns>A Size that represents the element size you want.</returns>
		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
		protected override Size MeasureOverride(Size availableSize)
		{

			// This panel can provide no useful measurement or layout if it isn't an items host.
			if (!this.IsItemsHost)
				return availableSize;

			// This is the collection that is associated with the user interface in this panel.  Items that can fit in the panel or have been explicitly set to be
			// visible will be added to this collection.
			this.uiElementCollection.Clear();

			// This is the only place where the decision about what items actually appear in the panel is made.  It is desired for an event to be associated with
			// this control that is invoked when items are added or removed from the panel.  However, the GadgetPanel will go through several passes of adding and 
			// removing items before it determines what fits and what doesn't.  For this reason a simple trigger on the list wouldn't work because there'd be too 
			// many false signals as items are measured.  To provide the required functionality of a trigger that only fires when items are added and removed from 
			// the final version of the panel, a copy of the list is made here before the measurement and will be used later to determine if the final set of items
			// has changed.
			List<Object> originalList = new List<Object>();
			foreach (Object item in this.gadgetBar.PanelItems)
				originalList.Add(item);

			// After each pass through the measuring the child controls the logical items will be shifted around.  Some of them will be moved to the overflow panel 
			// and some will remain in the main panel.  This confuses the items container generator when it comes time to creating the containers as there is a 
			// dependency within the generator on the logical parent.  Therefore, The original logical relationship of the items is restored here in order to set 
			// things right for the ItemsContainerGenerator.  Note that the base ItemsControl class is used as a source for the logical items because it contains 
			// the overflow item as well.
			this.gadgetBar.PanelItems.Clear();
			this.gadgetBar.OverflowItems.Clear();
			ItemsControl itemsControl = this.gadgetBar as ItemsControl;
			foreach (Object item in itemsControl.Items)
				this.gadgetBar.PanelItems.Add(item);

			// The order of the items as they come out of the generator is used to determine their order in the overflow panel.  This table is used to determine the
			// relative order of items as they are moved to the overflow panel.
			Dictionary<Object, Int32> itemTable = new Dictionary<object, int>();

			// Used to indicate that a seperator has been added to the overflow menu.
			Boolean hasSeparator = false;

			// The main panel must make a proper measurement of all the child controls to determine if there's enough space.  If there isn't, then the items are 
			// moved out of the main panel and into an overflow panel in a very well prescribed order.  The trouble with this is that items don't move out of their
			// containers very well.  They seem to be broken when another items container tries to use the same item.  In this case, the items container for the
			// items in the main panel are broken when the overflow panel creates menu item containers for them.  There seems to be no mechanism to repair a broken
			// container so they are regenerated here.  Also, recycling doesn't appear to work.  Once a container is broken, apparently it must be discarded so each
			// time through the measure override a new set of containers is generated.
			IItemContainerGenerator iItemContainerGenerator = this.itemContainerGenerator as ItemContainerGenerator;
			iItemContainerGenerator.RemoveAll();

			// When items are moved out of the main panel and into the overflow panel they keep the same relative order.  The algorithm to do this is a bit tricky
			// as the items are moved in several passes.  The first pass takes the items that are marked to always appear in the overflow panel.  The next pass
			// takes the items from the near side of the visible panel that are marked to be moved as needed.  The final pass takes the items from the far side of
			// the main panel. These items provided cursors to manage the ordering of the overflow panel as it is filled.
			Int32 itemIndex = 0;
			Int32 nearOverflowIndex = 0;
			Int32 farOverflowIndex = 0;
			Int32 nearPanelIndex = 0;
			Int32 farPanelIndex = 0;

			// This variable will capture the overflow menu item, if it exists as part of the members of the panel.  The overflow menu item has special properties
			// in that an item that doesn't fit into the panel will be made children of this item.
			OverflowItem overflowItem = null;

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
					// container based on the item type.  Logical relationships for the container can also be established at this time.
					iItemContainerGenerator.PrepareItemContainer(uiElement);

                    // Overflow menu items have special meaning for the panel.  Any item that doesn't fit into the panel will be added to this visual element.
                    OverflowItem generatedOverflowItem = uiElement as OverflowItem;
                    if (generatedOverflowItem != null)
                    {

						// Capture the element for use later.  The items the items can't fit into the panel will be added to this item.
                        overflowItem = generatedOverflowItem;

						// Overflow elements that are added "As Needed" are a special case.  These items will remain hidden until room runs out on the panel for 
						// the other items.  At that point, the OverflowItem will appear as a menu item and those items that don't fit will be moved to the 
						// OverflowItem.
						if (GadgetBar.GetOverflowMode(overflowItem) == OverflowMode.AsNeeded)
							continue;

					}

					// This constructs a mapping of the item to its ordinal and is used to determine the relative order of the overflow items as they are added to
					// the overflow panel in multiple passes.
					Object item = this.itemContainerGenerator.ItemFromContainer(uiElement);
					itemTable.Add(item, itemIndex++);

					// The ToolDock is used to determine whether the new item container is added to the near or far side of the panel.
					ToolDock toolDock = GadgetBar.GetToolDock(uiElement);

					// The OverflowMode attached property can be use to force an item into the overflow panel.
					switch (GadgetBar.GetOverflowMode(uiElement))
					{
					case OverflowMode.Always:

						// A separator is injected into the overflow panel when there are both near and far aligned items.
						if (toolDock == ToolDock.Far && nearOverflowIndex != 0 && !hasSeparator)
						{
							hasSeparator = true;
							this.gadgetBar.OverflowItems.Insert(nearOverflowIndex, new Separator());
							farOverflowIndex++;
						}
		
						// These items always appear in the overflow panel in the order they were added to the Items property of the GadgetBar.
						this.gadgetBar.PanelItems.Remove(item);

						// Conversely the item removed from the main panel is moved into the overflow panel.  The same item can not be the logical child of two
						// windows at the same time.  The observable lists will take care of removing the child from one parent and giving it to the other.
						this.gadgetBar.OverflowItems.Insert(toolDock == ToolDock.Near ? nearOverflowIndex : farOverflowIndex, uiElement);
						nearOverflowIndex += toolDock == ToolDock.Near ? 1 : 0;
						farOverflowIndex++;

						// Remove the generated item if it has no place in the main panel.
						Int32 containerIndex = this.itemContainerGenerator.IndexFromContainer(uiElement);
						GeneratorPosition generatorPosition = iItemContainerGenerator.GeneratorPositionFromIndex(containerIndex);
						iItemContainerGenerator.Remove(generatorPosition, 1);

						break;

					default:

						// These items include the ones that will move to the overflow panel if needed and the ones that never move.  A second pass will determine
						// if any of the 'As Needed' items need to be moved.  Note that the collection of containers is not the same as the collection of items.
						// The collection of items is a logical organization whereas the collection of containers is visual.
						this.uiElementCollection.Insert(toolDock == ToolDock.Near ? nearPanelIndex : farPanelIndex, uiElement);
						nearPanelIndex += toolDock == ToolDock.Near ? 1 : 0;
						farPanelIndex++;

						break;

					}
				}
			}

			// This keeps track of how much space in the panel is occupied by the items as they are laid out.
			Size allocatedSize = new Size();

			// This constraint is used to allow the controls to measure themselves out in the direction in which the panel is oriented.  That is, if it has a
			// horizontal orientation then an infinite amount of space is given during the measurement process in this direction.  It allows the controls the
			// calculate their theoretical size.  If the item doesn't fit, it will be removed from the panel and its desired size will be recalculated inside the
			// overflow panel.  If it does fit, then the desired size is the actual size it is given in the panel.
			Size infiniteSize = new Size(Double.PositiveInfinity, availableSize.Height);

			// This pass will measure everything that wants to appear in this panel.  An infinite amount of room is given in the direction in which this panel is
			// oriented so the measurement operation won't be constrained.  Another pass will actually determine if the items fit or not.
			foreach (UIElement uiElement in this.uiElementCollection)
			{
				uiElement.Measure(infiniteSize);
				allocatedSize = new Size(allocatedSize.Width + uiElement.DesiredSize.Width, Math.Max(allocatedSize.Height, uiElement.DesiredSize.Height));
			}

			// This will attempt to make sure that everything can fit into the alloted space.  If there isn't enough room and individual items are willing to be
			// placed into the overflow panel, then they are removed from this panel.  This concept is very important because moving logical children from one 
			// container to another breaks the container and it can't be repaired.  That is why the items must be regenerated each time the panel is measured.  The
			// items are moved in two passes: the items on the near side of the panel are removed before the items on the far side.  When an overflow of items does
			// occurs a control will appear on the panel that allows the user to access the overflow items.
			Double availableLength = availableSize.Width;
			Double allocatedLength = allocatedSize.Width;
			if (allocatedLength > availableLength || this.gadgetBar.OverflowItems.Count != 0)
			{

				// When an overflow occurs a special item appears on the panel and provides access to the overflow panel for the user.  When this special item is
				// added to the panel the measuring algorithm will need to consider its size when trying to work out what items fit and what items need to be 
				// pushed into the overflow panel.  When this item appears it will always have priority over the items that can be moved into the overflow panel.
				// Note that the logical ordering of the children has no impact on how they behave visually so the element can just be added to the logical
				// children.
				if (overflowItem != null && GadgetBar.GetOverflowMode(overflowItem) != OverflowMode.Never)
				{
					this.uiElementCollection.Insert(nearPanelIndex, overflowItem);
					this.gadgetBar.PanelItems.Add(overflowItem);
					overflowItem.Measure(infiniteSize);
					allocatedLength += overflowItem.DesiredSize.Width;
				}

				// The calculation of what items can appear in the overflow panel is accomplished in two passes.  The first pass will look at the near-aligned items
				// and move them into the overflow panel starting from the farthest item to the item to the nearest until the items fit in the available space.
				Int32 index = nearPanelIndex - 1;
				while (allocatedLength > availableLength && index >= 0)
				{

					// This will move the next element in the panel into the overflow panel as needed.  Note that the overflow button is never moved into the
					// overflow panel and that only the near items are considered during the first pass.
					UIElement uiElement = this.uiElementCollection[index];
					if (GadgetBar.GetOverflowMode(uiElement) == OverflowMode.AsNeeded)
					{

						// This element will no longer appear on the main panel.  This will remove both the visual and the logical relationship.  If the logical
						// relationship isn't broken then this item can't be added to the overflow panel as an item can have only one logical parent at a time.  If 
						// the proper logical relation isn't made then the containers and the contents won't pick up the proper styles.  This is particularly
						// important for menu items as top level items behave differently than sub-menu items.
						this.uiElementCollection.Remove(uiElement);
						Object item = this.itemContainerGenerator.ItemFromContainer(uiElement);
						this.gadgetBar.PanelItems.Remove(item);

						// This will provide the housekeeping with the ItemsContainerGenerator by removing containers that aren't needed for this panel.
						Int32 containerIndex = this.itemContainerGenerator.IndexFromContainer(uiElement);
						GeneratorPosition generatorPosition = iItemContainerGenerator.GeneratorPositionFromIndex(containerIndex);
						iItemContainerGenerator.Remove(generatorPosition, 1);

						// Insert the item into the overflow panel in the same relative order that the item had in the origian Items list of the host.
						Int32 newItemIndex = itemTable[item];
						for (Int32 oldItemIndex = 0; oldItemIndex <= nearOverflowIndex; oldItemIndex++)
							if (oldItemIndex == nearOverflowIndex)
								this.gadgetBar.OverflowItems.Insert(nearOverflowIndex, item);
							else
								if (newItemIndex < itemTable[this.gadgetBar.OverflowItems[oldItemIndex]])
								{
									this.gadgetBar.OverflowItems.Insert(oldItemIndex, item);
									break;
								}

						// These act as cursors when ordering the items in the overflow panel.
						nearOverflowIndex++;
						farOverflowIndex++;
						nearPanelIndex--;

						// Adjust the available space by the size of the item that was just removed to the overflow panel.
						allocatedLength -= uiElement.DesiredSize.Width;

						// Adding items to the overflow panel can change its role in the GadgetBar.  Each time through this iteration the overflow item needs to be
						// re-measured to account for any changes.  Failure to re-measure the overflow item can lead to a infinite loop of measuring the panel.
						allocatedLength -= overflowItem.DesiredSize.Width;
						overflowItem.Measure(infiniteSize);
						allocatedLength += overflowItem.DesiredSize.Width;

					}

					// Consider the next container in the list of near-aligned items.
					index--;

				}

				// If there is still not enough room in the visible panel the far-aligned items will be moved to the overflow panel.
				index = nearPanelIndex + 1;
				while (allocatedLength > availableLength && index < this.uiElementCollection.Count)
				{

					// This will move the next element in the panel into the overflow panel as needed.  Note that the overflow button is never moved into the
					// overflow panel and that only the near items are considered during the first pass.
					UIElement uiElement = this.uiElementCollection[index];
					if (GadgetBar.GetOverflowMode(uiElement) == OverflowMode.AsNeeded)
					{

						// This element will no longer appear on the main panel.  This will remove both the visual and the logical relationship.  If the logical
						// relationship isn't broken then this item can't be added to the overflow panel as an item can have only one logical parent at a time.  If 
						// the proper logical relation isn't made then the containers and the contents won't pick up the proper styles.  This is particularly
						// important for menu items as top level items behave differently than sub-menu items.
						this.uiElementCollection.Remove(uiElement);
						Object item = this.itemContainerGenerator.ItemFromContainer(uiElement);
						this.gadgetBar.PanelItems.Remove(item);

						// This will provide the housekeeping with the ItemsContainerGenerator by removing containers that aren't needed for this panel.
						Int32 containerIndex = this.itemContainerGenerator.IndexFromContainer(uiElement);
						GeneratorPosition generatorPosition = iItemContainerGenerator.GeneratorPositionFromIndex(containerIndex);
						iItemContainerGenerator.Remove(generatorPosition, 1);

						// A seperator is created when far-aligned items occupy the same overflow panel as near-aligned items.
						if (nearOverflowIndex != 0 && !hasSeparator)
						{
							hasSeparator = true;
							this.gadgetBar.OverflowItems.Insert(nearOverflowIndex, new Separator());
							farOverflowIndex++;
						}

						// Insert the item into the overflow panel in the same relative order that the item had in the origian Items list of the host.  Note that
						// the position of the seperator will change the starting point for examining the far-aligned items for the proper order.
						Int32 newItemIndex = itemTable[item];
						Int32 startingPoint = hasSeparator ? nearOverflowIndex + 1 : nearOverflowIndex;
						for (Int32 oldItemIndex = startingPoint; oldItemIndex <= farOverflowIndex; oldItemIndex++)
							if (oldItemIndex == farOverflowIndex)
								this.gadgetBar.OverflowItems.Insert(farOverflowIndex, item);
							else
								if (newItemIndex < itemTable[this.gadgetBar.OverflowItems[oldItemIndex]])
								{
									this.gadgetBar.OverflowItems.Insert(oldItemIndex, item);
									break;
								}

						// This act as cursors when ordering the items in the overflow panel.
						farOverflowIndex++;

						// Adjust the available space by the size of the item that was just removed to the overflow panel.
						allocatedLength -= uiElement.DesiredSize.Width;

						// Adding items to the overflow panel can change its role in the GadgetBar.  Each time through this iteration the overflow item needs to be
						// re-measured to account for any changes.  Failure to re-measure the overflow item can lead to a infinite loop of measuring the panel.
						allocatedLength -= overflowItem.DesiredSize.Width;
						overflowItem.Measure(infiniteSize);
						allocatedLength += overflowItem.DesiredSize.Width;

					}
					else
					{

						// Consider the next far-aligned item to see if it can be moved out of the visible panel.
						index++;

					}

				}

			}

			// This will advise any listeners that the items in the panel have changed.  Since panels are not part of a standard template, this information is
			// normally inaccessible to a parent class except as events bubbled up through the visual tree hierarchy.
			Boolean isEqual = this.gadgetBar.PanelItems.Count == originalList.Count;
			for (Int32 index = 0; isEqual && index < this.gadgetBar.PanelItems.Count; index++)
				isEqual = Object.Equals(this.gadgetBar.PanelItems[index], originalList[index]);
			if (!isEqual)
				this.RaiseEvent(new RoutedEventArgs(GadgetPanel.ItemsChangedEvent, this));

			// This is how much room is needed for the panel.  Note that the maximum height (or width) of the tool panel is determined by all the items whether they
			// appear in the tool panel or the overflow panel.  This one-size-fits-all approach keeps the panel from jumping around as items are added from or
			// removed to the overflow panel.
			return new Size(allocatedLength, allocatedSize.Height);

		}

		/// <summary>
		/// Indicates that the IsItemsHost property value has changed.
		/// </summary>
		/// <param name="oldIsItemsHost">The old property value.</param>
		/// <param name="newIsItemsHost">The new property value.</param>
		protected override void OnIsItemsHostChanged(Boolean oldIsItemsHost, Boolean newIsItemsHost)
		{

			// When a panel is going to host items, it needs to set up some housekeeping for the generator and notify the host.  Likewise, when the services of a
			// panel are no longer needed to host an item, the panel needs to clean up.
			if (newIsItemsHost)
			{

				// The GadgetPanel must communicate to the host when adding or removing items from the panel so that the logical relationships can be maintained. An
				// item will normally want to consider the GadgetBar as its logical parent, but if that item is moved into the overflow item then that relationship
				// needs to change.  Since the parent of an item is the only one who can add or remove logical children, it needs to be notified that a change has
				// taken place, for example, when an item is moved into the overflow item.
				this.gadgetBar = ItemsControl.GetItemsOwner(this) as GadgetBar;

				// Conversly, the GadgetBar needs to know what panel is hosting the items so it can force a re-measurement when the properties on items change.  For
				// example, when the docking or overflow mode changes, the panel needs to be re-measured and re-arranged.
				this.gadgetBar.gadgetPanel = this;

				// This will initialize the generator of containers for the panel and hook it into the handler for updating the panel.
				IItemContainerGenerator iItemContainerGenerator = this.gadgetBar.ItemContainerGenerator;
				if (iItemContainerGenerator != null)
				{
					this.itemContainerGenerator = iItemContainerGenerator.GetItemContainerGeneratorForPanel(this);
					if (this.itemContainerGenerator != null)
						this.itemContainerGenerator.ItemsChanged += new ItemsChangedEventHandler(this.OnItemsChanged);
				}


			}
			else
			{

				// This will clean up the relationship with the owner when the panel is no longer needed as a host for items.
				this.gadgetBar.gadgetPanel = null;
				this.gadgetBar = null;

				// With no owner, there can be no container generators either.
				if (this.itemContainerGenerator != null)
					this.itemContainerGenerator.ItemsChanged -= new ItemsChangedEventHandler(this.OnItemsChanged);
				this.itemContainerGenerator = null;

			}

			// The base class must be called for the panel to be properly connected to the GadgetBar.  If it isn't, the base class won't be aware of its panel and 
			// the keyboard and mouse will not work properly when navigating.
			base.OnIsItemsHostChanged(oldIsItemsHost, newIsItemsHost);

		}

		/// <summary>
		/// Called when the Items collection that is associated with the ItemsControl for this Panel changes.
		/// </summary>
		/// <param name="sender">The Object that raised the event.</param>
		/// <param name="itemsChangedEventArgs">Provides data for the ItemsChanged event.</param>
		protected void OnItemsChanged(Object sender, ItemsChangedEventArgs itemsChangedEventArgs)
		{

			// There is no DependencyProperty to trigger a new measurement so adding or removing items from the ItemsControlGenerator provides the trigger.
			this.InvalidateMeasure();

		}

	}

}
