using System.Security.Claims;
using DodjelaStanovaZG.Helpers.IHelpers;

namespace DodjelaStanovaZG.Helpers
{

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? throw new Exception("Korisnik nije prijavljen.");
        }
    }
}