using System.Security.Claims;
using DodjelaStanovaZG.Services;
using Moq;
using Xunit;

namespace DodjelaStanovaZG.Tests.Services;

public class UserContextServiceTests
{
    [Fact]
    public void GetCurrentUserId_NoClaim_Throws()
    {
        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity())
        };

        var accessor = Mock.Of<IHttpContextAccessor>(a => a.HttpContext == context);
        var logger = Mock.Of<ILogger<UserContextService>>();

        var service = new UserContextService(accessor, logger);

        var ex = Assert.Throws<Exception>(() => service.GetCurrentUserId());
        Assert.Equal("Korisnik nije prijavljen.", ex.Message);
    }


    [Fact]
    public void GetCurrentUserId_WithClaim_ReturnsId()
    {
        var context = new DefaultHttpContext();
        context.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "123")
        }));

        var accessor = Mock.Of<IHttpContextAccessor>(a => a.HttpContext == context);
        var logger = Mock.Of<ILogger<UserContextService>>();

        var service = new UserContextService(accessor, logger);

        var result = service.GetCurrentUserId();

        Assert.Equal("123", result);
    }

}