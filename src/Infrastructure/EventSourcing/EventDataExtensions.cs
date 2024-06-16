using Domain.EventSourcing;
using Domain.Models;
using Newtonsoft.Json;

namespace Infrastructure.EventSourcing;

public static class EventDataExtensions
{
	public static EventData ToEventData(this IEvent @event)
	{
		var payloadType = @event.GetType().Name;
		var payloadJson = JsonConvert.SerializeObject(@event);

		return new EventData(payloadType, payloadJson);
	}

	public static MongoEvent ToMongoEvent(this EventData @event, int streamPosition, string streamName)
	{
		return new MongoEvent
		{
			Type = @event.Type,
			Payload = @event.Payload,
			StreamPosition = (uint)streamPosition,
			StreamName = streamName
		};
	}
}
