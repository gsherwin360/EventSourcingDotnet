using Domain.EventSourcing;
using Infrastructure.MongoDb;
using MongoDB.Driver;

namespace Infrastructure.EventSourcing;

public class MongoEventStore : MongoRepositoryBase<MongoEvent>, IEventStore
{
	public MongoEventStore(IMongoContext mongoContext) : base(mongoContext)
	{
	}

	public async Task AppendAsync(string streamName, ICollection<EventData> events, int currentVersion, CancellationToken cancellationToken = default)
	{
		var collection = await GetCollectionAsync();

		var storedEvents = await collection.Find(x => x.StreamName == streamName).ToListAsync();

		var currentStreamPosition = storedEvents.Count;
		if (currentVersion != currentStreamPosition - 1)
		{
			throw new InvalidOperationException("Current version doesn't match the count of events in the event store");
		}

		var mongoEvents = new List<MongoEvent>();
		foreach (var @event in events)
		{
			mongoEvents.Add(@event.ToMongoEvent(currentStreamPosition++, streamName));
		}

		await collection.InsertManyAsync(mongoEvents, null, cancellationToken);
	}

	protected override string GetCollectionName()
	{
		return nameof(MongoEvent);
	}
}
