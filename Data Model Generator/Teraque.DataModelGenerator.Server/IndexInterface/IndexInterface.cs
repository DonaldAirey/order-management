namespace Teraque.DataModelGenerator.IndexInterface
{

	using System;
	using System.CodeDom;
	using System.Reflection;

	/// <summary>
	/// Creates a CodeDOM description of a strongly typed index.
	/// </summary>
	/// <copyright>Copyright � 2010-2012 - Teraque, Inc.  All Rights Reserved.</copyright>
	public class IndexInterface : CodeTypeDeclaration
	{

		/// <summary>
		/// Creates a CodeDOM description of a strongly typed index.
		/// </summary>
		/// <param name="tableSchema">The description of a table.</param>
		public IndexInterface(TableSchema tableSchema)
		{

			//    /// <summary>
			//    /// Represents a means of identifying a Gender row using a set of columns in which all values must be unique.
			//    /// </summary>
			//    public interface IGenderIndex {
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Represents a means of identifying a {0} row using a set of columns in which all values must be unique.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Name = String.Format("I{0}Index", tableSchema.Name);
			this.TypeAttributes = TypeAttributes.Public | TypeAttributes.Interface;

			// Methods
			this.Members.Add(new FindByKeyMethod(tableSchema));

			//    }

		}

	}

}
