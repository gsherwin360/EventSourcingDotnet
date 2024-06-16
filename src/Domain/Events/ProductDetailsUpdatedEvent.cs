using Domain.Models;

namespace Domain.Events;

public record ProductDetailsUpdatedEvent(string NewName, decimal NewPrice) : IEvent
{
	public DateTime When { get; private set; } = DateTime.UtcNow;

	public static ProductDetailsUpdatedEvent Create(string newName, decimal newPrice)
	{
		if (string.IsNullOrEmpty(newName))
			throw new ArgumentException($"'{nameof(newName)}' cannot be null or empty.", nameof(newName));

		if (newPrice < 0)
			throw new ArgumentException($"'{nameof(newPrice)}' cannot be a non-negative value.", nameof(newPrice));

		return new ProductDetailsUpdatedEvent(newName, newPrice);	
	}
}
