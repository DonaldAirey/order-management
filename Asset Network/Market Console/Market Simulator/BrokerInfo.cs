namespace Teraque.AssetNetwork
{

	using System;

	/// <summary>
	/// Information about a broker.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	internal class BrokerInfo
	{

		/// <summary>
		/// The broker's name.
		/// </summary>
		String nameField;

		/// <summary>
		/// The broker's symbol.
		/// </summary>
		String symbolField;

		/// <summary>
		/// The broker's name.
		/// </summary>
		public String Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}

		/// <summary>
		/// The broker's symbol.
		/// </summary>
		public String Symbol
		{
			get
			{
				return this.symbolField;
			}
			set
			{
				this.symbolField = value;
			}
		}

	}

}
