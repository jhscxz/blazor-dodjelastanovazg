using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services
{
    public class SocijalniNatjecajDetaljiService : ISocijalniNatjecajDetaljiService
    {
        private readonly ApplicationDbContext _context;

        public SocijalniNatjecajDetaljiService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SocijalniNatjecajDto> GetDetaljiAsync(long natjecajId)
        {
            var entity = await _context.SocijalniNatjecajZahtjevi
                .Include(x => x.Clanovi)
                //.Include(x => x.BodovniPodaci) // Ako imaš bodovne podatke, uključi ih ovdje
                .FirstOrDefaultAsync(x => x.NatjecajId == natjecajId);

            if (entity == null)
            {
                throw new Exception("Zahtjev nije pronađen.");
            }

            // Mapiranje entiteta u DTO; pretpostavljamo da je podnositelj prvi član u kolekciji.
            var dto = new SocijalniNatjecajDto
            {
                NatjecajId = entity.NatjecajId,
                KlasaPredmeta = entity.KlasaPredmeta,
                DatumPodnosenjaZahtjeva = entity.DatumPodnosenjaZahtjeva,
                Adresa = entity.Adresa,
                ImePrezime = entity.Clanovi.FirstOrDefault()?.ImePrezime ?? string.Empty,
                Oib = entity.Clanovi.FirstOrDefault()?.Oib,
                RezultatObrade = entity.RezultatObrade,
                NapomenaObrade = entity.NapomenaObrade,
                Bodovni = new SocijalniBodovniDto
                {
                    // Mapiraj bodovne podatke prema potrebi.
                },
                Clanovi = entity.Clanovi.Select(clan => new SocijalniNatjecajClanDto
                {
                    Id = clan.Id,
                    ImePrezime = clan.ImePrezime,
                    Oib = clan.Oib,
                    Srodstvo = clan.Srodstvo
                }).ToList()
            };

            return dto;
        }
    }
}
