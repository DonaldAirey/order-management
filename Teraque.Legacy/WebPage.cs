namespace Teraque
{

	using System;

    /// <summary>
	/// Summary description for WebPage.
	/// </summary>
	public class WebPage
	{

		public readonly System.Uri Uri;

		public WebPage(params object[] parameter)
		{

			try
			{

				// Initialize the object.
				this.Uri = new System.Uri((string)parameter[0]);

			}
			catch (Exception exception)
			{

				Log.Error("{0}, {1}", exception.Message, exception.StackTrace);

			}

		}

	}

}
