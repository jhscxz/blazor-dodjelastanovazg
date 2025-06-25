using DodjelaStanovaZG.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DodjelaStanovaZG.Tests.Services;

public class PasswordServiceTests
{
    [Fact]
    public async Task ChangeOwnPasswordAsync_UserNotFound_ReturnsFailedResult()
    {
        // Arrange
        var userId = "user1";

        var userStore = Mock.Of<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            userStore, null!, null!, null!, null!, null!, null!, null!, null!);

        userManager.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync((IdentityUser?)null);

        var loggerMock = Mock.Of<ILogger<PasswordService>>();
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        httpContextAccessor.Setup(x => x.HttpContext!.Connection.RemoteIpAddress).Returns(System.Net.IPAddress.Parse("127.0.0.1"));

        var service = new PasswordService(userManager.Object, loggerMock, httpContextAccessor.Object);

        // Act
        var result = await service.ChangeOwnPasswordAsync(userId, "old", "new");

        // Assert
        Assert.False(result.Succeeded);
        var error = Assert.Single(result.Errors);
        Assert.Equal($"Korisnik s ID '{userId}' ne postoji.", error.Description);

        userManager.Verify(u => u.ChangePasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}