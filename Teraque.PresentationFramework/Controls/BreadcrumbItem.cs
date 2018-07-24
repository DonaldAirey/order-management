namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;

	/// <summary>
	/// An item that appears on the BreadcrumbBar.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class BreadcrumbItem : Gadget
	{

		/// <summary>
		/// This list holds the submenu items while the BreadcrumbItem is in the overflow list.
		/// </summary>
		Collection<Object> hiddenList;

		/// <summary>
		/// Initializes the BreadcrumbItem class.
		/// </summary>
		static BreadcrumbItem()
		{

			// This is important for being able to find the implicit default style in theme files.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(BreadcrumbItem), new FrameworkPropertyMetadata(typeof(BreadcrumbItem)));

		}

		/// <summary>
		/// Initializes a new instance of BreadcrumbItem class.
		/// </summary>
		public BreadcrumbItem()
		{

			// In order for the BreadcrumbItem to act like a simple MenuItem when it is in the overflow panel the child menu items need to be removed from the item
			// and saved in a safe place.  When the BreadcrumbItem is restored to the top-level the child items are replaced.  The main reason for this is to allow
			// the item to be selected with a single click when it is in the overflow panel.  If it has child items then it behaves like a submenu header which is
			// not desired.
			this.hiddenList = new Collection<Object>();

		}

		/// <summary>
		/// Clears the selection for the entire BreadcrumbItem.
		/// </summary>
		public void ClearSelection()
		{

			// Clear each of the items of their 'Selected' properties.  Note that the location of the child items will change depending on whether the 
			// BreadcrumbItem is in the top-level menu or in a submenu.  Child items are hidden when the BreadcrumbItem lives in the overflow panel so that it can 
			// behave like an item an not a header.
			foreach (Gadget gadget in this.Items)
				gadget.IsParent = false;
			foreach (Gadget gadget in this.hiddenList)
				gadget.IsParent = false;

		}

		/// <summary>
		/// Sets the child MenuItem with the given key to be selected.
		/// </summary>
		/// <param name="dataContext">The data context of the item to be selected.</param>
		public void SetSelection(Object dataContext)
		{

			// Use the data context to determine which of the child items should be selected.  Note that the location of the child items will change depending on 
			// whether the BreadcrumbItem is in the top-level menu or in a submenu.  Child items are hidden when the BreadcrumbItem lives in the overflow panel so
			// that it can behave like an item an not a header.
			foreach (Gadget gadget in this.Items)
				if (gadget.DataContext == dataContext)
					gadget.IsParent = true;
			foreach (Gadget gadget in this.hiddenList)
				if (gadget.DataContext == dataContext)
					gadget.IsParent = true;

		}

		/// <summary>
        /// Invoked whenever the effective value of any dependency property on this FrameworkElement has been updated.
		/// </summary>
		/// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{

            // Always call the base implementation, as the first operation in your implementation.  Failure to do this will significantly disable the entire WPF 
            // property system, which causes incorrect values to be reported. The specific FrameworkElement implementation is also responsible for maintaining 
            // proper state for a variety of properties that affect the visible user interface. These include invalidating the visual tree based on changes to 
            // style at appropriate times.
            base.OnPropertyChanged(e);

            // This will catch a transition of a gadget from a submenu item to a top-level item.  In order to act like a simple menu item when the BreadcrumbItem is
            // in an overflow item or the child of another BreadcrumbItem, it must not have any children.  It's not enough to try to change the appearance of the
            // element based on the role, the children must be removed in order for it to behave properly.  That is, when a BreadcrumbItem is presented in a 
            // submenu, then clicking on the item should take you to that part of the path, not present the child items of that path (as it would if it was a
            // header).  To accomplish this, the children are moved into a holding area while a BreadcrumbItem is part of a submenu.
            if (e.Property == MenuItem.RoleProperty)
			{

                // Extract the new and old roles from the generic arguments.
                MenuItemRole newRole = (MenuItemRole)e.NewValue;
                MenuItemRole oldRole = (MenuItemRole)e.OldValue;

				// The state transition indicates when items should be moved out of their invisible holding area.  When a BreadcrumbItem is placed on the menu it
				// becomes a top-level header for the submenu items.  This allows the user to navigate through the BreadcrumbBar by selecting the submenu items for
				// the next level of the path.  Note that due to a slight eccentricity with the updating of the roles by the base classes, both the TopLevelItem and
                // TopLevelHeader need to be checked.  Theoretically, only a transition from a SubmenuItem to a TopLevelItem is possible, but there is some residual
                // memory of the items that leads the base class to allow the illegal transition.
                if (oldRole == MenuItemRole.SubmenuItem && (newRole == MenuItemRole.TopLevelItem || newRole == MenuItemRole.TopLevelHeader))
				{
					foreach (Object item in this.hiddenList)
						this.Items.Add(item);
					this.hiddenList.Clear();
				}

				// When a BreadcrumbItem is place on the submenu it looses all the child items so it can act like a regular menu item.  Clicking on the item when it
				// resides in the overflow panel should create a path to show that item.  It should not bring up a submenu which it would do without moving the items
                // to an invisible holding area.  Note that a transition from SubmenuItem to SubmenuHeader will also moving things into the holding area.  This is
                // done to catch the condition where items are added to a BreadcrumbItem that is not yet associated with a menu.  The role of an orphaned (new)
                // BreadcrumbItem is a SubmenuItem.
                if ((oldRole == MenuItemRole.TopLevelHeader || oldRole == MenuItemRole.SubmenuItem) && newRole == MenuItemRole.SubmenuHeader)
				{
					foreach (Object item in this.Items)
						this.hiddenList.Add(item);
					this.Items.Clear();
				}

			}

		}

	}

}
