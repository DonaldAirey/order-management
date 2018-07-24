namespace Teraque
{

	/// <summary>
	/// Classification of parties to a transaction.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public enum PartyTypeCode
	{

		/// <summary>
		/// A broker that acts only as an agent.
		/// </summary>
		Agency,

		/// <summary>
		/// A broker that acts as a principal or agent.
		/// </summary>
		Broker,

		/// <summary>
		/// A Hedge Fund.
		/// </summary>
		Hedge,

        /// <summary>
        /// An institutional money manager.
        /// </summary>
        Institution,

		/// <summary>
		/// Not a valid counterparty.
		/// </summary>
		NotValid,

		/// <summary>
		/// Use the parent in a hierarchy of party types.
		/// </summary>
		UseParent

	}

}
