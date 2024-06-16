using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.EventSourcing;

public class MongoEvent
{
	[BsonId]
	[BsonElement]
	[BsonRepresentation(MongoDB.Bson.BsonType.String)]
	public Guid Id { get; set; }

	[BsonElement]
	[BsonRequired]
	public string Type { get; set; } = string.Empty;

	[BsonJson]
	[BsonRequired]
	public string Payload { get; set; } = string.Empty;

	[BsonElement]
	[BsonRequired]
	public string StreamName { get; set; } = string.Empty;

	[BsonElement]
	[BsonRequired]
	public uint StreamPosition { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class BsonJsonAttribute : Attribute
{
}
