using DodjelaStanovaZG.Seeders;
using DodjelaStanovaZG.Services;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var seedService = scope.ServiceProvider.GetRequiredService<SeedService>();

            // Migracije baze
            context.Database.Migrate();

            // Seed dokumentacije (ako još nije)
            if (!context.DokazniDokumenti.Any())
            {
                var dokumenti = DokazniDokumentSeeder.GetObaveznaDokumentacija();
                context.DokazniDokumenti.AddRange(dokumenti);
                context.SaveChanges();
            }

            // Seed SuperAdmin korisnika
            await seedService.SeedUserAsync(UserSeedData.GetSuperAdmin());

            // Seed Admin korisnika
            await seedService.SeedUserAsync(UserSeedData.GetAdmin());

            // Seed ostalih korisnika
            foreach (var user in UserSeedData.GetRegularUsers())
            {
                await seedService.SeedUserAsync(user);
            }
        }
    }
}