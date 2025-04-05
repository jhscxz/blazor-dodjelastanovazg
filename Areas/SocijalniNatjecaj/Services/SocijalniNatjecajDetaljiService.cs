using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
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

        public async Task<SocijalniNatjecajDto> GetDetaljiAsync(long id)
        {
            var entity = await _context.SocijalniNatjecajZahtjevi
                .Include(x => x.Clanovi)
                .Include(x => x.BodovniPodaci)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                throw new Exception("Zahtjev nije pronađen.");

            var podnositelj = entity.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);

            return new SocijalniNatjecajDto
            {
                Id = entity.Id,
                NatjecajId = entity.NatjecajId,
                KlasaPredmeta = entity.KlasaPredmeta,
                DatumPodnosenjaZahtjeva = entity.DatumPodnosenjaZahtjeva,
                Adresa = entity.Adresa,
                ImePrezime = podnositelj?.ImePrezime ?? string.Empty,
                Oib = podnositelj?.Oib,
                RezultatObrade = entity.RezultatObrade,
                NapomenaObrade = entity.NapomenaObrade,
                Bodovni = entity.BodovniPodaci != null
                    ? new SocijalniBodovniDto
                    {
                        // mapiraj polja ako imaš
                    }
                    : new SocijalniBodovniDto(),
                Clanovi = entity.Clanovi.Select(clan => new SocijalniNatjecajClanDto
                {
                    Id = clan.Id,
                    ImePrezime = clan.ImePrezime,
                    Oib = clan.Oib,
                    Srodstvo = clan.Srodstvo
                }).ToList()
            };
        }

    }
}
