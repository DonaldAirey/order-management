namespace Teraque.Windows
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using Teraque.Windows.Controls;

	/// <summary>
	/// A shallow copy of the ColumnViewColumn that isn't linked to a view.  Used in the ColumnViewChooseDetail dialog box.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class ColumnDescription : DependencyObject
	{

		/// <summary>
		/// The ColumnViewColumn.
		/// </summary>
		ColumnViewColumn columnField;

		/// <summary>
		/// The description of the column.
		/// </summary>
		String descriptionField;

		/// <summary>
		/// Indicates that the ColumnViewColumn is visible.
		/// </summary>
		Boolean isVisibleField;

		/// <summary>
		/// The width of the column.
		/// </summary>
		Double widthField;

		/// <summary>
		/// Initializes a new instance of the ColumnDescription class.
		/// </summary>
		/// <param name="columnViewColumn">The ColumnViewColumn used as a source for this shallow clone.</param>
		public ColumnDescription(ColumnViewColumn columnViewColumn)
		{

			// Initialize the object
			this.columnField = columnViewColumn;
			this.descriptionField = columnViewColumn.Description;
			this.isVisibleField = columnViewColumn.IsVisible;
			this.widthField = columnViewColumn.Width;

		}

		/// <summary>
		/// The ColumnViewColumn that is the source of this shallow clone.
		/// </summary>
		public ColumnViewColumn Column
		{
			get
			{
				return this.columnField;
			}
		}

		/// <summary>
		/// Gets or sets the minimum width for a column.
		/// </summary>
		public Boolean IsVisible
		{
			get
			{
				return this.isVisibleField;
			}
		}

		/// <summary>
		/// Gets or sets the header text associated with the column.
		/// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public String Description
		{
			get
			{
				return this.descriptionField;
			}
		}

		/// <summary>
		/// Gets or sets the width of the column. 
		/// </summary>
		public Double Width
		{
			get
			{
				return this.widthField;
			}
		}

	}

}
