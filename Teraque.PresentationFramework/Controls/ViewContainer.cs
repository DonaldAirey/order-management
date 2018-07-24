namespace Teraque.Windows.Controls
{

	using System;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Markup;

	/// <summary>
	/// Used to hold the resources of a ViewBase.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[ContentProperty("View")]
	public class ViewContainer : FrameworkElement
	{

		/// <summary>
		/// Identifies the View dependency property.
		/// </summary>
		public static readonly DependencyProperty ViewProperty = DependencyProperty.Register(
			"View",
			typeof(ViewBase),
			typeof(ViewContainer));

		/// <summary>
		/// Gets or sets the view.
		/// </summary>
		public ViewBase View
		{
			get
			{
				return this.GetValue(ViewContainer.ViewProperty) as ViewBase;
			}
			set
			{
				this.SetValue(ViewContainer.ViewProperty, value);
			}
		}

	}

}
