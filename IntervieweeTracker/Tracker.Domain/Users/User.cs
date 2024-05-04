using BCryptNet = BCrypt.Net.BCrypt;

namespace Tracker.Domain.Users;

public class User
{
    public Guid Id { get; private init; }
    public Guid RoleId { get; private set; }
    public string Name { get; private init; }
    public Email Email { get; private init; }
    public string PasswordHash { get; private set; }

    public User(Guid id, Guid roleId, string name, Email email, string password)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(roleId);
        ArgumentNullException.ThrowIfNull(email);

        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty", nameof(id));
        }

        if (roleId == Guid.Empty)
        {
            throw new ArgumentException("RoleId cannot be empty", nameof(roleId));
        }

        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("User name cannot be null, empty, or whitespace", nameof(name));
        }

        Id = id;
        RoleId = roleId;
        Name = name;
        Email = email;
        PasswordHash = BCryptNet.EnhancedHashPassword(password);
    }

    public bool VerifyPassword(string password)
    {
        return BCryptNet.EnhancedVerify(password, PasswordHash);
    }

    public static User Create(Guid roleId, string name, Email email, string password)
    {
        return new User(Guid.NewGuid(), roleId, name, email, password);
    }
}