namespace Teraque
{

	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Represents errors that occur when locking records for a transaction.
	/// </summary>
	[Serializable]
	public class LockException : Exception
	{

		/// <summary>
		/// Initializes a new instance of the LockException class.
		/// </summary>
		public LockException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the LockException class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public LockException(String message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the LockException class.
		/// </summary>
		/// <param name="message">The message that gives more information about the Win32 error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner
		/// exception is specified.</param>
		public LockException(String message, Exception innerException) :
			base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the LockException class.
		/// </summary>
		/// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
		protected LockException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

	}

}
