namespace Teraque.AssetNetwork
{

	using System;
	using Teraque.AssetNetwork.Properties;
	using Teraque.AssetNetwork.WebService;

	internal class UserInfo
	{

		/// <summary>
		/// Gets the user's unique identifier.
		/// </summary>
		public static Guid UserId
		{

			get
			{

				Guid userId;

				while (true)
				{
					try
					{
						using (WebServiceClient webServiceClient = new WebServiceClient(Settings.Default.WebServiceEndpoint))
						{
							userId = webServiceClient.GetUserId();
							break;
						}
					}
					catch { }
				}

				return userId;

			}

		}

	}

}
