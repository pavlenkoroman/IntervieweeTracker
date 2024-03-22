using AutoFixture;
using FluentAssertions;
using Moq;
using Tracker.Application.Repositories;
using Tracker.Application.StepTemplates.Commands;
using Tracker.Application.StepTemplates.Handlers;
using Tracker.Domain.WorkflowTemplates;
using Tracker.Tests.Application.Extensions;
using Xunit;

namespace Tracker.Tests.Application.StepTemplates;

public class CreateStepTemplateCommandHandlerTests
{
    private readonly IFixture _fixture;

    public CreateStepTemplateCommandHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public async Task CreateStepTemplateByUserId_Should_CompleteSuccessfully()
    {
        // Arrange
        var stepTemplate = StepTemplate.CreateByUser(
            _fixture.Create<string>(),
            _fixture.Create<int>(),
            _fixture.Create<Guid>());

        var stepTemplateRepositoryMock = new Mock<IStepTemplateRepository>(MockBehavior.Strict);
        stepTemplateRepositoryMock.Setup(stepTemplateRepository => stepTemplateRepository
                .Create(
                    It.Is<StepTemplate>(st =>
                        st.Title == stepTemplate.Title &&
                        st.Order == stepTemplate.Order &&
                        st.UserId == stepTemplate.UserId &&
                        st.RoleId == null),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var stepTemplateRepository = stepTemplateRepositoryMock.Object;

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(tenantRepository => tenantRepository.StepTemplates)
            .Returns(stepTemplateRepository);
        tenantRepositoryMock
            .Setup(tenantRepository => tenantRepository.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var tenantRepository = tenantRepositoryMock.Object;

        var command = new CreateStepTemplateCommand(stepTemplate.Title, stepTemplate.Order, stepTemplate.UserId, null);
        var sut = new CreateStepTemplateCommandHandler(tenantRepository);

        // Act
        var result = await sut.Handle(command, new CancellationToken());

        // Assert
        stepTemplateRepositoryMock.Verify(
            repository => repository
                .Create(
                    It.Is<StepTemplate>(st =>
                        st.Title == stepTemplate.Title &&
                        st.Order == stepTemplate.Order &&
                        st.UserId == stepTemplate.UserId),
                    It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Create was never invoked");

        Assert.IsType<Guid>(result);
    }
    
    [Fact]
    public async Task CreateStepTemplateByRoleId_Should_CompleteSuccessfully()
    {
        // Arrange
        var stepTemplate = StepTemplate.CreateByRole(
            _fixture.Create<string>(),
            _fixture.Create<int>(),
            _fixture.Create<Guid>());

        var stepTemplateRepositoryMock = new Mock<IStepTemplateRepository>(MockBehavior.Strict);
        stepTemplateRepositoryMock.Setup(stepTemplateRepository => stepTemplateRepository
                .Create(
                    It.Is<StepTemplate>(st =>
                        st.Title == stepTemplate.Title &&
                        st.Order == stepTemplate.Order &&
                        st.UserId == null &&
                        st.RoleId == stepTemplate.RoleId),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var stepTemplateRepository = stepTemplateRepositoryMock.Object;

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(tenantRepository => tenantRepository.StepTemplates)
            .Returns(stepTemplateRepository);
        tenantRepositoryMock
            .Setup(tenantRepository => tenantRepository.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var tenantRepository = tenantRepositoryMock.Object;

        var command = new CreateStepTemplateCommand(stepTemplate.Title, stepTemplate.Order, null, stepTemplate.RoleId);
        var sut = new CreateStepTemplateCommandHandler(tenantRepository);

        // Act
        var result = await sut.Handle(command, new CancellationToken());

        // Assert
        stepTemplateRepositoryMock.Verify(
            repository => repository
                .Create(
                    It.Is<StepTemplate>(st =>
                        st.Title == stepTemplate.Title &&
                        st.Order == stepTemplate.Order &&
                        st.UserId == stepTemplate.UserId),
                    It.IsAny<CancellationToken>()),
            Times.Exactly(1),
            "Create was never invoked");

        Assert.IsType<Guid>(result);
    }
    
    [Fact]
    public async Task CreateStepTemplate_WithoutRoleOrUser_Should_ThrowException()
    {
        // Arrange
        var stepTemplate = StepTemplate.CreateByRole(
            _fixture.Create<string>(),
            _fixture.Create<int>(),
            _fixture.Create<Guid>());

        var stepTemplateRepositoryMock = new Mock<IStepTemplateRepository>(MockBehavior.Strict);
        stepTemplateRepositoryMock.Setup(stepTemplateRepository => stepTemplateRepository
                .Create(
                    It.Is<StepTemplate>(st =>
                        st.Title == stepTemplate.Title &&
                        st.Order == stepTemplate.Order &&
                        st.UserId == null &&
                        st.RoleId == stepTemplate.RoleId),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var stepTemplateRepository = stepTemplateRepositoryMock.Object;

        var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
        tenantRepositoryMock
            .SetupGet(tenantRepository => tenantRepository.StepTemplates)
            .Returns(stepTemplateRepository);
        tenantRepositoryMock
            .Setup(tenantRepository => tenantRepository.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var tenantRepository = tenantRepositoryMock.Object;

        var command = new CreateStepTemplateCommand(stepTemplate.Title, stepTemplate.Order, null, null);
        var sut = new CreateStepTemplateCommandHandler(tenantRepository);

        // Act
        var act = async () => await sut.Handle(command, new CancellationToken());
        
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("At least one between User Id and Role Id must be not null (Parameter 'UserId RoleId')");
    }
}