using System.ComponentModel.DataAnnotations;

namespace Presentation.Api.Models;

public class UpdateProductDetailsModel
{
	[Required]
	public string Name { get; set; } = string.Empty;

	[Required]
	public decimal Price { get; set; }
}