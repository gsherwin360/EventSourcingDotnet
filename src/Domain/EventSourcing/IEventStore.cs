namespace Domain.EventSourcing;

public interface IEventStore
{
	Task AppendAsync(string streamName, ICollection<EventData> events, int currentVersion, CancellationToken cancellationToken = default);
}