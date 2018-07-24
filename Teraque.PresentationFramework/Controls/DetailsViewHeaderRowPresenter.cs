namespace Teraque.Windows.Controls
{

	using System;
	using System.ComponentModel;

	/// <summary>
	/// Represents an object that is used to define the layout of a row of column headers.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class DetailsViewHeaderRowPresenter : ColumnViewHeaderRowPresenter
	{

		/// <summary>
		/// Create the visual element for the header.
		/// </summary>
		/// <param name="columnViewColumn">The ColumnViewColumn to be created.</param>
		/// <returns>The visual element representing the logical ColumnViewColumn.</returns>
		protected override ColumnViewColumnHeader CreateHeader(ColumnViewColumn columnViewColumn)
		{

			// This is the default column that is created for this presenter.  Inheriting classes can override this for customer header.
			return new DetailsViewColumnHeader();

		}

		/// <summary>
		/// Handles a change to the column collection associated with this presenter.
		/// </summary>
		/// <param name="notifyCollectionChangedEventArgs">The object that originated the event.</param>
		/// <param name="notifyCollectionChangedEventArgs">Provides data for the CollectionChanged event.</param>
		protected override void OnColumnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{

			// The base class needs to adjust the column collection before we can operate on it.  This will force the visual children that display the headers to 
			// align with the column definitions.
			base.OnColumnCollectionChanged(sender, notifyCollectionChangedEventArgs);

			// This will cycle through the columns and set the 'IsFirst' property on the first visual header in the presenter.  This property is used to give a 
			// distinctive style to the first column, such as a hanging indent.
			if (this.Columns != null)
				for (Int32 columnIndex = 0; columnIndex < this.Columns.Count; columnIndex++)
				{
					ColumnViewColumn columnViewColumn = this.Columns[columnIndex] as ColumnViewColumn;
					DetailsViewColumnHeader detailsViewColumnHeader = this.GetHeaderFromColumn(columnViewColumn) as DetailsViewColumnHeader;
					detailsViewColumnHeader.SetValue(DetailsViewColumnHeader.isFirstPropertyKey, columnIndex == 0);
				}

		}

	}

}