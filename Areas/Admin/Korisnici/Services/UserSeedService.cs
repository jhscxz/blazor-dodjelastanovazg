using Microsoft.AspNetCore.Identity;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.Services;

public class UserSeedService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserSeedService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedSuperAdminUser()
    {
        string superAdminEmail = "superadmin@example.com";
        string superAdminPassword = "SuperAdmin@123";
        string superAdminRole = "SuperAdmin";

        if (!await _roleManager.RoleExistsAsync(superAdminRole))
        {
            await _roleManager.CreateAsync(new IdentityRole(superAdminRole));
        }

        var superAdminUser = await _userManager.FindByEmailAsync(superAdminEmail);
        if (superAdminUser == null)
        {
            superAdminUser = new IdentityUser
            {
                UserName = "superadmin",
                Email = superAdminEmail,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(superAdminUser, superAdminPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(superAdminUser, superAdminRole);
            }
        }
        else if (!await _userManager.IsInRoleAsync(superAdminUser, superAdminRole))
        {
            await _userManager.AddToRoleAsync(superAdminUser, superAdminRole);
        }
    }

    public async Task SeedAdminUser()
    {
        string adminEmail = "jhsc.xz@gmail.com";
        string adminPassword = "Admin@123";
        string adminRole = "Admin";

        if (!await _roleManager.RoleExistsAsync(adminRole))
        {
            await _roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        var adminUser = await _userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = "jhusic",
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }
        else if (!await _userManager.IsInRoleAsync(adminUser, adminRole))
        {
            await _userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }

    public async Task SeedUsers(IEnumerable<SeedUserModel> seedUsers)
    {
        foreach (var user in seedUsers)
        {
            foreach (var role in user.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var identityUser = await _userManager.FindByEmailAsync(user.Email);
            if (identityUser == null)
            {
                identityUser = new IdentityUser
                {
                    UserName = user.Username,
                    Email = user.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(identityUser, user.Password);
                if (result.Succeeded)
                {
                    foreach (var role in user.Roles)
                    {
                        await _userManager.AddToRoleAsync(identityUser, role);
                    }
                }
            }
            else
            {
                foreach (var role in user.Roles)
                {
                    if (!await _userManager.IsInRoleAsync(identityUser, role))
                    {
                        await _userManager.AddToRoleAsync(identityUser, role);
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
