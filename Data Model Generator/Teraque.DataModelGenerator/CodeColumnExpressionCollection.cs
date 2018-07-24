namespace Teraque.DataModelGenerator
{

    using System.CodeDom;

	public class CodeColumnExpressionCollection : CodeExpressionCollection
	{

		public CodeColumnExpressionCollection(ColumnSchema[] columns)
		{

			foreach (ColumnSchema columnSchema in columns)
			{
				this.Add(new CodeArgumentReferenceExpression(CommonConversion.ToCamelCase(columnSchema.Name)));
			}

		}

	}



}
