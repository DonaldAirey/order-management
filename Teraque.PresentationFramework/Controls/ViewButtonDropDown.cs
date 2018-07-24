namespace Teraque.Windows.Controls
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Input;

	/// <summary>
	/// A button and menu combination that allows the user to select a view for the content page.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[TemplatePart(Name = partSlider, Type = typeof(Slider))]
	public class ViewButtonDropDown : Control
	{

		/// <summary>
		/// The name of the slider part.
		/// </summary>
		const String partSlider = "PART_ViewSlider";

		/// <summary>
		/// The slider part of the popup control.
		/// </summary>
		ViewSlider viewSlider;

		/// <summary>
		/// Initialize the ViewButtonDropDown type.
		/// </summary>
		static ViewButtonDropDown()
		{

			// This is a complex control and will manage it's own focus scope.
			FocusManager.IsFocusScopeProperty.OverrideMetadata(typeof(ViewButtonDropDown), new FrameworkPropertyMetadata(true));

		}

		/// <summary>
		/// Initializes a new instance of the ViewButtonDropDown class.
		/// </summary>
		public ViewButtonDropDown()
		{

			// Initialize the object.
			this.FocusVisualStyle = null;

		}

		/// <summary>
		/// Is invoked whenever application code or internal processes call ApplyTemplate.
		/// </summary>
		public override void OnApplyTemplate()
		{

			// Once we've dug down into the templates to find the slider it can be attached to the value property of this control.  Moving the slider will change
			// the value and any listeners attached to the value of this property will see the change also.
			this.viewSlider = this.GetTemplateChild(ViewButtonDropDown.partSlider) as ViewSlider;
	
		}

		/// <summary>
		/// Invoked when an unhandled Keyboard.GotKeyboardFocus attached event reaches an element in its route that is derived from this class.
		/// </summary>
		/// <param name="e">The KeyboardFocusChangedEventArgs that contains the event data.</param>
		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{

            // Validate parameters
            if (e == null)
                throw new ArgumentNullException("e");

            // Giving the focus to the slider control makes this compatible with the existing ViewButton in the Windows Explorer.  However, it would be an 
			// interesting challenge to try to interpret the keyboard navigation buttons for this control.  Alas, that excercise is left for a later time.
			if (e.NewFocus == this)
			{
				DependencyObject focusScope = FocusManager.GetFocusScope(this);
				IInputElement focusedElement = FocusManager.GetFocusedElement(focusScope);
				if (focusedElement != this.viewSlider)
					FocusManager.SetFocusedElement(focusScope, this.viewSlider);
			}
			
		}

		/// <summary>
		/// Invoked when the parent of this element in the visual tree is changed. Overrides OnVisualParentChanged.
		/// </summary>
		/// <param name="oldParent">The old parent element. May be null to indicate that the element did not have a visual parent previously.</param>
		protected override void OnVisualParentChanged(DependencyObject oldParent)
		{

			// This is a 'self-binding' control.  When the parent changes, clear the relationship to the old parent.
			if (BindingOperations.IsDataBound(this.viewSlider, ViewSlider.ValueProperty))
				BindingOperations.ClearBinding(this.viewSlider, ViewSlider.ValueProperty);

			// When the button on the ViewButton control is used to select a different mode, the slider needs to reflect that mode and vica-versa.  This will walk
			// up the visual tree until the owner of this drop down window is found and attach itself to the property that controls the display mode and the
			// scaling.  Alternatively, when the slider is moved, the value of the display mode and scale will change with the value of the slider.
			Binding valueBinding = new Binding();
			valueBinding.Path = new PropertyPath("ViewValue");
			valueBinding.Source = VisualTreeExtensions.FindAncestor<ViewButton>(this);
			valueBinding.Mode = BindingMode.TwoWay;
			BindingOperations.SetBinding(this.viewSlider, ViewSlider.ValueProperty, valueBinding);
			
			// There is significant processing in the base class for this method.  Removing this call will have adverse side effects.
			base.OnVisualParentChanged(oldParent);

		}

	}

}
