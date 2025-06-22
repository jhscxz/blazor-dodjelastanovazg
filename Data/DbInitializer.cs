using DodjelaStanovaZG.Data.Seeders;
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
            await context.Database.MigrateAsync();
            
            await seedService.SeedFromConfigurationAsync();
        }
    }
}