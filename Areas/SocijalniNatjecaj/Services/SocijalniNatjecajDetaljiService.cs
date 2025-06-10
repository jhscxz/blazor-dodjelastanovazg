using System.Security.Claims;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniNatjecajDetaljiService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    : ISocijalniNatjecajDetaljiService
{
    public async Task<SocijalniNatjecajZahtjevDto> GetDetaljiAsync(long id)
    {
        var entity = await context.SocijalniNatjecajZahtjevi
            .Include(x => x.Clanovi).ThenInclude(c => c.CreatedByUser)
            .Include(x => x.Clanovi).ThenInclude(c => c.UpdatedByUser)
            .Include(x => x.BodovniPodaci)
            .Include(x => x.KucanstvoPodaci).ThenInclude(k => k!.CreatedByUser)
            .Include(x => x.KucanstvoPodaci).ThenInclude(k => k!.UpdatedByUser)
            .Include(x => x.CreatedByUser)
            .Include(x => x.UpdatedByUser)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception("Zahtjev nije pronađen.");

        var podnositelj = entity.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);

        return new SocijalniNatjecajZahtjevDto
        {
            Id = entity.Id,
            NatjecajId = entity.NatjecajId,
            KlasaPredmeta = entity.KlasaPredmeta,
            DatumPodnosenjaZahtjeva = entity.DatumPodnosenjaZahtjeva,
            Adresa = entity.Adresa,
            Email = entity.Email,
            ImePrezime = podnositelj?.ImePrezime ?? string.Empty,
            Oib = podnositelj?.Oib,
            RezultatObrade = entity.RezultatObrade,
            NapomenaObrade = entity.NapomenaObrade,
            Bodovni = new SocijalniBodovniDto(),
            KucanstvoPodaci = entity.KucanstvoPodaci is not null
                ? new SocijalniKucanstvoPodaciDto
                {
                    UkupniPrihodKucanstva = entity.KucanstvoPodaci.UkupniPrihodKucanstva,
                    PrebivanjeOd = entity.KucanstvoPodaci.PrebivanjeOd,
                    StambeniStatusKucanstva = entity.KucanstvoPodaci.StambeniStatusKucanstva,
                    SastavKucanstva = entity.KucanstvoPodaci.SastavKucanstva,
                    ZahtjevId = entity.Id
                }.WithAuditFrom(entity.KucanstvoPodaci)
                : null,
            Clanovi = entity.Clanovi.Select(clan => new SocijalniNatjecajClanDto
            {
                Id = clan.Id,
                ZahtjevId = entity.Id,
                ImePrezime = clan.ImePrezime,
                Oib = clan.Oib,
                Srodstvo = clan.Srodstvo,
                DatumRodjenja = clan.DatumRodjenja
            }.WithAuditFrom(clan)).ToList()
        }.WithAuditFrom(entity);
    }

    public async Task<SocijalniNatjecajZahtjev> GetZahtjevByIdAsync(long zahtjevId)
    {
        return await context.SocijalniNatjecajZahtjevi
            .Include(z => z.Clanovi)
            .Include(z => z.Natjecaj)
            .Include(z => z.BodovniPodaci)
            .Include(z => z.KucanstvoPodaci)
            .FirstOrDefaultAsync(z => z.Id == zahtjevId)
            ?? throw new Exception($"Zahtjev s ID-om {zahtjevId} nije pronađen.");
    }

    public async Task UpdateOsnovniPodaciAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto)
    {
        var zahtjev = await context.SocijalniNatjecajZahtjevi
            .FirstOrDefaultAsync(z => z.Id == zahtjevId)
            ?? throw new Exception($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

        zahtjev.KlasaPredmeta = dto.KlasaPredmeta ?? 0;
        zahtjev.Adresa = dto.Adresa;
        zahtjev.Email = dto.Email;
        zahtjev.DatumPodnosenjaZahtjeva = dto.DatumPodnosenjaZahtjeva!.Value;
        zahtjev.NapomenaObrade = dto.NapomenaObrade;
        zahtjev.RezultatObrade = dto.RezultatObrade ?? RezultatObrade.Nepotpun;

        AuditHelper.ApplyAudit(zahtjev, GetCurrentUserId(), false);
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

    public string GetCurrentUserId()
    {
        return httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? throw new Exception("Korisnik nije prijavljen.");
    }
}
