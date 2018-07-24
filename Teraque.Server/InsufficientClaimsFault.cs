namespace Teraque
{

	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// A fault that occurs when the user doesn't possess the required claims.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[DataContract]
	public class InsufficientClaimsFault
	{

		/// <summary>
		/// The message of the fault.
		/// </summary>
		[DataMember]
		String messageField;

		/// <summary>
		/// Initialize a new instance of the ArgumentFault class.
		/// </summary>
		/// <param name="message">The message of the fault.</param>
		public InsufficientClaimsFault(String message)
		{

			// Initialize the object.
			this.messageField = message;

		}

		/// <summary>
		/// Gets the message of the fault.
		/// </summary>
		public String Message
		{
			get
			{
				return this.messageField;
			}
		}

	}

}
