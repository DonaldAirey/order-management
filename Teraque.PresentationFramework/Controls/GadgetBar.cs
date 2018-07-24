namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Controls.Primitives;
	using System.Windows.Data;
	using System.Windows.Input;
	using System.Windows.Markup;
	using System.Windows.Threading;
	using Teraque.Windows;
	using Teraque.Windows.Data;

	/// <summary>
	/// Represents a Windows menu control with an overflow panel support that enables you to hierarchically organize elements associated with commands and event
	/// handlers.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[ContentProperty("Items")]
	public class GadgetBar : Menu
	{

		/// <summary>
		/// A list of the logical children added to the Teraque.GadgetBar.
		/// </summary>
		List<Object> childList;

		/// <summary>
		/// This collection allows us to provide an implicit overflow item along with the items provided by the consumer of this control.
		/// </summary>
		CompositeCollection compositCollection;

		/// <summary>
		/// Identifies the HasItems dependency property.
		/// </summary>
		public new static readonly DependencyProperty HasItemsProperty;

		/// <summary>
		/// Identifies the HasItems dependency property.key.
		/// </summary>
		static readonly DependencyPropertyKey hasItemsPropertyKey = DependencyProperty.RegisterReadOnly(
			"HasItems",
			typeof(Boolean),
			typeof(GadgetBar),
			new FrameworkPropertyMetadata(false));

		/// <summary>
		/// This provides a collection that can be accessed directly.
		/// </summary>
		ViewableCollection items;

		/// <summary>
		/// Identifies the ItemsSource dependency property.
		/// </summary>
		public new static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
			"ItemsSource",
			typeof(IEnumerable),
			typeof(GadgetBar),
			new FrameworkPropertyMetadata(GadgetBar.OnItemsSourcePropertyChanged));

		/// <summary>
		/// The panel that arranges the items and provides the logic for overflowing.
		/// </summary>
		internal GadgetPanel gadgetPanel;

		/// <summary>
		/// Identifies the OverflowMode attached property.
		/// </summary>
		public static readonly DependencyProperty OverflowModeProperty = DependencyProperty.RegisterAttached(
			"OverflowMode",
			typeof(OverflowMode),
			typeof(GadgetBar),
			new FrameworkPropertyMetadata(OverflowMode.AsNeeded, new PropertyChangedCallback(GadgetBar.OnOverflowModePropertyChanged)));

		/// <summary>
		/// Identifies the OverflowItemStyle dependency property.
		/// </summary>
		public static readonly DependencyProperty OverflowItemStyleProperty = DependencyProperty.Register(
			"OverflowItemStyle",
			typeof(Style),
			typeof(GadgetBar),
			new FrameworkPropertyMetadata(GadgetBar.OnOverflowItemStylePropertyChanged));
	
		/// <summary>
		/// All GadgetBars have an implicit overflow item which will display the items that don't fit into the GadgetPanel.
		/// </summary>
		OverflowItem overflowItem;

		/// <summary>
		/// Identifies the Owner attached dependency property.
		/// </summary>
		static readonly DependencyProperty ownerProperty = DependencyProperty.RegisterAttached(
			"Owner",
			typeof(GadgetBar),
			typeof(GadgetBar));

		/// <summary>
		/// Identifies the PanelItems dependency property.
		/// </summary>
		public static readonly DependencyProperty PanelItemsProperty;

		/// <summary>
		/// Identifies the PanelItems dependency property key.
		/// </summary>
		static readonly DependencyPropertyKey panelItemsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
			"PanelItems",
			typeof(ObservableCollection<Object>),
			typeof(GadgetBar),
			new FrameworkPropertyMetadata());

		/// <summary>
		/// Identifies the ToolDock attached property.
		/// </summary>
		public static readonly DependencyProperty ToolDockProperty = DependencyProperty.RegisterAttached(
			"ToolDock",
			typeof(ToolDock),
			typeof(GadgetBar),
			new FrameworkPropertyMetadata(ToolDock.Near, new PropertyChangedCallback(GadgetBar.OnToolDockPropertyChanged)));

		/// <summary>
		/// Initializes the GadgetBar class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static GadgetBar()
		{

			// This allows GadgetBar instances to find their implicit styles in the themes.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GadgetBar), new FrameworkPropertyMetadata(typeof(GadgetBar)));

			// These keys provides public access to read-only dependency Objects.  Since the compiler doesn't check dependancies on initializers, we must force them
			// to be assigned after the fields are initialized.
			GadgetBar.HasItemsProperty = GadgetBar.hasItemsPropertyKey.DependencyProperty;
			GadgetBar.PanelItemsProperty = GadgetBar.panelItemsPropertyKey.DependencyProperty;

		}

		/// <summary>
		/// Initializes a new instance of the GadgetBar class.
		/// </summary>
		public GadgetBar()
		{

			// The PanelItems is an ObservableCollection that is used by the GadgetPanel to communicate with the panel.  It describes which of the child items of 
			// this contol are visible on the panel and therefore, should be logical children of the GadgetBar.  When items can't fit onto the panel during the 
			// measurement pass (or are forced there through properties), they are given to the overflow item which then becomes the logical parent.  Since only a 
			// parent can give up logical control, the PanelItems list is used to basically ask the GadgetBar to add and remove logical children as the panel 
			// measures itself.  However, the VisibleCollection has a flaw for this purpose: by the time the 'Reset' event is handled, all the items have been 
			// expunged from the list.  This parallel list of items is used to handle the 'Reset'.
			this.childList = new List<Object>();

			// This observable collections is used to communicate the set of logical children that are associated with the visible panel.  As items are added or
			// removed from the collection, so are they added or removed as logical children of this control.  This is necessary so that menu items can pick up the
			// role attribute that goes along with the parent menu.  For example, if a menu item is placed on the top level (visible) panel, then it should have a
			// down-arrow glyph to indicate that it has child items.  If that same menu item is moved to an overflow panel because it can't fit in the visible
			// panel, it is now a submenu and should have a right or left arrow to indicate children.  This is only possible when the item has the proper logical
			// connection to the parent menu or overflow control.
			this.SetValue(GadgetBar.panelItemsPropertyKey, new ObservableCollection<Object>());

			// This event handler will add or remove logical children to this control.  The GadgetPanel will manage the contents of the list because it knows
			// what can fit and what needs to be put into the overflow panel.  This control will use the collection to add and remove the logical children.
			this.PanelItems.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnPanelItemsPropertyChanged);

			// All GadgetBars have an implicit OverflowItem control where items that don't fit onto the main panel are moved.
			this.overflowItem = new OverflowItem();

			// IMPORTANT CONCEPT: every GadgetBar has an implicit overflow menu item that catches the items that don't fit into the panel.  This item must be part
			// of the 'Items' in an ItemsControl for the keyboard navigation to work.  Internally, the WPF base classes have a dependency on the 
			// ItemsContainerGenerator that is not obvious.  The overflow item can't be added as a stand-alone element in the panels where the items are displayed
			// if the navigation keys are to work.  So far, so good.  But how do you create an implicit item for an items control?  In this design it is a 
			// ViewableCollection (functionally equivalent to an ItemsCollection but without the connection to the FrameworkElements) that is combined with the 
			// overflow item to make a composite collection.  The composite collection is then joined to the SourceItems of the underlying ItemsControl.  The user
			// of this control only ever sees the 'Items' and the 'ItemsSource'.
			this.items = new ViewableCollection();

			// The Composite collection is where the implicit overflow item is merged with the collection of items available to the user of this control.  This
			// design makes the API to the consumer of this control appear to be a standard ItemsControl, but allows us to add an implicit overflow item that
			// can not be manipulated directly by the consumer yet feeds into the ItemsContainerGenerator like any other item in the GadgetBar.
			this.compositCollection = new CompositeCollection();
			this.compositCollection.Add(this.overflowItem);
			this.compositCollection.Add(new CollectionContainer() { Collection = this.items });
			base.ItemsSource = this.compositCollection;

		}

		/// <summary>
		/// The currently selected Gadget.
		/// </summary>
		Gadget CurrentSelection
		{

			get
			{

				// Cycle through all the items dispalyed in the panel looking for one that is selected.  The base class maintains this information but does not 
				// share it with its descendants, so the logic is emulated here.
				foreach (FrameworkElement frameworkElement in this.PanelItems)
				{
					Gadget gadget = frameworkElement as Gadget;
					if (gadget != null && Selector.GetIsSelected(gadget))
						return gadget;
				}

				// At this point the GadgetBar does not have any selected items.
				return null;

			}

		}

		/// <summary>
		/// Gets the collection used to generate the content of the control.
		/// </summary>
		public new ViewableCollection Items
		{
			get
			{
				return this.items;
			}
		}

		/// <summary>
		/// Gets or sets a collection used to generate the content of the ItemsControl.
		/// </summary>
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new IEnumerable ItemsSource
		{
			get
			{
				return this.GetValue(GadgetBar.ItemsSourceProperty) as IEnumerable;
			}
			set
			{
				this.SetValue(GadgetBar.ItemsSourceProperty, value);
			}
		}

		/// <summary>
		/// Gets an enumerator for the logical child Objects of the ItemsControl Object.
		/// </summary>
		protected override IEnumerator LogicalChildren
		{
			get
			{

				// This control will hide Objects that can't fit into the visible area.  The hidden Objects must become logical children of the overflow panel so 
				// they can pick up the proper styling.  Those overflow items can no longer be part of this control's logical children.  This must be done because 
				// the logcal parent of the item determines the menu role and therefore the style used to present the item.  Also, the logical relationship is critical
				// for the command infrastructure to be able to find the commands through routing.
				return this.PanelItems.GetEnumerator();

			}
		}

		/// <summary>
		/// The items that don't fit on the main panel.
		/// </summary>
		public ItemCollection OverflowItems
		{
			get
			{
				return this.overflowItem.Items;
			}
		}

		/// <summary>
		/// Gets or sets the style for the OverflowItem.
		/// </summary>
		public Style OverflowItemStyle
		{
			get
			{
				return this.GetValue(GadgetBar.OverflowItemStyleProperty) as Style;
			}
			set
			{
				this.SetValue(GadgetBar.OverflowItemStyleProperty, value);
			}
		}

		/// <summary>
		/// A modifiable list of the logical children of this control.
		/// </summary>
		internal ObservableCollection<Object> PanelItems
		{
			get
			{
				return this.GetValue(GadgetBar.PanelItemsProperty) as ObservableCollection<Object>;
			}
		}

		/// <summary>
		/// Gets the value of the ToolDock attached property for a specified UIElement.
		/// </summary>
		/// <param name="dependencyObject">The element from which the property value is read.</param>
		/// <returns>The ToolDock property value for the element.</returns>
		[AttachedPropertyBrowsableForChildren]
		public static ToolDock GetToolDock(DependencyObject dependencyObject)
		{

			// Validate the parameter and return the attached property.
			if (dependencyObject == null)
				throw new ArgumentNullException("dependencyObject");
			return (ToolDock)dependencyObject.GetValue(GadgetBar.ToolDockProperty);

		}

		/// <summary>
		/// Gets the value of the OverflowMode attached property for a specified UIElement.
		/// </summary>
		/// <param name="dependencyObject">The element from which the property value is read.</param>
		/// <returns>The OverflowMode property value for the element.</returns>
		[AttachedPropertyBrowsableForChildren(IncludeDescendants = true)]
		public static OverflowMode GetOverflowMode(DependencyObject dependencyObject)
		{

			// Validate the parameter and return the attached property.
			if (dependencyObject == null)
				throw new ArgumentNullException("dependencyObject");
			return (OverflowMode)dependencyObject.GetValue(OverflowModeProperty);

		}

		/// <summary>
		/// Gets the value of the Owner attached property for a specified UIElement.
		/// </summary>
		/// <param name="dependencyObject">The element from which the property value is read.</param>
		/// <returns>The Owner property value for the element.</returns>
		public static GadgetBar GetOwner(DependencyObject dependencyObject)
		{

			// Validate the parameter and return the attached property.
			if (dependencyObject == null)
				throw new ArgumentNullException("dependencyObject");
			return dependencyObject.GetValue(GadgetBar.ownerProperty) as GadgetBar;

		}

		/// <summary>
		/// Invoked when the effective property value of the ItemsSource property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnItemsSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the event arguments.
			GadgetBar gadgetBar = (GadgetBar)dependencyObject;
			IEnumerable newValue = (IEnumerable)dependencyPropertyChangedEventArgs.NewValue;

			// The ItemsSource can be cleared by setting the property to 'null' (and providing it's not data bound).  Otherwise this handler will set the
			// source of the internal collection to use the supplied collection.
			if (dependencyPropertyChangedEventArgs.NewValue == null && !BindingOperations.IsDataBound(dependencyObject, GadgetBar.ItemsSourceProperty))
				gadgetBar.items.ClearItemsSource();
			else
				gadgetBar.items.ItemsSource = newValue;

		}

		/// <summary>
		/// Called whenever the the items in the main panel have changed.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="notifyCollectionChangedEventArgs">A NotifyCollectionChangedEventArgs that contains the event data.</param>
		void OnPanelItemsPropertyChanged(Object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{

			// The main idea here is to allow the panel to add or remove logical children by manipulating the observable list.  The parallel list 'childList' 
			// contains a copy of the PanelItems so it can clear the items when the list is reset.
			switch (notifyCollectionChangedEventArgs.Action)
			{

			case NotifyCollectionChangedAction.Add:

				// Add each of the new items.
				foreach (Object child in notifyCollectionChangedEventArgs.NewItems)
				{
					this.childList.Add(child);
					this.AddLogicalChild(child);
				}
				break;

			case NotifyCollectionChangedAction.Remove:

				// Remove each of the old items.
				foreach (Object child in notifyCollectionChangedEventArgs.OldItems)
				{
					this.childList.Remove(child);
					this.RemoveLogicalChild(child);
				}
				break;

			case NotifyCollectionChangedAction.Reset:

				// At this point the PanelItems list has been cleared and there is no good place to go to find out what items need to be removed.  The ancillary
				// 'childList' provides this information so the items in the PanelItems collection will always reflect the logical children of this control.
				foreach (Object child in this.childList)
					this.RemoveLogicalChild(child);
				this.childList.Clear();
				break;

			}

		}

		/// <summary>
		/// Invoked when the effective property value of the OverflowMode attached property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnOverflowModePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will force the parent panel to evaluate the measurements of the children again.
			GadgetBar gadgetBar = GadgetBar.GetOwner(dependencyObject);
			if (gadgetBar != null && gadgetBar.gadgetPanel != null)
				gadgetBar.gadgetPanel.InvalidateMeasure();

		}

		/// <summary>
		/// Invoked when the effective property value of the OverflowItemStyle property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnOverflowItemStylePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will give an explicit style to the OverflowItem if one is provided.  If none is provided, then an implicit style based on the OverflowItem type 
			// is given provided one has been provided within the scope of the resource where this GadgetBar has been declared.
			GadgetBar gadgetBar = dependencyObject as GadgetBar;
			Style style = dependencyPropertyChangedEventArgs.NewValue as Style;
			if (style == null)
				style = gadgetBar.TryFindResource(typeof(OverflowItem)) as Style;
			gadgetBar.overflowItem.Style = style;

		}

		/// <summary>
		/// Invoked when a GotKeyboardFocus attached event reaches this element in its route.
		/// </summary>
		/// <param name="e">The KeyboardFocusChangedEventArgs that contains the event data.</param>
		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{

			// Validate parameters
			if (e == null)
				throw new ArgumentNullException("e");

			// If this control gets the keyboard focus and hasn't already selected an item to have the focus, use the native 'MoveFocus' method to find the first
			// item in the GadgetBar and pass it the focus.
			if (e.NewFocus == this)
			{

				// For reasons that aren't entirely clear, its possible to get the keyboard focus when this element is visible but before all the items have been
				// measured and arranged.  This will force a layout pass to instantiate the items in the GadgetBar which is necessary before the focus can be moved
				// to the first child inside the control.
				if (!this.IsArrangeValid || !this.IsMeasureValid)
					this.UpdateLayout();

				// This will move the focus to the first visible item inside the GadgetBar the first time it is used.  Note that the keyboard navigation functions 
				// are not used (e.g. MoveFocus).  Mainly because they are brain-dead, but mostly because they do not work on menus where the keyboard and tab 
				// navigation have been disabled.  Just because navigation has been disabled doesn't mean we don't want to control the initialization of the focus.
				DependencyObject focusScope = FocusManager.GetFocusScope(this);
				if (FocusManager.GetFocusedElement(focusScope) == null)
					foreach (Object item in this.PanelItems)
					{
						UIElement uiElement = item as UIElement;
						if (uiElement != null && uiElement.IsVisible)
						{
							FocusManager.SetFocusedElement(focusScope, uiElement as IInputElement);
							break;
						}
					}

			}

			// This method has no default implementation. Because an intermediate class in the inheritance might implement this method, we recommend that you call 
			// the base implementation in your implementation.
			base.OnGotKeyboardFocus(e);
			
		}

		/// <summary>
		/// Responds to the KeyDown event.
		/// </summary>
		/// <param name="e">A KeyEventArgs that contains the event data.</param>
		protected override void OnKeyDown(KeyEventArgs e)
		{

			// Validate the parameters.
			if (e == null)
				throw new ArgumentNullException("e");

			// There is an inconsistency if not an outright bug with WPF's handling of the menus.  When you open up a submenu, the mouse is captured by the MenuBase
			// class.  When you click on the menu item again, the submenu disappears and the mouse capture is released.  However, when you hit the escape key, the
			// submenu disappears but the mouse capture is retained until you hit the Escape key again.  To fix this bug (there is no reason for the MenuBase to
			// retain the mouse capture when non of the submenus are open), the submenu is closed here before the key is processed.  While this doesn't appear to have
 			// any reasonable side-effect, it kicks logic into place deep in the MenuBase class that will close out the menu mode properly.
			if (e.Key == Key.Escape)
			{
				Gadget gadget = this.CurrentSelection;
				if (gadget != null && gadget.IsSubmenuOpen)
					gadget.IsSubmenuOpen = false;
			}
			
			// The base class has considerable processing on this message that needs to be completed.
			base.OnKeyDown(e);

		}

		/// <summary>
		/// Invoked when an unhandled Keyboard.PreviewKeyDown attached event reaches an element in its route that is derived from this class.
		/// </summary>
		/// <param name="e">The KeyboardFocusChangedEventArgs that contains the event data.</param>
		protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{

            // Validate the parameters.
            if (e == null)
                throw new ArgumentNullException("e");

            // This will attempt to fix a bug with the Menu class.  When an item is selected from the submenu of a top-level MenuItem, or when the user hits Escape 
			// in a Menu, the base class attempts to clear the "MenuMode" and "Restore the Previous Focus".  This creates serious problems when the current 
			// MenuItem is also the current focus element of the main window because WPF attempts to restore to focus to the element that already has the focus and 
			// gets confused.  The end effect is that the current item is de-selected even though it still has the focus.  This logic attempts to intercept the 
			// "Restore Previous Focus" logic and make it work intuitively by keeping allowing the item with the keyboard focus to stay 'selected' in the menu.
			UIElement oldFocusElement = e.OldFocus as UIElement;
			UIElement newFocusElement = e.NewFocus as UIElement;
			if (newFocusElement != null && this.IsDescendantOf(newFocusElement) && oldFocusElement != null && this.IsAncestorOf(oldFocusElement))
			{
				Gadget gadget = oldFocusElement as Gadget;
				if (gadget != null)
					gadget.SimulateIsKeyboardFocusWithinChanged();
			}

			// Though there is no default implementation, it is recommend that the base class should be called.
			base.OnPreviewLostKeyboardFocus(e);

		}

		/// <summary>
		/// Invoked when the effective property value of the ToolDock property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnToolDockPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// This will force the parent panel to evaluate the measurements of the children again.
			GadgetBar gadgetBar = GadgetBar.GetOwner(dependencyObject);
			if (gadgetBar != null && gadgetBar.gadgetPanel != null)
				gadgetBar.gadgetPanel.InvalidateMeasure();

		}

		/// <summary>
		/// Prepares the specified element to display the specified item.
		/// </summary>
		/// <param name="element">The element used to display the specified item.</param>
		/// <param name="item">The item to display.</param>
		protected override void PrepareContainerForItemOverride(DependencyObject element, Object item)
		{

			// If a separator is added to this ItemsControl without a specific style, then normally the original style for the separator in the WPF themes is chosen
			// which may not be appropriate for the gadgets and gadget bars.  This could be remedied by supplying an explicit style for ever separator used, but
			// this would be cumbersome in the XAML and prone to errors.  This logic will apply a specific separator style to every separator used by the gadget
			// making it possible to centralize the look of a separator.
			Separator separator = element as Separator;
			if (separator != null && separator.Style == null)
				separator.SetResourceReference(FrameworkElement.StyleProperty, new ComponentResourceKey(typeof(Gadget), "GadgetSeperatorStyle"));

			// This attached property allows an item to find the panel to which it belongs.  It is used for items that may not have a visual tree, for example, menu
			// items that are hidden in an overflow item have no visual tree to navigate.  The 'owner' is a type of proxy for the visual ancestor that needs to be
			// re-measured in order for any changes in this element to be reflected in the user interface.
			GadgetBar.SetOwner(element, this);

			// Generated containers don't seem to automatically be added as logical children.  This is bad because the item container generator uses the logical
			// parent when calling the 'ItemFromContainer' method.  This code allows the 'MeasureOverride' in the Panel to find an item given the container when
			// the container and item aren't the same. 
			if (element != item)
				this.AddLogicalChild(element);

			// Allow the base class to handle the reset of the preparation.
			base.PrepareContainerForItemOverride(element, item);

		}

		/// <summary>
		/// Sets the local value of the OverflowMode attached property.
		/// </summary>
		/// <param name="dependencyObject">The target Object for the value.</param>
		/// <param name="overflowMode">The OverflowMode setting for this Object.</param>
		public static void SetOverflowMode(DependencyObject dependencyObject, OverflowMode overflowMode)
		{

			// Validate the target and set the attached property.
			if (dependencyObject == null)
				throw new ArgumentNullException("dependencyObject");
			dependencyObject.SetValue(GadgetBar.OverflowModeProperty, overflowMode);

		}

		/// <summary>
		/// Sets the owner of an GadgetBar item.
		/// </summary>
		/// <param name="dependencyObject">The target Object for the value.</param>
		/// <param name="owner">The GadgetBar that originally owned the items.</param>
		internal static void SetOwner(DependencyObject dependencyObject, GadgetBar owner)
		{

			// This value is used when any of the attached properties change on an item that require the panels to be measured again.
			if (dependencyObject == null)
				throw new ArgumentNullException("dependencyObject");
			dependencyObject.SetValue(GadgetBar.ownerProperty, owner);

		}

		/// <summary>
		/// Sets the local value of the ToolDock attached property.
		/// </summary>
		/// <param name="dependencyObject">The target Object for the value.</param>
		/// <param name="toolDock">The ToolDock setting for this Object.</param>
		public static void SetToolDock(DependencyObject dependencyObject, ToolDock toolDock)
		{

			// Validate the target and set the attached property.
			if (dependencyObject == null)
				throw new ArgumentNullException("dependencyObject");
			dependencyObject.SetValue(GadgetBar.ToolDockProperty, toolDock);

		}

	}

}
