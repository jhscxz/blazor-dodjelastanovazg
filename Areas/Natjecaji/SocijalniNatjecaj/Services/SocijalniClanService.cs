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
    public class SocijalniClanService(ApplicationDbContext context) : ISocijalniClanService
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        private IQueryable<SocijalniNatjecajZahtjev> BaseZahtjevQuery(bool asNoTracking = false)
        {
            var query = _context.SocijalniNatjecajZahtjevi.Include(z => z.Clanovi);

            return asNoTracking ? query.AsNoTracking() : query;
        }

        private async Task<SocijalniNatjecajZahtjev> GetZahtjevByIdAsync(long id, bool asNoTracking)
        {
            var zahtjev = await BaseZahtjevQuery(asNoTracking).FirstOrDefaultAsync(z => z.Id == id);

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
            await GetZahtjevByIdAsync(noviClan.ZahtjevId, true);

            await _context.SocijalniNatjecajClanovi.AddAsync(noviClan);
            await _context.SaveChangesAsync();

            return noviClan.ToDto();
        }

        public async Task<SocijalniNatjecajClanDto> EditClanAsync(SocijalniNatjecajClanDto azurirani)
        {
            var zahtjev = await GetZahtjevByIdAsync(azurirani.ZahtjevId, false);
            var clan = GetClanById(zahtjev, azurirani.Id);

            clan.ImePrezime = azurirani.ImePrezime;
            clan.Oib = azurirani.Oib;
            clan.Srodstvo = azurirani.Srodstvo;
            clan.DatumRodjenja = azurirani.DatumRodjenja;

            _context.SocijalniNatjecajClanovi.Update(clan);
            await _context.SaveChangesAsync();

            return clan.ToDto();
        }

        public async Task RemoveClanAsync(long zahtjevId, long clanId)
        {
            var zahtjev = await GetZahtjevByIdAsync(zahtjevId, false);
            var clan = GetClanById(zahtjev, clanId);

            _context.SocijalniNatjecajClanovi.Remove(clan);
            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<long, List<SocijalniNatjecajClanDto>>> GetForZahtjeviAsync(IEnumerable<long> zahtjevIds)
        {
            var clanovi = await _context.SocijalniNatjecajClanovi
                .Where(c => zahtjevIds.Contains(c.ZahtjevId))
                .ProjectToType<SocijalniNatjecajClanDto>()
                .ToListAsync();

            return clanovi
                .GroupBy(c => c.ZahtjevId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }
    }
}