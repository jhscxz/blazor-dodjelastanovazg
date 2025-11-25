using DodjelaStanovaZG.Data.Seeders.Models;
using Microsoft.AspNetCore.Identity;

namespace DodjelaStanovaZG.Data.Seeders;

internal class SeedService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
{
    public async Task SeedFromConfigurationAsync()
    {
        await SeedUserAsync(UserSeedData.GetManagement(configuration));
        foreach (var user in UserSeedData.GetUsers(configuration))
        {
            await SeedUserAsync(user);
        }
    }

    private async Task SeedUserAsync(SeedUserModel user)
    {
        foreach (var role in user.Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var identityUser = await userManager.FindByEmailAsync(user.Email);
        if (identityUser == null)
        {
            identityUser = new IdentityUser
            {
                UserName = user.Username,
                Email = user.Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(identityUser, user.Password);
            if (result.Succeeded)
            {
                foreach (var role in user.Roles)
                {
                    await userManager.AddToRoleAsync(identityUser, role);
                }
            }
        }
    }
}