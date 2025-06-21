using Microsoft.AspNetCore.Identity;

namespace DodjelaStanovaZG.Services.Interfaces
{
    public interface IPasswordService
    {
        Task<IdentityResult> ChangeOwnPasswordAsync(string userId, string oldPassword, string newPassword);
        Task<IdentityResult> ResetPasswordAsync(string userId, string newPassword);
        Task<IdentityUser?> GetUserByIdAsync(string userId);
    }
}
