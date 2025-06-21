using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DodjelaStanovaZG.Services
{
    public class PasswordService(
        UserManager<IdentityUser> userManager)
        : IPasswordService
    {
        public async Task<IdentityResult> ChangeOwnPasswordAsync(
            string userId,
            string oldPassword,
            string newPassword)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Korisnik s ID '{userId}' ne postoji."
                });
            }

            var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result;
        }

        public async Task<IdentityResult> ResetPasswordAsync(string userId, string newPassword)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Korisnik s ID '{userId}' ne postoji."
                });
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, newPassword);
            return result;
        }

        public async Task<IdentityUser?> GetUserByIdAsync(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }
    }
}
