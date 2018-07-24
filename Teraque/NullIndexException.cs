namespace Teraque
{

	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Represents errors that occur calling the unmanaged Win32 libraries.
	/// </summary>
	[Serializable]
	public class NullIndexException : Exception
	{

		/// <summary>
		/// Initializes a new instance of the NullIndexException class.
		/// </summary>
		public NullIndexException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the Win32Exception class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public NullIndexException(String message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the Exception class with a specified error message and a reference to the inner exception that is the cause of this
		/// exception.
		/// </summary>
		/// <param name="message">The message that gives more information about the Win32 error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner
		/// exception is specified.</param>
		public NullIndexException(String message, Exception innerException) :
			base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the Exception class with serialized data.
		/// </summary>
		/// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
		protected NullIndexException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

	}

}
