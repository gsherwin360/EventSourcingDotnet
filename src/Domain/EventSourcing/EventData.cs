namespace Domain.EventSourcing;

public sealed record EventData(string Type, string Payload);