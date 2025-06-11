using System.Security.Claims;
using DodjelaStanovaZG.Helpers.IHelpers;

namespace DodjelaStanovaZG.Helpers;

public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
{
    public string GetCurrentUserId()
    {
        return httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? throw new Exception("Korisnik nije prijavljen.");
    }
}