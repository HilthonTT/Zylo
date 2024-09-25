using FluentAssertions;
using Zylo.Domain.Users;
using Zylo.Domain.Users.DomainEvents;
using Zylo.Domain.Users.ValueObjects;

namespace Domain.UnitTests.Users;

public class UserTests
{
    [Fact]
    public void Create_Should_CreateUser_WhenIsValid()
    {
        // Arrange
        Email email = Email.Create("test@test.com").Value;
        FirstName firstName = FirstName.Create("FirstName").Value;
        LastName lastName = LastName.Create("LastName").Value;
        string passwordHash = "PasswordHash";

        // Act
        var user = User.Create(email, firstName, lastName, passwordHash);

        // Assert
        user.Should().NotBeNull();
    }

    [Fact]
    public void Create_Should_RaiseDomainEvent_WhenIsValid()
    {
        // Arrange
        Email email = Email.Create("test@test.com").Value;
        FirstName firstName = FirstName.Create("FirstName").Value;
        LastName lastName = LastName.Create("LastName").Value;
        string passwordHash = "PasswordHash";

        // Act
        var user = User.Create(email, firstName, lastName, passwordHash);

        // Assert
        user.DomainEvents
            .Should().ContainSingle()
            .Which
            .Should().BeOfType<UserCreatedDomainEvent>();
    }
}
