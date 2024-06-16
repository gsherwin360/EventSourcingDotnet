using Domain.EventSourcing;
using Domain.Models;
using System.Linq.Expressions;

namespace Infrastructure.EventSourcing;

public class Persistence<T> : IPersistence<T> where T : Aggregate
{
	private readonly IEventStore eventStore;
	private readonly IStateStore<T> stateStore;

	public Persistence(IEventStore eventStore, IStateStore<T> stateStore)
	{
		this.eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
		this.stateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));
	}

	public async Task SaveAsync(T aggregate, int oldVersion = -1, CancellationToken ct = default)
	{
		var @events = aggregate.DequeueUncommittedEvents();

		await this.stateStore.WriteAsync(aggregate, oldVersion, ct);

		var streamName = $"{typeof(T).Name}-{aggregate.Id}";
		var eventData = @events.Select(e => e.ToEventData()).ToList();

		await this.eventStore.AppendAsync(streamName, eventData, oldVersion, ct);
	}

	public Task<T> GetAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default)
	{
		return this.stateStore.FindAsync(filter, ct);
	}

	public IStateStore<T> StateStore { get => this.stateStore; }

	public IEventStore EventStore { get => this.eventStore; }
}
