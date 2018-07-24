namespace Teraque
{

	using System;

	/// <summary>
	/// Provides a product code from the object.
	/// </summary>
	public interface IProductId
	{

		/// <summary>
		/// A unique identifier of the product.
		/// </summary>
		Guid ProductId { get; }

	}

}
