using System.Net.Mail;
using AutoFixture;
using Tracker.Domain.Users;

namespace Tracker.Tests.Domain.Extensions;

public static class FixtureExtensions
{
    public static void ExecuteAllCustomizations(this IFixture fixture)
    {
        fixture.Customize<Email>(x =>
            x.FromFactory(() => new Email(fixture.Create<MailAddress>().Address)));
    }
}