namespace Teraque.Windows.Controls
{

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;

	/// <summary>
	/// Represents a column header for a ColumnViewColumn.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class DetailsViewColumnHeader : ColumnViewColumnHeader
	{

		/// <summary>
		/// Identifies the IsFirst dependency property.
		/// </summary>
		public static readonly DependencyProperty IsFirstProperty;

		/// <summary>
		/// Identifies the isFirstProperty dependency property key.
		/// </summary>
		internal static readonly DependencyPropertyKey isFirstPropertyKey = DependencyProperty.RegisterReadOnly(
			"IsFirst",
			typeof(Boolean),
			typeof(ColumnViewColumnHeader),
			new FrameworkPropertyMetadata());

		/// <summary>
		/// Initializes the DetailsViewColumnHeader class.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static DetailsViewColumnHeader()
		{

			// This allows the class to have its own style in the theme.
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
				typeof(DetailsViewColumnHeader),
				new FrameworkPropertyMetadata(typeof(DetailsViewColumnHeader)));

			// These properties must be set after the corresonding property key is defined.  This is done here because setting these when the field is declared can
			// lead to bugs if the fields should be reorderer.
			DetailsViewColumnHeader.IsFirstProperty = DetailsViewColumnHeader.isFirstPropertyKey.DependencyProperty;

		}

		/// <summary>
		/// Gets a value that indicates whether a ColumnViewColumnHeader is the first column.
		/// </summary>
		public Boolean IsFirst
		{
			get
			{
				return (Boolean)this.GetValue(DetailsViewColumnHeader.IsFirstProperty);
			}
		}

	}

}