using FluentAssertions;
using SharedKernel;
using Zylo.Domain.Users.ValueObjects;

namespace Domain.UnitTests.Users;

public class EmailTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("plainaddress")]
    [InlineData("missingatsign.com")]
    [InlineData("@missinglocalpart.com")]
    [InlineData("user@.com")]
    [InlineData("user@com")]
    [InlineData("user@domain..com")]
    [InlineData("user@domain,com")]
    [InlineData("user@domain@domain.com")]
    [InlineData("user@domain.com ")] // Trailing space
    [InlineData(" user@domain.com")] // Leading space
    public void Email_Should_ReturnFailure_WhenValueIsInvalid(string? value)
    {
        // Act
        Result<Email> emailResult = Email.Create(value);

        // Assert
        emailResult.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Email_Should_ReturnSuccess_WhenValueIsValid()
    {
        // Act
        Result<Email> emailResult = Email.Create("test@test.com");

        // Assert
        emailResult.IsSuccess.Should().BeTrue();
    }
}
