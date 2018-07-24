namespace Teraque
{

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Runtime.Serialization;
	using Teraque;

	/// <summary>
	/// This object is used to contain Fields (Fields are Tag-object pairs).
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[DataContract]
	[KnownType(typeof(OrderTypeCode))]
	[KnownType(typeof(OrderStatusCode))]
	[KnownType(typeof(SideCode))]
	[KnownType(typeof(StatusCode))]
	[KnownType(typeof(TimeInForceCode))]
	public class Message : IEnumerable<KeyValuePair<Tag, Object>>
	{

		/// <summary>
		/// A dictionary of tags.
		/// </summary>
		[DataMember]
		Dictionary<Tag, Object> tagDictionary;

		/// <summary>
		/// Initializes a new instance of a Message class.
		/// </summary>
		public Message()
		{

			// Initailize the object.
			this.tagDictionary = new Dictionary<Tag, object>();

		}

		/// <summary>
		/// Initializes a new instance of the Message class that contains elements copied from the specified Message.
		/// </summary>
		/// <param name="message">The Message whose elements are copied to the new Message.</param>
		public Message(Message message)
		{

			// Initialize the object with the source message.
			this.tagDictionary = new Dictionary<Tag, object>(message.tagDictionary);

		}

		/// <summary>
		/// Gets or sets the value in the message at the specified index.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <returns>The value in the message having the specified tag.</returns>
		public Object this[Tag tag]
		{
			get
			{
				return this.tagDictionary[tag];
			}
			set
			{
				this.tagDictionary[tag] = value;
			}
		}

		/// <summary>
		/// Adds a Field to the message.
		/// </summary>
		/// <param name="field"></param>
		public void Add(Field field)
		{

			// This adds the field to the dictionary.
			this.tagDictionary[field.Tag] = field.Value;

		}

		/// <summary>
		/// Removes all fields from the Message.
		/// </summary>
		public void Clear()
		{

			// Clear the message.
			this.tagDictionary.Clear();

		}

		/// <summary>
		/// Determines whether the Message contains the specified key. 
		/// </summary>
		/// <param name="tag">The key to locate in the Message.</param>
		/// <returns>true if the Message contains an element with the specified key; otherwise, false.</returns>
		public Boolean ContainsKey(Tag tag)
		{

			// Check to see if the message has the specified key.
			return this.tagDictionary.ContainsKey(tag);

		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="tag">The key of the value to get.</param>
		/// <param name="value">
		/// When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the
		/// value parameter. This parameter is passed uninitialized.
		/// </param>
		/// <returns>true if the Message contains an element with the specified key; otherwise, false.</returns>
		public Boolean TryGetValue(Tag tag, out Object value)
		{

			// Get the value from the dictionary.
			return this.tagDictionary.TryGetValue(tag, out value);

		}

		/// <summary>
		/// Returns an enumerator that iterates through the fields in the Message.
		/// </summary>
		/// <returns>An enumerator structure for the Message.</returns>
		public IEnumerator<KeyValuePair<Tag, object>> GetEnumerator()
		{

			// User the dictionary's enumerator when iterating through this object.
			return this.tagDictionary.GetEnumerator();

		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{

			// User the dictionary's untyped enumerator when iterating through this object.
			return ((IEnumerable)this.tagDictionary).GetEnumerator();

		}

		/// <summary>
		/// Packs the Message into a binary format.
		/// </summary>
		/// <returns>A stream of Bytes representing the FIX Message.</returns>
		public virtual Byte[] ToPacket()
		{

			// The default is to encode the string version into an array using Base64 encoding.
			return Convert.FromBase64String(this.ToString());

		}

		/// <summary>
		/// Removes the value with the specified tag from the Message. 
		/// </summary>
		/// <param name="tag">The tag of the field to remove.</param>
		public void Remove(Tag tag)
		{

			// Remove the specified tag from the message.
			this.tagDictionary.Remove(tag);

		}
	
		/// <summary>
		/// Gets or sets the Account.
		/// </summary>
		public String Account
		{
			get
			{
				return this.tagDictionary[Tag.Account] as String;
			}
			set
			{
				this.tagDictionary[Tag.Account] = value;
			}
		}

		/// <summary>
		/// Gets or sets the AvgPx.
		/// </summary>
		public Decimal AvgPx
		{
			get
			{
				return (Decimal)this.tagDictionary[Tag.AvgPx];
			}
			set
			{
				this.tagDictionary[Tag.AvgPx] = value;
			}
		}

		/// <summary>
		/// Gets or sets the BeginSeqNo.
		/// </summary>
		public Int32 BeginSeqNo
		{
			get
			{
				return (Int32)this.tagDictionary[Tag.BeginSeqNo];
			}
			set
			{
				this.tagDictionary[Tag.BeginSeqNo] = value;
			}
		}

		/// <summary>
		/// Gets or sets the BeginString.
		/// </summary>
		public String BeginString
		{
			get
			{
				return this.tagDictionary[Tag.BeginString] as String;
			}
			set
			{
				this.tagDictionary[Tag.BeginString] = value;
			}
		}

		/// <summary>
		/// Gets or sets the BusinessRejectReason.
		/// </summary>
		public BusinessRejectReasonCode BusinessRejectReason
		{
			get
			{
				return (BusinessRejectReasonCode)this.tagDictionary[Tag.BusinessRejectReason];
			}
			set
			{
				this.tagDictionary[Tag.BusinessRejectReason] = value;
			}
		}

		/// <summary>
		/// Gets or sets the BusinessRejectRefID.
		/// </summary>
		public String BusinessRejectRefID
		{
			get
			{
				return this.tagDictionary[Tag.BusinessRejectRefID] as String;
			}
			set
			{
				this.tagDictionary[Tag.BusinessRejectRefID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the CheckSum.
		/// </summary>
		public Int32 CheckSum
		{
			get
			{
				return (Int32)this.tagDictionary[Tag.CheckSum];
			}
			set
			{
				this.tagDictionary[Tag.CheckSum] = value;
			}
		}

		/// <summary>
		/// Gets or sets the ClientID.
		/// </summary>
		public String ClientID
		{
			get
			{
				return this.tagDictionary[Tag.ClientID] as String;
			}
			set
			{
				this.tagDictionary[Tag.ClientID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the ClOrdID.
		/// </summary>
		public String ClOrdID
		{
			get
			{
				return this.tagDictionary[Tag.ClOrdID] as String;
			}
			set
			{
				this.tagDictionary[Tag.ClOrdID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the CmsCxlQty.
		/// </summary>
		public Decimal CmsCxlQty
		{
			get
			{
				return (Decimal)this.tagDictionary[Tag.CmsCxlQty];
			}
			set
			{
				this.tagDictionary[Tag.CmsCxlQty] = value;
			}
		}

		/// <summary>
		/// Gets or sets the CmsLeavesQty.
		/// </summary>
		public Decimal CmsLeavesQty
		{
			get
			{
				return (Decimal)this.tagDictionary[Tag.CmsLeavesQty];
			}
			set
			{
				this.tagDictionary[Tag.CmsLeavesQty] = value;
			}
		}

		/// <summary>
		/// Gets or sets the Commission.
		/// </summary>
		public Decimal Commission
		{
			get
			{
				return (Decimal)this.tagDictionary[Tag.Commission];
			}
			set
			{
				this.tagDictionary[Tag.Commission] = value;
			}
		}

		/// <summary>
		/// Gets or sets the CommType.
		/// </summary>
		public CommissionType CommType
		{
			get
			{
				return (CommissionType)this.tagDictionary[Tag.CommType];
			}
			set
			{
				this.tagDictionary[Tag.CommType] = value;
			}
		}

		/// <summary>
		/// Gets the number of fields in the message.
		/// </summary>
		public Int32 Count
		{
			get
			{
				return this.tagDictionary.Count;
			}
		}

		/// <summary>
		/// Gets or sets the CumQty.
		/// </summary>
		public Decimal CumQty
		{
			get
			{
				return (Decimal)this.tagDictionary[Tag.CumQty];
			}
			set
			{
				this.tagDictionary[Tag.CumQty] = value;
			}
		}

		/// <summary>
		/// Gets or sets the CxlRejReason.
		/// </summary>
		public CancelRejectReasonCode CxlRejReason
		{
			get
			{
				return (CancelRejectReasonCode)this.tagDictionary[Tag.CxlRejReason];
			}
			set
			{
				this.tagDictionary[Tag.CxlRejReason] = value;
			}
		}

		/// <summary>
		/// Gets or sets the CxlRejResponseTo.
		/// </summary>
		public CancelRejectResponseCode CxlRejResponseTo
		{
			get
			{
				return (CancelRejectResponseCode)this.tagDictionary[Tag.CxlRejResponseTo];
			}
			set
			{
				this.tagDictionary[Tag.CxlRejResponseTo] = value;
			}
		}

		/// <summary>
		/// Gets or sets the DeliverToCompID.
		/// </summary>
		public String DeliverToCompID
		{
			get
			{
				return this.tagDictionary[Tag.DeliverToCompID] as String;
			}
			set
			{
				this.tagDictionary[Tag.DeliverToCompID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the DeliverToSubID.
		/// </summary>
		public String DeliverToSubID
		{
			get
			{
				return this.tagDictionary[Tag.DeliverToSubID] as String;
			}
			set
			{
				this.tagDictionary[Tag.DeliverToSubID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the DiscretionInst.
		/// </summary>
		public DiscretionInstructionCode DiscretionInst
		{
			get
			{
				return (DiscretionInstructionCode)this.tagDictionary[Tag.DiscretionInst];
			}
			set
			{
				this.tagDictionary[Tag.DiscretionInst] = value;
			}
		}

		/// <summary>
		/// Gets or sets the DiscretionOffset.
		/// </summary>
		public Decimal DiscretionOffset
		{
			get
			{
				return (Decimal)this.tagDictionary[Tag.DiscretionOffset];
			}
			set
			{
				this.tagDictionary[Tag.DiscretionOffset] = value;
			}
		}

		/// <summary>
		/// Gets or sets the DKReason.
		/// </summary>
		public DontKnowReasonCode DKReason
		{
			get
			{
				return (DontKnowReasonCode)this.tagDictionary[Tag.DKReason];
			}
			set
			{
				this.tagDictionary[Tag.DKReason] = value;
			}
		}

		/// <summary>
		/// Gets or sets the EncodedText.
		/// </summary>
		public String EncodedText
		{
			get
			{
				return this.tagDictionary[Tag.EncodedText] as String;
			}
			set
			{
				this.tagDictionary[Tag.EncodedText] = value;
			}
		}

		/// <summary>
		/// Gets or sets the EncryptMethod.
		/// </summary>
		public EncryptMethodCode EncryptMethod
		{
			get
			{
				return (EncryptMethodCode)this.tagDictionary[Tag.EncryptMethod];
			}
			set
			{
				this.tagDictionary[Tag.EncryptMethod] = value;
			}
		}

		/// <summary>
		/// Gets or sets the EndSeqNo.
		/// </summary>
		public Int32 EndSeqNo
		{
			get
			{
				return (Int32)this.tagDictionary[Tag.EndSeqNo];
			}
			set
			{
				this.tagDictionary[Tag.EndSeqNo] = value;
			}
		}

		/// <summary>
		/// Gets or sets the ExDestination.
		/// </summary>
		public String ExDestination
		{
			get
			{
				return this.tagDictionary[Tag.ExDestination] as String;
			}
			set
			{
				this.tagDictionary[Tag.ExDestination] = value;
			}
		}

		/// <summary>
		/// Gets or sets the ExecBroker.
		/// </summary>
		public String ExecBroker
		{
			get
			{
				return this.tagDictionary[Tag.ExecBroker] as String;
			}
			set
			{
				this.tagDictionary[Tag.ExecBroker] = value;
			}
		}

		/// <summary>
		/// Gets or sets the ExecID.
		/// </summary>
		public String ExecID
		{
			get
			{
				return this.tagDictionary[Tag.ExecID] as String;
			}
			set
			{
				this.tagDictionary[Tag.ExecID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the ExecInst.
		/// </summary>
		public String ExecInst
		{
			get
			{
				return this.tagDictionary[Tag.ExecInst] as String;
			}
			set
			{
				this.tagDictionary[Tag.ExecInst] = value;
			}
		}

		/// <summary>
		/// Gets or sets the ExecRefID.
		/// </summary>
		public String ExecRefID
		{
			get
			{
				return this.tagDictionary[Tag.ExecRefID] as String;
			}
			set
			{
				this.tagDictionary[Tag.ExecRefID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the ExecTransType.
		/// </summary>
		public ExecutionTransactionTypeCode ExecTransType
		{
			get
			{
				return (ExecutionTransactionTypeCode)this.tagDictionary[Tag.ExecTransType];
			}
			set
			{
				this.tagDictionary[Tag.ExecTransType] = value;
			}
		}

		/// <summary>
		/// Gets or sets the ExecType.
		/// </summary>
		public ExecutionTypeCode ExecType
		{
			get
			{
				return (ExecutionTypeCode)this.tagDictionary[Tag.ExecType];
			}
			set
			{
				this.tagDictionary[Tag.ExecType] = value;
			}
		}

		/// <summary>
		/// Gets or sets the ExpireTime.
		/// </summary>
		public DateTime ExpireTime
		{
			get
			{
				return (DateTime)this.tagDictionary[Tag.ExpireTime];
			}
			set
			{
				this.tagDictionary[Tag.ExpireTime] = value;
			}
		}

		/// <summary>
		/// Gets or sets the GapFillFlag.
		/// </summary>
		public Boolean GapFillFlag
		{
			get
			{
				return (Boolean)this.tagDictionary[Tag.GapFillFlag];
			}
			set
			{
				this.tagDictionary[Tag.GapFillFlag] = value;
			}
		}

		/// <summary>
		/// Gets or sets the HandlInst.
		/// </summary>
		public HandleInstructionCode HandlInst
		{
			get
			{
				return (HandleInstructionCode)this.tagDictionary[Tag.HandlInst];
			}
			set
			{
				this.tagDictionary[Tag.HandlInst] = value;
			}
		}

		/// <summary>
		/// Gets or sets the HeartBtInt.
		/// </summary>
		public Int32 HeartBtInt
		{
			get
			{
				return (Int32)this.tagDictionary[Tag.HeartBtInt];
			}
			set
			{
				this.tagDictionary[Tag.HeartBtInt] = value;
			}
		}

		/// <summary>
		/// Gets or sets the InternalError.
		/// </summary>
		public String InternalError
		{
			get
			{
				return this.tagDictionary[Tag.InternalError] as String;
			}
			set
			{
				this.tagDictionary[Tag.InternalError] = value;
			}
		}

		/// <summary>
		/// Gets or sets the InternalRecordId.
		/// </summary>
		public Int32 InternalRecordId
		{
			get
			{
				return (Int32)this.tagDictionary[Tag.InternalRecordId];
			}
			set
			{
				this.tagDictionary[Tag.InternalRecordId] = value;
			}
		}

		/// <summary>
		/// Gets or sets the InternalSourceId.
		/// </summary>
		public String InternalSourceId
		{
			get
			{
				return this.tagDictionary[Tag.InternalSourceId] as String;
			}
			set
			{
				this.tagDictionary[Tag.InternalSourceId] = value;
			}
		}

		/// <summary>
		/// Gets or sets the IoiId.
		/// </summary>
		public String IoiId
		{
			get
			{
				return this.tagDictionary[Tag.IoiId] as String;
			}
			set
			{
				this.tagDictionary[Tag.IoiId] = value;
			}
		}

		/// <summary>
		/// Gets or sets the IoiNaturalFlag
		/// </summary>
		public Boolean IoiNaturalFlag
		{
			get
			{
				return (Boolean)this.tagDictionary[Tag.IoiNaturalFlag];
			}
			set
			{
				this.tagDictionary[Tag.IoiNaturalFlag] = value;
			}
		}

		/// <summary>
		/// Gets or sets the IoiQltyInd.
		/// </summary>
		public IoiQualityIndicatorCode IoiQltyInd
		{
			get
			{
				return (IoiQualityIndicatorCode)this.tagDictionary[Tag.IoiQltyInd];
			}
			set
			{
				this.tagDictionary[Tag.IoiQltyInd] = value;
			}
		}

		/// <summary>
		/// Gets or sets the IoiQualifierGroup.
		/// </summary>
		public List<IoiQualifierCode> IoiQualifierGroup
		{
			get
			{
				return this.tagDictionary[Tag.IoiQualifierGroup] as List<IoiQualifierCode>;
			}
			set
			{
				this.tagDictionary[Tag.IoiQualifierGroup] = value;
			}
		}

		/// <summary>
		/// Gets or sets the IoiRefID.
		/// </summary>
		public String IoiRefID
		{
			get
			{
				return this.tagDictionary[Tag.IoiRefID] as String;
			}
			set
			{
				this.tagDictionary[Tag.IoiRefID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the IoiShares.
		/// </summary>
		public String IoiShares
		{
			get
			{
				return this.tagDictionary[Tag.IoiShares] as String;
			}
			set
			{
				this.tagDictionary[Tag.IoiShares] = value;
			}
		}

		/// <summary>
		/// Gets or sets the IoiTransType.
		/// </summary>
		public IoiTransactionTypeCode IoiTransType
		{
			get
			{
				return (IoiTransactionTypeCode)this.tagDictionary[Tag.IoiTransType];
			}
			set
			{
				this.tagDictionary[Tag.IoiTransType] = value;
			}
		}

		/// <summary>
		/// Gets or sets the LastCapacity.
		/// </summary>
		public LastCapacityCode LastCapacity
		{
			get
			{
				return (LastCapacityCode)this.tagDictionary[Tag.LastCapacity];
			}
			set
			{
				this.tagDictionary[Tag.LastCapacity] = value;
			}
		}
		/// <summary>
		/// Gets or sets the LastPx.
		/// </summary>
		public Decimal LastPx
		{
			get
			{
				return (Decimal)this.tagDictionary[Tag.LastPx];
			}
			set
			{
				this.tagDictionary[Tag.LastPx] = value;
			}
		}

		/// <summary>
		/// Gets or sets the LastShares.
		/// </summary>
		public Decimal LastShares
		{
			get
			{
				return (Decimal)this.tagDictionary[Tag.LastShares];
			}
			set
			{
				this.tagDictionary[Tag.LastShares] = value;
			}
		}

		/// <summary>
		/// Gets or sets the LeavesQty.
		/// </summary>
		public Decimal LeavesQty
		{
			get
			{
				return (Decimal)this.tagDictionary[Tag.LeavesQty];
			}
			set
			{
				this.tagDictionary[Tag.LeavesQty] = value;
			}
		}

		/// <summary>
		/// Gets or sets the MaturityMonthYear.
		/// </summary>
		public DateTime MaturityMonthYear
		{
			get
			{
				return (DateTime)this.tagDictionary[Tag.MaturityMonthYear];
			}
			set
			{
				this.tagDictionary[Tag.MaturityMonthYear] = value;
			}
		}

		/// <summary>
		/// Gets or sets the MaxFloor.
		/// </summary>
		public Decimal MaxFloor
		{
			get
			{
				return (Decimal)this.tagDictionary[Tag.MaxFloor];
			}
			set
			{
				this.tagDictionary[Tag.MaxFloor] = value;
			}
		}

		/// <summary>
		/// Gets or sets the MinQty.
		/// </summary>
		public Decimal MinQty
		{
			get
			{
				return (Decimal)this.tagDictionary[Tag.MinQty];
			}
			set
			{
				this.tagDictionary[Tag.MinQty] = value;
			}
		}
		/// <summary>
		/// Gets or sets the MsgSeqNum.
		/// </summary>
		public Int32 MsgSeqNum
		{
			get
			{
				return (Int32)this.tagDictionary[Tag.MsgSeqNum];
			}
			set
			{
				this.tagDictionary[Tag.MsgSeqNum] = value;
			}
		}

		/// <summary>
		/// Gets or sets the MsgType.
		/// </summary>
		public MsgType MsgType
		{
			get
			{
				return (MsgType)this.tagDictionary[Tag.MsgType];
			}
			set
			{
				this.tagDictionary[Tag.MsgType] = value;
			}
		}

		/// <summary>
		/// Gets or sets the NewSeqNo.
		/// </summary>
		public Int32 NewSeqNo
		{
			get
			{
				return (Int32)this.tagDictionary[Tag.NewSeqNo];
			}
			set
			{
				this.tagDictionary[Tag.NewSeqNo] = value;
			}
		}

		/// <summary>
		/// Gets or sets the NextExpectedMsgSeqNum.
		/// </summary>
		public Int32 NextExpectedMsgSeqNum
		{
			get
			{
				return (Int32)this.tagDictionary[Tag.NextExpectedMsgSeqNum];
			}
			set
			{
				this.tagDictionary[Tag.NextExpectedMsgSeqNum] = value;
			}
		}

		/// <summary>
		/// Gets or sets the OnBehalfOfCompID.
		/// </summary>
		public String OnBehalfOfCompID
		{
			get
			{
				return this.tagDictionary[Tag.OnBehalfOfCompID] as String;
			}
			set
			{
				this.tagDictionary[Tag.OnBehalfOfCompID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the OnBehalfOfSubID.
		/// </summary>
		public String OnBehalfOfSubID
		{
			get
			{
				return this.tagDictionary[Tag.OnBehalfOfSubID] as String;
			}
			set
			{
				this.tagDictionary[Tag.OnBehalfOfSubID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the OrderID.
		/// </summary>
		public String OrderID
		{
			get
			{
				return this.tagDictionary[Tag.OrderID] as String;
			}
			set
			{
				this.tagDictionary[Tag.OrderID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the OrderQty.
		/// </summary>
		public Decimal OrderQty
		{
			get
			{
				return (Decimal)this.tagDictionary[Tag.OrderQty];
			}
			set
			{
				this.tagDictionary[Tag.OrderQty] = value;
			}
		}

		/// <summary>
		/// Gets or sets the OrdRejReason.
		/// </summary>
		public OrdRejReason OrdRejReason
		{
			get
			{
				return (OrdRejReason)this.tagDictionary[Tag.OrdRejReason];
			}
			set
			{
				this.tagDictionary[Tag.OrdRejReason] = value;
			}
		}

		/// <summary>
		/// Gets or sets the OrdStatus.
		/// </summary>
		public OrderStatusCode OrderStatusCode
		{
			get
			{
				return (OrderStatusCode)this.tagDictionary[Tag.OrdStatus];
			}
			set
			{
				this.tagDictionary[Tag.OrdStatus] = value;
			}
		}

		/// <summary>
		/// Gets or sets the OrdType.
		/// </summary>
		public OrderTypeCode OrdType
		{
			get
			{
				return (OrderTypeCode)this.tagDictionary[Tag.OrdType];
			}
			set
			{
				this.tagDictionary[Tag.OrdType] = value;
			}
		}

		/// <summary>
		/// Gets or sets the OrigClOrdID.
		/// </summary>
		public String OrigClOrdID
		{
			get
			{
				return this.tagDictionary[Tag.OrigClOrdID] as String;
			}
			set
			{
				this.tagDictionary[Tag.OrigClOrdID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the OrigSendingTime.
		/// </summary>
		public DateTime OrigSendingTime
		{
			get
			{
				return (DateTime)this.tagDictionary[Tag.OrigSendingTime];
			}
			set
			{
				this.tagDictionary[Tag.OrigSendingTime] = value;
			}
		}

		/// <summary>
		/// Gets or sets the Password.
		/// </summary>
		public String Password
		{
			get
			{
				return this.tagDictionary[Tag.Password] as String;
			}
			set
			{
				this.tagDictionary[Tag.Password] = value;
			}
		}

		/// <summary>
		/// Gets or sets the PegDifference.
		/// </summary>
		public Decimal PegDifference
		{
			get
			{
				return (Decimal)this.tagDictionary[Tag.PegDifference];
			}
			set
			{
				this.tagDictionary[Tag.PegDifference] = value;
			}
		}

		/// <summary>
		/// Gets or sets the PossDupFlag
		/// </summary>
		public Boolean PossDupFlag
		{
			get
			{
				object possDupObject = this.tagDictionary[Tag.PossDupFlag];
				return possDupObject == null ? false : (Boolean)possDupObject;
			}
			set
			{
				this.tagDictionary[Tag.PossDupFlag] = value;
			}
		}

		/// <summary>
		/// Gets or sets the Price.
		/// </summary>
		public Decimal Price
		{
			get
			{
				return (Decimal)this.tagDictionary[Tag.Price];
			}
			set
			{
				this.tagDictionary[Tag.Price] = value;
			}
		}

		/// <summary>
		/// Gets or sets the RawData.
		/// </summary>
		public String RawData
		{
			get
			{
				return this.tagDictionary[Tag.RawData] as String;
			}
			set
			{
				this.tagDictionary[Tag.RawData] = value;
			}
		}

		/// <summary>
		/// Gets or sets the RefMsgType.
		/// </summary>
		public MsgType RefMsgType
		{
			get
			{
				return (MsgType)this.tagDictionary[Tag.RefMsgType];
			}
			set
			{
				this.tagDictionary[Tag.RefMsgType] = value;
			}
		}

		/// <summary>
		/// Gets or sets the ResetSeqNumFlag.
		/// </summary>
		public Boolean ResetSeqNumFlag
		{
			get
			{
				object resetSeqNumObject = this.tagDictionary[Tag.ResetSeqNumFlag];
				return resetSeqNumObject == null ? false : (Boolean)resetSeqNumObject;
			}
			set
			{
				this.tagDictionary[Tag.ResetSeqNumFlag] = value;
			}
		}

		/// <summary>
		/// Gets or sets the RoutingGroup.
		/// </summary>
		public List<RoutingItem> RoutingGroup
		{
			get
			{
				return this.tagDictionary[Tag.RoutingGroup] as List<RoutingItem>;
			}
			set
			{
				this.tagDictionary[Tag.RoutingGroup] = value;
			}
		}

		/// <summary>
		/// Gets or sets the Rule80A.
		/// </summary>
		public String Rule80A
		{
			get
			{
				return this.tagDictionary[Tag.Rule80A] as String;
			}
			set
			{
				this.tagDictionary[Tag.Rule80A] = value;
			}
		}

		/// <summary>
		/// <summary>
		/// Gets or sets the SecurityID.
		/// </summary>
		public String SecurityID
		{
			get
			{
				return this.tagDictionary[Tag.SecurityID] as String;
			}
			set
			{
				this.tagDictionary[Tag.SecurityID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the SecurityType.
		/// </summary>
		public SecurityType SecurityType
		{
			get
			{
				return (SecurityType)this.tagDictionary[Tag.SecurityType];
			}
			set
			{
				this.tagDictionary[Tag.SecurityType] = value;
			}
		}

		/// <summary>
		/// Gets or sets the SenderCompID.
		/// </summary>
		public String SenderCompID
		{
			get
			{
				return this.tagDictionary[Tag.SenderCompID] as String;
			}
			set
			{
				this.tagDictionary[Tag.SenderCompID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the SenderSubID.
		/// </summary>
		public String SenderSubID
		{
			get
			{
				return this.tagDictionary[Tag.SenderSubID] as String;
			}
			set
			{
				this.tagDictionary[Tag.SenderSubID] = value;
			}
		}

		/// Gets or sets the SendingTime.
		/// </summary>
		public DateTime SendingTime
		{
			get
			{
				return (DateTime)this.tagDictionary[Tag.SendingTime];
			}
			set
			{
				this.tagDictionary[Tag.SendingTime] = value;
			}
		}

		/// <summary>
		/// Gets or sets the SessionRejectReason.
		/// </summary>
		public SessionRejectReason SessionRejectReason
		{
			get
			{
				return (SessionRejectReason)this.tagDictionary[Tag.SessionRejectReason];
			}
			set
			{
				this.tagDictionary[Tag.SessionRejectReason] = value;
			}
		}

		/// <summary>
		/// Gets or sets the SideCode.
		/// </summary>
		public SideCode SideCode
		{
			get
			{
				return (SideCode)this.tagDictionary[Tag.Side];
			}
			set
			{
				this.tagDictionary[Tag.Side] = value;
			}
		}

		/// <summary>
		/// Gets or sets the StopPx.
		/// </summary>
		public Decimal StopPx
		{
			get
			{
				return (Decimal)this.tagDictionary[Tag.StopPx];
			}
			set
			{
				this.tagDictionary[Tag.StopPx] = value;
			}
		}

		/// <summary>
		/// Gets or sets the StrikePrice.
		/// </summary>
		public Decimal StrikePrice
		{
			get
			{
				return (Decimal)this.tagDictionary[Tag.StrikePrice];
			}
			set
			{
				this.tagDictionary[Tag.StrikePrice] = value;
			}
		}

		/// <summary>
		/// Gets or sets the Symbol.
		/// </summary>
		public String Symbol
		{
			get
			{
				return this.tagDictionary[Tag.Symbol] as String;
			}
			set
			{
				this.tagDictionary[Tag.Symbol] = value;
			}
		}

		/// <summary>
		/// Gets or sets the SymbolSfx.
		/// </summary>
		public String SymbolSfx
		{
			get
			{
				return this.tagDictionary[Tag.SymbolSfx] as String;
			}
			set
			{
				this.tagDictionary[Tag.SymbolSfx] = value;
			}
		}

		/// <summary>
		/// Gets or sets the TargetCompID.
		/// </summary>
		public String TargetCompID
		{
			get
			{
				return this.tagDictionary[Tag.TargetCompID] as String;
			}
			set
			{
				this.tagDictionary[Tag.TargetCompID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the TargetLocationID.
		/// </summary>
		public String TargetLocationID
		{
			get
			{
				return this.tagDictionary[Tag.TargetLocationID] as String;
			}
			set
			{
				this.tagDictionary[Tag.TargetLocationID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the TargetSubID.
		/// </summary>
		public String TargetSubID
		{
			get
			{
				return this.tagDictionary[Tag.TargetSubID] as String;
			}
			set
			{
				this.tagDictionary[Tag.TargetSubID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the TestReqID.
		/// </summary>
		public String TestReqID
		{
			get
			{
				return this.tagDictionary[Tag.TestReqID] as String;
			}
			set
			{
				this.tagDictionary[Tag.TestReqID] = value;
			}
		}

		/// <summary>
		/// Gets or sets the Text.
		/// </summary>
		public String Text
		{
			get
			{
				return this.tagDictionary[Tag.Text] as String;
			}
			set
			{
				this.tagDictionary[Tag.Text] = value;
			}
		}

		/// <summary>
		/// Gets or sets the TimeInForceCode.
		/// </summary>
		public TimeInForceCode TimeInForceCode
		{
			get
			{
				return (TimeInForceCode)this.tagDictionary[Tag.TimeInForce];
			}
			set
			{
				this.tagDictionary[Tag.TimeInForce] = value;
			}
		}

		/// <summary>
		/// Gets or sets the TransactTime.
		/// </summary>
		public DateTime TransactTime
		{
			get
			{
				return (DateTime)this.tagDictionary[Tag.TransactTime];
			}
			set
			{
				this.tagDictionary[Tag.TransactTime] = value;
			}
		}

		/// <summary>
		/// Gets or sets the Username.
		/// </summary>
		public String Username
		{
			get
			{
				return this.tagDictionary[Tag.Username] as String;
			}
			set
			{
				this.tagDictionary[Tag.Username] = value;
			}
		}

		/// <summary>
		/// Gets or sets the ValidUntilTime.
		/// </summary>
		public DateTime ValidUntilTime
		{
			get
			{
				return (DateTime)this.tagDictionary[Tag.ValidUntilTime];
			}
			set
			{
				this.tagDictionary[Tag.ValidUntilTime] = value;
			}
		}

	}

}
