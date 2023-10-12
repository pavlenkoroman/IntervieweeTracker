using Tracker.Domain.Users;

namespace Tracker.Domain.WorkflowTemplates;

public class StepTemplate
{
    public Guid Id { get; private init; }
    public string Title { get; private set; }
    public int Order { get; private set; }
    public Guid? UserId { get; private set; }
    public Guid? RoleId { get; private set; }

    public StepTemplate(Guid id, string title, int order, Guid? userId, Guid? roleId)
    {
        ArgumentNullException.ThrowIfNull(id);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("Guid cannot be empty", nameof(id));
        }

        if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be null, empty, or whitespace", nameof(title));
        }

        Id = id;
        Title = title;
        Order = order;
        UserId = userId;
        RoleId = roleId;
    }

    public static StepTemplate CreateByUser(string name, int order, Guid? userId)
    {
        return new StepTemplate(Guid.NewGuid(), name, order, userId, null);
    }

    public static StepTemplate CreateByRole(string name, int order, Guid? roleId)
    {
        return new StepTemplate(Guid.NewGuid(), name, order, null, roleId);
    }

    public void UpdateUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        UserId = user.Id;
        RoleId = user.RoleId;
    }
}