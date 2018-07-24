namespace Teraque
{

	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Status codes for an order.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum StatusCode
	{

		/// <summary>
		/// Active on the crossing network.
		/// </summary>
		Active,

		/// <summary>
		/// Accepted for negotiations.
		/// </summary>
		Accepted,

		/// <summary>
		/// Accepted for bidding.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ForBidding")]
		AcceptedForBidding,

		/// <summary>
		/// The order is canceled.
		/// </summary>
		Canceled,

		/// <summary>
		/// The order is closed.
		/// </summary>
		Closed,

		/// <summary>
		/// The order has been confirmed by both sides.
		/// </summary>
		Confirmed,

		/// <summary>
		/// The transaction was declined.
		/// </summary>
		Declined,

		/// <summary>
		/// The object has been deleted.
		/// </summary>
		Deleted,

		/// <summary>
		/// Done for the day.
		/// </summary>
		DoneForDay,

		/// <summary>
		/// The order is in an error state.
		/// </summary>
		Error,

		/// <summary>
		/// An empty object.
		/// </summary>
		Empty,

		/// <summary>
		/// The order has expired.
		/// </summary>
		Expired,

		/// <summary>
		/// The order has been filled.
		/// </summary>
		Filled,

		/// <summary>
		/// The order is locked.
		/// </summary>
		Locked,

		/// <summary>
		/// There are negotiations in progress.
		/// </summary>
		Negotiating,

		/// <summary>
		/// The order is new.
		/// </summary>
		New,

		/// <summary>
		/// The offer has been accepted.
		/// </summary>
		OfferAccepted,

		/// <summary>
		/// The order is open.
		/// </summary>
		Open,

		/// <summary>
		/// The order is pending.
		/// </summary>
		Pending,

		/// <summary>
		/// The object is pending a cancel verification.
		/// </summary>
		PendingCancel,

		/// <summary>
		/// The order is awaing a new verification.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
		PendingNew,

		/// <summary>
		/// The order is awainging verification of a replacement order.
		/// </summary>
		PendingReplace,

		/// <summary>
		/// The order is partially filled.
		/// </summary>
		PartiallyFilled,

		/// <summary>
		/// The order has been partially matched.
		/// </summary>
		PartialMatch,

		/// <summary>
		/// The order was rejected.
		/// </summary>
		Rejected,

		/// <summary>
		/// The order was replaced.
		/// </summary>
		Replaced,

		/// <summary>
		/// The order has been stopped.
		/// </summary>
		Stopped,

		/// <summary>
		/// The order has been submitted.
		/// </summary>
		Submitted,

		/// <summary>
		/// The order has been suspended.
		/// </summary>
		Suspended,

		/// <summary>
		/// The order is a match to a counterparty order.
		/// </summary>
		ValidMatch,

		/// <summary>
		/// The order is a match to a counterparty order and has matching funds.
		/// </summary>
		ValidMatchFunds

	}

}
