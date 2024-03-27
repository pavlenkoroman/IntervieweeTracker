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
        var password = _fixture.Create<string>();

        // Act
        var user = User.Create(roleId, name, email, password);

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
        var password = _fixture.Create<string>();

        // Act
        Action action = () => User.Create(roleId, name, email, password);

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
        var password = _fixture.Create<string>();

        // Act
        Action action = () => User.Create(emptyRoleId, name, email, password);

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
        var password = _fixture.Create<string>();

        // Act
        Action action = () => User.Create(roleId, emptyName, email, password);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("User name cannot be null*");
    }

    [Fact]
    public void CreateUser_EmptyId_ThrowsArgumentException()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var name = "John Doe";
        var email = _fixture.Create<Email>();
        var password = _fixture.Create<string>();

        // Act
        Action action = () => new User(Guid.Empty, roleId, name, email, password);

        // Assertion
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Id cannot be empty (Parameter 'id')");
    }

    [Fact]
    public void Verify_UserPasswordWithValidPassword_Should_Return_True()
    {
        // Arrange
        var roleId = _fixture.Create<Guid>();
        var name = _fixture.Create<string>();
        var email = _fixture.Create<Email>();
        var password = _fixture.Create<string>();
        var user = User.Create(roleId, name, email, password);
        
        // Act
        var isPasswordValid = user.VerifyPassword(password);
        
        // Arrange
        isPasswordValid.Should().BeTrue();
    }
    
    [Fact]
    public void Verify_UserPasswordWithInvalidPassword_Should_Return_False()
    {
        // Arrange
        var roleId = _fixture.Create<Guid>();
        var name = _fixture.Create<string>();
        var email = _fixture.Create<Email>();
        var password = _fixture.Create<string>();
        var user = User.Create(roleId, name, email, password);
        
        // Act
        var invalidPassword = _fixture.Create<string>();
        var isPasswordValid = user.VerifyPassword(invalidPassword);
        
        // Arrange
        isPasswordValid.Should().BeFalse();
    }
}