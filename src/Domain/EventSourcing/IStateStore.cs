using System.Linq.Expressions;

namespace Domain.EventSourcing;

public interface IStateStore<T>
{
	Task WriteAsync(T aggregate, int oldVersion = -1, CancellationToken cancellationToken = default);

	Task<T> ReadAsync(Guid key, CancellationToken ct = default);

	Task<T> FindAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default);

	Task<IQueryable<T>> GetAll(CancellationToken ct = default);
}