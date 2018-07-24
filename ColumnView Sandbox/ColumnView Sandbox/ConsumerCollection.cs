namespace Teraque.Windows
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Collections.Specialized;
	using System.Windows;
	using System.Windows.Data;
	using System.Xml.Linq;

	/// <summary>
	/// An observable collection of consumers.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class ConsumerCollection : ObservableCollection<Consumer>
	{

		/// <summary>
		/// Information needed to defer and update and restore live updates when disposed.
		/// </summary>
		public class DeferRefreshInfo : IDisposable
		{

			/// <summary>
			/// The collection that will have it's notifications disabled while this object is alive.
			/// </summary>
			ConsumerCollection consumerCollection;

			/// <summary>
			/// Initialize a new instance of the DeferFrefreshInfo class.
			/// </summary>
			/// <param name="consumerCollection">The collection that will have it's notifications disabled while this object is alive.</param>
			public DeferRefreshInfo(ConsumerCollection consumerCollection)
			{

				// Initialize the object and disable the collection changed notifications.
				this.consumerCollection = consumerCollection;
				this.consumerCollection.isRefreshDisabled = true;

			}

			/// <summary>
			/// Allows the collection to send notifications about changes to the collection again.
			/// </summary>
			public void Dispose()
			{

				// This will allow the collection to send out notifications as it is changed and sends out a message that the entire collection
				// has changed.
				this.consumerCollection.isRefreshDisabled = false;
				this.consumerCollection.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

			}

		}

		/// <summary>
		/// The collection of consumers.
		/// </summary>
		ConsumerView consumerView;

		/// <summary>
		/// Used to disable the collection changed update during a bulk insert.
		/// </summary>
		Boolean isRefreshDisabled;

		/// <summary>
		/// Initializes a new instance of the ConsumerCollection class.
		/// </summary>
		public ConsumerCollection()
		{

			// This is the view that provides filtering and sorting for this collection.
			this.consumerView = new ConsumerView(this);

			// Load the XML file of consumers into a data model.
			using (this.DeferRefresh())
			{
				XDocument xdocument = XDocument.Load(Application.GetResourceStream(new Uri("/Consumers.xml", UriKind.Relative)).Stream);
				foreach (XElement consumerElement in xdocument.Root.Nodes())
					this.Add(new Consumer(consumerElement));
			}

		}

		/// <summary>
		/// Creates an object that can be used to defer updates to the collection.
		/// </summary>
		/// <returns>An object that can be used to defer updates.</returns>
		public DeferRefreshInfo DeferRefresh()
		{

			// Creating this object will prevent events from being invoked when the collection changes.  Disposing of the object will restore the
			// events and send out a message that the collection has changed.
			return new DeferRefreshInfo(this);

		}

		/// <summary>
		/// Raises the CollectionChanged event with the provided arguments.
		/// </summary>
		/// <param name="e">Arguments of the event being raised.</param>
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{

			// Don't update the collection during a deferral.  When deferring is finished, a RESET event should be sent out to consumers of the collection.
			if (!this.isRefreshDisabled)
				base.OnCollectionChanged(e);

		}

		/// <summary>
		/// Gets the view that provides sorting and filtering.
		/// </summary>
		public ConsumerView View
		{
			get
			{
				return this.consumerView;
			}
		}

	}

}
