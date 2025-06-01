using System.Security.Claims;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniNatjecajService(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor)
    : ISocijalniNatjecajService
{
    private string CurrentUserId =>
        httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Unknown";

    public Task<List<SocijalniNatjecajZahtjevDto>> GetAllAsync() =>
        context.SocijalniNatjecajZahtjevi
               .Select(x => new SocijalniNatjecajZahtjevDto
               {
                   Id = x.Id,
                   KlasaPredmeta = x.KlasaPredmeta,
                   DatumPodnosenjaZahtjeva = x.DatumPodnosenjaZahtjeva,
                   Adresa = x.Adresa!,
                   NatjecajId = x.NatjecajId
               })
               .ToListAsync();

    public async Task<SocijalniNatjecajZahtjev> CreateAsync(
        SocijalniNatjecajZahtjevDto dto,
        string? imePrezime,
        string? oib)
    {
        var zahtjev = new SocijalniNatjecajZahtjev
        {
            NatjecajId = dto.NatjecajId,
            KlasaPredmeta = dto.KlasaPredmeta!.Value,
            DatumPodnosenjaZahtjeva = dto.DatumPodnosenjaZahtjeva ?? DateTime.UtcNow,
            Adresa = dto.Adresa,
            Email = dto.Email,
            RezultatObrade = dto.RezultatObrade!.Value,
            NapomenaObrade = dto.NapomenaObrade
        };

        zahtjev.Clanovi =
        [
            new SocijalniNatjecajClan
            {
                Zahtjev = zahtjev,
                ImePrezime = imePrezime,
                Oib = string.IsNullOrWhiteSpace(oib) ? null : oib,
                Srodstvo = Srodstvo.PodnositeljZahtjeva
            }
        ];

        zahtjev.KucanstvoPodaci = new SocijalniNatjecajKucanstvoPodaci { Zahtjev = zahtjev };
        zahtjev.BodovniPodaci = new SocijalniNatjecajBodovniPodaci   { Zahtjev = zahtjev };

        // audit
        AuditHelper.ApplyAudit(zahtjev, CurrentUserId, true);
        AuditHelper.ApplyAudit(zahtjev.Clanovi.Single(), CurrentUserId, true);
        AuditHelper.ApplyAudit(zahtjev.KucanstvoPodaci, CurrentUserId, true);
        AuditHelper.ApplyAudit(zahtjev.BodovniPodaci, CurrentUserId, true);

        await context.SocijalniNatjecajZahtjevi.AddAsync(zahtjev);

        // Vraćamo *tracked* entitet – SaveChanges će biti pozvan u UnitOfWork.
        return zahtjev;
    }

    public async Task<SocijalniNatjecajZahtjevDto> GetByIdAsync(long id)
    {
        var entity = await context.SocijalniNatjecajZahtjevi
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            throw new($"Zahtjev s ID-jem {id} nije pronađen.");

        return new SocijalniNatjecajZahtjevDto
        {
            Id = entity.Id,
            KlasaPredmeta = entity.KlasaPredmeta,
            DatumPodnosenjaZahtjeva = entity.DatumPodnosenjaZahtjeva,
            Adresa = entity.Adresa!,
            NatjecajId = entity.NatjecajId,
            Email = entity.Email,
            RezultatObrade = entity.RezultatObrade,
            NapomenaObrade = entity.NapomenaObrade
        };
    }
}