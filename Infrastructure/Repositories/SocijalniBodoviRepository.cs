using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Infrastructure.Repositories;

public class SocijalniBodoviRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : ISocijalniBodoviRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory = contextFactory;

    public async Task<SocijalniNatjecajZahtjev?> GetZahtjevWithDetailsAsync(long zahtjevId)
    {
        await using var context = _contextFactory.CreateDbContext();
        return await context.SocijalniNatjecajZahtjevi
            .Include(z => z.Natjecaj)
            .Include(z => z.Clanovi)
            .Include(z => z.KucanstvoPodaci)
            .ThenInclude(k => k!.Prihod)
            .Include(z => z.BodovniPodaci)
            .Include(z => z.Bodovi)
            .FirstOrDefaultAsync(z => z.Id == zahtjevId);
    }

    public Task AddBodoviAsync(SocijalniNatjecajBodovi bodovi)
    {
        await using var context = _contextFactory.CreateDbContext();
        context.SocijalniNatjecajBodovi.Add(bodovi);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        await using var context = _contextFactory.CreateDbContext();
        return context.SaveChangesAsync();
    }
}