using AutoFixture;
using FluentAssertions;
using Tracker.Domain.Users;
using Xunit;

namespace Tracker.Tests.Domain.Users;

public class RoleTests
{
    private readonly IFixture _fixture = new Fixture();

    [Fact]
    public void Role_Constructor_ThrowsArgumentNullException_WhenIdIsNull()
    {
        // Arrange
        var id = Guid.Empty;
        var title = _fixture.Create<string>();

        // Act
        Action act = () => new Role(id, title);
        
        // Assert
        act.Should().Throw<ArgumentException>().And.ParamName.Should().Be("id");
    }

    [Fact]
    public void Role_Constructor_ThrowsArgumentNullException_WhenTitleIsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        string title = null;

        // Act
        Action act = () => new Role(id, title);
        
        // Assert
        act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("title");
    }

    [Fact]
    public void Role_Constructor_SetsIdAndTitle_WhenValidArgumentsProvided()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = _fixture.Create<string>();

        // Act
        var role = new Role(id, title);

        // Assert
        role.Id.Should().Be(id);
        role.Title.Should().Be(title);
    }

    [Fact]
    public void Role_Create_ReturnsRoleWithNonEmptyGuid()
    {
        // Act
        var role = Role.Create(_fixture.Create<string>());

        // Assert
        role.Id.Should().NotBe(Guid.Empty);
    }
}