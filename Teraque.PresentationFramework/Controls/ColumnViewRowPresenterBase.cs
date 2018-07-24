namespace Teraque.Windows.Controls
{

	using System;
	using System.Collections;
	using System.Collections.ObjectModel;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Diagnostics.CodeAnalysis;
	using System.ComponentModel;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;

	/// <summary>
	/// Common class for ColumnView rows and headers.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public abstract class ColumnViewRowPresenterBase : Canvas, IWeakEventListener
	{

		/// <summary>
		/// Identifies the Columns dependency property.
		/// </summary>
		public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
			"Columns",
			typeof(ColumnViewColumnCollection),
			typeof(ColumnViewRowPresenterBase),
			new FrameworkPropertyMetadata(
				null,
				FrameworkPropertyMetadataOptions.AffectsMeasure,
				new PropertyChangedCallback(ColumnViewRowPresenterBase.OnColumnsPropertyChanged)));

		/// <summary>
		/// Gets or sets the collection of columns used by this presenter.
		/// </summary>
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ColumnViewColumnCollection Columns
		{
			get
			{
				return this.GetValue(ColumnViewRowPresenterBase.ColumnsProperty) as ColumnViewColumnCollection;
			}
			set
			{
				this.SetValue(ColumnViewRowPresenterBase.ColumnsProperty, value);
			}
		}

		/// <summary>
		/// Handles a change to the column collection.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="notifyCollectionChangedEventArgs">The event arguments.</param>
		protected virtual void OnColumnCollectionChanged(Object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs) { }

		/// <summary>
		/// Handles a change to the column properties.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="notifyCollectionChangedEventArgs">The event arguments.</param>
		protected virtual void OnColumnPropertyChanged(Object sender, PropertyChangedEventArgs propertyChangedEventArgs) { }

		/// <summary>
		/// Handles a change to the Columns property.
		/// </summary>
		/// <param name="dependencyObject">The DependancyObject that owns the property.</param>
		/// <param name="dependencyPropertyChangedEventArgs">The DependencyProperty that has changed.</param>
		static void OnColumnsPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{

			// The listener is the object that owns the Columns property.
			ColumnViewRowPresenterBase columnViewRowPresenterBase = dependencyObject as ColumnViewRowPresenterBase;

			// This will disconnect the old listener from getting changes to the columns.
			ColumnViewColumnCollection oldCollection = dependencyPropertyChangedEventArgs.OldValue as ColumnViewColumnCollection;
			if (oldCollection != null)
			{

				// This will disconnect the RowPresenter from changes made to the old column collection.
				CollectionChangedEventManager.RemoveListener(oldCollection, columnViewRowPresenterBase);

				// This will clear out any items from the old collection.  This is preferrable to a collection 'Reset' because we can provide the columns that have
				// gone out of scope which might not be available any other way.
				columnViewRowPresenterBase.OnColumnCollectionChanged(
					columnViewRowPresenterBase,
					new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldCollection));

			}

			// This will connect the new listener to get changes to the columns.
			ColumnViewColumnCollection newCollection = dependencyPropertyChangedEventArgs.NewValue as ColumnViewColumnCollection;
			if (newCollection != null)
			{

				// This will connect the RowPresenter to the collection of columns so that any changes made to the column set is reflected in the arrangement of
				// cells in the view.
				CollectionChangedEventManager.AddListener(newCollection, columnViewRowPresenterBase);

				// This will syncrhonize the view to the new column set.
				columnViewRowPresenterBase.OnColumnCollectionChanged(
					columnViewRowPresenterBase,
					new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newCollection));

			}

		}

		/// <summary>
		/// Receives events from the centralized event manager.
		/// </summary>
		/// <param name="managerType">The type of the WeakEventManager calling this method.</param>
		/// <param name="sender">Object that originated the event.</param>
		/// <param name="args">Event data.</param>
		/// <returns>
		/// true if the listener handled the event. It is considered an error by the WeakEventManager handling in WPF to register a listener for an event that the
		/// listener does not handle. Regardless, the method should return false if it receives an event that it does not recognize or handle.
		/// </returns>
		Boolean IWeakEventListener.ReceiveWeakEvent(Type managerType, Object sender, EventArgs args)
		{

			// This will handle the changes to the column collection.
			if (managerType == typeof(CollectionChangedEventManager))
			{
				this.OnColumnCollectionChanged(sender, args as NotifyCollectionChangedEventArgs);
				return true;
			}

			// This will handle the changes to the column property.
			if (managerType == typeof(PropertyChangedEventManager))
			{
				this.OnColumnPropertyChanged(sender, args as PropertyChangedEventArgs);
				return true;
			}

			// We should never get here but if we do, this is the right response.
			return false;

		}

	}

}