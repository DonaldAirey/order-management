namespace Teraque.Windows.Controls
{

	using System;
	using System.Windows;

	/// <summary>
	/// Presents the rows in the view.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class DetailsViewRowPresenter : ColumnViewRowPresenter
	{

		/// <summary>
		/// The size of the hanging indent for the first column.
		/// </summary>
		const Double hangingIndent = 32.0;

		/// <summary>
		/// When overridden in a derived class, positions child elements and determines a size for a FrameworkElement derived class.
		/// </summary>
		/// <param name="arrangeSize">The final area within the parent that this element should use to arrange itself and its children.</param>
		/// <returns>The actual size used.</returns>
		protected override Size ArrangeOverride(Size arrangeSize)
		{

			// This will lay out all the element horizontally across the space of the RowPresenter using either the fixed width or the width of the item.
			Point location = new Point();
			Double remainingWidth = arrangeSize.Width;

			// Evaluate the size of all the columns in this header.
			if (this.Columns != null)
				for (Int32 columnIndex = 0; columnIndex < this.Columns.Count; columnIndex++)
				{
					ColumnViewColumn columnViewColumn = this.Columns[columnIndex];
					UIElement uiElement = this.Children[columnIndex];
					Double width = columnIndex == 0 ? columnViewColumn.Width - DetailsViewRowPresenter.hangingIndent : columnViewColumn.Width;
					Rect elementRect = new Rect(location.X, 0.0, width, arrangeSize.Height);
					uiElement.Arrange(elementRect);
					remainingWidth -= elementRect.Width;
					location.X += elementRect.Width;
				}

			// This is the size of the row after laying everything out.
			return arrangeSize;

		}

		/// <summary>
		/// When overridden in a derived class, measures the size in layout required for child elements and determines a size for the FrameworkElement-derived
		/// class.
		/// </summary>
		/// <param name="constraint">
		/// The available size that this element can give to child elements. Infinity can be specified as a value to indicate that the element will size to
		/// whatever content is available.
		/// </param>
		/// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
		protected override Size MeasureOverride(Size constraint)
		{

			// This keeps track of the space needed by this row.
			Size totalSize = new Size();

			// Evaluate the size of all the columns in this row.  Note that even though the cell may only want a fraction of the space available to it, we will
			// reserve all the space that the header has asked for (horizontally, that is).  The standard version of this control tries to auto-calculate the width
			// of the column but it can only do that for the visible items.  Since it is a virtualizing panel, there's no way to tell how wide each of the columns
			// needs to be without creating each one and measuring it.  This would result in horrible performance.  Therefore, it's not possible to automatically
			// calculate the width of a column without incurring a terrible performance penalty.  This design simplifies the process of calculating the column width
			// by assuming the column have given us the same width for every column in the virtual display.
			if (this.Columns != null)
				for (Int32 columnIndex = 0; columnIndex < this.Columns.Count; columnIndex++)
				{
					ColumnViewColumn columnViewColumn = this.Columns[columnIndex];
					UIElement uiElement = this.Children[columnIndex];
					Double remainingWidth = Math.Max(0.0, constraint.Width - totalSize.Width);
					Double width = columnIndex == 0 ? columnViewColumn.Width - DetailsViewRowPresenter.hangingIndent : columnViewColumn.Width;
					Size elementSize = new Size(Math.Min(remainingWidth, width), constraint.Height);
					uiElement.Measure(elementSize);
					totalSize.Width += elementSize.Width;
					totalSize.Height = Math.Max(totalSize.Height, uiElement.DesiredSize.Height);
				}

			// This is the total space required for this row.
			return totalSize;

		}

	}

}