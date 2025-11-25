using DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;

namespace DodjelaStanovaZG.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        await DbInitializer.SeedAsync(services);
        var natjecajSeeder = services.GetRequiredService<NatjecajSeedService>();
        await natjecajSeeder.SeedNatjecajiAsync();
    }
}