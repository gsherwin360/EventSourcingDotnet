using Infrastructure.MongoDb;
using MongoDB.Driver;

namespace Infrastructure
{
	public abstract class MongoRepositoryBase<T>
	{
		protected readonly IMongoContext mongoContext;

		protected MongoRepositoryBase(IMongoContext mongoContext)
		{
			this.mongoContext = mongoContext ?? throw new ArgumentNullException(nameof(mongoContext));
		}

		protected virtual IMongoCollection<T> Collection
		{
			get
			{
				return this.mongoContext.GetCollection<T>(GetCollectionName());
			}
		}

		protected virtual string GetCollectionName()
		{
			// Implement this method in derived classes to provide collection names
			throw new NotImplementedException("GetCollectionName method must be implemented in derived classes.");
		}

		protected virtual async Task<IMongoCollection<T>> GetCollectionAsync()
		{
			return await Task.FromResult(Collection);
		}
	}
}
