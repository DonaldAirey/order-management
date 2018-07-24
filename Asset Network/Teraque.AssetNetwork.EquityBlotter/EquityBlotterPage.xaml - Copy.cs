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
		/// Delegate for handling a generic fault.
		/// </summary>
		/// <param name="workingOrders">The working orders in the batch.</param>
		/// <param name="faultException">The generic fault.</param>
		delegate void FaultCallback(WebService.WorkingOrder[] workingOrders, FaultException faultException);

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
		/// Handles generic faults.
		/// </summary>
		/// <param name="workingOrders">The batch of Working Order updates.</param>
		/// <param name="batchFault">The generic FaultException.</param>
		void HandleFaultException(WebService.WorkingOrder[] workingOrders, FaultException faultException)
		{

			// This will try to make the generic faults less generic.  The same thing could have been accomplished with a 'Catch' clause but then we would have
			// needed a separate foreground handler for each kind of fault.  This will test to see if we had an 'InsufficientClaims' fault which will invoke a
			// specific message.
			FaultException<InsufficientClaimsFault> insufficientClaimsFault = faultException as FaultException<InsufficientClaimsFault>;
			if (insufficientClaimsFault != null)
			{

				// The user doesn't have the required permission to execute this method.
				MessageBox.Show(
					Properties.Resources.InsufficientClaimsExceptionError,
					Settings.Default.ApplicationName,
					MessageBoxButton.OK,
					MessageBoxImage.Error);

			}
			else
			{

				// Check to see if the record is busy.
				FaultException<OptimisticConcurrencyFault> optimisticConcurrencyFault = faultException as FaultException<OptimisticConcurrencyFault>;
				if (optimisticConcurrencyFault != null)
				{

					// This record is busy - please try again later.
					MessageBox.Show(
						Properties.Resources.OptimisticConcurrencyExceptionError,
						Settings.Default.ApplicationName,
						MessageBoxButton.OK,
						MessageBoxImage.Error);

				}
				else
				{

					// If we haven't recognized what kind of fault was generated, then we'll just emit to the user whatever message came with this fault.
					MessageBox.Show(faultException.Message, Settings.Default.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);

				}

			}

			// When the entire batch fails, we'll restore the entire Model View back to it's original state (the one it had before generating the batch).
			WorkingOrderCollection<EquityWorkingOrder> workingOrderCollection = this.equityWorkingOrderView.WorkingOrders;
			foreach (WebService.WorkingOrder workingOrder in workingOrders)
			{
				DataModel.WorkingOrderRow workingOrderRow = DataModel.WorkingOrder.WorkingOrderKey.Find(workingOrder.WorkingOrderId);
				Int32 index = workingOrderCollection.Find(workingOrderRow.WorkingOrderId);
				if (index >= 0)
				{
					EquityWorkingOrder equityWorkingOrder = workingOrderCollection[index];
					workingOrderCollection.View.EditItem(equityWorkingOrder);
					equityWorkingOrder.IsBrokerMatch = workingOrderRow.IsBrokerMatch;
					equityWorkingOrder.IsHedgeMatch = workingOrderRow.IsHedgeMatch;
					equityWorkingOrder.IsInstitutionMatch = workingOrderRow.IsInstitutionMatch;
					workingOrderCollection.View.CommitEdit();
				}
			}

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

			// This will extract the fields of the working order from the View Model and initiate a background task that will call the server to update the record.
			WebService.WorkingOrder workingOrder = new WebService.WorkingOrder()
			{
				IsBrokerMatch = equityWorkingOrder.IsBrokerMatch,
				IsHedgeMatch = equityWorkingOrder.IsHedgeMatch,
				IsInstitutionMatch = equityWorkingOrder.IsInstitutionMatch,
				RowVersion = equityWorkingOrder.RowVersion,
				SideCode = equityWorkingOrder.SideCode,
				TimeInForceCode = equityWorkingOrder.TimeInForceCode,
				WorkingOrderId = equityWorkingOrder.WorkingOrderId
			};
			ThreadPool.QueueUserWorkItem(equityBlotterPage.UpdateWorkingOrderThread, new WebService.WorkingOrder[] { workingOrder });

		}

		/// <summary>
		/// Updates the working order.
		/// </summary>
		/// <param name="state">A generic thread argument.</param>
		void UpdateWorkingOrderThread(Object state)
		{

			// Extract the batch of working orders from the generic argument.
			WebService.WorkingOrder[] workingOrders = state as WebService.WorkingOrder[];

			// A Web Service client is required to update the working orders.
			using (WebService.WebServiceClient webServiceClient = new WebService.WebServiceClient(Settings.Default.WebServiceEndpoint))
			{

				try
				{

					// Call the server to update the batch of working orders.
					webServiceClient.UpdateWorkingOrder(workingOrders);

				}
				catch (FaultException faultException)
				{

					// Call the foreground to process any security errors.
					this.Dispatcher.BeginInvoke(
						DispatcherPriority.Normal,
						new FaultCallback(this.HandleFaultException),
						workingOrders,
						faultException);

				}

			}

		}

	}

}
