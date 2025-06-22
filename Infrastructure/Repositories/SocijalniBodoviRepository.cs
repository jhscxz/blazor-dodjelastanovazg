using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Infrastructure.Repositories;

public class SocijalniBodoviRepository : ISocijalniBodoviRepository
{
    public async Task<SocijalniNatjecajZahtjev?> GetZahtjevWithDetailsAsync(ApplicationDbContext context, long zahtjevId)
    
    {
        return await context.SocijalniNatjecajZahtjevi
            .Include(z => z.Natjecaj)
            .Include(z => z.Clanovi)
            .Include(z => z.KucanstvoPodaci)
            .ThenInclude(k => k!.Prihod)
            .Include(z => z.BodovniPodaci)
            .Include(z => z.Bodovi)
            .FirstOrDefaultAsync(z => z.Id == zahtjevId);
    }

    public async Task AddBodoviAsync(ApplicationDbContext context, SocijalniNatjecajBodovi bodovi)
    {
        await context.SocijalniNatjecajBodovi.AddAsync(bodovi);
    }

    public async Task SaveChangesAsync(ApplicationDbContext context)
    {
        await context.SaveChangesAsync();
    }
}