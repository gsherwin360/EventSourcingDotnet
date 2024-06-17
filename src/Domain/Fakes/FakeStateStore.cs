using Domain.EventSourcing;
using Domain.Models;
using System.Linq.Expressions;

namespace Domain.Fakes;

public class FakeStateStore<T> : IStateStore<T> where T : Aggregate
{
	private Dictionary<Guid, T> aggregates;

	public FakeStateStore(Dictionary<Guid, T> aggregates)
	{
		this.aggregates = aggregates;
	}

	public Task<T> FindAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default)
	{
		return Task.FromResult(this.aggregates.Values.FirstOrDefault(filter.Compile()));
	}

	public async Task<IQueryable<T>> GetAll(CancellationToken ct = default)
	{
		return this.aggregates.Values.AsQueryable();
	}

	public Task<T> ReadAsync(Guid key, CancellationToken ct = default)
	{
		this.aggregates.TryGetValue(key, out T result);
		return Task.FromResult(result);
	}

	public Task WriteAsync(T aggregate, int oldVersion = -1, CancellationToken cancellationToken = default)
	{
		if (this.aggregates.TryGetValue(aggregate.Id, out T existingAggregate))
		{
			this.aggregates[aggregate.Id] = aggregate;
			return Task.CompletedTask;
		}

		this.aggregates.Add(aggregate.Id, aggregate);
		return Task.CompletedTask;
	}
}

