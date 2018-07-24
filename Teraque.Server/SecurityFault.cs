
namespace Teraque
{

	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Indicates a user's credentials aren't suitable for the current operation.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[DataContract]
	public class SecurityFault
	{

		/// <summary>
		/// The message of the fault.
		/// </summary>
		[DataMember]
		public String Message { get; private set; }

		/// <summary>
		/// Initialize a new instance of the SecurityFault class.
		/// </summary>
		/// <param name="message">The message of the fault.</param>
		public SecurityFault(String message)
		{

			// Initialize the object.
			this.Message = message;

		}

	}

}
