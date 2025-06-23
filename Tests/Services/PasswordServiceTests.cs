using DodjelaStanovaZG.Services;
using Microsoft.AspNetCore.Identity;
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

        var store = Mock.Of<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(
            store, null!, null!, null!, null!, null!, null!, null!, null!);

        userManager.Setup(u => u.FindByIdAsync(userId))
            .ReturnsAsync((IdentityUser?)null);

        var service = new PasswordService(userManager.Object);

        // Act
        var result = await service.ChangeOwnPasswordAsync(userId, "old", "new");

        // Assert
        Assert.False(result.Succeeded);
        var error = Assert.Single(result.Errors);
        Assert.Equal($"Korisnik s ID '{userId}' ne postoji.", error.Description);

        userManager.Verify(u =>
                u.ChangePasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }
}