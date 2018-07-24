namespace Teraque
{

	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// A generic fault.
	/// </summary>
	[DataContract]
	public class ExceptionFault
	{

		/// <summary>
		/// The exception.
		/// </summary>
		[DataMember]
		Exception exceptionField;

		/// <summary>
		/// Creates a new instance of a GenericFault class.
		/// </summary>
		/// <param name="exception">The inner exception.</param>
		public ExceptionFault(Exception exception)
		{

			// Initialize the object.
			this.exceptionField = exception;

		}

		/// <summary>
		/// Get the exception.
		/// </summary>
		public Exception Exception
		{
			get
			{
				return this.exceptionField;
			}
		}

	}

}
