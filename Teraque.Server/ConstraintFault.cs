namespace Teraque
{

	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// A fault that occurs when an update is made to an already updated record.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	[DataContract]
	public class ConstraintFault
	{

		/// <summary>
		/// The constraint violation message.
		/// </summary>
		[DataMember]
		String messageField;

		/// <summary>
		/// Initialize a new instance of the ConstraintFault class.
		/// </summary>
		/// <param name="message">The constraint violation message.</param>
		public ConstraintFault(String message)
		{

			// Initialize the object
			this.messageField = message;

		}

		/// <summary>
		/// Gets the constraint violation message.
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
