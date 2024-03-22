using System.Net.Mail;
using AutoFixture;
using AutoFixture.AutoMoq;
using Tracker.Domain.Requests;
using Tracker.Domain.Requests.Workflows;
using Tracker.Domain.Users;
using Tracker.Domain.WorkflowTemplates;

namespace Tracker.Tests.Application.Extensions;

public static class FixtureExtensions
{
    public static void ExecuteAllCustomizations(this IFixture fixture)
    {
        fixture.Customize(new AutoMoqCustomization());

        fixture.Customize<Email>(x =>
            x.FromFactory(() => new Email(fixture.Create<MailAddress>().Address)));

        fixture.Customize<Request>(x =>
            x.FromFactory(() =>
                Request.Create(fixture.Create<Guid>(), fixture.Create<Document>(), fixture.Create<Workflow>())));

        var orderState = 0;

        fixture
            .Customize<StepTemplate>(x => x.FromFactory(() =>
            {
                orderState++;
                return StepTemplate.CreateByUser(fixture.Create<string>(), orderState, fixture.Create<Guid>());
            }));

        fixture.Customize<WorkflowTemplate>(x => x.FromFactory(() => WorkflowTemplate.Create(
            fixture.Create<string>(),
            fixture.CreateMany<StepTemplate>().ToArray())));

        fixture.Customize<Request>(x => x.FromFactory(() =>
        {
            var workflowTemplate = fixture.Create<WorkflowTemplate>();
            var request = workflowTemplate.CreateRequest(fixture.Create<User>(), fixture.Create<Document>());
            return request;
        }));
    }
}