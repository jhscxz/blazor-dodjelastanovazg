using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;

public class NatjecajSeedService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    private readonly ILogger<NatjecajSeedService> _logger;
    public NatjecajSeedService(IDbContextFactory<ApplicationDbContext> contextFactory, ILogger<NatjecajSeedService> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    public async Task SeedNatjecajiAsync()
    {
        await using var context = _contextFactory.CreateDbContext();

        if (await context.Natjecaji.AnyAsync())
        {
            _logger.LogInformation("Natječaji već postoje, preskačem seeding.");
            return;
        }

        var lista = new List<Natjecaj>
        {
            new()
            {
                PriustiviIliSocijalni = 2, // Socijalni
                Klasa = 1,
                ProsjekPlace = 1200.00m,
                Zakljucen = 2, // Zaključen
                DatumObjave = DateOnly.FromDateTime(DateTime.Today.AddMonths(-2)),
                RokZaPrijavu = DateOnly.FromDateTime(DateTime.Today.AddDays(15)),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Natjecaj
            {
                PriustiviIliSocijalni = 2, // Socijalni
                Klasa = 2,
                ProsjekPlace = 1300.00m,
                Zakljucen = 1, // Aktivan
                DatumObjave = DateOnly.FromDateTime(DateTime.Today),
                RokZaPrijavu = DateOnly.FromDateTime(DateTime.Today.AddMonths(1)),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        context.Natjecaji.AddRange(lista);
        await context.SaveChangesAsync();
        _logger.LogInformation("Dodano {Count} natječaja u bazu.", lista.Count);
    }
}