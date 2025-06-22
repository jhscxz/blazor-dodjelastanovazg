using DodjelaStanovaZG.Data.Seeders.Models;

namespace DodjelaStanovaZG.Data.Seeders;

public static class UserSeedData
{
    public static SeedUserModel GetManagement() => new() { Username = "jhusic", Email = "jhsc.xz@gmail.com", Password = "Admin@123", Roles = ["Management"] };

    public static List<SeedUserModel> GetUsers() =>
    [
        new() { Username = "ivanivic", Email = "ivanivic@example.com", Password = "User@123", Roles = ["ReferentReadOnly"] },
        new() { Username = "markomarkic", Email = "markomarkic@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "petarpetrovic", Email = "petarpetrovic@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "lukalukic", Email = "lukalukic@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "josipjosic", Email = "josipjosic@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "anteantic", Email = "anteantic@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "mariomarinovic", Email = "mariomarinovic@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "nikolanikolic", Email = "nikolanikolic@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "dariodario", Email = "dariodario@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "zoranzoranic", Email = "zoranzoranic@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "gorangoric", Email = "gorangoric@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "tomislavtomicic", Email = "tomislavtomicic@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "stjepanstjesisic", Email = "stjepanstjesisic@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "mihaelmiskovic", Email = "mihaelmiskovic@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "andrejandrejic", Email = "andrejandrejic@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "darkodarkic", Email = "darkodarkic@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "sasasasacic", Email = "sasasasacic@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "damirdamirovic", Email = "damirdamirovic@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "bojanbojanic", Email = "bojanbojanic@example.com", Password = "User@123", Roles = ["Referent"] },
        new() { Username = "kresikreso", Email = "kresikreso@example.com", Password = "User@123", Roles = ["Referent"] }
    ];
}
