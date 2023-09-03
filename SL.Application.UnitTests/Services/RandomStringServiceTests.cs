using FluentAssertions;
using SL.Application.Services;

namespace SL.Application.UnitTests.Services;

public class RandomStringServiceTests
{
    [Fact]
    public void GetRandomString_ShouldReturnRandomString()
    {
        // Arrange
        var sut = new RandomStringService();

        // Act
        var randomString = sut.GetRandomString();

        // Assert
        randomString.Should().HaveLength(6);
    }
}