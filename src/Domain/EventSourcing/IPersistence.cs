using System.Linq.Expressions;

namespace Domain.EventSourcing;

public interface IPersistence<T>
{
    Task SaveAsync(T aggregate, int oldVersion = -1, CancellationToken ct = default);

    Task<T> GetAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default);

    public IStateStore<T> StateStore { get; }

    public IEventStore EventStore { get; }
}
