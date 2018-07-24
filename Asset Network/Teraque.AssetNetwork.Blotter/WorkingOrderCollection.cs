namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections;
	using System.Collections.ObjectModel;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Diagnostics.CodeAnalysis;
	using System.ComponentModel;
	using System.Data;
	using System.Linq;
	using System.Windows.Threading;
	using Teraque.AssetNetwork;

    /// <summary>
    /// A collection of Working Orders.
    /// </summary>
    /// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
    public class WorkingOrderCollection<TType> : ObservableCollection<TType>, IDisposable, INotifyItemPropertyChanged where TType : WorkingOrder
	{

		/// <summary>
		/// The unique identifier of the blotter that holds these orders.
		/// </summary>
		Guid blotterIdField;

		/// <summary>
		/// Contains the identifiers of this and all the child blotters.  Used to quickly assess if a given event pertains to this collection.
		/// </summary>
		HashSet<Guid> blotterIdSet = new HashSet<Guid>();

		/// <summary>
		/// Invoked when the property of an item in the collection has changed.
		/// </summary>
		public event EventHandler<ItemPropertyChangedEventArgs> ItemPropertyChanged;

		/// <summary>
		/// The collection of workingOrders.
		/// </summary>
		WorkingOrderCollectionView<TType> workingOrderCollectionView;

		/// <summary>
		/// Initializes a new instance of the WorkingOrderCollection class.
		/// </summary>
		[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public WorkingOrderCollection(Guid blotterId)
		{

			// Initialize the object.
			this.blotterIdField = blotterId;

			// This is the view that provides filtering and sorting for this collection.  It can be overridden in descendant classes to provide additional
			// functionality.
			this.workingOrderCollectionView = this.CreateView();

			// Link the collection into the data model.
			DataModel.Entity.EntityRowChanged += this.OnEntityRowChanged;
			DataModel.Price.PriceRowChanged += this.OnPriceRowChanged;
			DataModel.Security.SecurityRowChanged += this.OnSecurityRowChanged;
			DataModel.DestinationOrder.DestinationOrderRowChanged += this.OnDestinationOrderRowChanged;
			DataModel.DestinationOrder.DestinationOrderRowDeleted += this.OnDestinationOrderRowDeleted;
			DataModel.Execution.ExecutionRowChanged += this.OnExecutionRowChanged;
			DataModel.Execution.ExecutionRowDeleted += this.OnExecutionRowDeleted;
			DataModel.SourceOrder.SourceOrderRowChanged += this.OnSourceOrderRowChanged;
			DataModel.SourceOrder.SourceOrderRowDeleted += this.OnSourceOrderRowDeleted;
			DataModel.WorkingOrder.WorkingOrderRowChanged += this.OnWorkingOrderRowChanged;

			// This will recursively create working order records for this and all the blotter's descendants from the data model.
			DataModel.BlotterRow blotterRow = DataModel.Blotter.BlotterKey.Find(blotterId);
			if (blotterRow != null)
				this.Copy(blotterRow);

		}

		/// <summary>
		/// Finalize this instance of the WorkingOrderCollection class.
		/// </summary>
		~WorkingOrderCollection()
		{

			// This will dispose of the unmanaged resources.  There are none in this class but there may be some in descendant classes.
			this.Dispose(false);

		}

		/// <summary>
		/// Gets the blotter identifier of this collection.
		/// </summary>
		public Guid BlotterId
		{
			get
			{
				return this.blotterIdField;
			}
		}

		/// <summary>
		/// Gets the view that provides sorting and filtering.
		/// </summary>
		public WorkingOrderCollectionView<TType> View
		{
			get
			{
				return this.workingOrderCollectionView;
			}
		}

		/// <summary>
		/// Gets a unique set of the identifiers of the blotters present in this collection.
		/// </summary>
		public HashSet<Guid> BlotterIdSet
		{
			get
			{
				return this.blotterIdSet;
			}
		}

		/// <summary>
		/// Copy the collection from the data model.
		/// </summary>
		/// <param name="blotterId">The identifier of the blotter to which the orders are associated.</param>
		void Copy(DataModel.BlotterRow blotterRow)
		{

			// The collection is hierarchical.  That is, a blotter can have child blotters and those blotters can have children and so on.  This HashSet is a flat
			// collection of the blotter and all its descendants and is used to quickly determine if any given change to the data model might affect one of the 
			// WorkingOrder records or any of the properties of that WorkingOrder.
			this.blotterIdSet.Add(blotterRow.BlotterId);

			// In order to make merging results as fast as possible, the collection is sortred by the unique identifier of the WorkingOrder.  When a row in the 
			// client data model is updated, we will first find the blotter to which the change belongs and, if that blotter is part of the set constructed above,
			// we'll then find the working order affected using a binary search.  The idea is to quickly find out if a given change to the data model applies to the
			// viewed collection and, if it does, which records are affected.
			foreach (DataModel.WorkingOrderRow workingOrderRow in blotterRow.GetWorkingOrderRows())
			{
				Int32 index = this.BinarySearch(order => order.WorkingOrderId, workingOrderRow.WorkingOrderId);
				TType workingOrder = this.CreateInstanceCore(workingOrderRow);
				workingOrder.PropertyChanged += this.OnItemPropertyChanged;
				this.Insert(~index, workingOrder);
			}

			// This will recurse into each of the descendant blotters copying the data into the collection.  The collection is a flattened view of all the working
			// orders; there is no attempt by design to make the report hierarchical even though the relationship between the blotters is.
			var childBlotterRows = from entityTreeRow in blotterRow.EntityRow.GetEntityTreeRowsByFK_Entity_EntityTree_ParentId()
								   from childBlotterRow in entityTreeRow.EntityRowByFK_Entity_EntityTree_ChildId.GetBlotterRows()
								   select childBlotterRow;
			foreach (DataModel.BlotterRow childBlotterRow in childBlotterRows)
				this.Copy(childBlotterRow);

		}

		/// <summary>
		/// Creates a new instance of the TType row.
		/// </summary>
		/// <param name="workingOrderRow">The data model record that is the source of information for the new row.</param>
		/// <returns>A model view version of the data model record.</returns>
		protected virtual TType CreateInstanceCore(DataModel.WorkingOrderRow workingOrderRow)
		{

			// Create a model view version of the data model working order row.
			return new WorkingOrder(workingOrderRow) as TType;

		}

		/// <summary>
		/// Creates a new view for the WorkingOrderCollection.
		/// </summary>
		/// <returns>A new view for the WorkingOrderCollection.</returns>
		protected virtual WorkingOrderCollectionView<TType> CreateView()
		{

			// This is the view that provides filtering and sorting for this collection.
			return new WorkingOrderCollectionView<TType>(this);

		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{

			// Call the virtual method to allow derived classes to clean up resources.
			this.Dispose(true);

			// Since we took care of cleaning up the resources, there is no need to call the finalizer.
			GC.SuppressFinalize(this);

		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing">true to indicate that the object is being disposed, false to indicate that the object is being finalized.</param>
		protected virtual void Dispose(Boolean disposing)
		{

			// This will remove this object from the data model events.
			if (disposing)
			{
				DataModel.Entity.EntityRowChanged -= this.OnEntityRowChanged;
				DataModel.Price.PriceRowChanged -= this.OnPriceRowChanged;
				DataModel.Security.SecurityRowChanged -= this.OnSecurityRowChanged;
				DataModel.DestinationOrder.DestinationOrderRowChanged -= this.OnDestinationOrderRowChanged;
				DataModel.DestinationOrder.DestinationOrderRowDeleted -= this.OnDestinationOrderRowDeleted;
				DataModel.Execution.ExecutionRowChanged -= this.OnExecutionRowChanged;
				DataModel.Execution.ExecutionRowDeleted -= this.OnExecutionRowDeleted;
				DataModel.SourceOrder.SourceOrderRowChanged -= this.OnSourceOrderRowChanged;
				DataModel.SourceOrder.SourceOrderRowDeleted -= this.OnSourceOrderRowDeleted;
				DataModel.WorkingOrder.WorkingOrderRowChanged -= this.OnWorkingOrderRowChanged;
			}

		}

		/// <summary>
		/// Finds the Working Order with the given identifier.
		/// </summary>
		/// <param name="workingOrderId">The unique identifier of the working order to be found.</param>
		/// <returns>
		/// The zero-based index of item in the list if found; otherwise, a negative number that is the bitwise complement of the index of the next element that is
		/// larger than item or, if there is no larger element, the bitwise complement of Count.
		/// </returns>
		public Int32 Find(Guid workingOrderId)
		{

			// Use the binary search to find the element and return the index to it.
			return this.BinarySearch(order => order.WorkingOrderId, workingOrderId);

		}
		
		/// <summary>
		/// Handles a change to a DestinationOrder row.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="destinationOrderRowChangeEventArgs">The event arguments.</param>
		protected virtual void OnDestinationOrderRowChanged(Object sender, DataModel.DestinationOrderRowChangeEventArgs destinationOrderRowChangeEventArgs)
		{

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (destinationOrderRowChangeEventArgs == null)
				throw new ArgumentNullException("destinationOrderRowChangeEventArgs");

			// We're only interested in additions and changes that affect the WorkingOrder records in this blotter.
			if (destinationOrderRowChangeEventArgs.Action == DataRowAction.Add || destinationOrderRowChangeEventArgs.Action == DataRowAction.Change)
			{
				DataModel.WorkingOrderRow workingOrderRow = destinationOrderRowChangeEventArgs.Row.WorkingOrderRow;
				if (this.blotterIdSet.Contains(workingOrderRow.BlotterId))
				{
					Int32 index = this.BinarySearch(order => order.WorkingOrderId, workingOrderRow.WorkingOrderId);
					if (index >= 0)
						this.UpdateDestinationOrderQuantity(this[index], workingOrderRow);
				}
			}

		}

		/// <summary>
		/// Handles the deletion of a DestinationOrder row.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="destinationOrderRowChangeEventArgs">The event arguments.</param>
		protected virtual void OnDestinationOrderRowDeleted(Object sender, DataModel.DestinationOrderRowChangeEventArgs destinationOrderRowChangeEventArgs)
		{

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (destinationOrderRowChangeEventArgs == null)
				throw new ArgumentNullException("destinationOrderRowChangeEventArgs");

			// We're only interested in deletes that affect the WorkingOrder records in this blotter.
			if (destinationOrderRowChangeEventArgs.Action == DataRowAction.Delete)
			{
				Guid workingOrderId = (Guid)destinationOrderRowChangeEventArgs.Row[DataModel.DestinationOrder.WorkingOrderIdColumn, DataRowVersion.Original];
				DataModel.WorkingOrderRow workingOrderRow = DataModel.WorkingOrder.WorkingOrderKey.Find(workingOrderId);
				if (workingOrderRow != null && this.blotterIdSet.Contains(workingOrderRow.BlotterId))
				{
					Int32 index = this.BinarySearch(order => order.WorkingOrderId, workingOrderRow.WorkingOrderId);
					if (index >= 0)
						this.UpdateDestinationOrderQuantity(this[index], workingOrderRow);
				}
			}

		}

		/// <summary>
		/// Handles a change to the Entity table.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="entityRowChangeEventArgs">The event arguments.</param>
		protected virtual void OnEntityRowChanged(Object sender, DataModel.EntityRowChangeEventArgs entityRowChangeEventArgs)
		{

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (entityRowChangeEventArgs == null)
				throw new ArgumentNullException("entityRowChangeEventArgs");

			// We're only interested in changes.
			if (entityRowChangeEventArgs.Action == DataRowAction.Change)
			{

				// Select all the WorkingOrderRows that are related to the Securities that are related to the EntityRow that has changed.  We are looking for 
				// changes to the names of the securities which are kept in the Entity table.
				var workingOrderRows = from securityRow in entityRowChangeEventArgs.Row.GetSecurityRows()
									   from workingOrderRow in securityRow.GetWorkingOrderRowsByFK_Security_WorkingOrder_SecurityId()
									   select workingOrderRow;

				// Iterate through all the rows affected by the change to the entity row and update the properties related to the entity table.  For example, the
				// full name of the security is found in the entity row.  Note that we immediately filter out events not related to this blotter or its children.
				foreach (DataModel.WorkingOrderRow workingOrderRow in workingOrderRows)
					if (this.blotterIdSet.Contains(workingOrderRow.BlotterId))
					{

						// This will copy and commit the changes made to the Entity row to the WorkingOrder record.
						Int32 index = this.BinarySearch(order => order.WorkingOrderId, workingOrderRow.WorkingOrderId);
						if (index >= 0)
						{
							TType workingOrder = this[index];
							this.workingOrderCollectionView.EditItem(workingOrder);
							workingOrder.SecurityName = entityRowChangeEventArgs.Row.Name;
							this.workingOrderCollectionView.CommitEdit();
						}

					}

			}
		
		}

		/// <summary>
		/// Handles a change to the Execution row.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="executionRowChangeEventArgs">The event arguments.</param>
		protected virtual void OnExecutionRowChanged(Object sender, DataModel.ExecutionRowChangeEventArgs executionRowChangeEventArgs)
		{

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (executionRowChangeEventArgs == null)
				throw new ArgumentNullException("executionRowChangeEventArgs");

			// We're only interested in additions and changes that affect the WorkingOrder records in this blotter.
			if (executionRowChangeEventArgs.Action == DataRowAction.Add || executionRowChangeEventArgs.Action == DataRowAction.Change)
			{
				DataModel.WorkingOrderRow workingOrderRow = executionRowChangeEventArgs.Row.DestinationOrderRow.WorkingOrderRow;
				if (this.blotterIdSet.Contains(workingOrderRow.BlotterId))
				{
					Int32 index = this.BinarySearch(order => order.WorkingOrderId, workingOrderRow.WorkingOrderId);
					if (index >= 0)
						this.UpdateExecutionQuantity(this[index], workingOrderRow);
				}
			}

		}

		/// <summary>
		/// Handles the deletion of a Execution row.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="executionRowChangeEventArgs">The event arguments.</param>
		protected virtual void OnExecutionRowDeleted(Object sender, DataModel.ExecutionRowChangeEventArgs executionRowChangeEventArgs)
		{

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (executionRowChangeEventArgs == null)
				throw new ArgumentNullException("executionRowChangeEventArgs");

			// We're only interested in deletes that affect the WorkingOrder records in this blotter.
			if (executionRowChangeEventArgs.Action == DataRowAction.Delete)
			{
				Guid destinationOrderId = (Guid)executionRowChangeEventArgs.Row[DataModel.Execution.DestinationOrderIdColumn, DataRowVersion.Original];
				DataModel.DestinationOrderRow destinationOrderRow = DataModel.DestinationOrder.DestinationOrderKey.Find(destinationOrderId);
				if (destinationOrderRow != null)
				{
					DataModel.WorkingOrderRow workingOrderRow = destinationOrderRow.WorkingOrderRow;
					if (this.blotterIdSet.Contains(workingOrderRow.BlotterId))
					{
						Int32 index = this.BinarySearch(order => order.WorkingOrderId, workingOrderRow.WorkingOrderId);
						if (index >= 0)
							this.UpdateExecutionQuantity(this[index], workingOrderRow);
					}
				}
			}

		}

		/// <summary>
		/// Handles a change to the properties of an item in the collection.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="e">The event data.</param>
		void OnItemPropertyChanged(Object sender, PropertyChangedEventArgs e)
		{

			// The event handler is not invoked when the data is updated internally.  If the change comes from the user, then we check to see if there is an event
			// handler installed and, if there is one, we invoke it.
			if (!this.View.IsEditingItem)
				if (this.ItemPropertyChanged != null)
					this.ItemPropertyChanged(this, new ItemPropertyChangedEventArgs(sender, e.PropertyName));

		}

		/// <summary>
		/// Handles a change to the Price table.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="priceRowChangeEventArgs">The event arguments.</param>
		protected virtual void OnPriceRowChanged(Object sender, DataModel.PriceRowChangeEventArgs priceRowChangeEventArgs)
		{

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (priceRowChangeEventArgs == null)
				throw new ArgumentNullException("priceRowChangeEventArgs");

			// We're only interested in adds and changes.
			if (priceRowChangeEventArgs.Action == DataRowAction.Add || priceRowChangeEventArgs.Action == DataRowAction.Change)
			{

				// Select all the WorkingOrderRows that are related to the Securities that are related to the PriceRow that has changed.  We are looking for
				// changes to the price of items displayed in this blotter.
				DataModel.SecurityRow securityRow = priceRowChangeEventArgs.Row.SecurityRowByFK_Security_Price_SecurityId;
				var workingOrderRows = from workingOrderRow in securityRow.GetWorkingOrderRowsByFK_Security_WorkingOrder_SecurityId()
									   select workingOrderRow;

				// Iterate through all the rows affected by the change to the Price row and update the properties related to the entity table.
				foreach (DataModel.WorkingOrderRow workingOrderRow in workingOrderRows)
					if (this.blotterIdSet.Contains(workingOrderRow.BlotterId))
					{

						// This will copy and commit the changes made to the Price row to the WorkingOrder record.
						Int32 index = this.BinarySearch(order => order.WorkingOrderId, workingOrderRow.WorkingOrderId);
						if (index >= 0)
						{

							// These factors are used display the market value using the notations provided for price and quantity.
							Decimal quantityFactor = workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SecurityId.QuantityFactor;
							Decimal priceFactor = workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SecurityId.PriceFactor;

							// This sets the prices and updates any field related to the prices.
							TType workingOrder = this[index];
							this.workingOrderCollectionView.EditItem(workingOrder);
							workingOrder.AskPrice.Price = priceRowChangeEventArgs.Row.AskPrice;
							workingOrder.BidPrice.Price = priceRowChangeEventArgs.Row.BidPrice;
							workingOrder.LastPrice.Price = priceRowChangeEventArgs.Row.LastPrice;
							workingOrder.MarketValue = workingOrder.SourceOrderQuantity * quantityFactor * workingOrder.LastPrice.Price * priceFactor;
							this.workingOrderCollectionView.CommitEdit();

						}

					}

			}

		}

		/// <summary>
		/// Handles a change to the Security row.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="securityRowChangeEventArgs">The event arguments.</param>
		protected virtual void OnSecurityRowChanged(Object sender, DataModel.SecurityRowChangeEventArgs securityRowChangeEventArgs)
		{

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (securityRowChangeEventArgs == null)
				throw new ArgumentNullException("securityRowChangeEventArgs");

			// We're only interested in changes.
			if (securityRowChangeEventArgs.Action == DataRowAction.Change)
			{

				// Select all the WorkingOrderRows that are related to the Securities that has changed.  We are looking for changes to the symbol of items displayed
				// in this blotter.
				var workingOrderRows = from workingOrderRow in securityRowChangeEventArgs.Row.GetWorkingOrderRowsByFK_Security_WorkingOrder_SecurityId()
									   select workingOrderRow;

				// Iterate through all the rows affected by the change to Security table.
				foreach (DataModel.WorkingOrderRow workingOrderRow in workingOrderRows)
				{
					Int32 index = this.BinarySearch(order => order.WorkingOrderId, workingOrderRow.WorkingOrderId);
					if (index >= 0)
					{
						TType workingOrder = this[index];
						this.workingOrderCollectionView.EditItem(workingOrder);
						workingOrder.Symbol = securityRowChangeEventArgs.Row.Symbol;
						this.workingOrderCollectionView.CommitEdit();
					}
				}

			}

		}

		/// <summary>
		/// Handles a change to a SourceOrder row.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="sourceOrderRowChangeEventArgs">The event arguments.</param>
		protected virtual void OnSourceOrderRowChanged(Object sender, DataModel.SourceOrderRowChangeEventArgs sourceOrderRowChangeEventArgs)
		{

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (sourceOrderRowChangeEventArgs == null)
				throw new ArgumentNullException("sourceOrderRowChangeEventArgs");

			// We're only interested in additions and changes that affect the WorkingOrder records in this blotter.
			if (sourceOrderRowChangeEventArgs.Action == DataRowAction.Add || sourceOrderRowChangeEventArgs.Action == DataRowAction.Change)
			{
				DataModel.WorkingOrderRow workingOrderRow = sourceOrderRowChangeEventArgs.Row.WorkingOrderRow;
				if (this.blotterIdSet.Contains(workingOrderRow.BlotterId))
				{
					Int32 index = this.BinarySearch(order => order.WorkingOrderId, workingOrderRow.WorkingOrderId);
					if (index >= 0)
						this.UpdateSourceOrderQuantity(this[index], workingOrderRow);
				}
			}

		}

		/// <summary>
		/// Handles the deletion of a SourceOrder row.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="sourceOrderRowChangeEventArgs">The event arguments.</param>
		protected virtual void OnSourceOrderRowDeleted(Object sender, DataModel.SourceOrderRowChangeEventArgs sourceOrderRowChangeEventArgs)
		{

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (sourceOrderRowChangeEventArgs == null)
				throw new ArgumentNullException("sourceOrderRowChangeEventArgs");

			// We're only interested in deletes that affect the WorkingOrder records in this blotter.
			if (sourceOrderRowChangeEventArgs.Action == DataRowAction.Delete)
			{
				Guid workingOrderId = (Guid)sourceOrderRowChangeEventArgs.Row[DataModel.SourceOrder.WorkingOrderIdColumn, DataRowVersion.Original];
				DataModel.WorkingOrderRow workingOrderRow = DataModel.WorkingOrder.WorkingOrderKey.Find(workingOrderId);
				if (workingOrderRow != null && this.blotterIdSet.Contains(workingOrderRow.BlotterId))
				{
					Int32 index = this.BinarySearch(order => order.WorkingOrderId, workingOrderRow.WorkingOrderId);
					if (index >= 0)
						this.UpdateSourceOrderQuantity(this[index], workingOrderRow);
				}
			}

		}

		/// <summary>
		/// Handles a change to the WorkingOrder row.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="workingOrderRowChangeEventArgs">The event arguments.</param>
		protected virtual void OnWorkingOrderRowChanged(Object sender, DataModel.WorkingOrderRowChangeEventArgs workingOrderRowChangeEventArgs)
		{

			// Validate the parameters.
			if (sender == null)
				throw new ArgumentNullException("sender");
			if (workingOrderRowChangeEventArgs == null)
				throw new ArgumentNullException("workingOrderRowChangeEventArgs");

			// If the new working order belongs to this blotter or one of its descendants, then add it in the proper place.
			if (workingOrderRowChangeEventArgs.Action == DataRowAction.Add)
			{
				DataModel.WorkingOrderRow workingOrderRow = workingOrderRowChangeEventArgs.Row;
				if (this.blotterIdSet.Contains(workingOrderRow.BlotterId))
				{
					Int32 index = this.BinarySearch(order => order.WorkingOrderId, workingOrderRow.WorkingOrderId);
					this.Insert(~index, this.CreateInstanceCore(workingOrderRowChangeEventArgs.Row));
				}
			}

			// This will copy the modified elements from the data model into the collection and commit the changes.
			if (workingOrderRowChangeEventArgs.Action == DataRowAction.Change)
			{
				DataModel.WorkingOrderRow workingOrderRow = workingOrderRowChangeEventArgs.Row;
				if (this.blotterIdSet.Contains(workingOrderRow.BlotterId))
				{
					Int32 index = this.BinarySearch(order => order.WorkingOrderId, workingOrderRow.WorkingOrderId);
					WorkingOrder workingOrder = this[index];
					this.workingOrderCollectionView.EditItem(workingOrder);
					workingOrder.IsActive = workingOrderRow.StatusRow.StatusCode != StatusCode.Filled;
					workingOrder.BlotterName = workingOrderRow.BlotterRow.EntityRow.Name;
					workingOrder.CreatedBy = workingOrderRow.UserRowByFK_User_WorkingOrder_CreatedUserId.EntityRow.Name;
					workingOrder.CreatedTime = workingOrderRow.CreatedTime;
					workingOrder.ModifiedBy = workingOrderRow.UserRowByFK_User_WorkingOrder_ModifiedUserId.EntityRow.Name;
					workingOrder.ModifiedTime = workingOrderRow.ModifiedTime;
					workingOrder.RowVersion = workingOrderRow.RowVersion;
					workingOrder.SecurityName = workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SecurityId.EntityRow.Name;
					workingOrder.SettlementDate = workingOrderRow.SettlementDate;
					workingOrder.SideCode = workingOrderRow.SideRow.SideCode;
					workingOrder.SideMnemonic = workingOrderRow.SideRow.Mnemonic;
					workingOrder.StatusCode = workingOrderRow.StatusRow.StatusCode;
					workingOrder.Symbol = workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SecurityId.Symbol;
					workingOrder.TradeDate = workingOrderRow.TradeDate;
					workingOrder.WorkingOrderId = workingOrderRow.WorkingOrderId;
					this.workingOrderCollectionView.CommitEdit();
				}

			}
		}

		/// <summary>
		/// Updates the aggregates associated with a DestinationOrder.
		/// </summary>
		/// <param name="workingOrderRow">The working order to which the DestinationOrder belongs.</param>
		void UpdateDestinationOrderQuantity(WorkingOrder workingOrder, DataModel.WorkingOrderRow workingOrderRow)
		{

			// Aggregate the execution and destination order quantities.
			Decimal executionQuantity = 0.0m;
			Decimal destinationOrderQuantity = 0.0m;
			foreach (DataModel.DestinationOrderRow destinationOrderRow in workingOrderRow.GetDestinationOrderRows())
			{
				destinationOrderQuantity += (destinationOrderRow.OrderedQuantity - destinationOrderRow.CanceledQuantity);
				foreach (DataModel.ExecutionRow executionRow in destinationOrderRow.GetExecutionRows())
					executionQuantity += executionRow.ExecutionQuantity;
			}

			// Update and commit the changes to the WorkingOrder record.
			this.workingOrderCollectionView.EditItem(workingOrder);
			workingOrder.DestinationOrderQuantity = destinationOrderQuantity;
			workingOrder.LeavesQuantity = destinationOrderQuantity - executionQuantity;
			this.workingOrderCollectionView.CommitEdit();

		}

		/// <summary>
		/// Updates the aggregates associated with an Execution.
		/// </summary>
		/// <param name="workingOrderRow">The working order to which the Execution belongs.</param>
		void UpdateExecutionQuantity(WorkingOrder workingOrder, DataModel.WorkingOrderRow workingOrderRow)
		{

			// Aggregate the execution and destination order quantities.
			Decimal executionQuantity = 0.0m;
			Decimal destinationOrderQuantity = 0.0m;
			foreach (DataModel.DestinationOrderRow destinationOrderRow in workingOrderRow.GetDestinationOrderRows())
			{
				destinationOrderQuantity += (destinationOrderRow.OrderedQuantity - destinationOrderRow.CanceledQuantity);
				foreach (DataModel.ExecutionRow executionRow in destinationOrderRow.GetExecutionRows())
					executionQuantity += executionRow.ExecutionQuantity;
			}

			// Update and commit the changes to the WorkingOrder record.
			this.workingOrderCollectionView.EditItem(workingOrder);
			workingOrder.ExecutionQuantity = executionQuantity;
			workingOrder.LeavesQuantity = destinationOrderQuantity - executionQuantity;
			this.workingOrderCollectionView.CommitEdit();

		}

		/// <summary>
		/// Updates the aggregates associated with a SourceOrder.
		/// </summary>
		/// <param name="workingOrderRow">The working order to which the SourceOrder belongs.</param>
		void UpdateSourceOrderQuantity(WorkingOrder workingOrder, DataModel.WorkingOrderRow workingOrderRow)
		{

			// These factors are used display the quantities and prices in industry standard notations.
			Decimal quantityFactor = workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SecurityId.QuantityFactor;
			Decimal priceFactor = workingOrderRow.SecurityRowByFK_Security_WorkingOrder_SecurityId.PriceFactor;

			// Aggregate the execution and source order quantities.
			Decimal sourceOrderQuantity = 0.0m;
			foreach (DataModel.SourceOrderRow sourceOrderRow in workingOrderRow.GetSourceOrderRows())
				sourceOrderQuantity += sourceOrderRow.OrderedQuantity;

			// Update and commit the changes to the WorkingOrder record.
			this.workingOrderCollectionView.EditItem(workingOrder);
			workingOrder.SourceOrderQuantity = sourceOrderQuantity;
			workingOrder.MarketValue = sourceOrderQuantity * quantityFactor * workingOrder.LastPrice.Price * priceFactor;
			this.workingOrderCollectionView.CommitEdit();

		}

	}

}
