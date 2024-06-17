using Domain.Events;
using Domain.Models;

namespace Domain.Entities;

public class Product : Aggregate
{
	public string Name { get; private set; } = string.Empty;
	public decimal Price { get; private set; }
	public int NumberOfStocks { get; private set; }

	private Product() { }

	private Product(Guid id, string name, decimal price, int numberOfStocks)
	{
		var @event = ProductCreatedEvent.Create(id, name, price, numberOfStocks);
		this.Apply(@event);
		this.Enqueue(@event);
	}

	public static Product Create(Guid productId, string name, decimal price, int numberOfStocks)
	{
		if (string.IsNullOrEmpty(name))
			throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));

		if (price < 0)
			throw new ArgumentException($"'{nameof(price)}' cannot be a non-negative value.", nameof(price));

		if (numberOfStocks <= 0)
			throw new ArgumentException($"'{nameof(numberOfStocks)}' cannot be zero or a negative value.", nameof(numberOfStocks));

		return new Product(productId, name, price, numberOfStocks);
	}

	public void UpdateDetails(string newName, decimal newPrice)
	{
		if (string.IsNullOrEmpty(newName))
			throw new ArgumentException($"'{nameof(newName)}' cannot be null or empty.", nameof(newName));

		if (newPrice < 0)
			throw new ArgumentException($"'{nameof(newPrice)}' cannot be a non-negative value.", nameof(newPrice));

		var @event = ProductDetailsUpdatedEvent.Create(newName, newPrice);
		this.Apply(@event);
		this.Enqueue(@event);
	}

	public void AdjustStock(int adjustment)
	{
		if (adjustment == 0) return;

		var @event = ProductStockAdjustedEvent.Create(adjustment);
		this.Apply(@event);
		this.Enqueue(@event);
	}

	public override void Apply(IEvent @event)
	{
		switch (@event)
		{
			case ProductCreatedEvent productCreatedEvent:
				this.ApplyEvent(productCreatedEvent);
				break;
			case ProductDetailsUpdatedEvent productDetailsUpdatedEvent:
				this.ApplyEvent(productDetailsUpdatedEvent);
				break;
			case ProductStockAdjustedEvent productStockAdjustedEvent:
				this.ApplyEvent(productStockAdjustedEvent);
				break;
			default:
				return;
		}

		this.Version++;
	}

	public void ApplyEvent(ProductCreatedEvent @event)
	{
		this.Id = @event.ProductId;
		this.Name = @event.Name;
		this.Price = @event.Price;
		this.NumberOfStocks = @event.NumberOfStocks;
		this.DateCreated = @event.When;
		this.LastModifiedDate = @event.When;
	}

	public void ApplyEvent(ProductDetailsUpdatedEvent @event)
	{
		this.Name = @event.NewName;
		this.Price = @event.NewPrice;
		this.LastModifiedDate = @event.When;
	}

	public void ApplyEvent(ProductStockAdjustedEvent @event)
	{
		this.NumberOfStocks += @event.Adjustment;
		this.LastModifiedDate = @event.When;
	}
}
