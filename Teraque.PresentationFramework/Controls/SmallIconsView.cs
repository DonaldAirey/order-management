namespace Teraque.Windows.Controls
{

	using System;
	using System.Windows;

	/// <summary>
	/// A view for the SmallListView used to display items as small icons.
	/// </summary>
	/// <copyright>Copyright � 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class SmallIconsView : IconsView
	{

		/// <summary>
		/// Gets the object that is associated with the style for the view mode.
		/// </summary>
		protected override Object DefaultStyleKey
		{
			get
			{
				return new ComponentResourceKey(typeof(SmallIconsView), "SmallIconsViewStyle");
			}
		}

		/// <summary>
		/// Gets the style to use for the items in the view mode.
		/// </summary>
		protected override Object ItemContainerDefaultStyleKey
		{
			get
			{
				return new ComponentResourceKey(typeof(SmallIconsView), "SmallIconsViewItemContainerStyle");
			}
		}

	}

}
