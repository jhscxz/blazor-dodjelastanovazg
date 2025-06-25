using System.Security.Claims;
using DodjelaStanovaZG.Services.Interfaces;

namespace DodjelaStanovaZG.Services;

public class UserContextService(IHttpContextAccessor httpContextAccessor, ILogger<UserContextService> logger) : IUserContextService
{
    public string GetCurrentUserId()
    {
        var user = httpContextAccessor.HttpContext?.User;
        var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            logger.LogWarning("Poziv metode GetCurrentUserId bez prijavljenog korisnika. IP: {Ip}, Vrijeme: {Time}", GetIp(), Now());
            throw new InvalidOperationException("Korisnik nije prijavljen.");
        }

        logger.LogDebug("Dohvaćen ID trenutno prijavljenog korisnika: {UserId} s IP: {Ip} u {Time}", userId, GetIp(), Now());

        return userId;
    }

    private string GetIp() => httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "nepoznato";

    private string Now() => DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
}