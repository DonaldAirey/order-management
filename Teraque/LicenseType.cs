namespace Teraque
{

	using System;

	/// <summary>
	/// Describes how a license is managed by the client.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public static class LicenseType
	{

		/// <summary>
		/// This is not a valid license.  Used for testing.
		/// </summary>
		public const Byte NotValid = 0x00;

		/// <summary>
		/// Evaluation License that reminds the user to register.
		/// </summary>
		public const Byte EvaluationNag = 0x01;

		/// <summary>
		/// Evaluation 1 Month License.
		/// </summary>
		public const Byte Evaluation1Month = 0x02;

		/// <summary>
		/// Evaluation 2 Month License.
		/// </summary>
		public const Byte Evaluation2Month = 0x03;

		/// <summary>
		/// Evaluation 3 Month License.
		/// </summary>
		public const Byte Evaluation3Month = 0x04;

		/// <summary>
		/// Evaluation 4 Month License.
		/// </summary>
		public const Byte Evaluation4Month = 0x05;

		/// <summary>
		/// Evaluation 5 Month License.
		/// </summary>
		public const Byte Evaluation5Month = 0x06;

		/// <summary>
		/// Evaluation 6 Month License.
		/// </summary>
		public const Byte Evaluation6Month = 0x07;

		/// <summary>
		/// Full 1 Year License.
		/// </summary>
		public const Byte Full1Year = 0x08;

		/// <summary>
		/// Full 2 Year License.
		/// </summary>
		public const Byte Full2Year = 0x09;

		/// <summary>
		/// Full 3 Year License.
		/// </summary>
		public const Byte Full3Year = 0x0A;

		/// <summary>
		/// Full 4 Year License.
		/// </summary>
		public const Byte Full4Year = 0x0B;

		/// <summary>
		/// Full 5 Year License.
		/// </summary>
		public const Byte Full5Year = 0x0C;

		/// <summary>
		/// Perpetual License.
		/// </summary>
		public const Byte Perpetual = 0x0D;

	}

}
