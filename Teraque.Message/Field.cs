namespace Teraque
{

	using System;

	/// <summary>
	/// A Tag and Value pair.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class Field
	{

		/// <summary>
		/// Describes the function of the value in this field.
		/// </summary>
		public Tag Tag;

		/// <summary>
		/// A generic value that has no meaning without the tag.
		/// </summary>
		public Object Value;

		/// <summary>
		/// Creates a Tag and Value pair.
		/// </summary>
		/// <param name="tag">Describes the function of the value in this field.</param>
		/// <param name="value">A generic value that has no meaning without the tag.</param>
		public Field(Tag tag, Object value)
		{

			// Initialize the object
			this.Tag = tag;
			this.Value = value;

		}

	}

}
