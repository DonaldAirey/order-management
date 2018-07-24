namespace Teraque.Windows.Controls
{

	using System;
	using System.ComponentModel;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Input;

	/// <summary>
	/// A view used to display items as icons.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[TemplatePart(Name = IconsView.partBorder, Type = typeof(Border))]
	[TemplatePart(Name = IconsView.partImage, Type = typeof(Image))]
	public abstract class IconsView : ItemsView
	{

		/// <summary>
		/// Identifies the IconSize dependency property.
		/// </summary>
		public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
			"IconSize",
			typeof(Double),
			typeof(IconsView));

		/// <summary>
		/// Identifies the ItemWidth dependency property.
		/// </summary>
		public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(
			"ItemWidth",
			typeof(Double),
			typeof(IconsView));

		/// <summary>
		/// The name of the Border part.
		/// </summary>
		const String partBorder = "PART_Border";

		/// <summary>
		/// The name of the Image part.
		/// </summary>
		const String partImage = "PART_Image";

		/// <summary>
		/// Gets or sets the size of the icon in this view.
		/// </summary>
		[Bindable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Double IconSize
		{
			get
			{
				return (Double)this.GetValue(IconsView.IconSizeProperty);
			}
			set
			{
				this.SetValue(IconsView.IconSizeProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the width of the border on the item.
		/// </summary>
		[Bindable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Double ItemWidth
		{
			get
			{
				return (Double)this.GetValue(IconsView.ItemWidthProperty);
			}
			set
			{
				this.SetValue(IconsView.ItemWidthProperty, value);
			}
		}

		/// <summary>
		/// Occurs when the element is laid out, rendered, and ready for interaction.
		/// </summary>
		/// <param name="listViewItem">The item that has been loaded into the view.</param>
		protected override void OnItemLoaded(ListViewItem listViewItem)
		{

            // Validate parameters
            if (listViewItem == null)
                throw new ArgumentNullException("listViewItem");

            // This will bind the IconSize property to the image (if the part exists) in the container so all the items can grow and shrink as the IconSize property
			// is altered.
			Image image = listViewItem.Template.FindName(IconsView.partImage, listViewItem) as Image;
			if (image != null)
			{

				// Bind the height of the image to the IconSize property.  This allows a single setting to control the size of all icons.
				Binding iconHeightBinding = new Binding();
				iconHeightBinding.Path = new PropertyPath("IconSize");
				iconHeightBinding.Source = this;
				BindingOperations.SetBinding(image, Image.HeightProperty, iconHeightBinding);

				// Bind the width of the image to the IconSize property.  This allows a single setting to control the size of all icons.
				Binding iconWidthBinding = new Binding();
				iconWidthBinding.Path = new PropertyPath("IconSize");
				iconWidthBinding.Source = this;
				BindingOperations.SetBinding(image, Image.WidthProperty, iconWidthBinding);

			}

			// This allows the width of the item to be set by the view.  Because the width of the item is based on two parameters -- a minimum width and the width
			// of the icon plus a margin -- it is not easy to calculate the total width of the element ahead of time.  A previous iteration of this design tried to
			// use the actual width of the grid that held the image to calculate the width of the TextBlock where the name of the item was displayed, but this
			// proved to be visually cumbersome as it would display the element once with the text spread out all over the place.  Then when the actual width of the
			// image grid was calculated, the text would occupy the expected area.  This chicken-before-the-egg problem was resolved by fixing the width of the
			// element in code.  The width of the element is attached to the view here and the view will find the width from a predefined table of widths.
			Border border = listViewItem.Template.FindName(IconsView.partBorder, listViewItem) as Border;
			if (border != null)
			{

				// Bind the width of border to this view.  That way, changing one value changes the widths of all the elements in the display.
				Binding itemWidthBinding = new Binding();
				itemWidthBinding.Path = new PropertyPath("ItemWidth");
				itemWidthBinding.Source = this;
				BindingOperations.SetBinding(border, Border.WidthProperty, itemWidthBinding);

			}

		}

		/// <summary>
		/// Occurs when the element is removed from within an element tree of loaded elements. 
		/// </summary>
		/// <param name="listViewItem">The item that has been removed from the view.</param>
		protected override void OnItemUnloaded(ListViewItem listViewItem)
		{

            // Validate parameters
            if (listViewItem == null)
                throw new ArgumentNullException("listViewItem");

            // The binding to the image element must be cleared when the item is unloaded or binding errors will be emitted.
			Image image = listViewItem.Template.FindName(partImage, listViewItem) as Image;
			if (image != null)
			{
				BindingOperations.ClearBinding(image, Image.HeightProperty);
				BindingOperations.ClearBinding(image, Image.WidthProperty);
			}

			// The binding to the border element must be cleared when the item is unloaded or binding errors will be emitted.
			Border border = listViewItem.Template.FindName(partBorder, listViewItem) as Border;
			if (border != null)
				BindingOperations.ClearBinding(border, Border.MarginProperty);

		}

	}

}
