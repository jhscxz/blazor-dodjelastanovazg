using Microsoft.AspNetCore.Identity;
using DodjelaStanovaZG.Seeders.Models;
namespace DodjelaStanovaZG.Services;

public class SeedService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
{
    public async Task SeedUserAsync(SeedUserModel user)
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