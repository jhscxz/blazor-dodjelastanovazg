using System.Security.Claims;
using DodjelaStanovaZG.Services.Interfaces;

namespace DodjelaStanovaZG.Services;

public class UserContextService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<UserContextService> logger)
    : IUserContextService
{
    public string GetCurrentUserId()
    {
        var id = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(id))
        {
            logger.LogWarning("GetCurrentUserId called but no user is logged in.");
            throw new Exception("Korisnik nije prijavljen.");
        }

        logger.LogInformation("Current user ID: {UserId}", id);
        return id;
    }
}