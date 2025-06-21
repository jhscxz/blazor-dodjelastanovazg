using System.Security.Claims;
using DodjelaStanovaZG.Services.Interfaces;

namespace DodjelaStanovaZG.Services;

public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
{
    public string GetCurrentUserId()
    {
        return httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? throw new Exception("Korisnik nije prijavljen.");
    }
}