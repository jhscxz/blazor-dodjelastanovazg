using Microsoft.AspNetCore.Identity;

namespace DodjelaStanovaZG.Services
{
    public class SeedService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAdminUser()
        {
            string adminEmail = "jhsc.xz@gmail.com";
            string adminPassword = "Admin@123"; // OBAVEZNO promijeniti na sigurnu lozinku!
            string adminRole = "Admin";

            // Kreira admin rolu ako ne postoji
            if (!await _roleManager.RoleExistsAsync(adminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // Provjera postoji li admin korisnik
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
                    await _userManager.AddToRoleAsync(adminUser, adminRole); // Dodavanje uloge admina
                }
                else
                {
                    // Ispis grešaka ako kreiranje korisnika ne uspije
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }
            else
            {
                // Osiguravanje da admin korisnik ima administratorsku ulogu
                if (!await _userManager.IsInRoleAsync(adminUser, adminRole))
                {
                    await _userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }
    }
}
