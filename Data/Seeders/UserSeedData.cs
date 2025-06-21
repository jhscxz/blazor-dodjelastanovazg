using DodjelaStanovaZG.Data.Seeders.Models;

namespace DodjelaStanovaZG.Seeders;

public static class UserSeedData
{
    public static SeedUserModel GetSuperAdmin() => new() { Username = "superadmin", Email = "superadmin@example.com", Password = "SuperAdmin@123", Roles = ["SuperAdmin"] };

    public static SeedUserModel GetAdmin() => new() { Username = "jhusic", Email = "jhsc.xz@gmail.com", Password = "Admin@123", Roles = ["Admin"] };

    public static List<SeedUserModel> GetRegularUsers() =>
    [
        new() { Username = "ivanivic", Email = "ivanivic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "markomarkic", Email = "markomarkic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "petarpetrovic", Email = "petarpetrovic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "lukalukic", Email = "lukalukic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "josipjosic", Email = "josipjosic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "anteantic", Email = "anteantic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "mariomarinovic", Email = "mariomarinovic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "nikolanikolic", Email = "nikolanikolic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "dariodario", Email = "dariodario@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "zoranzoranic", Email = "zoranzoranic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "gorangoric", Email = "gorangoric@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "tomislavtomicic", Email = "tomislavtomicic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "stjepanstjesisic", Email = "stjepanstjesisic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "mihaelmiskovic", Email = "mihaelmiskovic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "andrejandrejic", Email = "andrejandrejic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "darkodarkic", Email = "darkodarkic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "sasasasacic", Email = "sasasasacic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "damirdamirovic", Email = "damirdamirovic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "bojanbojanic", Email = "bojanbojanic@example.com", Password = "User@123", Roles = ["User"] },
        new() { Username = "kresikreso", Email = "kresikreso@example.com", Password = "User@123", Roles = ["User"] }
    ];
}
