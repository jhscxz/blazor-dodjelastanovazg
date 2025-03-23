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
        /// Seeding superadmin korisnika. Postoji samo jedan superadmin.
        /// Napomena: Promijenite lozinku u sigurnu verziju!
        /// </summary>
        public async Task SeedSuperAdminUser()
        {
            string superAdminEmail = "superadmin@example.com";
            string superAdminPassword = "SuperAdmin@123"; // OBAVEZNO promijeniti na sigurnu lozinku!
            string superAdminRole = "SuperAdmin";

            // Kreiraj rolu ako ne postoji
            if (!await _roleManager.RoleExistsAsync(superAdminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(superAdminRole));
            }

            // Provjera postoji li superadmin korisnik
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
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Greška pri kreiranju superadmin korisnika: {error.Description}");
                    }
                }
            }
            else
            {
                if (!await _userManager.IsInRoleAsync(superAdminUser, superAdminRole))
                {
                    await _userManager.AddToRoleAsync(superAdminUser, superAdminRole);
                }
            }
        }

        /// <summary>
        /// Seeding administrativnog korisnika.
        /// Napomena: Promijenite lozinku u sigurnu verziju!
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
                        Console.WriteLine($"Greška pri kreiranju admin korisnika: {error.Description}");
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
                            Console.WriteLine($"Greška pri kreiranju korisnika {user.Email}: {error.Description}");
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

        /// <summary>
        /// Metoda za seeding svih korisnika: superadmin, admin i regularni korisnici (Korisnik).
        /// </summary>
        public async Task SeedAllUsers()
        {
            // Seed superadmin (jedini)
            await SeedSuperAdminUser();

            // Seed admin korisnika
            await SeedAdminUser();

            // Seed regularnih korisnika (20-ak korisnika)
            var seedUsers = new List<SeedUserModel>
            {
                new SeedUserModel { Username = "ivanivic", Email = "ivanivic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "markomarkic", Email = "markomarkic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "petarpetrovic", Email = "petarpetrovic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "lukalukic", Email = "lukalukic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "josipjosic", Email = "josipjosic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "anteantic", Email = "anteantic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "mariomarinovic", Email = "mariomarinovic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "nikolanikolic", Email = "nikolanikolic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "dariodario", Email = "dariodario@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "zoranzoranic", Email = "zoranzoranic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "gorangoric", Email = "gorangoric@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "tomislavtomicic", Email = "tomislavtomicic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "stjepanstjesisic", Email = "stjepanstjesisic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "mihaelmiskovic", Email = "mihaelmiskovic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "andrejandrejic", Email = "andrejandrejic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "darkodarkic", Email = "darkodarkic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "sasasasacic", Email = "sasasasacic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "damirdamirovic", Email = "damirdamirovic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "bojanbojanic", Email = "bojanbojanic@example.com", Password = "User@123", Roles = new List<string> { "User" } },
                new SeedUserModel { Username = "kresikreso", Email = "kresikreso@example.com", Password = "User@123", Roles = new List<string> { "User" } }
            };

            await SeedUsers(seedUsers);
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
