using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Infrastructure.Repositories;

public class SocijalniBodoviRepository(ApplicationDbContext context) : ISocijalniBodoviRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<SocijalniNatjecajZahtjev?> GetZahtjevWithDetailsAsync(long zahtjevId)
    {
        return await _context.SocijalniNatjecajZahtjevi
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
        _context.SocijalniNatjecajBodovi.Add(bodovi);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}