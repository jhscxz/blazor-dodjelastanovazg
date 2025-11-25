using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DodjelaStanovaZG.Services;

public class PasswordService(
    UserManager<IdentityUser> userManager, ILogger<PasswordService> logger, IHttpContextAccessor httpContextAccessor) : IPasswordService
{
    public async Task<IdentityResult> ChangeOwnPasswordAsync(string userId, string oldPassword, string newPassword)
    {
        var user = await GetUserByIdAsync(userId);
        if (user is null)
        {
            logger.LogWarning("Pokušaj promjene lozinke za nepostojećeg korisnika: {UserId} s IP {Ip} u {Time}.", userId, GetClientIp(), Now());
            return UserNotFound(userId);
        }

        var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        if (result.Succeeded)
        {
            logger.LogInformation("Lozinka promijenjena za korisnika {UserId} s IP {Ip} u {Time}.", userId, GetClientIp(), Now());
        }
        else
        {
            logger.LogWarning("Neuspješna promjena lozinke za korisnika {UserId} s IP {Ip} u {Time}.", userId, GetClientIp(), Now());
        }

        return result;
    }

    public async Task<IdentityResult> ResetPasswordAsync(string userId, string newPassword)
    {
        var user = await GetUserByIdAsync(userId);
        if (user is null)
        {
            logger.LogWarning("Pokušaj resetiranja lozinke za nepostojećeg korisnika: {UserId} s IP {Ip} u {Time}.", userId, GetClientIp(), Now());
            return UserNotFound(userId);
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, newPassword);

        if (result.Succeeded)
        {
            logger.LogInformation("Lozinka resetirana za korisnika {UserId} s IP {Ip} u {Time}.", userId, GetClientIp(), Now());
        }
        else
        {
            logger.LogWarning("Resetiranje lozinke nije uspjelo za korisnika {UserId} s IP {Ip} u {Time}.", userId, GetClientIp(), Now());
        }

        return result;
    }

    public async Task<IdentityUser?> GetUserByIdAsync(string userId) => await userManager.FindByIdAsync(userId);

    private static IdentityResult UserNotFound(string userId) =>
        IdentityResult.Failed(new IdentityError
        {
            Description = $"Korisnik s ID '{userId}' ne postoji."
        });

    private string GetClientIp() => httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "nepoznato";

    private string Now() => DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
}
