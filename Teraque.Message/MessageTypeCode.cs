namespace Teraque
{

	/// <summary>
	/// Defines message type.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum MsgType
	{

		/// <summary>
		/// Heartbeat
		/// </summary>
		Heartbeat,

		/// <summary>
		/// TestRequest
		/// </summary>
		TestRequest,

		/// <summary>
		/// ResendRequest
		/// </summary>
		ResendRequest,

		/// <summary>
		/// Reject
		/// </summary>
		Reject,

		/// <summary>
		/// SequenceReset
		/// </summary>
		SequenceReset,

		/// <summary>
		/// LogOff
		/// </summary>
		LogOff,

		/// <summary>
		/// IOI
		/// </summary>
		IOI,

		/// <summary>
		/// Advertisement
		/// </summary>
		Advertisement,

		/// <summary>
		/// ExecutionReport
		/// </summary>
		ExecutionReport,

		/// <summary>
		/// OrderCancelReject
		/// </summary>
		OrderCancelReject,

		/// <summary>
		/// DerivativeSecurityList
		/// </summary>
		DerivativeSecurityList,

		/// <summary>
		/// NewOrderMultileg
		/// </summary>
		NewOrderMultileg,

		/// <summary>
		/// MultilegOrderCancelReplace
		/// </summary>
		MultilegOrderCancelReplace,

		/// <summary>
		/// TradeCaptureReportRequest
		/// </summary>
		TradeCaptureReportRequest,

		/// <summary>
		/// TradeCaptureReport
		/// </summary>
		TradeCaptureReport,

		/// <summary>
		/// OrderMassStatusRequest
		/// </summary>
		OrderMassStatusRequest,

		/// <summary>
		/// QuoteRequestReject
		/// </summary>
		QuoteRequestReject,

		/// <summary>
		/// RFQRequest
		/// </summary>
		RFQRequest,

		/// <summary>
		/// QuoteStatusReport
		/// </summary>
		QuoteStatusReport,

		/// <summary>
		/// QuoteResponse
		/// </summary>
		QuoteResponse,

		/// <summary>
		/// Confirmation
		/// </summary>
		Confirmation,

		/// <summary>
		/// PositionMaintenanceRequest
		/// </summary>
		PositionMaintenanceRequest,

		/// <summary>
		/// PositionMaintenanceReport
		/// </summary>
		PositionMaintenanceReport,

		/// <summary>
		/// RequestForPositions
		/// </summary>
		RequestForPositions,

		/// <summary>
		/// RequestForPositionsAck
		/// </summary>
		RequestForPositionsAck,

		/// <summary>
		/// PositionReport
		/// </summary>
		PositionReport,

		/// <summary>
		/// TradeCaptureReportRequestAck
		/// </summary>
		TradeCaptureReportRequestAck,

		/// <summary>
		/// TradeCaptureReportAck
		/// </summary>
		TradeCaptureReportAck,

		/// <summary>
		/// AllocationReport
		/// </summary>
		AllocationReport,

		/// <summary>
		/// AllocationReportAck
		/// </summary>
		AllocationReportAck,

		/// <summary>
		/// ConfirmationAck
		/// </summary>
		ConfirmationAck,

		/// <summary>
		/// SettlementInstructionRequest
		/// </summary>
		SettlementInstructionRequest,

		/// <summary>
		/// AssignmentReport
		/// </summary>
		AssignmentReport,

		/// <summary>
		/// CollateralRequest
		/// </summary>
		CollateralRequest,

		/// <summary>
		/// CollateralAssignment
		/// </summary>
		CollateralAssignment,

		/// <summary>
		/// CollateralResponse
		/// </summary>
		CollateralResponse,

		/// <summary>
		/// Logon
		/// </summary>
		Logon,

		/// <summary>
		/// CollateralReport
		/// </summary>
		CollateralReport,

		/// <summary>
		/// CollateralInquiry
		/// </summary>
		CollateralInquiry,

		/// <summary>
		/// NetworkCounterpartySystemStatusRequest
		/// </summary>
		NetworkCounterpartySystemStatusRequest,

		/// <summary>
		/// NetworkCounterpartySystemStatusResponse
		/// </summary>
		NetworkCounterpartySystemStatusResponse,

		/// <summary>
		/// UserRequest
		/// </summary>
		UserRequest,

		/// <summary>
		/// UserResponse
		/// </summary>
		UserResponse,

		/// <summary>
		/// CollateralInquiryAck
		/// </summary>
		CollateralInquiryAck,

		/// <summary>
		/// ConfirmationRequest
		/// </summary>
		ConfirmationRequest,

		/// <summary>
		/// TradingSessionListRequest
		/// </summary>
		TradingSessionListRequest,

		/// <summary>
		/// TradingSessionList
		/// </summary>
		TradingSessionList,

		/// <summary>
		/// SecurityListUpdateReport
		/// </summary>
		SecurityListUpdateReport,

		/// <summary>
		/// AdjustedPositionReport
		/// </summary>
		AdjustedPositionReport,

		/// <summary>
		/// AllocationInstructionAlert
		/// </summary>
		AllocationInstructionAlert,

		/// <summary>
		/// ExecutionAcknowledgement
		/// </summary>
		ExecutionAcknowledgement,

		/// <summary>
		/// ContraryIntentionReport
		/// </summary>
		ContraryIntentionReport,

		/// <summary>
		/// SecurityDefinitionUpdateReport
		/// </summary>
		SecurityDefinitionUpdateReport,

		/// <summary>
		/// SettlementObligationReport
		/// </summary>
		SettlementObligationReport,

		/// <summary>
		/// DerivativeSecurityListUpdateReport
		/// </summary>
		DerivativeSecurityListUpdateReport,

		/// <summary>
		/// TradingSessionListUpdateReport
		/// </summary>
		TradingSessionListUpdateReport,

		/// <summary>
		/// MarketDefinitionRequest
		/// </summary>
		MarketDefinitionRequest,

		/// <summary>
		/// MarketDefinition
		/// </summary>
		MarketDefinition,

		/// <summary>
		/// MarketDefinitionUpdateReport
		/// </summary>
		MarketDefinitionUpdateReport,

		/// <summary>
		/// ApplicationMessageRequest
		/// </summary>
		ApplicationMessageRequest,

		/// <summary>
		/// ApplicationMessageRequestAck
		/// </summary>
		ApplicationMessageRequestAck,

		/// <summary>
		/// ApplicationMessageReport
		/// </summary>
		ApplicationMessageReport,

		/// <summary>
		/// OrderMassActionReport
		/// </summary>
		OrderMassActionReport,

		/// <summary>
		/// News
		/// </summary>
		News,

		/// <summary>
		/// OrderMassActionRequest
		/// </summary>
		OrderMassActionRequest,

		/// <summary>
		/// UserNotification
		/// </summary>
		UserNotification,

		/// <summary>
		/// StreamAssignmentRequest
		/// </summary>
		StreamAssignmentRequest,

		/// <summary>
		/// StreamAssignmentReport
		/// </summary>
		StreamAssignmentReport,

		/// <summary>
		/// StreamAssignmentReportACK
		/// </summary>
		StreamAssignmentReportACK,

		/// <summary>
		/// Email
		/// </summary>
		Email,

		/// <summary>
		/// NewOrderSingle
		/// </summary>
		NewOrderSingle,

		/// <summary>
		/// NewOrderList
		/// </summary>
		NewOrderList,

		/// <summary>
		/// OrderCancelRequest
		/// </summary>
		OrderCancelRequest,

		/// <summary>
		/// OrderCancelReplaceRequest
		/// </summary>
		OrderCancelReplaceRequest,

		/// <summary>
		/// OrderStatusRequest
		/// </summary>
		OrderStatusRequest,

		/// <summary>
		/// AllocationInstruction
		/// </summary>
		AllocationInstruction,

		/// <summary>
		/// ListCancelRequest
		/// </summary>
		ListCancelRequest,

		/// <summary>
		/// ListExecute
		/// </summary>
		ListExecute,

		/// <summary>
		/// ListStatusRequest
		/// </summary>
		ListStatusRequest,

		/// <summary>
		/// ListStatus
		/// </summary>
		ListStatus,

		/// <summary>
		/// AllocationInstructionAck
		/// </summary>
		AllocationInstructionAck,

		/// <summary>
		/// DontKnowTrade
		/// </summary>
		DontKnowTrade,

		/// <summary>
		/// QuoteRequest
		/// </summary>
		QuoteRequest,

		/// <summary>
		/// Quote
		/// </summary>
		Quote,

		/// <summary>
		/// SettlementInstructions
		/// </summary>
		SettlementInstructions,

		/// <summary>
		/// MarketDataRequest
		/// </summary>
		MarketDataRequest,

		/// <summary>
		/// MarketDataSnapshotFullRefresh
		/// </summary>
		MarketDataSnapshotFullRefresh,

		/// <summary>
		/// MarketDataIncrementalRefresh
		/// </summary>
		MarketDataIncrementalRefresh,

		/// <summary>
		/// MarketDataRequestReject
		/// </summary>
		MarketDataRequestReject,

		/// <summary>
		/// QuoteCancel
		/// </summary>
		QuoteCancel,

		/// <summary>
		/// QuoteStatusRequest
		/// </summary>
		QuoteStatusRequest,

		/// <summary>
		/// MassQuoteAcknowledgement
		/// </summary>
		MassQuoteAcknowledgement,

		/// <summary>
		/// SecurityDefinitionRequest
		/// </summary>
		SecurityDefinitionRequest,

		/// <summary>
		/// SecurityDefinition
		/// </summary>
		SecurityDefinition,

		/// <summary>
		/// SecurityStatusRequest
		/// </summary>
		SecurityStatusRequest,

		/// <summary>
		/// SecurityStatus
		/// </summary>
		SecurityStatus,

		/// <summary>
		/// TradingSessionStatusRequest
		/// </summary>
		TradingSessionStatusRequest,

		/// <summary>
		/// TradingSessionStatus
		/// </summary>
		TradingSessionStatus,

		/// <summary>
		/// MassQuote
		/// </summary>
		MassQuote,

		/// <summary>
		/// BusinessMessageReject
		/// </summary>
		BusinessMessageReject,

		/// <summary>
		/// BidRequest
		/// </summary>
		BidRequest,

		/// <summary>
		/// BidResponse
		/// </summary>
		BidResponse,

		/// <summary>
		/// ListStrikePrice
		/// </summary>
		ListStrikePrice,

		/// <summary>
		/// XMLnonFIX
		/// </summary>
		XMLnonFIX,

		/// <summary>
		/// RegistrationInstructions
		/// </summary>
		RegistrationInstructions,

		/// <summary>
		/// RegistrationInstructionsResponse
		/// </summary>
		RegistrationInstructionsResponse,

		/// <summary>
		/// OrderMassCancelRequest
		/// </summary>
		OrderMassCancelRequest,

		/// <summary>
		/// OrderMassCancelReport
		/// </summary>
		OrderMassCancelReport,

		/// <summary>
		/// NewOrderCross
		/// </summary>
		NewOrderCross,

		/// <summary>
		/// CrossOrderCancelReplaceRequest
		/// </summary>
		CrossOrderCancelReplaceRequest,

		/// <summary>
		/// CrossOrderCancelRequest
		/// </summary>
		CrossOrderCancelRequest,

		/// <summary>
		/// SecurityTypeRequest
		/// </summary>
		SecurityTypeRequest,

		/// <summary>
		/// SecurityTypes
		/// </summary>
		SecurityTypes,

		/// <summary>
		/// SecurityListRequest
		/// </summary>
		SecurityListRequest,

		/// <summary>
		/// SecurityList
		/// </summary>
		SecurityList,

		/// <summary>
		/// DerivativeSecurityListRequest
		/// </summary>
		DerivativeSecurityListRequest

	}

}
