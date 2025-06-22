using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;

public class NatjecajSeedService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<NatjecajSeedService> _logger;
    public NatjecajSeedService(ApplicationDbContext context, ILogger<NatjecajSeedService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedNatjecajiAsync()
    {
        if (await _context.Natjecaji.AnyAsync())
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

        _context.Natjecaji.AddRange(lista);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Dodano {Count} natječaja u bazu.", lista.Count);
    }
}