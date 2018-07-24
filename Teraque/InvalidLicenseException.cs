namespace Teraque
{

	using System;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Runtime.Serialization;

	/// <summary>
	/// Represents the exception thrown when a component cannot be granted a license due to the evaluation period expiring.
	/// </summary>
	[Serializable]
	[SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
	public class InvalidLicenseException : LicenseException
	{

		/// <summary>
		/// The instance of the component that was not granted a license.
		/// </summary>
		Object instance;

		/// <summary>
		/// Initializes a new instance of the LicenseException class for the type and the instance of the component that was denied a license.
		/// </summary>
		/// <param name="instance">The instance of the component that was not granted a license.</param>
		public InvalidLicenseException(Object instance)
			: this(instance, ExceptionMessage.Format(Properties.ExceptionMessages.InvalidLicense, InvalidLicenseException.GetType(instance))) { }

		/// <summary>
		/// Initializes a new instance of the LicenseException class for the type and the instance of the component that was denied a license, along with a message
		/// to display.
		/// </summary>
		/// <param name="instance">The instance of the component that was not granted a license.</param>
		/// <param name="message">The exception message to display.</param>
		public InvalidLicenseException(Object instance, String message) : base(InvalidLicenseException.GetType(instance), instance, message)
		{

			// Initialize the object.
			this.instance = instance;
		
		}

		/// <summary>
		/// Initializes a new instance of the LicenseException class with the given SerializationInfo and StreamingContext.
		/// </summary>
		/// <param name="serializationInfo">The SerializationInfo to be used for deserialization.</param>
		/// <param name="streamingContext">The destination to be used for deserialization.</param>
		protected InvalidLicenseException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

		/// <summary>
		/// The instance of the component that was not granted a license.
		/// </summary>
		public Object Instance
		{
			get
			{
				return this.instance;
			}
		}

		/// <summary>
		/// Sets the SerializationInfo with information about the exception.
		/// </summary>
		/// <param name="info">The SerializationInfo to be used for deserialization.</param>
		/// <param name="context">The destination to be used for deserialization.</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{

			// Alow the base class to serials the bulk of the exception.
			base.GetObjectData(info, context);

			// This will add the properties that are specific to this class.
			info.AddValue("Instance", this.instance);

		}

		/// <summary>
		/// Validates and gets the type of the given instance.
		/// </summary>
		/// <param name="instance">The instance of the object that failed license validation.</param>
		/// <returns>The type of the given instance.</returns>
		protected static Type GetType(Object instance)
		{

			// Validate the arguments
			if (instance == null)
				throw new ArgumentNullException("instance");

			// Get the type.
			return instance.GetType();

		}

	}

}
