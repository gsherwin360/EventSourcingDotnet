using Domain.Models;

namespace Domain.Events;

public record ProductCreatedEvent(
	Guid ProductId,
	string Name,
	decimal Price,
	int NumberOfStocks) : IEvent
{
	public DateTime When { get; private set; } = DateTime.UtcNow;

	public static ProductCreatedEvent Create(Guid id, string name, decimal price, int numberOfStocks)
	{
		if (id == Guid.Empty)
			throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id));

		if (string.IsNullOrEmpty(name))
			throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));

		if (price < 0)
			throw new ArgumentException($"'{nameof(price)}' cannot be a non-negative value.", nameof(price));

		if (numberOfStocks <= 0)
			throw new ArgumentException($"'{nameof(numberOfStocks)}' cannot be zero or a negative value.", nameof(numberOfStocks));

		return new ProductCreatedEvent(id, name, price, numberOfStocks);
	}
}
