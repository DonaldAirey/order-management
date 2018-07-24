namespace Teraque
{

	using System;
	using System.ServiceModel.Configuration;

	/// <summary>
	/// Represents a configuration element that configures a client credential.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class PromptedClientCredentialsElement : ClientCredentialsElement
	{

		/// <summary>
		/// Gets the type of this behavior element.
		/// </summary>
		public override Type BehaviorType
		{
			get { return typeof(PromptedClientCredentials); }
		}

		/// <summary>
		/// Creates a custom behavior based on the settings of this configuration element.
		/// </summary>
		/// <returns>A custom behavior based on the settings of this configuration element.</returns>
		protected override object CreateBehavior()
		{

			// This custom behavior will prompt the user for credentials before using a channel.
			PromptedClientCredentials customClientCredentials = new PromptedClientCredentials();
			base.ApplyConfiguration(customClientCredentials);
			return customClientCredentials;

		}

	}

}
