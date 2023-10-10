namespace Tracker.Domain.Requests.Events;

public sealed class RequestCreatedEvent : IEvent
{
    public Guid Id { get; }
    public Guid RequestId { get; }
    public string Data { get; }

    public RequestCreatedEvent(Guid id, Guid requestId, string data)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(requestId);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("Guid cannot be empty", nameof(id));
        }

        if (requestId == Guid.Empty)
        {
            throw new ArgumentException("RequestId cannot be empty", nameof(requestId));
        }

        if (string.IsNullOrEmpty(data) || string.IsNullOrWhiteSpace(data))
        {
            throw new ArgumentException("Data cannot be null, empty, or whitespace", nameof(data));
        }

        Id = id;
        RequestId = requestId;
        Data = data;
    }

    public static RequestCreatedEvent Create(Guid requestId, string data)
    {
        return new RequestCreatedEvent(Guid.NewGuid(), requestId, data);
    }
}