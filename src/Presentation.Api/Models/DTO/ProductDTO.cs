using Domain.Entities;

namespace Presentation.Api.Models.DTOs;

public class ProductDTO
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
	public decimal Price { get; private set; }
	public int NumberOfStocks { get; private set; }
	public DateTime DateCreated { get; private set; }
	public DateTime LastModifiedDate { get; private set; }

	public static IEnumerable<ProductDTO> ToProductDTOMapList(IEnumerable<Product> source)
	{
		return source.Select(item => new ProductDTO
		{
			Id = item.Id,
			Name = item.Name,
			Price = item.Price,
			NumberOfStocks = item.NumberOfStocks,
			DateCreated = item.DateCreated,
			LastModifiedDate = item.LastModifiedDate
		});
	}
}