namespace Teraque
{

    using System;
    using System.Runtime.Serialization;

	/// <summary>
	/// A fault that occurs when the arguments don't meet the expected values.
	/// </summary>
	/// <copyright>Copyright � 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[DataContract]
	public class ArgumentFault
	{

		/// <summary>
		/// The message of the fault.
		/// </summary>
		[DataMember]
		public String Message { get; private set; }

		/// <summary>
		/// Initialize a new instance of the ArgumentFault class.
		/// </summary>
		/// <param name="message">The message of the fault.</param>
		public ArgumentFault(String message)
		{

			// Initialize the object.
			this.Message = message;

		}

	}

}
