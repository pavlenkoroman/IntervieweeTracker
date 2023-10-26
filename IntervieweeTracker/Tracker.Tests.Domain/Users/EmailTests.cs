using AutoFixture;
using FluentAssertions;
using Tracker.Domain.Users;
using Xunit;

namespace Tracker.Tests.Domain.Users;

public class EmailTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void CreateEmail_ValidEmailValue_EmailCreated()
    {
        // Arrange
        var validEmail = "test@example.com";

        // Act
        var email = new Email(validEmail);

        // Assert
        email.Should().NotBeNull();
        email.Value.Should().Be(validEmail);
    }

    [Fact]
    public void CreateEmail_InvalidEmailValue_ThrowsArgumentException()
    {
        // Arrange
        var invalidEmail = _fixture.Create<string>(); // Generate invalid email

        // Act & Assert
        Action action = () => new Email(invalidEmail);

        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Email value is not valid. (Parameter 'value')");
    }
}