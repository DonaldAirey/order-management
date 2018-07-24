namespace Teraque.Windows.Controls
{

	using System;
	using System.Windows;

	/// <summary>
	/// A view for the ListView used to display items in a list.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class SimpleListView : ItemsView
	{

		/// <summary>
		/// Gets the object that is associated with the style for the view mode.
		/// </summary>
		protected override object DefaultStyleKey
		{
			get
			{
				return new ComponentResourceKey(typeof(SimpleListView), "SimpleListViewStyle");
			}
		}

		/// <summary>
		/// Gets the style to use for the items in the view mode.
		/// </summary>
		protected override Object ItemContainerDefaultStyleKey
		{
			get
			{
				return new ComponentResourceKey(typeof(SimpleListView), "SimpleListViewItemContainerStyle");
			}
		}

	}

}
