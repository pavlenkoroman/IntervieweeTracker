using AutoFixture;
using FluentAssertions;
using Tracker.Domain.Requests;
using Tracker.Domain.Users;
using Tracker.Tests.Domain.Extensions;
using Xunit;

namespace Tracker.Tests.Domain.Requests;

public class DocumentTests
{
    private readonly IFixture _fixture;

    public DocumentTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void Document_Constructor_ThrowsArgumentNullException_WhenEmailIsNull()
    {
        // Arrange
        var name = _fixture.Create<string>();
        Email email = null;
        var resume = _fixture.Create<Uri>();

        // Act and Assert
        Action act = () => new Document(name, email, resume);
        act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("email");
    }

    [Fact]
    public void Document_Constructor_ThrowsArgumentException_WhenNameIsNullOrEmpty()
    {
        // Arrange
        string name = null;
        var email = _fixture.Create<Email>();
        var resume = _fixture.Create<Uri>();

        // Act and Assert
        Action act = () => new Document(name, email, resume);
        act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("name");
    }

    [Fact]
    public void Document_Create_ReturnsDocumentWithValidData()
    {
        // Arrange
        var name = _fixture.Create<string>();
        var email = _fixture.Create<Email>();
        var resume = _fixture.Create<Uri>();

        // Act
        var document = Document.Create(name, email, resume);

        // Assert
        document.Name.Should().Be(name);
        document.Email.Should().Be(email);
        document.Resume.Should().Be(resume);
    }
}