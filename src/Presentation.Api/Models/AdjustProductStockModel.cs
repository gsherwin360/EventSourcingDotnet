using System.ComponentModel.DataAnnotations;

namespace Presentation.Api.Models;

public class AdjustProductStockModel
{
	[Required]
	public int Adjustment { get; set; }
}