namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;

	/// <summary>
	/// Presents the rows in the view.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class ColumnViewRowPresenter : ColumnViewRowPresenterBase
	{

		/// <summary>
		/// Identifies the Column attached property.
		/// </summary>
		public static readonly DependencyProperty ColumnProperty = DependencyProperty.RegisterAttached(
			"Column",
			typeof(ColumnViewColumn),
			typeof(ColumnViewRowPresenter));

		/// <summary>
		/// Positions child elements and determines a size for a FrameworkElement derived class.
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
				for (Int32 columnIndex = 0; columnIndex < this.Columns.VisibleCount; columnIndex++)
				{
					ColumnViewColumn columnViewColumn = this.Columns[columnIndex];
					UIElement uiElement = this.Children[columnIndex];
					uiElement.Arrange(new Rect(location.X, 0.0, columnViewColumn.Width, arrangeSize.Height));
					remainingWidth -= columnViewColumn.Width;
					location.X += columnViewColumn.Width;
				}

			// This is the size of the row after laying everything out.
			return arrangeSize;

		}

		/// <summary>
		/// Create a visual element based on the properties of the column.
		/// </summary>
		/// <param name="columnViewColumn">The column that defines the cell.</param>
		/// <returns>A visual element that holds the content of the cell.</returns>
		public static FrameworkElement CreateCell(ColumnViewColumn columnViewColumn)
		{

			// Validate the parameters.
			if (columnViewColumn == null)
				throw new ArgumentNullException("columnViewColumn");

			// This will either create an element based on the templates associated with the ColumnViewColumn, or for simple binding, will create a simple cell and
			// bind it using the binding provided in the DisplayMemberBinding property.  Both variations return a FrameworkElement that is used as an element in the
			// row to display the data associated with the column.
			FrameworkElement frameworkElement;
			if (columnViewColumn.DisplayMemberBinding == null)
			{

				// This will create a generic ContentPresenter using the templates associated with the ColumnView and bind the content of the generic control to 
				// the data context (which contains the row data).  It is the responsibility of the cell to select which property of the row data it wants to 
				// display.  It is important to remember that a ContentPresenter is not like other controls.  The DataContext is not inheritied from the logical 
				// parent, it is set to the  Content property of the ContentPresenter.  The main problem we're solving here is that the child control that's 
				// generated from the ContentPresenter's template needs a DataContext, but the ContentPresenter doesn't have one since it's pointing to itself.  To 
				// solve this, we'll bind the Content of the ContentPresenter to the ColumnViewRowPresenter that is it's parent.  This effectively restores the
				// DataContext inheritance that all other controls have.  Clear as mud?
				ContentPresenter contentPresenter = new ContentPresenter();
				contentPresenter.ContentTemplate = columnViewColumn.CellTemplate;
				contentPresenter.ContentTemplateSelector = columnViewColumn.CellTemplateSelector;
				Binding dataContextBinding = new Binding() { Path = new PropertyPath("DataContext") };
				dataContextBinding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ColumnViewRowPresenter), 1);
				BindingOperations.SetBinding(contentPresenter, ContentPresenter.ContentProperty, dataContextBinding);
				frameworkElement = contentPresenter;

			}
			else
			{

				// This will create a cell to display the data provided by the 'DisplayMemberBinding' which selects which property of the row data is to be 
				// displayed.
				ColumnViewColumnCell columnViewColumnCell = new ColumnViewColumnCell();
				BindingOperations.SetBinding(columnViewColumnCell, ColumnViewColumnCell.ContentProperty, columnViewColumn.DisplayMemberBinding);
				frameworkElement = columnViewColumnCell;

			}

			// This element is used to present the data.
			return frameworkElement;

		}

		/// <summary>
		/// Installs the cell in the presenter.
		/// </summary>
		/// <param name="frameworkElement"></param>
		/// <param name="columnViewColumn"></param>
		void InstallCell(FrameworkElement frameworkElement, ColumnViewColumn columnViewColumn)
		{

			// Without the weak listener, the rows would not get updates when the column changes.  If we went with a strong event listener reference, then the rows
			// could not be garbage collected.  As the rows are managed by the ItemsGenerator, there is no chance to use an IDisposable interface on them, so the
			// weak event listener is the only chance we have of getting messages about changes to the columns.
			PropertyChangedEventManager.AddListener(columnViewColumn, this, String.Empty);

			// This allows the cell to find the cell which produced it and to which it is bound through a weak listner.  Though not essential to remove the listener
			// when the element is removed from this row, it is good housekeeping.
			frameworkElement.SetValue(ColumnViewRowPresenter.ColumnProperty, columnViewColumn);

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
				for (Int32 columnIndex = 0; columnIndex < this.Columns.VisibleCount; columnIndex++)
				{
					ColumnViewColumn columnViewColumn = this.Columns[columnIndex];
					UIElement uiElement = this.Children[columnIndex];
					Double remainingWidth = Math.Max(0.0, constraint.Width - totalSize.Width);
					uiElement.Measure(new Size(Math.Min(remainingWidth, columnViewColumn.Width), constraint.Height));
					totalSize.Width += columnViewColumn.Width;
					totalSize.Height = Math.Max(totalSize.Height, uiElement.DesiredSize.Height);

				}

			// This is the total space required for this row.
			return totalSize;

		}

		/// <summary>
		/// Handles a change to the column collection.
		/// </summary>
		/// <param name="notifyCollectionChangedEventArgs">The Object that originated the event.</param>
		/// <param name="notifyCollectionChangedEventArgs">Provides data for the CollectionChanged event.</param>
		protected override void OnColumnCollectionChanged(Object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{

			Int32 columnIndex;

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (notifyCollectionChangedEventArgs == null)
				throw new ArgumentNullException("notifyCollectionChangedEventArgs");

			// This will reconcile the visual elements in this control with the definition of the columns.
			switch (notifyCollectionChangedEventArgs.Action)
			{

			case NotifyCollectionChangedAction.Add:

				// Create a cell to hold the value and add each it to the row at the given index.
				columnIndex = notifyCollectionChangedEventArgs.NewStartingIndex == -1 ? 0 : notifyCollectionChangedEventArgs.NewStartingIndex;
				foreach (ColumnViewColumn columnViewColumn in notifyCollectionChangedEventArgs.NewItems)
				{
					if (columnViewColumn.IsVisible)
					{
						FrameworkElement frameworkElement = ColumnViewRowPresenter.CreateCell(columnViewColumn);
						this.InstallCell(frameworkElement, columnViewColumn);
						this.Children.Insert(columnIndex++, frameworkElement);
					}
					this.InvalidateMeasure();
				}

				break;

			case NotifyCollectionChangedAction.Move:

				// Columns that appear after the visible count in the collection are hidden items.  They exist only as a pool of potential columns.  The items below
				// the 'VisibleCount' are actually visible in this view and appear in the same order in which they appear in this control.  As items are moved in
				// and out of the visible range, we will create or destroy the visual elements used to show those columns in the viewer.
				Int32 oldIndex = notifyCollectionChangedEventArgs.OldStartingIndex;
				Int32 newIndex = notifyCollectionChangedEventArgs.NewStartingIndex;
				ColumnViewColumn movedColumn = this.Columns[newIndex];
				if (this.Children.Count == this.Columns.VisibleCount)
				{

					// At this point, the collection hasn't changed.  We are just moving columns around.
					UIElement uiElement = this.Children[oldIndex];
					this.Children.RemoveAt(notifyCollectionChangedEventArgs.OldStartingIndex);
					this.Children.Insert(notifyCollectionChangedEventArgs.NewStartingIndex, uiElement);
					this.InvalidateArrange();

				}
				else
				{

					// This will hide the column by destroying the visual elements used to display the column.
					if (oldIndex < this.Children.Count)
						this.Children.RemoveAt(oldIndex);

					// This will create a visual element to display the now visible column.
					if (movedColumn.IsVisible)
					{
						FrameworkElement frameworkElement = ColumnViewRowPresenter.CreateCell(movedColumn);
						this.InstallCell(frameworkElement, movedColumn);
						this.Children.Insert(newIndex, frameworkElement);
					}

				}

				break;

			case NotifyCollectionChangedAction.Remove:

				// Remove the offending cells from the specified location.
				columnIndex = notifyCollectionChangedEventArgs.OldStartingIndex;
				foreach (ColumnViewColumn columnViewColumn in notifyCollectionChangedEventArgs.OldItems)
					this.RemoveCell(columnIndex++);

				break;

			case NotifyCollectionChangedAction.Reset:

				// Clear them all.
				while (this.Children.Count != 0)
					this.RemoveCell(0);

				break;

			}

		}

		/// <summary>
		/// Handles a change to the column property.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="propertyChangedEventArgs">The event arguments.</param>
		protected override void OnColumnPropertyChanged(Object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (propertyChangedEventArgs == null)
				throw new ArgumentNullException("propertyChangedEventArgs");

			// This will get the column where the property change occurred from the sender.
			ColumnViewColumn columnViewColumn = sender as ColumnViewColumn;

			// If the column is visible, then it has a visual element which needs to have its properties reconciled with the ColumnViewColumn properties.
			if (columnViewColumn.IsVisible)
			{

				// This will find the visual element what has properites that must be reconciled with the header.
				Int32 columnIndex = this.Columns.IndexOf(columnViewColumn);
				ColumnViewColumnCell columnViewColumnCell = this.Children[columnIndex] as ColumnViewColumnCell;

				// This will apply the property in the column to the visual element representing that column in the current row.
				switch (propertyChangedEventArgs.PropertyName)
				{

				case "CellTemplate":
				case "CellTemplateSelector":
				case "DisplayMemberBinding":

					// Recreate the cell based on the new properties.
					this.Children.RemoveAt(columnIndex);
					FrameworkElement frameworkElement = ColumnViewRowPresenter.CreateCell(columnViewColumn);
					this.InstallCell(frameworkElement, columnViewColumn);
					this.Children.Insert(columnIndex, frameworkElement);
					break;

				case "Width":
				case "MinWidth":

					// Forcing a re-measurement of the row will pick up the changes made to the column's width.
					this.InvalidateMeasure();
					break;

				}

			}

		}

		/// <summary>
		/// Removes the column from the row.
		/// </summary>
		/// <param name="columnViewColumn">The ColumnViewColumn to be removed.</param>
		void RemoveCell(Int32 columnIndex)
		{

			// This will use the attached property to find the column to which this cell is bound through a weak listener.  It will remove the listner (though this
			// isn't a requirement for the cell to be garbage collected) and finally remove the cell from the row.
			ColumnViewColumn columnViewColumn = this.Children[columnIndex].GetValue(ColumnViewRowPresenter.ColumnProperty) as ColumnViewColumn;
			PropertyChangedEventManager.RemoveListener(columnViewColumn, this, String.Empty);
			this.Children.RemoveAt(columnIndex);

		}

	}

}