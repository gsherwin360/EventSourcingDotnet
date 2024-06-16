using MongoDB.Driver;

namespace Infrastructure.MongoDb;

public interface IMongoContext
{
	IMongoCollection<T> GetCollection<T>(string name);
}
