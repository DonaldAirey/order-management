namespace Teraque
{

    using System;
    using System.Runtime.Serialization;

	/// <summary>
	/// A fault that occurs when the format isn't correct.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[DataContract]
	public class FormatFault
	{

		/// <summary>
		/// The message of the fault.
		/// </summary>
		[DataMember]
		public String Message { get; private set; }

		/// <summary>
		/// Initialze a new instance of the FormatFault class.
		/// </summary>
		/// <param name="message">The message of the fault.</param>
		public FormatFault(String message)
		{

			// Initialize the object.
			this.Message = message;

		}

	}

}
