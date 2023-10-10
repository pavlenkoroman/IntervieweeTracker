namespace Tracker.Domain.Events;

public interface IEvent
{
    public Guid Id { get; }
    string Data { get; }
    public Guid RequestId { get; }
}