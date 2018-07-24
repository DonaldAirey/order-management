namespace Teraque.AssetNetwork.Windows
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Data;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Windows;
	using System.Windows.Input;
	using Teraque.AssetNetwork.WebService;
	using Teraque.AssetNetwork.Windows;
	using Teraque.Windows.Controls;
	using Teraque.Windows.Controls.Primitives;

	/// <summary>
	/// Represents a base control that displays a generic trade blotter.
	/// </summary>
	/// <copyright>Copyright © 2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class BlotterPageBase : ExplorerPage, INotifyPropertyChanged
	{

		/// <summary>
		/// The unique identifier of this blotter.
		/// </summary>
		Guid blotterIdField;

		/// <summary>
		/// Contains the identifiers of this and all the child blotters.  Used to quickly assess if a given event pertains to this collection.
		/// </summary>
		HashSet<Guid> blotterIdSet = new HashSet<Guid>();

		/// <summary>
		/// The time the blotter was created.
		/// </summary>
		DateTime createdTimeField;

		/// <summary>
		/// The quantity of shares sent to destinations on this page.
		/// </summary>
		Decimal destinationOrderQuantityField;

		/// <summary>
		/// The gap between the metadata key and the value.
		/// </summary>
		const Double centeredMetadataGap = 5.0;

		/// <summary>
		/// The ratio of shares executed to the number of shares sent to a destation on this page.
		/// </summary>
		Double executedPercentField;

		/// <summary>
		/// The quantity of shares executed on this page.
		/// </summary>
		Decimal executionQuantityField;

		/// <summary>
		/// The number of working orders on this page that have been filled.
		/// </summary>
		Int32 filledOrderCountField;

		/// <summary>
		/// The collection of metadata items that are displayed in the DetailBar.
		/// </summary>
		ObservableCollection<FrameworkElement> metadataField = new ObservableCollection<FrameworkElement>();

		/// <summary>
		/// The time the blotter was last modified.
		/// </summary>
		DateTime modifiedTimeField;

		/// <summary>
		/// The number of new working orders on this page.
		/// </summary>
		Int32 newOrderCountField;

		/// <summary>
		/// The ratio of shares sent to a destination to be executed to the total number of shares ordered on this page.
		/// </summary>
		Double orderedPercentField;

		/// <summary>
		/// The number of working orders on this page that have been partially filled.
		/// </summary>
		Int32 partialOrderCountField;

		/// <summary>
		/// The quantity of shares that have been ordered on this page.
		/// </summary>
		Decimal sourceOrderQuantityField;

		/// <summary>
		/// The number of working orders on this page.
		/// </summary>
		Int32 workingOrderCountField;

		/// <summary>
		/// Notifies listeners that a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Initializes a new instance of BlotterPageBase class.
		/// </summary>
		public BlotterPageBase()
		{

			// Pages are ephemeral.  These event handlers will load the context for this page when the page is navigated to and will unload the context when
			// navigated away from.
			this.Loaded += new RoutedEventHandler(this.OnLoaded);
			this.Unloaded += new RoutedEventHandler(this.OnUnloaded);

		}

		/// <summary>
		/// Gets the unique identifier of this blotter.
		/// </summary>
		public Guid BlotterId
		{
			get
			{
				return this.blotterIdField;
			}
		}

		/// <summary>
		/// Gets or sets the time the blotter was last created.
		/// </summary>
		public DateTime CreatedTime
		{
			get
			{
				return this.createdTimeField;
			}
			set
			{
				if (this.createdTimeField != value)
				{
					this.createdTimeField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("CreatedTime"));
				}
			}
		}

		/// <summary>
		/// Gets or sets the destination order quantity on this page.
		/// </summary>
		public Decimal DestinationOrderQuantity
		{
			get
			{
				return this.destinationOrderQuantityField;
			}
			set
			{
				if (this.destinationOrderQuantityField != value)
				{
					this.destinationOrderQuantityField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("DestinationOrderQuantity"));
				}
			}
		}

		/// <summary>
		/// Gets or sets the destination order quantity on this page.
		/// </summary>
		public Double ExecutedPercent
		{
			get
			{
				return this.executedPercentField;
			}
			set
			{
				if (this.executedPercentField != value)
				{
					this.executedPercentField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("ExecutedPercent"));
				}
			}
		}

		/// <summary>
		/// Gets or sets the destination order quantity on this page.
		/// </summary>
		public Decimal ExecutionQuantity
		{
			get
			{
				return this.executionQuantityField;
			}
			set
			{
				if (this.executionQuantityField != value)
				{
					this.executionQuantityField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("ExecutionQuantity"));
				}
			}
		}

		/// <summary>
		/// Gets or sets the destination order quantity on this page.
		/// </summary>
		public Int32 FilledOrderCount
		{
			get
			{
				return this.filledOrderCountField;
			}
			set
			{
				if (this.filledOrderCountField != value)
				{
					this.filledOrderCountField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("FilledOrderCount"));
				}
			}
		}

		/// <summary>
		/// Gets the collection of Metadata elements.
		/// </summary>
		protected ObservableCollection<FrameworkElement> Metadata
		{
			get
			{
				return this.metadataField;
			}
		}

		/// <summary>
		/// Gets or sets the time the blotter was last modified.
		/// </summary>
		public DateTime ModifiedTime
		{
			get
			{
				return this.modifiedTimeField;
			}
			set
			{
				if (this.modifiedTimeField != value)
				{
					this.modifiedTimeField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("ModifiedTime"));
				}
			}
		}

		/// <summary>
		/// Gets or sets the destination order quantity on this page.
		/// </summary>
		public Int32 NewOrderCount
		{
			get
			{
				return this.newOrderCountField;
			}
			set
			{
				if (this.newOrderCountField != value)
				{
					this.newOrderCountField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("NewOrderCount"));
				}
			}
		}

		/// <summary>
		/// Gets or sets the destination order quantity on this page.
		/// </summary>
		public Double OrderedPercent
		{
			get
			{
				return this.orderedPercentField;
			}
			set
			{
				if (this.orderedPercentField != value)
				{
					this.orderedPercentField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("OrderedPercent"));
				}
			}
		}

		/// <summary>
		/// Gets or sets the destination order quantity on this page.
		/// </summary>
		public Int32 PartialOrderCount
		{
			get
			{
				return this.partialOrderCountField;
			}
			set
			{
				if (this.partialOrderCountField != value)
				{
					this.partialOrderCountField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("PartialOrderCount"));
				}
			}
		}

		/// <summary>
		/// Gets or sets the destination order quantity on this page.
		/// </summary>
		public Decimal SourceOrderQuantity
		{
			get
			{
				return this.sourceOrderQuantityField;
			}
			set
			{
				if (this.sourceOrderQuantityField != value)
				{
					this.sourceOrderQuantityField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("SourceOrderQuantity"));
				}
			}
		}

		/// <summary>
		/// Gets or sets the destination order quantity on this page.
		/// </summary>
		public Int32 WorkingOrderCount
		{
			get
			{
				return this.workingOrderCountField;
			}
			set
			{
				if (this.workingOrderCountField != value)
				{
					this.workingOrderCountField = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("WorkingOrderCount"));
				}
			}
		}

		/// <summary>
		/// Collect the metadata from the data model.
		/// </summary>
		/// <param name="state">The generic thread start parameter.</param>
		protected virtual void CalculateMetadata()
		{

			// In order to prevent unnecessary updates to properties that are bound to visual elements, the calculations will fill in these fields first, then
			// update the bound properties.
			Int32 workingOrderCount = 0;
			Int32 filledOrderCount = 0;
			Int32 partialOrderCount = 0;
			Int32 newOrderCount = 0;
			Decimal executionQuantity = 0.0m;
			Decimal destinationOrderQuantity = 0.0m;
			Decimal sourceOrderQuantity = 0.0m;

			// Extract the created and modified times from the main blotter.  The other properties are recursively calculated.
			DataModel.BlotterRow mainBlotterRow = DataModel.Blotter.BlotterKey.Find(this.BlotterId);
			this.CreatedTime = mainBlotterRow.EntityRow.CreatedTime;
			this.ModifiedTime = mainBlotterRow.EntityRow.ModifiedTime;

			// Recursively calculate the statistics for the displayed blotter.
			foreach (Guid blotterId in this.blotterIdSet)
			{

				// Run through all the working orders in the next blotter in the hierarchy and aggregate the statistics.
				DataModel.BlotterRow blotterRow = DataModel.Blotter.BlotterKey.Find(blotterId);
				foreach (DataModel.WorkingOrderRow workingOrderRow in blotterRow.GetWorkingOrderRows())
				{

					// Aggregate the order totals.
					if (workingOrderRow.StatusCode == StatusCode.Filled)
						filledOrderCount++;
					if (workingOrderRow.StatusCode == StatusCode.PartiallyFilled)
						partialOrderCount++;
					if (workingOrderRow.StatusCode == StatusCode.New)
						newOrderCount++;
					workingOrderCount++;

					// Aggregate the total number of source orders.
					foreach (DataModel.SourceOrderRow sourceOrderRow in workingOrderRow.GetSourceOrderRows())
						sourceOrderQuantity += sourceOrderRow.OrderedQuantity;

					// Aggregate the total number of destination orders and executed shares.
					foreach (DataModel.DestinationOrderRow destinationOrderRow in workingOrderRow.GetDestinationOrderRows())
					{
						destinationOrderQuantity += (destinationOrderRow.OrderedQuantity - destinationOrderRow.CanceledQuantity);
						foreach (DataModel.ExecutionRow executionRow in destinationOrderRow.GetExecutionRows())
							executionQuantity += executionRow.ExecutionQuantity;
					}

				}

			}

			// After aggregating all the totals we udpate the properties that are tied (for the most part) to the visual elements.
			this.FilledOrderCount = filledOrderCount;
			this.PartialOrderCount = partialOrderCount;
			this.NewOrderCount = newOrderCount;
			this.SourceOrderQuantity = sourceOrderQuantity;
			this.WorkingOrderCount = workingOrderCount;
			this.SourceOrderQuantity = sourceOrderQuantity;
			this.DestinationOrderQuantity = destinationOrderQuantity;
			this.ExecutionQuantity = executionQuantity;
			this.OrderedPercent = destinationOrderQuantity == 0.0m ? 0.0 : Convert.ToDouble(destinationOrderQuantity / sourceOrderQuantity);
			this.ExecutedPercent = executionQuantity == 0.0m ? 0.0 : Convert.ToDouble(executionQuantity / destinationOrderQuantity);

		}

		/// <summary>
		/// Recursively discovers all the ancestor blotters of the current blotter.
		/// </summary>
		/// <param name="blotterRow">The current blotter row in the hierarchy.</param>
		void DiscoverBlotters(DataModel.BlotterRow blotterRow)
		{

			// Add the current blotter id to the set of all blotters handled by this control.
			this.blotterIdSet.Add(blotterRow.BlotterId);

			// This will recurse into the hierarchy and collect the ancestor blotter identifiers.
			var childBlotterRows = from entityTreeItem in blotterRow.EntityRow.GetEntityTreeRowsByFK_Entity_EntityTree_ParentId()
								   from childBlotterRow in entityTreeItem.EntityRowByFK_Entity_EntityTree_ChildId.GetBlotterRows()
								   select childBlotterRow;
			foreach (DataModel.BlotterRow childBlotterRow in childBlotterRows)
				this.DiscoverBlotters(childBlotterRow);

		}

		/// <summary>
		/// Initializes the metadata key/value pairs.
		/// </summary>
		protected virtual void InitializeMetadata()
		{

			// This will create elements for all the metadata key/value pairs that is displayed in the DetailsBar.
			this.metadataField.Add(DetailBar.CreateMetadataElement("Date modified:", this, DetailBar.CreateDateTimeMetadataValue("ModifiedTime")));
			this.metadataField.Add(DetailBar.CreateMetadataElement("Date Created:", this, DetailBar.CreateDateTimeMetadataValue("CreatedTime")));
			this.metadataField.Add(DetailBar.CreateMetadataElement("Filled Orders:", this, DetailBar.CreateInt32MetadataValue("FilledOrderCount")));
			this.metadataField.Add(DetailBar.CreateMetadataElement("New Orders:", this, DetailBar.CreateInt32MetadataValue("NewOrderCount")));
			this.metadataField.Add(DetailBar.CreateMetadataElement("Partial Orders:", this, DetailBar.CreateInt32MetadataValue("PartialOrderCount")));
			this.metadataField.Add(DetailBar.CreateMetadataElement("Total Working Orders:", this, DetailBar.CreateInt32MetadataValue("WorkingOrderCount")));
			this.metadataField.Add(DetailBar.CreateMetadataElement("Ordered:", this, DetailBar.CreateProgressMetadataValue("OrderedPercent", 0.0, 1.0)));
			this.metadataField.Add(DetailBar.CreateMetadataElement("Executed:", this, DetailBar.CreateProgressMetadataValue("ExecutedPercent", 0.0, 1.0)));

		}

		/// <summary>
		/// Handles a change to a DestinationOrder row.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="destinationOrderRowChangeEventArgs">The event arguments.</param>
		void OnDestinationOrderRowChanged(Object sender, DataModel.DestinationOrderRowChangeEventArgs destinationOrderRowChangeEventArgs)
		{

			// We're only interested in additions and changes that affect the WorkingOrder records in this blotter.
			if (destinationOrderRowChangeEventArgs.Action == DataRowAction.Add || destinationOrderRowChangeEventArgs.Action == DataRowAction.Change)
			{

				// This is designed to filter out all events that don't pertain to this blotter.
				DataModel.DestinationOrderRow destinationOrderRow = destinationOrderRowChangeEventArgs.Row;
				if (this.blotterIdSet.Contains(destinationOrderRow.WorkingOrderRow.BlotterId))
				{

					// Once the previous order is subtracted and the current order added to the totals we can calculate the percentages of orders completed and 
					// executed.
					if (destinationOrderRow.HasVersion(DataRowVersion.Original))
						this.destinationOrderQuantityField -= (Decimal)destinationOrderRow[DataModel.DestinationOrder.OrderedQuantityColumn, DataRowVersion.Original] -
							(Decimal)destinationOrderRow[DataModel.DestinationOrder.CanceledQuantityColumn, DataRowVersion.Original];
					this.DestinationOrderQuantity += destinationOrderRow.OrderedQuantity - destinationOrderRow.CanceledQuantity;
					this.OrderedPercent = this.SourceOrderQuantity == 0.0m ? 0.0 : Convert.ToDouble(this.DestinationOrderQuantity / this.SourceOrderQuantity);
					this.ExecutedPercent = this.DestinationOrderQuantity == 0.0m ? 0.0 : Convert.ToDouble(this.ExecutionQuantity / this.DestinationOrderQuantity);

				}

			}

		}

		/// <summary>
		/// Handles the deletion of a DestinationOrder row.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="destinationOrderRowChangeEventArgs">The event arguments.</param>
		void OnDestinationOrderRowDeleted(Object sender, DataModel.DestinationOrderRowChangeEventArgs destinationOrderRowChangeEventArgs)
		{

			// We're only interested in deletes that affect the WorkingOrder records in this blotter.
			if (destinationOrderRowChangeEventArgs.Action == DataRowAction.Delete)
			{

				// Filtering requires a little more work with a deleted record.  We need to first find the original record and then look up the working order to 
				// which it belonged.  We don't need to check for the existence of the working order as we know that this order was just deleted and, for it to have
				// existed in the first place, there must have been a working order.
				DataModel.DestinationOrderRow destinationOrderRow = destinationOrderRowChangeEventArgs.Row;
				Guid workingOrderId = (Guid)destinationOrderRow[DataModel.DestinationOrder.WorkingOrderIdColumn, DataRowVersion.Original];
				DataModel.WorkingOrderRow workingOrderRow = DataModel.WorkingOrder.WorkingOrderKey.Find(workingOrderId);
				if (this.blotterIdSet.Contains(workingOrderRow.BlotterId))
				{

					// Once the quantity has been removed from the totals, the percent ordered and executed can be updated.
					this.DestinationOrderQuantity -= (Decimal)destinationOrderRow[DataModel.DestinationOrder.OrderedQuantityColumn, DataRowVersion.Original] -
						(Decimal)destinationOrderRow[DataModel.DestinationOrder.CanceledQuantityColumn, DataRowVersion.Original];
					this.OrderedPercent = this.SourceOrderQuantity == 0.0m ? 0.0 : Convert.ToDouble(this.DestinationOrderQuantity / this.SourceOrderQuantity);
					this.ExecutedPercent = this.DestinationOrderQuantity == 0.0m ? 0.0 : Convert.ToDouble(this.ExecutionQuantity / this.DestinationOrderQuantity);

				}

			}

		}

		/// <summary>
		/// Handles a change to a Entity row.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="entityRowChangeEventArgs">The event arguments.</param>
		void OnEntityRowChanged(Object sender, DataModel.EntityRowChangeEventArgs entityRowChangeEventArgs)
		{

			// We're only interested in changes that affect this Entity.  When the created or modified dates change, update the related properties in the MVVM.
			if (entityRowChangeEventArgs.Action == DataRowAction.Change)
			{
				DataModel.EntityRow entityRow = entityRowChangeEventArgs.Row;
				if (entityRow.EntityId == this.BlotterId)
				{
					this.CreatedTime = entityRow.CreatedTime;
					this.ModifiedTime = entityRow.ModifiedTime;
				}
			}

		}

		/// <summary>
		/// Handles a change to the Execution row.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="executionRowChangeEventArgs">The event arguments.</param>
		void OnExecutionRowChanged(Object sender, DataModel.ExecutionRowChangeEventArgs executionRowChangeEventArgs)
		{

			// We're only interested in additions and changes that affect the WorkingOrder records in this blotter.
			if (executionRowChangeEventArgs.Action == DataRowAction.Add || executionRowChangeEventArgs.Action == DataRowAction.Change)
			{

				// This is designed to filter out all events that don't pertain to this blotter.
				DataModel.ExecutionRow executionRow = executionRowChangeEventArgs.Row;
				DataModel.DestinationOrderRow destinationOrderRow = executionRow.DestinationOrderRow;
				DataModel.WorkingOrderRow workingOrderRow = destinationOrderRow.WorkingOrderRow;
				if (this.blotterIdSet.Contains(workingOrderRow.BlotterId))
				{

					// Once the previous execution is subtracted and the current execution added to the totals we can calculate the percent executed.
					if (executionRow.HasVersion(DataRowVersion.Original))
						this.executionQuantityField -= (Decimal)executionRow[DataModel.Execution.ExecutionQuantityColumn, DataRowVersion.Original];
					this.ExecutionQuantity += executionRow.ExecutionQuantity;
					this.ExecutedPercent = this.DestinationOrderQuantity == 0.0m ? 0.0 : Convert.ToDouble(this.ExecutionQuantity / this.DestinationOrderQuantity);

				}

			}

		}

		/// <summary>
		/// Handles the deletion of a Execution row.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="executionRowChangeEventArgs">The event arguments.</param>
		void OnExecutionRowDeleted(Object sender, DataModel.ExecutionRowChangeEventArgs executionRowChangeEventArgs)
		{

			// We're only interested in deletes that affect the WorkingOrder records in this blotter.
			if (executionRowChangeEventArgs.Action == DataRowAction.Delete)
			{

				// Filtering requires a little more work with a deleted record.  We need to first find the original record and then look up the destination and  
				// working order to which it belonged.  We don't need to check for the existence of the destination order or working order as we know that this 
				// execution was just deleted and, for it to have existed in the first place, there must have been a parent destination and working order.
				DataModel.ExecutionRow executionRow = executionRowChangeEventArgs.Row;
				Guid destinationOrderId = (Guid)executionRow[DataModel.Execution.DestinationOrderIdColumn, DataRowVersion.Original];
				DataModel.DestinationOrderRow destinationOrderRow = DataModel.DestinationOrder.DestinationOrderKey.Find(destinationOrderId);
				DataModel.WorkingOrderRow workingOrderRow = destinationOrderRow.WorkingOrderRow;
				if (this.blotterIdSet.Contains(workingOrderRow.BlotterId))
				{

					// Once the quantity has been removed from the totals, the percent executed can be updated.
					this.ExecutionQuantity -= (Decimal)executionRow[DataModel.Execution.ExecutionQuantityColumn, DataRowVersion.Original];
					this.ExecutedPercent = this.DestinationOrderQuantity == 0.0m ? 0.0 : Convert.ToDouble(this.ExecutionQuantity / this.DestinationOrderQuantity);

				}

			}

		}

		/// <summary>
		/// Occurs when the element is laid out, rendered, and ready for interaction.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="routedEventArgs">The event data.</param>
		protected virtual void OnLoaded(Object sender, RoutedEventArgs routedEventArgs)
		{

			// This provides a data context for the blotter.
			AssetNetworkItem assetNetworkItem = this.DataContext as AssetNetworkItem;
			this.blotterIdField = assetNetworkItem.EntityId;

			// Data model changes can come in high frequency bursts.  To filter out the events that are relevant for this blotter view, we're going to flatten out 
			// the hierarchy and put all the blotter identifiers in a set.  Then, when a given working order row (or any row related to a working order) is 
			// modified or deleted, we can use this set as a filter to quickly establish what events need attension and what events can be ignored.
			this.blotterIdSet.Clear();
			this.DiscoverBlotters(DataModel.Blotter.BlotterKey.Find(this.BlotterId));

			// Call the virtual version of this method to initialize the event handlers.  Descendants can override this behavior to add additional tables to watch
			// for additional metadata.  It should be noted that the BlotterId and the expanded list of blotters has been generated and is available to the
			// descendant classes when this virtual method is called.
			this.OnLoaded();

			// The visual elements that that display metadata key/value pairs are initialized here.
			this.metadataField.Clear();
			this.InitializeMetadata();

			// This will calculate the initial values for the displayed metadata.  After this initial pass, in order to prevent scanning all the working orders in
			// all the blotters for every change that comes in, we'll peform delta operations on just the properties that have changed.
			this.CalculateMetadata();

			// This will send a message up to the frame that there is a collection of metadata available.  This will be bound to the DetailBar by the frame and the
			// items will appear hosted in the Detail bar.  These items in the DetailBar are not copies but the elements created above.  Any change to the items
			// above will be reflected immediately in the DetailBar.
			this.RaiseEvent(new ItemsSourceEventArgs(ExplorerFrame.DetailBarChangedEvent, this.metadataField));

		}

		/// <summary>
		/// Occurs when the element is laid out, rendered, and ready for interaction.
		/// </summary>
		protected virtual void OnLoaded()
		{

			// Link the page into the data model.
			DataModel.DestinationOrder.DestinationOrderRowChanged += this.OnDestinationOrderRowChanged;
			DataModel.DestinationOrder.DestinationOrderRowDeleted += this.OnDestinationOrderRowDeleted;
			DataModel.Entity.EntityRowChanged += this.OnEntityRowChanged;
			DataModel.Execution.ExecutionRowChanged += this.OnExecutionRowChanged;
			DataModel.Execution.ExecutionRowDeleted += this.OnExecutionRowDeleted;
			DataModel.SourceOrder.SourceOrderRowChanged += this.OnSourceOrderRowChanged;
			DataModel.SourceOrder.SourceOrderRowDeleted += this.OnSourceOrderRowDeleted;
			DataModel.WorkingOrder.WorkingOrderRowChanged += this.OnWorkingOrderRowChanged;

		}

		/// <summary>
		/// Occurs when a property has changed.
		/// </summary>
		/// <param name="propertyChangedEventArgs">The event data.</param>
		public void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
		{

			// This will notify anyone listening that the property has changed.
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, propertyChangedEventArgs);

		}

		/// <summary>
		/// Handles a change to a SourceOrder row.
		/// </summary>
		/// <param name="sender">The Object that originated the event.</param>
		/// <param name="sourceOrderRowChangeEventArgs">The event arguments.</param>
		void OnSourceOrderRowChanged(Object sender, DataModel.SourceOrderRowChangeEventArgs sourceOrderRowChangeEventArgs)
		{

			// We're only interested in additions and changes that affect the WorkingOrder records in this blotter.
			if (sourceOrderRowChangeEventArgs.Action == DataRowAction.Add || sourceOrderRowChangeEventArgs.Action == DataRowAction.Change)
			{

				// This is designed to filter out all events that don't pertain to this blotter.
				DataModel.SourceOrderRow sourceOrderRow = sourceOrderRowChangeEventArgs.Row;
				if (this.blotterIdSet.Contains(sourceOrderRow.WorkingOrderRow.BlotterId))
				{

					// Once the previous order is subtracted and the current order added to the totals we can calculate the percentages of orders completed.
					if (sourceOrderRow.HasVersion(DataRowVersion.Original))
						this.sourceOrderQuantityField -= (Decimal)sourceOrderRow[DataModel.SourceOrder.OrderedQuantityColumn, DataRowVersion.Original];
					this.SourceOrderQuantity += sourceOrderRow.OrderedQuantity;
					this.OrderedPercent = this.SourceOrderQuantity == 0.0m ? 0.0 : Convert.ToDouble(this.DestinationOrderQuantity / this.SourceOrderQuantity);

				}

			}

		}

		/// <summary>
		/// Handles the deletion of a SourceOrder row.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="sourceOrderRowChangeEventArgs">The event arguments.</param>
		void OnSourceOrderRowDeleted(Object sender, DataModel.SourceOrderRowChangeEventArgs sourceOrderRowChangeEventArgs)
		{

			// We're only interested in deletes that affect the WorkingOrder records in this blotter.
			if (sourceOrderRowChangeEventArgs.Action == DataRowAction.Delete)
			{

				// Filtering requires a little more work with a deleted record.  We need to first find the original record and then look up the working order to
				// which it belonged.  We don't need to check for the existence of the working order as we know that this order was just deleted and, for it to have
				// existed in the first place, there must have been a working order.
				DataModel.SourceOrderRow sourceOrderRow = sourceOrderRowChangeEventArgs.Row;
				Guid workingOrderId = (Guid)sourceOrderRow[DataModel.SourceOrder.WorkingOrderIdColumn, DataRowVersion.Original];
				DataModel.WorkingOrderRow workingOrderRow = DataModel.WorkingOrder.WorkingOrderKey.Find(workingOrderId);
				if (this.blotterIdSet.Contains(workingOrderRow.BlotterId))
				{

					// Once the quantity has been removed from the totals, the percent ordered and executed can be updated.
					this.SourceOrderQuantity -= (Decimal)sourceOrderRow[DataModel.SourceOrder.OrderedQuantityColumn, DataRowVersion.Original];
					this.OrderedPercent = this.SourceOrderQuantity == 0.0m ? 0.0 : Convert.ToDouble(this.DestinationOrderQuantity / this.SourceOrderQuantity);

				}

			}

		}

		/// <summary>
		/// Occurs when the element is removed from within an element tree of loaded elements.
		/// </summary>
		protected virtual void OnUnloaded()
		{

			// Unhook this page from the data model when we're unloaded.
			DataModel.DestinationOrder.DestinationOrderRowChanged -= this.OnDestinationOrderRowChanged;
			DataModel.DestinationOrder.DestinationOrderRowDeleted -= this.OnDestinationOrderRowDeleted;
			DataModel.Entity.EntityRowChanged -= this.OnEntityRowChanged;
			DataModel.Execution.ExecutionRowChanged -= this.OnExecutionRowChanged;
			DataModel.Execution.ExecutionRowDeleted -= this.OnExecutionRowDeleted;
			DataModel.SourceOrder.SourceOrderRowChanged -= this.OnSourceOrderRowChanged;
			DataModel.SourceOrder.SourceOrderRowDeleted -= this.OnSourceOrderRowDeleted;
			DataModel.WorkingOrder.WorkingOrderRowChanged -= this.OnWorkingOrderRowChanged;

		}

		/// <summary>
		/// Occurs when the element is removed from within an element tree of loaded elements.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		void OnUnloaded(Object sender, RoutedEventArgs e)
		{

			// Call the overridable version of this event handler.
			this.OnUnloaded();

		}

		/// <summary>
		/// Handles a change to the WorkingOrder row.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="workingOrderRowChangeEventArgs">The event arguments.</param>
		void OnWorkingOrderRowChanged(Object sender, DataModel.WorkingOrderRowChangeEventArgs workingOrderRowChangeEventArgs)
		{

			// If the new working order belongs to this blotter or one of its descendants, then add it in the proper place.
			if (workingOrderRowChangeEventArgs.Action == DataRowAction.Add)
			{
				DataModel.WorkingOrderRow workingOrderRow = workingOrderRowChangeEventArgs.Row;
				if (this.blotterIdSet.Contains(workingOrderRow.BlotterId))
					this.WorkingOrderCount++;
			}

			// This will handle changes to the working order.
			if (workingOrderRowChangeEventArgs.Action == DataRowAction.Change)
			{

				// This is designed to filter out all events that don't pertain to this blotter.
				DataModel.WorkingOrderRow workingOrderRow = workingOrderRowChangeEventArgs.Row;
				if (this.blotterIdSet.Contains(workingOrderRow.BlotterId))
				{

					// In order to minimize the number of times we update a propertie that is bound to a visual control, we'll get copies of the current statistics
					// and perform the updates on them.
					Int32 filledOrderCount = this.FilledOrderCount;
					Int32 partialOrderCount = this.PartialOrderCount;
					Int32 newOrderCount = this.NewOrderCount;

					// This will remove the previous status code (if there was a previous value) from the statistics.
					if (workingOrderRow.HasVersion(DataRowVersion.Original))
					{
						StatusCode oldStatusCode = (StatusCode)workingOrderRow[DataModel.WorkingOrder.StatusCodeColumn, DataRowVersion.Original];
						if (oldStatusCode == StatusCode.Filled)
							filledOrderCount--;
						if (oldStatusCode == StatusCode.PartiallyFilled)
							partialOrderCount--;
						if (oldStatusCode == StatusCode.New)
							newOrderCount--;
					}

					// This will update the statistics with the current status of the working order.
					StatusCode newStatusCode = workingOrderRow.StatusCode;
					if (newStatusCode == StatusCode.Filled)
						filledOrderCount++;
					if (newStatusCode == StatusCode.PartiallyFilled)
						partialOrderCount++;
					if (newStatusCode == StatusCode.New)
						newOrderCount++;

					// We've updated all the statistics and can put them back into the properties tied to the metadata controls.
					this.FilledOrderCount = filledOrderCount;
					this.PartialOrderCount = partialOrderCount;
					this.NewOrderCount = newOrderCount;

				}

			}

		}

	}

}
