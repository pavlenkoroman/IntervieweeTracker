using AutoFixture;
using FluentAssertions;
using Tracker.Domain.Users;
using Tracker.Tests.Domain.Extensions;
using Xunit;

namespace Tracker.Tests.Domain.Users;

public class UserTests
{
    private readonly Fixture _fixture;

    public UserTests()
    {
        _fixture = new Fixture();
        _fixture.ExecuteAllCustomizations();
    }

    [Fact]
    public void CreateUser_ValidParameters_UserCreated()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var name = "John Doe";
        var email = _fixture.Create<Email>();

        // Act
        var user = User.Create(roleId, name, email);

        // Assertion
        user.Should().NotBeNull();
        user.Id.Should().NotBe(Guid.Empty);
        user.RoleId.Should().Be(roleId);
        user.Name.Should().Be(name);
        user.Email.Should().Be(email);
    }

    [Fact]
    public void CreateUser_WithInvalidName_ThrowsArgumentException()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var name = "";
        var email = _fixture.Create<Email>();

        // Act
        Action action = () => User.Create(roleId, name, email);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreateUser_EmptyRoleId_ThrowsArgumentException()
    {
        // Arrange
        var emptyRoleId = Guid.Empty;
        var name = "John Doe";
        var email = _fixture.Create<Email>();

        // Act
        Action action = () => User.Create(emptyRoleId, name, email);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("RoleId cannot be empty*");
    }

    [Fact]
    public void CreateUser_EmptyName_ThrowsArgumentException()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var emptyName = "";
        var email = _fixture.Create<Email>();

        // Act
        Action action = () => User.Create(roleId, emptyName, email);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("User name cannot be null*");
    }
}