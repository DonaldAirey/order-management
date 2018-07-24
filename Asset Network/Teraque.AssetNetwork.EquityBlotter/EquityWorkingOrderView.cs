namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Markup;
	using Teraque.Windows.Controls;

	/// <summary>
	/// A WorkingOrderView of the Equity Working Orders.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class EquityWorkingOrderView : EquityWorkingOrderView<EquityWorkingOrder> { }

	/// <summary>
	/// A gernic WorkingOrderView of the Equity Working Orders.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class EquityWorkingOrderView<TType> : WorkingOrderView<TType> where TType : EquityWorkingOrder
	{

		/// <summary>
		/// Maps the name of the FilterItem to the predicate that handles that filter.
		/// </summary>
		static Dictionary<String, Predicate<TType>> filterDelegateMap = new Dictionary<String, Predicate<TType>>()
		{
			{ "FilterTimeInForceDay", (TType w) => {return w.TimeInForceCode == TimeInForceCode.Day;}},
			{ "FilterTimeInForceGtc", (TType w) => {return w.TimeInForceCode == TimeInForceCode.GoodTillCancel;}}
		};

		/// <summary>
		/// Initializes the WorkingOrderView class.
		/// </summary>
		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"), SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static EquityWorkingOrderView()
		{

			// This is a complex control and will manage it's own focus scope.
			FocusManager.IsFocusScopeProperty.OverrideMetadata(typeof(EquityWorkingOrderView<TType>), new FrameworkPropertyMetadata(true));

			// Combine the filters used for the equity blotter with the filters from the base blotter.
			EquityWorkingOrderView<TType>.MergeFilters(EquityWorkingOrderView<TType>.filterDelegateMap);

		}

		/// <summary>
		/// Initializes a new instance of the EquityWorkingOrderView class.
		/// </summary>
		public EquityWorkingOrderView()
		{

			// These are the various views supported by this items control (which is also a page).
			ViewContainer viewContainer = XamlReader.Load(
				Application.GetResourceStream(new Uri("/Teraque.AssetNetwork.EquityBlotter;component/Views/ColumnView.xaml", UriKind.RelativeOrAbsolute)).Stream) as ViewContainer;
			this.ColumnView = viewContainer.View as ColumnView;
			this.View = this.ColumnView;

		}

		/// <summary>
		/// Creates the collection of &lt;Type&gt; used by the view.
		/// </summary>
		/// <param name="blotterId">The blotter from which to construct the collection.</param>
		/// <returns>A collection of &lt;Type&gt; View Model records belonging to the given blotter.</returns>
		protected override WorkingOrderCollection<TType> CreateCollectionCore(Guid blotterId)
		{

			// This is used to construct a View Model collection for the viewer.  It can be overridden in a descenant to add additional properties to the View 
			// Model elements.
			return new EquityWorkingOrderCollection<TType>(blotterId);

		}

	}

}
