using Infrastructure.MongoDb;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

public class MongoContext : IMongoContext
{
	private readonly IMongoDatabase database;

	public MongoContext(IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("Connection");
		var mongoClient = new MongoClient(connectionString);
		this.database = mongoClient.GetDatabase(configuration["ConnectionStrings:Database"]);
	}

	public IMongoCollection<T> GetCollection<T>(string name)
	{
		return this.database.GetCollection<T>(name);
	}
}
