using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.Exceptions;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services
{
    public class SocijalniClanService(IDbContextFactory<ApplicationDbContext> contextFactory, ILogger<SocijalniClanService> logger) : ISocijalniClanService
    {
        private IQueryable<SocijalniNatjecajZahtjev> BaseZahtjevQuery(ApplicationDbContext context, bool asNoTracking = false)
        {
            var query = context.SocijalniNatjecajZahtjevi
                .Include(z => z.Clanovi)
                .Include(z => z.Natjecaj);
            
            return asNoTracking ? query.AsNoTracking() : query;
        }

        private async Task<SocijalniNatjecajZahtjev> GetZahtjevByIdAsync(long id, bool asNoTracking)
        {
            await using var context = await contextFactory.CreateDbContextAsync();
            var zahtjev = await BaseZahtjevQuery(context, asNoTracking).FirstOrDefaultAsync(z => z.Id == id);

            if (zahtjev == null) throw new NotFoundException($"Zahtjev s ID-om {id} nije pronađen.");

            return zahtjev;
        }

        private static SocijalniNatjecajClan GetClanById(SocijalniNatjecajZahtjev zahtjev, long clanId)
        {
            var clan = zahtjev.Clanovi.FirstOrDefault(c => c.Id == clanId);
            if (clan == null) throw new NotFoundException($"Član s ID-om {clanId} nije pronađen.");
            return clan;
        }

        public async Task<SocijalniNatjecajClanDto> AddClanAsync(SocijalniNatjecajClan noviClan)
        {
            var zahtjev = await GetZahtjevByIdAsync(noviClan.ZahtjevId, true);
            await using var context = await contextFactory.CreateDbContextAsync();
            var isClosed = await context.Natjecaji
                .Where(n => n.Id == zahtjev.NatjecajId)
                .Select(n => n.IsClosed)
                .FirstAsync();
            if (isClosed)
            {
                logger.LogWarning("Natječaj {NatjecajId} je zaključen i izmjene nisu moguće", zahtjev.NatjecajId);
                throw new InvalidOperationException($"Natječaj {zahtjev.NatjecajId} je zaključen i izmjene nisu moguće");
            }

            await context.SocijalniNatjecajClanovi.AddAsync(noviClan);
            await context.SaveChangesAsync();

            return noviClan.ToDto();
        }

        public async Task<SocijalniNatjecajClanDto> EditClanAsync(SocijalniNatjecajClanDto azurirani)
        {
            await using var context = await contextFactory.CreateDbContextAsync();
            var zahtjev = await GetZahtjevByIdAsync(azurirani.ZahtjevId, false);
            var isClosed = await context.Natjecaji
                .Where(n => n.Id == zahtjev.NatjecajId)
                .Select(n => n.IsClosed)
                .FirstAsync();
            if (isClosed)
            {
                logger.LogWarning("Natječaj {NatjecajId} je zaključen i izmjene nisu moguće", zahtjev.NatjecajId);
                throw new InvalidOperationException($"Natječaj {zahtjev.NatjecajId} je zaključen i izmjene nisu moguće");
            }
            var clan = GetClanById(zahtjev, azurirani.Id);

            clan.ImePrezime = azurirani.ImePrezime;
            clan.Oib = azurirani.Oib;
            clan.Srodstvo = azurirani.Srodstvo;
            clan.DatumRodjenja = azurirani.DatumRodjenja;

            context.SocijalniNatjecajClanovi.Update(clan);
            await context.SaveChangesAsync();

            return clan.ToDto();
        }

        public async Task RemoveClanAsync(long zahtjevId, long clanId)
        {
            await using var context = await contextFactory.CreateDbContextAsync();
            var zahtjev = await GetZahtjevByIdAsync(zahtjevId, false);
            var isClosed = await context.Natjecaji
                .Where(n => n.Id == zahtjev.NatjecajId)
                .Select(n => n.IsClosed)
                .FirstAsync();
            if (isClosed)
            {
                logger.LogWarning("Natječaj {NatjecajId} je zaključen i izmjene nisu moguće", zahtjev.NatjecajId);
                throw new InvalidOperationException($"Natječaj {zahtjev.NatjecajId} je zaključen i izmjene nisu moguće");
            }
            var clan = GetClanById(zahtjev, clanId);

            context.SocijalniNatjecajClanovi.Remove(clan);
            await context.SaveChangesAsync();
        }

        public async Task<Dictionary<long, List<SocijalniNatjecajClanDto>>> GetForZahtjeviAsync(IEnumerable<long> zahtjevIds)
        {
            await using var context = await contextFactory.CreateDbContextAsync();
            var clanovi = await context.SocijalniNatjecajClanovi
                .Where(c => zahtjevIds.Contains(c.ZahtjevId))
                .ProjectToType<SocijalniNatjecajClanDto>()
                .ToListAsync();

            return clanovi
                .GroupBy(c => c.ZahtjevId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }
    }
}