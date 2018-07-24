namespace Teraque.Windows.Controls
{

	using System;
	using System.ComponentModel;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Reflection;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Controls.Primitives;
	using System.Windows.Data;
	using System.Windows.Media;
	using System.Windows.Input;

	/// <summary>
	/// A general purpose control that can be morphed into a button, a menu item, a split button, etc.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[TemplatePart(Name = Gadget.partButton, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = Gadget.partMenuItem, Type = typeof(FrameworkElement))]
	public class Gadget : MenuItem
	{

		/// <summary>
		/// Identifies the ButtonFocusedBorder dependency property.
		/// </summary>
		public static readonly DependencyProperty ButtonFocusedBorderProperty = DependencyProperty.Register(
			"ButtonFocusedBorder",
			typeof(ControlTemplate),
			typeof(Gadget));

		/// <summary>
		/// Identifies the ButtonHighlightBorder dependency property.
		/// </summary>
		public static readonly DependencyProperty ButtonHighlightBorderProperty = DependencyProperty.Register(
			"ButtonHighlightBorder",
			typeof(ControlTemplate),
			typeof(Gadget));

		/// <summary>
		/// Identifies the ButtonNormalBorder dependency property.
		/// </summary>
		public static readonly DependencyProperty ButtonNormalBorderProperty = DependencyProperty.Register(
			"ButtonNormalBorder",
			typeof(ControlTemplate),
			typeof(Gadget));

		/// <summary>
		/// Identifies the ButtonPressedBorder dependency property.
		/// </summary>
		public static readonly DependencyProperty ButtonPressedBorderProperty = DependencyProperty.Register(
			"ButtonPressedBorder",
			typeof(ControlTemplate),
			typeof(Gadget));

		/// <summary>
		/// Identifies the ContentPadding dependency property key.
		/// </summary>
		public static readonly DependencyProperty ContentPaddingProperty = DependencyProperty.Register(
			"ContentPadding",
			typeof(Thickness),
			typeof(Gadget));

		/// <summary>
		/// Identifies the ContentPressedTransform dependency property.
		/// </summary>
		public static readonly DependencyProperty ContentPressedTransformProperty = DependencyProperty.Register(
			"ContentPressedTransform",
			typeof(Transform),
			typeof(Gadget));

		/// <summary>
		/// Identifies the CheckGlyph dependency property.
		/// </summary>
		public static readonly DependencyProperty CheckGlyphProperty = DependencyProperty.Register(
			"CheckGlyph",
			typeof(ImageSource),
			typeof(Gadget));

		/// <summary>
		/// Identifies the DropDown dependency property.
		/// </summary>
		public readonly static DependencyProperty DropDownProperty = DependencyProperty.Register(
			"DropDown",
			typeof(ControlTemplate),
			typeof(Gadget),
			new FrameworkPropertyMetadata(null, Gadget.OnDropDownPropertyChanged));

		/// <summary>
		/// Identifies the FocusedOpacity dependency property.
		/// </summary>
		public static readonly DependencyProperty FocusedOpacityProperty = DependencyProperty.Register(
			"FocusedOpacity",
			typeof(Double),
			typeof(Gadget));

		/// <summary>
		/// Identifies the HasAsNeededItems dependency property.
		/// </summary>
		public static readonly DependencyProperty HasAsNeededItemsProperty;

		/// <summary>
		/// Identifies the HasAsNeededItems dependency property key.
		/// </summary>
		static readonly DependencyPropertyKey hasAsNeededItemsPropertyKey = DependencyProperty.RegisterReadOnly(
			"HasAsNeededItems",
			typeof(Boolean),
			typeof(Gadget),
			new FrameworkPropertyMetadata(
				false,
				FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>
		/// Identifies the HasOverflowItems dependency property.
		/// </summary>
		public static readonly DependencyProperty HasOverflowItemsProperty;

		/// <summary>
		/// Identifies the HasOverflowItems dependency property key.
		/// </summary>
		static readonly DependencyPropertyKey hasOverflowItemsPropertyKey = DependencyProperty.RegisterReadOnly(
			"HasOverflowItems",
			typeof(Boolean),
			typeof(OverflowItem),
			new FrameworkPropertyMetadata(
				false,
				FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>
		/// Identifies the HeaderMargin dependency property key.
		/// </summary>
		public static readonly DependencyProperty HeaderMarginProperty = DependencyProperty.Register(
			"HeaderMargin",
			typeof(Thickness),
			typeof(Gadget));

		/// <summary>
		/// Identifies the HeaderVisibility dependency property key.
		/// </summary>
		public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register(
			"HeaderVisibility",
			typeof(Visibility),
			typeof(Gadget));

		/// <summary>
		/// Identifies the HighlightOpacity dependency property.
		/// </summary>
		public static readonly DependencyProperty HighlightOpacityProperty = DependencyProperty.Register(
			"HighlightOpacity",
			typeof(Double),
			typeof(Gadget));

		/// <summary>
		/// Identifies the HorizontalOffset dependency property.
		/// </summary>
		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(
			"HorizontalOffset",
			typeof(double),
			typeof(Gadget));

		/// <summary>
		/// Identifies the IconColumnWidth dependency property key.
		/// </summary>
		public static readonly DependencyProperty IconColumnWidthProperty = DependencyProperty.Register(
			"IconColumnWidth",
			typeof(Double),
			typeof(Gadget));

		/// <summary>
		/// Identifies the IconMargin dependency property key.
		/// </summary>
		public static readonly DependencyProperty IconMarginProperty = DependencyProperty.Register(
			"IconMargin",
			typeof(Thickness),
			typeof(Gadget));

		/// <summary>
		/// Identifies the IconVisibility dependency property key.
		/// </summary>
		public static readonly DependencyProperty IconVisibilityProperty = DependencyProperty.Register(
			"IconVisibility",
			typeof(Visibility),
			typeof(Gadget),
			new FrameworkPropertyMetadata(Visibility.Visible));

		/// <summary>
		/// Identifies the IsButton dependency property.
		/// </summary>
		public static readonly DependencyProperty IsButtonProperty = DependencyProperty.Register(
			"IsButton",
			typeof(Boolean),
			typeof(Gadget));

		/// <summary>
		/// Identifies the isExpandableBorderKey dependency property.
		/// </summary>
		public static readonly DependencyProperty IsExpandableProperty = DependencyProperty.Register(
			"IsExpandable",
			typeof(Boolean),
			typeof(Gadget));

		/// <summary>
		/// Identifies the IsHighlightAnimated dependency property.
		/// </summary>
		public static readonly DependencyProperty IsHighlightAnimatedProperty = DependencyProperty.Register(
			"IsHighlightAnimated",
			typeof(Boolean),
			typeof(Gadget));

		/// <summary>
		/// Identifies the IsMenuItem dependency property.
		/// </summary>
		public static readonly DependencyProperty IsMenuItemProperty = DependencyProperty.Register(
			"IsMenuItem",
			typeof(Boolean),
			typeof(Gadget),
			new FrameworkPropertyMetadata(true));

		/// <summary>
		/// Identifies the IsMouseOverButton dependency property.
		/// </summary>
		static DependencyProperty IsMouseOverButtonProperty;

		/// <summary>
		/// Identifies the IsMouseOverButton dependency property key.
		/// </summary>
		static DependencyPropertyKey isMouseOverButtonPropertyKey = DependencyProperty.RegisterReadOnly(
			"IsMouseOverButton",
			typeof(Boolean),
			typeof(Gadget),
			new FrameworkPropertyMetadata(false));

		/// <summary>
		/// Identifies the IsMouseOverMenuItem dependency property.
		/// </summary>
		static DependencyProperty IsMouseOverMenuItemProperty;

		/// <summary>
		/// Identifies the IsMouseOverMenuItem dependency property key.
		/// </summary>
		static DependencyPropertyKey isMouseOverMenuItemPropertyKey = DependencyProperty.RegisterReadOnly(
			"IsMouseOverMenuItem",
			typeof(Boolean),
			typeof(Gadget),
			new FrameworkPropertyMetadata(false));

		/// <summary>
		/// Identifies the IsParent dependency property key.
		/// </summary>
		public static readonly DependencyProperty IsParentProperty = DependencyProperty.Register(
			"IsParent",
			typeof(Boolean),
			typeof(Gadget),
			new FrameworkPropertyMetadata(false));

		/// <summary>
		/// Identifies the MenuItemFocusedBorder dependency property.
		/// </summary>
		public static readonly DependencyProperty MenuItemFocusedBorderProperty = DependencyProperty.Register(
			"MenuItemFocusedBorder",
			typeof(ControlTemplate),
			typeof(Gadget));

		/// <summary>
		/// Identifies the MenuItemHighlightBorder dependency property.
		/// </summary>
		public static readonly DependencyProperty MenuItemHighlightBorderProperty = DependencyProperty.Register(
			"MenuItemHighlightBorder",
			typeof(ControlTemplate),
			typeof(Gadget));

		/// <summary>
		/// Identifies the MenuItemNormalBorder dependency property.
		/// </summary>
		public static readonly DependencyProperty MenuItemNormalBorderProperty = DependencyProperty.Register(
			"MenuItemNormalBorder",
			typeof(ControlTemplate),
			typeof(Gadget));

		/// <summary>
		/// Identifies the MenuItemPressedBorder dependency property.
		/// </summary>
		public static readonly DependencyProperty MenuItemPressedBorderProperty = DependencyProperty.Register(
			"MenuItemPressedBorder",
			typeof(ControlTemplate),
			typeof(Gadget));

		/// <summary>
		/// Identifies the NormalOpacity dependency property.
		/// </summary>
		public static readonly DependencyProperty NormalOpacityProperty = DependencyProperty.Register(
			"NormalOpacity",
			typeof(Double),
			typeof(Gadget),
			new FrameworkPropertyMetadata(1.0));

		/// <summary>
		/// Identifies the OpenColumnWidth dependency property key.
		/// </summary>
		public static readonly DependencyProperty OpenColumnWidthProperty = DependencyProperty.Register(
			"OpenColumnWidth",
			typeof(Double),
			typeof(Gadget));

		/// <summary>
		/// Identifies the OpenGlyph dependency property.
		/// </summary>
		public static readonly DependencyProperty OpenGlyphProperty = DependencyProperty.Register(
			"OpenGlyph",
			typeof(ImageSource),
			typeof(Gadget));

		/// <summary>
		/// Identifies the OpenPadding dependency property key.
		/// </summary>
		public static readonly DependencyProperty OpenPaddingProperty = DependencyProperty.Register(
			"OpenPadding",
			typeof(Thickness),
			typeof(Gadget));

		/// <summary>
		/// Identifies the OpenPressedTransform dependency property.
		/// </summary>
		public static readonly DependencyProperty OpenPressedTransformProperty = DependencyProperty.Register(
			"OpenPressedTransform",
			typeof(Transform),
			typeof(Gadget));

		/// <summary>
		/// Identifies the OpenVisibility dependency property key.
		/// </summary>
		public static readonly DependencyProperty OpenVisibilityProperty = DependencyProperty.Register(
			"OpenVisibility",
			typeof(Visibility),
			typeof(Gadget));

		/// <summary>
		/// Identifies the OverflowGlyph dependency property.
		/// </summary>
		public static readonly DependencyProperty OverflowGlyphProperty = DependencyProperty.Register(
			"OverflowGlyph",
			typeof(ImageSource),
			typeof(Gadget));

		/// <summary>
		/// The name of the ComboBox part of the custom control.
		/// </summary>
		const String partButton = "PART_Button";

		/// <summary>
		/// The name of the ComboBox part of the custom control.
		/// </summary>
		const String partMenuItem = "PART_MenuItem";

		/// <summary>
		/// Identifies the Placement dependency property.
		/// </summary>
		public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(
			"Placement",
			typeof(PlacementMode),
			typeof(Gadget),
			new FrameworkPropertyMetadata(PlacementMode.Bottom));

		/// <summary>
		/// Identifies the PressedGlyph dependency property.
		/// </summary>
		public static readonly DependencyProperty PressedGlyphProperty = DependencyProperty.Register(
			"PressedGlyph",
			typeof(ImageSource),
			typeof(Gadget));

		/// <summary>
		/// Identifies the PressedOpacity dependency property.
		/// </summary>
		public static readonly DependencyProperty PressedOpacityProperty = DependencyProperty.Register(
			"PressedOpacity",
			typeof(Double),
			typeof(Gadget));

		/// <summary>
		/// 
		/// </summary>
		public new static readonly DependencyProperty RoleProperty;

		/// <summary>
		/// 
		/// </summary>
		static readonly DependencyPropertyKey rolePropertyKey = DependencyProperty.RegisterReadOnly(
			"Role",
			typeof(MenuItemRole),
			typeof(Gadget),
			new FrameworkPropertyMetadata(MenuItemRole.TopLevelItem));

		/// <summary>
		/// Identifies the SubmenuCheckGlyph dependency property.
		/// </summary>
		public static readonly DependencyProperty SubmenuCheckGlyphProperty = DependencyProperty.Register(
			"SubmenuCheckGlyph",
			typeof(ImageSource),
			typeof(Gadget));

		/// <summary>
		/// Identifies the SubmenuOpenGlyph dependency property.
		/// </summary>
		public static readonly DependencyProperty SubmenuOpenGlyphProperty = DependencyProperty.Register(
			"SubmenuOpenGlyph",
			typeof(ImageSource),
			typeof(Gadget));

		/// <summary>
		/// Identifies the SubmenuHeaderMargin dependency property key.
		/// </summary>
		public static readonly DependencyProperty SubmenuHeaderMarginProperty = DependencyProperty.Register(
			"SubmenuHeaderMargin",
			typeof(Thickness),
			typeof(Gadget));

		/// <summary>
		/// Identifies the SubmenuIconMargin dependency property key.
		/// </summary>
		public static readonly DependencyProperty SubmenuIconMarginProperty = DependencyProperty.Register(
			"SubmenuIconMargin",
			typeof(Thickness),
			typeof(Gadget));

		/// <summary>
		/// Identifies the SubmenuPressedGlyph dependency property.
		/// </summary>
		public static readonly DependencyProperty SubmenuPressedGlyphProperty = DependencyProperty.Register(
			"SubmenuPressedGlyph",
			typeof(ImageSource),
			typeof(Gadget));

		/// <summary>
		/// Identifies the OpenGlyphMargin dependency property key.
		/// </summary>
		public static readonly DependencyProperty SubmenuOpenPaddingProperty = DependencyProperty.Register(
			"OpenGlyphMargin",
			typeof(Thickness),
			typeof(Gadget));

		/// <summary>
		/// Identifies the VerticalOffset dependency property.
		/// </summary>
		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(
			"VerticalOffset",
			typeof(Double),
			typeof(Gadget));

		/// <summary>
		/// Initializes the Gadget class.
		/// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static Gadget()
		{

			// This allows the templates in resource files to be properly associated with this new class.  Without this override, the type of the base class would
			// be used as the key in any lookup involving resources dictionaries.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Gadget), new FrameworkPropertyMetadata(typeof(Gadget)));

            // These properties are extracted from the property keys in the static constructor because there is no guarantee of the order when the are initialized
            // using the fields.  That is, if a key is declared before the property, then it will initialized properly.  If the key is declared after the property,
            // it will not because of the forward reference.  This code pattern guarantees the proper initialization of the properties no matter what the order of
            // the fields in the declaration.
            Gadget.HasAsNeededItemsProperty = Gadget.hasAsNeededItemsPropertyKey.DependencyProperty;
			Gadget.HasOverflowItemsProperty = Gadget.hasOverflowItemsPropertyKey.DependencyProperty;
			Gadget.IsMouseOverButtonProperty = Gadget.isMouseOverButtonPropertyKey.DependencyProperty;
			Gadget.IsMouseOverMenuItemProperty = Gadget.isMouseOverMenuItemPropertyKey.DependencyProperty;
			Gadget.RoleProperty = Gadget.rolePropertyKey.DependencyProperty;

		}

		/// <summary>
		/// 
		/// </summary>
		public Gadget()
		{

			// This control will automatically bind itself to an IExplorerItem if one exists as the data context.
			this.DataContextChanged += this.OnDataContextChanged;

		}

		/// <summary>
		/// Gets or sets the ButtonFocusedBorder control template.
		/// </summary>
		public ControlTemplate ButtonFocusedBorder
		{
			get
			{
				return this.GetValue(Gadget.ButtonFocusedBorderProperty) as ControlTemplate;
			}
			set
			{
				this.SetValue(Gadget.ButtonFocusedBorderProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the ButtonHighlightBorder control template.
		/// </summary>
		public ControlTemplate ButtonHighlightBorder
		{
			get
			{
				return this.GetValue(Gadget.ButtonHighlightBorderProperty) as ControlTemplate;
			}
			set
			{
				this.SetValue(Gadget.ButtonHighlightBorderProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the ButtonNormalBorder control template.
		/// </summary>
		public ControlTemplate ButtonNormalBorder
		{
			get
			{
				return this.GetValue(Gadget.ButtonNormalBorderProperty) as ControlTemplate;
			}
			set
			{
				this.SetValue(Gadget.ButtonNormalBorderProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the ButtonPressedBorder control template.
		/// </summary>
		public ControlTemplate ButtonPressedBorder
		{
			get
			{
				return this.GetValue(Gadget.ButtonPressedBorderProperty) as ControlTemplate;
			}
			set
			{
				this.SetValue(Gadget.ButtonPressedBorderProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the glyph that indicates the item has been checked.
		/// </summary>
		public ImageSource CheckGlyph
		{
			get
			{
				return (ImageSource)this.GetValue(Gadget.CheckGlyphProperty);
			}
			set
			{
				this.SetValue(Gadget.CheckGlyphProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the padding around the glyph used to open up the next level.
		/// </summary>
		public Thickness ContentPadding
		{
			get
			{
				return (Thickness)this.GetValue(Gadget.ContentPaddingProperty);
			}
			set
			{
				this.SetValue(Gadget.ContentPaddingProperty, value);
			}
		}

		/// <summary>
		/// Get or sets the transform applied to the content of the item when it is pressed.
		/// </summary>
		public Double ContentPressedTransform
		{
			get
			{
				return (Double)this.GetValue(Gadget.ContentPressedTransformProperty);
			}
			set
			{
				this.SetValue(Gadget.ContentPressedTransformProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the control presented when the drop down is selected.
		/// </summary>
		public ControlTemplate DropDown
		{
			get
			{
				return (ControlTemplate)this.GetValue(Gadget.DropDownProperty);
			}
			set
			{
				this.SetValue(Gadget.DropDownProperty, value);
			}
		}

		/// <summary>
		/// Get or sets the the opacity of the item when it's pressed.
		/// </summary>
		public Double FocusedOpacity
		{
			get
			{
				return (Double)this.GetValue(Gadget.FocusedOpacityProperty);
			}
			set
			{
				this.SetValue(Gadget.FocusedOpacityProperty, value);
			}
		}

		/// <summary>
		/// Gets an indication of whether or not the control has items in its overflow panel.
		/// </summary>
		public Boolean HasAsNeededItems
		{
			get
			{
				return (Boolean)this.GetValue(OverflowItem.HasAsNeededItemsProperty);
			}
		}

		/// <summary>
		/// Gets an indication of whether or not the control has items in its overflow panel.
		/// </summary>
		public Boolean HasOverflowItems
		{
			get
			{
				return (Boolean)this.GetValue(OverflowItem.HasOverflowItemsProperty);
			}
		}

		/// <summary>
		/// Gets or sets the HeaderMargin.
		/// </summary>
		public Thickness HeaderMargin
		{
			get
			{
				return (Thickness)this.GetValue(Gadget.HeaderMarginProperty);
			}
			set
			{
				this.SetValue(Gadget.HeaderMarginProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the HeaderVisibility control template.
		/// </summary>
		public Visibility HeaderVisibility
		{
			get
			{
				return (Visibility)this.GetValue(Gadget.HeaderVisibilityProperty);
			}
			set
			{
				this.SetValue(Gadget.HeaderVisibilityProperty, value);
			}
		}

		/// <summary>
		/// Get or sets the the opacity of the item when it's pressed.
		/// </summary>
		public Double HighlightOpacity
		{
			get
			{
				return (Double)this.GetValue(Gadget.HighlightOpacityProperty);
			}
			set
			{
				this.SetValue(Gadget.HighlightOpacityProperty, value);
			}
		}

		/// <summary>
		/// Get or sets the horizontal distance between the target origin and the popup alignment point.
		/// </summary>
		public Double HorizontalOffset
		{
			get
			{
				return (Double)this.GetValue(Gadget.HorizontalOffsetProperty);
			}
			set
			{
				this.SetValue(Gadget.HorizontalOffsetProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the minimum width of the column in the submenu where the icons are displayed.
		/// </summary>
		public Double IconColumnWidth
		{
			get
			{
				return (Double)this.GetValue(Gadget.IconColumnWidthProperty);
			}
			set
			{
				this.SetValue(Gadget.IconColumnWidthProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the margin around the top level icon.
		/// </summary>
		public Thickness IconMargin
		{
			get
			{
				return (Thickness)this.GetValue(Gadget.IconMarginProperty);
			}
			set
			{
				this.SetValue(Gadget.IconMarginProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the IconVisibility control template.
		/// </summary>
		public Visibility IconVisibility
		{
			get
			{
				return (Visibility)this.GetValue(Gadget.IconVisibilityProperty);
			}
			set
			{
				this.SetValue(Gadget.IconVisibilityProperty, value);
			}
		}

		/// <summary>
		/// Gets an indication that the mouse is over the button part of the control.
		/// </summary>
		public Boolean IsButton
		{
			get
			{
				return (Boolean)this.GetValue(Gadget.IsButtonProperty);
			}
			set
			{
				this.SetValue(Gadget.IsButtonProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets whether the item allows for the display of child items.
		/// </summary>
		public Boolean IsExpandable
		{
			get
			{
				return (Boolean)this.GetValue(Gadget.IsExpandableProperty);
			}
			set
			{
				this.SetValue(Gadget.IsExpandableProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets an indication that the highlight should be animated.
		/// </summary>
		public Boolean IsHighlightAnimated
		{
			get
			{
				return (Boolean)this.GetValue(Gadget.IsHighlightAnimatedProperty);
			}
			set
			{
				this.SetValue(Gadget.IsHighlightAnimatedProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets an indication that the item acts like a MenuItem.
		/// </summary>
		public Boolean IsMenuItem
		{
			get
			{
				return (Boolean)this.GetValue(Gadget.IsMenuItemProperty);
			}
			set
			{
				this.SetValue(Gadget.IsMenuItemProperty, value);
			}
		}

		/// <summary>
		/// Gets an indication that the mouse is over the button part of the control.
		/// </summary>
		public Boolean IsMouseOverButton
		{
			get
			{
				return (Boolean)this.GetValue(Gadget.IsMouseOverButtonProperty);
			}
		}

		/// <summary>
		/// Gets an indication that the mouse is over the drop down button part of the control.
		/// </summary>
		public Boolean IsMouseOverMenuItem
		{
			get
			{
				return (Boolean)this.GetValue(Gadget.IsMouseOverMenuItemProperty);
			}
		}

		/// <summary>
		/// Gets or sets an indication that the item is a lexical parent of another item.
		/// </summary>
		public Boolean IsParent
		{
			get
			{
				return (Boolean)this.GetValue(Gadget.IsParentProperty);
			}
			set
			{
				this.SetValue(Gadget.IsParentProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the MenuItemFocusedBorder control template.
		/// </summary>
		public ControlTemplate MenuItemFocusedBorder
		{
			get
			{
				return this.GetValue(Gadget.MenuItemFocusedBorderProperty) as ControlTemplate;
			}
			set
			{
				this.SetValue(Gadget.MenuItemFocusedBorderProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the MenuItemHighlightBorder control template.
		/// </summary>
		public ControlTemplate MenuItemHighlightBorder
		{
			get
			{
				return this.GetValue(Gadget.MenuItemHighlightBorderProperty) as ControlTemplate;
			}
			set
			{
				this.SetValue(Gadget.MenuItemHighlightBorderProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the MenuItemNormalBorder control template.
		/// </summary>
		public ControlTemplate MenuItemNormalBorder
		{
			get
			{
				return this.GetValue(Gadget.MenuItemNormalBorderProperty) as ControlTemplate;
			}
			set
			{
				this.SetValue(Gadget.MenuItemNormalBorderProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the MenuItemPressedBorder control template.
		/// </summary>
		public ControlTemplate MenuItemPressedBorder
		{
			get
			{
				return this.GetValue(Gadget.MenuItemPressedBorderProperty) as ControlTemplate;
			}
			set
			{
				this.SetValue(Gadget.MenuItemPressedBorderProperty, value);
			}
		}

		/// <summary>
		/// Get or sets the the opacity of the item when it's pressed.
		/// </summary>
		public Double NormalOpacity
		{
			get
			{
				return (Double)this.GetValue(Gadget.NormalOpacityProperty);
			}
			set
			{
				this.SetValue(Gadget.NormalOpacityProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the width of the column in a submenu used to open up the next level of menus.
		/// </summary>
		public Double OpenColumnWidth
		{
			get
			{
				return (Double)this.GetValue(Gadget.OpenColumnWidthProperty);
			}
			set
			{
				this.SetValue(Gadget.OpenColumnWidthProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the normal glyph that is used to open up the next level of the menus.
		/// </summary>
		public ImageSource OpenGlyph
		{
			get
			{
				return (ImageSource)this.GetValue(Gadget.OpenGlyphProperty);
			}
			set
			{
				this.SetValue(Gadget.OpenGlyphProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the GlyphMargin.
		/// </summary>
		public Thickness OpenPadding
		{
			get
			{
				return (Thickness)this.GetValue(Gadget.OpenPaddingProperty);
			}
			set
			{
				this.SetValue(Gadget.OpenPaddingProperty, value);
			}
		}

		/// <summary>
		/// Get or sets the transform applied to the 'Open' glyph of the item when it is pressed.
		/// </summary>
		public Double OpenPressedTransform
		{
			get
			{
				return (Double)this.GetValue(Gadget.OpenPressedTransformProperty);
			}
			set
			{
				this.SetValue(Gadget.OpenPressedTransformProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the OpenVisibility control template.
		/// </summary>
		public Visibility OpenVisibility
		{
			get
			{
				return (Visibility)this.GetValue(Gadget.OpenVisibilityProperty);
			}
			set
			{
				this.SetValue(Gadget.OpenVisibilityProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the overflow glyph that is used to open up the next level of the menus.
		/// </summary>
		public ImageSource OverflowGlyph
		{
			get
			{
				return (ImageSource)this.GetValue(Gadget.OverflowGlyphProperty);
			}
			set
			{
				this.SetValue(Gadget.OverflowGlyphProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the pressed glyph that is used to open up the next level of the menus.
		/// </summary>
		public ImageSource PressedGlyph
		{
			get
			{
				return (ImageSource)this.GetValue(Gadget.PressedGlyphProperty);
			}
			set
			{
				this.SetValue(Gadget.PressedGlyphProperty, value);
			}
		}

		/// <summary>
		/// Get or sets the the opacity of the item when it's pressed.
		/// </summary>
		public Double PressedOpacity
		{
			get
			{
				return (Double)this.GetValue(Gadget.PressedOpacityProperty);
			}
			set
			{
				this.SetValue(Gadget.PressedOpacityProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the orientation of the Popup control when the control opens, and specifies the behavior of the Popup control when it overlaps screen
		/// boundaries.
		/// </summary>
		public PlacementMode Placement
		{
			get
			{
				return (PlacementMode)this.GetValue(Gadget.PlacementProperty);
			}
			set
			{
				this.SetValue(Gadget.PlacementProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the control presented when the drop down is selected.
		/// </summary>
		public new MenuItemRole Role
		{
			get
			{
				return (MenuItemRole)this.GetValue(Gadget.RoleProperty);
			}
			set
			{
				this.SetValue(Gadget.RoleProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the SubmenuHeaderMargin.
		/// </summary>
		public Thickness SubmenuHeaderMargin
		{
			get
			{
				return (Thickness)this.GetValue(Gadget.SubmenuHeaderMarginProperty);
			}
			set
			{
				this.SetValue(Gadget.SubmenuHeaderMarginProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the margin around the top level icon.
		/// </summary>
		public Thickness SubmenuIconMargin
		{
			get
			{
				return (Thickness)this.GetValue(Gadget.SubmenuIconMarginProperty);
			}
			set
			{
				this.SetValue(Gadget.SubmenuIconMarginProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the pressed glyph that is used to open up the next level of the menus.
		/// </summary>
		public ImageSource SubmenuPressedGlyph
		{
			get
			{
				return (ImageSource)this.GetValue(Gadget.SubmenuPressedGlyphProperty);
			}
			set
			{
				this.SetValue(Gadget.SubmenuPressedGlyphProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the normal glyph that is used to open up the next level of the menus.
		/// </summary>
		public ImageSource SubmenuOpenGlyph
		{
			get
			{
				return (ImageSource)this.GetValue(Gadget.SubmenuOpenGlyphProperty);
			}
			set
			{
				this.SetValue(Gadget.SubmenuOpenGlyphProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the GlyphMargin.
		/// </summary>
		public Thickness SubmenuOpenPadding
		{
			get
			{
				return (Thickness)this.GetValue(Gadget.SubmenuOpenPaddingProperty);
			}
			set
			{
				this.SetValue(Gadget.SubmenuOpenPaddingProperty, value);
			}
		}

		/// <summary>
		/// Get or sets the horizontal distance between the target origin and the popup alignment point.
		/// </summary>
		public Double VerticalOffset
		{
			get
			{
				return (Double)this.GetValue(Gadget.VerticalOffsetProperty);
			}
			set
			{
				this.SetValue(Gadget.VerticalOffsetProperty, value);
			}
		}

		/// <summary>
		/// Execute the command associated with this control.
		/// </summary>
		void ExecuteCommand()
		{

			// The Gadget can act both as a drop down menu item and a button.  When the button part of the Gadget it hit, this command will be executed the same way
			// it would be if this control was a button.
			if (this.Command != null)
				this.Command.Execute(this.CommandParameter);

		}

		/// <summary>
		/// Determines whether the given point is over the button part of the Gadget.
		/// </summary>
		/// <param name="point">A point to be tested.</param>
		/// <returns>True if the given point is over the drop down part of the control, false otherwise.</returns>
		protected Boolean IsMouseOverButtonPart(Point point)
		{

			// This will construct the rectangle that contains the MenuItem part of the control and test to see whether the given point (corrected to use the 
			// origin of the Gadget) lies within the control.
			UIElement dropDownButtonPart = this.GetTemplateChild(Gadget.partButton) as UIElement;
			if (dropDownButtonPart == null)
				return false;
			Rect rect = new Rect(dropDownButtonPart.TranslatePoint(new Point(), this), dropDownButtonPart.RenderSize);
			return rect.Contains(point);

		}

		/// <summary>
		/// Determines whether the given point is over the drop down button part of the Gadget.
		/// </summary>
		/// <param name="point">A point to be tested.</param>
		/// <returns>True if the given point is over the drop down part of the control, false otherwise.</returns>
		protected Boolean IsMouseOverMenuItemPart(Point point)
		{

			// This will construct the rectangle that contains the MenuItem part of the control and test to see whether the given point (corrected to use the 
			// origin of the Gadget) lies within the control.
			UIElement dropDownButtonPart = this.GetTemplateChild(Gadget.partMenuItem) as UIElement;
			if (dropDownButtonPart == null)
				return false;
			Rect rect = new Rect(dropDownButtonPart.TranslatePoint(new Point(), this), dropDownButtonPart.RenderSize);
			return rect.Contains(point);

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
				BindingOperations.ClearBinding(this, NavigatorItem.IsExpandableProperty);

			// This control will bind itself automatically to the properties of the IExplorerItem when it becomes the data context.  The advantage of peforming the
			// binding here instead of in the XAML is that there is no guarantee that an IExplorerItem will be the data context and this allows other data be be
			// used with the controls without it generating massive binding errors.
			if (newExplorerItem != null)
			{

				// The IsExpandable Property binding.
				Binding isExpandableBinding = new Binding();
				isExpandableBinding.Path = new PropertyPath("IsExpandable");
				isExpandableBinding.Source = newExplorerItem;
				BindingOperations.SetBinding(this, Gadget.IsExpandableProperty, isExpandableBinding);

			}

		}

		/// <summary>
		/// Invoked when the effective property value of the DropDown property changes.
		/// </summary>
		/// <param name="dependencyObject">The DependencyObject on which the property has changed value.</param>
		/// <param name="dependencyPropertyChangedEventArgs">Event data that is issued by any event that tracks changes to the effective value of this
		/// property.</param>
		static void OnDropDownPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// Extract the Gadget and the new view mode from the property change event arguments.
			Gadget gadget = dependencyObject as Gadget;
			ControlTemplate controlTemplate = dependencyPropertyChangedEventArgs.NewValue as ControlTemplate;

			// The control specified in the drop down will become the one and only child of this window.
			Control newDropDown = null;
			Control oldDropDown = gadget.Items.Count == 0 ? null : gadget.Items[0] as Control;

			// This will clear out any content that may have been generated previously.
			gadget.Items.Clear();

			// Gadgets can define their own content for the drop down area.  This would be like a calender control where you want all the user interface handling at
			// the top level of a menu, but when you click the 'Open' button, you get something that is not based on a list of items, such as a calendar or the view
			// mode selection.  This will create a custom content for the menu.  There will be only one child item when a control is created this way and all the
			// item selection logic will be inhibited.  The control that is generated from this template should be able to handle getting the focus and then it
			// should be able to manage the user interface until the focus is surrendered.
			if (controlTemplate != null)
			{
				Type targetType = controlTemplate.TargetType == null ? typeof(Control) : controlTemplate.TargetType;
				newDropDown = Activator.CreateInstance(targetType) as Control;
				newDropDown.Template = controlTemplate;
				gadget.Items.Add(newDropDown);
				newDropDown.ApplyTemplate();
			}

			// This will broadcast the property change to anyone listening.
			gadget.OnDropDownChanged(oldDropDown, newDropDown);

		}

		/// <summary>
		/// Invoked when the drop down template changes.
		/// </summary>
		/// <param name="oldDropDown">The old control for the drop down part of the Gadget.</param>
		/// <param name="newDropDown">The new control for the drop down part of the Gadget.</param>
		protected virtual void OnDropDownChanged(Control oldDropDown, Control newDropDown)
		{
		}

		/// <summary>
		/// Called whenever the mouse enters a Gadget.
		/// </summary>
		/// <param name="e">The event data for the MouseEnter event.</param>
		protected override void OnMouseEnter(MouseEventArgs e)
		{

            // Validate parameters
            if (e == null)
                throw new ArgumentNullException("e");

			// This property will indicate that the mouse is over the drop down part of the control.
			this.SetValue(Gadget.isMouseOverButtonPropertyKey, this.IsMouseOverButtonPart(e.GetPosition(this)));

			// This property will indicate that the mouse is over the button part of the control.
			this.SetValue(Gadget.isMouseOverMenuItemPropertyKey, this.IsMouseOverMenuItemPart(e.GetPosition(this)));

            // The base class has significant processing on this event which must be completed.
            base.OnMouseEnter(e);

        }

		/// <summary>
		/// Called whenever the mouse leaves a Gadget.
		/// </summary>
		/// <param name="e">The event data for the MouseLeave event.</param>
		protected override void OnMouseLeave(MouseEventArgs e)
		{

			// Allow the base class to handle the event first.
			base.OnMouseLeave(e);

			// The mouse has left the bulding.  This indicates that the mouse is no longer over any part of the control.  It is possible to remove one of these
			// properties and use the other in conjunction with the 'MouseOver' property but that design requires more 'MultiTrigger' statements in the XAML.  The 
			// current design appears to result in less complex XAML code which is always a good thing.
			this.SetValue(Gadget.isMouseOverButtonPropertyKey, false);
			this.SetValue(Gadget.isMouseOverMenuItemPropertyKey, false);

		}

		/// <summary>
		/// Called when the left mouse button is pressed.
		/// </summary>
		/// <param name="e">The event data for the MouseLeftButtonDown event.</param>
		[SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase")]
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{

			// Validate the arguments.
			if (e == null)
				throw new ArgumentNullException("e");

			// When the left mouse is pressed, check to see if the submenu access or the button command is requested.  If the submenu is requested then the command
			// passes through without change.  If the button part of the menu is pressed then the command associated with this control is invoked.
			if (!e.Handled && (this.Role == MenuItemRole.TopLevelHeader || this.Role == MenuItemRole.TopLevelItem))
			{

				// In order to make the XAML a little easier to code, any child of the PART_MenuItem control will also count when it comes time to figuring
				// out if the drop down button has been hit or not.  Without this the XML programmer would need to sprinkle the code with liberal 'IsHitTestVisible'
				// statements in order to get the desired effect of pressing the button and having the drop down menu appear.
				if (this.IsMouseOverButtonPart(e.GetPosition(this)))
				{

					// Normally a button does not aquire the keyboard focus, but a menu item does.  Having the Gadget take the focus when it is pressed seems to
					// work best in making it work in a complex group of focus scopes.
					if (!this.IsKeyboardFocusWithin)
						this.Focus();

					// When the button is pressed we can assume that the submenu is no longer of any interest.
					if (this.IsSubmenuOpen)
						this.IsSubmenuOpen = false;

					// This prevents the item from acting like a MenuItem when the button part has been pressed.
					e.Handled = true;

				}

				// If the left mouse is over the rendered area of the control and the left mouse button is down, then this control is 'Pressed.'
				Rect rect = new Rect(new Point(), base.RenderSize);
				this.IsPressed = Mouse.LeftButton == MouseButtonState.Pressed && base.IsMouseOver && rect.Contains(Mouse.GetPosition(this));

			}

			// Allow the base class to handle the rest of the event.
			base.OnMouseLeftButtonDown(e);

		}

		/// <summary>
		/// Called when the left mouse button is released.
		/// </summary>
		/// <param name="e">The event data for the MouseLeftButtonUp event.</param>
		[SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase")]
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{

            // Validate the parameters.
            if (e == null)
				throw new ArgumentNullException("e");

			// When the left mouse is pressed, check to see if the submenu access or the button command is requested.  If the submenu is requested then the command
			// passes through without change.  If the button part of the menu is pressed then the command associated with this control is invoked.
			if (!e.Handled && (this.Role == MenuItemRole.TopLevelHeader || this.Role == MenuItemRole.TopLevelItem))
			{

				// In order to make the XAML a little easier to code, any child of the PART_MenuItem control will also count when it comes time to figuring
				// out if the drop down button has been hit or not.  Without this the programmer would need to sprinkle the code with liberal 'IsHitTestVisible'
				// statements in order to get the desired effect of pressing the button and having the drop down menu appear.
				if (this.IsMouseOverButtonPart(e.GetPosition(this)))
				{

					// A MenuItem that acts as a header will not normally raise the events associated with a non-header MenuItem.  Since this control also acts 
					// like a button
					this.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent, this));

					// This will execute any commands associated with this menu item.
					this.ExecuteCommand();

					// Since this event is going to be marked as handled, the state of the button needs to be updated here.  It is usually updated by the base class
					// but if the processing is interrupted then the state won't be properly reflected unless it is handled here explicitly.
					Rect rect = new Rect(new Point(), base.RenderSize);
					this.IsPressed = Mouse.LeftButton == MouseButtonState.Pressed && base.IsMouseOver && rect.Contains(Mouse.GetPosition(this));

					// This prevents the item from acting like a MenuItem when the button part has been pressed.
					e.Handled = true;

				}

			}

			// Allow the base class to handle the rest of the event.
			base.OnMouseLeftButtonUp(e);

		}

		/// <summary>
		/// Called when the mouse is moved over a menu item.
		/// </summary>
		/// <param name="e">The event data for the MouseMove event.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{

            // Validate the parameters.
            if (e == null)
                throw new ArgumentNullException("e");

			// This property will indicate that the mouse is over the drop down part of the control.
			this.SetValue(Gadget.isMouseOverButtonPropertyKey, this.IsMouseOverButtonPart(e.GetPosition(this)));

			// This property will indicate that the mouse is over the button part of the control.
			this.SetValue(Gadget.isMouseOverMenuItemPropertyKey, this.IsMouseOverMenuItemPart(e.GetPosition(this)));

            // Allow the base class to handle the rest of the event.
            base.OnMouseMove(e);

        }

		/// <summary>
		/// Invoked whenever the effective value of any dependency property on this FrameworkElement has been updated.
		/// </summary>
		/// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{

			// This will intercept the base class (MenuItem) definition of the MenuItemRole and interpret it for the Gadget.  Gadgets have an additional property
			// called 'IsExpandable' which can programatically prevent an item with children from being expanded.
			if (e.Property == MenuItem.RoleProperty)
			{

				// Extract the specific arguments from the generic event arguments.
				MenuItemRole menuItemRole = (MenuItemRole)e.NewValue;

				// If the data context for this control is an IExplorerItem, then we can assume that the 'IsExpandable' property of the data context item has been 
				// bound to the container's property (MVVM).  Only when there is a data context that is recognized by this Gadget will the MenuItemRole be 
				// interpreted.  For all other data contexts the MenuItemRole will work the same as the base class.
				if (this.DataContext is IExplorerItem)
				{
					if (menuItemRole == MenuItemRole.TopLevelHeader && !this.IsExpandable)
						menuItemRole = MenuItemRole.TopLevelItem;
					if (menuItemRole == MenuItemRole.SubmenuHeader && !this.IsExpandable)
						menuItemRole = MenuItemRole.SubmenuItem;
				}

				// This will provide a property that mirrors the RolePropert of the base MenuItem class unless there is an IExplorerItem provided as a data
				// context.  In this scenario the 'IsExpandable' will inhibit an item from acting as a header even though the item has children.
				this.SetValue(Gadget.rolePropertyKey, menuItemRole);

			}

			// Allow the base class to handle the rest of the property change.
			base.OnPropertyChanged(e);

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

			// It is recommended that we allow the base class to handle the reset of the preparation.
			base.PrepareContainerForItemOverride(element, item);

		}

        /// <summary>
        /// Sets the value of the HasAsNeededItems property.
        /// </summary>
        /// <param name="hasAsNeededItems">An indication of whether the control has 'AsNeeded' items in the overflow control.</param>
        protected void SetHasAsNeededItems(Boolean hasAsNeededItems)
        {

            // This key is not available to inheriting classes and as the property accessors can't selectively allow public access to the 'Get' and protected 
            // access to the 'Set' accessors, this method was needed.
            this.SetValue(Gadget.hasAsNeededItemsPropertyKey, hasAsNeededItems);

        }

        /// <summary>
        /// Sets the value of the HasOverflowItems property.
        /// </summary>
        /// <param name="hasOverflowItems">An indication of whether the control has items in the overflow control.</param>
        protected void SetHasOverflowItems(Boolean hasOverflowItems)
        {

            // This key is not available to inheriting classes and as the property accessors can't selectively allow public access to the 'Get' and protected 
            // access to the 'Set' accessors, this method was needed.
            this.SetValue(Gadget.hasOverflowItemsPropertyKey, hasOverflowItems);

        }

		/// <summary>
		/// Sets the highlighted property.
		/// </summary>
		internal void SetIsHighlighted()
		{

			// This is meant to be used internally to clear the highlight feature.  The MenuBase and MenuItem classes do a poor job of this in the context of
			// Gadgets and BreadcrumbItems, so it's left to the descendant classes to fill in and fix this to make the interface more intuitive for the user.
			// Without the intervension of the descendant classes, the highlight remains even when the keyboard focus or mouse has moved away from the control that
			// owns the higlighted element.
			this.IsHighlighted = false;

		}

        /// <summary>
		/// Simulates the action of the control aquiring the keyboard focus.
		/// </summary>
		internal void SimulateIsKeyboardFocusWithinChanged()
		{

			// The Menu class is brain damaged in a big way.  When the internal 'MenuMode' is left, the Menu class attempts to "Restore The Previous Focus".  This 
			// is a misguided attempt to appease some class of applications that wants the main application window to get the focus back from a focus scope such as 
			// a Menu or GadgetBar and then give the focus to some other control in the application like the main content control.  Thus 'popping' the focus to 
			// whatever control had the focus before the menu or toolbar.  However, if the focused element of the main window was also the focused element in the 
			// Menu, then the logic for exiting the 'MenuMode' has some horrible side effects.  The keyboard focus never really changes though the item is 
			// de-selected.  The MenuItem attempts to give the focus back to the Main Window, but the Main Window effectively says that there's nothing it can do 
			// because MenuItem inside the Menu already has the focus.  In the mean time, the internal 'MenuMode' state does change.  This causes the MenuItem to 
			// be de-selected.  At this point, a mechanism is needed to reset the MenuItem back to the selected state it had before attempting to get rid of the 
			// keyboard focus.  This code will force the base class into thinking that it has re-aquired the focus (when it never really left) and thus, reset the 
			// state of the MenuItem.
			base.OnIsKeyboardFocusWithinChanged(new DependencyPropertyChangedEventArgs(IsKeyboardFocusWithinProperty, false, true));

		}

	}

}
