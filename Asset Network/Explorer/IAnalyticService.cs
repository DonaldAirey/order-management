namespace Teraque.AssetNetwork
{

	using System;
	using System.Collections.Generic;
	using System.Runtime.Serialization;
	using System.ServiceModel;

	/// <summary>
	/// Web Service Contract
	/// </summary>
	/// <copyright>Copyright © 2010-2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	[ServiceContract]
	public interface IAnalyticService
	{

		/// <summary>
		/// Peforms a generic query against the data model.
		/// </summary>
		[OperationContract]
		[ServiceKnownType(typeof(Object[]))]
		[ServiceKnownType(typeof(Object[][]))]
		Object GenericQuery(String query, Object[] arguments);

		/// <summary>
		/// Peforms a generic query against the data model.
		/// </summary>
		[OperationContract]
		[ServiceKnownType(typeof(Object[]))]
		[ServiceKnownType(typeof(Object[][]))]
		Object IndustryWeightQuery(String accountMnemonic, DateTime date, String schemeMnemonic);

		[OperationContract]
		Object QualityWeightQuery(String accountMnemonic, DateTime date, Int32 ratingScoreLow, Int32 ratingScoreHigh, Boolean isLong);

		[OperationContract]
		[ServiceKnownType(typeof(Object[]))]
		[ServiceKnownType(typeof(Object[][]))]
		Object SectorByWeightQuery(String accountMnemonic, DateTime date, String schemeMnemonic);

		[OperationContract]
		Object SectorWeightQuery(String accountMnemonic, DateTime date, String industryMnemonic, Boolean isLong, Boolean isDerivative);

		[OperationContract]
		Object SectorWeightedReturnQuery(String accountMnemonic, DateTime date, String industryMnemonic, Boolean isDerivative);

	}

}
