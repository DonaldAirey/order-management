namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Threading;
	using System.ServiceModel;
	using System.Windows;
	using System.Windows.Input;
	using System.Windows.Threading;
	using Teraque.AssetNetwork.Properties;
	using Teraque.AssetNetwork.WebService;

	/// <summary>
	/// Represents a control that displays a generic trade blotter.
	/// </summary>
	/// <copyright>Copyright © 2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class EquityBlotterPage : BlotterPageBase
	{

		/// <summary>
		/// Delegate for changing a working order item.
		/// </summary>
		/// <param name="equityBlotterPage">The window control that gets the notification of the status of the update.</param>
		/// <param name="equityWorkingOrder">The working order that has been modified.</param>
		delegate void ItemPropertyChangeHandler(EquityBlotterPage equityBlotterPage, EquityWorkingOrder equityWorkingOrder);

		/// <summary>
		/// Provides a mapping between the property of the Equity Working Order that has changed, and the handler for that action.
		/// </summary>
		static Dictionary<String, ItemPropertyChangeHandler> propertyChangeHandlerMap = new Dictionary<String, ItemPropertyChangeHandler>()
		{
			{"IsBrokerMatch", EquityBlotterPage.UpdateWorkingOrderProperty},
			{"IsHedgeMatch", EquityBlotterPage.UpdateWorkingOrderProperty},
			{"IsInstitutionMatch", EquityBlotterPage.UpdateWorkingOrderProperty},
			{"TimeInForceCode", EquityBlotterPage.UpdateWorkingOrderProperty},
			{"SideCode", EquityBlotterPage.UpdateWorkingOrderProperty}
		};

		/// <summary>
		/// Initializes a new instance of EquityBlotterPage class.
		/// </summary>
		public EquityBlotterPage()
		{

			// The IDE managed resources are initialized here.
			this.InitializeComponent();

			// When the view is loaded, we will hook into the collection of Working Orders and watch for updates made by the user to the orders.
			this.equityWorkingOrderView.Loaded += OnEquityWorkingOrderViewLoaded;

		}

		/// <summary>
		/// Constructs a DestinationOrder collection from the selected blotter.
		/// </summary>
		/// <param name="blotterRow">The main blotter containing Working Orders that are to be executed.</param>
		List<DestinationOrderInfo> ConstructDestinationOrder(DataModel.BlotterRow blotterRow)
		{

			// This is the destinatinon broker that will execute the orders.
			DataModel.DestinationRow destinationRow = DataModel.Destination.DestinationKeyExternalId0.Find("ASSET NETWORK ECN");
			Guid destinationId = destinationRow.DestinationId;

			// This list will collect the orders as the are generated recursively.
			List<DestinationOrderInfo> destinationOrders = new List<DestinationOrderInfo>();

			// Recursively construct the batch of orders from the current blotter and its ancestors.
			this.ConstructDestinationOrder(destinationOrders, blotterRow, destinationId);

			// This is a batch of DestinationOrders that can be executed by the destination broker.
			return destinationOrders;

		}

		/// <summary>
		/// Constructs a DestinationOrder collection from the selected blotter.
		/// </summary>
		/// <param name="destinationOrders">A collection of DestinationOrder records.</param>
		/// <param name="blotterRow">The main blotter containing Working Orders that are to be executed.</param>
		/// <param name="destinationRow">The destination broker.</param>
		void ConstructDestinationOrder(List<DestinationOrderInfo> destinationOrders, DataModel.BlotterRow blotterRow, Guid destinationId)
		{

			// For every Working Order in the blotter, construct a DestinationOrder record.
			foreach (DataModel.WorkingOrderRow workingOrderRow in blotterRow.GetWorkingOrderRows())
			{

				// Our Destination Order will completely fill all obligations from the source of the orders.
				Decimal sourceOrderQuantity = 0.0M;
				foreach (DataModel.SourceOrderRow sourceOrderRow in workingOrderRow.GetSourceOrderRows())
					sourceOrderQuantity += sourceOrderRow.OrderedQuantity;

				// The amount already committed to brokers is subtracted from the order amount.  When this batch completes, the Working Order will be 
				// completely filled.
				Decimal destinationOrderQuantity = 0.0M;
				foreach (DataModel.DestinationOrderRow destinationOrderRow in workingOrderRow.GetDestinationOrderRows())
					destinationOrderQuantity += destinationOrderRow.OrderedQuantity;

				// If this Working Order has shares that still need to be executed, then construct a DestinationOrder record.
				if (sourceOrderQuantity > destinationOrderQuantity)
				{
					DestinationOrderInfo destinationOrderInfo = new DestinationOrderInfo();
					destinationOrderInfo.BlotterId = workingOrderRow.BlotterId;
					destinationOrderInfo.DestinationId = destinationId;
					destinationOrderInfo.OrderedQuantity = sourceOrderQuantity - destinationOrderQuantity;
					destinationOrderInfo.OrderTypeCode = workingOrderRow.OrderTypeCode;
					destinationOrderInfo.SecurityId = workingOrderRow.SecurityId;
					destinationOrderInfo.SettlementId = workingOrderRow.SettlementId;
					destinationOrderInfo.SideCode = workingOrderRow.SideCode;
					destinationOrderInfo.TimeInForceCode = workingOrderRow.TimeInForceCode;
					destinationOrderInfo.WorkingOrderId = workingOrderRow.WorkingOrderId;
					destinationOrders.Add(destinationOrderInfo);
				}

			}

			// This will recurse into all the child blotters and constructor destination orders for all the working orders found there.
			var childBlotterRows = from entityTreeRow in blotterRow.EntityRow.GetEntityTreeRowsByFK_Entity_EntityTree_ParentId()
									from childBlotterRow in entityTreeRow.EntityRowByFK_Entity_EntityTree_ChildId.GetBlotterRows()
									select childBlotterRow;
			foreach (DataModel.BlotterRow childBlotterRow in childBlotterRows)
				this.ConstructDestinationOrder(destinationOrders, childBlotterRow, destinationId);

		}

		/// <summary>
		/// Invoked when the Equity Working Order View is loaded.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The routed event arguments.</param>
		void OnEquityWorkingOrderViewLoaded(Object sender, RoutedEventArgs e)
		{

			// When the view is loaded, hook into the working orders and watch for any changes to the properties of the orders.
			this.equityWorkingOrderView.WorkingOrders.ItemPropertyChanged += this.OnWorkingOrderPropertyChanged;

		}

		/// <summary>
		/// Creates Destination Orders for every item in the blotter.
		/// </summary>
		/// <param name="state"></param>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public void OnExecuteSlice(Object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
		{

			// Create a disposable channel to the middle tier.
			using (WebServiceClient webServiceClient = new WebServiceClient(Properties.Settings.Default.WebServiceEndpoint))
			{

				try
				{

					// This will create a batch of DestinationOrder records and send it to the web service to be executed by the destination broker.
					DataModel.BlotterRow blotterRow = DataModel.Blotter.BlotterKey.Find(this.BlotterId);
					List<DestinationOrderInfo> destinationOrders = this.ConstructDestinationOrder(blotterRow);
					webServiceClient.CreateDestinationOrders(destinationOrders.ToArray());

				}
				catch (Exception exception)
				{

					// Log communication problems.
					Log.Error("{0}, {1}", exception.Message, exception.StackTrace);

				}

			}

		}

		/// <summary>
		/// Handles a change to the properties of a Working Order in the view.
		/// </summary>
		/// <param name="sender">The object the originated the event.</param>
		/// <param name="e">The ItemPropertyChanged event arguments.</param>
		void OnWorkingOrderPropertyChanged(Object sender, ItemPropertyChangedEventArgs e)
		{

			// Extract from the event arguments the working order that has been modified by the user.
			ItemPropertyChangeHandler itemPropertyChangeHandler;
			if (EquityBlotterPage.propertyChangeHandlerMap.TryGetValue(e.PropertyName, out itemPropertyChangeHandler))
				itemPropertyChangeHandler(this, e.Item as EquityWorkingOrder);

		}

		/// <summary>
		/// Handles a general change to properties of a working order in the collection.
		/// </summary>
		/// <param name="equityBlotterPage">An object in the foreground thread that is notified of errors.</param>
		/// <param name="equityWorkingOrder">A working order that has changed.</param>
		static void UpdateWorkingOrderProperty(EquityBlotterPage equityBlotterPage, EquityWorkingOrder equityWorkingOrder)
		{

		}

		/// <summary>
		/// Updates the working order.
		/// </summary>
		/// <param name="state">A generic thread argument.</param>
		void UpdateWorkingOrderThread(Object state)
		{

		}

	}

}
