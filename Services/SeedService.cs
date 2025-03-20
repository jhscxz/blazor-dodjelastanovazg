using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        /// <summary>
        /// Seeding administrativnog korisnika.
        /// Napomena: Promijenite lozinku u sigurnu verziju.
        /// </summary>
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
                    await _userManager.AddToRoleAsync(adminUser, adminRole);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating admin user: {error.Description}");
                    }
                }
            }
            else
            {
                if (!await _userManager.IsInRoleAsync(adminUser, adminRole))
                {
                    await _userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }

        /// <summary>
        /// Seeding više korisnika prema zadanim podacima.
        /// Svaki korisnik može imati više uloga.
        /// </summary>
        /// <param name="seedUsers">Kolekcija modela s podacima o korisnicima za seeding</param>
        public async Task SeedUsers(IEnumerable<SeedUserModel> seedUsers)
        {
            foreach (var user in seedUsers)
            {
                // Osiguraj da sve uloge postoje
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
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Error creating user {user.Email}: {error.Description}");
                        }
                    }
                }
                else
                {
                    // Osiguraj da postoje sve zadane uloge korisnika
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
    }

    /// <summary>
    /// Model koji sadrži podatke potrebne za seeding korisnika.
    /// </summary>
    public class SeedUserModel
    {
        /// <summary>
        /// Korisničko ime.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Email adresa korisnika.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Lozinka korisnika.
        /// Napomena: Koristite jake lozinke u produkciji.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Lista uloga koje korisnik treba imati.
        /// </summary>
        public List<string> Roles { get; set; } = new();
    }
}
