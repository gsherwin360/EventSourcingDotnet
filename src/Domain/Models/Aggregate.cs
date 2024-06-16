using Domain.EventSourcing;

namespace Domain.Models;

public abstract class Aggregate
{
	public Guid Id { get; protected set; } = default!;

	public int Version { get; protected set; } = -1;

	public DateTime DateCreated { get; set; }

	public DateTime LastModifiedDate { get; set; }

	[NonSerialized]
	protected readonly Queue<IEvent> uncommittedEvents = new();

	public abstract void Apply(IEvent @event);

	public IEvent[] DequeueUncommittedEvents()
	{
		var dequeuedEvents = this.uncommittedEvents.ToArray();

		this.uncommittedEvents.Clear();

		return dequeuedEvents;
	}

	protected virtual void Enqueue(IEvent @event)
	{
		this.uncommittedEvents.Enqueue(@event);
	}
}