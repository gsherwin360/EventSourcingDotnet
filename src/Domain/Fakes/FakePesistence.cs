using Domain.EventSourcing;
using Domain.Models;
using System.Linq.Expressions;

namespace Domain.Fakes;

public class FakePersistence<T> : IPersistence<T> where T : Aggregate
{
	public Dictionary<Guid, T> Aggregates { get; private set; }

	public IStateStore<T> StateStore { get; private set; }

	public IEventStore EventStore => throw new NotImplementedException();

	public Task SaveAsync(T aggregate, int oldVersion = -1, CancellationToken ct = default)
	{
		this.StateStore.WriteAsync(aggregate, oldVersion, ct);
		return Task.CompletedTask;
	}

	public Task<T> GetAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default)
	{
		return this.StateStore.FindAsync(filter, ct);
	}

	public FakePersistence()
	{
		this.Aggregates = new Dictionary<Guid, T>();
		this.StateStore = new FakeStateStore<T>(this.Aggregates);
	}
}
