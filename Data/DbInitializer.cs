using DodjelaStanovaZG.Services;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Data;

public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context)
    {
        // Provjeri migracije
        context.Database.Migrate();

        // Seed dokumenata
        if (!context.DokazniDokumenti.Any())
        {
            var dokumenti = DokazniDokumentSeeder.GetObaveznaDokumentacija();
            context.DokazniDokumenti.AddRange(dokumenti);
            context.SaveChanges();
        }

        // (Tu možeš dodati i seedanje korisnika, uloga itd.)
    }
}