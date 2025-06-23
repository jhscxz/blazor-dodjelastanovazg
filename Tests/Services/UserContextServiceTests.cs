using System.Security.Claims;
using DodjelaStanovaZG.Services;
using Moq;
using Xunit;

namespace DodjelaStanovaZG.Tests.Services;

public class UserContextServiceTests
{
    [Fact]
    public void GetCurrentUserId_NoClaim_ThrowsAndLogsWarning()
    {
        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity())
        };
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(context);

        var logger = new Mock<ILogger<UserContextService>>();
        var service = new UserContextService(accessor.Object, logger.Object);

        var ex = Assert.Throws<Exception>(() => service.GetCurrentUserId());
        Assert.Equal("Korisnik nije prijavljen.", ex.Message);
        logger.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("GetCurrentUserId called but no user is logged in.")),
                It.IsAny<Exception?>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public void GetCurrentUserId_WithClaim_ReturnsIdAndLogsInfo()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "123")
        };
        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(claims))
        };
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(context);

        var logger = new Mock<ILogger<UserContextService>>();
        var service = new UserContextService(accessor.Object, logger.Object);

        var result = service.GetCurrentUserId();

        Assert.Equal("123", result);
        logger.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Current user ID") && v.ToString()!.Contains("123")),
                It.IsAny<Exception?>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Once);
    }
}