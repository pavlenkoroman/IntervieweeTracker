namespace Tracker.Domain.UserInfo;

public sealed class Role
{
    public Guid Id { get; private init; }
    public string Title { get; private set; }

    public Role(Guid id, string title)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(title);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("Guid cannot be empty", nameof(id));
        }

        Id = id;
        Title = title;
    }

    public static Role Create(string title)
    {
        return new Role(Guid.NewGuid(), title);
    }
}