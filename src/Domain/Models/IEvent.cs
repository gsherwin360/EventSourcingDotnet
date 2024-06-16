namespace Domain.Models;

public interface IEvent
{
	public DateTime When { get; }
}