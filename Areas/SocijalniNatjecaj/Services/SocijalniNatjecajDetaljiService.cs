using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services
{
    public class SocijalniNatjecajDetaljiService(ApplicationDbContext context) : ISocijalniNatjecajDetaljiService
    {
        public async Task<SocijalniNatjecajDto> GetDetaljiAsync(long id)
        {
            var entity = await context.SocijalniNatjecajZahtjevi
                .Include(x => x.Clanovi)
                .Include(x => x.BodovniPodaci)
                .Include(x => x.KucanstvoPodaci)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                throw new Exception("Zahtjev nije pronađen.");

            var podnositelj = entity.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);
            var bodovniDto = entity.BodovniPodaci != null
                ? new SocijalniBodovniDto()
                {
                    // mapiraj polja ako imaš
                }
                : new SocijalniBodovniDto();

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
                Bodovni = bodovniDto,
                KucanstvoPodaci = entity.KucanstvoPodaci is not null
                    ? new SocijalniKucanstvoPodaciDto
                    {
                        UkupniPrihodKucanstva = entity.KucanstvoPodaci.UkupniPrihodKucanstva,
                        PrebivanjeOd = entity.KucanstvoPodaci.PrebivanjeOd,
                        StambeniStatusKucanstva = entity.KucanstvoPodaci.StambeniStatusKucanstva,
                        SastavKucanstva = entity.KucanstvoPodaci.SastavKucanstva,
                        ZahtjevId = entity.Id
                    }
                    : null,

                Clanovi = entity.Clanovi.Select(clan => new SocijalniNatjecajClanDto
                {
                    Id = clan.Id,
                    ImePrezime = clan.ImePrezime,
                    Oib = clan.Oib,
                    Srodstvo = clan.Srodstvo,
                    DatumRodjenja = clan.DatumRodjenja
                }).ToList()
            };
        }

        public async Task AddClanAsync(SocijalniNatjecajClan noviClanDto)
        {
            var zahtjev = await context.SocijalniNatjecajZahtjevi
                .FirstOrDefaultAsync(z => z.Id == noviClanDto.ZahtjevId);

            if (zahtjev == null)
                throw new Exception($"Zahtjev s ID-om {noviClanDto.ZahtjevId} nije pronađen.");

            var noviClan = new SocijalniNatjecajClan
            {
                ImePrezime = noviClanDto.ImePrezime,
                Oib = noviClanDto.Oib,
                Srodstvo = noviClanDto.Srodstvo,
                DatumRodjenja = noviClanDto.DatumRodjenja,
                ZahtjevId = zahtjev.Id,
                Zahtjev = zahtjev
            };

            context.SocijalniNatjecajClanovi.Add(noviClan);
            await context.SaveChangesAsync();
        }


        public SocijalniNatjecajClan ConvertToEntity(SocijalniNatjecajClanDto clanDto, SocijalniNatjecajZahtjev zahtjev)
        {
            return new SocijalniNatjecajClan
            {
                ImePrezime = clanDto.ImePrezime,
                Oib = clanDto.Oib,
                Srodstvo = clanDto.Srodstvo,
                DatumRodjenja = clanDto.DatumRodjenja,
                ZahtjevId = zahtjev.Id,
                Zahtjev = zahtjev
            };
        }

        public async Task<SocijalniNatjecajZahtjev> GetZahtjevByIdAsync(long zahtjevId)
        {
            var zahtjev = await context.SocijalniNatjecajZahtjevi
                .Include(z => z.Clanovi)
                .Include(z => z.Natjecaj)
                .Include(z => z.BodovniPodaci)
                .Include(x => x.KucanstvoPodaci)
                .FirstOrDefaultAsync(z => z.Id == zahtjevId);

            if (zahtjev == null)
                throw new Exception($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

            return zahtjev;
        }
        
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
        
        public async Task UpdateKucanstvoPodaciAsync(long zahtjevId, SocijalniKucanstvoPodaciDto dto)
        {
            var zahtjev = await context.SocijalniNatjecajZahtjevi
                .Include(z => z.KucanstvoPodaci)
                .FirstOrDefaultAsync(z => z.Id == zahtjevId);

            if (zahtjev == null)
                throw new Exception($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

            if (zahtjev.KucanstvoPodaci == null)
            {
                zahtjev.KucanstvoPodaci = new SocijalniNatjecajKucanstvoPodaci
                {
                    ZahtjevId = zahtjevId
                };
                context.SocijalniNatjecajKucanstvoPodaci.Add(zahtjev.KucanstvoPodaci);
            }

            zahtjev.KucanstvoPodaci.UkupniPrihodKucanstva = dto.UkupniPrihodKucanstva!.Value;
            zahtjev.KucanstvoPodaci.PrebivanjeOd = dto.PrebivanjeOd!.Value;
            zahtjev.KucanstvoPodaci.StambeniStatusKucanstva = dto.StambeniStatusKucanstva!.Value;
            zahtjev.KucanstvoPodaci.SastavKucanstva = dto.SastavKucanstva!.Value;
            zahtjev.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();
        }
        public async Task UpdateOsnovniPodaciAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto)
        {
            var zahtjev = await context.SocijalniNatjecajZahtjevi
                .FirstOrDefaultAsync(z => z.Id == zahtjevId);

            if (zahtjev is null)
                throw new Exception($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

            zahtjev.KlasaPredmeta = dto.KlasaPredmeta ?? 0;
            zahtjev.Adresa = dto.Adresa;
            zahtjev.DatumPodnosenjaZahtjeva = dto.DatumPodnosenjaZahtjeva;
            zahtjev.NapomenaObrade = dto.NapomenaObrade;
            zahtjev.RezultatObrade = dto.RezultatObrade ?? RezultatObrade.Nepotpun;

            zahtjev.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();
        }



    }
}
