namespace Teraque.Windows.Data
{

	using System;
	using System.Windows.Data;
	using System.Globalization;
	using System.Windows.Controls;
	using System.Windows;
	using Teraque.Windows.Controls;

	/// <summary>
	/// 
	/// </summary>
	public class IndentConverter : IValueConverter
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
		{

			Double indentLevel = 0.0;
			Double indentSize = 0.0;
			TreeViewItem treeViewItem = value as TreeViewItem;
			ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(treeViewItem);
			while (itemsControl != null)
			{

				treeViewItem = itemsControl as TreeViewItem;
				if (treeViewItem != null)
					indentLevel += 1.0;
				else
				{
					Navigator navigator = itemsControl as Navigator;
					if (navigator != null)
						indentSize = navigator.Indent;
				}

				itemsControl = ItemsControl.ItemsControlFromItemContainer(treeViewItem);

			}

			return indentLevel * indentSize;

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

	}

}