namespace Teraque.AssetNetwork
{

	using System;

	/// <summary>
	/// A dynamically created WCF communication channel.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	class ClientChannel
	{

		/// <summary>
		/// The channel object.
		/// </summary>
		public Object ChannelObject { get; private set; }

		/// <summary>
		/// The channel type.
		/// </summary>
		public Type ChannelType { get; private set; }

		/// <summary>
		/// Initializes a new instance of ClientChannel class.
		/// </summary>
		/// <param name="channelObject">The channel instance.</param>
		/// <param name="channelType">The channel type.</param>
		public ClientChannel(Object channelObject, Type channelType)
		{

			// Initialize the object
			this.ChannelObject = channelObject;
			this.ChannelType = channelType;

		}

	}

}
