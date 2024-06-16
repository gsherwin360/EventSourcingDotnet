using Domain.Primitives;

namespace Application.Products.Errors;

public static class ProductErrors
{
	public static Error NotFound => new Error(
		"ProductNotFound",
		"The product does not exist.");
}