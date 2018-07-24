namespace Teraque.Windows.Controls
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Input;

	/// <summary>
	/// Implements a selectable item in a Navigator control.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class NavigatorItem : TreeViewItem
	{

		/// <summary>
		/// Identifies the isExpandableBorderKey dependency property.
		/// </summary>
		public static readonly DependencyProperty IsExpandableProperty = DependencyProperty.Register(
			"IsExpandable",
			typeof(Boolean),
			typeof(NavigatorItem));

		/// <summary>
		/// Identifies the isExpandableBorderKey dependency property.
		/// </summary>
		public static readonly DependencyProperty IsRootProperty = DependencyProperty.Register(
			"IsRoot",
			typeof(Boolean),
			typeof(NavigatorItem));

		/// <summary>
		/// Initializes a new instance of the NavigatorItem class.
		/// </summary>
		public NavigatorItem()
		{

			// This control will automatically bind itself to an IExplorerItem if one exists as the data context.
			this.DataContextChanged += this.OnDataContextChanged;

		}

		/// <summary>
		/// Gets or sets whether the item allows for the display of child items.
		/// </summary>
		public Boolean IsExpandable
		{
			get
			{
				return (Boolean)this.GetValue(NavigatorItem.IsExpandableProperty);
			}
			set
			{
				this.SetValue(NavigatorItem.IsExpandableProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets whether the item is the root of a hierarchy.
		/// </summary>
		public Boolean IsRoot
		{
			get
			{
				return (Boolean)this.GetValue(NavigatorItem.IsRootProperty);
			}
			set
			{
				this.SetValue(NavigatorItem.IsRootProperty, value);
			}
		}

		/// <summary>
		/// Creates a NavigatorItem to use to display content.
		/// </summary>
		/// <returns>A new TreeViewItem to use as a container for content.</returns>
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new NavigatorItem();
		}

		/// <summary>
		/// Determines whether the specified item is its own container or can be its own container.
		/// </summary>
		/// <param name="item">The object to evaluate.</param>
		/// <returns>true if item is a TreeViewItem; otherwise, false.</returns>
		protected override Boolean IsItemItsOwnContainerOverride(Object item)
		{
			return item is NavigatorItem;
		}

		/// <summary>
		/// Represents the method that will handle events raised when the DataContext Property is changed on a particular DependencyObject implementation.
		/// </summary>
		/// <param name="sender">The source of the event (typically the object where the property changed).</param>
		/// <param name="dependencyPropertyChangedEventArgs">The event data.</param>
		[SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
		void OnDataContextChanged(Object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the specific arguments from the generic event arguments.
			IExplorerItem newExplorerItem = dependencyPropertyChangedEventArgs.NewValue as IExplorerItem;
			IExplorerItem oldExplorerItem = dependencyPropertyChangedEventArgs.OldValue as IExplorerItem;

			// Clear the bindings when this control is no longer bound to the IExplorerItem.
			if (oldExplorerItem != null)
			{
				BindingOperations.ClearBinding(this, NavigatorItem.IsExpandableProperty);
				BindingOperations.ClearBinding(this, NavigatorItem.IsExpandedProperty);
				BindingOperations.ClearBinding(this, NavigatorItem.IsSelectedProperty);
				BindingOperations.ClearBinding(this, NavigatorItem.IsRootProperty);
			}

			// This control will bind itself automatically to the properties of the IExplorerItem when it becomes the data context.  The advantage of peforming the
			// binding here instead of in the XAML is that there is no guarantee that an IExplorerItem will be the data context and this allows other data be be
			// used with the controls without it generating massive binding errors.
			if (newExplorerItem != null)
			{

				// The IsExpandable Property binding.
				Binding isExpandableBinding = new Binding();
				isExpandableBinding.Path = new PropertyPath("IsExpandable");
				isExpandableBinding.Source = newExplorerItem;
				BindingOperations.SetBinding(this, NavigatorItem.IsExpandableProperty, isExpandableBinding);

				// The IsExpanded Property binding.
				Binding isExpandedBinding = new Binding();
				isExpandedBinding.Path = new PropertyPath("IsExpanded");
				isExpandedBinding.Source = newExplorerItem;
				isExpandedBinding.Mode = BindingMode.TwoWay;
				BindingOperations.SetBinding(this, NavigatorItem.IsExpandedProperty, isExpandedBinding);

				// The IsSelected Property binding.
				Binding isSelectedBinding = new Binding();
				isSelectedBinding.Path = new PropertyPath("IsSelected");
				isSelectedBinding.Source = newExplorerItem;
				isSelectedBinding.Mode = BindingMode.TwoWay;
				BindingOperations.SetBinding(this, NavigatorItem.IsSelectedProperty, isSelectedBinding);

				// The IsRoot Property binding.
				Binding isRootBinding = new Binding();
				isRootBinding.Path = new PropertyPath("IsRoot");
				isRootBinding.Source = newExplorerItem;
				BindingOperations.SetBinding(this, NavigatorItem.IsRootProperty, isRootBinding);

			}

		}

		/// <summary>
		/// Provides class handling for a MouseLeftButtonDown event.
		/// </summary>
		/// <param name="e">The event data.</param>
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{

			// Validate the arguments.
			if (e == null)
				throw new ArgumentNullException("e");

			// Only allow the NavigatorItem to be expanded if it allows for expansion.  Some items may want to maintain a logical hierarchy of items in order to
			// provide the proper data context for some controls downstream, but not allow for the expansion of a tree view item.  For example, the folders of a
			// directory structure would be visible and expandable in a Navigator, but the individual files would not.
			if (!this.IsExpandable && e.ClickCount % 2 == 0)
				e.Handled = true;

			// Allow the base class to handle the rest of the event.
			base.OnMouseLeftButtonDown(e);

		}

	}

}
