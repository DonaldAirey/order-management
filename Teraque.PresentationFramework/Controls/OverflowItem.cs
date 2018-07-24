namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections.Specialized;
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// A control for accessing the items that overflow from the GadgetBar.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class OverflowItem : Gadget
	{

		/// <summary>
		/// Initializes the OverflowItem class.
		/// </summary>
		static OverflowItem()
		{

			// This declares the use of {x:Type OverflowItem} as an implicit key for finding the template in a XAML theme.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(OverflowItem), new FrameworkPropertyMetadata(typeof(OverflowItem)));

		}

		/// <summary>
		/// Called when the Items property changes.
		/// </summary>
		/// <param name="e">The event data for the ItemsChanged event.</param>
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{

            // Validate the arguments.
            if (e == null)
                throw new ArgumentNullException("e");

            // Allow the base class to handle the event.
			base.OnItemsChanged(e);

			// This property indicates that the control has items in the overflow panel.
            this.SetHasOverflowItems(this.Items.Count != 0);

			// This property indicates that the control has items in the overflow panel that were put there because there wasn't enough space.
			Boolean hasAsNeededItems = false;
			foreach (Object item in this.Items)
			{
				DependencyObject dependencyObject = item as DependencyObject;
				if (dependencyObject != null && GadgetBar.GetOverflowMode(dependencyObject) == OverflowMode.AsNeeded)
					hasAsNeededItems = true;
			}
            this.SetHasAsNeededItems(hasAsNeededItems);

		}

	}

}
