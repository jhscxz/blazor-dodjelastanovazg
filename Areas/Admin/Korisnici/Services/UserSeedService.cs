using Microsoft.AspNetCore.Identity;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.Services;

public class UserSeedService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
{
    private async Task SeedSuperAdminUser()
    {
        const string superAdminEmail = "superadmin@example.com";
        const string superAdminPassword = "SuperAdmin@123";
        const string superAdminRole = "SuperAdmin";

        if (!await roleManager.RoleExistsAsync(superAdminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(superAdminRole));
        }

        var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);
        if (superAdminUser == null)
        {
            superAdminUser = new IdentityUser
            {
                UserName = "superadmin",
                Email = superAdminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(superAdminUser, superAdminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdminUser, superAdminRole);
            }
        }
        else if (!await userManager.IsInRoleAsync(superAdminUser, superAdminRole))
        {
            await userManager.AddToRoleAsync(superAdminUser, superAdminRole);
        }
    }

    private async Task SeedAdminUser()
    {
        const string adminEmail = "jhsc.xz@gmail.com";
        const string adminPassword = "Admin@123";
        const string adminRole = "Admin";

        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = "jhusic",
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }
        else if (!await userManager.IsInRoleAsync(adminUser, adminRole))
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }

    private async Task SeedUsers(IEnumerable<SeedUserModel> seedUsers)
    {
        foreach (var user in seedUsers)
        {
            foreach (var role in user.Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
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
                if (!result.Succeeded) continue;
                foreach (var role in user.Roles)
                {
                    await userManager.AddToRoleAsync(identityUser, role);
                }
            }
            else
            {
                foreach (var role in user.Roles)
                {
                    if (!await userManager.IsInRoleAsync(identityUser, role))
                    {
                        await userManager.AddToRoleAsync(identityUser, role);
                    }
                }
            }
        }
    }

    public async Task SeedAllUsers()
    {
        await SeedSuperAdminUser();
        await SeedAdminUser();

        var seedUsers = new List<SeedUserModel>
        {
            new SeedUserModel { Username = "ivanivic", Email = "ivanivic@example.com", Password = "User@123", Roles = new() { "User" } },
            new SeedUserModel { Username = "markomarkic", Email = "markomarkic@example.com", Password = "User@123", Roles = new() { "User" } },
            // ... Dodaj ostale korisnike po potrebi
        };

        await SeedUsers(seedUsers);
    }
}

public class SeedUserModel
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}
