namespace Tracker.Domain.Request;

public class IntervieweeDocument
{
    public string Name { get; private init; }
    public string Email { get; private init; }
    public Uri Resume { get; private init; }

    public IntervieweeDocument(string name, string email, Uri resume)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(resume);

        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null, empty, or whitespace", nameof(name));
        }

        Name = name;
        Email = email;
        Resume = resume;
    }

    public static IntervieweeDocument Create(string name, string email, Uri resume)
    {
        return new IntervieweeDocument(name, email, resume);
    }
}