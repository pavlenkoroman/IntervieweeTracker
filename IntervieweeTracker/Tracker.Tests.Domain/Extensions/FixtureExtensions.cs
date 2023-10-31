using System.Net.Mail;
using AutoFixture;
using Tracker.Domain.Requests;
using Tracker.Domain.Requests.Events;
using Tracker.Domain.Requests.Workflows;
using Tracker.Domain.Users;

namespace Tracker.Tests.Domain.Extensions;

public static class FixtureExtensions
{
    public static void ExecuteAllCustomizations(this IFixture fixture)
    {
        fixture.Customize<Email>(x =>
            x.FromFactory(() => new Email(fixture.Create<MailAddress>().Address)));

        fixture.Customize<Request>(x =>
            x.FromFactory(() =>
                Request.Create(Guid.NewGuid(), fixture.Create<Document>(), fixture.Create<Workflow>())));
    }
}