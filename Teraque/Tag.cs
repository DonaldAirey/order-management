namespace Teraque
{

	using System;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Fix Tags
	/// </summary>
	/// <remarks>
	/// Includes all tags from FIX 4.4 specification plus some user defined tags.
	/// </remarks>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum Tag
	{

		/// <summary>
		/// None
		/// </summary>
		None,

		/// <summary>
		/// Account
		/// </summary>
		Account,

		/// <summary>
		/// AdvId
		/// </summary>
		AdvId,

		/// <summary>
		/// AdvRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		AdvRefID,

		/// <summary>
		/// AdvSide
		/// </summary>
		AdvSide,

		/// <summary>
		/// AdvTransType
		/// </summary>
		AdvTransType,

		/// <summary>
		/// AvgPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Avg")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		AvgPx,

		/// <summary>
		/// BeginSeqNo
		/// </summary>
		BeginSeqNo,

		/// <summary>
		/// BeginString
		/// </summary>
		BeginString,

		/// <summary>
		/// BodyLength
		/// </summary>
		BodyLength,

		/// <summary>
		/// CheckSum
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "CheckSum")]
		CheckSum,

		/// <summary>
		/// ClOrdID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ord")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Cl")]
		ClOrdID,

		/// <summary>
		/// Commission
		/// </summary>
		Commission,

		/// <summary>
		/// CommType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Comm")]
		CommType,

		/// <summary>
		/// CumQty
		/// </summary>
		CumQty,

		/// <summary>
		/// Currency
		/// </summary>
		Currency,

		/// <summary>
		/// EndSeqNo
		/// </summary>
		EndSeqNo,

		/// <summary>
		/// ExecID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		ExecID,

		/// <summary>
		/// ExecInst
		/// </summary>
		ExecInst,

		/// <summary>
		/// ExecRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		ExecRefID,

		/// <summary>
		/// ExecTransType
		/// </summary>
		ExecTransType,

		/// <summary>
		/// HandlInst
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Handl")]
		HandlInst,

		/// <summary>
		/// IDSource
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		IDSource,

		/// <summary>
		/// Ioiid
		/// </summary>
		IoiId,

		/// <summary>
		/// IoiOthSvc
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Oth")]
		IoiOthSvc,

		/// <summary>
		/// IoiQltyInd
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qlty")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ind")]
		IoiQltyInd,

		/// <summary>
		/// IoiRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		IoiRefID,

		/// <summary>
		/// IoiShares
		/// </summary>
		IoiShares,

		/// <summary>
		/// IoiTransType
		/// </summary>
		IoiTransType,

		/// <summary>
		/// LastCapacity
		/// </summary>
		LastCapacity,

		/// <summary>
		/// LastMkt
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mkt")]
		LastMkt,

		/// <summary>
		/// LastPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		LastPx,

		/// <summary>
		/// LastShares
		/// </summary>
		LastShares,

		/// <summary>
		/// LinesOfText
		/// </summary>
		LinesOfText,

		/// <summary>
		/// MsgSeqNum
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
		MsgSeqNum,

		/// <summary>
		/// MsgType
		/// </summary>
		MsgType,

		/// <summary>
		/// NewSeqNo
		/// </summary>
		NewSeqNo,

		/// <summary>
		/// OrderID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		OrderID,

		/// <summary>
		/// OrderQty
		/// </summary>
		OrderQty,

		/// <summary>
		/// OrdStatus
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ord")]
		OrdStatus,

		/// <summary>
		/// OrdType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ord")]
		OrdType,

		/// <summary>
		/// OrigClOrdID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Orig")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ord")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Cl")]
		OrigClOrdID,

		/// <summary>
		/// OrigTime
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Orig")]
		OrigTime,

		/// <summary>
		/// PossDupFlag
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Poss")]
		PossDupFlag,

		/// <summary>
		/// Price
		/// </summary>
		Price,

		/// <summary>
		/// RefSeqNum
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
		RefSeqNum,

		/// <summary>
		/// RelatdSym
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Relatd")]
		RelatdSym,

		/// <summary>
		/// Rule80A
		/// </summary>
		Rule80A,

		/// <summary>
		/// SecurityID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SecurityID,

		/// <summary>
		/// SenderCompID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SenderCompID,

		/// <summary>
		/// SenderSubID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SenderSubID,

		/// <summary>
		/// SendingDate
		/// </summary>
		SendingDate,

		/// <summary>
		/// SendingTime
		/// </summary>
		SendingTime,

		/// <summary>
		/// Shares
		/// </summary>
		Shares,

		/// <summary>
		/// Side
		/// </summary>
		Side,

		/// <summary>
		/// Symbol
		/// </summary>
		Symbol,

		/// <summary>
		/// TargetCompID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		TargetCompID,

		/// <summary>
		/// TargetSubID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		TargetSubID,

		/// <summary>
		/// Text
		/// </summary>
		Text,

		/// <summary>
		/// TimeInForce
		/// </summary>
		TimeInForce,

		/// <summary>
		/// TransactTime
		/// </summary>
		TransactTime,

		/// <summary>
		/// Urgency
		/// </summary>
		Urgency,

		/// <summary>
		/// ValidUntilTime
		/// </summary>
		ValidUntilTime,

		/// <summary>
		/// SettlmntTyp
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Typ")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settlmnt")]
		SettlmntTyp,

		/// <summary>
		/// FutSettDate
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Fut")]
		FutSettDate,

		/// <summary>
		/// SymbolSfx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sfx")]
		SymbolSfx,

		/// <summary>
		/// ListID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		ListID,

		/// <summary>
		/// ListSeqNo
		/// </summary>
		ListSeqNo,

		/// <summary>
		/// TotNoOrders
		/// </summary>
		TotNoOrders,

		/// <summary>
		/// ListExecInst
		/// </summary>
		ListExecInst,

		/// <summary>
		/// AllocID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		AllocID,

		/// <summary>
		/// AllocTransType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocTransType,

		/// <summary>
		/// RefAllocID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		RefAllocID,

		/// <summary>
		/// NoOrders
		/// </summary>
		NoOrders,

		/// <summary>
		/// AvgPrxPrecision
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Prx")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Avg")]
		AvgPrxPrecision,

		/// <summary>
		/// TradeDate
		/// </summary>
		TradeDate,

		/// <summary>
		/// ExecBroker
		/// </summary>
		ExecBroker,

		/// <summary>
		/// OpenClose
		/// </summary>
		OpenClose,

		/// <summary>
		/// NoAllocs
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Allocs")]
		NoAllocs,

		/// <summary>
		/// AllocAccount
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocAccount,

		/// <summary>
		/// AllocShares
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocShares,

		/// <summary>
		/// ProcessCode
		/// </summary>
		ProcessCode,

		/// <summary>
		/// NoRpts
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rpts")]
		NoRpts,

		/// <summary>
		/// RptSeq
		/// </summary>
		RptSeq,

		/// <summary>
		/// CxlQty
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cxl")]
		CxlQty,

		/// <summary>
		/// NoDlvyInst
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dlvy")]
		NoDlvyInst,

		/// <summary>
		/// DlvyInst
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dlvy")]
		DlvyInst,

		/// <summary>
		/// AllocStatus
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocStatus,

		/// <summary>
		/// AllocRejCode
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rej")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocRejCode,

		/// <summary>
		/// Signature
		/// </summary>
		Signature,

		/// <summary>
		/// SecureDataLen
		/// </summary>
		SecureDataLen,

		/// <summary>
		/// SecureData
		/// </summary>
		SecureData,

		/// <summary>
		/// BrokerOfCredit
		/// </summary>
		BrokerOfCredit,

		/// <summary>
		/// SignatureLength
		/// </summary>
		SignatureLength,

		/// <summary>
		/// EmailType
		/// </summary>
		EmailType,

		/// <summary>
		/// RawDataLength
		/// </summary>
		RawDataLength,

		/// <summary>
		/// RawData
		/// </summary>
		RawData,

		/// <summary>
		/// PossResend
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Poss")]
		PossResend,

		/// <summary>
		/// EncryptMethod
		/// </summary>
		EncryptMethod,

		/// <summary>
		/// StopPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		StopPx,

		/// <summary>
		/// ExDestination
		/// </summary>
		ExDestination,

		/// <summary>
		/// CxlRejReason
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rej")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cxl")]
		CxlRejReason,

		/// <summary>
		/// OrdRejReason
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rej")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ord")]
		OrdRejReason,

		/// <summary>
		/// IoiQualifier
		/// </summary>
		IoiQualifier,

		/// <summary>
		/// WaveNo
		/// </summary>
		WaveNo,

		/// <summary>
		/// Issuer
		/// </summary>
		Issuer,

		/// <summary>
		/// SecurityDesc
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Desc")]
		SecurityDesc,

		/// <summary>
		/// HeartBtInt
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Bt")]
		HeartBtInt,

		/// <summary>
		/// ClientID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		ClientID,

		/// <summary>
		/// MinQty
		/// </summary>
		MinQty,

		/// <summary>
		/// MaxFloor
		/// </summary>
		MaxFloor,

		/// <summary>
		/// TestReqID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		TestReqID,

		/// <summary>
		/// ReportToExch
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Exch")]
		ReportToExch,

		/// <summary>
		/// LocateReqd
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Reqd")]
		LocateReqd,

		/// <summary>
		/// OnBehalfOfCompID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		OnBehalfOfCompID,

		/// <summary>
		/// OnBehalfOfSubID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		OnBehalfOfSubID,

		/// <summary>
		/// QuoteID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		QuoteID,

		/// <summary>
		/// NetMoney
		/// </summary>
		NetMoney,

		/// <summary>
		/// SettlCurrAmt
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Curr")]
		SettlCurrAmt,

		/// <summary>
		/// SettlCurrency
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		SettlCurrency,

		/// <summary>
		/// ForexReq
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Forex")]
		ForexReq,

		/// <summary>
		/// OrigSendingTime
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Orig")]
		OrigSendingTime,

		/// <summary>
		/// GapFillFlag
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag")]
		GapFillFlag,

		/// <summary>
		/// NoExecs
		/// </summary>
		NoExecs,

		/// <summary>
		/// CxlType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cxl")]
		CxlType,

		/// <summary>
		/// ExpireTime
		/// </summary>
		ExpireTime,

		/// <summary>
		/// DKReason
		/// </summary>
		DKReason,

		/// <summary>
		/// DeliverToCompID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		DeliverToCompID,

		/// <summary>
		/// DeliverToSubID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		DeliverToSubID,

		/// <summary>
		/// IoiNaturalFlag
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag")]
		IoiNaturalFlag,

		/// <summary>
		/// QuoteReqID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		QuoteReqID,

		/// <summary>
		/// BidPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		BidPx,

		/// <summary>
		/// OfferPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		OfferPx,

		/// <summary>
		/// BidSize
		/// </summary>
		BidSize,

		/// <summary>
		/// OfferSize
		/// </summary>
		OfferSize,

		/// <summary>
		/// NoMiscFees
		/// </summary>
		NoMiscFees,

		/// <summary>
		/// MiscFeeAmt
		/// </summary>
		MiscFeeAmt,

		/// <summary>
		/// MiscFeeCurr
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Curr")]
		MiscFeeCurr,

		/// <summary>
		/// MiscFeeType
		/// </summary>
		MiscFeeType,

		/// <summary>
		/// PrevClosePx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Prev")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		PrevClosePx,

		/// <summary>
		/// ResetSeqNumFlag
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
		ResetSeqNumFlag,

		/// <summary>
		/// SenderLocationID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SenderLocationID,

		/// <summary>
		/// TargetLocationID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		TargetLocationID,

		/// <summary>
		/// OnBehalfOfLocationID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		OnBehalfOfLocationID,

		/// <summary>
		/// DeliverToLocationID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		DeliverToLocationID,

		/// <summary>
		/// NoRelatedSym
		/// </summary>
		NoRelatedSym,

		/// <summary>
		/// Subject
		/// </summary>
		Subject,

		/// <summary>
		/// Headline
		/// </summary>
		Headline,

		/// <summary>
		/// URLLink
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "URL")]
		URLLink,

		/// <summary>
		/// ExecType
		/// </summary>
		ExecType,

		/// <summary>
		/// LeavesQty
		/// </summary>
		LeavesQty,

		/// <summary>
		/// CashOrderQty
		/// </summary>
		CashOrderQty,

		/// <summary>
		/// AllocAvgPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Avg")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		AllocAvgPx,

		/// <summary>
		/// AllocNetMoney
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocNetMoney,

		/// <summary>
		/// SettlCurrFxRate
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Curr")]
		SettlCurrFxRate,

		/// <summary>
		/// SettlCurrFxRateCalc
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Curr")]
		SettlCurrFxRateCalc,

		/// <summary>
		/// NumDaysInterest
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
		NumDaysInterest,

		/// <summary>
		/// AccruedInterestRate
		/// </summary>
		AccruedInterestRate,

		/// <summary>
		/// AccruedInterestAmt
		/// </summary>
		AccruedInterestAmt,

		/// <summary>
		/// SettlInstMode
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		SettlInstMode,

		/// <summary>
		/// AllocText
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocText,

		/// <summary>
		/// SettlInstID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SettlInstID,

		/// <summary>
		/// SettlInstTransType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		SettlInstTransType,

		/// <summary>
		/// EmailThreadID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		EmailThreadID,

		/// <summary>
		/// SettlInstSource
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		SettlInstSource,

		/// <summary>
		/// SettlLocation
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		SettlLocation,

		/// <summary>
		/// SecurityType
		/// </summary>
		SecurityType,

		/// <summary>
		/// EffectiveTime
		/// </summary>
		EffectiveTime,

		/// <summary>
		/// StandInstDbType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db")]
		StandInstDbType,

		/// <summary>
		/// StandInstDbName
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db")]
		StandInstDbName,

		/// <summary>
		/// StandInstDbID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db")]
		StandInstDbID,

		/// <summary>
		/// SettlDeliveryType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		SettlDeliveryType,

		/// <summary>
		/// SettlDepositoryCode
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		SettlDepositoryCode,

		/// <summary>
		/// SettlBrkrCode
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Brkr")]
		SettlBrkrCode,

		/// <summary>
		/// SettlInstCode
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		SettlInstCode,

		/// <summary>
		/// SecuritySettlAgentName
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		SecuritySettlAgentName,

		/// <summary>
		/// SecuritySettlAgentCode
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		SecuritySettlAgentCode,

		/// <summary>
		/// SecuritySettlAgentAcctNum
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
		SecuritySettlAgentAcctNum,

		/// <summary>
		/// SecuritySettlAgentAcctName
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		SecuritySettlAgentAcctName,

		/// <summary>
		/// SecuritySettlAgentContactName
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		SecuritySettlAgentContactName,

		/// <summary>
		/// SecuritySettlAgentContactPhone
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		SecuritySettlAgentContactPhone,

		/// <summary>
		/// CashSettlAgentName
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		CashSettlAgentName,

		/// <summary>
		/// CashSettlAgentCode
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		CashSettlAgentCode,

		/// <summary>
		/// CashSettlAgentAcctNum
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
		CashSettlAgentAcctNum,

		/// <summary>
		/// CashSettlAgentAcctName
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		CashSettlAgentAcctName,

		/// <summary>
		/// CashSettlAgentContactName
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		CashSettlAgentContactName,

		/// <summary>
		/// CashSettlAgentContactPhone
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		CashSettlAgentContactPhone,

		/// <summary>
		/// BidSpotRate
		/// </summary>
		BidSpotRate,

		/// <summary>
		/// BidForwardPoints
		/// </summary>
		BidForwardPoints,

		/// <summary>
		/// OfferSpotRate
		/// </summary>
		OfferSpotRate,

		/// <summary>
		/// OfferForwardPoints
		/// </summary>
		OfferForwardPoints,

		/// <summary>
		/// OrderQty2
		/// </summary>
		OrderQty2,

		/// <summary>
		/// FutSettDate2
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Fut")]
		FutSettDate2,

		/// <summary>
		/// LastSpotRate
		/// </summary>
		LastSpotRate,

		/// <summary>
		/// LastForwardPoints
		/// </summary>
		LastForwardPoints,

		/// <summary>
		/// AllocLinkID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		AllocLinkID,

		/// <summary>
		/// AllocLinkType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocLinkType,

		/// <summary>
		/// SecondaryOrderID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SecondaryOrderID,

		/// <summary>
		/// NoIoiQualifiers
		/// </summary>
		NoIoiQualifiers,

		/// <summary>
		/// MaturityMonthYear
		/// </summary>
		MaturityMonthYear,

		/// <summary>
		/// PutOrCall
		/// </summary>
		PutOrCall,

		/// <summary>
		/// StrikePrice
		/// </summary>
		StrikePrice,

		/// <summary>
		/// CoveredOrUncovered
		/// </summary>
		CoveredOrUncovered,

		/// <summary>
		/// CustomerOrFirm
		/// </summary>
		CustomerOrFirm,

		/// <summary>
		/// MaturityDay
		/// </summary>
		MaturityDay,

		/// <summary>
		/// OptAttribute
		/// </summary>
		OptAttribute,

		/// <summary>
		/// SecurityExchange
		/// </summary>
		SecurityExchange,

		/// <summary>
		/// NotifyBrokerOfCredit
		/// </summary>
		NotifyBrokerOfCredit,

		/// <summary>
		/// AllocHandlInst
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Handl")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocHandlInst,

		/// <summary>
		/// MaxShow
		/// </summary>
		MaxShow,

		/// <summary>
		/// PegDifference
		/// </summary>
		PegDifference,

		/// <summary>
		/// XmlDataLen
		/// </summary>
		XmlDataLen,

		/// <summary>
		/// XmlData
		/// </summary>
		XmlData,

		/// <summary>
		/// SettlInstRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SettlInstRefID,

		/// <summary>
		/// NoRoutingIDs
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IDs")]
		NoRoutingIDs,

		/// <summary>
		/// RoutingType
		/// </summary>
		RoutingType,

		/// <summary>
		/// RoutingID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		RoutingID,

		/// <summary>
		/// SpreadToBenchmark
		/// </summary>
		SpreadToBenchmark,

		/// <summary>
		/// Benchmark
		/// </summary>
		Benchmark,

		/// <summary>
		/// BenchmarkCurveCurrency
		/// </summary>
		BenchmarkCurveCurrency,

		/// <summary>
		/// BenchmarkCurveName
		/// </summary>
		BenchmarkCurveName,

		/// <summary>
		/// BenchmarkCurvePoint
		/// </summary>
		BenchmarkCurvePoint,

		/// <summary>
		/// CouponRate
		/// </summary>
		CouponRate,

		/// <summary>
		/// CouponPaymentDate
		/// </summary>
		CouponPaymentDate,

		/// <summary>
		/// IssueDate
		/// </summary>
		IssueDate,

		/// <summary>
		/// RepurchaseTerm
		/// </summary>
		RepurchaseTerm,

		/// <summary>
		/// RepurchaseRate
		/// </summary>
		RepurchaseRate,

		/// <summary>
		/// Factor
		/// </summary>
		Factor,

		/// <summary>
		/// TradeOriginationDate
		/// </summary>
		TradeOriginationDate,

		/// <summary>
		/// ExDate
		/// </summary>
		ExDate,

		/// <summary>
		/// ContractMultiplier
		/// </summary>
		ContractMultiplier,

		/// <summary>
		/// NoStipulations
		/// </summary>
		NoStipulations,

		/// <summary>
		/// StipulationType
		/// </summary>
		StipulationType,

		/// <summary>
		/// StipulationValue
		/// </summary>
		StipulationValue,

		/// <summary>
		/// YieldType
		/// </summary>
		YieldType,

		/// <summary>
		/// Yield
		/// </summary>
		Yield,

		/// <summary>
		/// TotalTakedown
		/// </summary>
		TotalTakedown,

		/// <summary>
		/// Concession
		/// </summary>
		Concession,

		/// <summary>
		/// RepoCollateralSecurityType
		/// </summary>
		RepoCollateralSecurityType,

		/// <summary>
		/// RedemptionDate
		/// </summary>
		RedemptionDate,

		/// <summary>
		/// UnderlyingCouponPaymentDate
		/// </summary>
		UnderlyingCouponPaymentDate,

		/// <summary>
		/// UnderlyingIssueDate
		/// </summary>
		UnderlyingIssueDate,

		/// <summary>
		/// UnderlyingRepoCollateralSecurityType
		/// </summary>
		UnderlyingRepoCollateralSecurityType,

		/// <summary>
		/// UnderlyingRepurchaseTerm
		/// </summary>
		UnderlyingRepurchaseTerm,

		/// <summary>
		/// UnderlyingRepurchaseRate
		/// </summary>
		UnderlyingRepurchaseRate,

		/// <summary>
		/// UnderlyingFactor
		/// </summary>
		UnderlyingFactor,

		/// <summary>
		/// UnderlyingRedemptionDate
		/// </summary>
		UnderlyingRedemptionDate,

		/// <summary>
		/// LegCouponPaymentDate
		/// </summary>
		LegCouponPaymentDate,

		/// <summary>
		/// LegIssueDate
		/// </summary>
		LegIssueDate,

		/// <summary>
		/// LegRepoCollateralSecurityType
		/// </summary>
		LegRepoCollateralSecurityType,

		/// <summary>
		/// LegRepurchaseTerm
		/// </summary>
		LegRepurchaseTerm,

		/// <summary>
		/// LegRepurchaseRate
		/// </summary>
		LegRepurchaseRate,

		/// <summary>
		/// LegFactor
		/// </summary>
		LegFactor,

		/// <summary>
		/// LegRedemptionDate
		/// </summary>
		LegRedemptionDate,

		/// <summary>
		/// CreditRating
		/// </summary>
		CreditRating,

		/// <summary>
		/// UnderlyingCreditRating
		/// </summary>
		UnderlyingCreditRating,

		/// <summary>
		/// LegCreditRating
		/// </summary>
		LegCreditRating,

		/// <summary>
		/// TradedFlatSwitch
		/// </summary>
		TradedFlatSwitch,

		/// <summary>
		/// BasisFeatureDate
		/// </summary>
		BasisFeatureDate,

		/// <summary>
		/// BasisFeaturePrice
		/// </summary>
		BasisFeaturePrice,

		/// <summary>
		/// MDReqID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		MDReqID,

		/// <summary>
		/// SubscriptionRequestType
		/// </summary>
		SubscriptionRequestType,

		/// <summary>
		/// MarketDepth
		/// </summary>
		MarketDepth,

		/// <summary>
		/// MDUpdateType
		/// </summary>
		MDUpdateType,

		/// <summary>
		/// AggregatedBook
		/// </summary>
		AggregatedBook,

		/// <summary>
		/// NoMDEntryTypes
		/// </summary>
		NoMDEntryTypes,

		/// <summary>
		/// NoMDEntries
		/// </summary>
		NoMDEntries,

		/// <summary>
		/// MDEntryType
		/// </summary>
		MDEntryType,

		/// <summary>
		/// MDEntryPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		MDEntryPx,

		/// <summary>
		/// MDEntrySize
		/// </summary>
		MDEntrySize,

		/// <summary>
		/// MDEntryDate
		/// </summary>
		MDEntryDate,

		/// <summary>
		/// MDEntryTime
		/// </summary>
		MDEntryTime,

		/// <summary>
		/// TickDirection
		/// </summary>
		TickDirection,

		/// <summary>
		/// MDMkt
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mkt")]
		MDMkt,

		/// <summary>
		/// QuoteCondition
		/// </summary>
		QuoteCondition,

		/// <summary>
		/// TradeCondition
		/// </summary>
		TradeCondition,

		/// <summary>
		/// MDEntryID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		MDEntryID,

		/// <summary>
		/// MDUpdateAction
		/// </summary>
		MDUpdateAction,

		/// <summary>
		/// MDEntryRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		MDEntryRefID,

		/// <summary>
		/// MDReqRejReason
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rej")]
		MDReqRejReason,

		/// <summary>
		/// MDEntryOriginator
		/// </summary>
		MDEntryOriginator,

		/// <summary>
		/// LocationID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		LocationID,

		/// <summary>
		/// DeskID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		DeskID,

		/// <summary>
		/// DeleteReason
		/// </summary>
		DeleteReason,

		/// <summary>
		/// OpenCloseSettlFlag
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		OpenCloseSettlFlag,

		/// <summary>
		/// SellerDays
		/// </summary>
		SellerDays,

		/// <summary>
		/// MDEntryBuyer
		/// </summary>
		MDEntryBuyer,

		/// <summary>
		/// MDEntrySeller
		/// </summary>
		MDEntrySeller,

		/// <summary>
		/// MDEntryPositionNo
		/// </summary>
		MDEntryPositionNo,

		/// <summary>
		/// FinancialStatus
		/// </summary>
		FinancialStatus,

		/// <summary>
		/// CorporateAction
		/// </summary>
		CorporateAction,

		/// <summary>
		/// DefBidSize
		/// </summary>
		DefBidSize,

		/// <summary>
		/// DefOfferSize
		/// </summary>
		DefOfferSize,

		/// <summary>
		/// NoQuoteEntries
		/// </summary>
		NoQuoteEntries,

		/// <summary>
		/// NoQuoteSets
		/// </summary>
		NoQuoteSets,

		/// <summary>
		/// QuoteStatus
		/// </summary>
		QuoteStatus,

		/// <summary>
		/// QuoteCancelType
		/// </summary>
		QuoteCancelType,

		/// <summary>
		/// QuoteEntryID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		QuoteEntryID,

		/// <summary>
		/// QuoteRejectReason
		/// </summary>
		QuoteRejectReason,

		/// <summary>
		/// QuoteResponseLevel
		/// </summary>
		QuoteResponseLevel,

		/// <summary>
		/// QuoteSetID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		QuoteSetID,

		/// <summary>
		/// QuoteRequestType
		/// </summary>
		QuoteRequestType,

		/// <summary>
		/// TotNoQuoteEntries
		/// </summary>
		TotNoQuoteEntries,

		/// <summary>
		/// UnderlyingSecurityIDSource
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		UnderlyingSecurityIDSource,

		/// <summary>
		/// UnderlyingIssuer
		/// </summary>
		UnderlyingIssuer,

		/// <summary>
		/// UnderlyingSecurityDesc
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Desc")]
		UnderlyingSecurityDesc,

		/// <summary>
		/// UnderlyingSecurityExchange
		/// </summary>
		UnderlyingSecurityExchange,

		/// <summary>
		/// UnderlyingSecurityID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		UnderlyingSecurityID,

		/// <summary>
		/// UnderlyingSecurityType
		/// </summary>
		UnderlyingSecurityType,

		/// <summary>
		/// UnderlyingSymbol
		/// </summary>
		UnderlyingSymbol,

		/// <summary>
		/// UnderlyingSymbolSfx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sfx")]
		UnderlyingSymbolSfx,

		/// <summary>
		/// UnderlyingMaturityMonthYear
		/// </summary>
		UnderlyingMaturityMonthYear,

		/// <summary>
		/// UnderlyingMaturityDay
		/// </summary>
		UnderlyingMaturityDay,

		/// <summary>
		/// UnderlyingPutOrCall
		/// </summary>
		UnderlyingPutOrCall,

		/// <summary>
		/// UnderlyingStrikePrice
		/// </summary>
		UnderlyingStrikePrice,

		/// <summary>
		/// UnderlyingOptAttribute
		/// </summary>
		UnderlyingOptAttribute,

		/// <summary>
		/// UnderlyingCurrency
		/// </summary>
		UnderlyingCurrency,

		/// <summary>
		/// RatioQty
		/// </summary>
		RatioQty,

		/// <summary>
		/// SecurityReqID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SecurityReqID,

		/// <summary>
		/// SecurityRequestType
		/// </summary>
		SecurityRequestType,

		/// <summary>
		/// SecurityResponseID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SecurityResponseID,

		/// <summary>
		/// SecurityResponseType
		/// </summary>
		SecurityResponseType,

		/// <summary>
		/// SecurityStatusReqID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SecurityStatusReqID,

		/// <summary>
		/// UnsolicitedIndicator
		/// </summary>
		UnsolicitedIndicator,

		/// <summary>
		/// SecurityTradingStatus
		/// </summary>
		SecurityTradingStatus,

		/// <summary>
		/// HaltReason
		/// </summary>
		HaltReason,

		/// <summary>
		/// InViewOfCommon
		/// </summary>
		InViewOfCommon,

		/// <summary>
		/// DueToRelated
		/// </summary>
		DueToRelated,

		/// <summary>
		/// BuyVolume
		/// </summary>
		BuyVolume,

		/// <summary>
		/// SellVolume
		/// </summary>
		SellVolume,

		/// <summary>
		/// HighPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		HighPx,

		/// <summary>
		/// LowPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		LowPx,

		/// <summary>
		/// Adjustment
		/// </summary>
		Adjustment,

		/// <summary>
		/// TradSesReqID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trad")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		TradSesReqID,

		/// <summary>
		/// TradingSessionID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		TradingSessionID,

		/// <summary>
		/// ContraTrader
		/// </summary>
		ContraTrader,

		/// <summary>
		/// TradSesMethod
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trad")]
		TradSesMethod,

		/// <summary>
		/// TradSesMode
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trad")]
		TradSesMode,

		/// <summary>
		/// TradSesStatus
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trad")]
		TradSesStatus,

		/// <summary>
		/// TradSesStartTime
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trad")]
		TradSesStartTime,

		/// <summary>
		/// TradSesOpenTime
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trad")]
		TradSesOpenTime,

		/// <summary>
		/// TradSesPreCloseTime
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trad")]
		TradSesPreCloseTime,

		/// <summary>
		/// TradSesCloseTime
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trad")]
		TradSesCloseTime,

		/// <summary>
		/// TradSesEndTime
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trad")]
		TradSesEndTime,

		/// <summary>
		/// NumberOfOrders
		/// </summary>
		NumberOfOrders,

		/// <summary>
		/// MessageEncoding
		/// </summary>
		MessageEncoding,

		/// <summary>
		/// EncodedIssuerLen
		/// </summary>
		EncodedIssuerLen,

		/// <summary>
		/// EncodedIssuer
		/// </summary>
		EncodedIssuer,

		/// <summary>
		/// EncodedSecurityDescLen
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Desc")]
		EncodedSecurityDescLen,

		/// <summary>
		/// EncodedSecurityDesc
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Desc")]
		EncodedSecurityDesc,

		/// <summary>
		/// EncodedListExecInstLen
		/// </summary>
		EncodedListExecInstLen,

		/// <summary>
		/// EncodedListExecInst
		/// </summary>
		EncodedListExecInst,

		/// <summary>
		/// EncodedTextLen
		/// </summary>
		EncodedTextLen,

		/// <summary>
		/// EncodedText
		/// </summary>
		EncodedText,

		/// <summary>
		/// EncodedSubjectLen
		/// </summary>
		EncodedSubjectLen,

		/// <summary>
		/// EncodedSubject
		/// </summary>
		EncodedSubject,

		/// <summary>
		/// EncodedHeadlineLen
		/// </summary>
		EncodedHeadlineLen,

		/// <summary>
		/// EncodedHeadline
		/// </summary>
		EncodedHeadline,

		/// <summary>
		/// EncodedAllocTextLen
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		EncodedAllocTextLen,

		/// <summary>
		/// EncodedAllocText
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		EncodedAllocText,

		/// <summary>
		/// EncodedUnderlyingIssuerLen
		/// </summary>
		EncodedUnderlyingIssuerLen,

		/// <summary>
		/// EncodedUnderlyingIssuer
		/// </summary>
		EncodedUnderlyingIssuer,

		/// <summary>
		/// EncodedUnderlyingSecurityDescLen
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Desc")]
		EncodedUnderlyingSecurityDescLen,

		/// <summary>
		/// EncodedUnderlyingSecurityDesc
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Desc")]
		EncodedUnderlyingSecurityDesc,

		/// <summary>
		/// AllocPrice
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocPrice,

		/// <summary>
		/// QuoteSetValidUntilTime
		/// </summary>
		QuoteSetValidUntilTime,

		/// <summary>
		/// QuoteEntryRejectReason
		/// </summary>
		QuoteEntryRejectReason,

		/// <summary>
		/// LastMsgSeqNumProcessed
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
		LastMsgSeqNumProcessed,

		/// <summary>
		/// OnBehalfOfSendingTime
		/// </summary>
		OnBehalfOfSendingTime,

		/// <summary>
		/// RefTagID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		RefTagID,

		/// <summary>
		/// RefMsgType
		/// </summary>
		RefMsgType,

		/// <summary>
		/// SessionRejectReason
		/// </summary>
		SessionRejectReason,

		/// <summary>
		/// BidRequestTransType
		/// </summary>
		BidRequestTransType,

		/// <summary>
		/// ContraBroker
		/// </summary>
		ContraBroker,

		/// <summary>
		/// ComplianceID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		ComplianceID,

		/// <summary>
		/// SolicitedFlag
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag")]
		SolicitedFlag,

		/// <summary>
		/// ExecRestatementReason
		/// </summary>
		ExecRestatementReason,

		/// <summary>
		/// BusinessRejectRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		BusinessRejectRefID,

		/// <summary>
		/// BusinessRejectReason
		/// </summary>
		BusinessRejectReason,

		/// <summary>
		/// GrossTradeAmt
		/// </summary>
		GrossTradeAmt,

		/// <summary>
		/// NoContraBrokers
		/// </summary>
		NoContraBrokers,

		/// <summary>
		/// MaxMessageSize
		/// </summary>
		MaxMessageSize,

		/// <summary>
		/// NoMsgTypes
		/// </summary>
		NoMsgTypes,

		/// <summary>
		/// MsgDirection
		/// </summary>
		MsgDirection,

		/// <summary>
		/// NoTradingSessions
		/// </summary>
		NoTradingSessions,

		/// <summary>
		/// TotalVolumeTraded
		/// </summary>
		TotalVolumeTraded,

		/// <summary>
		/// DiscretionInst
		/// </summary>
		DiscretionInst,

		/// <summary>
		/// DiscretionOffset
		/// </summary>
		DiscretionOffset,

		/// <summary>
		/// BidID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		BidID,

		/// <summary>
		/// ClientBidID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		ClientBidID,

		/// <summary>
		/// ListName
		/// </summary>
		ListName,

		/// <summary>
		/// TotNoRelatedSym
		/// </summary>
		TotNoRelatedSym,

		/// <summary>
		/// BidType
		/// </summary>
		BidType,

		/// <summary>
		/// NumTickets
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
		NumTickets,

		/// <summary>
		/// SideValue1
		/// </summary>
		SideValue1,

		/// <summary>
		/// SideValue2
		/// </summary>
		SideValue2,

		/// <summary>
		/// NoBidDescriptors
		/// </summary>
		NoBidDescriptors,

		/// <summary>
		/// BidDescriptorType
		/// </summary>
		BidDescriptorType,

		/// <summary>
		/// BidDescriptor
		/// </summary>
		BidDescriptor,

		/// <summary>
		/// SideValueInd
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ind")]
		SideValueInd,

		/// <summary>
		/// LiquidityPctLow
		/// </summary>
		LiquidityPctLow,

		/// <summary>
		/// LiquidityPctHigh
		/// </summary>
		LiquidityPctHigh,

		/// <summary>
		/// LiquidityValue
		/// </summary>
		LiquidityValue,

		/// <summary>
		/// EFPTrackingError
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "EFP")]
		EFPTrackingError,

		/// <summary>
		/// FairValue
		/// </summary>
		FairValue,

		/// <summary>
		/// OutsideIndexPct
		/// </summary>
		OutsideIndexPct,

		/// <summary>
		/// ValueOfFutures
		/// </summary>
		ValueOfFutures,

		/// <summary>
		/// LiquidityIndType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ind")]
		LiquidityIndType,

		/// <summary>
		/// WtAverageLiquidity
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Wt")]
		WtAverageLiquidity,

		/// <summary>
		/// ExchangeForPhysical
		/// </summary>
		ExchangeForPhysical,

		/// <summary>
		/// OutMainCntryUIndex
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cntry")]
		OutMainCntryUIndex,

		/// <summary>
		/// CrossPercent
		/// </summary>
		CrossPercent,

		/// <summary>
		/// ProgRptReqs
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Reqs")]
		ProgRptReqs,

		/// <summary>
		/// ProgPeriodInterval
		/// </summary>
		ProgPeriodInterval,

		/// <summary>
		/// IncTaxInd
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ind")]
		IncTaxInd,

		/// <summary>
		/// NumBidders
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
		NumBidders,

		/// <summary>
		/// BidTradeType
		/// </summary>
		BidTradeType,

		/// <summary>
		/// BasisPxType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		BasisPxType,

		/// <summary>
		/// NoBidComponents
		/// </summary>
		NoBidComponents,

		/// <summary>
		/// Country
		/// </summary>
		Country,

		/// <summary>
		/// TotNoStrikes
		/// </summary>
		TotNoStrikes,

		/// <summary>
		/// PriceType
		/// </summary>
		PriceType,

		/// <summary>
		/// DayOrderQty
		/// </summary>
		DayOrderQty,

		/// <summary>
		/// DayCumQty
		/// </summary>
		DayCumQty,

		/// <summary>
		/// DayAvgPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Avg")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		DayAvgPx,

		/// <summary>
		/// GTBookingInst
		/// </summary>
		GTBookingInst,

		/// <summary>
		/// NoStrikes
		/// </summary>
		NoStrikes,

		/// <summary>
		/// ListStatusType
		/// </summary>
		ListStatusType,

		/// <summary>
		/// NetGrossInd
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ind")]
		NetGrossInd,

		/// <summary>
		/// ListOrderStatus
		/// </summary>
		ListOrderStatus,

		/// <summary>
		/// ExpireDate
		/// </summary>
		ExpireDate,

		/// <summary>
		/// ListExecInstType
		/// </summary>
		ListExecInstType,

		/// <summary>
		/// CxlRejResponseTo
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rej")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cxl")]
		CxlRejResponseTo,

		/// <summary>
		/// UnderlyingCouponRate
		/// </summary>
		UnderlyingCouponRate,

		/// <summary>
		/// UnderlyingContractMultiplier
		/// </summary>
		UnderlyingContractMultiplier,

		/// <summary>
		/// ContraTradeQty
		/// </summary>
		ContraTradeQty,

		/// <summary>
		/// ContraTradeTime
		/// </summary>
		ContraTradeTime,

		/// <summary>
		/// ClearingFirm
		/// </summary>
		ClearingFirm,

		/// <summary>
		/// ClearingAccount
		/// </summary>
		ClearingAccount,

		/// <summary>
		/// LiquidityNumSecurities
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
		LiquidityNumSecurities,

		/// <summary>
		/// MultiLegReportingType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi")]
		MultiLegReportingType,

		/// <summary>
		/// StrikeTime
		/// </summary>
		StrikeTime,

		/// <summary>
		/// ListStatusText
		/// </summary>
		ListStatusText,

		/// <summary>
		/// EncodedListStatusTextLen
		/// </summary>
		EncodedListStatusTextLen,

		/// <summary>
		/// EncodedListStatusText
		/// </summary>
		EncodedListStatusText,

		/// <summary>
		/// PartyIDSource
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		PartyIDSource,

		/// <summary>
		/// PartyID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		PartyID,

		/// <summary>
		/// TotalVolumeTradedDate
		/// </summary>
		TotalVolumeTradedDate,

		/// <summary>
		/// TotalVolumeTraded_Time
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
		TotalVolumeTraded_Time,

		/// <summary>
		/// NetChgPrevDay
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Prev")]
		NetChgPrevDay,

		/// <summary>
		/// PartyRole
		/// </summary>
		PartyRole,

		/// <summary>
		/// NoPartyIDs
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IDs")]
		NoPartyIDs,

		/// <summary>
		/// NoSecurityAltID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		NoSecurityAltID,

		/// <summary>
		/// SecurityAltID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SecurityAltID,

		/// <summary>
		/// SecurityAltIDSource
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SecurityAltIDSource,

		/// <summary>
		/// NoUnderlyingSecurityAltID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		NoUnderlyingSecurityAltID,

		/// <summary>
		/// UnderlyingSecurityAltID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		UnderlyingSecurityAltID,

		/// <summary>
		/// UnderlyingSecurityAltIDSource
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		UnderlyingSecurityAltIDSource,

		/// <summary>
		/// Product
		/// </summary>
		Product,

		/// <summary>
		/// CFICode
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CFI")]
		CFICode,

		/// <summary>
		/// UnderlyingProduct
		/// </summary>
		UnderlyingProduct,

		/// <summary>
		/// UnderlyingCFICode
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CFI")]
		UnderlyingCFICode,

		/// <summary>
		/// TestMessageIndicator
		/// </summary>
		TestMessageIndicator,

		/// <summary>
		/// QuantityType
		/// </summary>
		QuantityType,

		/// <summary>
		/// BookingRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		BookingRefID,

		/// <summary>
		/// IndividualAllocID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		IndividualAllocID,

		/// <summary>
		/// RoundingDirection
		/// </summary>
		RoundingDirection,

		/// <summary>
		/// RoundingModulus
		/// </summary>
		RoundingModulus,

		/// <summary>
		/// CountryOfIssue
		/// </summary>
		CountryOfIssue,

		/// <summary>
		/// StateOrProvinceOfIssue
		/// </summary>
		StateOrProvinceOfIssue,

		/// <summary>
		/// LocaleOfIssue
		/// </summary>
		LocaleOfIssue,

		/// <summary>
		/// NoRegistDtls
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Regist")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dtls")]
		NoRegistDtls,

		/// <summary>
		/// MailingDtls
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dtls")]
		MailingDtls,

		/// <summary>
		/// InvestorCountryOfResidence
		/// </summary>
		InvestorCountryOfResidence,

		/// <summary>
		/// PaymentRef
		/// </summary>
		PaymentRef,

		/// <summary>
		/// DistribPaymentMethod
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Distrib")]
		DistribPaymentMethod,

		/// <summary>
		/// CashDistribCurr
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Distrib")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Curr")]
		CashDistribCurr,

		/// <summary>
		/// CommCurrency
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Comm")]
		CommCurrency,

		/// <summary>
		/// CancellationRights
		/// </summary>
		CancellationRights,

		/// <summary>
		/// MoneyLaunderingStatus
		/// </summary>
		MoneyLaunderingStatus,

		/// <summary>
		/// MailingInst
		/// </summary>
		MailingInst,

		/// <summary>
		/// TransBkdTime
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bkd")]
		TransBkdTime,

		/// <summary>
		/// ExecPriceType
		/// </summary>
		ExecPriceType,

		/// <summary>
		/// ExecPriceAdjustment
		/// </summary>
		ExecPriceAdjustment,

		/// <summary>
		/// DateOfBirth
		/// </summary>
		DateOfBirth,

		/// <summary>
		/// TradeReportTransType
		/// </summary>
		TradeReportTransType,

		/// <summary>
		/// CardHolderName
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "CardHolder")]
		CardHolderName,

		/// <summary>
		/// CardNumber
		/// </summary>
		CardNumber,

		/// <summary>
		/// CardExpDate
		/// </summary>
		CardExpDate,

		/// <summary>
		/// CardIssNum
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Iss")]
		CardIssNum,

		/// <summary>
		/// PaymentMethod
		/// </summary>
		PaymentMethod,

		/// <summary>
		/// RegistAcctType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Regist")]
		RegistAcctType,

		/// <summary>
		/// Designation
		/// </summary>
		Designation,

		/// <summary>
		/// TaxAdvantageType
		/// </summary>
		TaxAdvantageType,

		/// <summary>
		/// RegistRejReasonText
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rej")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Regist")]
		RegistRejReasonText,

		/// <summary>
		/// FundRenewWaiv
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Waiv")]
		FundRenewWaiv,

		/// <summary>
		/// CashDistribAgentName
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Distrib")]
		CashDistribAgentName,

		/// <summary>
		/// CashDistribAgentCode
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Distrib")]
		CashDistribAgentCode,

		/// <summary>
		/// CashDistribAgentAcctNumber
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Distrib")]
		CashDistribAgentAcctNumber,

		/// <summary>
		/// CashDistribPayRef
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Distrib")]
		CashDistribPayRef,

		/// <summary>
		/// CashDistribAgentAcctName
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Distrib")]
		CashDistribAgentAcctName,

		/// <summary>
		/// CardStartDate
		/// </summary>
		CardStartDate,

		/// <summary>
		/// PaymentDate
		/// </summary>
		PaymentDate,

		/// <summary>
		/// PaymentRemitterID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		PaymentRemitterID,

		/// <summary>
		/// RegistStatus
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Regist")]
		RegistStatus,

		/// <summary>
		/// RegistRejReasonCode
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rej")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Regist")]
		RegistRejReasonCode,

		/// <summary>
		/// RegistRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Regist")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		RegistRefID,

		/// <summary>
		/// RegistDtls
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Regist")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dtls")]
		RegistDtls,

		/// <summary>
		/// NoDistribInsts
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Insts")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Distrib")]
		NoDistribInsts,

		/// <summary>
		/// RegistEmail
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Regist")]
		RegistEmail,

		/// <summary>
		/// DistribPercentage
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Distrib")]
		DistribPercentage,

		/// <summary>
		/// RegistID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Regist")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		RegistID,

		/// <summary>
		/// RegistTransType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Regist")]
		RegistTransType,

		/// <summary>
		/// ExecValuationPoint
		/// </summary>
		ExecValuationPoint,

		/// <summary>
		/// OrderPercent
		/// </summary>
		OrderPercent,

		/// <summary>
		/// OwnershipType
		/// </summary>
		OwnershipType,

		/// <summary>
		/// NoContAmts
		/// </summary>
		NoContAmts,

		/// <summary>
		/// ContAmtType
		/// </summary>
		ContAmtType,

		/// <summary>
		/// ContAmtValue
		/// </summary>
		ContAmtValue,

		/// <summary>
		/// ContAmtCurr
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Curr")]
		ContAmtCurr,

		/// <summary>
		/// OwnerType
		/// </summary>
		OwnerType,

		/// <summary>
		/// PartySubID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		PartySubID,

		/// <summary>
		/// NestedPartyID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		NestedPartyID,

		/// <summary>
		/// NestedPartyIDSource
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		NestedPartyIDSource,

		/// <summary>
		/// SecondaryClOrdID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ord")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Cl")]
		SecondaryClOrdID,

		/// <summary>
		/// SecondaryExecID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SecondaryExecID,

		/// <summary>
		/// OrderCapacity
		/// </summary>
		OrderCapacity,

		/// <summary>
		/// OrderRestrictions
		/// </summary>
		OrderRestrictions,

		/// <summary>
		/// MassCancelRequestType
		/// </summary>
		MassCancelRequestType,

		/// <summary>
		/// MassCancelResponse
		/// </summary>
		MassCancelResponse,

		/// <summary>
		/// MassCancelRejectReason
		/// </summary>
		MassCancelRejectReason,

		/// <summary>
		/// TotalAffectedOrders
		/// </summary>
		TotalAffectedOrders,

		/// <summary>
		/// NoAffectedOrders
		/// </summary>
		NoAffectedOrders,

		/// <summary>
		/// AffectedOrderID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		AffectedOrderID,

		/// <summary>
		/// AffectedSecondaryOrderID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		AffectedSecondaryOrderID,

		/// <summary>
		/// QuoteType
		/// </summary>
		QuoteType,

		/// <summary>
		/// NestedPartyRole
		/// </summary>
		NestedPartyRole,

		/// <summary>
		/// NoNestedPartyIDs
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IDs")]
		NoNestedPartyIDs,

		/// <summary>
		/// TotalAccruedInterestAmt
		/// </summary>
		TotalAccruedInterestAmt,

		/// <summary>
		/// MaturityDate
		/// </summary>
		MaturityDate,

		/// <summary>
		/// UnderlyingMaturityDate
		/// </summary>
		UnderlyingMaturityDate,

		/// <summary>
		/// InstrRegistry
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Instr")]
		InstrRegistry,

		/// <summary>
		/// CashMargin
		/// </summary>
		CashMargin,

		/// <summary>
		/// NestedPartySubID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		NestedPartySubID,

		/// <summary>
		/// Scope
		/// </summary>
		Scope,

		/// <summary>
		/// MDImplicitDelete
		/// </summary>
		MDImplicitDelete,

		/// <summary>
		/// CrossID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		CrossID,

		/// <summary>
		/// CrossType
		/// </summary>
		CrossType,

		/// <summary>
		/// CrossPrioritization
		/// </summary>
		CrossPrioritization,

		/// <summary>
		/// OrigCrossID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Orig")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		OrigCrossID,

		/// <summary>
		/// NoSides
		/// </summary>
		NoSides,

		/// <summary>
		/// Username
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Username")]
		Username,

		/// <summary>
		/// Password
		/// </summary>
		Password,

		/// <summary>
		/// NoLegs
		/// </summary>
		NoLegs,

		/// <summary>
		/// LegCurrency
		/// </summary>
		LegCurrency,

		/// <summary>
		/// TotNoSecurityTypes
		/// </summary>
		TotNoSecurityTypes,

		/// <summary>
		/// NoSecurityTypes
		/// </summary>
		NoSecurityTypes,

		/// <summary>
		/// SecurityListRequestType
		/// </summary>
		SecurityListRequestType,

		/// <summary>
		/// SecurityRequestResult
		/// </summary>
		SecurityRequestResult,

		/// <summary>
		/// RoundLot
		/// </summary>
		RoundLot,

		/// <summary>
		/// MinTradeVol
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Vol")]
		MinTradeVol,

		/// <summary>
		/// MultiLegRptTypeReq
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi")]
		MultiLegRptTypeReq,

		/// <summary>
		/// LegPositionEffect
		/// </summary>
		LegPositionEffect,

		/// <summary>
		/// LegCoveredOrUncovered
		/// </summary>
		LegCoveredOrUncovered,

		/// <summary>
		/// LegPrice
		/// </summary>
		LegPrice,

		/// <summary>
		/// TradSesStatusRejReason
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trad")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rej")]
		TradSesStatusRejReason,

		/// <summary>
		/// TradeRequestID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		TradeRequestID,

		/// <summary>
		/// TradeRequestType
		/// </summary>
		TradeRequestType,

		/// <summary>
		/// PreviouslyReported
		/// </summary>
		PreviouslyReported,

		/// <summary>
		/// TradeReportID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		TradeReportID,

		/// <summary>
		/// TradeReportRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		TradeReportRefID,

		/// <summary>
		/// MatchStatus
		/// </summary>
		MatchStatus,

		/// <summary>
		/// MatchType
		/// </summary>
		MatchType,

		/// <summary>
		/// OddLot
		/// </summary>
		OddLot,

		/// <summary>
		/// NoClearingInstructions
		/// </summary>
		NoClearingInstructions,

		/// <summary>
		/// ClearingInstruction
		/// </summary>
		ClearingInstruction,

		/// <summary>
		/// TradeInputSource
		/// </summary>
		TradeInputSource,

		/// <summary>
		/// TradeInputDevice
		/// </summary>
		TradeInputDevice,

		/// <summary>
		/// NoDates
		/// </summary>
		NoDates,

		/// <summary>
		/// AccountType
		/// </summary>
		AccountType,

		/// <summary>
		/// CustOrderCapacity
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cust")]
		CustOrderCapacity,

		/// <summary>
		/// ClOrdLinkID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ord")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Cl")]
		ClOrdLinkID,

		/// <summary>
		/// MassStatusReqID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		MassStatusReqID,

		/// <summary>
		/// MassStatusReqType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		MassStatusReqType,

		/// <summary>
		/// OrigOrdModTime
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Orig")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ord")]
		OrigOrdModTime,

		/// <summary>
		/// LegSettlType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		LegSettlType,

		/// <summary>
		/// LegSettlDate
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		LegSettlDate,

		/// <summary>
		/// DayBookingInst
		/// </summary>
		DayBookingInst,

		/// <summary>
		/// BookingUnit
		/// </summary>
		BookingUnit,

		/// <summary>
		/// PreallocMethod
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Prealloc")]
		PreallocMethod,

		/// <summary>
		/// UnderlyingCountryOfIssue
		/// </summary>
		UnderlyingCountryOfIssue,

		/// <summary>
		/// UnderlyingStateOrProvinceOfIssue
		/// </summary>
		UnderlyingStateOrProvinceOfIssue,

		/// <summary>
		/// UnderlyingLocaleOfIssue
		/// </summary>
		UnderlyingLocaleOfIssue,

		/// <summary>
		/// UnderlyingInstrRegistry
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Instr")]
		UnderlyingInstrRegistry,

		/// <summary>
		/// LegCountryOfIssue
		/// </summary>
		LegCountryOfIssue,

		/// <summary>
		/// LegStateOrProvinceOfIssue
		/// </summary>
		LegStateOrProvinceOfIssue,

		/// <summary>
		/// LegLocaleOfIssue
		/// </summary>
		LegLocaleOfIssue,

		/// <summary>
		/// LegInstrRegistry
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Instr")]
		LegInstrRegistry,

		/// <summary>
		/// LegSymbol
		/// </summary>
		LegSymbol,

		/// <summary>
		/// LegSymbolSfx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sfx")]
		LegSymbolSfx,

		/// <summary>
		/// LegSecurityID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		LegSecurityID,

		/// <summary>
		/// LegSecurityIDSource
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		LegSecurityIDSource,

		/// <summary>
		/// NoLegSecurityAltID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		NoLegSecurityAltID,

		/// <summary>
		/// LegSecurityAltID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		LegSecurityAltID,

		/// <summary>
		/// LegSecurityAltIDSource
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		LegSecurityAltIDSource,

		/// <summary>
		/// LegProduct
		/// </summary>
		LegProduct,

		/// <summary>
		/// LegCFICode
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CFI")]
		LegCFICode,

		/// <summary>
		/// LegSecurityType
		/// </summary>
		LegSecurityType,

		/// <summary>
		/// LegMaturityMonthYear
		/// </summary>
		LegMaturityMonthYear,

		/// <summary>
		/// LegMaturityDate
		/// </summary>
		LegMaturityDate,

		/// <summary>
		/// LegStrikePrice
		/// </summary>
		LegStrikePrice,

		/// <summary>
		/// LegOptAttribute
		/// </summary>
		LegOptAttribute,

		/// <summary>
		/// LegContractMultiplier
		/// </summary>
		LegContractMultiplier,

		/// <summary>
		/// LegCouponRate
		/// </summary>
		LegCouponRate,

		/// <summary>
		/// LegSecurityExchange
		/// </summary>
		LegSecurityExchange,

		/// <summary>
		/// LegIssuer
		/// </summary>
		LegIssuer,

		/// <summary>
		/// EncodedLegIssuerLen
		/// </summary>
		EncodedLegIssuerLen,

		/// <summary>
		/// EncodedLegIssuer
		/// </summary>
		EncodedLegIssuer,

		/// <summary>
		/// LegSecurityDesc
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Desc")]
		LegSecurityDesc,

		/// <summary>
		/// EncodedLegSecurityDescLen
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Desc")]
		EncodedLegSecurityDescLen,

		/// <summary>
		/// EncodedLegSecurityDesc
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Desc")]
		EncodedLegSecurityDesc,

		/// <summary>
		/// LegRatioQty
		/// </summary>
		LegRatioQty,

		/// <summary>
		/// LegSide
		/// </summary>
		LegSide,

		/// <summary>
		/// TradingSessionSubID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		TradingSessionSubID,

		/// <summary>
		/// AllocType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocType,

		/// <summary>
		/// NoHops
		/// </summary>
		NoHops,

		/// <summary>
		/// HopCompID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		HopCompID,

		/// <summary>
		/// HopSendingTime
		/// </summary>
		HopSendingTime,

		/// <summary>
		/// HopRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		HopRefID,

		/// <summary>
		/// MidPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		MidPx,

		/// <summary>
		/// BidYield
		/// </summary>
		BidYield,

		/// <summary>
		/// MidYield
		/// </summary>
		MidYield,

		/// <summary>
		/// OfferYield
		/// </summary>
		OfferYield,

		/// <summary>
		/// ClearingFeeIndicator
		/// </summary>
		ClearingFeeIndicator,

		/// <summary>
		/// WorkingIndicator
		/// </summary>
		WorkingIndicator,

		/// <summary>
		/// LegLastPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		LegLastPx,

		/// <summary>
		/// PriorityIndicator
		/// </summary>
		PriorityIndicator,

		/// <summary>
		/// PriceImprovement
		/// </summary>
		PriceImprovement,

		/// <summary>
		/// Price2
		/// </summary>
		Price2,

		/// <summary>
		/// LastForwardPoints2
		/// </summary>
		LastForwardPoints2,

		/// <summary>
		/// BidForwardPoints2
		/// </summary>
		BidForwardPoints2,

		/// <summary>
		/// OfferForwardPoints2
		/// </summary>
		OfferForwardPoints2,

		/// <summary>
		/// RFQReqID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "RFQ")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		RFQReqID,

		/// <summary>
		/// MktBidPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mkt")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		MktBidPx,

		/// <summary>
		/// MktOfferPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mkt")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		MktOfferPx,

		/// <summary>
		/// MinBidSize
		/// </summary>
		MinBidSize,

		/// <summary>
		/// MinOfferSize
		/// </summary>
		MinOfferSize,

		/// <summary>
		/// QuoteStatusReqID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		QuoteStatusReqID,

		/// <summary>
		/// LegalConfirm
		/// </summary>
		LegalConfirm,

		/// <summary>
		/// UnderlyingLastPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		UnderlyingLastPx,

		/// <summary>
		/// UnderlyingLastQty
		/// </summary>
		UnderlyingLastQty,

		/// <summary>
		/// SecDefStatus
		/// </summary>
		SecDefStatus,

		/// <summary>
		/// LegRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		LegRefID,

		/// <summary>
		/// ContraLegRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		ContraLegRefID,

		/// <summary>
		/// SettlCurrBidFxRate
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Curr")]
		SettlCurrBidFxRate,

		/// <summary>
		/// SettlCurrOfferFxRate
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Curr")]
		SettlCurrOfferFxRate,

		/// <summary>
		/// QuoteRequestRejectReason
		/// </summary>
		QuoteRequestRejectReason,

		/// <summary>
		/// SideComplianceID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SideComplianceID,

		/// <summary>
		/// AcctIDSource
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		AcctIDSource,

		/// <summary>
		/// AllocAcctIDSource
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		AllocAcctIDSource,

		/// <summary>
		/// BenchmarkPrice
		/// </summary>
		BenchmarkPrice,

		/// <summary>
		/// BenchmarkPriceType
		/// </summary>
		BenchmarkPriceType,

		/// <summary>
		/// ConfirmID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		ConfirmID,

		/// <summary>
		/// ConfirmStatus
		/// </summary>
		ConfirmStatus,

		/// <summary>
		/// ConfirmTransType
		/// </summary>
		ConfirmTransType,

		/// <summary>
		/// ContractSettlMonth
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		ContractSettlMonth,

		/// <summary>
		/// DeliveryForm
		/// </summary>
		DeliveryForm,

		/// <summary>
		/// LastParPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		LastParPx,

		/// <summary>
		/// NoLegAllocs
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Allocs")]
		NoLegAllocs,

		/// <summary>
		/// LegAllocAccount
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		LegAllocAccount,

		/// <summary>
		/// LegIndividualAllocID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		LegIndividualAllocID,

		/// <summary>
		/// LegAllocQty
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		LegAllocQty,

		/// <summary>
		/// LegAllocAcctIDSource
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		LegAllocAcctIDSource,

		/// <summary>
		/// LegSettlCurrency
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		LegSettlCurrency,

		/// <summary>
		/// LegBenchmarkCurveCurrency
		/// </summary>
		LegBenchmarkCurveCurrency,

		/// <summary>
		/// LegBenchmarkCurveName
		/// </summary>
		LegBenchmarkCurveName,

		/// <summary>
		/// LegBenchmarkCurvePoint
		/// </summary>
		LegBenchmarkCurvePoint,

		/// <summary>
		/// LegBenchmarkPrice
		/// </summary>
		LegBenchmarkPrice,

		/// <summary>
		/// LegBenchmarkPriceType
		/// </summary>
		LegBenchmarkPriceType,

		/// <summary>
		/// LegBidPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		LegBidPx,

		/// <summary>
		/// LegIoiQty
		/// </summary>
		LegIoiQty,

		/// <summary>
		/// NoLegStipulations
		/// </summary>
		NoLegStipulations,

		/// <summary>
		/// LegOfferPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		LegOfferPx,

		/// <summary>
		/// LegOrderQty
		/// </summary>
		LegOrderQty,

		/// <summary>
		/// LegPriceType
		/// </summary>
		LegPriceType,

		/// <summary>
		/// LegQty
		/// </summary>
		LegQty,

		/// <summary>
		/// LegStipulationType
		/// </summary>
		LegStipulationType,

		/// <summary>
		/// LegStipulationValue
		/// </summary>
		LegStipulationValue,

		/// <summary>
		/// LegSwapType
		/// </summary>
		LegSwapType,

		/// <summary>
		/// Pool
		/// </summary>
		Pool,

		/// <summary>
		/// QuotePriceType
		/// </summary>
		QuotePriceType,

		/// <summary>
		/// QuoteRespID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Resp")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		QuoteRespID,

		/// <summary>
		/// QuoteRespType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Resp")]
		QuoteRespType,

		/// <summary>
		/// QuoteQualifier
		/// </summary>
		QuoteQualifier,

		/// <summary>
		/// YieldRedemptionDate
		/// </summary>
		YieldRedemptionDate,

		/// <summary>
		/// YieldRedemptionPrice
		/// </summary>
		YieldRedemptionPrice,

		/// <summary>
		/// YieldRedemptionPriceType
		/// </summary>
		YieldRedemptionPriceType,

		/// <summary>
		/// BenchmarkSecurityID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		BenchmarkSecurityID,

		/// <summary>
		/// ReversalIndicator
		/// </summary>
		ReversalIndicator,

		/// <summary>
		/// YieldCalcDate
		/// </summary>
		YieldCalcDate,

		/// <summary>
		/// NoPositions
		/// </summary>
		NoPositions,

		/// <summary>
		/// PosType
		/// </summary>
		PosType,

		/// <summary>
		/// LongQty
		/// </summary>
		LongQty,

		/// <summary>
		/// ShortQty
		/// </summary>
		ShortQty,

		/// <summary>
		/// PosQtyStatus
		/// </summary>
		PosQtyStatus,

		/// <summary>
		/// PosAmtType
		/// </summary>
		PosAmtType,

		/// <summary>
		/// PosAmt
		/// </summary>
		PosAmt,

		/// <summary>
		/// PosTransType
		/// </summary>
		PosTransType,

		/// <summary>
		/// PosReqID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		PosReqID,

		/// <summary>
		/// NoUnderlyings
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Underlyings")]
		NoUnderlyings,

		/// <summary>
		/// PosMaintAction
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Maint")]
		PosMaintAction,

		/// <summary>
		/// OrigPosReqRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Orig")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		OrigPosReqRefID,

		/// <summary>
		/// PosMaintRptRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Maint")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		PosMaintRptRefID,

		/// <summary>
		/// ClearingBusinessDate
		/// </summary>
		ClearingBusinessDate,

		/// <summary>
		/// SettlSessID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sess")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SettlSessID,

		/// <summary>
		/// SettlSessSubID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sess")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SettlSessSubID,

		/// <summary>
		/// AdjustmentType
		/// </summary>
		AdjustmentType,

		/// <summary>
		/// ContraryInstructionIndicator
		/// </summary>
		ContraryInstructionIndicator,

		/// <summary>
		/// PriorSpreadIndicator
		/// </summary>
		PriorSpreadIndicator,

		/// <summary>
		/// PosMaintRptID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Maint")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		PosMaintRptID,

		/// <summary>
		/// PosMaintStatus
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Maint")]
		PosMaintStatus,

		/// <summary>
		/// PosMaintResult
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Maint")]
		PosMaintResult,

		/// <summary>
		/// PosReqType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		PosReqType,

		/// <summary>
		/// ResponseTransportType
		/// </summary>
		ResponseTransportType,

		/// <summary>
		/// ResponseDestination
		/// </summary>
		ResponseDestination,

		/// <summary>
		/// TotalNumPosReports
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
		TotalNumPosReports,

		/// <summary>
		/// PosReqResult
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		PosReqResult,

		/// <summary>
		/// PosReqStatus
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		PosReqStatus,

		/// <summary>
		/// SettlPrice
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		SettlPrice,

		/// <summary>
		/// SettlPriceType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		SettlPriceType,

		/// <summary>
		/// UnderlyingSettlPrice
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		UnderlyingSettlPrice,

		/// <summary>
		/// UnderlyingSettlPriceType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		UnderlyingSettlPriceType,

		/// <summary>
		/// PriorSettlPrice
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		PriorSettlPrice,

		/// <summary>
		/// NoQuoteQualifiers
		/// </summary>
		NoQuoteQualifiers,

		/// <summary>
		/// AllocSettlCurrency
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocSettlCurrency,

		/// <summary>
		/// AllocSettlCurrAmt
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Curr")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocSettlCurrAmt,

		/// <summary>
		/// InterestAtMaturity
		/// </summary>
		InterestAtMaturity,

		/// <summary>
		/// LegDatedDate
		/// </summary>
		LegDatedDate,

		/// <summary>
		/// LegPool
		/// </summary>
		LegPool,

		/// <summary>
		/// AllocInterestAtMaturity
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocInterestAtMaturity,

		/// <summary>
		/// AllocAccruedInterestAmt
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocAccruedInterestAmt,

		/// <summary>
		/// DeliveryDate
		/// </summary>
		DeliveryDate,

		/// <summary>
		/// AssignmentMethod
		/// </summary>
		AssignmentMethod,

		/// <summary>
		/// AssignmentUnit
		/// </summary>
		AssignmentUnit,

		/// <summary>
		/// OpenInterest
		/// </summary>
		OpenInterest,

		/// <summary>
		/// ExerciseMethod
		/// </summary>
		ExerciseMethod,

		/// <summary>
		/// TotNumTradeReports
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
		TotNumTradeReports,

		/// <summary>
		/// TradeRequestResult
		/// </summary>
		TradeRequestResult,

		/// <summary>
		/// TradeRequestStatus
		/// </summary>
		TradeRequestStatus,

		/// <summary>
		/// TradeReportRejectReason
		/// </summary>
		TradeReportRejectReason,

		/// <summary>
		/// SideMultiLegReportingType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi")]
		SideMultiLegReportingType,

		/// <summary>
		/// NoPosAmt
		/// </summary>
		NoPosAmt,

		/// <summary>
		/// AutoAcceptIndicator
		/// </summary>
		AutoAcceptIndicator,

		/// <summary>
		/// AllocReportID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		AllocReportID,

		/// <summary>
		/// NoNested2PartyIDs
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IDs")]
		NoNested2PartyIDs,

		/// <summary>
		/// Nested2PartyID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		Nested2PartyID,

		/// <summary>
		/// Nested2PartyIDSource
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		Nested2PartyIDSource,

		/// <summary>
		/// Nested2PartyRole
		/// </summary>
		Nested2PartyRole,

		/// <summary>
		/// Nested2PartySubID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		Nested2PartySubID,

		/// <summary>
		/// BenchmarkSecurityIDSource
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		BenchmarkSecurityIDSource,

		/// <summary>
		/// SecuritySubType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "SubType")]
		SecuritySubType,

		/// <summary>
		/// UnderlyingSecuritySubType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "SubType")]
		UnderlyingSecuritySubType,

		/// <summary>
		/// LegSecuritySubType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "SubType")]
		LegSecuritySubType,

		/// <summary>
		/// AllowableOneSidednessPct
		/// </summary>
		AllowableOneSidednessPct,

		/// <summary>
		/// AllowableOneSidednessValue
		/// </summary>
		AllowableOneSidednessValue,

		/// <summary>
		/// AllowableOneSidednessCurr
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Curr")]
		AllowableOneSidednessCurr,

		/// <summary>
		/// NoTrdRegTimestamps
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trd")]
		NoTrdRegTimestamps,

		/// <summary>
		/// TrdRegTimestamp
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trd")]
		TrdRegTimestamp,

		/// <summary>
		/// TrdRegTimestampType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trd")]
		TrdRegTimestampType,

		/// <summary>
		/// TrdRegTimestampOrigin
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trd")]
		TrdRegTimestampOrigin,

		/// <summary>
		/// ConfirmRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		ConfirmRefID,

		/// <summary>
		/// ConfirmType
		/// </summary>
		ConfirmType,

		/// <summary>
		/// ConfirmRejReason
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rej")]
		ConfirmRejReason,

		/// <summary>
		/// BookingType
		/// </summary>
		BookingType,

		/// <summary>
		/// IndividualAllocRejCode
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rej")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		IndividualAllocRejCode,

		/// <summary>
		/// SettlInstMsgID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SettlInstMsgID,

		/// <summary>
		/// NoSettlInst
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		NoSettlInst,

		/// <summary>
		/// LastUpdateTime
		/// </summary>
		LastUpdateTime,

		/// <summary>
		/// AllocSettlInstType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocSettlInstType,

		/// <summary>
		/// NoSettlPartyIDs
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IDs")]
		NoSettlPartyIDs,

		/// <summary>
		/// SettlPartyID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SettlPartyID,

		/// <summary>
		/// SettlPartyIDSource
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SettlPartyIDSource,

		/// <summary>
		/// SettlPartyRole
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		SettlPartyRole,

		/// <summary>
		/// SettlPartySubID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SettlPartySubID,

		/// <summary>
		/// SettlPartySubIDType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SettlPartySubIDType,

		/// <summary>
		/// DlvyInstType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dlvy")]
		DlvyInstType,

		/// <summary>
		/// TerminationType
		/// </summary>
		TerminationType,

		/// <summary>
		/// NextExpectedMsgSeqNum
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
		NextExpectedMsgSeqNum,

		/// <summary>
		/// OrdStatusReqID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ord")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		OrdStatusReqID,

		/// <summary>
		/// SettlInstReqID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SettlInstReqID,

		/// <summary>
		/// SettlInstReqRejCode
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rej")]
		SettlInstReqRejCode,

		/// <summary>
		/// SecondaryAllocID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SecondaryAllocID,

		/// <summary>
		/// AllocReportType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocReportType,

		/// <summary>
		/// AllocReportRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		AllocReportRefID,

		/// <summary>
		/// AllocCancReplaceReason
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Canc")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocCancReplaceReason,

		/// <summary>
		/// CopyMsgIndicator
		/// </summary>
		CopyMsgIndicator,

		/// <summary>
		/// AllocAccountType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocAccountType,

		/// <summary>
		/// OrderAvgPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Avg")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		OrderAvgPx,

		/// <summary>
		/// OrderBookingQty
		/// </summary>
		OrderBookingQty,

		/// <summary>
		/// NoSettlPartySubIDs
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IDs")]
		NoSettlPartySubIDs,

		/// <summary>
		/// NoPartySubIDs
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IDs")]
		NoPartySubIDs,

		/// <summary>
		/// PartySubIDType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		PartySubIDType,

		/// <summary>
		/// NoNestedPartySubIDs
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IDs")]
		NoNestedPartySubIDs,

		/// <summary>
		/// NestedPartySubIDType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		NestedPartySubIDType,

		/// <summary>
		/// NoNested2PartySubIDs
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IDs")]
		NoNested2PartySubIDs,

		/// <summary>
		/// Nested2PartySubIDType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		Nested2PartySubIDType,

		/// <summary>
		/// AllocIntermedReqType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Intermed")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocIntermedReqType,

		/// <summary>
		/// UnderlyingPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		UnderlyingPx,

		/// <summary>
		/// PriceDelta
		/// </summary>
		PriceDelta,

		/// <summary>
		/// ApplQueueMax
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Appl")]
		ApplQueueMax,

		/// <summary>
		/// ApplQueueDepth
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Appl")]
		ApplQueueDepth,

		/// <summary>
		/// ApplQueueResolution
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Appl")]
		ApplQueueResolution,

		/// <summary>
		/// ApplQueueAction
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Appl")]
		ApplQueueAction,

		/// <summary>
		/// NoAltMDSource
		/// </summary>
		NoAltMDSource,

		/// <summary>
		/// AltMDSourceID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		AltMDSourceID,

		/// <summary>
		/// SecondaryTradeReportID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SecondaryTradeReportID,

		/// <summary>
		/// AvgPxIndicator
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Avg")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		AvgPxIndicator,

		/// <summary>
		/// TradeLinkID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		TradeLinkID,

		/// <summary>
		/// OrderInputDevice
		/// </summary>
		OrderInputDevice,

		/// <summary>
		/// UnderlyingTradingSessionID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		UnderlyingTradingSessionID,

		/// <summary>
		/// UnderlyingTradingSessionSubID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		UnderlyingTradingSessionSubID,

		/// <summary>
		/// TradeLegRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		TradeLegRefID,

		/// <summary>
		/// ExchangeRule
		/// </summary>
		ExchangeRule,

		/// <summary>
		/// TradeAllocIndicator
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		TradeAllocIndicator,

		/// <summary>
		/// ExpirationCycle
		/// </summary>
		ExpirationCycle,

		/// <summary>
		/// TrdType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trd")]
		TrdType,

		/// <summary>
		/// TrdSubType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trd")]
		[SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "SubType")]
		TrdSubType,

		/// <summary>
		/// TransferReason
		/// </summary>
		TransferReason,

		/// <summary>
		/// AsgnReqID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Asgn")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		AsgnReqID,

		/// <summary>
		/// TotNumAssignmentReports
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
		TotNumAssignmentReports,

		/// <summary>
		/// AsgnRptID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Asgn")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		AsgnRptID,

		/// <summary>
		/// ThresholdAmount
		/// </summary>
		ThresholdAmount,

		/// <summary>
		/// PegMoveType
		/// </summary>
		PegMoveType,

		/// <summary>
		/// PegOffsetType
		/// </summary>
		PegOffsetType,

		/// <summary>
		/// PegLimitType
		/// </summary>
		PegLimitType,

		/// <summary>
		/// PegRoundDirection
		/// </summary>
		PegRoundDirection,

		/// <summary>
		/// PeggedPrice
		/// </summary>
		PeggedPrice,

		/// <summary>
		/// PegScope
		/// </summary>
		PegScope,

		/// <summary>
		/// DiscretionMoveType
		/// </summary>
		DiscretionMoveType,

		/// <summary>
		/// DiscretionOffsetType
		/// </summary>
		DiscretionOffsetType,

		/// <summary>
		/// DiscretionLimitType
		/// </summary>
		DiscretionLimitType,

		/// <summary>
		/// DiscretionRoundDirection
		/// </summary>
		DiscretionRoundDirection,

		/// <summary>
		/// DiscretionPrice
		/// </summary>
		DiscretionPrice,

		/// <summary>
		/// DiscretionScope
		/// </summary>
		DiscretionScope,

		/// <summary>
		/// TargetStrategy
		/// </summary>
		TargetStrategy,

		/// <summary>
		/// TargetStrategyParameters
		/// </summary>
		TargetStrategyParameters,

		/// <summary>
		/// ParticipationRate
		/// </summary>
		ParticipationRate,

		/// <summary>
		/// TargetStrategyPerformance
		/// </summary>
		TargetStrategyPerformance,

		/// <summary>
		/// LastLiquidityInd
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ind")]
		LastLiquidityInd,

		/// <summary>
		/// PublishTrdIndicator
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trd")]
		PublishTrdIndicator,

		/// <summary>
		/// ShortSaleReason
		/// </summary>
		ShortSaleReason,

		/// <summary>
		/// QtyType
		/// </summary>
		QtyType,

		/// <summary>
		/// SecondaryTrdType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trd")]
		SecondaryTrdType,

		/// <summary>
		/// TradeReportType
		/// </summary>
		TradeReportType,

		/// <summary>
		/// AllocNoOrdersType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alloc")]
		AllocNoOrdersType,

		/// <summary>
		/// SharedCommission
		/// </summary>
		SharedCommission,

		/// <summary>
		/// ConfirmReqID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		ConfirmReqID,

		/// <summary>
		/// AvgParPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Avg")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		AvgParPx,

		/// <summary>
		/// ReportedPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		ReportedPx,

		/// <summary>
		/// NoCapacities
		/// </summary>
		NoCapacities,

		/// <summary>
		/// OrderCapacityQty
		/// </summary>
		OrderCapacityQty,

		/// <summary>
		/// NoEvents
		/// </summary>
		NoEvents,

		/// <summary>
		/// EventType
		/// </summary>
		EventType,

		/// <summary>
		/// EventDate
		/// </summary>
		EventDate,

		/// <summary>
		/// EventPx
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Px")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Px")]
		EventPx,

		/// <summary>
		/// EventText
		/// </summary>
		EventText,

		/// <summary>
		/// PctAtRisk
		/// </summary>
		PctAtRisk,

		/// <summary>
		/// NoInstrAttrib
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Instr")]
		NoInstrAttrib,

		/// <summary>
		/// InstrAttribType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Instr")]
		InstrAttribType,

		/// <summary>
		/// InstrAttribValue
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Instr")]
		InstrAttribValue,

		/// <summary>
		/// DatedDate
		/// </summary>
		DatedDate,

		/// <summary>
		/// InterestAccrualDate
		/// </summary>
		InterestAccrualDate,

		/// <summary>
		/// CPProgram
		/// </summary>
		CPProgram,

		/// <summary>
		/// CPRegType
		/// </summary>
		CPRegType,

		/// <summary>
		/// UnderlyingCPProgram
		/// </summary>
		UnderlyingCPProgram,

		/// <summary>
		/// UnderlyingCPRegType
		/// </summary>
		UnderlyingCPRegType,

		/// <summary>
		/// UnderlyingQty
		/// </summary>
		UnderlyingQty,

		/// <summary>
		/// TrdMatchID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trd")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		TrdMatchID,

		/// <summary>
		/// SecondaryTradeReportRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		SecondaryTradeReportRefID,

		/// <summary>
		/// UnderlyingDirtyPrice
		/// </summary>
		UnderlyingDirtyPrice,

		/// <summary>
		/// UnderlyingEndPrice
		/// </summary>
		UnderlyingEndPrice,

		/// <summary>
		/// UnderlyingStartValue
		/// </summary>
		UnderlyingStartValue,

		/// <summary>
		/// UnderlyingCurrentValue
		/// </summary>
		UnderlyingCurrentValue,

		/// <summary>
		/// UnderlyingEndValue
		/// </summary>
		UnderlyingEndValue,

		/// <summary>
		/// NoUnderlyingStips
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Stips")]
		NoUnderlyingStips,

		/// <summary>
		/// UnderlyingStipType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Stip")]
		UnderlyingStipType,

		/// <summary>
		/// UnderlyingStipValue
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Stip")]
		UnderlyingStipValue,

		/// <summary>
		/// MaturityNetMoney
		/// </summary>
		MaturityNetMoney,

		/// <summary>
		/// MiscFeeBasis
		/// </summary>
		MiscFeeBasis,

		/// <summary>
		/// TotNoAllocs
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Allocs")]
		TotNoAllocs,

		/// <summary>
		/// LastFragment
		/// </summary>
		LastFragment,

		/// <summary>
		/// CollReqID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Req")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Coll")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		CollReqID,

		/// <summary>
		/// CollAsgnReason
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Coll")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Asgn")]
		CollAsgnReason,

		/// <summary>
		/// CollInquiryQualifier
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Coll")]
		CollInquiryQualifier,

		/// <summary>
		/// NoTrades
		/// </summary>
		NoTrades,

		/// <summary>
		/// MarginRatio
		/// </summary>
		MarginRatio,

		/// <summary>
		/// MarginExcess
		/// </summary>
		MarginExcess,

		/// <summary>
		/// TotalNetValue
		/// </summary>
		TotalNetValue,

		/// <summary>
		/// CashOutstanding
		/// </summary>
		CashOutstanding,

		/// <summary>
		/// CollAsgnID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Coll")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Asgn")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		CollAsgnID,

		/// <summary>
		/// CollAsgnTransType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Coll")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Asgn")]
		CollAsgnTransType,

		/// <summary>
		/// CollRespID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Resp")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Coll")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		CollRespID,

		/// <summary>
		/// CollAsgnRespType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Resp")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Coll")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Asgn")]
		CollAsgnRespType,

		/// <summary>
		/// CollAsgnRejectReason
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Coll")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Asgn")]
		CollAsgnRejectReason,

		/// <summary>
		/// CollAsgnRefID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Coll")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Asgn")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		CollAsgnRefID,

		/// <summary>
		/// CollRptID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Coll")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		CollRptID,

		/// <summary>
		/// CollInquiryID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Coll")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		CollInquiryID,

		/// <summary>
		/// CollStatus
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Coll")]
		CollStatus,

		/// <summary>
		/// TotNumReports
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
		TotNumReports,

		/// <summary>
		/// LastRptRequested
		/// </summary>
		LastRptRequested,

		/// <summary>
		/// AgreementDesc
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Desc")]
		AgreementDesc,

		/// <summary>
		/// AgreementID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		AgreementID,

		/// <summary>
		/// AgreementDate
		/// </summary>
		AgreementDate,

		/// <summary>
		/// StartDate
		/// </summary>
		StartDate,

		/// <summary>
		/// EndDate
		/// </summary>
		EndDate,

		/// <summary>
		/// AgreementCurrency
		/// </summary>
		AgreementCurrency,

		/// <summary>
		/// DeliveryType
		/// </summary>
		DeliveryType,

		/// <summary>
		/// EndAccruedInterestAmt
		/// </summary>
		EndAccruedInterestAmt,

		/// <summary>
		/// StartCash
		/// </summary>
		StartCash,

		/// <summary>
		/// EndCash
		/// </summary>
		EndCash,

		/// <summary>
		/// UserRequestID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		UserRequestID,

		/// <summary>
		/// UserRequestType
		/// </summary>
		UserRequestType,

		/// <summary>
		/// NewPassword
		/// </summary>
		NewPassword,

		/// <summary>
		/// UserStatus
		/// </summary>
		UserStatus,

		/// <summary>
		/// UserStatusText
		/// </summary>
		UserStatusText,

		/// <summary>
		/// StatusValue
		/// </summary>
		StatusValue,

		/// <summary>
		/// StatusText
		/// </summary>
		StatusText,

		/// <summary>
		/// RefCompID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		RefCompID,

		/// <summary>
		/// RefSubID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		RefSubID,

		/// <summary>
		/// NetworkResponseID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		NetworkResponseID,

		/// <summary>
		/// NetworkRequestID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		NetworkRequestID,

		/// <summary>
		/// LastNetworkResponseID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		LastNetworkResponseID,

		/// <summary>
		/// NetworkRequestType
		/// </summary>
		NetworkRequestType,

		/// <summary>
		/// NoCompIDs
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IDs")]
		NoCompIDs,

		/// <summary>
		/// NetworkStatusResponseType
		/// </summary>
		NetworkStatusResponseType,

		/// <summary>
		/// NoCollInquiryQualifier
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Coll")]
		NoCollInquiryQualifier,

		/// <summary>
		/// TrdRptStatus
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trd")]
		TrdRptStatus,

		/// <summary>
		/// AffirmStatus
		/// </summary>
		AffirmStatus,

		/// <summary>
		/// UnderlyingStrikeCurrency
		/// </summary>
		UnderlyingStrikeCurrency,

		/// <summary>
		/// LegStrikeCurrency
		/// </summary>
		LegStrikeCurrency,

		/// <summary>
		/// TimeBracket
		/// </summary>
		TimeBracket,

		/// <summary>
		/// CollAction
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Coll")]
		CollAction,

		/// <summary>
		/// CollInquiryStatus
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Coll")]
		CollInquiryStatus,

		/// <summary>
		/// CollInquiryResult
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Coll")]
		CollInquiryResult,

		/// <summary>
		/// StrikeCurrency
		/// </summary>
		StrikeCurrency,

		/// <summary>
		/// NoNested3PartyIDs
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IDs")]
		NoNested3PartyIDs,

		/// <summary>
		/// Nested3PartyID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		Nested3PartyID,

		/// <summary>
		/// Nested3PartyIDSource
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		Nested3PartyIDSource,

		/// <summary>
		/// Nested3PartyRole
		/// </summary>
		Nested3PartyRole,

		/// <summary>
		/// NoNested3PartySubIDs
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IDs")]
		NoNested3PartySubIDs,

		/// <summary>
		/// Nested3PartySubID
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		Nested3PartySubID,

		/// <summary>
		/// Nested3PartySubIDType
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
		Nested3PartySubIDType,

		/// <summary>
		/// LegContractSettlMonth
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Settl")]
		LegContractSettlMonth,

		/// <summary>
		/// LegInterestAccrualDate
		/// </summary>
		LegInterestAccrualDate,

		/// <summary>
		/// Tag6060
		/// </summary>
		Tag6060,

		/// <summary>
		/// Tag6061
		/// </summary>
		Tag6061,

		/// <summary>
		/// Tag6062
		/// </summary>
		Tag6062,

		/// <summary>
		/// Tag6063
		/// </summary>
		Tag6063,

		/// <summary>
		/// Tag6064
		/// </summary>
		Tag6064,

		/// <summary>
		/// Tag6065
		/// </summary>
		Tag6065,

		/// <summary>
		/// Tag6066
		/// </summary>
		Tag6066,

		/// <summary>
		/// Tag6067
		/// </summary>
		Tag6067,

		/// <summary>
		/// Tag6068
		/// </summary>
		Tag6068,

		/// <summary>
		/// Tag6069
		/// </summary>
		Tag6069,

		/// <summary>
		/// Tag8020
		/// </summary>
		Tag8020,

		/// <summary>
		/// Tag8021
		/// </summary>
		Tag8021,

		/// <summary>
		/// Tag8022
		/// </summary>
		Tag8022,

		/// <summary>
		/// Tag8023
		/// </summary>
		Tag8023,

		/// <summary>
		/// Tag8024
		/// </summary>
		Tag8024,

		/// <summary>
		/// Tag8025
		/// </summary>
		Tag8025,

		/// <summary>
		/// Tag8026
		/// </summary>
		Tag8026,

		/// <summary>
		/// Tag8027
		/// </summary>
		Tag8027,

		/// <summary>
		/// Tag8028
		/// </summary>
		Tag8028,

		/// <summary>
		/// Tag8029
		/// </summary>
		Tag8029,

		/// <summary>
		/// Tag9426
		/// </summary>
		Tag9426,

		/// <summary>
		/// CmsCxlQty
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cxl")]
		CmsCxlQty,

		/// <summary>
		/// CmsLeavesQty
		/// </summary>
		CmsLeavesQty,

		/// <summary>
		/// Tag9890
		/// </summary>
		Tag9890,

		/// <summary>
		/// Tag9891
		/// </summary>
		Tag9891,

		/// <summary>
		/// Tag9892
		/// </summary>
		Tag9892,

		/// <summary>
		/// Tag9893
		/// </summary>
		Tag9893,

		/// <summary>
		/// Tag9894
		/// </summary>
		Tag9894,

		/// <summary>
		/// Tag9895
		/// </summary>
		Tag9895,

		/// <summary>
		/// Tag9896
		/// </summary>
		Tag9896,

		/// <summary>
		/// Tag9897
		/// </summary>
		Tag9897,

		/// <summary>
		/// Tag9898
		/// </summary>
		Tag9898,

		/// <summary>
		/// Tag9899
		/// </summary>
		Tag9899,

		/// <summary>
		/// Tag9980
		/// </summary>
		Tag9980,

		/// <summary>
		/// Tag9981
		/// </summary>
		Tag9981,

		/// <summary>
		/// Tag9982
		/// </summary>
		Tag9982,

		/// <summary>
		/// Tag9983
		/// </summary>
		Tag9983,

		/// <summary>
		/// Tag9984
		/// </summary>
		Tag9984,

		/// <summary>
		/// Tag9985
		/// </summary>
		Tag9985,

		/// <summary>
		/// Tag9986
		/// </summary>
		Tag9986,

		/// <summary>
		/// Tag9987
		/// </summary>
		Tag9987,

		/// <summary>
		/// Tag9988
		/// </summary>
		Tag9988,

		/// <summary>
		/// Tag9989
		/// </summary>
		Tag9989,

		/// <summary>
		/// InternalSourceId
		/// </summary>
		InternalSourceId,

		/// <summary>
		/// InternalRecordId
		/// </summary>
		InternalRecordId,

		/// <summary>
		/// InternalError
		/// </summary>
		InternalError,

		/// <summary>
		/// RoutingGroup
		/// </summary>
		RoutingGroup,

		/// <summary>
		/// IoiQualifierGroup
		/// </summary>
		IoiQualifierGroup

	}

}
