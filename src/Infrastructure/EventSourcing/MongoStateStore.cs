using Domain.EventSourcing;
using Domain.Models;
using Infrastructure.MongoDb;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Infrastructure.EventSourcing;

public sealed class MongoStateStore<T> : MongoRepositoryBase<T>, IStateStore<T> where T : Aggregate
{
	public MongoStateStore(IMongoContext mongoContext) : base(mongoContext)
	{
	}

	public async Task<T> ReadAsync(Guid key, CancellationToken ct = default)
	{
		var collection = await GetCollectionAsync();
		var filter = Builders<T>.Filter.Eq(x => x.Id, key);
		return await collection.Find(filter).FirstOrDefaultAsync(ct);
	}

	public async Task<T> FindAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default)
	{
		var collection = await GetCollectionAsync();
		return await collection.Find(filter).FirstOrDefaultAsync(ct);
	}

	public async Task<IQueryable<T>> GetAll(CancellationToken ct = default)
	{
		var collection = await this.GetCollectionAsync();
		return collection.AsQueryable();
	}

	public async Task WriteAsync(T aggregate, int oldVersion = -1, CancellationToken cancellationToken = default)
	{
		var collection = await this.GetCollectionAsync();

		await collection.ReplaceOneAsync<T>(
			x => x.Id == aggregate.Id,
			aggregate,
			new ReplaceOptions { IsUpsert = true },
			cancellationToken);
	}

	protected override string GetCollectionName()
	{
		return typeof(T).Name;
	}
}
