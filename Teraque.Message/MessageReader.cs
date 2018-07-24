namespace Teraque
{

	using System;
	using System.IO;
	using Teraque;

	/// <summary>
	/// Summary description for MessageReader.
	/// </summary>
	public class MessageReader : BinaryReader
	{

		public MessageReader(FileStream fileStream) : base(fileStream) {}

		public Message ReadMessage()
		{

			//Message message = new Message();
			//int tagCount = ReadInt32();
			//for (int tagIndex = 0; tagIndex < tagCount; tagIndex++)
			//    message.Add(ReadField());

			return new Message();

		}

		public Field ReadField()
		{

			Tag tag;
			try
			{
				tag =(Tag)ReadInt32();
			}
			catch (Exception exception)
			{
				throw new Exception(string.Format("Error reading Tag from message: {0}", exception.ToString()));
			}

			switch (tag)
			{

			case Tag.GapFillFlag:
			case Tag.PossDupFlag:
			case Tag.ResetSeqNumFlag:

				// Boolean Values
				return new Field(tag, ReadBoolean());
			
			case Tag.BeginSeqNo:
			case Tag.BodyLength:
			case Tag.CheckSum:
			case Tag.EncodedTextLen:
			case Tag.EndSeqNo:
			case Tag.HeartBtInt:
			case Tag.InternalRecordId:
			case Tag.MsgSeqNum:
			case Tag.NewSeqNo:
			case Tag.NextExpectedMsgSeqNum:
			case Tag.RawDataLength:
			case Tag.RefSeqNum:

				// Int32 Values
				return new Field(tag, ReadInt32());

			case Tag.AvgPx:
			case Tag.Commission:
			case Tag.CmsCxlQty:
			case Tag.CmsLeavesQty:
			case Tag.CumQty:
			case Tag.DiscretionOffset:
			case Tag.LastPx:
			case Tag.LastShares:
			case Tag.LeavesQty:
			case Tag.MaxFloor:
			case Tag.MinQty:
			case Tag.OrderQty:
			case Tag.PegDifference:
			case Tag.Price:
			case Tag.StopPx:
			case Tag.StrikePrice:

				// Decimal Values
				return new Field(tag, ReadDecimal());

			case Tag.ExpireTime:
			case Tag.OrigSendingTime:
			case Tag.SendingTime:
			case Tag.TransactTime:

				// DateTime Values
				int year = ReadInt32();
				int month = ReadInt32();
				int day = ReadInt32();
				int hour = ReadInt32();
				int minute = ReadInt32();
				int second = ReadInt32();
				return new Field(tag, new DateTime(year, month, day, hour, minute, second));

			case Tag.MaturityMonthYear:

				string monthYear = ReadString();
				int expYear = Int32.Parse(monthYear.Substring(0, 4));
				int expMonth = Int32.Parse(monthYear.Substring(4, 2));
				return new Field(tag, new DateTime(expYear, expMonth, 1));


			// Enum Data Types
			case Tag.BusinessRejectReason: return new Field(tag, (BusinessRejectReasonCode)ReadInt32());
			case Tag.CxlRejReason: return new Field(tag, (CancelRejectReasonCode)ReadInt32());
			case Tag.CxlRejResponseTo: return new Field(tag, (CancelRejectResponseCode)ReadInt32());
			case Tag.CommType: return new Field(tag, (CommissionType)ReadInt32());
			case Tag.DiscretionInst: return new Field(tag, (DiscretionInstructionCode)ReadInt32());
			case Tag.DKReason: return new Field(tag, (DontKnowReasonCode)ReadInt32());
			case Tag.EncryptMethod: return new Field(tag, (EncryptMethodCode)ReadInt32());
			case Tag.ExecTransType: return new Field(tag, (ExecutionTransactionTypeCode)ReadInt32());
			case Tag.ExecType: return new Field(tag, (ExecutionTypeCode)ReadInt32());
			case Tag.HandlInst: return new Field(tag, (HandleInstructionCode)ReadInt32());
			case Tag.LastCapacity: return new Field(tag, (LastCapacityCode)ReadInt32());
			case Tag.MsgType: return new Field(tag, (MsgType)ReadInt32());
			case Tag.RefMsgType: return new Field(tag, (MsgType)ReadInt32());
			case Tag.OrdRejReason: return new Field(tag, (OrdRejReason)ReadInt32());
			case Tag.OrdStatus: return new Field(tag, (OrderStatusCode)ReadInt32());
			case Tag.OrdType: return new Field(tag, (OrderTypeCode)ReadInt32());
			case Tag.SecurityType: return new Field(tag, (SecurityType)ReadInt32());
			case Tag.SessionRejectReason: return new Field(tag, (SessionRejectReason)ReadInt32());
			case Tag.Side: return new Field(tag, (SideCode)ReadInt32());
			case Tag.TimeInForce: return new Field(tag, (TimeInForceCode)ReadInt32());

			default:
				// Read tags that are not specified above as string values
				return new Field(tag, ReadString());

			}

		}

	}

}
