using System.ComponentModel.DataAnnotations;

namespace Presentation.Api.Models;

public class CreateProductModel
{
	[Required]
	public string Name { get; set; } = string.Empty;

	[Required]
	public decimal Price { get; set; }

	[Required]
	public int NumberOfStocks { get; set; }
}