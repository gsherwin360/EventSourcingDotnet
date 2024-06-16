using Domain.Models;

namespace Domain.Events;

public record ProductStocksAdjustedEvent(int Adjustment) : IEvent
{
	public DateTime When { get; private set; } = DateTime.UtcNow;

	public static ProductStocksAdjustedEvent Create(int newNumberOfStocks)
	{
		if (newNumberOfStocks < 0)
			throw new ArgumentException($"'{nameof(newNumberOfStocks)}' cannot be zero or a negative value.", nameof(newNumberOfStocks));

		return new ProductStocksAdjustedEvent(newNumberOfStocks);
	}
}